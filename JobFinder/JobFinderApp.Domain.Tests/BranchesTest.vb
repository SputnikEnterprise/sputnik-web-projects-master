'------------------------------------
' File: BranchesTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports JobFinderApp.Contracts
Imports System.Collections.Specialized

<TestClass()>
Public Class BranchesTest

    <TestMethod()>
    Public Sub ReadBranches_DataCanBeReadFromRepository_BranchesAreRetunedAsNameValueCollection()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim branches As Branches = New Branches(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim branchesProvidedByRepository As New List(Of String)

        branchesProvidedByRepository.Add("IT")
        branchesProvidedByRepository.Add("Industry")
        branchesProvidedByRepository.Add("Service/Gastronomy")

        ' Repository returns some branches.
        repositoryServiceMock.Setup(Function(repositoryService As IRepositoryService) repositoryService.ReadBranches(It.IsAny(Of String))).Returns(branchesProvidedByRepository)

        ' ----------Act----------
        Dim branchesCollection As NameValueCollection = branches.ReadBranches("dummyMandantGuid")

        ' ----------Assert----------
        Assert.IsTrue(branchesCollection.Count = branchesProvidedByRepository.Count)

        For i As Integer = 0 To branchesProvidedByRepository.Count - 1
            Dim branchFromRepository As String = branchesProvidedByRepository(i)
            Dim branch As String = branchesCollection(i)
            Assert.IsTrue(branchFromRepository = branch)
        Next

    End Sub

End Class
