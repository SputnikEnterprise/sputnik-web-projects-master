'------------------------------------
' File: ILoggingService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Public Enum LogLevel
    Info_Level
    Debug_Level
    Error_Level
End Enum


Public Interface ILoggingService
    ''' <summary>
    ''' Writes a message to the log.
    ''' </summary>
    ''' <param name="message">The message text</param>
    ''' <param name="logLevel">The log level</param>
    Sub Log(ByVal message As String, logLevel As LogLevel)
End Interface
