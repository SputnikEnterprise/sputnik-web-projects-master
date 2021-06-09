
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects

Namespace DocumentScan


	''' <summary>
	''' Interface for DocumentScan database access.
	''' </summary>
	Public Interface IScanDatabaseAccess

		Function LoadCustomerDataForScanjobList() As IEnumerable(Of MandantData)
		Function AddAssignedScanJob(ByVal customerID As String, ByVal scanID As String) As ScanDTO
		Function LoadScanJobData(ByVal customerID As String, ByVal scanID As Integer?, ByVal assignedDate As DateTime?) As IEnumerable(Of ScanAttachmentDTO)
		Function LoadAssignedScanJobData(ByVal customerID As String, ByVal scanID As Integer?) As ScanAttachmentDTO
		Function UpdateAssignedScanJob(ByVal customerID As String, ByVal scanID As String, ByVal userData As String) As Boolean
		Function AddReportDropInJob(ByVal customerID As String, ByVal scanData As ScanDropInDTO) As Boolean
		Function AddCVDropInJob(ByVal customerID As String, ByVal UserData As SystemUserData, ByVal scanData As ScanDropInDTO) As Boolean
		Function LoadScanJobsNotificationsData(ByVal customerID As String, ByVal excludeChecked As Boolean?) As IEnumerable(Of ScanDTO)
		Function LoadNotificationEMailData(ByVal customerID As String) As EMailNotificationData


	End Interface


End Namespace
