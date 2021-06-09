'------------------------------------
' File: ISearchResultDetails.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Public Interface ISearchResultDetails

#Region "Properties"

    ''' <summary>
    ''' The contact title.
    ''' </summary>
    Property ContactTitle As String

    ''' <summary>
    ''' The vacancy telephone contact data.
    ''' </summary>
    Property ContactTelephone As ISearchResultDetail

    ''' <summary>
    ''' The vacancy email contact data.
    ''' </summary>
    Property ContactEmail As ISearchResultDetail

    ''' <summary>
    ''' The Picture Url
    ''' </summary>
    Property ContactPictureUrl As String

    ''' <summary>
    ''' Search result values which no special meaning (regular). 
    ''' </summary>
    ReadOnly Property RegularSearchResultDetailValues As List(Of ISearchResultDetail)

#End Region

#Region "Methods"

    ''' <summary>
    ''' Adds a regular search result value.
    ''' </summary>
    ''' <param name="searchResultValue">The search result value.</param>
    ''' <remarks>A regualar search result value will be displayed in a seperat row on the client.</remarks>
    Sub AddRegularSearchResultValue(ByVal searchResultValue As ISearchResultDetail)

#End Region

End Interface
