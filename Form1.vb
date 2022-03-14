Imports Newtonsoft
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Linq
Imports RestSharp
Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text

Public Class Form1
    Public Class Customer
        Public Property Id As String
        Public Property Email As String
        Public Property First_name As String
        Public Property Last_name As String
        Public Property Phone As String
        Public Property Note As String
        Public Property Tags As String
        Public Property State As String
        Public Property Currency As String
        Public Property Tax_exempt As String
        Public Property Verified_email As String
        Public Property Accepts_marketing As String
        Public Property Accepts_marketing_updated_at As String
        Public Property Orders_count As String
        Public Property Total_spent As String
        Public Property Last_order_id As String
        Public Property Last_order_name As String
        Public Property Created_at As String
        Public Property Updated_at As String
        Public Property Addresses As String

    End Class


    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Dim GetCustomers As String = "/admin/api/2020-10/customers.json"
        'Dim ShopName As String = "Retail"
        Dim Username As String = "ad2ce1bbb53cda0c801fc989e3ff304f"
        Dim Password As String = "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"
        Dim Shop As String = "ivanleathercraft"


        Dim HasNext As Boolean = True
        Dim Head As String = "https://" & Username & ":" & Password & "@" & Shop & ".myshopify.com"
        Dim Resource As String = GetCustomers
        Dim Query As String = "?limit=250"
        Dim Url As String = Head + Resource + Query
        Dim mLogiKey As Byte() = Encoding.Default.GetBytes(Username + ":" + Password)
        Dim LoginKey As String = "Basic " + Convert.ToBase64String(mLogiKey)

        Dim myWebRequest As WebRequest = WebRequest.Create(Url)
        myWebRequest.ContentType = "application/JSON"
        myWebRequest.Method = "GET"
        myWebRequest.Headers.Add("Authorization", LoginKey)
        Dim myWebResponse As WebResponse = myWebRequest.GetResponse()
        Dim StreamRead As StreamReader = New StreamReader(myWebResponse.GetResponseStream())
        Dim ProductsJSON As String = StreamRead.ReadToEnd()
        Me.TextBox1.Text = ProductsJSON



        MsgBox("OK !")
    End Sub
End Class
