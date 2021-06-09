'------------------------------------
' File: FileController.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports JobFinderApp.Domain

Namespace Controllers
    ''' <summary>
    ''' Controller used as file providing handler.
    ''' </summary>
    <HandleError(View:="ErrorView")> _
    Public Class FileController
        Inherits System.Web.Mvc.Controller

#Region "Private Fields"

        ''' <summary>
        ''' The repository service.
        ''' </summary>
        Private repositoryService As IRepositoryService

#End Region

#Region "Constructor"

        ''' <summary>
        ''' The constructor.
        ''' </summary>
        ''' <param name="repositoryService">The repository service.</param>
        Public Sub New(ByVal repositoryService As IRepositoryService)
            Me.repositoryService = repositoryService
        End Sub

#End Region

#Region "Actions"

        ''' <summary>
        ''' Returns the icon belonging to the Mandant.
        ''' </summary>
        ''' <param name="mandantGuid">The mandant guid.</param>
        ''' <returns>The icon image.</returns>
        Function GetMandantIcon(ByVal mandantGuid As String) As FileResult
            Dim fileContents() As Byte
            Dim decryptedMandantGuid = Utility.Utility.DeSerializeParameterObject(mandantGuid, True)
            fileContents = Me.repositoryService.ReadMandantIcon(decryptedMandantGuid)
            Return File(fileContents, "image/png")
        End Function

        ''' <summary>
        ''' Returns the picture belonging to the berater of a mandant.
        ''' </summary>
        ''' <param name="mandantGuid">The mandant guid.</param>
        ''' <returns>The picture.</returns>
        Function GetMandantUserPicture(ByVal mandantGuid As String, userId As String) As FileResult
            Dim fileContents() As Byte
            Dim decryptedMandantGuid = Utility.Utility.DeSerializeParameterObject(mandantGuid, True)
            Dim decryptedUserId = Utility.Utility.DeSerializeParameterObject(userId, True)
            fileContents = Me.repositoryService.ReadUserPicture(decryptedMandantGuid, decryptedUserId)
            Return File(fileContents, "image/jpeg")
        End Function
#End Region

    End Class
End Namespace
