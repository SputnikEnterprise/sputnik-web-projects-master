Imports Microsoft.VisualBasic

Public Class NavLinkInfo

    Public Sub New(ByVal text As String, ByVal url As String)
        _Text = text
        _Url = url
    End Sub


    ' Link text
    Dim _Text As String
    Public ReadOnly Property Text() As String
        Get
            Return _Text
        End Get
    End Property

    ' Url
    Dim _Url As String
    Public ReadOnly Property Url() As String
        Get
            Return _Url
        End Get
    End Property

End Class
