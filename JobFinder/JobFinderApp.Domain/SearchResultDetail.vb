'------------------------------------
' File: SearchResultDetail.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

''' <summary>
''' Vacancy detail.
''' </summary>
Public Class SearchResultDetail
    Implements ISearchResultDetail

    ''' <see cref="ISearchResultDetail.Title"/>
    Public Property Title As String Implements ISearchResultDetail.Title

    ''' <see cref="ISearchResultDetail.Value"/>
    Public Property Value As String Implements ISearchResultDetail.Value

End Class
