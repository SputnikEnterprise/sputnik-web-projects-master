'---------------------------------------------------------
' New propose.aspx.vb
'
' © by mf Sputnik Informatik GmbH
'---------------------------------------------------------

Imports System.Web.UI
Imports System.Data
Imports System.Net.Mail
Imports System.IO
Imports System.Data.SqlClient

Imports Microsoft.VisualBasic
Imports System
Imports System.Threading
Imports DevExpress.Web.DemoUtils
Imports System.Web.UI.WebControls
Imports DevExpress.Web
Imports System.Web.UI.Page

''' <summary>
''' Shows 'New propose' documents.
''' </summary>
Partial Class NewPropose
  Inherits CustomerBasePage


#Region "Properties"
  Private Const DOC_ART As String = "Vorschlag"
  Private m_PrintNecessary As Boolean = False


  'Feedback Handling
  Protected Property FeedbackResponseText As String = String.Empty

  ' DocScan
  'Dim _DocScanBytes As Byte()
  Protected Property DocScanBytes() As Byte()
  Protected Property FeedbackCount() As Integer
  '	Get
  '		Return _DocScanBytes
  '	End Get

  '	Set(ByVal value As Byte())
  '		_DocScanBytes = value
  '	End Set
  'End Property

  'ShowPdf 
  'Dim _PdfDocId As Integer = -1
  Public Property PdfDocId As Integer
  '	Get
  '		Return _PdfDocId
  '	End Get
  '	Private Set(ByVal value As Integer)
  '		_PdfDocId = value
  '	End Set
  'End Property

  Public MessageAcceptPropose As String
  Public MessageAcceptProposeTitle As String
  Public MessageSignPropose As String
  Public MessageSignProposeTitle As String
  Public MessageDeclinePropose As String
  Public MessageDeclineProposeTitle As String
  Public MessageFeedbackContract As String
  Public MessageFeedbackContractTitle As String


#End Region

#Region "Methods"

  ''' <summary>
  ''' Loads the page contents
  ''' </summary>
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'Javascript message translation
    MessageAcceptPropose = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_ACCEPT_PROPOSAL")
    MessageAcceptProposeTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_ACCEPT_PROPOSAL_TITLE")
    '
    MessageSignPropose = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_SIGN_CONTRACT")
    MessageSignProposeTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_SIGN_CONTRACT_TITLE")
    '
    MessageDeclinePropose = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_DECLINE_PROPOSAL")
    MessageDeclineProposeTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_DECLINE_PROPOSAL_TITLE")

    MessageFeedbackContract = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_DECLINE_PROPOSAL")
    MessageFeedbackContractTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_PROPOSAL_QUEST_DECLINE_PROPOSAL_TITLE")


    ' Make the client not cache the page, since it must be updated each time it is showed.
    Response.Cache.SetExpires(DateTime.Now)
    Response.Cache.SetCacheability(HttpCacheability.NoCache)
    m_PrintNecessary = False

    If Not LoadDocuments(grid, lblNumberOfDocsInfoAmount, pnlNumberOfDocs, DOC_ART, " AND (IsNull(GetResult, 0) = 0)") Then

      ' If no documents could be loaded, then show a message to the user
      Dim master As SiteMaster = CType(Me.Master, SiteMaster)
      master.ShowInfoMessage(True)
      pnlNumberOfDocs.Visible = False
    End If

    Dim previousPage As NewPropose = Page.PreviousPage
    If Not previousPage Is Nothing Then
      If previousPage.PdfDocId > -1 Then

        Dim dbId As Integer = previousPage.PdfDocId
        Dim documentStreamer = String.Format("{0}Documents.ashx?id={1}&{2}={3}&fn={4}", ResolveUrl("~"), dbId, ApplicationInfo.CurrentGuidName, ApplicationInfo.CurrentGuidValue, DOC_ART + ".pdf")
        Response.Redirect(documentStreamer)

      End If
    End If

  End Sub


  ''' <summary>
  ''' Sets the link to the PDF Streamer
  ''' </summary>
  Protected Sub GridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grid.RowDataBound

    If e.Row.RowType = DataControlRowType.DataRow Then

      Dim pdfIcon As ImageButton = CType(e.Row.FindControl("pdfIcon"), ImageButton)

      Dim index As Integer = Convert.ToInt32(e.Row.RowIndex)
      Dim dbId As Integer = grid.DataKeys.Item(index).Value

      Dim documentStreamer = String.Format("{0}Documents.ashx?id={1}&{2}={3}&fn={4}", ResolveUrl("~"), dbId, ApplicationInfo.CurrentGuidName, ApplicationInfo.CurrentGuidValue, DOC_ART + ".pdf")
      'Document open handling changed by Martin, 26.12.18
      pdfIcon.OnClientClick = String.Format("javascript:window.open('{0}', '_blank');return false;", documentStreamer)
      'pdfIcon.OnClientClick = String.Format("javascript:window.location='{0}';return false;", documentStreamer)

      'get fax and mail
      Dim userEMail As String = grid.DataKeys(index).Values("User_eMail").ToString()
      Dim userTelefax As String = grid.DataKeys(index).Values("User_Telefax").ToString()

      ' Register javascript confirm dialog for accept button
      Dim acceptButton As LinkButton = CType(e.Row.FindControl("lnkAccept"), LinkButton)
      acceptButton.CommandArgument = index
      Dim postBackEventAccept As String = ClientScript.GetPostBackEventReference(acceptButton, String.Empty)
      Dim postBackAcceptParameter As String = postBackEventAccept.Replace("__doPostBack('", "").Replace("','')", "")
      Dim showPrintDialog As String
      If (m_PrintNecessary) Then
        showPrintDialog = "true"
      Else
        showPrintDialog = "false"
      End If
      acceptButton.OnClientClick = "javascript: ConfirmAcceptContract('" & postBackAcceptParameter & "'," & showPrintDialog & ",'" & userEMail & "','" & userTelefax & "'); return false;"


      ' Register javascript confirm dialog for decline button
      Dim declineButton As LinkButton = CType(e.Row.FindControl("lnkDecline"), LinkButton)
      declineButton.CommandArgument = index
      Dim postBackEventDecline As String = ClientScript.GetPostBackEventReference(declineButton, String.Empty)
      Dim postBackDeclineParameter As String = postBackEventDecline.Replace("__doPostBack('", "").Replace("','')", "")
      declineButton.OnClientClick = "javascript: ConfirmDeclineContract('" & postBackDeclineParameter & "'); return false;"


      'Handling Feedback Button
      Dim feedbackButton As LinkButton = CType(e.Row.FindControl("lnkFeeback"), LinkButton)
      feedbackButton.CommandArgument = index

      ExcecuteSQLQuery(String.Format("SELECT COUNT(*) AS CountFeedback FROM Dbo.[tbl_Customer_WOSDocument_State] WHERE (ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where ID = {0}) AND ([Customer_Feedback] IS NOT NULL OR [Customer_Feedback] <> ''))", dbId), AddressOf ProcessFeedbackCount)

      If FeedbackCount > 0 Then
        feedbackButton.Visible = False
      End If


    End If


  End Sub


  Protected Sub GridView_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)

    Dim index As Integer = Convert.ToInt32(e.CommandArgument)
    Dim dbId As Integer = grid.DataKeys.Item(index).Value
    Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

    If e.CommandName = "AcceptContract" Then

      Try
        ' Mark the contract as accepted
        If (ExcecuteSQLNonQuery(String.Format("UPDATE Kunden_Doc_Online SET GetResult = 1, Get_On = GetDate() WHERE ID = {0} AND GetResult Is NULL", dbId)) = 1) Then
					ExcecuteSQLNonQuery(String.Format("UPDATE Dbo.[tbl_Customer_WOSDocument_State] SET GetResult = 1, Get_On = GetDate(), [ViewedResult] = 1, [Viewed_On] = GetDate(), [NotifyAdvisor] = 'true' WHERE ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where ID = {0}) AND GetResult Is NULL", dbId))

				End If

      Catch ex As Exception

        Response.Write(String.Format("Fehler: (e.CommandName = AcceptContract) {0}", ex.Message))

      Finally

        Response.Redirect("~/Kunden/NewPropose.aspx?" & appInfo.ZHDQueryParameterName & "=" & appInfo.CurrentGuidValue)

      End Try

    ElseIf e.CommandName = "DeclineContract" Then
      Try

        If ExcecuteSQLNonQuery(String.Format("UPDATE Kunden_Doc_Online SET GetResult = 2, Get_On = GetDate() WHERE ID = {0} AND GetResult Is NULL", dbId)) = 1 Then
					ExcecuteSQLNonQuery(String.Format("UPDATE Dbo.[tbl_Customer_WOSDocument_State] SET GetResult = 2, Get_On = GetDate(), [ViewedResult] = 1, [Viewed_On] = GetDate(), [NotifyAdvisor] = 'true' WHERE ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where ID = {0}) AND GetResult Is NULL", dbId))

				End If

      Catch ex As Exception

				Response.Write(String.Format("Fehler: (e.CommandName = DeclineContract) {0} ccReceiver: {1} emailReceiver: {2}", ex.Message, String.Empty, String.Empty))

			Finally

        Response.Redirect("~/Kunden/NewPropose.aspx?" & appInfo.ZHDQueryParameterName & "=" & appInfo.CurrentGuidValue)

      End Try

    ElseIf e.CommandName = "ShowFeedback" Then

      Session("FeedbackID") = dbId
      Response.Redirect("~/Kunden/NewPropose_Feedback.aspx?" & appInfo.ZHDQueryParameterName & "=" & appInfo.CurrentGuidValue)

    End If

  End Sub


  Private Function FillAcceptContractEmailTempateForKD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

    Try
      Dim esNr As String = Utility.GetColumnTextStr(dataRow, "ESNr", String.Empty)
      Dim adviser As String = Utility.GetColumnTextStr(dataRow, "KD_Berater", String.Empty)
      Dim customerName As String = Session("CustomerName")
      Dim customerStreet As String = Session("CustomerStreet")
      Dim customerPlace As String = Session("CustomerPlace")
      Dim userTelephone As String = Utility.GetColumnTextStr(dataRow, "User_Telefon", String.Empty)
      Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
      Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
      Dim customerHompage As String = Session("CustomerHomepage")

      emailTemplateText = emailTemplateText.Replace("{ESNr}", esNr)
      emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
      emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
      emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
      emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
      emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
      emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
      emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
      emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)

    Catch ex As Exception
      emailTemplateText = String.Format("Fehler (FillAcceptContractEmailTempateForKD): {0}", ex.Message)

    End Try

    Return emailTemplateText
  End Function


  Private Function FillDeclineContractEmailTempateForKD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

    Try
      Dim esNr As String = Utility.GetColumnTextStr(dataRow, "ESNr", String.Empty)
      Dim adviser As String = Utility.GetColumnTextStr(dataRow, "KD_Berater", String.Empty)
      Dim customerName As String = Session("CustomerName")
      Dim customerStreet As String = Session("CustomerStreet")
      Dim customerPlace As String = Session("CustomerPlace")
      Dim userTelephone As String = Utility.GetColumnTextStr(dataRow, "User_Telefon", String.Empty)
      Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
      Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
      Dim customerHompage As String = Session("CustomerHomepage")

      emailTemplateText = emailTemplateText.Replace("{ESNr}", esNr)
      emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
      emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
      emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
      emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
      emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
      emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
      emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
      emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)

    Catch ex As Exception
      emailTemplateText = String.Format("Fehler (FillDeclineContractEmailTempateForKD): {0}", ex.Message)

    End Try

    Return emailTemplateText

  End Function


  Private Function FillAcceptContractEmailTempateForZHD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

    Try
      Dim esNr As String = Utility.GetColumnTextStr(dataRow, "ESNr", String.Empty)
      Dim salutation As String = Utility.GetColumnTextStr(dataRow, "ZHD_BriefAnrede", String.Empty)
      Dim zhdName As String = Utility.GetColumnTextStr(dataRow, "ZHD_Nachname", String.Empty)
      Dim adviser As String = Utility.GetColumnTextStr(dataRow, "ZHD_Berater", String.Empty)
      Dim customerName As String = Session("CustomerName")
      Dim customerStreet As String = Session("CustomerStreet")
      Dim customerPlace As String = Session("CustomerPlace")
      Dim userTelephone As String = Utility.GetColumnTextStr(dataRow, "User_Telefon", String.Empty)
      Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
      Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
      Dim customerHompage As String = Session("CustomerHomepage")

      emailTemplateText = emailTemplateText.Replace("{ESNr}", esNr)
      emailTemplateText = emailTemplateText.Replace("{Anrede}", salutation)
      emailTemplateText = emailTemplateText.Replace("{Nachname}", zhdName)
      emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
      emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
      emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
      emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
      emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
      emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
      emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
      emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)

    Catch ex As Exception
      emailTemplateText = String.Format("Fehler (FillAcceptContractEmailTempateForZHD): {0}", ex.Message)
    End Try

    Return emailTemplateText

  End Function


  Private Function FillDeclineContractEmailTempateForZHD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

    Try
      Dim salutation As String = Utility.GetColumnTextStr(dataRow, "ZHD_BriefAnrede", String.Empty)
      Dim zhdName As String = Utility.GetColumnTextStr(dataRow, "ZHD_Nachname", String.Empty)
      Dim esNr As String = Utility.GetColumnTextStr(dataRow, "ESNr", String.Empty)
      Dim adviser As String = Utility.GetColumnTextStr(dataRow, "ZHD_Berater", String.Empty)
      Dim customerName As String = Session("CustomerName")
      Dim customerStreet As String = Session("CustomerStreet")
      Dim customerPlace As String = Session("CustomerPlace")
      Dim userTelephone As String = dataRow("User_Telefon")
      Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
      Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
      Dim customerHompage As String = Session("CustomerHomepage")

      emailTemplateText = emailTemplateText.Replace("{Anrede}", salutation)
      emailTemplateText = emailTemplateText.Replace("{Nachname}", zhdName)
      emailTemplateText = emailTemplateText.Replace("{ESNr}", esNr)
      emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
      emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
      emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
      emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
      emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
      emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
      emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
      emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)

    Catch ex As Exception
      emailTemplateText = String.Format("Fehler (FillDeclineContractEmailTempateForZHD): {0}", ex.Message)

    End Try

    Return emailTemplateText

  End Function

  ''' <summary>
  ''' Needed for paging support
  ''' </summary>
  Protected Sub GridView_PageIndexChanging(ByVal sender As Object,
                                           ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grid.PageIndexChanging
    grid.PageIndex = e.NewPageIndex
    grid.DataBind()
  End Sub

  ''' <summary>
  ''' Receives the document bytes which are read from the database.
  ''' </summary>
  Private Sub ProcessContractBytes(ByVal dr As SqlDataReader)

    If Not dr.IsDBNull(dr.GetOrdinal("DocScan")) Then
      DocScanBytes = dr("DocScan")
    End If

  End Sub


  Private Sub ProcessFeedbackCount(ByVal dr As SqlDataReader)

    If Not dr.IsDBNull(dr.GetOrdinal("CountFeedback")) Then
      FeedbackCount = dr("CountFeedback")
    End If

  End Sub


#End Region

End Class