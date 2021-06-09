'------------------------------------
' File: Cantons.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports System.Collections.Specialized

''' <summary>
''' Manages cantons of switzerland.
''' </summary>
''' <remarks></remarks>
Public Class Cantons
    Inherits BaseModel

#Region "Private Fields"

    ''' <summary>
    ''' The swiss canton dictionary.
    ''' </summary>
    Private Shared swissCantonsDictionary As New Dictionary(Of String, Canton)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    Public Sub New(ByVal repositoryService As IRepositoryService, ByVal loggingService As ILoggingService, ByVal statisticsService As IStatisticsService)
        MyBase.New(repositoryService, loggingService, statisticsService)

        If Cantons.swissCantonsDictionary.Count = 0 Then
            ' Fill canton dictionary if still empty.
            Me.AddCanton(New Canton("Zürich", "ZH"))
            Me.AddCanton(New Canton("Bern", "BE"))
            Me.AddCanton(New Canton("Luzern", "LU"))
            Me.AddCanton(New Canton("Uri", "UR"))
            Me.AddCanton(New Canton("Schwyz", "SZ"))
            Me.AddCanton(New Canton("Obwalden", "OW"))
            Me.AddCanton(New Canton("Nidwalden", "NW"))
            Me.AddCanton(New Canton("Glarus", "GL"))
            Me.AddCanton(New Canton("Zug", "ZG"))
            Me.AddCanton(New Canton("Freiburg", "FR"))
            Me.AddCanton(New Canton("Solothurn", "SO"))
            Me.AddCanton(New Canton("Basel-Stadt", "BS"))
            Me.AddCanton(New Canton("Basel-Landschaft", "BL"))
            Me.AddCanton(New Canton("Schaffhausen", "SH"))
            Me.AddCanton(New Canton("Appenzell Ausserrhoden", "AR"))
            Me.AddCanton(New Canton("Appenzell Innerrhoden", "AI"))
            Me.AddCanton(New Canton("St. Gallen", "SG"))
            Me.AddCanton(New Canton("Graubünden", "GR"))
            Me.AddCanton(New Canton("Aargau", "AG"))
            Me.AddCanton(New Canton("Thurgau", "TG"))
            Me.AddCanton(New Canton("Tessin", "TI"))
            Me.AddCanton(New Canton("Waadt", "VD"))
            Me.AddCanton(New Canton("Wallis", "VS"))
            Me.AddCanton(New Canton("Neuenburg", "NE"))
            Me.AddCanton(New Canton("Genf", "GE"))
            Me.AddCanton(New Canton("Jura", "JU"))

            ' Add FL as an additional canton.
            Me.AddCanton(New Canton("Liechtenstein", "FL"))
        End If

    End Sub

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' Retreives all cantons to vacancies belonging to a mandant.
    ''' </summary>
    ''' <param name="mandantGuid"></param>
    ''' <returns>The NameValueCollection, each entry of with has a unique key (canton abbreviation) and a value (canton textual representation) </returns>
    Public Function ReadCantons(ByVal mandantGuid As String) As NameValueCollection

        ' Read the list of cantons which have vacacencies.
        Dim cantonsWithVacancies As List(Of String) = Me.repositoryService.ReadCantonAbbreviations(mandantGuid)

        ' The canton items.
        Dim cantonItems As New NameValueCollection(cantonsWithVacancies.Count)

        For Each cantonAbbreviation In cantonsWithVacancies
            Dim canton As Canton = Me.GetCantonNameByAbbreviation(cantonAbbreviation)
            If Not canton Is Nothing Then
                Dim cantonText As String = canton.CantonNameWithAbbreviation
                cantonItems.Add(cantonAbbreviation, cantonText)
            Else
                Me.loggingService.Log(String.Format("No canton with abbreviation '{0}' could be found.", cantonAbbreviation), LogLevel.Debug_Level)
            End If
        Next

        Return cantonItems

    End Function

#End Region

#Region "Methods"


    ''' <summary>
    ''' Gets a canton name by abbreviation.
    ''' </summary>
    ''' <param name="cantonAbbreviation">The canton abbreviation.</param>
    ''' <returns>The canton name object.</returns>
    Private Function GetCantonNameByAbbreviation(cantonAbbreviation As String) As Canton

        If Not Cantons.swissCantonsDictionary.Keys.Contains(cantonAbbreviation) Then
            Return Nothing
        End If

        Return Cantons.swissCantonsDictionary(cantonAbbreviation)

    End Function

    ''' <summary>
    ''' Adds a canton name.
    ''' </summary>
    ''' <param name="canton">The canton.</param>
    Private Sub AddCanton(canton As Canton)

        If (String.IsNullOrEmpty(canton.CantonAbbreviation)) Then
            Me.loggingService.Log("A canton without an abbreviation can not be added.", LogLevel.Debug_Level)
            Return
        End If

        If Cantons.swissCantonsDictionary.Keys.Contains(canton.CantonAbbreviation) Then
            Me.loggingService.Log(String.Format("The canton '{0}' already exists in the list.", canton.CantonAbbreviation), LogLevel.Debug_Level)
            Return
        End If

        Cantons.swissCantonsDictionary.Add(canton.CantonAbbreviation, canton)
    End Sub

#End Region

End Class
