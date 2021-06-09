'------------------------------------
' File: VacanciesTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports JobFinderApp.Contracts
Imports System.Web
Imports System.Web.Caching
Imports System.Collections.Specialized

<TestClass()>
Public Class VacanciesTest

    <TestMethod()>
    Public Sub ReadVacancies_DatabaseReturnsListOfVacancies_DataFromDatabaseIsReturnedCorrectly()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim vacancyDescriptionsProvidedByDB As New List(Of ISearchItem)

        vacancyDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 1, .EncryptedID = "encryptedId1", .Title = "Software Developer Java"})
        vacancyDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 2, .EncryptedID = "encryptedId2", .Title = "Software Developer C#"})
        vacancyDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 3, .EncryptedID = "encryptedId3", .Title = "Softer Developer C++"})

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadVacancyDescriptions(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String))).Returns(vacancyDescriptionsProvidedByDB)

        ' ----------Act----------
        Dim listOfVacancyDescriptions As List(Of SearchItem) = vacancies.ReadVacancies("manandtGuid", "vacancyTitle", "cantonAbbreviation", "branch", "0", "0")

        ' ----------Assert----------
        Assert.IsTrue(listOfVacancyDescriptions.Count = vacancyDescriptionsProvidedByDB.Count)

        For i As Integer = 0 To listOfVacancyDescriptions.Count - 1
            Dim vacancyDescriptionFromRepository As ISearchItem = vacancyDescriptionsProvidedByDB(i)

            Assert.IsTrue(listOfVacancyDescriptions.Where(Function(vacacyDescription As SearchItem) vacacyDescription.ID = vacancyDescriptionFromRepository.ID And _
                                                                                                            vacacyDescription.Title = vacancyDescriptionFromRepository.Title).Count() = 1)
        Next


    End Sub

    <TestMethod()>
    Public Sub ReadVacancyDetails_MandantGuidIsNotProvided_ArgumentNullExceptionIsThrown()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim argumentNullExceptionIsThrown As Boolean = False

        ' ----------Act----------
        Try
            vacancies.ReadVacancyDetails(Nothing, "1", "dummyLanguage")
        Catch ex As Exception
            argumentNullExceptionIsThrown = True
        End Try

        ' ----------Assert----------
        Assert.IsTrue(argumentNullExceptionIsThrown)

    End Sub

    <TestMethod()>
    Public Sub ReadVacancyDetails_VacancyIdIsNotProvided_ArgumentNullExceptionIsThrown()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim argumentNullExceptionIsThrown As Boolean = False

        ' ----------Act----------
        Try
            vacancies.ReadVacancyDetails("dummyMandantGuid", Nothing, "dummyLanguage")
        Catch ex As Exception
            argumentNullExceptionIsThrown = True
        End Try

        ' ----------Assert----------
        Assert.IsTrue(argumentNullExceptionIsThrown)

    End Sub


    <TestMethod()>
    Public Sub ReadVacancyDetails_VacancyIdIsNotANumber_NothingIsReturnedAndLogEntryIsWrittern()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' ----------Act----------
        Dim vacancyDetails As ISearchResultDetails = vacancies.ReadVacancyDetails("dummyMandantGuid", "notANumber", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(vacancyDetails Is Nothing)

        ' Verify that log is written
        loggingServiceMock.Verify(Sub(logService As ILoggingService) logService.Log(It.IsAny(Of String), LogLevel.Debug_Level), Times.Once)

    End Sub


    <TestMethod()>
    Public Sub ReadVacancyDetails_TranslationServiceIsNotInCache_NothingIsReturnedAndLogEntryIsWrittern()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        ' Make sure translation service is not in cache
        HttpRuntime.Cache.Remove(Constants.TRANSLATION_SERVICE_CACHE_KEY)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' ----------Act----------
        Dim vacancyDetails As ISearchResultDetails = vacancies.ReadVacancyDetails("dummyMandantGuid", "1", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(vacancyDetails Is Nothing)

        ' Verify that log is written
        loggingServiceMock.Verify(Sub(logService As ILoggingService) logService.Log(It.IsAny(Of String), LogLevel.Error_Level), Times.Once)

    End Sub

    <TestMethod()>
    Public Sub ReadVacancyDetails_RepositoryDoesNotReturnVacancyDetails_NothingIsReturnedAndLogEntryIsWritten()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)
        Dim translationServiceMock As New Mock(Of ITranslationService)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' Add the translation service to the application cache.
        HttpRuntime.Cache.Remove(Constants.TRANSLATION_SERVICE_CACHE_KEY)
        HttpRuntime.Cache.Add(Constants.TRANSLATION_SERVICE_CACHE_KEY, translationServiceMock.Object, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)

        translationServiceMock.Setup(Function(translationService As ITranslationService) translationService.GetTranslation(It.IsAny(Of String), It.IsAny(Of String))).Returns("translatedText")
        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadVacancyDetails(It.IsAny(Of String), It.IsAny(Of Integer))).Returns(CType(Nothing, OrderedDictionary))

        ' ----------Act----------
        Dim vacancyDetails As ISearchResultDetails = vacancies.ReadVacancyDetails("dummyMandantGuid", "1", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(vacancyDetails Is Nothing)

        ' Verify that log is written
        loggingServiceMock.Verify(Sub(logService As ILoggingService) logService.Log(It.IsAny(Of String), LogLevel.Debug_Level), Times.Once)

    End Sub


    <TestMethod()>
    Public Sub ReadVacancyDetails_RepositoryReturnsVacancyDetails_VacancyInformationIsReturnedCorrectly()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)
        Dim translationServiceMock As New Mock(Of ITranslationService)

        ' Add the translation service to the application cache.
        HttpRuntime.Cache.Remove(Constants.TRANSLATION_SERVICE_CACHE_KEY)
        HttpRuntime.Cache.Add(Constants.TRANSLATION_SERVICE_CACHE_KEY, translationServiceMock.Object, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)

        translationServiceMock.Setup(Function(translationService As ITranslationService) translationService.GetTranslation(It.IsAny(Of String), It.IsAny(Of String))).Returns("translatedText")

        Dim vacancyDetailsProvidedByRepository As New OrderedDictionary()
        Dim userEmail As String = "max.mustermann@testcompany.ch"
        Dim userTelephone As String = "123456"

        vacancyDetailsProvidedByRepository.Add("User_eMail", userEmail)
        vacancyDetailsProvidedByRepository.Add("User_Telefon", userTelephone)
        vacancyDetailsProvidedByRepository.Add("Canton", "St. Gallen")
        vacancyDetailsProvidedByRepository.Add("Branche", "IT")
        vacancyDetailsProvidedByRepository.Add("WorkingTime", "100%")

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadVacancyDetails(It.IsAny(Of String), It.IsAny(Of Integer))).Returns(vacancyDetailsProvidedByRepository)

        Dim vacancies As New Vacancies(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' ----------Act----------
        Dim vacancyDetails As ISearchResultDetails = vacancies.ReadVacancyDetails("dummyMandantGuid", "1", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(vacancyDetails.RegularSearchResultDetailValues.Count() = vacancyDetailsProvidedByRepository.Count())

        Assert.IsTrue(vacancyDetails.ContactEmail.Value = userEmail)
        Assert.IsTrue(vacancyDetails.ContactTelephone.Value = userTelephone)

        For i As Integer = 0 To vacancyDetailsProvidedByRepository.Keys.Count - 1
            Dim valueFromRepository = vacancyDetailsProvidedByRepository(i)
            Dim vacancyDetailValue = vacancyDetails.RegularSearchResultDetailValues(i).Value
            Assert.IsTrue(vacancyDetailValue = valueFromRepository)
        Next

    End Sub

End Class
