Public Class ResponseModel(Of T)
    Public Property NextUrl As String
    Public Property PreviousUrl As String
    Public Property HasNext As Boolean
    Public Property HasPrevious As Boolean
    Public Property JsonString As String
    Public Property JsonData As T
End Class
