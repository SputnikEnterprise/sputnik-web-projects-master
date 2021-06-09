
Imports System.Web.Services
Imports System.ComponentModel

Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DocumentScan
Imports System.IO
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.Logging



' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPScanJobUtility.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPScanJobUtility
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPScanJobUtility"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_Scan As ScanDatabaseAccess
	Private m_DatabaseAccess As DatabaseAccessBase

	Private m_ReportScanFileName As String
	Private m_CVLScanFileName As String


	Public Sub New()

		m_utility = New ClsUtilities
		m_Scan = New ScanDatabaseAccess(My.Settings.ConnStr_Scanning, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


	<WebMethod(Description:="Get assigned scan data")>
	Function GetAssignedScanJob(ByVal customerID As String, ByVal scanID As String) As ScanDTO

		Dim result As ScanDTO = Nothing
		m_customerID = customerID

		Try
			result = New ScanDTO
			result = m_Scan.AddAssignedScanJob(customerID, scanID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("GetAssignedScanJob:{0}customerID: {1}{0}scanID: {2}{0}{3}", vbNewLine, m_customerID, scanID, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetAssignedScanJob", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update scan job record as checked")>
	Function UpdateAssignedScanJobs(ByVal customerID As String, ByVal scanID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_Scan.UpdateAssignedScanJob(customerID, scanID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("UpdateAssignedScanJobs:{0}customerID: {1}{0}scanID: {2}{0}userData: {3}{0}{4}", vbNewLine, m_customerID, scanID, userData, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedScanJobs", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Report Drop-In")> _
	Function GetReportDropInJob(ByVal customerID As String, ByVal scanData As ScanDropInDTO) As Boolean

		Dim result As Boolean = True
		m_customerID = customerID

		' Read all data.
		If scanData.ScanContent Is Nothing Then Return False
		Try

			result = result AndAlso CreateReportContentIntoFileSystem(scanData)
			result = result AndAlso m_Scan.AddReportDropInJob(customerID, scanData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("GetReportDropInJob:{0}customerID: {1}{0}ScanFileName: {2}{0}{3}", vbNewLine, m_customerID, scanData.ScanFileName, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetReportDropInJob", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="CV Drop-In")>
	Function GetCVDropInJob(ByVal customerID As String, ByVal scanData As ScanDropInDTO) As Boolean
		Dim result As Boolean = True
		Dim msgContent As String = String.Empty
		m_customerID = customerID

		' Read all data.
		If scanData.ScanContent Is Nothing Then Return False

		Try
			msgContent = String.Format("{0}: function not activ any more!!!", customerID)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCVDropInJob", .MessageContent = msgContent})

		Catch ex As Exception
			msgContent = ex.ToString
			m_Logger.LogError(String.Format("GetCVDropInJob:{0}customerID: {1}{0}ScanFileName: {2}{0}{3}", vbNewLine, m_customerID, scanData.ScanFileName, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCVDropInJob", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="CVL Drop-In")>
	Function GetCVLDropInJob(ByVal customerID As String, ByVal UserData As SystemUserData, ByVal scanData As ScanDropInDTO) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		' Read all data.
		If scanData.ScanContent Is Nothing Then Return False

		Try
			result = result AndAlso CreateCVContentIntoFileSystem(scanData)
			result = result AndAlso m_Scan.AddCVDropInJob(customerID, UserData, scanData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("GetCVLDropInJob:{0}customerID: {1}{0}ScanFileName: {2}{0}UserData.UserFullName: {3}{0}{4}", vbNewLine, m_customerID, scanData.ScanFileName, UserData.UserFullName, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCVLDropInJob", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Get all scan data from attachment")>
	Function LoadAllScanJobData(ByVal customerID As String, ByVal scanID As Integer, ByVal assignedDate As DateTime?) As ScanAttachmentDTO()

		Dim result As List(Of ScanAttachmentDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of ScanAttachmentDTO)
			result = m_Scan.LoadScanJobData(customerID, scanID, assignedDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("LoadAllScanJobData:{0}customerID: {1}{0}scanID: {2}{0}assignedDate: {3}{0}{4}", vbNewLine, m_customerID, scanID, assignedDate, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllScanJobData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Get assigned scan data from attachment")>
	Function LoadAssignedScanJobData(ByVal customerID As String, ByVal scanID As Integer) As ScanAttachmentDTO

		Dim result As ScanAttachmentDTO = Nothing
		m_customerID = customerID

		Try
			result = New ScanAttachmentDTO
			result = m_Scan.LoadAssignedScanJobData(customerID, scanID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("LoadAssignedScanJobData:{0}customerID: {1}{0}scanID: {2}{0}{3}", vbNewLine, m_customerID, scanID, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedScanJobData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function



#Region "helpers"

	Private Function CreateReportContentIntoFileSystem(ByVal scanData As ScanDropInDTO) As Boolean
		Dim success As Boolean = True
		Dim scanFolderToWatch As String = My.Settings.ReportDropInFolder
		Dim tempFileName = System.IO.Path.GetTempFileName()

		Try
			Dim bytes() = scanData.ScanContent
			If String.IsNullOrWhiteSpace(scanData.FileExtension) Then scanData.FileExtension = "PDF"
			tempFileName = System.IO.Path.ChangeExtension(tempFileName, scanData.FileExtension)

			success = success AndAlso (Not bytes Is Nothing) AndAlso m_utility.WriteFileBytes(tempFileName, bytes)

			Dim fileNametoMove As String = Path.Combine(scanFolderToWatch, m_customerID, New FileInfo(tempFileName).Name)
			If Not String.IsNullOrWhiteSpace(scanFolderToWatch) Then File.Move(tempFileName, fileNametoMove)
			scanData.ScanFileName = fileNametoMove

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("GetReportDropInJob:{0}customerID: {1}{0}scanFolderToWatch: {2}{0}tempFileName: {3}{0}{4}", vbNewLine, m_customerID, scanFolderToWatch, tempFileName, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CreateReportContentIntoFileSystem", .MessageContent = msgContent})
			success = False

		End Try


		Return success

	End Function

	Private Function CreateCVContentIntoFileSystem(ByVal scanData As ScanDropInDTO) As Boolean
		Dim success As Boolean = True
		Dim scanFolderToWatch As String = My.Settings.CVDropInFolder
		Dim tempFileName = System.IO.Path.GetTempFileName()

		Try
			Dim bytes() = scanData.ScanContent
			If String.IsNullOrWhiteSpace(scanData.FileExtension) Then scanData.FileExtension = "PDF"
			tempFileName = System.IO.Path.ChangeExtension(tempFileName, scanData.FileExtension)

			success = success AndAlso (Not bytes Is Nothing) AndAlso m_utility.WriteFileBytes(tempFileName, bytes)

			Dim fileNametoMove As String = Path.Combine(scanFolderToWatch, m_customerID, New FileInfo(tempFileName).Name)
			If Not String.IsNullOrWhiteSpace(scanFolderToWatch) Then File.Move(tempFileName, fileNametoMove)
			scanData.ScanFileName = fileNametoMove


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("CreateCVContentIntoFileSystem:{0}customerID: {1}{0}scanFolderToWatch: {2}{0}tempFileName: {3}{0}{4}", vbNewLine, m_customerID, scanFolderToWatch, tempFileName, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CreateCVContentIntoFileSystem", .MessageContent = msgContent})
			success = False

		End Try


		Return success

	End Function


#End Region


End Class