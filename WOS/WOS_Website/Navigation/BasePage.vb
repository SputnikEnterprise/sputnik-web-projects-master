'---------------------------------------------------------
' BasePage.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Net.Mail
Imports System.Data.SqlClient
Imports System.IO

''' <summary>
''' The base class of the application pages.
''' </summary>
Public MustInherit Class BasePage
	Inherits System.Web.UI.Page

#Region "Const"
#End Region

	' Application Info
	Dim _ApplicationInfo As ApplicationInfo
	Protected ReadOnly Property ApplicationInfo() As ApplicationInfo
		Get
			Return _ApplicationInfo
		End Get
	End Property

	' Constructor
	Public Sub New()
		_ApplicationInfo = New ApplicationInfo()
	End Sub

	' Checks if GUID is valid.
	Public Function IsGuiValid() As Boolean

		'// Check GET-Parameter
		Try
			' Check if the GUID name is correct
			If Not String.IsNullOrEmpty(Request.Item(ApplicationInfo.CandidateQueryParameterName)) Then
				ApplicationInfo.CurrentGuidName = ApplicationInfo.CandidateQueryParameterName
			ElseIf Not String.IsNullOrEmpty(Request.Item(ApplicationInfo.KDQueryParameterName)) Then
				ApplicationInfo.CurrentGuidName = ApplicationInfo.KDQueryParameterName
			ElseIf Not String.IsNullOrEmpty(Request.Item(ApplicationInfo.ZHDQueryParameterName)) Then
				ApplicationInfo.CurrentGuidName = ApplicationInfo.ZHDQueryParameterName
			Else
				Return False
			End If

			' Stores the current GUID name
			ApplicationInfo.CurrentGuidValue = Request.QueryString(ApplicationInfo.CurrentGuidName)
			ApplicationInfo.DocumentSQLStrings = GetDocumentSQLStrings(ApplicationInfo.CurrentGuidName)

			' Checks if the GUID has the correct format
			Dim regex As System.Text.RegularExpressions.Regex = New System.Text.RegularExpressions.Regex(Me.ApplicationInfo.RegexGui)
			If Not regex.IsMatch(ApplicationInfo.CurrentGuidValue) Then
				Return False
			End If

			' Stores the application info object in the session
			Session.Add(ApplicationInfo.SESSION_KEY, _ApplicationInfo)

			Return True
		Catch
		End Try

		Return False

	End Function


	' Tries to load the customer data based on the supplied GUID.
	Public Function TryToLoadCustomerData() As Boolean

		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = Me.ApplicationInfo.ConnectionString

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.Connection = conn

		' Set the correct SQL query based on the query parameter name
		cmd.CommandText = ApplicationInfo.DocumentSQLStrings.MySettingsCustomerDataSQL

		' Add the current GUID value to the SQL query
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@CurrentGuidValue", ApplicationInfo.CurrentGuidValue)

		Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
		Dim success = True

		Try
			conn.Open()
			dr = cmd.ExecuteReader()

			If dr.HasRows Then

				dr.Read()

				' Read the customer information
				If Not dr.IsDBNull(dr.GetOrdinal("ID")) Then
					Session.Add("CustomerId", dr.Item("ID"))
				End If


				' Reads customer data
				Session.Add("CustomerName", Utility.GetColumnTextStr(dr, "Customer_Name", String.Empty))
				Session.Add("CustomerStreet", Utility.GetColumnTextStr(dr, "Customer_Strasse", String.Empty))
				Session.Add("CustomerPlace", Utility.GetColumnTextStr(dr, "Customer_Ort", String.Empty))

				Dim telephone As String = Utility.GetColumnTextStr(dr, "Customer_Telefon", String.Empty)
				Dim telefax As String = Utility.GetColumnTextStr(dr, "Customer_Telefax", String.Empty)
				Dim eMail As String = Utility.GetColumnTextStr(dr, "Customer_Email", String.Empty)

				If Not String.IsNullOrWhiteSpace(telephone) Then
					If Not String.IsNullOrWhiteSpace(telefax) Then
						Session.Add("CustomerTelephone", String.Format("Tel.: {0}<br>Fax: {1}<br>EMail: {2}", telephone, telefax, eMail))
					Else
						Session.Add("CustomerTelephone", String.Format("Tel.: {0}<br>EMail: {1}", telephone, eMail))
					End If
				End If

				'Session.Add("CustomerEMail", "EMail:" & Utility.GetColumnTextStr(dr, "Customer_Email", String.Empty))
				Session.Add("CustomerHomepage", Utility.GetColumnTextStr(dr, "Customer_Homepage", String.Empty))

				dr.Close()

				' Change sql command to retrieve the welcome text
				cmd.CommandText = ApplicationInfo.DocumentSQLStrings.WelcomeMessageSQL

				dr = cmd.ExecuteReader()
				dr.Read()

				If dr.HasRows Then

					' Gets the welcome text from a concrete sub class
					Session.Add("WelcomeName", GetWelcomeText(dr))
					Session.Add("UserName", GetUserName(dr))

					Dim userEmail As String = GetUserEmail(dr).Split("#")(0)
					Session.Add("UserEmail", userEmail)
					Dim language As String = Utility.GetColumnTextStr(dr, "Language", "")
					If language.Length >= 2 Then
						Session.Add(Imt.Common.I18N.TranslationService.KEY_LANGUAGE, language.Substring(0, 2))
					End If
					Session.Add("WelcomeSubText", GetWelcomeSubText())

				Else
					Return False
				End If
			Else
				Return False

			End If

		Catch ex As Exception
			Return False
		Finally
			dr.Close()
			conn.Close()
			conn.Dispose()
		End Try

		Return True

	End Function

	' Executes a non query SQL Command
	Public Function ExcecuteSQLNonQuery(ByVal sqlNonQuery As String) As Integer
		' Creates the connection object
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = Me.ApplicationInfo.ConnectionString

		' Creates the sql command
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.Connection = conn
		cmd.CommandText = sqlNonQuery

		Try
			conn.Open()

			' executes the non query command
			Return cmd.ExecuteNonQuery()

		Finally
			If Not conn.State = ConnectionState.Closed Then
				conn.Close()
			End If
		End Try

		Return -1

	End Function

	' Executes a query SQL Command
	Public Sub ExcecuteSQLQuery(ByVal sqlNonQuery As String, ByRef rowProcessor As Action(Of SqlDataReader))

		' Creates the connection object
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = Me.ApplicationInfo.ConnectionString

		' Creates the command object
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.Connection = conn
		cmd.CommandText = sqlNonQuery

		Dim dr As System.Data.SqlClient.SqlDataReader

		Try
			conn.Open()

			' Executes the reader
			dr = cmd.ExecuteReader()

			While (dr.Read())
				' call the action method
				rowProcessor(dr)
			End While

			dr.Close()
		Finally
			If Not conn.State = ConnectionState.Closed Then
				conn.Close()
			End If
		End Try

	End Sub

	' Gets a DocumentSQLStrings object
	Public Function GetDocumentSQLStrings(ByVal param As String) As DocumentSQLStrings

		Dim documentSQLStrings As DocumentSQLStrings = New DocumentSQLStrings()

		If ApplicationInfo.CurrentGuidName = ApplicationInfo.CandidateQueryParameterName Then
			'			documentSQLStrings.MySettingsCustomerDataSQL = "SELECT TOP 1 * FROM MySetting WHERE MA_Guid = (SELECT TOP 1 Customer_ID FROM Kandidaten WHERE MA_Guid = @CurrentGuidValue)"
			'			documentSQLStrings.WelcomeMessageSQL = "SELECT TOP 1 MA.MA_Vorname, MA.MA_Nachname, MA.MA_Language AS Language, MA.MA_EMail FROM Kandidaten MA WHERE MA.MA_Guid = @CurrentGuidValue"
			'			documentSQLStrings.LoadDocumentInformationSQL = "SELECT ID, Doc_Art, Doc_info, DocFileName, convert(varchar(10),Transfered_On,104) As Transfered_On, Transfered_On AS Orig_Transfered_On FROM Kandidaten_Doc_Online WHERE DOC_ART = @Doc_Art AND Owner_Guid = @CurrentGuid "
			'			documentSQLStrings.LoadDocumentForEmailForwardSQL = "SELECT ID, DocScan, Doc_Art, Doc_info, DocFileName, MA_Vorname, MA_Nachname, " &
			'"IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, " &
			'"IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, " &
			'"IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_eMail, " &
			'"IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, " &
			'				"User_Vorname, User_Nachname, convert(varchar(10),Transfered_On,104) As Transfered_On FROM Kandidaten_Doc_Online WHERE DOC_ART = @Doc_Art AND Owner_Guid = @CurrentGuid AND ID = @ID "
			'			'documentSQLStrings.LoadDocumentForEmailForwardSQL = "SELECT ID, DocScan, Doc_Art, Doc_info, DocFileName, MA_Vorname, MA_Nachname, User_Telefon, User_Telefax, User_eMail, User_Vorname, User_Nachname, convert(varchar(10),Transfered_On,104) As Transfered_On FROM Kandidaten_Doc_Online WHERE DOC_ART = @Doc_Art AND Owner_Guid = @CurrentGuid AND ID = @ID "
			'			documentSQLStrings.LoadDocScanSQL = "SELECT [DocScan] FROM Kandidaten_Doc_Online WHERE ID = @ID AND Owner_Guid = @CurrentGuid ORDER BY Transfered_On DESC"

			documentSQLStrings = BuildDocumentSQLMAQuery(param)

		ElseIf ApplicationInfo.CurrentGuidName = ApplicationInfo.KDQueryParameterName Then
			'			documentSQLStrings.MySettingsCustomerDataSQL = "SELECT TOP 1 * FROM MySetting WHERE KD_Guid = (SELECT TOP 1 Customer_ID FROM Kunden WHERE KD_Guid = @CurrentGuidValue)"
			'			documentSQLStrings.WelcomeMessageSQL = "SELECT TOP 1 KD.KD_Name, KD.KD_Language AS Language, KD.KD_eMail FROM Kunden KD WHERE KD.KD_Guid =  @CurrentGuidValue"
			'			documentSQLStrings.LoadDocumentInformationSQL = "SELECT ID, Doc_Art, Doc_info, DocFileName, KD_eMail, ZHD_eMail, KD_Berater, ZHD_BriefAnrede, User_Telefon, User_Telefax, User_eMail, ESNr, convert(varchar(10),Transfered_On,104) as Transfered_On, Transfered_On AS Orig_Transfered_On, convert(varchar(10),Get_On,104) As Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND KD_Guid = @CurrentGuid "
			'			documentSQLStrings.LoadDocumentForEmailForwardSQL = "SELECT ID, DocScan, Doc_Art, Doc_info, DocFileName, KD_eMail, ZHD_eMail, KD_Berater, ZHD_BriefAnrede, " &
			'"IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, " &
			'"IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, " &
			'"IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_eMail, " &
			'"IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, " &
			'			"ESNr, KD_Name, " &
			'			"convert(varchar(10),Transfered_On,104) as Transfered_On, " &
			'			"convert(varchar(10),Get_On,104) As Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND KD_Guid = @CurrentGuid AND ID = @ID"
			'			documentSQLStrings.LoadDocScanSQL = "SELECT [DocScan] FROM Kunden_Doc_Online WHERE ID = @ID AND KD_Guid = @CurrentGuid ORDER BY Transfered_On DESC"

			documentSQLStrings = BuildDocumentSQLKDQuery(param)

		ElseIf ApplicationInfo.CurrentGuidName = ApplicationInfo.ZHDQueryParameterName Then
			'			documentSQLStrings.MySettingsCustomerDataSQL = "SELECT TOP 1 * FROM MySetting WHERE KD_Guid = (SELECT TOP 1 Customer_ID FROM Kunden_ZHD WHERE ZHD_Guid = @CurrentGuidValue)"

			'			'documentSQLStrings.WelcomeMessageSQL = "SELECT TOP 1 ZHD.ZHD_Vorname, ZHD.ZHD_Nachname, ZHD.ZHDSex, KD.KD_Language AS Language, ZHD.ZHD_eMail FROM Kunden_ZHD ZHD LEFT JOIN  Kunden KD ON ZHD.KD_Guid = KD.KD_Guid WHERE ZHD.ZHD_Guid = @CurrentGuidValue"
			'			documentSQLStrings.WelcomeMessageSQL = "SELECT TOP 1 ZHD.ZHD_Vorname, ZHD.ZHD_Nachname, ZHD.ZHDSex, KD.KD_Language AS Language, (SELECT Top 1 ZHD_EMail From Kunden_ZHD WHERE zhd_guid = @CurrentGuidValue) ZHD_eMail FROM Kunden_ZHD ZHD LEFT JOIN  Kunden KD ON ZHD.KD_Guid = KD.KD_Guid WHERE ZHD.ZHD_Guid = @CurrentGuidValue"

			'			documentSQLStrings.LoadDocumentInformationSQL = "SELECT ID, Doc_Art, Doc_info, DocFileName, KD_eMail, (SELECT Top 1 ZHD_EMail From Kunden_ZHD WHERE zhd_guid = @CurrentGuid) ZHD_eMail, ZHD_Berater, ZHD_BriefAnrede, ZHD_Nachname, User_Telefon, User_Telefax, User_eMail, ESNr, convert(varchar(10),Transfered_On,104) As Transfered_On, Transfered_On AS Orig_Transfered_On, Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND ZHD_Guid = @CurrentGuid "

			'			documentSQLStrings.LoadDocumentForEmailForwardSQL = "SELECT ID, DocScan, Doc_Art,  Doc_info, DocFileName, KD_eMail, (SELECT Top 1 ZHD_EMail From Kunden_ZHD WHERE zhd_guid = @CurrentGuid) ZHD_eMail, ZHD_Berater, ZHD_BriefAnrede, ZHD_Vorname, ZHD_Nachname, KD_Name, " &
			'"IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, " &
			'"IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, " &
			'"IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_eMail, " &
			'"IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, " &
			'				"ESNr, convert(varchar(10),Transfered_On,104) As Transfered_On,  Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND ZHD_Guid = @CurrentGuid AND ID = @ID "

			'			documentSQLStrings.LoadDocScanSQL = "SELECT [DocScan] FROM Kunden_Doc_Online WHERE ID = @ID AND ZHD_Guid = @CurrentGuid ORDER BY Transfered_On DESC"
			documentSQLStrings = BuildDocumentSQLZHDQuery(param)

		Else

			Return Nothing
		End If

		Return documentSQLStrings
	End Function

	Private Function BuildDocumentSQLMAQuery(ByVal param As String) As DocumentSQLStrings
		Dim documentSQLStrings As DocumentSQLStrings = New DocumentSQLStrings()

		Dim sql As String

		sql = "SELECT TOP 1 * FROM MySetting WHERE WOS_Guid = (SELECT TOP 1 WOS_Guid FROM Kandidaten WHERE MA_Guid = @CurrentGuidValue)"
		documentSQLStrings.MySettingsCustomerDataSQL = sql


		sql = "SELECT TOP 1 MA.MA_Vorname, MA.MA_Nachname, MA.MA_Language AS Language, MA.MA_EMail FROM Kandidaten MA WHERE MA.MA_Guid = @CurrentGuidValue"
		documentSQLStrings.WelcomeMessageSQL = sql


		sql = "SELECT ID, Doc_Art, Doc_info, DocFileName, convert(varchar(10),Transfered_On,104) As Transfered_On, Transfered_On AS Orig_Transfered_On FROM Kandidaten_Doc_Online WHERE DOC_ART = @Doc_Art AND Owner_Guid = @CurrentGuid "
		documentSQLStrings.LoadDocumentInformationSQL = sql


		sql = "SELECT ID, DocScan, Doc_Art, Doc_info, DocFileName, MA_Vorname, MA_Nachname, "
		sql &= "IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, "
		sql &= "IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, "
		sql &= "IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_eMail, "
		sql &= "IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kandidaten_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kandidaten_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, "
		sql &=	"User_Vorname, User_Nachname, convert(varchar(10),Transfered_On,104) As Transfered_On FROM Kandidaten_Doc_Online WHERE DOC_ART = @Doc_Art AND Owner_Guid = @CurrentGuid AND ID = @ID "
		documentSQLStrings.LoadDocumentForEmailForwardSQL = sql


		sql = "SELECT [DocScan] FROM Kandidaten_Doc_Online WHERE ID = @ID AND Owner_Guid = @CurrentGuid ORDER BY Transfered_On DESC"
		documentSQLStrings.LoadDocScanSQL = sql


		Return documentSQLStrings

	End Function

	Private Function BuildDocumentSQLKDQuery(ByVal param As String) As DocumentSQLStrings
		Dim documentSQLStrings As DocumentSQLStrings = New DocumentSQLStrings()

		Dim sql As String

		sql = "SELECT TOP 1 * FROM MySetting WHERE WOS_Guid = (SELECT TOP 1 WOS_Guid FROM Kunden WHERE KD_Guid = @CurrentGuidValue)"
		documentSQLStrings.MySettingsCustomerDataSQL = sql


		sql = "SELECT TOP 1 KD.KD_Name, KD.KD_Language AS Language, KD.KD_eMail FROM Kunden KD WHERE KD.KD_Guid =  @CurrentGuidValue"
		documentSQLStrings.WelcomeMessageSQL = sql


		sql = "SELECT ID, Doc_Art, Doc_info, DocFileName, KD_eMail, ZHD_eMail, KD_Berater, ZHD_BriefAnrede, User_Telefon, User_Telefax, User_eMail, ESNr, convert(varchar(10),Transfered_On,104) as Transfered_On, Transfered_On AS Orig_Transfered_On, "
		sql &= "convert(varchar(10),Get_On,104) As Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art And KD_Guid = @CurrentGuid "
		documentSQLStrings.LoadDocumentInformationSQL = sql


		sql = "SELECT ID, DocScan, Doc_Art, Doc_info, DocFileName, KD_eMail, ZHD_eMail, KD_Berater, ZHD_BriefAnrede, "
		sql &= "IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, "
		sql &= "IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, "
		sql &= "IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_eMail, "
		sql &= "IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, "
		sql &= "ESNr, KD_Name, "
		sql &= "convert(varchar(10),Transfered_On,104) as Transfered_On, "
		sql &= "convert(varchar(10),Get_On,104) As Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND KD_Guid = @CurrentGuid AND ID = @ID"
		documentSQLStrings.LoadDocumentForEmailForwardSQL = sql


		sql = "SELECT [DocScan] FROM Kunden_Doc_Online WHERE ID = @ID AND KD_Guid = @CurrentGuid ORDER BY Transfered_On DESC"
		documentSQLStrings.LoadDocScanSQL = sql


		sql = "UPDATE Dbo.[tbl_Customer_WOSDocument_State] SET [ViewedResult] = 1, [Viewed_On] = GetDate() WHERE ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where KD_Guid = @CurrentGuid AND ID = @ID)"
		documentSQLStrings.UpdateDocumentView = sql


		Return documentSQLStrings

	End Function

	Private Function BuildDocumentSQLZHDQuery(ByVal param As String) As DocumentSQLStrings
		Dim documentSQLStrings As DocumentSQLStrings = New DocumentSQLStrings()

		Dim sql As String

		sql = "SELECT TOP 1 * FROM MySetting WHERE WOS_Guid = (SELECT TOP 1 WOS_Guid FROM Kunden_ZHD WHERE ZHD_Guid = @CurrentGuidValue)"
		documentSQLStrings.MySettingsCustomerDataSQL = sql


		sql = "SELECT TOP 1 ZHD.ZHD_Vorname, ZHD.ZHD_Nachname, ZHD.ZHDSex, KD.KD_Language AS Language, "
		sql &= "(SELECT Top 1 IsNull(ZHD_EMail, '') From Kunden_ZHD WHERE zhd_guid = @CurrentGuidValue) ZHD_eMail "
		sql &= "FROM Kunden_ZHD ZHD LEFT JOIN Kunden KD "
		sql &= "ON ZHD.KD_Guid = KD.KD_Guid "
		sql &= "WHERE ZHD.ZHD_Guid = @CurrentGuidValue"
		documentSQLStrings.WelcomeMessageSQL = sql


		sql = "SELECT ID, Doc_Art, Doc_info, DocFileName, KD_eMail, (SELECT Top 1 ZHD_EMail From Kunden_ZHD WHERE zhd_guid = @CurrentGuid) ZHD_eMail, "
		sql &= "ZHD_Berater, ZHD_BriefAnrede, ZHD_Nachname, User_Telefon, User_Telefax, User_eMail, "
		sql &= "ESNr, convert(varchar(10),Transfered_On,104) As Transfered_On, "
		sql &= "Transfered_On AS Orig_Transfered_On, Get_ON "
		sql &= "FROM Kunden_Doc_Online "
		sql &= "WHERE DOC_ART = @Doc_Art And ZHD_Guid = @CurrentGuid "
		documentSQLStrings.LoadDocumentInformationSQL = sql


		sql = "SELECT ID, DocScan, Doc_Art,  Doc_info, DocFileName, KD_eMail, (SELECT Top 1 ZHD_EMail From Kunden_ZHD WHERE zhd_guid = @CurrentGuid) ZHD_eMail, ZHD_Berater, ZHD_BriefAnrede, ZHD_Vorname, ZHD_Nachname, KD_Name, "
		sql &= "IsNull( (Select Top 1 User_Telefon From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefon, "
		sql &= "IsNull( (Select Top 1 User_Telefax From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Telefax, "
		sql &= "IsNull( (Select Top 1 User_eMail From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_eMail, "
		sql &= "IsNull( (Select Top 1 User_Homepage From Customer_Users Where Customer_ID = IsNull((Select Top 1 Customer_ID From Kunden_Doc_Online WHERE ID = @ID), '') And User_ID = IsNull((Select Top 1 LogedUser_ID From Kunden_Doc_Online WHERE ID = @ID), '')), '') User_Homepage, "
		sql &= "ESNr, convert(varchar(10),Transfered_On,104) As Transfered_On,  Get_ON FROM Kunden_Doc_Online WHERE DOC_ART = @Doc_Art AND ZHD_Guid = @CurrentGuid AND ID = @ID "
		documentSQLStrings.LoadDocumentForEmailForwardSQL = sql


		sql = "SELECT [DocScan] FROM Kunden_Doc_Online WHERE ID = @ID AND ZHD_Guid = @CurrentGuid ORDER BY Transfered_On DESC"
		documentSQLStrings.LoadDocScanSQL = sql


		sql = "UPDATE Dbo.[tbl_Customer_WOSDocument_State] SET [ViewedResult] = 1, [Viewed_On] = GetDate(), [NotifyAdvisor] = case when ViewedResult = 1 then [NotifyAdvisor] else 'true' end WHERE ID = (Select Top (1) FK_StateID From Kunden_Doc_Online Where ZHD_Guid = @CurrentGuid AND ID = @ID)"
		documentSQLStrings.UpdateDocumentView = sql


		Return documentSQLStrings

	End Function

	' The welcome  text is supplied by a concrete sub class
	Public MustOverride Function GetWelcomeText(ByVal dr As SqlDataReader) As String
	Public MustOverride Function GetUserName(ByVal dr As SqlDataReader) As String
	Public MustOverride Function GetUserEmail(ByVal dr As SqlDataReader) As String

	''' <summary>
	''' Returns sub welcome text for candidate pages
	''' </summary>
	Private Function GetWelcomeSubText() As String

		If (ApplicationInfo.CurrentGuidName = ApplicationInfo.CandidateQueryParameterName) Then
			Return Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_MASTER_PREPARED_DOCUMENTS_SP")
		ElseIf (ApplicationInfo.CurrentGuidName = ApplicationInfo.KDQueryParameterName) Then
			Return Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_MASTER_PREPARED_DOCUMENTS_KD")
		ElseIf (ApplicationInfo.CurrentGuidName = ApplicationInfo.ZHDQueryParameterName) Then
			Return Imt.Common.I18N.TranslationService.Instance.GetStringValue("TEXT_MASTER_PREPARED_DOCUMENTS_ZHD")
		Else
			Return String.Empty
		End If

	End Function
End Class
