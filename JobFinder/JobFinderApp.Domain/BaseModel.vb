'------------------------------------
' File: BaseModel.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

''' <summary>
''' Base Class for models, that includes all commonly needed facilities.
''' </summary>
Public Class BaseModel

#Region "Protected Fields"

    ''' <summary>
    ''' The repository service.
    ''' </summary>
    Protected repositoryService As IRepositoryService

    ''' <summary>
    ''' The logging service.
    ''' </summary>
    Protected loggingService As ILoggingService

    ''' <summary>
    ''' The statistics service.
    ''' </summary>
    Protected statisticsService As IStatisticsService

#End Region

#Region "Constructor"

    Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService, ByVal statisticsService As IStatisticsService)
        Me.repositoryService = repositoryService
        Me.loggingService = loggingService
        Me.statisticsService = statisticsService
    End Sub

#End Region

End Class
