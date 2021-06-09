'------------------------------------
' File: CandidatesTest.vb
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
Public Class CandidatesTest

    <TestMethod()>
    Public Sub ReadJobQualifications_DatabaseReturnsListOfJobQualifications_DataFromDatabaseIsReturnedCorrectly()
        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim candidateDescriptionsProvidedByDB As New List(Of String)

        candidateDescriptionsProvidedByDB.Add("Software Developer Java")
        candidateDescriptionsProvidedByDB.Add("Software Developer C#")
        candidateDescriptionsProvidedByDB.Add("Software Developer C++")
        candidateDescriptionsProvidedByDB.Add("Software Developer C++")

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadJobQualifications(It.IsAny(Of String))).Returns(candidateDescriptionsProvidedByDB)

        ' ----------Act----------
        Dim listOfJobQualifications As NameValueCollection = candidates.ReadJobQualifications("manandtGuid")

        ' ----------Assert----------
        ' Two candidates has the same job qualification "Software Developer C++"
        Assert.IsTrue(listOfJobQualifications.Count = candidateDescriptionsProvidedByDB.Count - 1)
        ' logging service is never called
        loggingServiceMock.Verify(Sub(loggingService) loggingService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Never())
        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())

    End Sub

    <TestMethod()>
    Public Sub ReadCandidates_DatabaseReturnsListOfCandidates_DataFromDatabaseIsReturnedCorrectly()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim candidateDescriptionsProvidedByDB As New List(Of ISearchItem)

        candidateDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 1, .EncryptedID = "encryptedId1", .Title = "Software Developer Java"})
        candidateDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 2, .EncryptedID = "encryptedId2", .Title = "Software Developer C#"})
        candidateDescriptionsProvidedByDB.Add(New SearchItem() With {.ID = 3, .EncryptedID = "encryptedId3", .Title = "Softer Developer C++"})

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadCandidateDescriptions(It.IsAny(Of String), It.IsAny(Of String))).Returns(candidateDescriptionsProvidedByDB)

        ' ----------Act----------
        Dim listOfCandidateDescriptions As List(Of SearchItem) = candidates.ReadCandidates("manandtGuid", "jobQualification", "0", "0")

        ' ----------Assert----------
        ' Correct number of returned records
        Assert.IsTrue(listOfCandidateDescriptions.Count = candidateDescriptionsProvidedByDB.Count)

        ' Each record is present exactly once
        For i As Integer = 0 To listOfCandidateDescriptions.Count - 1
            Dim vacancyDescriptionFromRepository As ISearchItem = candidateDescriptionsProvidedByDB(i)

            Assert.IsTrue(listOfCandidateDescriptions.Where(Function(vacacyDescription As SearchItem) vacacyDescription.ID = vacancyDescriptionFromRepository.ID And _
                                                                                                            vacacyDescription.Title = vacancyDescriptionFromRepository.Title).Count() = 1)
        Next

        ' logging service is never called
        loggingServiceMock.Verify(Sub(loggingService) loggingService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Never())
        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Once())

    End Sub

    <TestMethod()>
    Public Sub ReadCandidatesDetails_RepositoryReturnsCandidateDetails_CandidateInformationIsReturnedCorrectly()
        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)
        Dim translationServiceMock As New Mock(Of ITranslationService)

        ' Add the translation service to the application cache.
        HttpRuntime.Cache.Remove(Constants.TRANSLATION_SERVICE_CACHE_KEY)
        HttpRuntime.Cache.Add(Constants.TRANSLATION_SERVICE_CACHE_KEY, translationServiceMock.Object, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)

        translationServiceMock.Setup(Function(translationService As ITranslationService) translationService.GetTranslation(It.IsAny(Of String), It.IsAny(Of String))).Returns("translatedText")

        Dim candidateDetailsProvidedByRepository As New OrderedDictionary()
        Const userEmail As String = "max.mustermann@testcompany.ch"
        Const userTelephone As String = "123456"

        candidateDetailsProvidedByRepository.Add("User_eMail", userEmail)
        candidateDetailsProvidedByRepository.Add("User_Telefon", userTelephone)
        candidateDetailsProvidedByRepository.Add("Canton", "St. Gallen")
        candidateDetailsProvidedByRepository.Add("Branche", "IT")
        candidateDetailsProvidedByRepository.Add("WorkingTime", "100%")

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadCandidateDetails(It.IsAny(Of String), It.IsAny(Of Integer))).Returns(candidateDetailsProvidedByRepository)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' ----------Act----------
        Dim candidateDetails As ISearchResultDetails = candidates.ReadCandidateDetails("dummyMandantGuid", "1", "dummyLanguage")

        ' ----------Assert----------
        ' Correct number of returned records
        Assert.IsTrue(candidateDetails.RegularSearchResultDetailValues.Count() = candidateDetailsProvidedByRepository.Count())

        Assert.IsTrue(candidateDetails.ContactEmail.Value = userEmail)
        Assert.IsTrue(candidateDetails.ContactTelephone.Value = userTelephone)

        ' Same values
        For i As Integer = 0 To candidateDetailsProvidedByRepository.Keys.Count - 1
            Dim valueFromRepository = candidateDetailsProvidedByRepository(i)
            Dim vacancyDetailValue = candidateDetails.RegularSearchResultDetailValues(i).Value
            Assert.IsTrue(vacancyDetailValue = valueFromRepository)
        Next

        ' logging service is never called
        loggingServiceMock.Verify(Sub(loggingService) loggingService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Never())
        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())
    End Sub

    <TestMethod()>
    Public Sub ReadCandidateDetails_MandantGuidIsNotProvided_ArgumentNullExceptionIsThrown()
        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim argumentNullExceptionIsThrown As Boolean = False

        ' ----------Act----------
        Try
            candidates.ReadCandidateDetails(Nothing, "1", "dummyLanguage")
        Catch ex As Exception
            argumentNullExceptionIsThrown = True
        End Try

        ' ----------Assert----------
        Assert.IsTrue(argumentNullExceptionIsThrown)

        ' logging service is never called, only throw exception.
        loggingServiceMock.Verify(Sub(loggingService) loggingService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Never())
        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())
    End Sub

    <TestMethod()>
    Public Sub ReadCandidateDetails_CandidateIdIsNotProvided_ArgumentNullExceptionIsThrown()
        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim argumentNullExceptionIsThrown As Boolean = False

        ' ----------Act----------
        Try
            candidates.ReadCandidateDetails("dummyMandantGuid", Nothing, "dummyLanguage")
        Catch ex As Exception
            argumentNullExceptionIsThrown = True
        End Try

        ' ----------Assert----------
        Assert.IsTrue(argumentNullExceptionIsThrown)

        ' logging service is never called, only throw exception.
        loggingServiceMock.Verify(Sub(loggingService) loggingService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Never())
        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())
    End Sub


    <TestMethod()>
    Public Sub ReadCandidateDetails_CandidateIdIsNotANumber_NothingIsReturnedAndLogEntryIsWrittern()
        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' ----------Act----------
        Dim candidateDetails As ISearchResultDetails = candidates.ReadCandidateDetails("dummyMandantGuid", "notANumber", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(candidateDetails Is Nothing)

        ' Verify that log is written
        loggingServiceMock.Verify(Sub(logService) logService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Once)

        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())
    End Sub

    <TestMethod()>
    Public Sub ReadCandidateDetails_TranslationServiceIsNotInCache_NothingIsReturnedAndLogEntryIsWrittern()

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
        loggingServiceMock.Verify(Sub(logService) logService.Log(It.IsAny(Of String), It.IsAny(Of JobFinderApp.Contracts.LogLevel)), Times.Once())

        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())

    End Sub

    <TestMethod()>
    Public Sub ReadCandidateDetails_RepositoryDoesNotReturnCandidateDetails_NothingIsReturnedAndLogEntryIsWritten()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)
        Dim translationServiceMock As New Mock(Of ITranslationService)

        Dim candidates As New Candidates(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        ' Add the translation service to the application cache.
        HttpRuntime.Cache.Remove(Constants.TRANSLATION_SERVICE_CACHE_KEY)
        HttpRuntime.Cache.Add(Constants.TRANSLATION_SERVICE_CACHE_KEY, translationServiceMock.Object, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)

        translationServiceMock.Setup(Function(translationService) translationService.GetTranslation(It.IsAny(Of String), It.IsAny(Of String))).Returns("translatedText")
        repositoryServiceMock.Setup(Function(repository) repository.ReadCandidateDetails(It.IsAny(Of String), It.IsAny(Of Integer))).Returns(CType(Nothing, OrderedDictionary))

        ' ----------Act----------
        Dim candidateDetails As ISearchResultDetails = candidates.ReadCandidateDetails("dummyMandantGuid", "1", "dummyLanguage")

        ' ----------Assert----------
        Assert.IsTrue(candidateDetails Is Nothing)

        ' Verify that log is written
        loggingServiceMock.Verify(Sub(logService) logService.Log(It.IsAny(Of String), LogLevel.Debug_Level), Times.Once)

        ' Statistics service is never called
        statisticsServiceMock.Verify(Function(iStatisticsService) iStatisticsService.UpdateCandidateQueryStatistics(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Never())

    End Sub
End Class
