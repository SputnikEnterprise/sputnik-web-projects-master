Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_Services.JobPlatform.OstJobsCH
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPOstJobsCHVacancies.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPOstJobsCHVacancies
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
	''' saves OstJobs.ch Data into tblOstJobCHPlattform
	''' </summary>
	''' <param name="customerGuid">The customer guid.</param>
	''' <param name="userGuid">The user guid.</param>
	''' <param name="vacancyData">The vacancy data.</param>
	''' <returns>Boolean flag indicating success.</returns>
	<WebMethod(Description:="Speichert OstJobs.ch Daten in die interne Datenbanken.")>
	Function SaveOstJobCHVacancy(ByVal customerGuid As String,
								ByVal userGuid As String,
								ByVal vacancyData As DataTable) As Boolean

		Dim utility As New ClsUtilities
		Dim strMessage As New StringBuilder()
		If (vacancyData Is Nothing) Then
			Return False
		End If

		If Not vacancyData.Rows.Count = 1 Then
			Return False
		End If

		Dim conn As SqlConnection = Nothing
		Try

			Dim vacancyRowData = vacancyData.Rows(0)

			' Extract parameter data from data row.
			Dim vakNr As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "VakNr"))
			Dim jobVersion As Integer = 1
			Dim company As Integer = Val(GetValueFromdataRow(vacancyRowData, "Company"))

			Dim jobplattformsCustomerData = m_SysInfo.LoadJobPlattformCustomerData(customerGuid, userGuid, company)
			Dim allowedToTransfer As Boolean = True
			Dim advisorFullname As String = String.Empty
			Dim customerName As String = String.Empty

			If jobplattformsCustomerData Is Nothing Then
				allowedToTransfer = False

			Else
				allowedToTransfer = Not String.IsNullOrWhiteSpace(jobplattformsCustomerData.PlattformLabel) AndAlso jobplattformsCustomerData.PlattformLabel.ToUpper = "ostjob.ch".ToUpper
				customerName = jobplattformsCustomerData.CustomerName
				advisorFullname = jobplattformsCustomerData.Advisorfullname
			End If

			If Not allowedToTransfer Then
				m_utility.SendNotificationMails("info@sputnik-it.com",
												"notification@sputnik-it.com",
												"You are not allowed to send data to jobplattform (ostjob.ch)!",
												String.Format("<b>You are not allowed to send data to jobplattform (ostjob.ch)!</b>{0}CustomerGuid: {1}{0}company: {2}{0}CustomerName: {3}{0}AdvisorName: {4}{0}Vakanznummer: {5}",
															  "<br>", customerGuid, company, customerName, advisorFullname, vakNr),
												Nothing)
				Throw New Exception(String.Format("(CustomerGuid={0}, company={1}). You are not allowed to send data to jobplattform (ostjob.ch)!", customerGuid, company))
			End If


			Dim title As String = GetValueFromdataRow(vacancyRowData, "Title")
			Dim workplaceCity As String = GetValueFromdataRow(vacancyRowData, "Workplace_city")
			Dim workplaceZip As String = GetValueFromdataRow(vacancyRowData, "Workplace_Zip")
			Dim workplaceCountry As String = GetValueFromdataRow(vacancyRowData, "Workplace_country")

			Dim company_description As String = GetValueFromdataRow(vacancyRowData, "_OJob_company_description")
			Dim aufgabe As String = GetValueFromdataRow(vacancyRowData, "_OJob_Aufgaben")
			Dim anforderungen As String = GetValueFromdataRow(vacancyRowData, "_OJob_requiremnents")
			' Description
			Dim wirBieten As String = GetValueFromdataRow(vacancyRowData, "_OJob_WirBieten")

			Dim contact As String = GetValueFromdataRow(vacancyRowData, "Contact")
			Dim links_description As String = GetValueFromdataRow(vacancyRowData, "description_url")
			Dim links_application As String = GetValueFromdataRow(vacancyRowData, "application_url")

			Dim publicationOstjob_ch As Boolean? = GetValueFromdataRow(vacancyRowData, "ostjob_ch")
			Dim publicationWestjob_at As Boolean? = GetValueFromdataRow(vacancyRowData, "westjob_at")
			Dim publicationNicejob_de As Boolean? = GetValueFromdataRow(vacancyRowData, "nicejob_de")
			Dim publicationZentraljob_ch As Boolean? = GetValueFromdataRow(vacancyRowData, "zentraljob_ch")
			Dim publicationMinisite As Boolean? = GetValueFromdataRow(vacancyRowData, "minisite")
			Dim apprenticeship As Boolean? = GetValueFromdataRow(vacancyRowData, "lehrstelle")
			Dim template As String = GetValueFromdataRow(vacancyRowData, "template")
			Dim keywords As String = GetValueFromdataRow(vacancyRowData, "Keywords")

			Dim startDate As String = GetValueFromdataRow(vacancyRowData, "StartDate")
			Dim endDate As String = GetValueFromdataRow(vacancyRowData, "EndDate")

			Dim isOnline As Boolean? = GetValueFromdataRow(vacancyRowData, "IsOnline")

			' Save vacancy in databse.

			Dim connString As String = My.Settings.ConnStr_New_spContract
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Create New Vacancy For OstJob.CH]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VakNr ", ReplaceMissing(vakNr, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("JobVersion", jobVersion))
			listOfParams.Add(New SqlClient.SqlParameter("Company", ReplaceMissing(company, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Title", ReplaceMissing(title, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Workplace_Country", ReplaceMissing(workplaceCountry, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Workplace_Zip", ReplaceMissing(workplaceZip, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Workplace_City", ReplaceMissing(workplaceCity, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Company_Description", ReplaceMissing(company_description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Aufgabe", ReplaceMissing(aufgabe, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Anforderungen", ReplaceMissing(anforderungen, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("WirBieten", ReplaceMissing(wirBieten, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("contact", ReplaceMissing(contact, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Description_url", ReplaceMissing(links_description, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Application_url", ReplaceMissing(links_application, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Apprenticeship", ReplaceMissing(apprenticeship, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Template", ReplaceMissing(template, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Keywords", ReplaceMissing(keywords, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("StartDate", ReplaceMissing(startDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EndDate", ReplaceMissing(endDate, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("IsOnline", ReplaceMissing(isOnline, False)))


			listOfParams.Add(New SqlClient.SqlParameter("Publication_ostjob_ch", ReplaceMissing(publicationOstjob_ch, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Publication_westjob_at", ReplaceMissing(publicationWestjob_at, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Publication_nicejob_de", ReplaceMissing(publicationNicejob_de, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Publication_zentraljob_ch", ReplaceMissing(publicationZentraljob_ch, False)))
			listOfParams.Add(New SqlClient.SqlParameter("Publication_minisite", ReplaceMissing(publicationMinisite, False)))

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
				Dim ostJobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_OstJobCh")
				Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, customerGuid)
				Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

				If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)

				Dim vacanciesGenerator As New JobPlatform.OstJobsCH.OstJobChVacanciesXMLGenerator(customerGuid)
				Dim xDoc = vacanciesGenerator.GenerateVacanciesXml()

				Try
					SaveXmlDocument(xDoc, customerGuid)

				Catch ex As Exception
					If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)
					Throw New Exception(String.Format("(CustomerGuid={0}). SaveXmlDocument={1}", customerGuid, ex.ToString))

				End Try

			Catch ex As Exception
				Throw New Exception(String.Format("(CustomerGuid={0},). OstJobChRoot={1}", customerGuid, ex.ToString))

			End Try

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerGuid, .SourceModul = "SPOstJobsCHVacancies", .MessageHeader = "SaveOstJobCHVacancy", .MessageContent = msgContent})
			'SaveErrToDb(String.Format("Fehler {1}:{0}{2}", vbNewLine, "SaveOstJobCHVacancy", ex.Message))

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
	''' Saves the ostjob.ch xml document on the file system.
	''' </summary>
	''' <param name="xDoc">The xml document.</param>
	Protected Sub SaveXmlDocument(ByVal xDoc As XDocument, ByVal customerGuid As String)

		Dim ostJobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_OstJobCh")
		Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, customerGuid)
		Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

		Try

			If Not Directory.Exists(organsiationRootPath) Then
				Directory.CreateDirectory(organsiationRootPath)
			End If

			Dim xmlCode As String = String.Empty

			Using sw As StringWriter = New StringWriter()
				xDoc.Save(sw)
				xmlCode = sw.ToString()
			End Using

			'Using sw As New System.IO.StreamWriter(System.IO.File.Open(xmlFileName, FileMode.Create))
			'End Using

		Catch ex As Exception
			Dim msgContent = String.Format("ostJobChRoot: {1}{0}organsiationRootPath{2}{0}xmlFileName: {3}{0}{4}", vbNewLine, ostJobChRoot, organsiationRootPath, xmlFileName, ex.ToString)
			Dim utility As New ClsUtilities
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerGuid, .SourceModul = "SPOstJobsCHVacancies", .MessageHeader = "SaveXmlDocument", .MessageContent = msgContent})

		End Try


		Try
			xDoc.Save(xmlFileName)
		Catch ex As Exception
			Dim msgContent = String.Format("ostJobChRoot: {1}{0}organsiationRootPath{2}{0}xmlFileName: {3}{0}{4}", vbNewLine, ostJobChRoot, organsiationRootPath, xmlFileName, ex.ToString)
			Dim utility As New ClsUtilities
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerGuid, .SourceModul = "SPOstJobsCHVacancies", .MessageHeader = "SaveXmlDocument(2)", .MessageContent = msgContent})

			' Maybe the file is currently in use -> wait a little bit.
			Threading.Thread.Sleep(500)
			xDoc.Save(xmlFileName)
		End Try

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
	'  Dim connString As String = My.Settings.ConnStr_New_spContract
	'  Dim strSQL As String = String.Empty
	'  strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'  strSQL &= "CreatedOn) Values ("
	'  strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

	'  Dim Conn As SqlConnection = New SqlConnection(connString)
	'  Conn.Open()

	'  Try
	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'    cmd.CommandType = Data.CommandType.Text
	'    Dim param As System.Data.SqlClient.SqlParameter
	'    param = cmd.Parameters.AddWithValue("@Modulname", "Err: spOstJobsCHVacansices.asmx")
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

