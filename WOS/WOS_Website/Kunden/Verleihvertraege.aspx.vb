'---------------------------------------------------------
' Verleihvertraege.aspx.vb
'
' © by mf Sputnik Informatik GmbH
'---------------------------------------------------------

Imports System.Web.UI
Imports System.Data
Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient

''' <summary>
''' Shows 'Verleihvertraege' documents.
''' </summary>
Partial Class Verleihvertraege
    Inherits CustomerBasePage

#Region "Properties"
    Private Const DOC_ART As String = "Verleihvertrag"
#End Region

#Region "Methods"

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    ' geändert durch Sputnik 5.5.11
    If Not LoadDocuments(grid, lblNumberOfDocsInfoAmount, pnlNumberOfDocs, DOC_ART, " AND GetResult >= 1") Then

      ' If no documents could be loaded, then show a message to the user
      Dim master As SiteMaster = CType(Me.Master, SiteMaster)
            master.ShowInfoMessage(True)
      pnlNumberOfDocs.Visible = False
    End If

    End Sub

    ''' <summary>
    ''' Sets the link to the PDF Streamer
    ''' </summary>
    Protected Sub GridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grid.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            ' PDF Icon
            Dim pdfIcon As ImageButton = CType(e.Row.FindControl("pdfIcon"), ImageButton)

            Dim index As Integer = Convert.ToInt32(e.Row.RowIndex)
            Dim dbId As Integer = grid.DataKeys.Item(index).Value

            Dim documentStreamer = String.Format("{0}Documents.ashx?id={1}&{2}={3}&fn={4}", ResolveUrl("~"), dbId, ApplicationInfo.CurrentGuidName, ApplicationInfo.CurrentGuidValue, DOC_ART + ".pdf")

      'Document open handling changed by Martin, 26.12.18
      pdfIcon.OnClientClick = String.Format("javascript:window.open('{0}', '_blank');return false;", documentStreamer)
      'pdfIcon.OnClientClick = String.Format("javascript:window.location='{0}';return false;", documentStreamer)

      '--------- format accept data----------

      ' Read the underlying DataSet of the grid
      Dim docDataSet As DataSet = CType(grid.DataSource, DataSet)

      ' Find the correct DataRow
      Dim row As DataRow = docDataSet.Tables(0).Rows().Find(dbId)

      Dim acceptDate As DateTime = row("Get_On")

      ' Accept Date
      Dim lblAcceptDate As Label = CType(e.Row.FindControl("lblAcceptedDate"), Label)
            lblAcceptDate.Text = acceptDate.ToString("dd.MM.yyyy HH:mm")

            '--------- forward button----------

            ' Register javascript for document forwarding
            Dim forwardButton As LinkButton = CType(e.Row.FindControl("lnkFoward"), LinkButton)
            forwardButton.OnClientClick = CreateForwardDocumentJavaScript(dbId, DOC_ART, forwardButton.ClientID)

        End If

    End Sub

    ''' <summary>
    ''' Needed for paging support
    ''' </summary>
    Protected Sub GridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grid.PageIndexChanging
        grid.PageIndex = e.NewPageIndex
        grid.DataBind()
    End Sub

#End Region

End Class