'------------------------------------
' File: Statistics.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

Namespace Services

    ''' <summary>
    ''' Class used to handle the statistics.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class StatisticsService
        Implements IStatisticsService

        ''' <summary>
        ''' The repository service.
        ''' </summary>
        Private repositoryService As IRepositoryService

        ''' <summary>
        ''' The logging service.
        ''' </summary>
        Private loggingService As ILoggingService

#Region "Constructor"
        Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService)
            Me.repositoryService = repositoryService
            Me.loggingService = loggingService
        End Sub
#End Region

#Region "Public Properties for IStatistics"

        ''' <see cref="IStatisticsService.UpdateDownloadStatistics"/>
        Public Function UpdateDownloadStatistics(ByVal mandantGuid As String, ByVal appId As String) As Boolean Implements Contracts.IStatisticsService.UpdateDownloadStatistics
            ' Check for mandatory parameters.
            If String.IsNullOrEmpty(mandantGuid) Then
                Throw New ArgumentNullException("mandantGuid")
            End If

            If String.IsNullOrEmpty(appId) Then
                Throw New ArgumentNullException("appId")
            End If

            Try
                Me.repositoryService.UpdateDownloadStatistics(mandantGuid, appId)
            Catch ex As Exception
                Me.loggingService.Log("Error in updating download statistics. Message: " & ex.Message, LogLevel.Debug_Level)
                Return False
            End Try

            Return True
        End Function

        ''' <see cref="IStatisticsService.UpdateVacancyQueryStatistics"/>
        Public Function UpdateVacancyQueryStatistics(ByVal mandantGuid As String,
                                              ByVal cantonFilter As String,
                                              ByVal branchFilter As String,
                                              ByVal vacancyTitleFilter As String,
                                              ByVal clientLatitude As String,
                                              ByVal clientLongitude As String) As Boolean Implements Contracts.IStatisticsService.UpdateVacancyQueryStatistics

            If String.IsNullOrEmpty(mandantGuid) Then
                Throw New ArgumentNullException("mandantGuid")
            End If

            Try
                Me.repositoryService.UpdateVacancyQueryStatistics(mandantGuid, cantonFilter, branchFilter, vacancyTitleFilter, clientLatitude, clientLongitude)
            Catch ex As Exception
                Me.loggingService.Log("Error in updating query statistics. Message: " & ex.Message, LogLevel.Debug_Level)
                Return False
            End Try

            Return True

        End Function

        ''' <see cref="IStatisticsService.UpdateCandidateQueryStatistics"/>
        Public Function UpdateCandidateQueryStatistics(ByVal mandantGuid As String,
                                                       ByVal jobQualificationSearchValue As String,
                                                       ByVal clientLatitude As String,
                                                       ByVal clientLongitude As String) As Boolean Implements Contracts.IStatisticsService.UpdateCandidateQueryStatistics

            If String.IsNullOrEmpty(mandantGuid) Then
                Throw New ArgumentNullException("mandantGuid")
            End If

            Try
                Me.repositoryService.UpdateCandidateQueryStatistics(mandantGuid, jobQualificationSearchValue, clientLatitude, clientLongitude)
            Catch ex As Exception
                Me.loggingService.Log("Error in updating query statistics. Message: " & ex.Message, LogLevel.Debug_Level)
                Return False
            End Try

            Return True

        End Function

#End Region

    End Class

End Namespace
