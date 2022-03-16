Imports ClassLibrary1.Utils
Imports WinFormsLibrary1.Extensions

Public Class Form1
    Private _baseUrl As String = "https://ivanleathercraft.myshopify.com/admin/api/2020-10/"
    Private _username As String = "ad2ce1bbb53cda0c801fc989e3ff304f"
    Private _password As String = "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"

    Private _currentResponseModel As ResponseModel(Of CustomerList)

    Public ReadOnly Property GetKey() As String
        Get
            Return APIUtil.GetAuthorizationString(_username, _password)
        End Get
    End Property

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        'Using HttpClient
        Dim client = APIUtil.GetHttpClient(_baseUrl, GetKey())

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

        Dim client = APIUtil.GetHttpClient(_baseUrl, GetKey())
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
