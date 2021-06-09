'------------------------------------
' File: Log4NetLoggingService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------
Imports JobFinderApp.Contracts

Imports log4net
Imports log4net.Config
Imports log4net.Core

Namespace Services

    Public Class Log4NetLoggingService
        Implements ILoggingService

#Region "Private Fields"

        ''' <summary>
        ''' The logger.
        ''' </summary>
        Private Shared ReadOnly logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Constructor"

        Public Sub New()
            ' Configure log4net
            XmlConfigurator.Configure()
        End Sub

#End Region

#Region "Public methods"

        ''' <see cref="ILoggingService.Log" />
        Public Sub Log(message As String, log As LogLevel) Implements ILoggingService.Log

            Select Case log
                Case LogLevel.Info_Level
                    Log4NetLoggingService.logger.Info(message)
                Case LogLevel.Debug_Level
                    Log4NetLoggingService.logger.Debug(message)
                Case LogLevel.Error_Level
                    Log4NetLoggingService.logger.Error(message)
                Case Else
                    ' Do noting
            End Select
        End Sub

#End Region

    End Class

End Namespace