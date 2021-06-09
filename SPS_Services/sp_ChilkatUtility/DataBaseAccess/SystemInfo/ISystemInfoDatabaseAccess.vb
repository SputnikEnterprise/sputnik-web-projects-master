

Imports sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects

Namespace SystemInfo


	''' <summary>
	''' Interface for Systeminfo database access.
	''' </summary>
	Public Interface ISystemInfoDatabaseAccess

		Function AddErrorMessage(ByVal customerID As String, ByVal msgData As ErrorMessageData) As Boolean
		Function AddNotifyMessage(ByVal customerID As String, ByVal msgData As ErrorMessageData) As Boolean

	End Interface


End Namespace
