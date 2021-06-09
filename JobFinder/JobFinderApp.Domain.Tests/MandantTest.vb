'------------------------------------
' File: MandantTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports JobFinderApp.Contracts

<TestClass()>
Public Class MandantTest

    <TestMethod()>
    Public Sub MandantConstructor_MandantGuidIsValid_MandantPropertyDataMatchesWithDataProvidedByRepository()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)

        Dim mandantGuid As String = "FA23AD10-FDCD-42f1-BC3E-53761773513F"

        Dim mandantDetailsProvidedByRepository = New MandantDetails() With {
            .EmailAddress = "max.mustermann@testcompany.ch",
            .Guid = mandantGuid,
            .HomePage = "http://www.test.ch",
            .Name = "DummyName",
            .IsValidMandantGuid = True
        }

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadMandantDetails(mandantGuid)).Returns(mandantDetailsProvidedByRepository)

        ' ----------Act----------
        Dim mandant As Mandant = New Mandant(mandantGuid, repositoryServiceMock.Object)

        ' ----------Assert----------
        Assert.IsTrue(mandant.IsValidMandantGuid)
        Assert.IsTrue(mandant.EmailAddress = mandantDetailsProvidedByRepository.EmailAddress)
        Assert.IsTrue(mandant.Guid = mandantDetailsProvidedByRepository.Guid)
        Assert.IsTrue(mandant.HomePage = mandantDetailsProvidedByRepository.HomePage)
        Assert.IsTrue(mandant.Name = mandantDetailsProvidedByRepository.Name)

    End Sub

    <TestMethod()>
    Public Sub MandantConstructor_MandantGuidIsInvalid_MandantIsNotValid()

        ' ----------Arrange----------
        Dim repositoryServiceMock As New Mock(Of IRepositoryService)

        Dim mandantGuid As String = "XXXXXXXX-XXXX-XXXX-XXXX-XXXXXXXXXXXX"

        Dim mandantDetailsProvidedByRepository = New MandantDetails() With {
            .IsValidMandantGuid = False
        }

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadMandantDetails(mandantGuid)).Returns(mandantDetailsProvidedByRepository)

        ' ----------Act----------
        Dim mandant As Mandant = New Mandant(mandantGuid, repositoryServiceMock.Object)

        ' ----------Assert----------
        Assert.IsFalse(mandant.IsValidMandantGuid)

    End Sub

    <TestMethod()>
    Public Sub AboutInfoUrl_InfoUrlDoesNotStartWithHttpProtocolString_HttpProtocolStringIsAttachedToInfoUrl()


        Dim repositoryServiceMock As New Mock(Of IRepositoryService)

        Dim mandantGuid As String = "FA23AD10-FDCD-42f1-BC3E-53761773513F"

        Dim mandantDetailsProvidedByRepository = New MandantDetails() With {
            .IsValidMandantGuid = True
        }

        repositoryServiceMock.Setup(Function(repository As IRepositoryService) repository.ReadMandantDetails(mandantGuid)).Returns(mandantDetailsProvidedByRepository)

        Dim mandant As Mandant = New Mandant(mandantGuid, repositoryServiceMock.Object)

        Dim aboutInfoUrl As String = "www.test.ch"

        ' ----------Act----------

        mandant.AboutInfoUrl = aboutInfoUrl
        Dim infoUrl = mandant.AboutInfoUrl

        ' ----------Assert----------

        Assert.IsTrue(infoUrl = "http://" & aboutInfoUrl)

    End Sub

End Class
