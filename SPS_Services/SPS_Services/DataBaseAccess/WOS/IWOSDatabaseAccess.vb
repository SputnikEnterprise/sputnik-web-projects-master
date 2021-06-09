
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects

Namespace WOSInfo


	''' <summary>
	''' Interface for WOS info database access.
	''' </summary>
	Public Interface IWOSDatabaseAccess

		Function LoadAssignedWOSOwnerMasterData(ByVal customer_ID As String, ByVal WOSEnum As WOSModulData.ModulArt, ByVal withDocData As Boolean) As WOSOwnerData

		Function AddWOSCustomerDocumentData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal customerWosData As CustomerWOSData) As Boolean
		Function LoadAssignedCustomerWOSData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal modulNumber As Integer) As IEnumerable(Of CustomerWOSDataDTO)
		Function LoadAssignedCustomerWOSDataByDocArt(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal modulNumber As Integer, ByVal modulDocArt As Integer) As IEnumerable(Of CustomerWOSDataDTO)
		Function UpdateAssignedDocNotificationAsDone(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal recID As Integer) As Boolean

		Function LoadWOSEMailNotificationsData(ByVal customerWosGuid As String, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of WOSNotificationDTO)
		Function LoadMailNotificationData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of EMailNotificationDTO)
		Function LoadWOSModulEMailNotificationsData(ByVal customerWosGuid As String, ByVal modulNumber As Integer?, ByVal number As Integer?) As IEnumerable(Of WOSNotificationDTO)
		Function UpdateWOSCustomerUserData(ByVal customerID As String, ByVal userData As SystemUserData) As Boolean

		Function LoadWOSAvailableEmployeeData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal qualifications As String, ByVal location As String, ByVal canton As String, ByVal brunchOffice As String) As IEnumerable(Of AvailableEmployeeDTO)
		Function LoadAssignedAvailableEmployeeData(ByVal customerID As String, ByVal wosGuid As String, ByVal employeeNumber As Integer) As AvailableEmployeeDTO
		Function LoadAssignedAvailableEmployeeApplicationData(ByVal customerID As String, ByVal wosGuid As String, ByVal employeeNumber As Integer) As AvailableEmployeeApplicationFields

		Function AddWOSAvailableCandidatesData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult
		Function RemoveWOSAvailableCandidatesData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult

		Function AddWOSEmployeeDocumentData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal customerWosData As EmployeeWOSData) As Boolean
		Function LoadWOSAvailableEmployeeDocumentData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal employeeNumber As Integer) As IEnumerable(Of AvailableEmployeeTemplateData)
		Function LoadVacancyData(ByVal customerID As String, ByVal customerWOSID As String, ByVal kdNumber As Integer?, ByVal vakNumber As Integer?) As IEnumerable(Of KDVakanzenDTO)
		Function DeleteVacancyData(ByVal customerID As String, ByVal wosID As String, ByVal vakNumber As Integer) As Boolean


		Function AddInternalVacancyData(ByVal customerID As String, ByVal wosID As String, ByVal userGuid As String, ByVal CustomerData As CustomerUserData, ByVal vacancyData As DataTable) As Boolean

		Function LoadJobplattformCounterData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal jobsCHAccountNumber As Integer, ByVal ostJobAccountNumber As String) As JobplattformCounterDataDTO

	End Interface


End Namespace
