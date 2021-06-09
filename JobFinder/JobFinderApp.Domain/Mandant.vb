'------------------------------------
' File: Mandant.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports System.Web

Public Class Mandant
    Implements IMandantDetails

#Region "Public Properties for IMandantDetails"

    Public Property EmailAddress As String Implements Contracts.IMandantDetails.EmailAddress
        Get
            Return Me.internalMandantDetails.EmailAddress
        End Get
        Set(value As String)
            Me.internalMandantDetails.EmailAddress = value
        End Set
    End Property

    Public Property Guid As String Implements Contracts.IMandantDetails.Guid
        Get
            Return Me.mandantGuid
        End Get
        Set(value As String)
            Me.mandantGuid = value
        End Set
    End Property

    Public Property HomePage As String Implements Contracts.IMandantDetails.HomePage
        Get
            Return Me.internalMandantDetails.HomePage
        End Get
        Set(value As String)
            Me.internalMandantDetails.HomePage = value
        End Set
    End Property

    Public Property IsValidMandantGuid As Boolean Implements Contracts.IMandantDetails.IsValidMandantGuid
        Get
            Return Me.internalMandantDetails.IsValidMandantGuid
        End Get
        Set(value As Boolean)
            Me.internalMandantDetails.IsValidMandantGuid = value
        End Set
    End Property

    Public Property Name As String Implements Contracts.IMandantDetails.Name
        Get
            Return Me.internalMandantDetails.Name
        End Get
        Set(value As String)
            Me.internalMandantDetails.Name = value
        End Set
    End Property

#End Region

#Region "Other Public Properties"

    ''' <summary>
    ''' Info Url used in About Page.
    ''' </summary>
    Public Property AboutInfoUrl As String
        Get
            Return Me.internalAboutInfoUrl
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) AndAlso Not value.ToLower.StartsWith("http://") Then
                value = "http://" & value
            End If
            Me.internalAboutInfoUrl = value
        End Set
    End Property


    ''' <summary>
    ''' The Mandant Icon url.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks>Use this only if IsValidMandantGuid is True.</remarks>
    Public ReadOnly Property IconUrl
        Get
            Dim baseUrl = Utility.Utility.GetBaseUrl()
            Dim serializedParameter = Utility.Utility.SerializeParameterObject(Me.mandantGuid, True)
            serializedParameter = HttpUtility.UrlEncode(serializedParameter)
            Return baseUrl & Constants.ICON_RELATIVE_URL  & serializedParameter
        End Get
    End Property
#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    Public Sub New(ByVal mandantGuid As String, ByVal repositoryService As IRepositoryService)
        Me.mandantGuid = mandantGuid
        Me.internalMandantDetails = repositoryService.ReadMandantDetails(Me.mandantGuid)
    End Sub

#End Region

#Region "Private Fields"

    ''' <summary>
    ''' The mandant guid.
    ''' </summary>
    Private mandantGuid As String

    ''' <summary>
    ''' The mandant details that comes from the repository
    ''' </summary>
    Private internalMandantDetails As IMandantDetails

    ''' <summary>
    ''' The internal about info url used with the property AboutInfoUrl
    ''' </summary>
    Private internalAboutInfoUrl As String

#End Region

End Class
