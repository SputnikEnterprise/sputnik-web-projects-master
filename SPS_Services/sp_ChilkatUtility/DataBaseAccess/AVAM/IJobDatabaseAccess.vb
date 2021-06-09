
Imports sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects
Imports sp_WebServiceUtility.JobPlatform


Namespace WOSInfo


	''' <summary>
	''' Interface for WOS info database access.
	''' </summary>
	Public Interface IJobDatabaseAccess

		Function LoadAssignedQueryData(ByVal customerID As String, ByVal jobRoomID As String) As SPAVAMQueryResultData

		Function AddAVAMAdvertismentData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal notify As Boolean, ByVal avamData As SPAVAMJobCreationData) As Boolean
		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal avamData As SPAVAMJobCreationData) As Boolean
		Function UpdateAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal avamData As SPAVAMJobCreationData) As Boolean

	End Interface


End Namespace
