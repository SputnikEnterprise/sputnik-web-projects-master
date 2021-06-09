Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_Services.JobPlatform.JobsCH
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPJobsCHVacancies.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPJobsCHVacancies
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPJobsCHVacancies"


	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities

		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)

	End Sub


	''' <summary>
	''' saves Jobs.ch Data into tblJobCHPlattform
	''' </summary>
	''' <param name="customerGuid">The customer guid.</param>
	''' <param name="userGuid">The user guid.</param>
	''' <param name="vacancyData">The vacancy data.</param>
	''' <returns>Boolean flag indicating success.</returns>
	<WebMethod(Description:="Speichert Jobs.ch Daten in die interne Datenbanken.")>
	Function SaveJobCHVacancy(ByVal customerGuid As String,
							ByVal userGuid As String,
							ByVal vacancyData As DataTable) As Boolean

		Dim utility As New ClsUtilities
		Dim strMessage As New StringBuilder()
		Dim conn As SqlConnection = Nothing

		Try
			If (vacancyData Is Nothing) Then
				Throw New Exception(String.Format("(CustomerGuid={0}, userGuid={1} >>> vacancyData was nothing!", customerGuid, userGuid))
				Return False
			End If

			If Not vacancyData.Rows.Count = 1 Then
				Throw New Exception(String.Format("(CustomerGuid={0}, userGuid={1} >>> more than one record was founded!", customerGuid, userGuid))
				Return False
			End If

			Dim vacancyRowData = vacancyData.Rows(0)

			Dim organisationID As String = GetValueFromdataRow(vacancyRowData, "Jobs_Organisation_ID")
			Dim organisationSubID As String = GetValueFromdataRow(vacancyRowData, "Jobs_Organisation_SubID")
			Dim vakNr As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "VakNr"))
			Dim inseratID As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "VakNr"))


			Dim jobplattformsCustomerData = m_SysInfo.LoadJobPlattformCustomerData(customerGuid, userGuid, Val(organisationSubID))
			Dim allowedToTransfer As Boolean = True
			Dim advisorFullname As String = String.Empty
			Dim customerName As String = String.Empty

			If jobplattformsCustomerData Is Nothing Then
				allowedToTransfer = False

			Else
				allowedToTransfer = (Not String.IsNullOrWhiteSpace(jobplattformsCustomerData.PlattformLabel)) AndAlso jobplattformsCustomerData.PlattformLabel.ToUpper = "Jobs.CH".ToUpper
				customerName = jobplattformsCustomerData.CustomerName
				advisorFullname = jobplattformsCustomerData.Advisorfullname

			End If

			If Not allowedToTransfer Then
				m_utility.SendNotificationMails("info@sputnik-it.com",
												"notification@sputnik-it.com",
												"You are not allowed to send data to jobplattform (jobs.ch)!",
												String.Format("<b>You are not allowed to send data to jobplattform (jobs.ch)!</b>{0}CustomerGuid: {1}{0}OrganisationId: {2}{0}OrganisationsubId: {3}{0}CustomerName: {4}{0}AdvisorName: {5}{0}Vakanzennummer: {6}",
															  "<br>", customerGuid, organisationID, organisationSubID, customerName, advisorFullname, vakNr),
												Nothing)
				Throw New Exception(String.Format("(CustomerGuid={0}, OrganisationId={1}, OrganisationsubId={2}). You are not allowed to send data to jobplattform (jobs.ch)!", customerGuid, organisationID, organisationSubID))
			End If

			' Extract parameter data from data row.
			Dim vorspann As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Vorspann")
			Dim beruf As String = GetValueFromdataRow(vacancyRowData, "Bezeichnung")
			Dim taetigkeit As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Aufgabe")
			Dim anforderung As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Anforderung")
			Dim wirBieten As String = GetValueFromdataRow(vacancyRowData, "_Jobs_WirBieten")
			Dim plz As String = GetValueFromdataRow(vacancyRowData, "JobPlz")
			Dim ort As String = GetValueFromdataRow(vacancyRowData, "JobOrt")
			Dim kontakt As String = GetValueFromdataRow(vacancyRowData, "UserKontakt")
			Dim email As String = GetValueFromdataRow(vacancyRowData, "UsereMail")
			Dim url As String = GetValueFromdataRow(vacancyRowData, "our_url")
			Dim startDate As String = GetValueFromdataRow(vacancyRowData, "StartDate")
			Dim endDate As String = GetValueFromdataRow(vacancyRowData, "EndDate")
			Dim titel As String = If(GetValueFromdataRow(vacancyRowData, "JobCH_Titel") = String.Empty, GetValueFromdataRow(vacancyRowData, "Bezeichnung"), GetValueFromdataRow(vacancyRowData, "JobCH_Titel"))
			Dim anriss As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anriss")
			Dim firma As String = GetValueFromdataRow(vacancyRowData, "Firma1")
			Dim anstellungsart As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anstellungsart")
			Dim our_URL As String = GetValueFromdataRow(vacancyRowData, "Our_URL")
			Dim anstellungsgrad_von_bis As String = GetValueFromdataRow(vacancyRowData, "JobCH_Anstellungsgrad")
			Dim rubrikID As String = GetValueFromdataRow(vacancyRowData, "JobCH_RubrikID")
			Dim position As String = GetValueFromdataRow(vacancyRowData, "JobCH_Position")
			Dim branche As String = GetValueFromdataRow(vacancyRowData, "JobCH_Branche")
			Dim sprache As String = GetValueFromdataRow(vacancyRowData, "JobCH_Sprache")
			Dim region As String = GetValueFromdataRow(vacancyRowData, "JobCH_RegionData")
			Dim alter_von_bis As String = GetValueFromdataRow(vacancyRowData, "JobCH_Alter_von_Bis")
			Dim sprachkenntniss_Kandidat As String = GetValueFromdataRow(vacancyRowData, "JobCH_SprachKenntniss")
			Dim sprachkenntniss_Niveau As String = GetValueFromdataRow(vacancyRowData, "JobCH_SprachNiveau")
			Dim bildungsniveau As String = GetValueFromdataRow(vacancyRowData, "JobCH_BildungsNiveauData")
			Dim berufserfahrung As String = GetValueFromdataRow(vacancyRowData, "JobCH_Beruferfahrung")
			Dim berufserfahrung_Position As String = GetValueFromdataRow(vacancyRowData, "JobCH_Beruferfahrung_position")
			Dim layout As String = GetValueFromdataRow(vacancyRowData, "Jobs_Layout_ID")
			Dim logo As String = GetValueFromdataRow(vacancyRowData, "Jobs_Logo_ID")
			Dim bewerben_URL As String = GetValueFromdataRow(vacancyRowData, "Bewerben_URL")
			Dim angebot As String = GetValueFromdataRow(vacancyRowData, "Jobs_Angebot_Value")
			Dim direkt_URL As String = GetValueFromdataRow(vacancyRowData, "Direkt_URL")
			direkt_URL = String.Format(direkt_URL, vakNr)

			Dim direkt_URL_Post_Args As String = String.Empty
			Dim xing_Poster_URL As String = GetValueFromdataRow(vacancyRowData, "Xing_Poster_URL")
			Dim xing_Company_Profile_URL As String = GetValueFromdataRow(vacancyRowData, "Xing_Company_Profile_URL")
			Dim xing_Company_Is_Poc As String = If(GetValueFromdataRow(vacancyRowData, "Xing_Company_Is_Poc") = False, "0", "1")


			Dim setOnline As String = If(GetValueFromdataRow(vacancyRowData, "IsOnline") = False, "0", "1")

			' Save vacancy in databse.

			Dim connString As String = My.Settings.ConnStr_New_spContract
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Create New Vacancy For Jobs.CH]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr ", ReplaceMissing(vakNr, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("OrganisationID ", ReplaceMissing(organisationSubID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("InseratID", ReplaceMissing(inseratID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Vorspann", ReplaceMissing(vorspann, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Beruf", ReplaceMissing(beruf, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Taetigkeit", ReplaceMissing(taetigkeit, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anforderung", ReplaceMissing(anforderung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("WirBieten", ReplaceMissing(wirBieten, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(plz, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(ort, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Kontakt", ReplaceMissing(kontakt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(email, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("URL", ReplaceMissing(url, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("StartDate", ReplaceMissing(startDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EndDate", ReplaceMissing(endDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Titel", ReplaceMissing(titel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anriss", ReplaceMissing(anriss, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Firma", ReplaceMissing(firma, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anstellungsart", ReplaceMissing(anstellungsart, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Our_URL", ReplaceMissing(our_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anstellungsgrad_Von_Bis", ReplaceMissing(anstellungsgrad_von_bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("RubrikID", ReplaceMissing(rubrikID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Position", ReplaceMissing(position, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Branche", ReplaceMissing(branche, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprache", ReplaceMissing(sprache, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Region", ReplaceMissing(region, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Alter_Von_Bis", ReplaceMissing(alter_von_bis, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprachkenntniss_Kandidat", ReplaceMissing(sprachkenntniss_Kandidat, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Sprachkenntniss_Niveau", ReplaceMissing(sprachkenntniss_Niveau, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bildungsniveau", ReplaceMissing(bildungsniveau, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Berufserfahrung", ReplaceMissing(berufserfahrung, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Berufserfahrung_Position", ReplaceMissing(berufserfahrung_Position, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Lyout", ReplaceMissing(layout, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Logo", ReplaceMissing(logo, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Bewerben_URL", ReplaceMissing(bewerben_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Angebot", ReplaceMissing(angebot, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Direkt_URL", ReplaceMissing(direkt_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Direkt_URL_Post_Args", ReplaceMissing(direkt_URL_Post_Args, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Poster_URL", ReplaceMissing(xing_Poster_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Company_Profile_URL", ReplaceMissing(xing_Company_Profile_URL, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Xing_Company_Is_Poc", ReplaceMissing(xing_Company_Is_Poc, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("SetOnline", ReplaceMissing(setOnline, DBNull.Value)))

			conn.Open()
			cmd.Parameters.AddRange(listOfParams.ToArray())

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																					cmd.Parameters(i).ParameterName,
																					cmd.Parameters(i).DbType,
																					cmd.Parameters(i).Size,
																					cmd.Parameters(i).Value,
																					ControlChars.NewLine))
			Next

			cmd.ExecuteNonQuery()

			Try
				Dim jobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_JobCh")
				Dim organsiationRootPath = System.IO.Path.Combine(jobChRoot, organisationSubID.ToString)
				Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

#If DEBUG Then
				organsiationRootPath = "C:\temp\222222"
				xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")
#End If

				If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)

				Dim vacanciesGenerator As New JobPlatform.JobsCH.JobChVacanciesXMLGenerator(customerGuid, organisationSubID)
				Dim xDoc = vacanciesGenerator.GenerateVacanciesXml()

				Try
					SaveXmlDocument(xDoc, organisationSubID)
					utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerGuid, .SourceModul = "SPJobsCHVacancies", .MessageHeader = "SaveJobCHVacancy",
															 .MessageContent = String.Format("organisationSubID: {0} >>> SetOnline: {1} >>> xmlFileName: {2} | Data was saved in db.", organisationSubID, setOnline, xmlFileName)})

				Catch ex As Exception
					If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)
					Throw New Exception(String.Format("(CustomerGuid={0}, OrganisationId={1}, OrganisationsubId={2}). SaveXmlDocument={3}", customerGuid, organisationID, organisationSubID, ex.ToString))

				End Try

			Catch ex As Exception
				Throw New Exception(String.Format("(CustomerGuid={0}, OrganisationId={1}). jobChRoot={2}", customerGuid, organisationID, ex.ToString))

			End Try

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerGuid, .SourceModul = "SPJobsCHVacancies", .MessageHeader = "SaveJobCHVacancy", .MessageContent = msgContent})

			'SaveErrToDb(String.Format("Fehler {1}:{0}{2}", vbNewLine, "SaveJobCHVacancy", ex.Message))

			Return False

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

		Return True
	End Function

	''' <summary>
	''' Saves the jobCh xml document on the file system.
	''' </summary>
	''' <param name="xDoc">The xml document.</param>
	Protected Sub SaveXmlDocument(ByVal xDoc As XDocument, ByVal organisationId As Integer)

		Dim jobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_JobCh")
		Dim organsiationRootPath = System.IO.Path.Combine(jobChRoot, organisationId.ToString)
		Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

#If DEBUG Then
		organsiationRootPath = "C:\temp\222222"
		xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")
#End If

		If Not Directory.Exists(organsiationRootPath) Then
			Directory.CreateDirectory(organsiationRootPath)
		End If

		Dim xmlCode As String = String.Empty

		' Use a special string writer here in order to get the iso-8859-1 encodinginto the the xml.
		Using sw As StringWriterWithEncoding = New StringWriterWithEncoding(Encoding.GetEncoding(xDoc.Declaration.Encoding))
			xDoc.Save(sw)
			xmlCode = sw.ToString()
		End Using

		Using sw As New System.IO.StreamWriter(System.IO.File.Open(xmlFileName, FileMode.Create), Encoding.GetEncoding("ISO-8859-1"))

			Try
				sw.WriteLine(xmlCode)
			Catch ex As Exception
				' Maybe the file is currently in use -> wait a little bit.
				Threading.Thread.Sleep(500)
				sw.WriteLine(xmlCode)
			End Try

		End Using

	End Sub

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

	'''' <summary>
	'''' Saves an error to the database.
	'''' </summary>
	'''' <param name="strErrorMessage">The error message.</param>
	'Function SaveErrToDb(ByVal strErrorMessage As String) As String
	'  Dim strResult As String = "Erfolgreich..."
	'Dim connString As String = My.Settings.ConnStr_New_spContract
	'Dim strSQL As String = String.Empty
	'  strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'  strSQL &= "CreatedOn) Values ("
	'  strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

	'  Dim Conn As SqlConnection = New SqlConnection(connString)
	'  Conn.Open()

	'  Try
	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'    cmd.CommandType = Data.CommandType.Text
	'    Dim param As System.Data.SqlClient.SqlParameter
	'    param = cmd.Parameters.AddWithValue("@Modulname", "Err: spJobsCHVacansices.asmx")
	'    param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
	'    param = cmd.Parameters.AddWithValue("@RequestParam", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

	'    cmd.ExecuteNonQuery()

	'  Catch ex As Exception
	'    '     strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)

	'  Finally

	'    If Not Conn Is Nothing Then
	'      Conn.Close()
	'      Conn.Dispose()
	'    End If

	'  End Try

	'  Return strResult
	'End Function

End Class

