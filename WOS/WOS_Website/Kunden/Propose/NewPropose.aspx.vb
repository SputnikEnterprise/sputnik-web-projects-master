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

''' <summary>
''' Shows 'New propose' documents.
''' </summary>
Partial Class NewPropose
	Inherits CustomerBasePage

#Region "Properties"
	Private Const DOC_ART As String = "Verleihvertrag"
	Private isPrintNecessary As Boolean = False

	' DocScan
	Dim _DocScanBytes As Byte()
	Protected Property DocScanBytes() As Byte()
		Get
			Return _DocScanBytes
		End Get

		Set(ByVal value As Byte())
			_DocScanBytes = value
		End Set
	End Property

	'ShowPdf 
	Dim _PdfDocId As Integer = -1
	Public Property PdfDocId As Integer
		Get
			Return _PdfDocId
		End Get
		Private Set(ByVal value As Integer)
			_PdfDocId = value
		End Set
	End Property

	Public MessageAcceptContract As String
	Public MessageAcceptContractTitle As String
	Public MessageSignContract As String
	Public MessageSignContractTitle As String
	Public MessageDeclineContract As String
	Public MessageDeclineContractTitle As String


#End Region

#Region "Methods"

	''' <summary>
	''' Loads the page contents
	''' </summary>
	Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
		'Javascript message translation
		MessageAcceptContract = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_ACCEPT_CONTRACT")
		MessageAcceptContractTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_ACCEPT_CONTRACT_TITLE")
		'
		MessageSignContract = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_SIGN_CONTRACT")
		MessageSignContractTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_SIGN_CONTRACT_TITLE")
		'
		MessageDeclineContract = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_DECLINE_CONTRACT")
		MessageDeclineContractTitle = Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_OPEN_CONTRACT_QUEST_DECLINE_CONTRACT_TITLE")



		' Make the client not cache the page, since it must be updated each time it is showed.
		Response.Cache.SetExpires(DateTime.Now)
		Response.Cache.SetCacheability(HttpCacheability.NoCache)
		isPrintNecessary = GetPrintNecessary()

		If Not LoadDocuments(grid, lblNumberOfDocsInfoAmount, pnlNumberOfDocs, DOC_ART, " AND (GetResult IS NULL OR GetResult = 0)") Then

			' If no documents could be loaded, then show a message to the user
			Dim master As SiteMaster = CType(Me.Master, SiteMaster)
			master.ShowInfoMessage(True)
			pnlNumberOfDocs.Visible = False
		End If

		Dim previousPage As NewPropose = Page.PreviousPage
		If Not previousPage Is Nothing Then
			If previousPage.PdfDocId > -1 Then

				Dim dbId As Integer = previousPage.PdfDocId
				Dim documentStreamer = String.Format("{0}Documents.ashx?id={1}&{2}={3}&fn={4}", _
																				 ResolveUrl("~"), dbId, ApplicationInfo.CurrentGuidName, _
																				 ApplicationInfo.CurrentGuidValue, DOC_ART + ".pdf")
				Response.Redirect(documentStreamer)
				'Dim location As String = String.Format("window.location=""{0}"";", documentStreamer)
				'Dim redirect As String = "function openPdf(){ var x = $('#Hidden1').val(); if(parseInt(x) == 0) {$('#Hidden1').val('1'); window.setTimeout('" & location & "', 1000);}} openPdf(); "
				'Page.ClientScript.RegisterStartupScript(Me.GetType(), "showPdf", redirect, True)

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

			Dim documentStreamer = String.Format("{0}Documents.ashx?id={1}&{2}={3}&fn={4}", _
																					 ResolveUrl("~"), dbId, ApplicationInfo.CurrentGuidName, _
																					 ApplicationInfo.CurrentGuidValue, DOC_ART + ".pdf")
			pdfIcon.OnClientClick = String.Format("javascript:window.location='{0}';return false;", documentStreamer)

			'get fax and mail
			Dim userEMail As String = grid.DataKeys(index).Values("User_eMail").ToString()
			Dim userTelefax As String = grid.DataKeys(index).Values("User_Telefax").ToString()

			' Register javascript confirm dialog for accept button
			Dim acceptButton As LinkButton = CType(e.Row.FindControl("lnkAccept"), LinkButton)
			acceptButton.CommandArgument = index
			Dim postBackEventAccept As String = ClientScript.GetPostBackEventReference(acceptButton, String.Empty)
			Dim postBackAcceptParameter As String = postBackEventAccept.Replace("__doPostBack('", "").Replace("','')", "")
			Dim showPrintDialog As String
			If (isPrintNecessary) Then
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

		End If

	End Sub

	''' <summary>
	''' Sends a PDF document to the customer, when the customer has clicked onto a PDF logo
	''' </summary>
	Protected Sub GridView_RowCommand(ByVal source As Object, ByVal e As GridViewCommandEventArgs)

		Dim index As Integer = Convert.ToInt32(e.CommandArgument)
		Dim dbId As Integer = grid.DataKeys.Item(index).Value
		Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		Dim ccReceiver As String = String.Empty
		Dim emailReceiver As String = String.Empty

		If e.CommandName = "AcceptContract" Then

			Try
				' Mark the contract as accepted
				If (ExcecuteSQLNonQuery(String.Format("UPDATE Kunden_Doc_Online SET GetResult = 1, Get_On = '{0}' WHERE ID = {1} AND GetResult Is NULL", Date.Now, dbId)) = 1) Then

					' Read the document bytes
					ExcecuteSQLQuery(String.Format("SELECT DocScan FROM Kunden_Doc_Online WHERE ID =  {0}", dbId), AddressOf ProcessContractBytes)

					' Read the underlying DataSet of the grid
					Dim docDataSet As DataSet = CType(grid.DataSource, DataSet)

					' Find the correct DataRow
					Dim row As DataRow = docDataSet.Tables(0).Rows().Find(dbId)

					'Dim docInfo As String = row("Doc_Info")  ' geändert durch Sputnik 30.4.11
					Dim docInfo As String = row("DocFileName")

					Dim emailTempleText As String = String.Empty
					'Dim emailReceiver As String = String.Empty

					' A different template is used for KD and ZHD
					If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
						emailTempleText = Utility.GetEmailTemplate("KD_AcceptContract.txt")
						emailTempleText = FillAcceptContractEmailTempateForKD(emailTempleText, row)
						emailReceiver = row("KD_eMail")

					ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
						emailTempleText = Utility.GetEmailTemplate("ZHD_AcceptContract.txt")
						emailTempleText = FillAcceptContractEmailTempateForZHD(emailTempleText, row)
						emailReceiver = row("ZHD_eMail")

					Else
						Return
					End If

					' The adivsor is used as a CC-email receiver
					'Dim ccReceiver As String = row("User_eMail")
					ccReceiver = row("User_eMail")

					' hinzugefügt duch Sputnik 30.4.11 (es kann durch # mehrere Adressen übergeben werden.)
					ccReceiver = ccReceiver.Replace("#", ", ")
					emailReceiver = emailReceiver.Replace("#", ", ")

					' Send mail to (KD or ZHD) and the advisor
					Dim strAttachmentFilename As String = String.Format("{0}", docInfo)
					Utility.SendMailWithAttachment(appInfo.EmailSenderAddress, emailReceiver, ccReceiver, "Vertragsbestätigung", _
																				 emailTempleText, True, strAttachmentFilename, _
																				 DocScanBytes, "application/pdf")

				End If

			Catch ex As Exception
				Response.Write(String.Format("Fehler: (e.CommandName = AcceptContract) {0} ccReceiver: {1} emailReceiver: {2}", _
																		 ex.Message, ccReceiver, emailReceiver))
			End Try

			If isPrintNecessary Then
				PdfDocId = dbId
			End If
			Server.Transfer("~/Kunden/NewPropose.aspx")

		ElseIf e.CommandName = "DeclineContract" Then
			Try
				' Decline contract
				If ExcecuteSQLNonQuery(String.Format("UPDATE Kunden_Doc_Online SET GetResult = 2, Get_On = '{0}' WHERE ID = {1} AND GetResult Is NULL", Date.Now, dbId)) = 1 Then

					' Read the underlying DataSet of the grid
					Dim docDataSet As DataSet = CType(grid.DataSource, DataSet)

					' Find the correct DataRow
					Dim row As DataRow = docDataSet.Tables(0).Rows().Find(dbId)


					Dim emailTempleText As String = String.Empty
					'Dim emailReceiver As String = String.Empty


					' A different template is used for KD and ZHD
					If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
						emailTempleText = Utility.GetEmailTemplate("KD_DeclineContract.txt")
						emailTempleText = FillDeclineContractEmailTempateForKD(emailTempleText, row)
						emailReceiver = row("KD_eMail")
					ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
						emailTempleText = Utility.GetEmailTemplate("ZHD_DeclineContract.txt")
						emailTempleText = FillDeclineContractEmailTempateForZHD(emailTempleText, row)
						emailReceiver = row("ZHD_eMail")
					Else
						Return
					End If

					' The adivsor is used as a CC-email receiver
					'Dim ccReceiver As String = row("User_eMail")
					ccReceiver = row("User_eMail")
					' hinzugefügt duch Sputnik 30.4.11 (es kann durch # mehrere Adressen übergeben werden.)
					ccReceiver = ccReceiver.Replace("#", ", ")
					emailReceiver = emailReceiver.Replace("#", ", ")

					' Send mail to (KD or ZHD) and the advisor
					Utility.SendMail(appInfo.EmailSenderAddress, emailReceiver, ccReceiver, "Vertrag abgelehnt", emailTempleText, True)


				End If

			Catch ex As Exception
				Response.Write(String.Format("Fehler: (e.CommandName = DeclineContract) {0} ccReceiver: {1} emailReceiver: {2}", _
														 ex.Message, ccReceiver, emailReceiver))
			End Try
			Server.Transfer("~/Kunden/NewPropose.aspx")
		End If
	End Sub

	'GetPrintNecessary
	Private Function GetPrintNecessary() As Boolean

		'Only for debugging. Later remove this:
		' Return True ' geändert duch Sputnik: 5.10.2011

		' The connection to the database
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString()

		' The SQL Command
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.CommandText = "SELECT TOP 1 PrintVerleih FROM MySetting Where ID = @CustomerId"
		cmd.Connection = conn

		' The customerId is stored in the session
		If Not Session("CustomerId") Is Nothing Then

			Dim dataReader As System.Data.SqlClient.SqlDataReader = Nothing
			Try
				' Open connection to databse an read logo data
				conn.Open()
				' The customerId is stored in the session Sputnik 5.10.2011
				Dim customerId As String = Session("CustomerId")
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("@CustomerId", customerId)

				dataReader = cmd.ExecuteReader()
				dataReader.Read()

				Dim PrintVerleih As Integer = Utility.GetColumnInteger(dataReader, "PrintVerleih", 0)
				Return PrintVerleih = 1			' Sputnik 5.10.2011

			Finally
				dataReader.Close()
				conn.Close()
				conn.Dispose()
			End Try
		End If
		Return False
	End Function



	' Fills the accept contract email template for a KD
	Private Function FillAcceptContractEmailTempateForKD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

		' geändert durch Sputnik 30.4.11
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


	' Fills the decline contract email template for a KD
	Private Function FillDeclineContractEmailTempateForKD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

		' geändert durch Sputnik 30.4.11
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

	' Fills the accept contract email template for a ZHD
	Private Function FillAcceptContractEmailTempateForZHD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

		' geändert durch 30.4.11
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


	' Fills the decline contract email template for a ZHD
	Private Function FillDeclineContractEmailTempateForZHD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

		' geändert durch 30.4.11
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
	Protected Sub GridView_PageIndexChanging(ByVal sender As Object, _
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

#End Region

End Class