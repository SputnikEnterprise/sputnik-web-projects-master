'---------------------------------------------------------
' NotFound.vb
'
' © by mf Sputnik Informatik GmbH
'---------------------------------------------------------


' This page is shown in error cases
Partial Public Class NotFound
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim master As SiteMaster = CType(Me.Master, SiteMaster)


        Dim errorQueryParameter As String = Request.QueryString(Utility.ERROR_TYPE_PARAMETER)

        If errorQueryParameter = Utility.ERROR_TYPE_DOCUMENT Then
            txtError.TextKey = "TEXT_DOCUMENT_NOT_FOUND_ERROR"
            txtErrorMessage.TextKey = "TEXT_DOCUMENT_NOT_FOUND_MESSAGE"
        Else
            master.HideLogoAndCustomerInfo()
        End If

    End Sub

    Protected Overrides Sub OnPreRenderComplete(ByVal e As EventArgs)
        MyBase.OnPreRender(e)

        Dim errorMessageParameter As String = Request.QueryString(Utility.ERROR_MESSAGE_PARAMETER)
        txtErrorMessage.Text = txtErrorMessage.Text & errorMessageParameter
    End Sub
End Class
