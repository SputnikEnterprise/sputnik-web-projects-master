'------------------------------------
' File: SearchResultDetails.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

Public Class SearchResultDetails
    Implements ISearchResultDetails


#Region "Private Fields"

    ''' <summary>
    ''' List of regular search result detail values.
    ''' </summary>
    ''' <remarks>This values will be displayed in a list on the client side.</remarks>
    Private ReadOnly listOfRegularSearchResultDetailValues As New List(Of ISearchResultDetail)

#End Region

#Region "Public Properties"

    ''' <see cref="ISearchResultDetails.ContactTitle"/>
    Public Property ContactTitle As String Implements Contracts.ISearchResultDetails.ContactTitle

    ''' <see cref="ISearchResultDetails.ContactTelephone"/>
    Public Property ContactTelephone As Contracts.ISearchResultDetail = New SearchResultDetail() Implements Contracts.ISearchResultDetails.ContactTelephone

    ''' <see cref="ISearchResultDetails.ContactEmail"/>
    Public Property ContactEmail As Contracts.ISearchResultDetail = New SearchResultDetail() Implements Contracts.ISearchResultDetails.ContactEmail

    ''' <see cref="ISearchResultDetails.ContactPictureUrl"/>
    Public Property ContactPictureUrl As String = String.Empty Implements Contracts.ISearchResultDetails.ContactPictureUrl

    ''' <see cref="ISearchResultDetails.RegularSearchResultDetailValues"/>
    Public ReadOnly Property RegularSearchResultDetailValues As System.Collections.Generic.List(Of Contracts.ISearchResultDetail) Implements Contracts.ISearchResultDetails.RegularSearchResultDetailValues
        Get
            Return Me.listOfRegularSearchResultDetailValues.ToList()
        End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <see cref="ISearchResultDetails.AddRegularSearchResultValue"/>
    Public Sub AddRegularSearchResultValue(searchResultValue As Contracts.ISearchResultDetail) Implements Contracts.ISearchResultDetails.AddRegularSearchResultValue
        Me.listOfRegularSearchResultDetailValues.Add(searchResultValue)
    End Sub

#End Region

End Class

