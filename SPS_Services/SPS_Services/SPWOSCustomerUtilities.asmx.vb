
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.WOSUtilities
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.WOSUtilities.WOSDbAccess

Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DocumentScan
Imports wsSPS_Services.WOSInfo
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.Logging


' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPWOSCustomerUtilities.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPWOSCustomerUtilities
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPWOSCustomerUtilities"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As systeminfoDatabaseAccess
	Private m_Scan As ScanDatabaseAccess
	Private m_WOS As WOSDatabaseAccess
	Private m_NewWOS As WOSDatabaseAccess
	Private m_PublicData As PublicDataDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities

		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_Scan = New ScanDatabaseAccess(My.Settings.ConnStr_Scanning, Language.German)
		m_WOS = New WOSDatabaseAccess(My.Settings.ConnStr_spContract, Language.German)
		m_NewWOS = New WOSDatabaseAccess(My.Settings.ConnStr_New_spContract, Language.German)
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub

	<WebMethod(Description:="Zur Sicherung eines Kundendokuments aus der lokalen Datenbank.")>
	Function AddAssignedCustomerWOSDocument(ByVal customerID As String, ByVal customerWosData As CustomerWOSData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			If customerWosData Is Nothing Then Return False
			Dim ownerData = m_NewWOS.LoadAssignedWOSOwnerMasterData(customerWosData.CustomerWOSID, WOSModulData.ModulArt.CustomerDocument, False)
			If ownerData Is Nothing Then Throw New Exception(String.Format("{0}: ownder data could not be founded!", customerWosData.CustomerWOSID))

			If Not String.IsNullOrWhiteSpace(customerWosData.CustomerWOSID) Then
				result = m_NewWOS.AddWOSCustomerDocumentData(m_customerID, customerWosData.CustomerWOSID, customerWosData)
			End If

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAssignedCustomerWOSDocument", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Zur Kontrolle eines Kunden Moduls.")>
	Function LoadAssignedCustomerWOSModul(ByVal customerID As String, ByVal CustomerWOSID As String, ByVal modulGuid As String, ByVal modulNumber As Integer) As CustomerWOSDataDTO()
		Dim result As List(Of CustomerWOSDataDTO) = Nothing
		m_customerID = customerID

		Try
			If String.IsNullOrWhiteSpace(CustomerWOSID) Then Return Nothing

			result = m_NewWOS.LoadAssignedCustomerWOSData(m_customerID, CustomerWOSID, modulGuid, modulNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCustomerWOSModul", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray
	End Function

	<WebMethod(Description:="Zur Kontrolle eines Kunden Moduls.")>
	Function LoadAssignedCustomerWOSModulByDocArt(ByVal customerID As String, ByVal CustomerWOSID As String, ByVal modulGuid As String, ByVal modulNumber As Integer, ByVal modulDocArt As Integer) As CustomerWOSDataDTO()
		Dim result As List(Of CustomerWOSDataDTO) = Nothing
		m_customerID = customerID

		Try
			If String.IsNullOrWhiteSpace(CustomerWOSID) Then Return Nothing

			result = m_NewWOS.LoadAssignedCustomerWOSDataByDocArt(m_customerID, CustomerWOSID, modulGuid, modulNumber, modulDocArt)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCustomerWOSModulByDocArt", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="set notification field to done")>
	Function UpdateAssignedDocNotificationAsDone(ByVal customerID As String, ByVal CustomerWOSID As String, ByVal modulGuid As String, ByVal recID As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			If String.IsNullOrWhiteSpace(CustomerWOSID) Then Return Nothing

			result = m_NewWOS.UpdateAssignedDocNotificationAsDone(m_customerID, CustomerWOSID, modulGuid, recID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedDocNotificationAsDone", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function

#Region "vacancy data"

	<WebMethod(Description:="loads all vacancies for customerwosid.")>
	Function LoadTransferedVacancyDataFromWOS(ByVal customerID As String, ByVal customerWosID As String, ByVal kdNumber As Integer?, ByVal vakNumber As Integer?) As KDVakanzenDTO()
		Dim result As List(Of KDVakanzenDTO) = Nothing
		m_customerID = customerID

		Try
			If customerWosID Is Nothing OrElse String.IsNullOrWhiteSpace(customerWosID) Then Return Nothing
			Dim ownerData = m_NewWOS.LoadAssignedWOSOwnerMasterData(customerWosID, WOSModulData.ModulArt.VacancyDodument, False)
			If ownerData Is Nothing Then Throw New Exception(String.Format("{0}: owner data could not be founded!", customerWosID))

			result = m_NewWOS.LoadVacancyData(m_customerID, customerWosID, kdNumber, vakNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTransferedVacancyDataFromWOS", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="delete vacancy for customerwosid.")>
	Function DeleteVacancyDataFromWOS(ByVal customerID As String, ByVal customerWosID As String, ByVal vakNumbers As Integer()) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			If Not String.IsNullOrWhiteSpace(customerWosID) Then
				For Each number In vakNumbers
					result = m_NewWOS.DeleteVacancyData(m_customerID, customerWosID, number)
				Next
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeleteVacancyDataFromWOS", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="delete vacancy for customerwosid.")>
	Function DeleteAssignedVacancyDataFromWOS(ByVal customerID As String, ByVal customerWosID As String, ByVal vakNumber As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			If Not String.IsNullOrWhiteSpace(customerWosID) Then result = m_NewWOS.DeleteVacancyData(m_customerID, customerWosID, vakNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeleteAssignedVacancyDataFromWOS", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


#End Region


	'Dim wosUtility As New WOSDbAccess
	'	Dim utility As New ClsUtilities

	'	If (customerWosData Is Nothing) Then
	'		Throw New Exception("customerWosData is nothing!")
	'	End If

	'	Dim allowedToUse = wosUtility.IsUserAllowedForUsingService(customerWosData.CustomerWOSID, WOSModulData.ModulArt.CustomerDocument)
	'	If Not allowedToUse Then
	'		Throw New Exception(String.Format("NotAllowed: GetUserID: {0}", customerWosData.CustomerWOSID))
	'	End If
	'	Dim connString As String = My.Settings.ConnStr_spContract
	'	Dim strMessage As New StringBuilder()


	'	Dim conn As SqlConnection = Nothing
	'	Try

	'		' Extract parameter data from data row.
	'		Dim kdNr As Integer? = customerWosData.CustomerNumber
	'		Dim ZHDNr As Integer? = customerWosData.CresponsibleNumber
	'		Dim ESNr As Integer? = customerWosData.EmploymentNumber
	'		Dim ESLohnNr As Integer? = customerWosData.EmploymentLineNumber
	'		Dim RPNr As Integer? = customerWosData.ReportNumber
	'		Dim RENr As Integer? = customerWosData.InvoiceNumber
	'		Dim proposeNr As Integer? = customerWosData.ProposeNumber

	'		Dim MDName As String = customerWosData.customername
	'		Dim LogedUser_Guid As String = customerWosData.LogedUserID
	'		Dim Customer_ID As String = customerWosData.CustomerWOSID

	'		Dim KD_Name As String = customerWosData.KD_Name
	'		Dim ZHD_Vorname As String = customerWosData.ZHD_Vorname
	'		Dim ZHD_Nachname As String = customerWosData.ZHD_Nachname
	'		Dim KD_Filiale As String = customerWosData.KD_Filiale

	'		Dim KD_Postfach As String = customerWosData.KD_Postfach
	'		Dim KD_Strasse As String = customerWosData.KD_Strasse
	'		Dim KD_PLZ As String = customerWosData.KD_PLZ
	'		Dim KD_Kanton As String = String.Empty
	'		Dim KD_Ort As String = customerWosData.KD_Ort
	'		Dim KD_AGB_Wos As String = customerWosData.KD_AGB_Wos
	'		Dim ZHDSex As String = customerWosData.ZHDSex
	'		Dim ZHD_Briefanrede As String = customerWosData.Zhd_BriefAnrede
	'		Dim DoNotShowContractInWOS As Boolean? = customerWosData.DoNotShowContractInWOS

	'		Dim KD_EMail As String = customerWosData.KD_Email
	'		Dim myArray = KD_EMail.Split("#"c)
	'		KD_EMail = String.Join(",", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

	'		Dim KD_Guid As String = customerWosData.KDTransferedGuid
	'		Dim ZHD_Guid As String = customerWosData.ZHDTransferedGuid
	'		Dim Doc_Guid As String = customerWosData.AssignedDocumentGuid
	'		Dim Doc_Art As String = customerWosData.AssignedDocumentArtName
	'		Dim Doc_Info As String = customerWosData.AssignedDocumentInfo
	'		Dim Result As String = String.Empty
	'		Dim KD_Berater As String = customerWosData.KD_Berater
	'		Dim ZHD_Berater As String = customerWosData.Zhd_Berater

	'		Dim KD_Beruf As String = customerWosData.KD_Beruf
	'		myArray = KD_Beruf.Split("#"c)
	'		KD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

	'		Dim KD_Branche As String = customerWosData.KD_Branche
	'		myArray = KD_Branche.Split("#"c)
	'		KD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

	'		Dim ZHD_Beruf As String = customerWosData.Zhd_Beruf
	'		myArray = ZHD_Beruf.Split("#"c)
	'		ZHD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

	'		Dim ZHD_Branche As String = customerWosData.Zhd_Branche
	'		myArray = ZHD_Branche.Split("#"c)
	'		ZHD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

	'		Dim ZHD_AGB_WOS As String = customerWosData.ZHD_AGB_Wos
	'		Dim ZHD_GebDat As DateTime? = customerWosData.ZHD_GebDat
	'		Dim TransferedUser As String = String.Empty

	'		Dim US_Nachname As String = customerWosData.UserName
	'		Dim US_Vorname As String = customerWosData.UserVorname
	'		Dim US_Telefon As String = customerWosData.UserTelefon
	'		Dim US_Telefax As String = customerWosData.UserTelefax
	'		Dim US_eMail As String = customerWosData.UserMail

	'		Dim MD_Telefon As String = customerWosData.MDTelefon
	'		Dim MD_DTelefon As String = customerWosData.MD_DTelefon
	'		Dim MD_Telefax As String = customerWosData.MDTelefax
	'		Dim MD_eMail As String = customerWosData.MDMail
	'		If String.IsNullOrWhiteSpace(MD_Telefon) Then MD_Telefon = US_Telefon
	'		If String.IsNullOrWhiteSpace(MD_DTelefon) Then MD_DTelefon = US_Telefon
	'		If String.IsNullOrWhiteSpace(MD_Telefax) Then MD_Telefax = US_Telefax
	'		If String.IsNullOrWhiteSpace(MD_eMail) Then MD_eMail = US_eMail

	'		Dim KD_Language As String = customerWosData.KD_Language
	'		Dim ZHD_EMail As String = customerWosData.ZHD_EMail
	'		Dim DocFilename As String = customerWosData.ScanDocName
	'		Dim DocScan As Byte() = customerWosData.ScanDoc

	'		Dim User_Initial As String = customerWosData.UserInitial
	'		Dim User_Sex As String = customerWosData.UserSex
	'		Dim User_Filiale As String = customerWosData.UserFiliale
	'		Dim UserSign As Byte() = customerWosData.UserSign
	'		Dim UserPicture As Byte() = customerWosData.UserPicture


	'		conn = New SqlConnection(connString)

	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Create New Customer Document For WOS]", conn)
	'		cmd.CommandType = CommandType.StoredProcedure

	'		Dim listOfParams As New List(Of SqlClient.SqlParameter)

	'		listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(kdNr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(ZHDNr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(ESNr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(ESLohnNr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(RPNr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(RENr, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(proposeNr, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("LogedUser_Guid", ReplaceMissing(LogedUser_Guid, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(Customer_ID, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Name", ReplaceMissing(KD_Name, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Vorname", ReplaceMissing(ZHD_Vorname, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Nachname", ReplaceMissing(ZHD_Nachname, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Filiale", ReplaceMissing(KD_Filiale, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Postfach", ReplaceMissing(KD_Postfach, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Strasse", ReplaceMissing(KD_Strasse, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("KD_PLZ", ReplaceMissing(KD_PLZ, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Kanton", ReplaceMissing(KD_Kanton, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Ort", ReplaceMissing(KD_Ort, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_AGB_Wos", ReplaceMissing(KD_AGB_Wos, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHDSex", ReplaceMissing(ZHDSex, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Briefanrede", ReplaceMissing(ZHD_Briefanrede, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("DoNotShowContractInWOS", ReplaceMissing(DoNotShowContractInWOS, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_EMail", ReplaceMissing(KD_EMail, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Guid", ReplaceMissing(KD_Guid, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Guid", ReplaceMissing(ZHD_Guid, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(Result, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Berater", ReplaceMissing(KD_Berater, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Berater", ReplaceMissing(ZHD_Berater, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Beruf", ReplaceMissing(KD_Beruf, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Branche", ReplaceMissing(KD_Branche, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Beruf", ReplaceMissing(ZHD_Beruf, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_Branche", ReplaceMissing(ZHD_Branche, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_AGB_WOS", ReplaceMissing(ZHD_AGB_WOS, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("TransferedUser", ReplaceMissing(TransferedUser, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("US_Nachname", ReplaceMissing(US_Nachname, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("US_Vorname", ReplaceMissing(US_Vorname, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("US_Telefon", ReplaceMissing(MD_Telefon, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("US_Telefax", ReplaceMissing(MD_Telefax, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("US_eMail", ReplaceMissing(MD_eMail, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("KD_Language", ReplaceMissing(KD_Language, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("ZHD_EMail", ReplaceMissing(ZHD_EMail, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(DocFilename, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(DocScan, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("Customer_Name", ReplaceMissing(MDName, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(User_Initial, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("User_Sex", ReplaceMissing(User_Sex, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(User_Filiale, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(UserPicture, DBNull.Value)))
	'		listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(UserSign, DBNull.Value)))

	'		listOfParams.Add(New SqlClient.SqlParameter("SignTransferedDocument", ReplaceMissing(customerWosData.SignTransferedDocument.GetValueOrDefault(False), False)))

	'		cmd.Parameters.AddRange(listOfParams.ToArray())

	'		conn.Open()

	'		For i As Integer = 0 To cmd.Parameters.Count - 1
	'			strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
	'																			cmd.Parameters(i).ParameterName,
	'																			cmd.Parameters(i).DbType,
	'																			cmd.Parameters(i).Size,
	'																			cmd.Parameters(i).Value,
	'																			ControlChars.NewLine))
	'		Next

	'		cmd.ExecuteNonQuery()

	'	Catch ex As Exception
	'		Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
	'		utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = If(customerWosData Is Nothing, String.Empty, customerWosData.CustomerWOSID),
	'																																.SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddCustomerDocumentToWOS", .MessageContent = msgContent})

	'		'wosUtility.SaveErrToDb(ex.ToString, strMessage.ToString)

	'		Return False

	'	Finally

	'		If Not conn Is Nothing Then
	'			conn.Close()
	'			conn.Dispose()
	'		End If

	'	End Try

	'	Return True

	'End Function



	<WebMethod(Description:="Zur Sicherung eines Kundendokuments aus der lokalen Datenbank.")>
	Function AddCustomerDocumentToWOS(ByVal customerWosData As CustomerWOSData) As Boolean

		Dim wosUtility As New WOSDbAccess
		Dim utility As New ClsUtilities

		If (customerWosData Is Nothing) Then
			Throw New Exception("customerWosData is nothing!")
		End If

		Dim allowedToUse = True ' wosUtility.IsUserAllowedForUsingService(customerWosData.CustomerWOSID, WOSModulData.ModulArt.CustomerDocument)
		If Not allowedToUse Then
			Throw New Exception(String.Format("NotAllowed: GetUserID: {0}", customerWosData.CustomerWOSID))
		End If
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strMessage As New StringBuilder()


		Dim conn As SqlConnection = Nothing
		Try

			' Extract parameter data from data row.
			Dim kdNr As Integer? = customerWosData.CustomerNumber
			Dim ZHDNr As Integer? = customerWosData.CresponsibleNumber
			Dim ESNr As Integer? = customerWosData.EmploymentNumber
			Dim ESLohnNr As Integer? = customerWosData.EmploymentLineNumber
			Dim RPNr As Integer? = customerWosData.ReportNumber
			Dim RENr As Integer? = customerWosData.InvoiceNumber
			Dim proposeNr As Integer? = customerWosData.ProposeNumber

			Dim MDName As String = customerWosData.customername
			Dim LogedUser_Guid As String = customerWosData.LogedUserID
			Dim Customer_ID As String = customerWosData.CustomerWOSID

			Dim KD_Name As String = customerWosData.KD_Name
			Dim ZHD_Vorname As String = customerWosData.ZHD_Vorname
			Dim ZHD_Nachname As String = customerWosData.ZHD_Nachname
			Dim KD_Filiale As String = customerWosData.KD_Filiale

			Dim KD_Postfach As String = customerWosData.KD_Postfach
			Dim KD_Strasse As String = customerWosData.KD_Strasse
			Dim KD_PLZ As String = customerWosData.KD_PLZ
			Dim KD_Kanton As String = String.Empty
			Dim KD_Ort As String = customerWosData.KD_Ort
			Dim KD_AGB_Wos As String = customerWosData.KD_AGB_Wos
			Dim ZHDSex As String = customerWosData.ZHDSex
			Dim ZHD_Briefanrede As String = customerWosData.Zhd_BriefAnrede
			Dim DoNotShowContractInWOS As Boolean? = customerWosData.DoNotShowContractInWOS

			Dim KD_EMail As String = customerWosData.KD_Email
			Dim myArray = KD_EMail.Split("#"c)
			KD_EMail = String.Join(",", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim KD_Guid As String = customerWosData.KDTransferedGuid
			Dim ZHD_Guid As String = customerWosData.ZHDTransferedGuid
			Dim Doc_Guid As String = customerWosData.AssignedDocumentGuid
			Dim Doc_Art As String = customerWosData.AssignedDocumentArtName
			Dim Doc_Info As String = customerWosData.AssignedDocumentInfo
			Dim Result As String = String.Empty
			Dim KD_Berater As String = customerWosData.KD_Berater
			Dim ZHD_Berater As String = customerWosData.Zhd_Berater

			Dim KD_Beruf As String = customerWosData.KD_Beruf
			myArray = KD_Beruf.Split("#"c)
			KD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim KD_Branche As String = customerWosData.KD_Branche
			myArray = KD_Branche.Split("#"c)
			KD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim ZHD_Beruf As String = customerWosData.Zhd_Beruf
			myArray = ZHD_Beruf.Split("#"c)
			ZHD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim ZHD_Branche As String = customerWosData.Zhd_Branche
			myArray = ZHD_Branche.Split("#"c)
			ZHD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim ZHD_AGB_WOS As String = customerWosData.ZHD_AGB_Wos
			Dim ZHD_GebDat As DateTime? = customerWosData.ZHD_GebDat
			Dim TransferedUser As String = String.Empty

			Dim US_Nachname As String = customerWosData.UserName
			Dim US_Vorname As String = customerWosData.UserVorname
			Dim US_Telefon As String = customerWosData.UserTelefon
			Dim US_Telefax As String = customerWosData.UserTelefax
			Dim US_eMail As String = customerWosData.UserMail

			Dim MD_Telefon As String = customerWosData.MDTelefon
			Dim MD_DTelefon As String = customerWosData.MD_DTelefon
			Dim MD_Telefax As String = customerWosData.MDTelefax
			Dim MD_eMail As String = customerWosData.MDMail
			If String.IsNullOrWhiteSpace(MD_Telefon) Then MD_Telefon = US_Telefon
			If String.IsNullOrWhiteSpace(MD_DTelefon) Then MD_DTelefon = US_Telefon
			If String.IsNullOrWhiteSpace(MD_Telefax) Then MD_Telefax = US_Telefax
			If String.IsNullOrWhiteSpace(MD_eMail) Then MD_eMail = US_eMail

			Dim KD_Language As String = customerWosData.KD_Language
			Dim ZHD_EMail As String = customerWosData.ZHD_EMail
			Dim DocFilename As String = customerWosData.ScanDocName
			Dim DocScan As Byte() = customerWosData.ScanDoc

			Dim User_Initial As String = customerWosData.UserInitial
			Dim User_Sex As String = customerWosData.UserSex
			Dim User_Filiale As String = customerWosData.UserFiliale
			Dim UserSign As Byte() = customerWosData.UserSign
			Dim UserPicture As Byte() = customerWosData.UserPicture


			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Create New Customer Document For WOS]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(kdNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(ZHDNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(RENr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(proposeNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("LogedUser_Guid", ReplaceMissing(LogedUser_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(Customer_ID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KD_Name", ReplaceMissing(KD_Name, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Vorname", ReplaceMissing(ZHD_Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Nachname", ReplaceMissing(ZHD_Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Filiale", ReplaceMissing(KD_Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Postfach", ReplaceMissing(KD_Postfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Strasse", ReplaceMissing(KD_Strasse, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KD_PLZ", ReplaceMissing(KD_PLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Kanton", ReplaceMissing(KD_Kanton, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KD_Ort", ReplaceMissing(KD_Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_AGB_Wos", ReplaceMissing(KD_AGB_Wos, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHDSex", ReplaceMissing(ZHDSex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Briefanrede", ReplaceMissing(ZHD_Briefanrede, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DoNotShowContractInWOS", ReplaceMissing(DoNotShowContractInWOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_EMail", ReplaceMissing(KD_EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Guid", ReplaceMissing(KD_Guid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Guid", ReplaceMissing(ZHD_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(Result, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Berater", ReplaceMissing(KD_Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Berater", ReplaceMissing(ZHD_Berater, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Beruf", ReplaceMissing(KD_Beruf, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("KD_Branche", ReplaceMissing(KD_Branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Beruf", ReplaceMissing(ZHD_Beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_Branche", ReplaceMissing(ZHD_Branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ZHD_AGB_WOS", ReplaceMissing(ZHD_AGB_WOS, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("TransferedUser", ReplaceMissing(TransferedUser, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Nachname", ReplaceMissing(US_Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Vorname", ReplaceMissing(US_Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Telefon", ReplaceMissing(MD_Telefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Telefax", ReplaceMissing(MD_Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_eMail", ReplaceMissing(MD_eMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("KD_Language", ReplaceMissing(KD_Language, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ZHD_EMail", ReplaceMissing(ZHD_EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(DocFilename, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(DocScan, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Name", ReplaceMissing(MDName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(User_Initial, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Sex", ReplaceMissing(User_Sex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(User_Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(UserPicture, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(UserSign, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SignTransferedDocument", ReplaceMissing(customerWosData.SignTransferedDocument.GetValueOrDefault(False), False)))

			cmd.Parameters.AddRange(listOfParams.ToArray())

			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			cmd.ExecuteNonQuery()

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = If(customerWosData Is Nothing, String.Empty, customerWosData.CustomerWOSID),
																																	.SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddCustomerDocumentToWOS", .MessageContent = msgContent})

			'wosUtility.SaveErrToDb(ex.ToString, strMessage.ToString)

			Return False

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

		Return True

	End Function


#Region "LOG-Entry"

	<WebMethod(Description:="Lists all customer documents which are transfered.")>
	Function ListCustomerDocuments(ByVal customerWosGuid As String, ByVal year As Integer?, ByVal month As Integer?) As CustomerWOSDataDTO()

		Dim listOfSearchResultDTO As List(Of CustomerWOSDataDTO) = Nothing
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[List Customer WOS Document Data]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", utility.ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", utility.ReplaceMissing(month, 0)))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CustomerWOSDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			If (Not reader Is Nothing) Then

				listOfSearchResultDTO = New List(Of CustomerWOSDataDTO)

				While reader.Read

					Dim data As New CustomerWOSDataDTO

					data.ID = utility.SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = utility.SafeGetString(reader, "Customer_ID")

					data.CustomerNumber = utility.SafeGetInteger(reader, "KDNr", Nothing)
					data.CRepesponsibleNumber = utility.SafeGetInteger(reader, "ZHDNr", Nothing)
					data.EmploymentNumber = utility.SafeGetInteger(reader, "ESNr", Nothing)
					data.InvoiceNumber = utility.SafeGetInteger(reader, "RENr", Nothing)
					data.ReportNumber = utility.SafeGetInteger(reader, "RPNr", Nothing)

					data.CustomerName = utility.SafeGetString(reader, "Firma")
					data.ZFirstName = utility.SafeGetString(reader, "ZFirstName")
					data.ZLastName = utility.SafeGetString(reader, "ZLastName")

					data.CustomerGuid = utility.SafeGetString(reader, "KD_Guid")
					data.ZHDGuid = utility.SafeGetString(reader, "ZHD_Guid")
					data.DocGuid = utility.SafeGetString(reader, "Doc_Guid")

					data.DocumentArt = utility.SafeGetString(reader, "Doc_Art")
					data.DocumentInfo = utility.SafeGetString(reader, "Doc_Info")
					data.TransferedOn = utility.SafeGetDateTime(reader, "Transfered_On", Nothing)
					data.TransferedUser = utility.SafeGetString(reader, "Transfered_User")
					data.GetResult = utility.SafeGetInteger(reader, "GetResult", Nothing)
					data.GetOn = utility.SafeGetDateTime(reader, "Get_On", Nothing)
					data.LastNotification = utility.SafeGetDateTime(reader, "LastNotification", Nothing)


					listOfSearchResultDTO.Add(data)

				End While
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerWosGuid, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "ListCustomerDocuments", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		' Return search data as an array.
		Return listOfSearchResultDTO.ToArray()
	End Function


	<WebMethod(Description:="Get assigned scan data")>
	Function GetAssignedCustomerDocumentsData(ByVal customerID As String, ByVal recID As Integer) As CustomerWOSDataDTO

		Dim listOfSearchResultDTO As CustomerWOSDataDTO = Nothing
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get Assigned Customer WOS Document Data]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("RecID", recID))

			' Open connection to database.
			conn.Open()

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New CustomerWOSDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			If (Not reader Is Nothing AndAlso reader.Read()) Then

				listOfSearchResultDTO.ID = utility.SafeGetInteger(reader, "ID", 0)
				listOfSearchResultDTO.Customer_ID = utility.SafeGetString(reader, "Customer_ID")

				listOfSearchResultDTO.CustomerNumber = utility.SafeGetInteger(reader, "KDNr", Nothing)
				listOfSearchResultDTO.CRepesponsibleNumber = utility.SafeGetInteger(reader, "ZHDNr", Nothing)
				listOfSearchResultDTO.EmploymentNumber = utility.SafeGetInteger(reader, "ESNr", Nothing)
				listOfSearchResultDTO.InvoiceNumber = utility.SafeGetInteger(reader, "RENr", Nothing)
				listOfSearchResultDTO.ReportNumber = utility.SafeGetInteger(reader, "RPNr", Nothing)

				listOfSearchResultDTO.CustomerName = utility.SafeGetString(reader, "Firma")
				listOfSearchResultDTO.ZFirstName = utility.SafeGetString(reader, "ZFirstName")
				listOfSearchResultDTO.ZLastName = utility.SafeGetString(reader, "ZLastName")

				listOfSearchResultDTO.CustomerGuid = utility.SafeGetString(reader, "KD_Guid")
				listOfSearchResultDTO.ZHDGuid = utility.SafeGetString(reader, "ZHD_Guid")
				listOfSearchResultDTO.DocGuid = utility.SafeGetString(reader, "Doc_Guid")

				listOfSearchResultDTO.DocumentArt = utility.SafeGetString(reader, "Doc_Art")
				listOfSearchResultDTO.DocumentInfo = utility.SafeGetString(reader, "Doc_Info")
				listOfSearchResultDTO.TransferedOn = utility.SafeGetDateTime(reader, "Transfered_On", Nothing)
				listOfSearchResultDTO.TransferedUser = utility.SafeGetString(reader, "Transfered_User")
				listOfSearchResultDTO.GetResult = utility.SafeGetInteger(reader, "GetResult", Nothing)
				listOfSearchResultDTO.GetOn = utility.SafeGetDateTime(reader, "Get_On", Nothing)
				listOfSearchResultDTO.LastNotification = utility.SafeGetDateTime(reader, "LastNotification", Nothing)
				listOfSearchResultDTO.ScanContent = utility.SafeGetByteArray(reader, "ScanContent")

			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetAssignedCustomerDocumentsData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		' Return search data as an array.
		Return listOfSearchResultDTO
	End Function


#End Region


#Region "Funktionen zum Löschen der Datensätze vom Kundendoc..."

	<WebMethod(Description:="Löscht das ausgewählte Kunden-Document.")>
	Function DeleteAssignedCustomerDocumentWithGuid(ByVal customerWosData As CustomerWOSData) As Boolean

		Dim success As Boolean = True
		Dim strMessage As New StringBuilder()
		Dim sql As String = "[Delete Assigned Customer Document With DocGuid]"
		m_customerID = String.Empty

		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		conn = New SqlConnection(connString)

		Try
			conn.Open()

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@CustomerID", m_customerID)
			param = cmd.Parameters.AddWithValue("@WOSGuid", customerWosData.CustomerWOSID)
			param = cmd.Parameters.AddWithValue("@DocumentArt", customerWosData.AssignedDocumentArtName)
			param = cmd.Parameters.AddWithValue("@DocumentGuid", customerWosData.AssignedDocumentGuid)

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			m_Logger.LogInfo(String.Format("SQL: {0} | CustomerID: {1} | WOSGuid: {2} | DocumentArt: {3} | DocumentGuid: {4}", sql, m_customerID, customerWosData.CustomerWOSID, customerWosData.AssignedDocumentArtName, customerWosData.AssignedDocumentGuid))
			cmd.ExecuteNonQuery()


		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, sql, strMessage.ToString, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeleteAssignedCustomerDocumentWithGuid", .MessageContent = msgContent})

			success = False

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try


		Return success

	End Function

	<WebMethod(Description:="Löscht das ausgewählte Kunden-Document.")>
	Function DeleteSelectedKDDocWithArt(ByVal strUserID As String,
																ByVal strKD_Guid As String,
																ByVal iKDNr As Integer,
																ByVal strDoc_Art As String,
																ByVal strDoc_Info As String) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKDDocWithArt"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDDoc With DocInfo]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@Doc_Art", strDoc_Art)
			param = cmd.Parameters.AddWithValue("@Doc_Info", strDoc_Info)
			cmd.ExecuteNonQuery()           ' Datensatz löschen...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_DeleteSelectedKDDocWithArt: {0}", ex.ToString)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-Einsatz).")>
	Function DeleteSelectedKDESDoc(ByVal strUserID As String,
																ByVal strKD_Guid As String,
																ByVal iKDNr As Integer,
																ByVal iESNr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKDESDoc"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDESDoc]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
			cmd.ExecuteNonQuery()           ' Datensatz löschen...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_DeleteSelectedKDESDoc: {0}", ex.ToString)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-RP).")>
	Function DeleteSelectedKDRPDoc(ByVal strUserID As String,
																ByVal strKD_Guid As String,
																ByVal iKDNr As Integer,
																ByVal iRPNr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKDRPDoc"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDRPDoc]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
			cmd.ExecuteNonQuery()           ' Datensatz löschen...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_DeleteSelectedKDRPDoc: {0}", ex.ToString)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-RE).")>
	Function DeleteSelectedKDREDoc(ByVal strUserID As String,
																ByVal strKD_Guid As String,
																ByVal iKDNr As Integer,
																ByVal iRENr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKDREDoc"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDREDoc]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@RENr", iRENr)
			cmd.ExecuteNonQuery()           ' Datensatz löschen...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_DeleteSelectedKDREDoc: {0}", ex.ToString)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-Alle).")>
	Function DeleteSelectedAllKDDoc(ByVal strUserID As String,
															ByVal strKD_Guid As String,
															ByVal iKDNr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedAllKDDoc"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected AllKDDoc]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			cmd.ExecuteNonQuery()           ' Datensatz löschen...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_DeleteSelectedAllKDDoc: {0}", ex.ToString)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht den ausgewählten Kunden (Kunden).")>
	Function DeleteSelectedKundenRec(ByVal strUserID As String,
														ByVal strKD_Guid As String,
														ByVal strZHD_Guid As String) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KD-Rec From ALL KD_Db]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

			cmd.ExecuteNonQuery()           ' Datensatz löschen...

		Catch ex As Exception
			strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Löscht den ausgewählten Kunden_ZHD (Kunden_ZHD).")>
	Function DeleteSelectedZHDRec(ByVal strUserID As String,
														ByVal strKD_Guid As String,
														ByVal strZHD_Guid As String) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedZHDRec"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDZHD-Rec From Kunden ZHD]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

			cmd.ExecuteNonQuery()           ' Datensatz löschen...

		Catch ex As Exception
			strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedZHDRec", ex.Message)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Ändert das ALTE KD_Guid durch NEUEN KD_Guid in allen Tabellen.")>
	Function ChangeKDGuidWithNewGuid(ByVal strUserID As String,
														ByVal strOldKD_Guid As String,
														ByVal strNewKD_Guid As String) As String
		Dim strValue As String = "Erfolgreich: ChangeKDGuidWithNewGuid"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Change Selected KDGuid With NewGuid]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@OldKD_Guid", strOldKD_Guid)
			param = cmd.Parameters.AddWithValue("@NewKD_Guid", strNewKD_Guid)
			cmd.ExecuteNonQuery()           ' Datensatz ändern...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_ChangeKDGuidWithNewGuid: {0}", ex.ToString)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	<WebMethod(Description:="Ändert das ALTE KDZHD_Guid durch NEUEN KDZHD_Guid in allen Tabellen.")>
	Function ChangeKDZHDGuidWithNewGuid(ByVal strUserID As String,
																		ByVal strKD_Guid As String,
																		ByVal strOldKDZHD_Guid As String,
																		ByVal strNewKDZHD_Guid As String) As String
		Dim strValue As String = "Erfolgreich: ChangeKDZHDGuidWithNewGuid"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Change Selected KDZHDGuid With NewGuid]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@OldKD_Guid", strOldKDZHD_Guid)
			param = cmd.Parameters.AddWithValue("@NewKD_Guid", strNewKDZHD_Guid)
			cmd.ExecuteNonQuery()           ' Datensatz ändern...


		Catch ex As Exception
			strValue = String.Format("Fehler_Main_ChangeKDZHDGuidWithNewGuid: {0}", ex.ToString)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function



	''' <summary>
	''' löscht den ausgewählten Kundendatensatz NUR aus der Kunden
	''' </summary>
	''' <param name="strUserID"></param>
	''' <param name="strKD_Guid"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function DeleteSelectedKundenRec(ByVal strUserID As String,
																	 ByVal strKD_Guid As String) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KD-Rec From Kunden]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
			'      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

			cmd.ExecuteNonQuery()           ' Datensatz löschen...

		Catch ex As Exception
			strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	Function DeleteSelectedKundenRec(ByVal strUserID As String,
																 ByVal iKDNr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KD-Rec From Kunden With KDNr]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			'      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

			cmd.ExecuteNonQuery()           ' Datensatz löschen...

		Catch ex As Exception
			strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function

	Function DeleteSelectedZHDRec(ByVal strUserID As String,
														ByVal iKDNr As Integer,
														ByVal iZHDNr As Integer) As String
		Dim strValue As String = "Erfolgreich: DeleteSelectedZHDRec"
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim strSQL As String = "[Delete Selected KDZHD-Rec From Kunden ZHD With ZHDNr]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
			param = cmd.Parameters.AddWithValue("@ZHDNr", iZHDNr)

			cmd.ExecuteNonQuery()           ' Datensatz löschen...

		Catch ex As Exception
			strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedZHDRec", ex.Message)
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return strValue
	End Function


#End Region


#Region "Hilf-Funktionen..."


	''' <summary>
	''' Splits a two value string.
	''' </summary>
	''' <param name="str">The string.</param>
	''' <param name="delimeter">The delimeter.</param>
	''' <returns>Tuple with values.</returns>
	Private Function SplitTwoValueString(ByVal str As String, ByVal delimeter As String) As Tuple(Of Integer?, Integer?)

		Dim value1 As Integer? = Nothing
		Dim value2 As Integer? = Nothing

		If Not String.IsNullOrEmpty(str) Then

			Dim tokens As String() = str.Trim().Split(delimeter)

			If tokens.Count = 2 Then
				value1 = Integer.Parse(tokens(0))
				value2 = Integer.Parse(tokens(1))
			End If

		End If

		Return New Tuple(Of Integer?, Integer?)(value1, value2)

	End Function

	''' <summary>
	''' Gets a value from a data row.
	''' </summary>
	''' <param name="dataRow">The data row.</param>
	''' <param name="column">The column.</param>
	''' <returns>The value or nothing.</returns>
	Private Function GetValueFromdataRow(ByVal dataRow As DataRow, ByVal column As String) As Object

		If Not dataRow.IsNull(column) Then
			Dim value As Object = dataRow(column)
			Return value
		End If

		Return Nothing
	End Function

	''' <summary>
	''' Parsets a nullable integer.
	''' </summary>
	''' <param name="str">The string.</param>
	''' <returns>Nullable integer value or nothing.</returns>
	Private Function ParseNullableInt(ByVal str As String) As Integer?

		If String.IsNullOrEmpty(str) Then
			Return Nothing
		End If

		Return Integer.Parse(str)

	End Function

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
		If (obj Is Nothing) Then
			Return replacementObject
		Else
			Return obj
		End If
	End Function


	Function GetDbValue(ByVal myVale As String) As String

		If String.IsNullOrEmpty(myVale) Then
			Return String.Empty
		Else
			Return myVale
		End If

	End Function

	Private Function ArrayToString(ByVal ar() As String, Optional ByVal Seperator As String = ", ") As String
		Dim AtS As New System.Text.StringBuilder
		For i As Integer = 0 To ar.Length - 1
			AtS.Append(ar(i))
			If i <> ar.Length - 1 Then
				AtS.Append(Seperator)
			End If
		Next

		Return AtS.ToString
	End Function

	Public Function ConvertDataReaderToDataSet(ByVal reader As SqlDataReader) As DataSet
		Dim dataSet As DataSet = New DataSet
		Dim schemaTable As DataTable = reader.GetSchemaTable()
		Dim dataTable As DataTable = New DataTable
		Dim intCounter As Integer
		For intCounter = 0 To schemaTable.Rows.Count - 1
			Dim dataRow As DataRow = schemaTable.Rows(intCounter)
			Dim columnName As String = CType(dataRow("ColumnName"), String)
			Dim column As DataColumn = New DataColumn(columnName, _
					 CType(dataRow("DataType"), Type))
			dataTable.Columns.Add(column)
		Next
		dataSet.Tables.Add(dataTable)
		While reader.Read()
			Dim dataRow As DataRow = dataTable.NewRow()
			For intCounter = 0 To reader.FieldCount - 1
				dataRow(intCounter) = reader.GetValue(intCounter)
			Next
			dataTable.Rows.Add(dataRow)
		End While

		Return dataSet
	End Function


#End Region


End Class