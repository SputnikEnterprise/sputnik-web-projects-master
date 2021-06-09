'------------------------------------
' File: ILogger.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Interface for logger implementations
''' </summary>
Public Interface ILogger

    ''' <summary>
    ''' Writes a message to the console.
    ''' </summary>
    ''' <param name="message">The message text</param>
    Sub LogMessageLocal(ByVal message As String)


    ''' <summary>
    ''' Logs an error on the server.
    ''' </summary>
    ''' <param name="errorMessage">The error message</param>
    ''' <param name="stringException">The exception text</param>
    Sub LogErrorRemote(ByVal errorMessage As String, ByVal stringException As String)
End Interface
