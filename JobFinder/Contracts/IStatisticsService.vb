'------------------------------------
' File: IStatisticsService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Statistics interface.
''' </summary>
Public Interface IStatisticsService

    ''' <summary>
    ''' This method updates the download statistics.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="appId">The application specific id. Each installed application should have its unique id.</param>
    ''' <returns>True if the statistics could be updated, false otherwise.</returns>
    Function UpdateDownloadStatistics(ByVal mandantGuid As String, ByVal appId As String) As Boolean


    ''' <summary>
    ''' This method updates the query statistics.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="cantonFilter">The used canton filter.</param>
    ''' <param name="branchFilter">The used branch filter.</param>
    ''' <param name="vacancyTitleFilter">The used vacancy title filter.</param>
    ''' <returns>True if the statistics could be updated, false otherwise.</returns>
    Function UpdateVacancyQueryStatistics(ByVal mandantGuid As String, ByVal cantonFilter As String, ByVal branchFilter As String, ByVal vacancyTitleFilter As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As Boolean

    ''' <summary>
    ''' This method updates the query statistics.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="jobQualificationSearchValue">The used canton filter.</param>
    ''' <returns>True if the statistics could be updated, false otherwise.</returns>
    Function UpdateCandidateQueryStatistics(ByVal mandantGuid As String, ByVal jobQualificationSearchValue As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As Boolean

End Interface
