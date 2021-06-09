'------------------------------------
' File: MandantDetails.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

Public Class MandantDetails
    Implements IMandantDetails

#Region "Interface Properties"

    ''' <see cref="IMandantDetails.Guid"/>
    Public Property Guid As String Implements IMandantDetails.Guid

    ''' <see cref="IMandantDetails.IsValidMandantGuid"/>
    Public Property IsValidMandantGuid As Boolean Implements IMandantDetails.IsValidMandantGuid

    ''' <see cref="IMandantDetails.Name"/>
    Public Property Name As String Implements IMandantDetails.Name
        Get
            If Not Me.IsValidMandantGuid Then
                Return String.Empty
            End If
            Return Me.internalName
        End Get
        Set(value As String)
            Me.internalName = value
        End Set
    End Property

    ''' <see cref="IMandantDetails.HomePage"/>
    Public Property HomePage As String Implements IMandantDetails.HomePage
        Get
            If Not Me.IsValidMandantGuid Then
                Return String.Empty
            End If
            Return Me.internalHomePage
        End Get
        Set(value As String)
            If Not String.IsNullOrEmpty(value) AndAlso Not value.ToLower.StartsWith("http://") Then
                value = "http://" & value
            End If
            Me.internalHomePage = value
        End Set
    End Property

    ''' <see cref="IMandantDetails.EmailAddress"/>
    Public Property EmailAddress As String Implements IMandantDetails.EmailAddress
        Get
            If Not Me.IsValidMandantGuid Then
                Return String.Empty
            End If
            Return Me.internalEmailAddress
        End Get
        Set(value As String)
            Me.internalEmailAddress = value
        End Set
    End Property

#End Region

#Region "Private Fieds"
    ''' <summary>
    ''' Internal state for property MandantName.
    ''' </summary>
    Private internalName As String

    ''' <summary>
    ''' Internal state for property MandantHomePage.
    ''' </summary>
    Private internalHomePage As String

    ''' <summary>
    ''' Internal state for property MandantEmailAddress.
    ''' </summary>
    Private internalEmailAddress As String
#End Region

End Class
