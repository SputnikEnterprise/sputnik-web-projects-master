
Imports System.Web.Services
Imports System.ComponentModel
Imports System.IO
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.JobPlatform.X28
Imports wsSPS_Services.CVLizer
Imports System.Net
Imports System.Net.Http.Headers


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPX28Profilmatcher.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPX28Profilmatcher
	Inherits System.Web.Services.WebService

#Region "private consts"

	Private Const ASMX_SERVICE_NAME As String = "SPX28Profilmatcher"

	Private Const X28_RM_WEB_REQUEST_URL As String = "https://api.x28.ch/search/api/jobs/xml"
	Private Const X28_RM_API_USERNAME As String = "sputnikit"
	Private Const X28_RM_API_USERPASSWORD As String = "XhDJxv!hc*qW5hNBj9x4"

#End Region


	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess
	Private m_CVLInfo As CVLizerDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_CVLInfo = New CVLizerDatabaseAccess(My.Settings.ConnStr_cvLizer, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function

	<WebMethod()>
	Function LoadPMQueryNotifications(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?) As ProfilMatcherNotificationData()
		Dim result As List(Of ProfilMatcherNotificationData) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of ProfilMatcherNotificationData)
			result = m_CVLInfo.LoadProfilmatcherQueryNotifications(m_customerID, userID, customerNumber, employeeNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPMQueryNotifications", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Function LoadPMQueryAssignedNotifications(ByVal customerID As String, ByVal id As Integer) As ProfilMatcherNotificationData
		Dim result As ProfilMatcherNotificationData = Nothing
		m_customerID = customerID

		Try
			result = New ProfilMatcherNotificationData
			result = m_CVLInfo.LoadProfilmatcherAssignedQueryNotification(m_customerID, id)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPMQueryAssignedNotifications", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function

	<WebMethod()>
	Function SavePMQueryNotificationData(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?, ByVal notify As Boolean,
																			 ByVal createdFrom As String, ByVal tplName As String, ByVal pmQueryString As String, ByVal pmQueryData As ProfilmatcherQueryData,
																			 ByVal pmJobResult As ProfilmatcherQueryResultData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = m_CVLInfo.AddProfilmatcherQueryNotificationData(customerID, userID, customerNumber, employeeNumber, tplName,
																									 pmJobResult.Total, notify, createdFrom,
																									 pmQueryString, pmJobResult.ResultContent)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "SavePMQueryNotificationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Function DeletePMQueryAssignedNotificationData(ByVal customerID As String, ByVal ID As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = m_CVLInfo.DeleteProfilmatcherAssignedNotificationData(customerID, ID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeletePMQueryAssignedNotificationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Function BuildPMQueryString(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As String

		Dim result As String = String.Empty
		m_customerID = customerID

		Try
			result = String.Empty

			Dim vacanciesGenerator As New Profilmatcher()
			Dim xDoc = vacanciesGenerator.GenerateVacanciesXml(queryData)
			SaveXmlDocument(xDoc)

			result = xDoc.ToString()


		Catch ex As Exception
			Dim msgContent = String.Format("{0}", ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildPMQueryString", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)

	End Function

	<WebMethod(Description:="Speichert OstJobs.ch Daten in die interne Datenbanken.")>
	Function LoadPMQueryResultData(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As ProfilmatcherQueryResultData

		Dim result As ProfilmatcherQueryResultData = Nothing
		m_customerID = customerID

		Try
			result = New ProfilmatcherQueryResultData
			Dim xmlQueryFile = BuildQueryFileName(queryData)

			Dim content As String = ""
			Using textReader As New System.IO.StreamReader(xmlQueryFile)
				content = textReader.ReadToEnd
			End Using
			Dim wsResult = LoadWebserviceProcess(content)
			Dim xmlData = New ParsX28ProfilMatcherXMLData(wsResult.APIResult) 'xmlQueryFile)

			result = xmlData.LoadProfilMatcherResultData()


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPMQueryData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)

	End Function

	<WebMethod()>
	Function LoadPMQueryResultDa(ByVal customerID As String, ByVal userID As String, ByVal queryData As ProfilmatcherQueryData) As String

		Dim result As String = String.Empty
		m_customerID = customerID

		Try
			result = String.Empty

			Dim vacanciesGenerator As New Profilmatcher()
			Dim xDoc = vacanciesGenerator.GenerateVacanciesXml(queryData)
			SaveXmlDocument(xDoc)

			result = xDoc.ToString()


		Catch ex As Exception
			Dim msgContent = String.Format("{0}", ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .CreatedFrom = userID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildPMQueryString", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)

	End Function



#Region "private methodes"

	Private Function BuildQueryFileName(ByVal queryData As ProfilmatcherQueryData) As String
		Dim result As String = String.Empty

		Try
			Dim vacanciesGenerator As New Profilmatcher()
			Dim xDoc = vacanciesGenerator.GenerateVacanciesXml(queryData)

			result = SaveXmlDocument(xDoc)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "BuildQueryFileName", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function

	Protected Function SaveXmlDocument(ByVal xDoc As XDocument) As String

		Dim ostJobChRoot = System.Configuration.ConfigurationManager.AppSettings("X28PMQueryPath")
		Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, m_customerID)
		Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, String.Format("QueryString_{0}.xml", Environment.TickCount))

		Try
			If File.Exists(xmlFileName) Then File.Delete(xmlFileName)
		Catch ex As Exception
			Threading.Thread.Sleep(500)
			File.Delete(xmlFileName)
		End Try

		If Not Directory.Exists(organsiationRootPath) Then
			Directory.CreateDirectory(organsiationRootPath)
		End If

		Dim xmlCode As String = String.Empty

		Using sw As StringWriter = New StringWriter()
			xDoc.Save(sw)
			xmlCode = sw.ToString()
		End Using


		Try
			xDoc.Save(xmlFileName)
		Catch ex As Exception
			' Maybe the file is currently in use -> wait a little bit.
			Threading.Thread.Sleep(500)
			xDoc.Save(xmlFileName)
		End Try


		Return xmlFileName
	End Function

	Private Function LoadWebserviceProcess(ByVal sb As String) As ProfilmatcherWebserviceResult ' HttpWebResponse
		Dim baseUri As Uri = New Uri(X28_RM_WEB_REQUEST_URL)
		Dim result = New ProfilmatcherWebserviceResult

		result.APIResponse = String.Empty
		result.APIResult = String.Empty

		'response = clsWebserviceProcess(PMQueryStringToSend, baseUri, "Post", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD)

		Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(
						System.Text.ASCIIEncoding.ASCII.GetBytes(
								String.Format("{0}:{1}", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD))))

		Dim xmlBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(sb)

		Dim req As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(baseUri), System.Net.HttpWebRequest)
		req.Method = "POST"
		req.ContentType = "application/xml"
		req.Headers.Add("Authorization", authHeader.ToString)

		req.ContentLength = xmlBytes.Length
		Dim post As System.IO.Stream = req.GetRequestStream
		post.Write(xmlBytes, 0, xmlBytes.Length)

		Dim value As String = Nothing
		Dim resp As System.Net.HttpWebResponse = CType(req.GetResponse, System.Net.HttpWebResponse)
		Dim reader As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream)
		value = reader.ReadToEnd
		reader.Close()

		result.HttpState = CStr(resp.StatusCode)

		result.APIResponse = result.HttpState
		result.APIResult = value

		If (resp.StatusCode <> HttpStatusCode.OK AndAlso resp.StatusCode <> HttpStatusCode.NoContent AndAlso resp.StatusCode <> HttpStatusCode.Created) Then
			result.APIResult = String.Empty
			result.APIResponse = resp.StatusCode

			Return result
		End If


		Return result

	End Function


#End Region


End Class

