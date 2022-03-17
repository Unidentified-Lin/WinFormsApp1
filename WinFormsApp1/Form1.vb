Imports System.IO
Imports ClassLibrary1.Utils
Imports WinFormsLibrary1.Extensions

Public Class Form1
    Private ReadOnly _retailEnvFile As String = "retail_env.yml"
    Private _retailEnv As IDictionary(Of String, String)

    Private _currentResponseModel As ResponseModel(Of CustomerList)

    Public ReadOnly Property GetEnv(key As String) As String
        Get
            If _retailEnv IsNot Nothing Then
                Return _retailEnv(key)
            End If

            Dim exist = File.Exists(_retailEnvFile)
            If Not exist Then
                Throw New Exception($"File {_retailEnvFile} not exist")
            End If
            _retailEnv = DotEnvFile.LoadFile(_retailEnvFile, True)

            Return _retailEnv(key)
        End Get
    End Property
    Public ReadOnly Property GetKey() As String
        Get
            Return APIUtil.GetAuthorizationString(GetEnv("api_key"), GetEnv("api_pass"))
        End Get
    End Property

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Using HttpClient
        Dim client = APIUtil.GetHttpClient($"{GetEnv("shop_api")}/admin/api/2022-01/", GetKey())

        _currentResponseModel = Await APIUtil.GetResponseAsync(Of CustomerList)(client, $"customers.json?limit={Limit.Text}")

        TextBox1.Text = _currentResponseModel.JsonString
        DataGridView1.AddObjDatas(_currentResponseModel.JsonData.Customers)
        StatusCheck()

    End Sub

    Private Async Sub Page_ClickAsync(sender As Object, e As EventArgs) Handles NextPage.Click, PreviousPage.Click
        Dim direction As Boolean = IIf(CType(sender, Button).Name.Equals("NextPage"), _currentResponseModel.HasNext, _currentResponseModel.HasPrevious)
        Dim directionUrl As String = IIf(CType(sender, Button).Name.Equals("NextPage"), _currentResponseModel.NextUrl, _currentResponseModel.PreviousUrl)

        If Not direction Then
            Return
        End If

        Dim client = APIUtil.GetHttpClient(GetEnv("shop_api"), GetKey())
        _currentResponseModel = Await APIUtil.GetResponseAsync(Of CustomerList)(client, directionUrl)
        TextBox1.Text = _currentResponseModel.JsonString
        DataGridView1.AddObjDatas(_currentResponseModel.JsonData.Customers)
        StatusCheck()
    End Sub

    Private Sub StatusCheck()
        NextPage.Visible = _currentResponseModel.HasNext
        PreviousPage.Visible = _currentResponseModel.HasPrevious
    End Sub

End Class
