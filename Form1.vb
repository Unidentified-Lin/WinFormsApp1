Imports Newtonsoft.Json
Imports ShopifySharp
Imports ShopifySharp.Lists
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Net.Http.Json
Imports System.Text
Imports System.Text.Json
Imports System.Text.Json.Serialization

Public Class Form1

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
        'Dim result = Await GetRequestWithHttpClient(Of CustomerList)(Url, mLogiKey)
        'TextBox1.Text = result.Item1
        'DataGridView1.AddObjDatas(result.Item2.Customers)

        Dim service = New CustomerService("https://ivanleathercraft.myshopify.com", "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")
        Dim result As ListResult(Of ShopifySharp.Customer) = Await service.ListAsync()
        DataGridView1.AddObjDatas(result.Items.ToList())

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

        MsgBox("OK !")
    End Sub

    Private Shared Async Function GetRequestWithHttpClient(Of T)(url As String, mLogiKey As Byte()) As Task(Of Tuple(Of String, T))
        Dim client = New HttpClient
        client.BaseAddress = New Uri(url)
        client.DefaultRequestHeaders.Accept.Clear()
        client.DefaultRequestHeaders.Accept.Add(New Headers.MediaTypeWithQualityHeaderValue("application/json"))
        client.DefaultRequestHeaders.Authorization = New Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(mLogiKey))
        Dim request = New HttpRequestMessage(HttpMethod.Get, url)
        Dim response As Task(Of HttpResponseMessage) = client.SendAsync(request)
        Dim content As HttpContent = response.Result.Content
        Dim jsonString As String = Await content.ReadAsStringAsync()
        Dim jsonData As T = Await content.ReadFromJsonAsync(Of T)
        Return Tuple.Create(jsonString, jsonData)
    End Function

End Class
