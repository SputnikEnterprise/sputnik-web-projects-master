'------------------------------------
' File: ISearchItem.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Vacancy description.
''' </summary>
Public Interface ISearchItem

    ''' <summary>
    ''' The id of the search item (database PK). 
    ''' </summary>
    Property ID As Integer

    ''' <summary>
    ''' The title to show of the search item.
    ''' </summary>
    Property Title As String

End Interface
