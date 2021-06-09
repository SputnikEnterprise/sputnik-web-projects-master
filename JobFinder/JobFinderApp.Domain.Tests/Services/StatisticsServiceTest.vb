'------------------------------------
' File: StatisticsServiceTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports JobFinderApp.Contracts
Imports JobFinderApp.Domain.Services

Namespace Services

    <TestClass()>
    Public Class StatisticsServiceTest

        <TestMethod()>
        Public Sub UpdateDownloadStatistics_MandantGuidIsNotProvided_ArgumentNullExceptionIsThrown()

            ' ----------Arrange----------
            Dim repositoryServiceMock As New Mock(Of IRepositoryService)
            Dim loggingServiceMock As New Mock(Of ILoggingService)

            Dim statistics As New StatisticsService(repositoryServiceMock.Object, loggingServiceMock.Object)

            Dim argumentNullExceptionIsThrown As Boolean = False

            ' ----------Act----------
            Try
                statistics.UpdateDownloadStatistics(Nothing, "dummyAppId")
            Catch ex As Exception
                argumentNullExceptionIsThrown = True
            End Try

            ' ----------Assert----------
            Assert.IsTrue(argumentNullExceptionIsThrown)

        End Sub

        <TestMethod()>
        Public Sub UpdateDownloadStatistics_AppIdIsNotProvided_ArgumentNullExceptionIsThrown()

            ' ----------Arrange----------
            Dim repositoryServiceMock As New Mock(Of IRepositoryService)
            Dim loggingServiceMock As New Mock(Of ILoggingService)

            Dim statistics As New StatisticsService(repositoryServiceMock.Object, loggingServiceMock.Object)

            Dim argumentNullExceptionIsThrown As Boolean = False

            ' ----------Act----------
            Try
                statistics.UpdateDownloadStatistics("dummyMandantGuid", Nothing)
            Catch ex As Exception
                argumentNullExceptionIsThrown = True
            End Try

            ' ----------Assert----------
            Assert.IsTrue(argumentNullExceptionIsThrown)

        End Sub

        <TestMethod()>
        Public Sub UpdateDownloadStatistics_MandantGuidAndAppIdAreProvided_DataIsPassedToRepository()

            ' ----------Arrange----------
            Dim repositoryServiceMock As New Mock(Of IRepositoryService)
            Dim loggingServiceMock As New Mock(Of ILoggingService)

            Dim statistics As New StatisticsService(repositoryServiceMock.Object, loggingServiceMock.Object)

            Dim argumentNullExceptionIsThrown As Boolean = False

            Dim mandantGuid As String = "dummyMandantGuid"
            Dim appId As String = "dummyAppId"

            ' ----------Act----------
            Try
                statistics.UpdateDownloadStatistics(mandantGuid, appId)
            Catch ex As Exception
                argumentNullExceptionIsThrown = True
            End Try

            ' ----------Assert----------
            Assert.IsFalse(argumentNullExceptionIsThrown)

            repositoryServiceMock.Verify(Sub(o As IRepositoryService) o.UpdateDownloadStatistics(mandantGuid, appId), Times.Once)

        End Sub

    End Class

End Namespace