'------------------------------------
' File: IRepositoryService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Collections.Specialized

Public Interface IRepositoryService

    ''' <summary>
    ''' Reads the mandant informations by a given mandant guid.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <returns>The mandant informations.</returns>
    Function ReadMandantDetails(ByVal mandantGuid As String) As IMandantDetails

    ''' <summary>
    ''' Retreives the mandant icon to a given mandant guid.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <returns>The mandant icon byte array.</returns>
    Function ReadMandantIcon(ByVal mandantGuid As String) As Byte()

    ''' <summary>
    ''' Retreives the berater picture.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="userId">The user id of berater.</param>
    ''' <returns>The mandant icon byte array.</returns>
    Function ReadUserPicture(ByVal mandantGuid As String, ByVal userId As String) As Byte()

    ''' <summary>
    ''' Reads the list of cantons which have vacacencies.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <returns>List of canton abbreviations that have vacacencies.</returns>
    Function ReadCantonAbbreviations(ByVal mandantGuid As String) As IEnumerable(Of String)

    ''' <summary>
    ''' Reads the branches filtered by canton.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="cantonAbbreviation">The canton abbreviation. If this parameter is Nothing, no filtering happens, and all cantons are returned.</param>
    ''' <returns>The list of branches, that have vacancies.</returns>
    Function ReadBranches(ByVal mandantGuid As String,
                          Optional cantonAbbreviation As String = Nothing) As IEnumerable(Of String)

    ''' <summary>
    ''' Reads vacancy descriptions.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="cantonAbbreviation">The canton abbreviation.</param>
    ''' <param name="branch">The branch.</param>
    ''' <param name="vacancyText">The vacancyText.</param>
    ''' <returns>List of vacancies.</returns>
    Function ReadVacancyDescriptions(ByVal mandantGuid As String,
                                     Optional ByVal cantonAbbreviation As String = Nothing,
                                     Optional ByVal branch As String = Nothing,
                                     Optional ByVal vacancyText As String = Nothing) As IEnumerable(Of ISearchItem)

    ''' <summary>
    ''' Reads candidate descriptions.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="jobQualificationSearchValue">The candidate job qualification.</param>
    ''' <returns>List of candidates.</returns>
    Function ReadCandidateDescriptions(ByVal mandantGuid As String,
                                       Optional ByVal jobQualificationSearchValue As String = Nothing) As IEnumerable(Of ISearchItem)

    ''' <summary>
    ''' Reads vacancy detail values.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="vacancyId">The vacancy id.</param>
    ''' <returns>Column values as key value pairs or nothing in error case.</returns>
    ''' <remarks>If a column value could not be read it is not included in the result dictionary.</remarks>
    Function ReadVacancyDetails(ByVal mandantGuid As String,
                                ByVal vacancyId As Integer) As OrderedDictionary

    ''' <summary>
    ''' Reads the candidate job qualifications of candidates that belong to a mandant.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <returns>The list of candidate job qualifications.</returns>
    Function ReadJobQualifications(ByVal mandantGuid As String) As IEnumerable(Of String)

    ''' <summary>
    ''' Reads candidate detail values.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="candidateId">The candidate id.</param>
    ''' <returns>Column values as key value pairs or nothing in error case.</returns>
    ''' <remarks>If a column value could not be read it is not included in the result dictionary.</remarks>
    Function ReadCandidateDetails(ByVal mandantGuid As String,
                                  ByVal candidateId As Integer) As OrderedDictionary


    ''' Statistics **********************************************************************************************************************
    ''' 
    ''' <summary>
    ''' Updates the download statistics of the corresponding Mandant, using an unique application identifier.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="appId">A unique application id. The same client should always produce the same application id.</param>
    Sub UpdateDownloadStatistics(ByVal mandantGuid As String, ByVal appId As String)

    ''' <summary>
    ''' This method updates the query statistics for a job search.
    ''' Updates the query statistics of the corresponding Mandant, using an unique application identifier.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="cantonFilter">The used canton filter.</param>
    ''' <param name="branchFilter">The used branch filter.</param>
    ''' <param name="vacancyTitleFilter">The used vacancy title filter.</param>
    Sub UpdateVacancyQueryStatistics(ByVal mandantGuid As String, ByVal cantonFilter As String, ByVal branchFilter As String, ByVal vacancyTitleFilter As String, ByVal clientLatitude As String, ByVal clientLongitude As String)

    ''' <summary>
    ''' This method updates the query statistics for a candidate search.
    ''' </summary>
    ''' <param name="mandantGuid">The mandant guid.</param>
    ''' <param name="jobQualificationSearchValue">The used job qualification search filter.</param>
    Sub UpdateCandidateQueryStatistics(ByVal mandantGuid As String, ByVal jobQualificationSearchValue As String, ByVal clientLatitude As String, ByVal clientLongitude As String)

End Interface
