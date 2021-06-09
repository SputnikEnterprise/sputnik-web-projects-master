'------------------------------------
' File: SearchItem.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.Web.Script.Serialization
Imports JobFinderApp.Contracts

''' <summary>
''' Vacancy description which introduces an encrypted id and hides the ID for javascript objects.
''' </summary>
Public Class SearchItem
    Implements ISearchItem

    ''' <see cref="ISearchItem.ID"/>
    <ScriptIgnore()>
    Public Property ID As Integer Implements Contracts.ISearchItem.ID

    ''' <see cref="ISearchItem.Title"/>
    Public Property Title As String Implements Contracts.ISearchItem.Title

    ''' <summary>
    ''' Use ab encrypted ID instead of ID. 
    ''' </summary>
    Public Property EncryptedID As String

End Class
