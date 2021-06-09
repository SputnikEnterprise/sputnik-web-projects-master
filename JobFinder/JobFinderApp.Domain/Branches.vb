'------------------------------------
' File: Branches.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports System.Collections.Specialized


''' <summary>
''' A container for branches.
''' </summary>
Public Class Branches
    Inherits BaseModel

#Region "Constructor"
    Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService, ByVal statisticsService As IStatisticsService)
        MyBase.New(repositoryService, loggingService, statisticsService)
    End Sub
#End Region


    ''' <summary>
    ''' Retreives all branches to vacancies belonging to a mandant.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <param name="cantonAbbreviation"></param>
    ''' <returns>The NameValueCollection, each entry of with has a unique key (branch abbreviation) and a value (branch textual representation) </returns>
    ''' <remarks></remarks>
    Public Function ReadBranches(ByVal mandantGuid As String, Optional ByVal cantonAbbreviation As String = Nothing) As NameValueCollection

        ' Read the list of branches which have vacacencies.
        Dim branches As List(Of String) = Me.repositoryService.ReadBranches(mandantGuid, cantonAbbreviation)

        ' The branch items.
        Dim brancheItems As New NameValueCollection(branches.Count)

        For Each branch In branches
            brancheItems.Add(branch, branch)
        Next

        Return brancheItems

    End Function

End Class
