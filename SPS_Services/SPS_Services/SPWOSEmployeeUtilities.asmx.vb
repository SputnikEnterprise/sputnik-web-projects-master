Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.WOSUtilities
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.WOSUtilities.WOSDbAccess
Imports wsSPS_Services.WOSInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects

Imports wsSPS_Services.Logging


' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPWOSEmployeeUtilities.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPWOSEmployeeUtilities
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPWOSEmployeeUtilities"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_WOSData As WOSDatabaseAccess
	Private m_NewWOS As WOSDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_WOSData = New WOSDatabaseAccess(My.Settings.ConnStr_spContract, Language.German)
		m_NewWOS = New WOSDatabaseAccess(My.Settings.ConnStr_New_spContract, Language.German)

	End Sub

	<WebMethod(Description:="List available employee data")>
	Function LoadAvailableEmployee(ByVal customerID As String, ByVal customerWosID As String, ByVal qualifications As String, ByVal location As String, ByVal canton As String, ByVal brunchOffice As String) As AvailableEmployeeDTO()
		Dim result As List(Of AvailableEmployeeDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of AvailableEmployeeDTO)
			result = m_NewWOS.LoadWOSAvailableEmployeeData(customerID, customerWosID, qualifications, location, canton, brunchOffice)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAvailableCandidates", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List assigned available employee data")>
	Function LoadAssignedAvailableEmployeeData(ByVal customerID As String, ByVal customerWosID As String, ByVal employeeNumber As Integer) As AvailableEmployeeDTO
		Dim result As AvailableEmployeeDTO = Nothing
		m_customerID = customerID

		Try
			result = New AvailableEmployeeDTO
			result = m_NewWOS.LoadAssignedAvailableEmployeeData(customerID, customerWosID, employeeNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List assigned available employee application data")>
	Function LoadAssignedAvailableEmployeeApplicationData(ByVal customerID As String, ByVal customerWosID As String, ByVal employeeNumber As Integer) As AvailableEmployeeApplicationFields
		Dim result As AvailableEmployeeApplicationFields = Nothing
		m_customerID = customerID

		Try
			result = New AvailableEmployeeApplicationFields
			result = m_NewWOS.LoadAssignedAvailableEmployeeApplicationData(customerID, customerWosID, employeeNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeApplicationData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Zur Sicherung eines Kandidaten.")>
	Function AddAvailableEmployeeData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult
		Dim result As WebServiceResult = Nothing
		m_customerID = customerID

		Try
			result = New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}
			result = m_NewWOS.AddWOSAvailableCandidatesData(customerID, wosID, employeeData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAvailableEmployeeData", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function

	<WebMethod(Description:="Zur Löschung eines Kandidaten.")>
	Function RemoveAvailableEmployeeData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult
		Dim result As WebServiceResult = Nothing
		m_customerID = customerID

		Try
			result = New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}
			result = m_NewWOS.RemoveWOSAvailableCandidatesData(customerID, wosID, employeeData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "RemoveAvailableEmployeeData", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function


	''ByVal strMA_Guid As String, ByVal iMANr As Integer, ByVal employeeData As DataSet, ByVal aCustomerData As String()) As String
	'Dim strValue As String = String.Empty
	'Dim _clsSystem As New ClsMain_Net
	'Dim strUserID As String = aCustomerData(0)
	'Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "MA_Guid")
	'If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)

	'' Datensatz löschen
	'strValue = DeleteSelectedMAOnline(strUserID, strMA_Guid, iMANr)

	'Dim connString As String = My.Settings.ConnStr_MA
	'Dim strSQL As String = "Insert Into Kandidaten_Online ("
	'strSQL &= "MANr, Customer_ID, Customer_Name, "
	'strSQL &= "Customer_Strasse, Customer_Ort, Customer_Telefon, Customer_eMail, "
	'strSQL &= "Berater, MA_Filiale, MA_Kanton, MA_Ort, MA_Vorname, MA_Nachname, "
	'strSQL &= "MA_Kontakt, MA_State1, MA_State2, MA_Beruf, "
	'strSQL &= "JobProzent, MAGebDat, MASex, MAZivil, "
	'strSQL &= "MAFSchein, MAAuto, MANationality, Bewillig, "
	'strSQL &= "BriefAnrede, MA_Res1, MA_Res2, MA_Res3, MA_Res4, "
	'strSQL &= "MA_MSprache, MA_SSprache, MA_Eigenschaft, "

	'strSQL &= "Transfered_User, Transfered_On, Transfered_Guid, "
	'strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail"
	'strSQL &= ") Values ("

	'strSQL &= "@MANr, @Customer_ID, @Customer_Name, "
	'strSQL &= "@Customer_Strasse, @Customer_Ort, @Customer_Telefon, @Customer_eMail, "
	'strSQL &= "@Berater, @MA_Filiale, @MA_Kanton, @MA_Ort, @MA_Vorname, @MA_Nachname, "
	'strSQL &= "@MAKontakt, @MAState1, @MAState2, @MABeruf, "
	'strSQL &= "@JobProzent, @MAGebDat, @MASex, @MAZivil, "
	'strSQL &= "@MAFSchein, @MAAuto, @MANationality, @Bewillig, "
	'strSQL &= "@BriefAnrede, @MA_Res1, @MA_Res2, @MA_Res3, @MA_Res4, "
	'strSQL &= "@MA_MSprache, @MA_SSprache, @MA_Eigenschaft, "

	'strSQL &= "@Transfered_User, @Transfered_On, @Transfered_Guid, "
	'strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail"

	'strSQL &= ")"

	'Dim Conn As SqlConnection = New SqlConnection(connString)
	'Conn.Open()

	'Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'cmd.CommandType = Data.CommandType.Text
	'Dim param As System.Data.SqlClient.SqlParameter
	'' Daten vom Client
	'Dim dt As DataTable = MyDs.Tables("Kandidaten_Online")

	'param = cmd.Parameters.AddWithValue("@MANr", iMANr) ' Val(GetDbValue(CStr(dt.Rows(0)("MANr").ToString))))
	'param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
	'param = cmd.Parameters.AddWithValue("@Customer_Name", aCustomerData(1))

	'param = cmd.Parameters.AddWithValue("@Customer_Strasse", aCustomerData(2))
	'param = cmd.Parameters.AddWithValue("@Customer_Ort", aCustomerData(3))
	'param = cmd.Parameters.AddWithValue("@Customer_Telefon", aCustomerData(4))
	'param = cmd.Parameters.AddWithValue("@Customer_eMail", aCustomerData(6))

	'param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(0)("Berater").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Filiale", GetDbValue(dt.Rows(0)("MA_Filiale").ToString))

	'param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(0)("MA_Kanton").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(0)("MA_Ort").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(0)("MA_Vorname").ToString))
	'param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(0)("MA_Nachname").ToString))

	'param = cmd.Parameters.AddWithValue("@MAKontakt", GetDbValue(dt.Rows(0)("MA_Kontakt").ToString))
	'param = cmd.Parameters.AddWithValue("@MAState1", GetDbValue(dt.Rows(0)("MA_State1").ToString))
	'param = cmd.Parameters.AddWithValue("@MAState2", GetDbValue(dt.Rows(0)("MA_State2").ToString))
	'param = cmd.Parameters.AddWithValue("@MABeruf", GetDbValue(dt.Rows(0)("MA_Beruf").ToString))
	'param = cmd.Parameters.AddWithValue("@JobProzent", GetDbValue(CStr(dt.Rows(0)("JobProzent").ToString)))
	'param = cmd.Parameters.AddWithValue("@MAGebDat", GetDbValue(CStr(dt.Rows(0)("MA_GebDat").ToString)))
	'param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(0)("MASex").ToString)))
	'param = cmd.Parameters.AddWithValue("@MAZivil", GetDbValue(CStr(dt.Rows(0)("Zivilstand").ToString)))

	'param = cmd.Parameters.AddWithValue("@MAFSchein", GetDbValue(CStr(dt.Rows(0)("MAFSchein").ToString)))
	'param = cmd.Parameters.AddWithValue("@MAAuto", GetDbValue(CStr(dt.Rows(0)("MAAuto").ToString)))
	'param = cmd.Parameters.AddWithValue("@MANationality", GetDbValue(CStr(dt.Rows(0)("MA_Nationality").ToString)))
	'param = cmd.Parameters.AddWithValue("@Bewillig", GetDbValue(CStr(dt.Rows(0)("Bewillig").ToString)))
	'param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(0)("BriefAnrede").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Res1", GetDbValue(CStr(dt.Rows(0)("MA_Res1").ToString)))

	'param = cmd.Parameters.AddWithValue("@MA_Res2", GetDbValue(CStr(dt.Rows(0)("MA_Res2").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Res3", GetDbValue(CStr(dt.Rows(0)("MA_Res3").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Res4", GetDbValue(CStr(dt.Rows(0)("MA_Res4").ToString)))

	'param = cmd.Parameters.AddWithValue("@MA_MSprache", GetDbValue(CStr(dt.Rows(0)("MA_MSprache").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_SSprache", GetDbValue(CStr(dt.Rows(0)("MA_SSprache").ToString)))
	'param = cmd.Parameters.AddWithValue("@MA_Eigenschaft", GetDbValue(CStr(dt.Rows(0)("MA_Eigenschaft").ToString)))

	'param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(0)("Transfered_User").ToString)))
	'param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)
	'param = cmd.Parameters.AddWithValue("@Transfered_Guid", GetDbValue(CStr(dt.Rows(0)("MA_Guid").ToString)))

	'param = cmd.Parameters.AddWithValue("@User_Nachname", aCustomerData(7))
	'param = cmd.Parameters.AddWithValue("@User_Vorname", aCustomerData(8))
	'param = cmd.Parameters.AddWithValue("@User_Telefon", aCustomerData(9))
	'param = cmd.Parameters.AddWithValue("@User_Telefax", aCustomerData(10))
	'param = cmd.Parameters.AddWithValue("@User_eMail", aCustomerData(11))
	''    Return strValue

	'Try
	'	cmd.ExecuteNonQuery()           ' Kandidat einfügen...
	'	strValue = "SaveMyMA: Ihre Daten wurden erfolgreich importiert..."

	'	Try
	'		strValue = RegisterToKandidatDb(strMA_Guid, MyDs, "Kandidaten_Online", aCustomerData(0))

	'	Catch ex As Exception
	'		strValue = String.Format("Fehler ({0}): {1}", "RegisterToKandidatDb_0", ex.Message)
	'	End Try


	'Catch ex As Exception
	'	strValue = String.Format("Fehler ({0}): {1}", "SaveMyMA", ex.Message)
	'Finally

	'	If Not Conn Is Nothing Then
	'		Conn.Close()
	'		Conn.Dispose()
	'	End If

	'End Try

	'Return strValue

	<WebMethod(Description:="Zur Sicherung eines Kundendokuments aus der lokalen Datenbank.")>
	Function AddAssignedEmployeeWOSDocument(ByVal customerID As String, ByVal employeeWosData As EmployeeWOSData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			If employeeWosData Is Nothing Then Return False
			Dim ownerData = m_NewWOS.LoadAssignedWOSOwnerMasterData(employeeWosData.EmployeeWOSID, WOSModulData.ModulArt.EmployeeDocument, False)
			If ownerData Is Nothing Then Throw New Exception(String.Format("{0}: ownder data could not be founded!", employeeWosData.EmployeeWOSID))

			If Not String.IsNullOrWhiteSpace(employeeWosData.EmployeeWOSID) Then
				result = m_NewWOS.AddWOSEmployeeDocumentData(m_customerID, employeeWosData.EmployeeWOSID, employeeWosData)
			End If

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAssignedCustomerWOSDocument", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


	<WebMethod(Description:="Zur Sicherung eines Kandidatendokuments aus der lokalen Datenbank.")>
	Function AddEmployeeDocumentToWOS(ByVal wosData As EmployeeWOSData) As Boolean

		Dim wosUtility As New WOSDbAccess
		Dim utility As New ClsUtilities
		Dim strMessage As New StringBuilder()

		If wosData Is Nothing Then
			m_customerID = String.Empty
		Else
			m_customerID = wosData.EmployeeWOSID
		End If

		If (wosData Is Nothing) Then
			Throw New Exception("employeeWosData is nothing!")
		End If

		Dim allowedToUse = True ' wosUtility.IsUserAllowedForUsingService(wosData.EmployeeWOSID, WOSModulData.ModulArt.EmployeeDocument)
		If Not allowedToUse Then
			Throw New Exception(String.Format("NotAllowed: GetUserID: {0}", wosData.EmployeeWOSID))
		End If
		Dim connString As String = My.Settings.ConnStr_spContract


		Dim conn As SqlConnection = Nothing
		Try

			' Extract parameter data from data row.
			Dim maNr As Integer? = wosData.EmployeeNumber
			Dim ESNr As Integer? = wosData.EmploymentNumber
			Dim ESLohnNr As Integer? = wosData.EmploymentLineNumber
			Dim PayrollNr As Integer? = wosData.PayrollNumber
			Dim RPNr As Integer? = wosData.ReportNumber
			Dim RPLNr As Integer? = wosData.ReportLineNumber
			Dim RPDocNr As Integer? = wosData.ReportDocNumber

			Dim LogedUser_Guid As String = wosData.LogedUserID
			Dim Customer_ID As String = wosData.EmployeeWOSID
			Dim wosGuid As String = wosData.EmployeeWOSID

			Dim MA_Vorname As String = wosData.MA_Vorname
			Dim MA_Nachname As String = wosData.MA_Nachname
			Dim MA_Filiale As String = wosData.MA_Filiale

			Dim MA_Postfach As String = wosData.MA_Postfach
			Dim MA_Strasse As String = wosData.MA_Strasse
			Dim MA_PLZ As String = wosData.MA_PLZ
			Dim MA_Ort As String = wosData.MA_Ort
			Dim MA_AGB_Wos As String = wosData.MA_AGB_Wos
			Dim MASex As String = wosData.MA_Gender
			Dim MA_Briefanrede As String = wosData.MA_BriefAnrede

			Dim MA_EMail As String = wosData.MA_Email
			Dim myArray = MA_EMail.Split("#"c)
			MA_EMail = String.Join(",", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim MA_Guid As String = wosData.MATransferedGuid
			Dim Doc_Guid As String = wosData.AssignedDocumentGuid
			Dim Doc_Art As String = wosData.AssignedDocumentArtName
			Dim Doc_Info As String = wosData.AssignedDocumentInfo
			Dim Result As String = String.Empty
			Dim MA_Berater As String = wosData.MA_Berater
			Dim MA_Zivil As String = wosData.MA_Zivil
			Dim MA_Nationality As String = wosData.MA_Nationality

			Dim MA_Beruf As String = wosData.MA_Beruf
			myArray = MA_Beruf.Split("#"c)
			MA_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim MA_Branche As String = wosData.MA_Branche
			myArray = MA_Branche.Split("#"c)
			MA_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

			Dim MA_GebDat As DateTime? = wosData.MA_GebDat
			Dim TransferedUser As String = String.Empty

			Dim US_Nachname As String = wosData.UserName
			Dim US_Vorname As String = wosData.UserVorname
			Dim US_Telefon As String = wosData.UserTelefon
			Dim US_Telefax As String = wosData.UserTelefax
			Dim US_eMail As String = wosData.UserMail

			Dim MD_Telefon As String = wosData.MDTelefon
			Dim MD_DTelefon As String = wosData.MD_DTelefon
			Dim MD_Telefax As String = wosData.MDTelefax
			Dim MD_eMail As String = wosData.MDMail

			If String.IsNullOrWhiteSpace(MD_Telefon) Then MD_Telefon = US_Telefon
			If String.IsNullOrWhiteSpace(MD_DTelefon) Then MD_DTelefon = US_Telefon
			If String.IsNullOrWhiteSpace(MD_Telefax) Then MD_Telefax = US_Telefax
			If String.IsNullOrWhiteSpace(MD_eMail) Then MD_eMail = US_eMail


			Dim MA_Language As String = wosData.MA_Language

			Dim MA_FSchein As String = wosData.MA_FSchein
			Dim MA_Auto As String = wosData.MA_Auto
			Dim MA_Kontakt As String = wosData.MA_Kontakt
			Dim MA_State1 As String = wosData.MA_State1
			Dim MA_State2 As String = wosData.MA_State2
			Dim MA_Eigenschaft As String = wosData.MA_Eigenschaft
			Dim MA_SSprache As String = wosData.MA_SSprache
			Dim MA_MSprache As String = wosData.MA_MSprache
			Dim AHV_Nr As String = wosData.AHV_Nr
			Dim MA_Canton As String = wosData.MA_Canton


			Dim DocFilename As String = wosData.ScanDocName
			Dim DocScan As Byte() = wosData.ScanDoc

			Dim User_Initial As String = wosData.UserInitial
			Dim User_Sex As String = wosData.UserSex
			Dim User_Filiale As String = wosData.UserFiliale
			Dim UserSign As Byte() = wosData.UserSign
			Dim UserPicture As Byte() = wosData.UserPicture


			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Create New Employee Document For WOS]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(Customer_ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", ReplaceMissing(wosGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(maNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(ESNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(ESLohnNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(PayrollNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(RPNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(RPLNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RPDocNr", ReplaceMissing(RPDocNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("LogedUser_Guid", ReplaceMissing(LogedUser_Guid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Berater", ReplaceMissing(MA_Berater, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("MA_Vorname", ReplaceMissing(MA_Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Nachname", ReplaceMissing(MA_Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Filiale", ReplaceMissing(MA_Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Postfach", ReplaceMissing(MA_Postfach, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Strasse", ReplaceMissing(MA_Strasse, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_PLZ", ReplaceMissing(MA_PLZ, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Ort", ReplaceMissing(MA_Ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MASex", ReplaceMissing(MASex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MAZivil", ReplaceMissing(MA_Zivil, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Briefanrede", ReplaceMissing(MA_Briefanrede, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AGB_WOS", ReplaceMissing(MA_AGB_Wos, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Beruf", ReplaceMissing(MA_Beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Branche", ReplaceMissing(MA_Branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_EMail", ReplaceMissing(MA_EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_GebDat", ReplaceMissing(MA_GebDat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Language", ReplaceMissing(MA_Language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Nationality", ReplaceMissing(MA_Nationality, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Transfered_User", ReplaceMissing(TransferedUser, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Owner_Guid", ReplaceMissing(MA_Guid, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("MA_FSchein", ReplaceMissing(MA_FSchein, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Auto", ReplaceMissing(MA_Auto, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Kontakt", ReplaceMissing(MA_Kontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_State1", ReplaceMissing(MA_State1, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_State2", ReplaceMissing(MA_State2, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Eigenschaft", ReplaceMissing(MA_Eigenschaft, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_SSprache", ReplaceMissing(MA_SSprache, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_MSprache", ReplaceMissing(MA_MSprache, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("AHV_Nr", ReplaceMissing(AHV_Nr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Canton", ReplaceMissing(MA_Canton, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(Result, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("US_Nachname", ReplaceMissing(US_Nachname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Vorname", ReplaceMissing(US_Vorname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Telefon", ReplaceMissing(MD_Telefon, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_Telefax", ReplaceMissing(MD_Telefax, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("US_eMail", ReplaceMissing(MD_eMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(DocFilename, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(DocScan, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(User_Initial, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Sex", ReplaceMissing(User_Sex, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(User_Filiale, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(UserPicture, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(UserSign, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SignTransferedDocument", ReplaceMissing(wosData.SignTransferedDocument.GetValueOrDefault(False), False)))

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
			Dim success As Boolean = True
			If Doc_Art.ToLower.Contains("zwischenverdienstformular") OrElse Doc_Art.ToLower.Contains("arbeitgeber") Then
				success = success AndAlso DeleteAssignedEmployeeDocument(Customer_ID, maNr, MA_Guid, String.Empty, Doc_Art, Doc_Info)
			End If

			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = If(wosData Is Nothing, String.Empty, wosData.EmployeeWOSID), .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddEmployeeDocumentToWOS", .MessageContent = String.Format("{0} was deleted!", Doc_Art)})
			If success Then cmd.ExecuteNonQuery()


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = If(wosData Is Nothing, String.Empty, wosData.EmployeeWOSID), .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddEmployeeDocumentToWOS", .MessageContent = msgContent})


			Return False

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

		Return True

	End Function

	<WebMethod(Description:="Löscht das ausgewählte Modul-Document (MA-Doc).")>
	Function DeleteAssignedEmployeeDocument(ByVal strUserID As String,
																					ByVal iMANr As Integer,
																					ByVal employee_Guid As String,
																					ByVal Doc_Guid As String,
																					ByVal Doc_Art As String,
																					ByVal Doc_Info As String) As Boolean
		Dim wosUtility As New WOSDbAccess
		Dim strMessage As New StringBuilder()

		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		Dim sql As String = "[Delete Selected Employee Document]"

		Try
			conn = New SqlConnection(connString)
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(strUserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MA_Guid", ReplaceMissing(employee_Guid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(iMANr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, String.Empty)))

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


			m_Logger.LogInfo(String.Format("SQL: {0} | UserID: {1} | employee_Guid: {2} | MANr: {3} | Doc_Guid: {4} | Doc_Art: {5} | Doc_Info: {6}", sql, strUserID, employee_Guid, iMANr, Doc_Guid, Doc_Art, Doc_Info))
			cmd.ExecuteNonQuery()
			Return True


		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, sql, strMessage.ToString, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = strUserID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeleteAssignedEmployeeDocument", .MessageContent = msgContent})


			Return False

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

	End Function


#Region "LOG-Entry"

	<WebMethod(Description:="Lists all employee documents which are transfered.")> _
	Function ListEmployeeDocuments(ByVal customerWosGuid As String, ByVal year As Integer?, ByVal month As Integer?) As EmployeeWOSDataDTO()

		Dim listOfSearchResultDTO As List(Of EmployeeWOSDataDTO) = Nothing
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[List Employee WOS Document Data]", conn)
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
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}", _
																				cmd.Parameters(i).ParameterName, _
																				cmd.Parameters(i).DbType, _
																				cmd.Parameters(i).Size, _
																				cmd.Parameters(i).Value, _
																				ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of EmployeeWOSDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			If (Not reader Is Nothing) Then

				listOfSearchResultDTO = New List(Of EmployeeWOSDataDTO)

				While reader.Read

					Dim data As New EmployeeWOSDataDTO

					data.ID = utility.SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = utility.SafeGetString(reader, "Customer_ID")

					data.EmployeeNumber = utility.SafeGetInteger(reader, "MANr", Nothing)
					data.EmploymentNumber = utility.SafeGetInteger(reader, "ESNr", Nothing)
					data.PayrollNumber = utility.SafeGetInteger(reader, "LONr", Nothing)
					data.ReportNumber = utility.SafeGetInteger(reader, "RPNr", Nothing)

					data.EmployeeFirstName = utility.SafeGetString(reader, "MA_Vorname")
					data.EmployeeLastName = utility.SafeGetString(reader, "MA_Nachname")

					data.EmployeeGuid = utility.SafeGetString(reader, "Owner_Guid")
					data.DocGuid = utility.SafeGetString(reader, "Doc_Guid")

					data.DocumentArt = utility.SafeGetString(reader, "Doc_Art")
					data.DocumentInfo = utility.SafeGetString(reader, "Doc_Info")
					data.TransferedOn = utility.SafeGetDateTime(reader, "Transfered_On", Nothing)
					data.TransferedUser = utility.SafeGetString(reader, "Transfered_User")
					data.LastNotification = utility.SafeGetDateTime(reader, "LastNotification", Nothing)


					listOfSearchResultDTO.Add(data)

				End While
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerWosGuid, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "ListEmployeeDocuments", .MessageContent = msgContent})
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


	<WebMethod(Description:="Get assigned scan data")> _
	Function GetAssignedEmployeeDocumentsData(ByVal customerWosGuid As String, ByVal recID As Integer) As EmployeeWOSDataDTO

		Dim listOfSearchResultDTO As EmployeeWOSDataDTO = Nothing
		Dim connString As String = My.Settings.ConnStr_spContract
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get Assigned Employee WOS Document Data]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlClient.SqlParameter("RecID", recID))

			' Open connection to database.
			conn.Open()

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}", _
																				cmd.Parameters(i).ParameterName, _
																				cmd.Parameters(i).DbType, _
																				cmd.Parameters(i).Size, _
																				cmd.Parameters(i).Value, _
																				ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New EmployeeWOSDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			If (Not reader Is Nothing AndAlso reader.Read()) Then

				listOfSearchResultDTO.ID = utility.SafeGetInteger(reader, "ID", 0)
				listOfSearchResultDTO.Customer_ID = utility.SafeGetString(reader, "Customer_ID")

				listOfSearchResultDTO.EmployeeNumber = utility.SafeGetInteger(reader, "MANr", Nothing)
				listOfSearchResultDTO.EmploymentNumber = utility.SafeGetInteger(reader, "ESNr", Nothing)
				listOfSearchResultDTO.PayrollNumber = utility.SafeGetInteger(reader, "LONr", Nothing)
				listOfSearchResultDTO.ReportNumber = utility.SafeGetInteger(reader, "RPNr", Nothing)

				listOfSearchResultDTO.EmployeeFirstName = utility.SafeGetString(reader, "MA_Vorname")
				listOfSearchResultDTO.EmployeeLastName = utility.SafeGetString(reader, "MA_Nachname")

				listOfSearchResultDTO.EmployeeGuid = utility.SafeGetString(reader, "Owner_Guid")
				listOfSearchResultDTO.DocGuid = utility.SafeGetString(reader, "Doc_Guid")

				listOfSearchResultDTO.DocumentArt = utility.SafeGetString(reader, "Doc_Art")
				listOfSearchResultDTO.DocumentInfo = utility.SafeGetString(reader, "Doc_Info")
				listOfSearchResultDTO.TransferedOn = utility.SafeGetDateTime(reader, "Transfered_On", Nothing)
				listOfSearchResultDTO.TransferedUser = utility.SafeGetString(reader, "Transfered_User")
				listOfSearchResultDTO.LastNotification = utility.SafeGetDateTime(reader, "LastNotification", Nothing)
				listOfSearchResultDTO.ScanContent = utility.SafeGetByteArray(reader, "ScanContent")

			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerWosGuid, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetAssignedEmployeeDocumentsData", .MessageContent = msgContent})
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
			Dim column As DataColumn = New DataColumn(columnName,
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