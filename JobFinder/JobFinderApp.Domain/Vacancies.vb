'------------------------------------
' File: Vacancies.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports System.Collections.Specialized
Imports System.Web

Public Class Vacancies
    Inherits BaseModel

#Region "Constructor"
    Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService, ByVal statisticsService As IStatisticsService)
        MyBase.New(repositoryService, loggingService, statisticsService)
    End Sub
#End Region

    ''' <summary>
    ''' Gets all vacancies that match to the given criteria.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <param name="vacancyTitle"></param>
    ''' <param name="cantonAbbreviation"></param>
    ''' <param name="branch"></param>
    ''' <returns></returns>
    Public Function ReadVacancies(ByVal mandantGuid As String, ByVal vacancyTitle As String, ByVal cantonAbbreviation As String, ByVal branch As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As List(Of SearchItem)

        ' Search for vacancies.
        Dim iVacancyDescription As IEnumerable(Of ISearchItem) = Me.repositoryService.ReadVacancyDescriptions(mandantGuid, cantonAbbreviation, branch, vacancyTitle)
        Me.statisticsService.UpdateVacancyQueryStatistics(mandantGuid, cantonAbbreviation, branch, vacancyTitle, clientLatitude, clientLongitude)

        Dim vacancyDescription As New List(Of SearchItem)

        ' Cast to VacancyDescription objects
        For Each vacancy In iVacancyDescription
            vacancyDescription.Add(New SearchItem With {.ID = vacancy.ID,
                                                                .Title = vacancy.Title,
                                                                .EncryptedID = String.Empty})
        Next

        Return vacancyDescription


    End Function

    ''' <summary>
    ''' Retreives all details for a given vacancy.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <param name="vacancyId"></param>
    ''' <param name="language"></param>
    ''' <returns>The VacancyDetails Object or Nothing, if the details could not be retreived.</returns>
    Public Function ReadVacancyDetails(ByVal mandantGuid As String,
                                        ByVal vacancyId As String,
                                        ByVal language As String) As ISearchResultDetails

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        If String.IsNullOrEmpty(vacancyId) Then
            Throw New ArgumentNullException("vacancyId")
        End If

        Dim numericVacancyId As Integer = -1

        ' Parse the vacancy id.
        If Not Integer.TryParse(vacancyId, numericVacancyId) Then
            Me.loggingService.Log(String.Format("Parsing of vacancy id from string '{0}' failed.", vacancyId), LogLevel.Debug_Level)

            Return Nothing
        End If

        ' Get the translation service from the application cache.
        Dim translationService As ITranslationService = HttpRuntime.Cache(Constants.TRANSLATION_SERVICE_CACHE_KEY)

        ' If the translation service was not added to the application cache then something seriously went wrong.
        If (translationService Is Nothing) Then
            Me.loggingService.Log("Translation service could not be retrieved from application cache.", LogLevel.Error_Level)

            Return Nothing
        End If

        ' Read the column values for the vacancy.
        Dim vacancyColumnValues As OrderedDictionary = Me.repositoryService.ReadVacancyDetails(mandantGuid, numericVacancyId)

        ' Check if column values could be read.
        If vacancyColumnValues Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read vacancy column values for vacancy id '{0}'", numericVacancyId), LogLevel.Debug_Level)

            Return Nothing
        End If

        Dim vacancyDetails As New SearchResultDetails

        ' Default language if language cannot be paresd
        Dim isoLanguage As IsoLanguage = isoLanguage.DE

        ' Try parse the language.
        [Enum].TryParse(language, isoLanguage)

        ' Translate 'Contact' title
        vacancyDetails.ContactTitle = translationService.GetTranslation("TEXT_CONTACT_TITLE", isoLanguage)

        ' Determine the telephone contact data.
        vacancyDetails.ContactTelephone.Value = Utility.Utility.ReadDictionaryValue(vacancyColumnValues, Constants.COLUM_USER_TELEPHONE, Constants.COLUM_CUSTOMER_TELEPHONE, String.Empty)
        vacancyDetails.ContactTelephone.Title = translationService.GetTranslation("TEXT_CONTACT_TELEPHONE", isoLanguage)

        ' Determine  the  email contact data.
        vacancyDetails.ContactEmail.Value = Utility.Utility.ReadDictionaryValue(vacancyColumnValues, Constants.COLUM_USER_EMAIL, Constants.COLUM_CUSTOMER_EMAIL, String.Empty)
        vacancyDetails.ContactEmail.Title = translationService.GetTranslation("TEXT_CONTACT_EMAIL", isoLanguage)

        ' Remove contact data from value collection.
        vacancyColumnValues.Remove(Constants.COLUM_USER_TELEPHONE)
        vacancyColumnValues.Remove(Constants.COLUM_USER_EMAIL)
        vacancyColumnValues.Remove(Constants.COLUM_CUSTOMER_TELEPHONE)
        vacancyColumnValues.Remove(Constants.COLUM_CUSTOMER_EMAIL)

        If (vacancyColumnValues.Contains(Constants.COLUMN_MANDANT_USER_PICTURE_URL)) Then
            vacancyDetails.ContactPictureUrl = vacancyColumnValues(Constants.COLUMN_MANDANT_USER_PICTURE_URL)
            vacancyColumnValues.Remove(Constants.COLUMN_MANDANT_USER_PICTURE_URL)
        End If

        ' Use a string array for the key values, since OrderedDictionary (vacancyColumnValues) does not allow an indexed access on the keys.
        Dim valueKeys(vacancyColumnValues.Keys.Count) As [String]
        vacancyColumnValues.Keys.CopyTo(valueKeys, 0)

        For i As Integer = 0 To vacancyColumnValues.Keys.Count - 1

            Dim vacancyColumn As String = valueKeys(i)
            Dim value As String = vacancyColumnValues(i)

            ' The key for the column translation.
            Dim columnTranslationKey As String = Constants.COLUMN_TRANSLATION_PREFIX + vacancyColumn.ToUpper()

            Dim translatedColumn As String = translationService.GetTranslation(columnTranslationKey, isoLanguage)

            If (String.IsNullOrEmpty(translatedColumn)) Then
                Me.loggingService.Log(String.Format("Missing translation for column '{0}'.", vacancyColumn), LogLevel.Debug_Level)
                ' Although the translation is missing continue (its not that critical).
            End If

            If (String.IsNullOrEmpty(value)) Then
                ' Replace missing value with info text.
                value = translationService.GetTranslation("TEXT_MISSING_VALUE", isoLanguage)
            End If

            Dim vacancyDetailValue As New SearchResultDetail With {.Title = translatedColumn, .Value = value}

            vacancyDetails.AddRegularSearchResultValue(vacancyDetailValue)
        Next

        Return vacancyDetails
    End Function

End Class
