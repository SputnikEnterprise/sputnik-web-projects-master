
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


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET-AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
<System.Web.Script.Services.ScriptService()>
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/externalservices/SPVacancyServices.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPVacancyServices
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPVacancyServices"

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

#Region "WebMethods"

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Berufe (Titel)")>
	Function ListVacancyTitle(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Titel]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Bezeichnung
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Bezeichnung").ToString)

				i += 1
			End While
		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Regionen (Region)")>
	Function ListVacancyRegion(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Region]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Regionen
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Vak_Region").ToString)

				i += 1
			End While
		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Filiale (Filiale)")>
	Function ListVacancyBranch(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Filiale]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Filiale
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Filiale").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Ort (Ort)")>
	Function ListVacancyCity(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Ort]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Ort
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("JobOrt").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Kanton (Vak_Kanton)")>
	Function ListVacancyCanton(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Kanton]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Kanton
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Vak_Kanton").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Gruppe (Gruppe)")>
	Function ListVacancyGroup(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Gruppe]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Gruppe
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Gruppe").ToString)

				i += 1
			End While
		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-SubGruppe (SubGroup)")>
	Function ListVacancySubGroup(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-SubGruppe]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("SubGroup").ToString)

				i += 1
			End While
		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-SubGruppe anhand einer Grouppe (SubGroup)")>
	Function ListVacancySubGroupAssignedGroup(ByVal customerWOSID As String, ByVal vacancyGroup As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vacancy SubGruppe For Assigned Group]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			param = cmd.Parameters.AddWithValue("@Group", vacancyGroup)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("SubGroup").ToString)

				i += 1
			End While
		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function


	<WebMethod(Description:="Zur Auflistung der Vakanzen-Branchen (Branchen)")>
	Function ListVacancySector(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If String.IsNullOrWhiteSpace(customerWOSID) Then Return Nothing

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Branchen]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Gruppe
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Branchen").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-sologan (werbeschlagwort)")>
	Function ListVacancySlogan(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If String.IsNullOrWhiteSpace(customerWOSID) Then Return Nothing

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vak-Slogan]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@WOS_Guid", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Gruppe
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("Slogan").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen (Alle Datensätze)")>
	Function ListVacancies(ByVal customerWOSID As String, ByVal qualification As String,
											ByVal location As String, ByVal strKanton As String,
											ByVal region As String,
											ByVal branchOffice As String,
											ByVal vacancyGroup As String,
											ByVal employment As String,
											ByVal branch As String,
											ByVal jobCategory As String,
											ByVal jobDisipline As String,
											ByVal jobPosition As String) As KDVakanzenDTO()

		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[Load All Assigned Vacancy Data]"
		Dim strRecResult As String = String.Empty
		If String.IsNullOrWhiteSpace(customerWOSID) Then Return Nothing

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim result As List(Of KDVakanzenDTO) = New List(Of KDVakanzenDTO)

		' ---------------------------------------------------------------------------------------
		Try
			cmd.Parameters.Add(New SqlParameter("@CustomerID", ReplaceMissing(customerWOSID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@WOS_Guid", ReplaceMissing(customerWOSID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Beruf", ReplaceMissing(qualification, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Ort", ReplaceMissing(location, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Kanton", ReplaceMissing(strKanton, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Region", ReplaceMissing(region, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Filiale", ReplaceMissing(branchOffice, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Gruppe", ReplaceMissing(vacancyGroup, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Anstellung", ReplaceMissing(employment, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@Branche", ReplaceMissing(branch, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@JobCategorie", ReplaceMissing(jobCategory, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@JobDisipline", ReplaceMissing(jobDisipline, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@JobPosition", ReplaceMissing(jobPosition, String.Empty)))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			While reader.Read()
				Dim kdVakanzenDTO As KDVakanzenDTO = New KDVakanzenDTO With {
										.RecID = SafeGetInteger(reader, "ID", Nothing),
										.VakNr = SafeGetInteger(reader, "VakNr", Nothing),
										.KDNr = SafeGetInteger(reader, "KDNr", Nothing),
										.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing),
										.Customer_ID = SafeGetString(reader, "Customer_ID"),
										.Customer_Name = SafeGetString(reader, "Customer_Name"),
										.Customer_Strasse = SafeGetString(reader, "Customer_Strasse"),
										.Customer_Ort = SafeGetString(reader, "Customer_Ort"),
										.Customer_Telefon = SafeGetString(reader, "Customer_Telefon"),
										.Customer_eMail = SafeGetString(reader, "Customer_eMail"),
										.Berater = SafeGetString(reader, "Berater"),
										.Filiale = SafeGetString(reader, "Filiale"),
										.VakKontakt = SafeGetString(reader, "VakKontakt"),
										.VakState = SafeGetString(reader, "VakState"),
										.Bezeichnung = SafeGetString(reader, "Bezeichnung"),
										.TitelForSearch = SafeGetString(reader, "TitelForSearch"),
										.ShortDescription = SafeGetString(reader, "ShortDescription"),
										.Slogan = SafeGetString(reader, "Slogan"),
										.Gruppe = SafeGetString(reader, "Gruppe"),
										.SubGroup = SafeGetString(reader, "SubGroup"),
										.ExistLink = SafeGetBoolean(reader, "ExistLink", Nothing),
										.JobChannelPriority = SafeGetBoolean(reader, "JobChannelPriority", False),
										.VakLink = SafeGetString(reader, "VakLink"),
										.Beginn = SafeGetString(reader, "Beginn"),
										.JobProzent = SafeGetString(reader, "JobProzent"),
										.Anstellung = SafeGetString(reader, "Anstellung"),
										.Dauer = SafeGetString(reader, "Dauer"),
										.MAAge = SafeGetString(reader, "MAAge"),
										.MASex = SafeGetString(reader, "MASex"),
										.MAZivil = SafeGetString(reader, "MAZivil"),
										.MALohn = SafeGetString(reader, "MALohn"),
										.Jobtime = SafeGetString(reader, "Jobtime"),
										.JobOrt = SafeGetString(reader, "JobOrt"),
										.MAFSchein = SafeGetString(reader, "MAFSchein"),
										.MAAuto = SafeGetString(reader, "MAAuto"),
										.MANationality = SafeGetString(reader, "MANationality"),
										.IEExport = SafeGetBoolean(reader, "IEExport", Nothing),
										.KDBeschreibung = SafeGetString(reader, "KDBeschreibung"),
										.KDBietet = SafeGetString(reader, "KDBietet"),
										.SBeschreibung = SafeGetString(reader, "SBeschreibung"),
										.Reserve1 = SafeGetString(reader, "Reserve1"),
										.Taetigkeit = SafeGetString(reader, "Taetigkeit"),
										.Anforderung = SafeGetString(reader, "Anforderung"),
										.Reserve2 = SafeGetString(reader, "Reserve2"),
										.Reserve3 = SafeGetString(reader, "Reserve3"),
										.Ausbildung = SafeGetString(reader, "Ausbildung"),
										.Weiterbildung = SafeGetString(reader, "Weiterbildung"),
										.SKennt = SafeGetString(reader, "SKennt"),
										.EDVKennt = SafeGetString(reader, "EDVKennt"),
										.html_KDBeschreibung = SafeGetString(reader, "_KDBeschreibung"),
										.html_KDBietet = SafeGetString(reader, "_KDBietet"),
										.html_SBeschreibung = SafeGetString(reader, "_SBeschreibung"),
										.html_Reserve1 = SafeGetString(reader, "_Reserve1"),
										.html_Taetigkeit = SafeGetString(reader, "_Taetigkeit"),
										.html_Anforderung = SafeGetString(reader, "_Anforderung"),
										.html_Reserve2 = SafeGetString(reader, "_Reserve2"),
										.html_Reserve3 = SafeGetString(reader, "_Reserve3"),
										.html_Ausbildung = SafeGetString(reader, "_Ausbildung"),
										.html_Weiterbildung = SafeGetString(reader, "_Weiterbildung"),
										.html_SKennt = SafeGetString(reader, "_SKennt"),
										.html_EDVKennt = SafeGetString(reader, "_EDVKennt"),
										.Branchen = SafeGetString(reader, "Branchen"),
										.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing),
										.CreatedFrom = SafeGetString(reader, "CreatedFrom"),
										.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing),
										.ChangedFrom = SafeGetString(reader, "ChangedFrom"),
										.Transfered_User = SafeGetString(reader, "Transfered_User"),
										.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing),
										.Transfered_Guid = SafeGetString(reader, "Transfered_Guid"),
										.Result = SafeGetString(reader, "Result"),
										.Vak_Region = SafeGetString(reader, "Vak_Region"),
										.Vak_Kanton = SafeGetString(reader, "Vak_Kanton"),
										.MSprachen = SafeGetString(reader, "MSprachen"),
										.SSprachen = SafeGetString(reader, "SSprachen"),
										.Qualifikation = SafeGetString(reader, "Qualifikation"),
										.SQualifikation = SafeGetString(reader, "SQualifikation"),
										.User_Guid = SafeGetString(reader, "User_Guid"),
										.Job_Disciplines = SafeGetString(reader, "vje_Job_Disciplines", ""),
										.Job_Disciplines_Match = SafeGetString(reader, "Job_Disciplines_Match", ""),
										.Vak_Region_Match = SafeGetString(reader, "Vak_Region_Match", ""),
										.JobPLZ = SafeGetString(reader, "JobPLZ")
									}
				result.Add(kdVakanzenDTO)
			End While


		Catch ex As Exception
			m_Logger.LogInfo(String.Format("ListVacancies: customerWOSID: {1}{0}{2}", vbNewLine, customerWOSID, ex.ToString))
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "ListVacancies", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return result.ToArray()
	End Function

	<WebMethod(Description:="Zur Auflistung eines Datensatzes anhand eines ID in Form von Objekt")>
	Function GetSelectedVacancy(ByVal customerWOSID As String, ByVal recID As Integer) As KDVakanzenDTO()
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[Get Vakrec By ID Filtered]"

		If String.IsNullOrWhiteSpace(customerWOSID) OrElse recID = 0 Then Return Nothing
		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim result As List(Of KDVakanzenDTO) = New List(Of KDVakanzenDTO)

		' ---------------------------------------------------------------------------------------
		Try
			cmd.Parameters.Add(New SqlParameter("@UserID", ReplaceMissing(customerWOSID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@RecID", ReplaceMissing(recID, 0)))

			Dim reader As SqlDataReader = cmd.ExecuteReader

			While reader.Read()
				Dim kdVakanzenDTO As KDVakanzenDTO = New KDVakanzenDTO With {
										.RecID = SafeGetInteger(reader, "ID", Nothing),
										.VakNr = SafeGetInteger(reader, "VakNr", Nothing),
										.KDNr = SafeGetInteger(reader, "KDNr", Nothing),
										.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing),
										.Customer_ID = SafeGetString(reader, "Customer_ID"),
										.Customer_Name = SafeGetString(reader, "Customer_Name"),
										.Customer_Strasse = SafeGetString(reader, "Customer_Strasse"),
										.Customer_Ort = SafeGetString(reader, "Customer_Ort"),
										.Customer_Telefon = SafeGetString(reader, "Customer_Telefon"),
										.Customer_eMail = SafeGetString(reader, "Customer_eMail"),
										.Berater = SafeGetString(reader, "Berater"),
										.Filiale = SafeGetString(reader, "Filiale"),
										.VakKontakt = SafeGetString(reader, "VakKontakt"),
										.VakState = SafeGetString(reader, "VakState"),
										.Bezeichnung = SafeGetString(reader, "Bezeichnung"),
										.TitelForSearch = SafeGetString(reader, "TitelForSearch"),
										.ShortDescription = SafeGetString(reader, "ShortDescription"),
										.Slogan = SafeGetString(reader, "Slogan"),
										.Gruppe = SafeGetString(reader, "Gruppe"),
										.SubGroup = SafeGetString(reader, "SubGroup"),
										.ExistLink = SafeGetBoolean(reader, "ExistLink", Nothing),
										.JobChannelPriority = SafeGetBoolean(reader, "JobChannelPriority", False),
										.VakLink = SafeGetString(reader, "VakLink"),
										.Beginn = SafeGetString(reader, "Beginn"),
										.JobProzent = SafeGetString(reader, "JobProzent"),
										.Anstellung = SafeGetString(reader, "Anstellung"),
										.Dauer = SafeGetString(reader, "Dauer"),
										.MAAge = SafeGetString(reader, "MAAge"),
										.MASex = SafeGetString(reader, "MASex"),
										.MAZivil = SafeGetString(reader, "MAZivil"),
										.MALohn = SafeGetString(reader, "MALohn"),
										.Jobtime = SafeGetString(reader, "Jobtime"),
										.JobOrt = SafeGetString(reader, "JobOrt"),
										.MAFSchein = SafeGetString(reader, "MAFSchein"),
										.MAAuto = SafeGetString(reader, "MAAuto"),
										.MANationality = SafeGetString(reader, "MANationality"),
										.KDBeschreibung = SafeGetString(reader, "KDBeschreibung"),
										.KDBietet = SafeGetString(reader, "KDBietet"),
										.SBeschreibung = SafeGetString(reader, "SBeschreibung"),
										.Reserve1 = SafeGetString(reader, "Reserve1"),
										.Taetigkeit = SafeGetString(reader, "Taetigkeit"),
										.Anforderung = SafeGetString(reader, "Anforderung"),
										.Reserve2 = SafeGetString(reader, "Reserve2"),
										.Reserve3 = SafeGetString(reader, "Reserve3"),
										.Ausbildung = SafeGetString(reader, "Ausbildung"),
										.Weiterbildung = SafeGetString(reader, "Weiterbildung"),
										.SKennt = SafeGetString(reader, "SKennt"),
										.EDVKennt = SafeGetString(reader, "EDVKennt"),
										.html_KDBeschreibung = SafeGetString(reader, "_KDBeschreibung"),
										.html_KDBietet = SafeGetString(reader, "_KDBietet"),
										.html_SBeschreibung = SafeGetString(reader, "_SBeschreibung"),
										.html_Reserve1 = SafeGetString(reader, "_Reserve1"),
										.html_Taetigkeit = SafeGetString(reader, "_Taetigkeit"),
										.html_Anforderung = SafeGetString(reader, "_Anforderung"),
										.html_Reserve2 = SafeGetString(reader, "_Reserve2"),
										.html_Reserve3 = SafeGetString(reader, "_Reserve3"),
										.html_Ausbildung = SafeGetString(reader, "_Ausbildung"),
										.html_Weiterbildung = SafeGetString(reader, "_Weiterbildung"),
										.html_SKennt = SafeGetString(reader, "_SKennt"),
										.html_EDVKennt = SafeGetString(reader, "_EDVKennt"),
										.Branchen = SafeGetString(reader, "Branchen"),
										.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing),
										.CreatedFrom = SafeGetString(reader, "CreatedFrom"),
										.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing),
										.ChangedFrom = SafeGetString(reader, "ChangedFrom"),
										.Transfered_User = SafeGetString(reader, "Transfered_User"),
										.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing),
										.Transfered_Guid = SafeGetString(reader, "Transfered_Guid"),
										.Vak_Region = SafeGetString(reader, "Vak_Region"),
										.Vak_Kanton = SafeGetString(reader, "Vak_Kanton"),
										.MSprachen = SafeGetString(reader, "MSprachen"),
										.SSprachen = SafeGetString(reader, "SSprachen"),
										.Qualifikation = SafeGetString(reader, "Qualifikation"),
										.SQualifikation = SafeGetString(reader, "SQualifikation"),
										.User_Guid = SafeGetString(reader, "User_Guid"),
										.JobPLZ = SafeGetString(reader, "JobPLZ"),
										.Job_Categories = SafeGetString(reader, "Job_Categories"),
										.Job_Disciplines = SafeGetString(reader, "Job_Disciplines"),
										.Job_Position = SafeGetString(reader, "Job_Position"),
										.User_salutation = SafeGetString(reader, "BeraterSex"),
										.User_Firstname = SafeGetString(reader, "BeraterVorname"),
										.User_Lastname = SafeGetString(reader, "BeraterNachname"),
										.User_EMail = SafeGetString(reader, "BeraterEMail"),
										.User_Telefon = SafeGetString(reader, "BeraterTelefon"),
										.User_Picture = SafeGetByteArray(reader, "BeraterPicture")
									}
				result.Add(kdVakanzenDTO)
			End While


		Catch ex As Exception
			m_Logger.LogInfo(String.Format("GetSelectedVacancy: customerWOSID: {1} | recID: {2}{0}{3}", vbNewLine, customerWOSID, recID, ex.ToString))
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetSelectedVacancy", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return result.ToArray()

	End Function

	<WebMethod(Description:="Zur Auflistung eines Datensatzes anhand einer Vakanzennummer in Form von Objekt")>
	Function GetSelectedVacancyWithVacancyNumber(ByVal customerWosID As String, ByVal vacancyNumber As Integer) As KDVakanzenDTO()
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[Load Assigned Vacancy Data By Vacancynumber]"
		Dim strRecResult As String = String.Empty
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim result As List(Of KDVakanzenDTO) = New List(Of KDVakanzenDTO)

		' ---------------------------------------------------------------------------------------
		Try
			cmd.Parameters.Add(New SqlParameter("@CustomerID", ReplaceMissing(customerWosID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@WOS_Guid", ReplaceMissing(customerWosID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@vakNumber", ReplaceMissing(vacancyNumber, 0)))

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			Dim reader As SqlDataReader = cmd.ExecuteReader

			While reader.Read()
				Dim kdVakanzenDTO As KDVakanzenDTO = New KDVakanzenDTO With {
										.RecID = SafeGetInteger(reader, "ID", Nothing),
										.VakNr = SafeGetInteger(reader, "VakNr", Nothing),
										.KDNr = SafeGetInteger(reader, "KDNr", Nothing),
										.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing),
										.Customer_ID = SafeGetString(reader, "Customer_ID"),
										.Customer_Name = SafeGetString(reader, "Customer_Name"),
										.Customer_Strasse = SafeGetString(reader, "Customer_Strasse"),
										.Customer_Ort = SafeGetString(reader, "Customer_Ort"),
										.Customer_Telefon = SafeGetString(reader, "Customer_Telefon"),
										.Customer_eMail = SafeGetString(reader, "Customer_eMail"),
										.Berater = SafeGetString(reader, "Berater"),
										.Filiale = SafeGetString(reader, "Filiale"),
										.VakKontakt = SafeGetString(reader, "VakKontakt"),
										.VakState = SafeGetString(reader, "VakState"),
										.Bezeichnung = SafeGetString(reader, "Bezeichnung"),
										.TitelForSearch = SafeGetString(reader, "TitelForSearch"),
										.ShortDescription = SafeGetString(reader, "ShortDescription"),
										.Slogan = SafeGetString(reader, "Slogan"),
										.Gruppe = SafeGetString(reader, "Gruppe"),
										.SubGroup = SafeGetString(reader, "SubGroup"),
										.ExistLink = SafeGetBoolean(reader, "ExistLink", Nothing),
										.JobChannelPriority = SafeGetBoolean(reader, "JobChannelPriority", False),
										.VakLink = SafeGetString(reader, "VakLink"),
										.Beginn = SafeGetString(reader, "Beginn"),
										.JobProzent = SafeGetString(reader, "JobProzent"),
										.Anstellung = SafeGetString(reader, "Anstellung"),
										.Dauer = SafeGetString(reader, "Dauer"),
										.MAAge = SafeGetString(reader, "MAAge"),
										.MASex = SafeGetString(reader, "MASex"),
										.MAZivil = SafeGetString(reader, "MAZivil"),
										.MALohn = SafeGetString(reader, "MALohn"),
										.Jobtime = SafeGetString(reader, "Jobtime"),
										.JobOrt = SafeGetString(reader, "JobOrt"),
										.MAFSchein = SafeGetString(reader, "MAFSchein"),
										.MAAuto = SafeGetString(reader, "MAAuto"),
										.MANationality = SafeGetString(reader, "MANationality"),
										.KDBeschreibung = SafeGetString(reader, "KDBeschreibung"),
										.KDBietet = SafeGetString(reader, "KDBietet"),
										.SBeschreibung = SafeGetString(reader, "SBeschreibung"),
										.Reserve1 = SafeGetString(reader, "Reserve1"),
										.Taetigkeit = SafeGetString(reader, "Taetigkeit"),
										.Anforderung = SafeGetString(reader, "Anforderung"),
										.Reserve2 = SafeGetString(reader, "Reserve2"),
										.Reserve3 = SafeGetString(reader, "Reserve3"),
										.Ausbildung = SafeGetString(reader, "Ausbildung"),
										.Weiterbildung = SafeGetString(reader, "Weiterbildung"),
										.SKennt = SafeGetString(reader, "SKennt"),
										.EDVKennt = SafeGetString(reader, "EDVKennt"),
										.html_KDBeschreibung = SafeGetString(reader, "_KDBeschreibung"),
										.html_KDBietet = SafeGetString(reader, "_KDBietet"),
										.html_SBeschreibung = SafeGetString(reader, "_SBeschreibung"),
										.html_Reserve1 = SafeGetString(reader, "_Reserve1"),
										.html_Taetigkeit = SafeGetString(reader, "_Taetigkeit"),
										.html_Anforderung = SafeGetString(reader, "_Anforderung"),
										.html_Reserve2 = SafeGetString(reader, "_Reserve2"),
										.html_Reserve3 = SafeGetString(reader, "_Reserve3"),
										.html_Ausbildung = SafeGetString(reader, "_Ausbildung"),
										.html_Weiterbildung = SafeGetString(reader, "_Weiterbildung"),
										.html_SKennt = SafeGetString(reader, "_SKennt"),
										.html_EDVKennt = SafeGetString(reader, "_EDVKennt"),
										.Branchen = SafeGetString(reader, "Branchen"),
										.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing),
										.CreatedFrom = SafeGetString(reader, "CreatedFrom"),
										.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing),
										.ChangedFrom = SafeGetString(reader, "ChangedFrom"),
										.Transfered_User = SafeGetString(reader, "Transfered_User"),
										.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing),
										.Transfered_Guid = SafeGetString(reader, "Transfered_Guid"),
										.Vak_Region = SafeGetString(reader, "Vak_Region"),
										.Vak_Kanton = SafeGetString(reader, "Vak_Kanton"),
										.MSprachen = SafeGetString(reader, "MSprachen"),
										.SSprachen = SafeGetString(reader, "SSprachen"),
										.Qualifikation = SafeGetString(reader, "Qualifikation"),
										.SQualifikation = SafeGetString(reader, "SQualifikation"),
										.User_Guid = SafeGetString(reader, "User_Guid"),
										.JobPLZ = SafeGetString(reader, "JobPLZ"),
										.Job_Categories = SafeGetString(reader, "Job_Categories"),
										.Job_Disciplines = SafeGetString(reader, "Job_Disciplines"),
										.Job_Position = SafeGetString(reader, "Job_Position"),
										.User_salutation = SafeGetString(reader, "BeraterSex"),
										.User_Firstname = SafeGetString(reader, "BeraterVorname"),
										.User_Lastname = SafeGetString(reader, "BeraterNachname"),
										.User_EMail = SafeGetString(reader, "BeraterEMail"),
										.User_Telefon = SafeGetString(reader, "BeraterTelefon"),
										.User_Picture = SafeGetByteArray(reader, "BeraterPicture")
									}
				result.Add(kdVakanzenDTO)
			End While


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerWosID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetSelectedVacancyWithVacancyNumber", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return result.ToArray()

	End Function


	<WebMethod(Description:="Zur Auflistung eines Datensatzes anhand einer Vakanzennummer in Form von Objekt")>
	Function LoadAssignedVacancyWithCustomerID(ByVal customerID As String, ByVal vacancyNumber As Integer) As KDVakanzenDTO()
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[Load Assigned Vacancy Data By CustomerID]"
		Dim strRecResult As String = String.Empty
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim result As List(Of KDVakanzenDTO) = New List(Of KDVakanzenDTO)

		' ---------------------------------------------------------------------------------------
		Try
			cmd.Parameters.Add(New SqlParameter("@CustomerID", ReplaceMissing(customerID, String.Empty)))
			cmd.Parameters.Add(New SqlParameter("@vakNumber", ReplaceMissing(vacancyNumber, 0)))

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			Dim reader As SqlDataReader = cmd.ExecuteReader

			While reader.Read()
				Dim kdVakanzenDTO As KDVakanzenDTO = New KDVakanzenDTO With {
										.RecID = SafeGetInteger(reader, "ID", Nothing),
										.VakNr = SafeGetInteger(reader, "VakNr", Nothing),
										.KDNr = SafeGetInteger(reader, "KDNr", Nothing),
										.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing),
										.Customer_ID = SafeGetString(reader, "Customer_ID"),
										.Customer_Name = SafeGetString(reader, "Customer_Name"),
										.Customer_Strasse = SafeGetString(reader, "Customer_Strasse"),
										.Customer_Ort = SafeGetString(reader, "Customer_Ort"),
										.Customer_Telefon = SafeGetString(reader, "Customer_Telefon"),
										.Customer_eMail = SafeGetString(reader, "Customer_eMail"),
										.Berater = SafeGetString(reader, "Berater"),
										.Filiale = SafeGetString(reader, "Filiale"),
										.VakKontakt = SafeGetString(reader, "VakKontakt"),
										.VakState = SafeGetString(reader, "VakState"),
										.Bezeichnung = SafeGetString(reader, "Bezeichnung"),
										.TitelForSearch = SafeGetString(reader, "TitelForSearch"),
										.ShortDescription = SafeGetString(reader, "ShortDescription"),
										.Slogan = SafeGetString(reader, "Slogan"),
										.Gruppe = SafeGetString(reader, "Gruppe"),
										.SubGroup = SafeGetString(reader, "SubGroup"),
										.ExistLink = SafeGetBoolean(reader, "ExistLink", Nothing),
										.JobChannelPriority = SafeGetBoolean(reader, "JobChannelPriority", False),
										.VakLink = SafeGetString(reader, "VakLink"),
										.Beginn = SafeGetString(reader, "Beginn"),
										.JobProzent = SafeGetString(reader, "JobProzent"),
										.Anstellung = SafeGetString(reader, "Anstellung"),
										.Dauer = SafeGetString(reader, "Dauer"),
										.MAAge = SafeGetString(reader, "MAAge"),
										.MASex = SafeGetString(reader, "MASex"),
										.MAZivil = SafeGetString(reader, "MAZivil"),
										.MALohn = SafeGetString(reader, "MALohn"),
										.Jobtime = SafeGetString(reader, "Jobtime"),
										.JobOrt = SafeGetString(reader, "JobOrt"),
										.MAFSchein = SafeGetString(reader, "MAFSchein"),
										.MAAuto = SafeGetString(reader, "MAAuto"),
										.MANationality = SafeGetString(reader, "MANationality"),
										.KDBeschreibung = SafeGetString(reader, "KDBeschreibung"),
										.KDBietet = SafeGetString(reader, "KDBietet"),
										.SBeschreibung = SafeGetString(reader, "SBeschreibung"),
										.Reserve1 = SafeGetString(reader, "Reserve1"),
										.Taetigkeit = SafeGetString(reader, "Taetigkeit"),
										.Anforderung = SafeGetString(reader, "Anforderung"),
										.Reserve2 = SafeGetString(reader, "Reserve2"),
										.Reserve3 = SafeGetString(reader, "Reserve3"),
										.Ausbildung = SafeGetString(reader, "Ausbildung"),
										.Weiterbildung = SafeGetString(reader, "Weiterbildung"),
										.SKennt = SafeGetString(reader, "SKennt"),
										.EDVKennt = SafeGetString(reader, "EDVKennt"),
										.html_KDBeschreibung = SafeGetString(reader, "_KDBeschreibung"),
										.html_KDBietet = SafeGetString(reader, "_KDBietet"),
										.html_SBeschreibung = SafeGetString(reader, "_SBeschreibung"),
										.html_Reserve1 = SafeGetString(reader, "_Reserve1"),
										.html_Taetigkeit = SafeGetString(reader, "_Taetigkeit"),
										.html_Anforderung = SafeGetString(reader, "_Anforderung"),
										.html_Reserve2 = SafeGetString(reader, "_Reserve2"),
										.html_Reserve3 = SafeGetString(reader, "_Reserve3"),
										.html_Ausbildung = SafeGetString(reader, "_Ausbildung"),
										.html_Weiterbildung = SafeGetString(reader, "_Weiterbildung"),
										.html_SKennt = SafeGetString(reader, "_SKennt"),
										.html_EDVKennt = SafeGetString(reader, "_EDVKennt"),
										.Branchen = SafeGetString(reader, "Branchen"),
										.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing),
										.CreatedFrom = SafeGetString(reader, "CreatedFrom"),
										.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing),
										.ChangedFrom = SafeGetString(reader, "ChangedFrom"),
										.Transfered_User = SafeGetString(reader, "Transfered_User"),
										.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing),
										.Transfered_Guid = SafeGetString(reader, "Transfered_Guid"),
										.Vak_Region = SafeGetString(reader, "Vak_Region"),
										.Vak_Kanton = SafeGetString(reader, "Vak_Kanton"),
										.MSprachen = SafeGetString(reader, "MSprachen"),
										.SSprachen = SafeGetString(reader, "SSprachen"),
										.Qualifikation = SafeGetString(reader, "Qualifikation"),
										.SQualifikation = SafeGetString(reader, "SQualifikation"),
										.User_Guid = SafeGetString(reader, "User_Guid"),
										.JobPLZ = SafeGetString(reader, "JobPLZ"),
										.Job_Categories = SafeGetString(reader, "Job_Categories"),
										.Job_Disciplines = SafeGetString(reader, "Job_Disciplines"),
										.Job_Position = SafeGetString(reader, "Job_Position"),
										.User_salutation = SafeGetString(reader, "BeraterSex"),
										.User_Firstname = SafeGetString(reader, "BeraterVorname"),
										.User_Lastname = SafeGetString(reader, "BeraterNachname"),
										.User_EMail = SafeGetString(reader, "BeraterEMail"),
										.User_Telefon = SafeGetString(reader, "BeraterTelefon"),
										.User_Picture = SafeGetByteArray(reader, "BeraterPicture")
									}
				result.Add(kdVakanzenDTO)
			End While


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedVacancyWithCustomerID", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return result.ToArray()

	End Function

#Region "WebMethods JobCategories, JobDisciplines and positions"

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Berufsgruppen (Berufgruppe)")>
	Function ListVacancyJobCategory(ByVal customerWOSID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vacancy Job Categories]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Berufgruppen

			While rVakrec.Read
				s.Add(rVakrec("Berufgruppe").ToString)
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Fachrichtigungen (Fachrichtung)")>
	Function ListVacancyJobDisciplines(ByVal customerWOSID As String, ByVal JobCategories As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vacancy Job Disciplines]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			param = cmd.Parameters.AddWithValue("@JobCategories", JobCategories)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Fachrichtungen

			While rVakrec.Read
				s.Add(rVakrec("BerufErfahrung").ToString)
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Position (Berufposition)")>
	Function ListVacancyJobPositions(ByVal customerWOSID As String, ByVal jobCategory As String, ByVal jobDiscipline As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_New_spContract
		Dim strSQL As String = "[List Vacancy Job Positions]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", customerWOSID)
			param = cmd.Parameters.AddWithValue("@JobCategories", jobCategory)
			param = cmd.Parameters.AddWithValue("@Jobdisciplines", jobDiscipline)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader

			While rVakrec.Read
				s.Add(rVakrec("BerufPosition").ToString)
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

#End Region



#End Region






#Region "Private Methods"

	'Private Sub WriteConnectionHistory(ByVal customerWOSID As String, ByVal strBeruf As String,
	'										ByVal strOrt As String, ByVal strKanton As String,
	'										ByVal strRegion As String, ByVal strFiliale As String,
	'										ByVal strSortkeys As String, ByVal strRecResult As String)
	'	Dim _clsSystem As New ClsMain_Net
	'	Dim connString As String = My.Settings.ConnStr_New_spContract
	'	Dim sql As String
	'	Dim paramValue As String

	'	paramValue = String.Format("strUserID: {0} | strBeruf: {1} | strOrt: {2} | strKanton: {3} | strRegion: {4} | strFiliale: {5} | strSortkeys: {6} | strRecResult: {7}",
	'														 customerWOSID, strBeruf, strOrt, strKanton, strRegion, strFiliale, strSortkeys, strRecResult)
	'	sql = "Insert Into Tab_ModulUsage (ModulName, UseCount, UseDate, MachineID, ModulParameter, IsWebService) Values ("
	'	sql &= "@ModulName, @UseCount, "
	'	sql &= "GetDate(), @MachineID, @ModulParameter, @IsWebService)"


	'	Dim Conn As SqlConnection = New SqlConnection(connString)
	'	Conn.Open()

	'	Try
	'		Dim cmd As System.Data.SqlClient.SqlCommand
	'		cmd = New System.Data.SqlClient.SqlCommand(sql, Conn)
	'		cmd.CommandType = CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter

	'		param = cmd.Parameters.AddWithValue("@ModulName", "SPVakanzData.asmx")
	'		param = cmd.Parameters.AddWithValue("@UseCount", 1)
	'		param = cmd.Parameters.AddWithValue("@MachineID", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@ModulParameter", paramValue)
	'		param = cmd.Parameters.AddWithValue("@IsWebService", True)


	'		Try
	'			cmd.ExecuteNonQuery()


	'		Catch ex As Exception


	'		End Try

	'	Catch ex As Exception
	'	Finally

	'		If Not Conn Is Nothing Then
	'			Conn.Close()
	'			Conn.Dispose()
	'		End If

	'	End Try

	'End Sub

#End Region


#Region "Helpers"


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

	''' <summary>
	''' Returns a string or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetString(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns a boolean or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetBoolean(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an integer or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetInt32(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an datetime or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetDateTime(columnIndex)
		Else
			Return defaultValue
		End If
	End Function

	''' <summary>
	''' Returns an byte array or nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Protected Shared Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader(columnIndex)
		Else
			Return Nothing
		End If
	End Function

#End Region


End Class

