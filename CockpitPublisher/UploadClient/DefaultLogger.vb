'------------------------------------
' File: DefaultLogger.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Reflection
Imports UploadClient.CockpitWCFServices
Imports log4net

''' <summary>
''' Default logger
''' </summary>
Public Class DefaultLogger
    Implements ILogger

    '--Fields--

    Private m_UploadService As ITableDataUploadService

    ''' <summary>
    ''' Log4Net logger
    ''' </summary>
    Private logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    '--Methods--

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(ByVal uploadService As ITableDataUploadService)
        m_UploadService = uploadService
    End Sub

    ''' <summary>
    ''' Writes a message to the console.
    ''' </summary>
    ''' <param name="message">The message text</param>
    Public Sub LogMessageLocal(ByVal message As String) Implements ILogger.LogMessageLocal
        logger.Info(message)
    End Sub

    ''' <summary>
    ''' Logs an error on the server.
    ''' </summary>
    ''' <param name="errorMessage">The error message</param>
    ''' <param name="stringException">The exception text</param>
    Public Sub LogErrorRemote(ByVal errorMessage As String, ByVal stringException As String) Implements ILogger.LogErrorRemote

        LogMessageLocal(String.Format("{0}. Exception={1}", errorMessage, stringException))

        m_UploadService.LogError(errorMessage, stringException, My.Settings.CustomerName)
    End Sub

End Class
