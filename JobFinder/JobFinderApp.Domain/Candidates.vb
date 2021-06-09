'------------------------------------
' File: Candidates.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports System.Collections.Specialized
Imports System.Web

''' <summary>
''' A container for candidates.
''' </summary>
Public Class Candidates
    Inherits BaseModel

#Region "Constructor"
    Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService, ByVal statisticsService As IStatisticsService)
        MyBase.New(repositoryService, loggingService, statisticsService)
    End Sub
#End Region

    ''' <summary>
    ''' Retreives all job qualifications of candidates that belongs to a specific mandant.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <returns>The NameValueCollection, each entry of with has a unique key (job title) and a value (job title) </returns>
    ''' <remarks></remarks>
    Public Function ReadJobQualifications(ByVal mandantGuid As String) As NameValueCollection

        ' Read the list of job qualifications of available candidates.
        Dim jobQualifications As List(Of String) = Me.repositoryService.ReadJobQualifications(mandantGuid)

        ' The branch items.
        Dim jobQualificationItems As New NameValueCollection(jobQualifications.Count)

        For Each jobQualification In jobQualifications
            jobQualificationItems.Add(jobQualification, jobQualification)
        Next

        Return jobQualificationItems
    End Function

    ''' <summary>
    ''' Gets all candidates that match to the given criteria.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <param name="jobQualificationSearchValue"></param>
    ''' <returns></returns>
    Public Function ReadCandidates(ByVal mandantGuid As String, ByVal jobQualificationSearchValue As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As List(Of SearchItem)

        ' Search for candidates.
        Dim iCandidateDescriptions As IEnumerable(Of ISearchItem) = Me.repositoryService.ReadCandidateDescriptions(mandantGuid, jobQualificationSearchValue)
        Me.statisticsService.UpdateCandidateQueryStatistics(mandantGuid, jobQualificationSearchValue, clientLatitude, clientLongitude)

        Dim candidateDescriptions As New List(Of SearchItem)

        ' Cast to CandidateDescription objects
        For Each candidate In iCandidateDescriptions
            candidateDescriptions.Add(New SearchItem With {.ID = candidate.ID,
                                                                .Title = candidate.Title,
                                                                .EncryptedID = String.Empty})
        Next

        Return candidateDescriptions

    End Function

    ''' <summary>
    ''' Retreives all details for a given candidate.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <param name="candidateId"></param>
    ''' <param name="language"></param>
    ''' <returns>The VacancyDetails Object or Nothing, if the details could not be retreived.</returns>
    Public Function ReadCandidateDetails(ByVal mandantGuid As String,
                                         ByVal candidateId As String,
                                         ByVal language As String) As ISearchResultDetails

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        If String.IsNullOrEmpty(candidateId) Then
            Throw New ArgumentNullException("candidateId")
        End If

        Dim numericCandidateId As Integer = -1
        ' Parse the candidate id.
        If Not Integer.TryParse(candidateId, numericCandidateId) Then
            Me.loggingService.Log(String.Format("Parsing of candidate id from string '{0}' failed.", candidateId), LogLevel.Debug_Level)

            Return Nothing
        End If

        ' Get the translation service from the application cache.
        Dim translationService As ITranslationService = HttpRuntime.Cache(Constants.TRANSLATION_SERVICE_CACHE_KEY)

        ' If the translation service was not added to the application cache then something seriously went wrong.
        If (translationService Is Nothing) Then
            Me.loggingService.Log("Translation service could not be retrieved from application cache.", LogLevel.Error_Level)

            Return Nothing
        End If

        ' Read the column values for the candidate.
        Dim candidateColumnValues As OrderedDictionary = Me.repositoryService.ReadCandidateDetails(mandantGuid, numericCandidateId)

        ' Check if column values could be read.
        If candidateColumnValues Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read vacancy column values for candidate id '{0}'", numericCandidateId), LogLevel.Debug_Level)

            Return Nothing
        End If

        Dim candidateDetails As New SearchResultDetails

        ' Default language if language cannot be paresd
        Dim isoLanguage As IsoLanguage = isoLanguage.DE

        ' Try parse the language.
        [Enum].TryParse(language, isoLanguage)

        ' Translate 'Contact' title
        candidateDetails.ContactTitle = translationService.GetTranslation("TEXT_CONTACT_TITLE", isoLanguage)

        ' Determine the telephone contact data.
        candidateDetails.ContactTelephone.Value = Utility.Utility.ReadDictionaryValue(candidateColumnValues, Constants.COLUM_USER_TELEPHONE, Constants.COLUM_CUSTOMER_TELEPHONE, String.Empty)
        candidateDetails.ContactTelephone.Title = translationService.GetTranslation("TEXT_CONTACT_TELEPHONE", isoLanguage)

        ' Determine  the  email contact data.
        candidateDetails.ContactEmail.Value = Utility.Utility.ReadDictionaryValue(candidateColumnValues, Constants.COLUM_USER_EMAIL, Constants.COLUM_CUSTOMER_EMAIL, String.Empty)
        candidateDetails.ContactEmail.Title = translationService.GetTranslation("TEXT_CONTACT_EMAIL", isoLanguage)

        ' Remove contact data from value collection.
        candidateColumnValues.Remove(Constants.COLUM_USER_TELEPHONE)
        candidateColumnValues.Remove(Constants.COLUM_USER_EMAIL)
        candidateColumnValues.Remove(Constants.COLUM_CUSTOMER_TELEPHONE)
        candidateColumnValues.Remove(Constants.COLUM_CUSTOMER_EMAIL)

        ' Use a string array for the key values, since OrderedDictionary (candidateColumnValues) does not allow an indexed access on the keys.
        Dim valueKeys(candidateColumnValues.Keys.Count) As [String]
        candidateColumnValues.Keys.CopyTo(valueKeys, 0)

        For i As Integer = 0 To candidateColumnValues.Keys.Count - 1

            Dim candidateColumn As String = valueKeys(i)
            Dim value As String = candidateColumnValues(i)

            ' The key for the column translation.
            Dim columnTranslationKey As String = Constants.COLUMN_TRANSLATION_PREFIX + candidateColumn.ToUpper()

            Dim translatedColumn As String = translationService.GetTranslation(columnTranslationKey, isoLanguage)

            If (String.IsNullOrEmpty(translatedColumn)) Then
                Me.loggingService.Log(String.Format("Missing translation for column '{0}'.", candidateColumn), LogLevel.Debug_Level)
                ' Although the translation is missing continue (its not that critical).
            End If

            If (String.IsNullOrEmpty(value)) Then
                ' Replace missing value with info text.
                value = translationService.GetTranslation("TEXT_MISSING_VALUE", isoLanguage)
            End If

            Dim vacancyDetailValue As New SearchResultDetail With {.Title = translatedColumn, .Value = value}

            candidateDetails.AddRegularSearchResultValue(vacancyDetailValue)
        Next

        Return candidateDetails
    End Function
End Class
