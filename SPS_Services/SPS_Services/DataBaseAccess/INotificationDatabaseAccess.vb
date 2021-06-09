
Imports wsSPS_Services.DataTransferObject.Notification.DataObjects

Namespace Notification


	''' <summary>
	''' Interface for Notification database access.
	''' </summary>
	Public Interface INotificationDatabaseAccess

		''' Notifications
		Function LoadApplicationNotifications(ByVal customerID As String) As IEnumerable(Of ApplicationDataDTO)
		Function LoadApplicantNotifications(ByVal customerID As String) As IEnumerable(Of ApplicantDataDTO)
		Function LoadAssignedApplicationsForApplicant(ByVal customerID As String, ByVal applicantID As Integer) As IEnumerable(Of ApplicationDataDTO)
		Function UpdateAssignedApplicationDataAsChecked(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicationNumber As Integer?, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Function UpdateAssignedApplicationData(ByVal customerID As String, ByVal applicationData As ApplicationDataDTO) As Boolean
		Function UpdateAssignedApplicantData(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Function UpdateAssignedApplicantWithExistingEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal newExistingEmployeeNumber As Integer) As Boolean
		Function UpdateApplicantCVLPersonalWithEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal applicantID As Integer, ByVal applicantData As ApplicantDataDTO) As Boolean
		Function UpdateAllDataForAssignedApplicantData(ByVal customerID As String, ByVal applicantID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean

		Function LoadAssignedDocumentsForApplicant(ByVal customerID As String, ByVal applicantID As Integer) As IEnumerable(Of ApplicantDocumentDataDTO)
		Function UpdateAssignedDocumentData(ByVal customerID As String, ByVal recordID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean



	End Interface


End Namespace
