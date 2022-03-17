Imports ShopifySharp
Imports ShopifySharp.Lists
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Json
Imports System.Text

Public Class Form1
    Private ReadOnly _retailEnvFile As String = "retail_env.yml"
    Private _retailEnv As IDictionary(Of String, String)

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim exsist = File.Exists(_retailEnvFile)
        _retailEnv = DotEnvFile.LoadFile(_retailEnvFile, True)

        'Using HttpClient (replace Async Function with Sub)
        'Dim result = Await GetRequestWithHttpClient(Of CustomerList)(Url, mLogiKey)
        'TextBox1.Text = result.Item1
        'DataGridView1.AddObjDatas(result.Item2.Customers)

        Dim service = New CustomerService(_retailEnv("shop_api"), _retailEnv("access_token"))
        Dim result As ListResult(Of ShopifySharp.Customer) = Await service.ListAsync()
        DataGridView1.AddObjDatas(result.Items.ToList())

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
