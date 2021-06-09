'------------------------------------
' File: CantonsTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports Moq
Imports JobFinderApp.Contracts
Imports System.Collections.Specialized


<TestClass()>
Public Class CantonsTest

    <TestMethod()>
    Public Sub ReadCantons_DatabaseReturnsListOfCantons_CantonsAreReturnedAsNameValueCollection()

        ' ----------Arrange----------

        Dim repositoryServiceMock As New Mock(Of IRepositoryService)
        Dim loggingServiceMock As New Mock(Of ILoggingService)
        Dim statisticsServiceMock As New Mock(Of IStatisticsService)

        Dim cantons As New Cantons(repositoryServiceMock.Object, loggingServiceMock.Object, statisticsServiceMock.Object)

        Dim listOfCantonAbbreviationsDeliveredByRepository As New List(Of String)
        listOfCantonAbbreviationsDeliveredByRepository.Add("ZH")
        listOfCantonAbbreviationsDeliveredByRepository.Add("BE")
        listOfCantonAbbreviationsDeliveredByRepository.Add("LU")
        listOfCantonAbbreviationsDeliveredByRepository.Add("UR")
        listOfCantonAbbreviationsDeliveredByRepository.Add("SZ")
        listOfCantonAbbreviationsDeliveredByRepository.Add("OW")
        listOfCantonAbbreviationsDeliveredByRepository.Add("NW")
        listOfCantonAbbreviationsDeliveredByRepository.Add("GL")
        listOfCantonAbbreviationsDeliveredByRepository.Add("ZG")
        listOfCantonAbbreviationsDeliveredByRepository.Add("FR")
        listOfCantonAbbreviationsDeliveredByRepository.Add("SO")
        listOfCantonAbbreviationsDeliveredByRepository.Add("BS")
        listOfCantonAbbreviationsDeliveredByRepository.Add("BL")
        listOfCantonAbbreviationsDeliveredByRepository.Add("SH")
        listOfCantonAbbreviationsDeliveredByRepository.Add("AR")
        listOfCantonAbbreviationsDeliveredByRepository.Add("AI")
        listOfCantonAbbreviationsDeliveredByRepository.Add("SG")
        listOfCantonAbbreviationsDeliveredByRepository.Add("GR")
        listOfCantonAbbreviationsDeliveredByRepository.Add("AG")
        listOfCantonAbbreviationsDeliveredByRepository.Add("TG")
        listOfCantonAbbreviationsDeliveredByRepository.Add("TI")
        listOfCantonAbbreviationsDeliveredByRepository.Add("VD")
        listOfCantonAbbreviationsDeliveredByRepository.Add("VS")
        listOfCantonAbbreviationsDeliveredByRepository.Add("NE")
        listOfCantonAbbreviationsDeliveredByRepository.Add("GE")
        listOfCantonAbbreviationsDeliveredByRepository.Add("JU")

        ' FL (Liechtenstein) is a also handeled a canton
        listOfCantonAbbreviationsDeliveredByRepository.Add("FL")

        Dim mandantGuid As String = "FA23AD10-FDCD-42f1-BC3E-53761773513F"

        repositoryServiceMock.Setup(Function(service As IRepositoryService) service.ReadCantonAbbreviations(mandantGuid)).Returns(listOfCantonAbbreviationsDeliveredByRepository)

        ' ----------Act----------
        Dim cantonCollection As NameValueCollection = cantons.ReadCantons(mandantGuid)

        ' ----------Assert----------
        Assert.IsTrue(cantonCollection.Count = listOfCantonAbbreviationsDeliveredByRepository.Count)

        ' Checks that all cantons delivered by the database are in the collection.
        For Each cantonAbbreviation As String In listOfCantonAbbreviationsDeliveredByRepository
            Assert.IsNotNull(cantonCollection(cantonAbbreviation))
        Next

    End Sub

End Class
