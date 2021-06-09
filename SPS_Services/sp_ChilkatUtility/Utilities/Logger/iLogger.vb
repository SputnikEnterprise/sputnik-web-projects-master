
Namespace Logging

	''' <summary>
	''' Logging interface.
	''' </summary>
	Public Interface ILogger
		Sub LogError(ByVal errorMessage As String)
		Sub LogWarning(ByVal warningMessage As String)

		Sub LogInfo(errorMessage As String)
		Sub LogDebug(errorMessage As String)

	End Interface
End Namespace
