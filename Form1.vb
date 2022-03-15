Imports Newtonsoft.Json
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Json
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Class Form1
    Private _baseUrl As String = "https://ivanleathercraft.myshopify.com/admin/api/2020-10/"
    Private _username As String = "ad2ce1bbb53cda0c801fc989e3ff304f"
    Private _password As String = "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"
    Private _currentResponseModel As ResponseModel(Of CustomerList)
    Public Property CurrentResponseModel() As ResponseModel(Of CustomerList)
        Get
            Return _currentResponseModel
        End Get
        Set(ByVal value As ResponseModel(Of CustomerList))
            _currentResponseModel = value
        End Set
    End Property
    Public ReadOnly Property GetKey() As String
        Get
            Return Convert.ToBase64String(Encoding.Default.GetBytes(_username + ":" + _password))
        End Get
    End Property
    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim GetCustomers As String = "/admin/api/2020-10/customers.json"
        'Dim ShopName As String = "Retail"
        Dim Username As String = "ad2ce1bbb53cda0c801fc989e3ff304f"
        Dim Password As String = "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"
        Dim Shop As String = "ivanleathercraft"


        Dim HasNext As Boolean = True
        Dim Head As String = "https://" & Username & ":" & Password & "@" & Shop & ".myshopify.com"
        Dim Resource As String = GetCustomers
        Dim Query As String = "?limit=20"
        Dim Url As String = Head + Resource + Query
        Dim mLogiKey As Byte() = Encoding.Default.GetBytes(Username + ":" + Password)
        Dim LoginKey As String = "Basic " + Convert.ToBase64String(mLogiKey)

        'Using HttpClient (replace Async Function with Sub)
        Dim client = GetHttpClient(_baseUrl, GetKey())

        CurrentResponseModel = Await GetResponseAsync(Of CustomerList)(client, "customers.json?limit=10")

        TextBox1.Text = CurrentResponseModel.JsonString
        DataGridView1.AddObjDatas(CurrentResponseModel.JsonData.Customers)
        StatusCheck()

        'Using WebRequest
        'Dim myWebRequest As WebRequest = WebRequest.Create(Url)
        'myWebRequest.ContentType = "application/JSON"
        'myWebRequest.Method = "GET"
        'myWebRequest.Headers.Add("Authorization", LoginKey)
        'Dim myWebResponse As WebResponse = myWebRequest.GetResponse()
        'Dim StreamRead As StreamReader = New StreamReader(myWebResponse.GetResponseStream())
        'Dim ProductsJSON As String = StreamRead.ReadToEnd()
        'Me.TextBox1.Text = ProductsJSON
        'Dim obj = JsonConvert.DeserializeObject(Of CustomerList)(ProductsJSON)
        'DataGridView1.AddObjDatas(obj.Customers)

        'MsgBox("OK !")
    End Sub
    Private Shared Function GetHttpClient(baseUrl As String, key As String) As HttpClient
        Dim client = New HttpClient
        client.BaseAddress = New Uri(baseUrl)
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(New Headers.MediaTypeWithQualityHeaderValue("application/json"))
        client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Basic", key)
        Return client
    End Function


    Private Shared Async Function GetResponseAsync(Of T)(client As HttpClient, url As String) As Task(Of ResponseModel(Of T))
        Dim request = New HttpRequestMessage(HttpMethod.Get, url)
        Dim response As HttpResponseMessage = client.SendAsync(request).Result

        If (Not response.IsSuccessStatusCode) Then
            Return Nothing
        End If

        Dim rtnModel As New ResponseModel(Of T)
        Dim hasNext As Boolean = False
        Dim hasPrevious As Boolean = False
        Dim nextUrl As String = ""
        Dim previousUrl As String = ""

        If response.Headers.Contains("Link") Then
            Dim linkContents = response.Headers.GetValues("Link").First().Split(",")
            hasNext = linkContents.Any(Function(c) c.Contains("rel=""next"""))
            hasPrevious = linkContents.Any(Function(c) c.Contains("rel=""previous"""))
            nextUrl = GetUrlFromLinkContents(linkContents, PaginationDirection.NextUrl)
            previousUrl = GetUrlFromLinkContents(linkContents, PaginationDirection.PreviousUrl)
        End If

        Dim content As HttpContent = response.Content
        Dim jsonString As String = Await content.ReadAsStringAsync()
        Dim jsonData As T = Await content.ReadFromJsonAsync(Of T)

        rtnModel.HasNext = hasNext
        rtnModel.HasPrevious = hasPrevious
        rtnModel.NextUrl = nextUrl
        rtnModel.PreviousUrl = previousUrl
        rtnModel.JsonString = jsonString
        rtnModel.JsonData = jsonData

        Return rtnModel
    End Function
    Private Enum PaginationDirection
        PreviousUrl
        NextUrl
    End Enum

    Private Shared Function GetUrlFromLinkContents(linkContents As String(), dir As PaginationDirection) As String
        Dim relName As String
        Select Case dir
            Case PaginationDirection.PreviousUrl
                relName = "previous"
            Case Else
                relName = "next"
        End Select
        If Not linkContents.Any(Function(c) c.Contains($"rel=""{relName}""")) Then
            Return ""
        End If
        Dim urlSection = linkContents.First(Function(c) c.Contains($"rel=""{relName}""")).Split(";")(0).Trim()
        Return urlSection.Substring(urlSection.IndexOf("<") + 1, urlSection.IndexOf(">") - 1)
    End Function

    Private Shared Async Function GetAllResponseAsync(Of T)(client As HttpClient, url As String) As Task(Of List(Of T))
        Dim hasNext As Boolean = False
        Dim list As New List(Of T)
        Do While hasNext
            Dim responseModel = Await GetResponseAsync(Of T)(client, url)
            list.AddRange(responseModel.JsonData)
            url = responseModel.NextUrl
            hasNext = responseModel.HasNext
        Loop
        Return list
    End Function

    Private Async Sub Page_ClickAsync(sender As Object, e As EventArgs) Handles NextPage.Click, PreviousPage.Click
        Dim direction = IIf(CType(sender, Button).Name.Equals("NextPage"), CurrentResponseModel.HasNext, CurrentResponseModel.HasPrevious)
        Dim directionUrl = IIf(CType(sender, Button).Name.Equals("NextPage"), CurrentResponseModel.NextUrl, CurrentResponseModel.PreviousUrl)

        If Not direction Then
            Return
        End If

        Dim client = GetHttpClient(_baseUrl, GetKey())

        CurrentResponseModel = Await GetResponseAsync(Of CustomerList)(client, directionUrl)
        TextBox1.Text = CurrentResponseModel.JsonString
        DataGridView1.AddObjDatas(CurrentResponseModel.JsonData.Customers)
        StatusCheck()
    End Sub

    Private Sub StatusCheck()
        NextPage.Visible = CurrentResponseModel.HasNext
        PreviousPage.Visible = CurrentResponseModel.HasPrevious
    End Sub

End Class
