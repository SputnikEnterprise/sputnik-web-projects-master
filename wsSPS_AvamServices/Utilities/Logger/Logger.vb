Imports NLog

Namespace Logging

	''' <summary>
	''' The conrete logger. 
	''' </summary>
	Public Class Logger
		Implements ILogger

#Region "Private Fields"

		''' <summary>
		''' The NLog logger.
		''' </summary>
		Private m_Logger As NLog.Logger = LogManager.GetCurrentClassLogger()

#End Region

		''' <summary>
		''' Log info message
		''' </summary>
		''' <param name="errorMessage"></param>
		''' <remarks></remarks>
		Public Sub LogInfo(ByVal errorMessage As String) Implements ILogger.Loginfo
			m_Logger.Info(errorMessage)
		End Sub

		''' <summary>
		''' Log debug message
		''' </summary>
		''' <param name="errorMessage"></param>
		''' <remarks></remarks>
		Public Sub LogDebug(ByVal errorMessage As String) Implements ILogger.LogDebug
			m_Logger.Debug(errorMessage)
		End Sub

		''' <summary>
		''' Log error message.
		''' </summary>
		''' <param name="errorMessage">The error message.</param>
		Public Sub LogError(ByVal errorMessage As String) Implements ILogger.LogError
			m_Logger.Error(errorMessage)
		End Sub

		''' <summary>
		''' Log warning message.
		''' </summary>
		''' <param name="warningMessage">The warning message.</param>
		Public Sub LogWarning(ByVal warningMessage As String) Implements ILogger.LogWarning
			m_Logger.Warn(warningMessage)
		End Sub

	End Class
End Namespace