
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.IO
Imports wsSPS_Services.JobPlatform.JobsCH
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.JobPlatform.AVAM
Imports wsSPS_Services.WOSInfo
Imports wsSPS_Services.Logging

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPInternVacancies.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPInternVacancies
	Inherits System.Web.Services.WebService


#Region "private consts"

	Private Const ASMX_SERVICE_NAME As String = "SPInternVacancies"

#End Region


	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess
	Private m_PublicData As PublicDataDatabaseAccess
	Private m_AVAMData As AVAMDataDatabaseAccess
	Private m_NewWOS As WOSDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)
		m_AVAMData = New AVAMDataDatabaseAccess(My.Settings.ConnStr_spJobplattforms, Language.German)
		m_NewWOS = New WOSDatabaseAccess(My.Settings.ConnStr_New_spContract, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


#Region "jobs.ch common data"

	<WebMethod()>
	Function LoadJobCHBerufeData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHOccupationsData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHBerufeData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHFachbereichData(ByVal customerID As String, ByVal parentID As Integer, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHFachbereichData(m_customerID, parentID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHFachbereichData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHRegionData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHRegionData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHRegionData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHBranchenData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHBranchesData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHBranchenData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHLanguageData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHLanguagesData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHLanguageData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHLanguageNiveauData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHLanguageNiveauData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHLanguageNiveauData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHPositionData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHPositionData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHPositionData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadJobCHBildungNiveauData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadJobCHBildungNiveauData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHBildungNiveauData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

#End Region


#Region "avam jobs"

	<WebMethod()>
	Function LoadAVAMEducationData(ByVal customerID As String, ByVal language As String) As VacancyJobCHPeripheryDTO()
		Dim result As List(Of VacancyJobCHPeripheryDTO) = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("LoadAVAMEducationData: customerID: {0} | language: {1}", customerID, language))
			result = New List(Of VacancyJobCHPeripheryDTO)
			result = m_PublicData.LoadAVAMEducationsuData(m_customerID, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAVAMEducationData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod()>
	Function LoadSTMPAllJobTitleData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As STMPJobData()
		Dim result As List(Of STMPJobData) = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("LoadSTMPAllJobTitleData: customerID: {0} | userID: {1} | jobNumber: {2} | language: {3}", customerID, userID, jobNumber, language))
			result = New List(Of STMPJobData)
			result = m_PublicData.LoadSTMPJobData(m_customerID, userID, jobNumber, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSTMPAllJobTitleData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Function LoadOccupation2020Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As STMPJobData()
		Dim result As List(Of STMPJobData) = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("LoadOccupation2020Data: customerID: {0} | userID: {1} | jobNumber: {2} | language: {3}", customerID, userID, jobNumber, language))
			result = New List(Of STMPJobData)
			result = m_PublicData.LoadSTMPJob2020Data(m_customerID, userID, jobNumber, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadOccupation2020Data", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Function LoadOccupation2021Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As STMPJobData()
		Dim result As List(Of STMPJobData) = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("LoadOccupation2020Data: customerID: {0} | userID: {1} | jobNumber: {2} | language: {3}", customerID, userID, jobNumber, language))
			result = New List(Of STMPJobData)
			result = m_PublicData.LoadSTMPJob2021Data(m_customerID, userID, jobNumber, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadOccupation2021Data", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Function LoadOccupationMappingData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As STMPMappingData()
		Dim result As List(Of STMPMappingData) = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("LoadOccupationMappingData: customerID: {0} | userID: {1} | jobNumber: {2} | language: {3}", customerID, userID, jobNumber, language))
			result = New List(Of STMPMappingData)
			result = m_PublicData.LoadSTMPMappingData(m_customerID, userID, jobNumber, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadOccupationMappingData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Function AddAVAMCreationDataToDatabase(ByVal customerID As String, ByVal userID As String, ByVal vacancyNumber As Integer, ByVal jobRoomID As String, ByVal queryContent As String,
										   ByVal resultContent As String, ByVal syncFrom As String,
										   ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?,
										   ByVal Notify As Boolean?, ByVal language As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("AddAVAMCreationDataToDatabase: customerID: {0} | userID: {1} | jobRoomID: {2} | syncFrom: {3}", customerID, userID, jobRoomID, syncFrom))
			result = m_AVAMData.AddAVAMNotifyResultData(m_customerID, userID, vacancyNumber, jobRoomID, queryContent, resultContent, ReportingObligation, ReportingObligationEndDate, Notify, syncFrom)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMCreationDataToDatabase", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function

	<WebMethod()>
	Function AddAVAMQueryResultDataToDatabase(ByVal customerID As String, ByVal userID As String, ByVal vacancyNumber As Integer, ByVal jobRoomID As String, ByVal queryContent As String, ByVal resultContent As String, ByVal syncFrom As String,
								   ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?, ByVal language As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("AddAVAMQueryResultDataToDatabase: customerID: {0} | userID: {1} | jobRoomID: {2} | syncFrom: {3}", customerID, userID, jobRoomID, syncFrom))
			result = m_AVAMData.AddAVAMQueryResultData(m_customerID, userID, vacancyNumber, jobRoomID, queryContent, resultContent, ReportingObligation, ReportingObligationEndDate, syncFrom)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMQueryResultDataToDatabase", .MessageContent = msgContent})
		Finally
		End Try


		Return result

	End Function

#End Region


	<WebMethod(Description:="Speichert Vakanzdaten in die interne Datenbanken.")>
	Function AddAssignedVacancyForInternalJobplattform(ByVal customerID As String, ByVal customerWOSID As String, ByVal userGuid As String, ByVal CustomerData As CustomerUserData, ByVal vacancyData As DataTable) As WebServiceResult
		Dim result As New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("AddAssignedVacancyForInternalJobplattform: customerID: {0} | customerWOSID: {1} | userGuid: {2}", customerID, customerWOSID, userGuid))
			If (vacancyData Is Nothing) Then
				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "missing vacancyData"}
			ElseIf String.IsNullOrWhiteSpace(customerID) Then
				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "missing customerID"}
			ElseIf String.IsNullOrWhiteSpace(customerWOSID) Then
				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "missing customerWOSID"}
			End If

			If Not vacancyData.Rows.Count = 1 Then
				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "more data is available"}
			End If

			Dim success = m_NewWOS.AddInternalVacancyData(customerID, customerWOSID, userGuid, CustomerData, vacancyData)
			result.JobResult = success

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}m_customerID: {2} | customerWOSID: {3}", vbNewLine, ex.ToString, m_customerID, customerWOSID)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAssignedVacancyForInternalJobplattform", .MessageContent = msgContent})

			result.JobResult = False
			result.JobResultMessage = ex.ToString
		Finally
		End Try

		Return (result)
	End Function

	'<WebMethod(Description:="shows number of published vacancies.")>
	'Function LoadNumberOfPublishedVacancyEachJobplattforms(ByVal customerID As String, ByVal customerWOSID As String, ByVal userGuid As String, ByVal CustomerData As CustomerUserData) As NumberOfVacancyData
	'	Dim result As New NumberOfVacancyData With {.JobResult = True, .JobResultMessage = String.Empty}
	'	m_customerID = customerID

	'	Try
	'		m_Logger.LogInfo(String.Format("AddAssignedVacancyForInternalJobplattform: customerID: {0} | customerWOSID: {1} | userGuid: {2}", customerID, customerWOSID, userGuid))
	'		If String.IsNullOrWhiteSpace(customerID) Then
	'			Return New NumberOfVacancyData With {.CustomerID = String.Empty, .NumberOfInternal = 0, .NumberOfJobCH = 0, .NumberOfJobChannel = 0, .NumberOfOstJob = 0}

	'		ElseIf String.IsNullOrWhiteSpace(customerWOSID) Then
	'			Return New NumberOfVacancyData With {.WOS_Guid = String.Empty, .NumberOfInternal = 0, .NumberOfJobCH = 0, .NumberOfJobChannel = 0, .NumberOfOstJob = 0}
	'		End If


	'		Dim success = m_NewWOS.AddInternalVacancyData(customerID, customerWOSID, userGuid, CustomerData, vacancyData)
	'		result.JobResult = success

	'	Catch ex As Exception
	'		Dim msgContent = String.Format("{1}{0}m_customerID: {2} | customerWOSID: {3}", vbNewLine, ex.ToString, m_customerID, customerWOSID)
	'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAssignedVacancyForInternalJobplattform", .MessageContent = msgContent})

	'		result.JobResult = False
	'		result.JobResultMessage = ex.ToString
	'	Finally
	'	End Try

	'	Return (result)
	'End Function


#Region "Helper methodes"

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

#End Region


End Class

