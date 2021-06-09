'------------------------------------
' File: RepositoryService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------
Imports JobFinderApp.Contracts
Imports JobFinderApp.Domain

Imports System.Reflection
Imports System.Collections.Specialized
Imports System.Data.Objects
Imports System.Web

''' <summary>
''' Database access.
''' </summary>
Public Class RepositoryService
    Implements IRepositoryService

#Region "Private Constants"

    Private Const GENERAL_BRANCH_CATEGORY = "Allgemein"
    Private Const GENERAL_JOB_QUALIFICATION = "Allgemein"

    ' This are mandatory fields that must always be read!
    Private Const MANDATORY_COLUMN_USER_TELEPHONE As String = "User_Telefon"
    Private Const MANDATORY_COLUMN_USER_EMAIL As String = "User_eMail"
    Private Const MANDATORY_COLUMN_CUSTOMER_TELEPHONE As String = "Customer_Telefon"
    Private Const MANDATORY_COLUMN_CUSTOMER_EMAIL As String = "Customer_eMail"
#End Region

#Region "Private fields"
    Private spContractsContext As SpContractEntities

    ''' <summary>
    ''' The logging service.
    ''' </summary>
    Private loggingService As ILoggingService
#End Region

#Region "Constructor"
    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="loggingService">The logging service.</param>
    Public Sub New(ByVal connectionString As String, ByVal loggingService As ILoggingService)
        Me.spContractsContext = New SpContractEntities(connectionString)
        Me.loggingService = loggingService
    End Sub
#End Region

#Region "Public Methods"

    ''' <see cref="IRepositoryService.ReadMandantDetails"/>
    Public Function ReadMandantDetails(ByVal mandantGuid As String) As IMandantDetails Implements IRepositoryService.ReadMandantDetails

        Dim mandantList = (From setting In Me.spContractsContext.MySetting _
                           Where setting.Vak_Guid = mandantGuid _
                           Select New With {setting.Vak_Guid, _
                                            setting.Customer_Name, _
                                            setting.Customer_Homepage, _
                                            setting.Customer_eMail} _
                                        Distinct).ToList()

        Dim mandant As New MandantDetails()
        mandant.Guid = mandantGuid
        If mandantList.Count = 0 Then
            mandant.IsValidMandantGuid = False
            Me.loggingService.Log("The mandant with guid '" & mandantGuid & "' is not valid.", LogLevel.Error_Level)
        Else
            mandant.IsValidMandantGuid = True
            mandant.Name = mandantList(0).Customer_Name
            mandant.HomePage = mandantList(0).Customer_Homepage
            mandant.EmailAddress = mandantList(0).Customer_eMail
        End If

        Return mandant
    End Function

    ''' <see cref="IRepositoryService.ReadMandantIcon"/>
    Public Function ReadMandantIcon(ByVal mandantGuid As String) As Byte() Implements IRepositoryService.ReadMandantIcon
        Dim mandantIcons = (From setting In Me.spContractsContext.MySetting _
                                   Where setting.Vak_Guid = mandantGuid _
                                   Select setting.Customer_Logo
                            Distinct).ToList()

        If mandantIcons.Count = 0 Then
            Me.loggingService.Log("The icon for mandant with guid '" & mandantGuid & "' could not be retreived.", LogLevel.Error_Level)
            Return Nothing
        Else
            Return mandantIcons(0)
        End If
    End Function

    ''' <see cref="IRepositoryService.ReadUserPicture"/>
    Public Function ReadUserPicture(ByVal mandantGuid As String, ByVal userId As String) As Byte() Implements IRepositoryService.ReadUserPicture
        Dim userPictures = (From customerUser In Me.spContractsContext.Customer_Users _
                                   Where customerUser.Customer_ID = mandantGuid _
                                   AndAlso customerUser.User_ID = userId _
                                   Select customerUser.User_Picture
                            Distinct).ToList()

        If userPictures.Count = 0 Then
            Me.loggingService.Log("The picture for mandant with guid '" & mandantGuid & "' and user id '" & userId & "' could not be retreived.", LogLevel.Error_Level)
            Return Nothing
        Else
            Return userPictures(0)
        End If
    End Function

    ''' <see cref="IRepositoryService.ReadCantonAbbreviations"/>
    Public Function ReadCantonAbbreviations(ByVal mandantGuid As String) As IEnumerable(Of String) Implements IRepositoryService.ReadCantonAbbreviations

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        ' Read all cantons which have vacacencies.
        Return Me.spContractsContext.KD_Vakanzen _
                                        .Where(Function(vacancy As KD_Vakanzen) Not String.IsNullOrEmpty(vacancy.Vak_Kanton) And vacancy.Customer_ID = mandantGuid) _
                                        .Select(Function(vacancy As KD_Vakanzen) vacancy.Vak_Kanton) _
                                        .Distinct().OrderBy(Function(cantonAbbreviation As String) cantonAbbreviation).ToList()

    End Function

    ''' <see cref="IRepositoryService.ReadBranches"/>
    Public Function ReadBranches(ByVal mandantGuid As String, Optional ByVal cantonAbbreviation As String = Nothing) As IEnumerable(Of String) Implements IRepositoryService.ReadBranches

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        Dim cantonAbbreviationFilter As String = Nothing

        ' Check canton abbreviation argument.
        If Not String.IsNullOrEmpty(cantonAbbreviation) AndAlso Not cantonAbbreviation = Constants.SELECT_ALL_WILDCARD Then
            cantonAbbreviationFilter = cantonAbbreviation
        End If

        Dim branchesQuery = Me.spContractsContext.KD_Vakanzen.Where(Function(vacancy As KD_Vakanzen) vacancy.Customer_ID = mandantGuid)

        ' Filter by canton, if canton abbreviation was passed.
        If Not String.IsNullOrEmpty(cantonAbbreviationFilter) Then
            branchesQuery = branchesQuery.Where(Function(vacancy As KD_Vakanzen) vacancy.Vak_Kanton = cantonAbbreviationFilter)
        End If

        ' Read the branches.
        Dim branchesStrings As List(Of String) = branchesQuery.Select(Function(vacancy As KD_Vakanzen) vacancy.Branchen).Distinct().ToList()

        Dim listOfBranches As New List(Of String)

        ' Split branches string e.g. (Hochbau#Tiefbau) by delimteter character '#'
        For Each branchString As String In branchesStrings

            If (String.IsNullOrEmpty(branchString)) Then
                listOfBranches.Add(GENERAL_BRANCH_CATEGORY)
            Else
                Dim multipleBranches As String() = branchString.Split("#")

                listOfBranches.AddRange(multipleBranches)
            End If
        Next

        ' Remove double entries.
        listOfBranches = listOfBranches.Distinct().ToList()

        ' Sort alphabetically.
        listOfBranches.Sort()

        Return listOfBranches

    End Function

    ''' <see cref="IRepositoryService.ReadVacancyDescriptions"/>
    Public Function ReadVacancyDescriptions(ByVal mandantGuid As String,
                                            Optional ByVal cantonAbbreviation As String = Nothing,
                                            Optional ByVal branch As String = Nothing,
                                            Optional ByVal vacancyText As String = Nothing) As IEnumerable(Of ISearchItem) Implements IRepositoryService.ReadVacancyDescriptions

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        Dim cantonFilter As String = Nothing
        Dim branchFilter As String = Nothing
        Dim vanancyTextFilter As String = Nothing

        ' Check canton abbreviation argument.
        If Not String.IsNullOrEmpty(cantonAbbreviation) AndAlso Not cantonAbbreviation = Constants.SELECT_ALL_WILDCARD Then
            cantonFilter = cantonAbbreviation
        End If

        ' Check branch argument.
        If Not String.IsNullOrEmpty(branch) AndAlso Not branch = Constants.SELECT_ALL_WILDCARD Then
            branchFilter = branch
        End If

        ' Check vacancy title argument.
        If Not String.IsNullOrEmpty(vacancyText) Then
            vanancyTextFilter = vacancyText
        End If

        Dim vacancyQuery = Me.spContractsContext.KD_Vakanzen.Where(Function(vacancy As KD_Vakanzen) vacancy.Customer_ID = mandantGuid)

        ' Filter by canton.
        If (Not String.IsNullOrEmpty(cantonFilter)) Then
            vacancyQuery = vacancyQuery.Where(Function(vacancy As KD_Vakanzen) vacancy.Vak_Kanton = cantonFilter)
        End If

        ' Filter by branch.
        If (Not String.IsNullOrEmpty(branchFilter)) Then
            If Not branch = GENERAL_BRANCH_CATEGORY Then
                vacancyQuery = vacancyQuery.Where(Function(vacancy As KD_Vakanzen) vacancy.Branchen.Contains(branchFilter))
            Else
                vacancyQuery = vacancyQuery.Where(Function(vacancy As KD_Vakanzen) String.IsNullOrEmpty(vacancy.Branchen))
            End If
        End If

        ' Filter by vacancy text.
        If (Not String.IsNullOrEmpty(vanancyTextFilter)) Then
            vacancyQuery = vacancyQuery.Where(Function(vacancy As KD_Vakanzen) vacancy.Bezeichnung.ToLower().Contains(vanancyTextFilter.ToLower()))
        End If

        ' Order by Date and Description
        vacancyQuery.OrderByDescending(Function(vacancy As KD_Vakanzen) vacancy.Transfered_On) _
                    .OrderBy(Function(vacancy As KD_Vakanzen) vacancy.Bezeichnung)

        ' Read the final vacancies.
        Return vacancyQuery.Select(Function(vacancy As KD_Vakanzen) New SearchItem With {.ID = vacancy.ID, .Title = vacancy.Bezeichnung}).ToList()

    End Function

    ''' <see cref="IRepositoryService.ReadCandidateDescriptions"/>
    Public Function ReadCandidateDescriptions(ByVal mandantGuid As String, Optional ByVal jobQualificationSearchValue As String = Nothing) As IEnumerable(Of ISearchItem) Implements Contracts.IRepositoryService.ReadCandidateDescriptions
        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        Dim jobQualificationSearchFilter As String = Nothing

        ' Check job qualification argument.
        If Not String.IsNullOrEmpty(jobQualificationSearchValue) AndAlso Not jobQualificationSearchValue = Constants.SELECT_ALL_WILDCARD Then
            jobQualificationSearchFilter = jobQualificationSearchValue
        End If

        Dim candidatesQuery = Me.spContractsContext.Kandidaten_Online.Where(Function(candidate As Kandidaten_Online) candidate.Customer_ID = mandantGuid)

        ' Filter by job qualification.
        If (Not String.IsNullOrEmpty(jobQualificationSearchFilter)) Then
            candidatesQuery = candidatesQuery.Where(Function(candidate As Kandidaten_Online) candidate.MA_Beruf.ToLower().Contains(jobQualificationSearchFilter.ToLower()))
        End If

        ' Order by Date and Job Qualification
        candidatesQuery.OrderByDescending(Function(candidate As Kandidaten_Online) candidate.Transfered_On) _
                    .OrderBy(Function(candidate As Kandidaten_Online) candidate.MA_Beruf.Replace("#", ", "))

        ' Return the candidates.
        Return candidatesQuery.Select(Function(candidate As Kandidaten_Online) New SearchItem With {.ID = candidate.ID, .Title = candidate.MA_Beruf.Replace("#", ", ")}).ToList()
    End Function

    ''' <see cref="IRepositoryService.ReadVacancyDetails"/>
    Public Function ReadVacancyDetails(ByVal mandantGuid As String, ByVal vacancyId As Integer) As OrderedDictionary Implements IRepositoryService.ReadVacancyDetails

        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        If String.IsNullOrEmpty(vacancyId) Then
            Throw New ArgumentNullException("vacancyId")
        End If

        ' First read the visible vacancy columns for the mandant guid.
        Dim listOfColumnsToRead As List(Of String) = Me.ReadVisibleVacancyColumns(mandantGuid)

        If listOfColumnsToRead Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read list of visible vacancy columns for mandant guid '{0}'.", mandantGuid), LogLevel.Debug_Level)

            Return Nothing
        End If

        ' Read the vacancy by a SP.
        Dim oResult As ObjectResult(Of Get_Vak_Rec_By_Id_Return_Type) = Me.spContractsContext.Get_Vacancy_StoredProcedure(mandantGuid, vacancyId)
        Dim vacancy As Get_Vak_Rec_By_Id_Return_Type = oResult.ToList().First()

        If vacancy Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read vacancy with id={0}", vacancyId), LogLevel.Debug_Level)

            Return Nothing
        End If

        Dim vacancyValues As New OrderedDictionary

        ' Read all column values.
        For Each columName As String In listOfColumnsToRead

            If (columName.Equals("Berater")) Then

                If (listOfColumnsToRead.Contains("User_Picture_App") AndAlso Not String.IsNullOrEmpty(vacancy.User_Guid)) Then

                    Dim mandantGuidParam As String = Utility.Utility.SerializeParameterObject(mandantGuid, True)
                    mandantGuidParam = HttpUtility.UrlEncode(mandantGuidParam)
                    Dim userGuidParam As String = Utility.Utility.SerializeParameterObject(vacancy.User_Guid.ToString(), True)
                    userGuidParam = HttpUtility.UrlEncode(userGuidParam)

                    Dim baseUrl = Utility.Utility.GetBaseUrl()
                    Dim url As String = baseUrl & String.Format(Constants.MANDANT_USER_PICTURE_RELATIVE_URL, mandantGuidParam, userGuidParam)

                    vacancyValues.Add(Constants.COLUMN_MANDANT_USER_PICTURE_URL, url)
                End If

                ' Use first and last name instead of shortcut
                Dim value As String = vacancy.User_Vorname & " " & vacancy.User_Nachname
                vacancyValues.Add(columName, value.ToString())

            ElseIf (columName.Equals("User_Picture_WOS") OrElse
                    columName.Equals("User_Picture_App")) Then
                ' Do nothing

            Else

                ' Get the column value via reflection mechanism.
                Dim propertyInfo As PropertyInfo = GetType(Get_Vak_Rec_By_Id_Return_Type).GetProperty(columName)

                If Not propertyInfo Is Nothing Then
                    Dim value As Object = propertyInfo.GetValue(vacancy, Nothing)

                    vacancyValues.Add(columName, If(value Is Nothing, String.Empty, value.ToString()))
                Else
                    Me.loggingService.Log(String.Format("Value for vacancy column '{0}' could not be read (VacancyId={1}).", columName, vacancyId), LogLevel.Debug_Level)
                End If
      End If
        Next

        Return vacancyValues

    End Function

    ''' <see cref="IRepositoryService.ReadJobQualifications"/>
    Public Function ReadJobQualifications(ByVal mandantGuid As String) As IEnumerable(Of String) Implements IRepositoryService.ReadJobQualifications
        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        ' Define the query
        Dim jobQualificationsQuery = Me.spContractsContext.Kandidaten_Online.Where(Function(candidate As Kandidaten_Online) candidate.Customer_ID = mandantGuid)

        ' Read the job qualifications.
        Dim jobQualificationStrings As List(Of String) = jobQualificationsQuery.Select(Function(candidate As Kandidaten_Online) candidate.MA_Beruf).Distinct().ToList()

        ' Split job qualification string e.g. (Personalfachmann#Personalberater) by delimteter character '#'
        Dim listOfJobQualifications As New List(Of String)

        For Each jobQualificationString As String In jobQualificationStrings

            If (String.IsNullOrEmpty(jobQualificationString)) Then
                listOfJobQualifications.Add(GENERAL_JOB_QUALIFICATION)
            Else
                Dim multipleJobQualifications As String() = jobQualificationString.Split("#")

                listOfJobQualifications.AddRange(multipleJobQualifications)
            End If
        Next

        ' Remove double entries.
        listOfJobQualifications = listOfJobQualifications.Distinct().ToList()

        ' Sort alphabetically.
        listOfJobQualifications.Sort()

        Return listOfJobQualifications

    End Function

    ''' <see cref="IRepositoryService.ReadCandidateDetails"/>
    Function ReadCandidateDetails(ByVal mandantGuid As String, ByVal candidateId As Integer) As OrderedDictionary Implements IRepositoryService.ReadCandidateDetails
        If String.IsNullOrEmpty(mandantGuid) Then
            Throw New ArgumentNullException("mandantGuid")
        End If

        If String.IsNullOrEmpty(candidateId) Then
            Throw New ArgumentNullException("candidateId")
        End If

        ' First read the visible candidate columns for the mandant guid.
        Dim listOfColumnsToRead As List(Of String) = Me.ReadVisibleVacancyColumns(mandantGuid)

        If listOfColumnsToRead Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read list of visible candidate columns for mandant guid '{0}'.", mandantGuid), LogLevel.Debug_Level)

            Return Nothing
        End If

        ' Read the candidate.
        Dim candidate As Kandidaten_Online = Me.spContractsContext.Kandidaten_Online _
                                             .Where(Function(candidate_online As Kandidaten_Online) candidate_online.ID = candidateId And candidate_online.Customer_ID = mandantGuid) _
                                             .FirstOrDefault()

        If candidate Is Nothing Then
            Me.loggingService.Log(String.Format("Could not read candidate with id={0}", candidateId), LogLevel.Debug_Level)

            Return Nothing
        End If

        Dim candidateValues As New OrderedDictionary

        ' Read all column values.
        For Each columName As String In listOfColumnsToRead

            ' Get the column value via reflection mechanism.
            Dim propertyInfo As PropertyInfo = GetType(Kandidaten_Online).GetProperty(columName)

            If Not propertyInfo Is Nothing Then
                Dim value As Object = propertyInfo.GetValue(candidate, Nothing)


                candidateValues.Add(columName, value.ToString())
            Else
                Me.loggingService.Log(String.Format("Value for candidate column '{0}' could not be read (CandidateId={1}).", columName, candidateId), LogLevel.Debug_Level)
            End If

        Next

        Return candidateValues
    End Function

    ''' <see cref="IRepositoryService.UpdateDownloadStatistics"/>
    Public Sub UpdateDownloadStatistics(ByVal mandantGuid As String, ByVal appId As String) Implements IRepositoryService.UpdateDownloadStatistics

        Dim mandantStat = GetMandantStats(mandantGuid)

        Dim downloadStats = (From statDownload In mandantStat.Stats_Downloads _
                           Where statDownload.ApplicationId = appId _
                           Select statDownload _
                           Distinct)

        ' Insert download statistics only if client does not exist yet.
        If downloadStats.Count() = 0 Then
            Dim stat As Stats_Downloads = New Stats_Downloads()
            stat.ApplicationId = appId
            stat.FirstDownloadDateTime = Date.Now
            mandantStat.Stats_Downloads.Add(stat)
        End If

        Me.spContractsContext.SaveChanges()

    End Sub

    ''' <see cref="IRepositoryService.UpdateVacancyQueryStatistics"/>
    Public Sub UpdateVacancyQueryStatistics(mandantGuid As String, cantonFilter As String, branchFilter As String, vacancyTitleFilter As String, ByVal clientLatitude As String, ByVal clientLongitude As String) Implements Contracts.IRepositoryService.UpdateVacancyQueryStatistics
        Dim mandantRecord As Stats_Mandants = GetMandantStats(mandantGuid)

        ' Always insert a query statistics record.
        Dim stat As Stats_Queries = New Stats_Queries()
        Dim geolocationCoordinate As Decimal
        stat.Client_Latitude = IIf(Decimal.TryParse(clientLatitude, geolocationCoordinate), geolocationCoordinate, 0)
        stat.Client_Longitude = IIf(Decimal.TryParse(clientLongitude, geolocationCoordinate), geolocationCoordinate, 0)
        stat.CantonFilter = cantonFilter
        stat.BranchFilter = branchFilter
        stat.VacancyTitleFilter = vacancyTitleFilter
        stat.QueryDateTime = DateTime.Now
        mandantRecord.Stats_Queries.Add(stat)

        Me.spContractsContext.SaveChanges()
    End Sub

    ''' <see cref="IRepositoryService.UpdateCandidateQueryStatistics"/>
    Sub UpdateCandidateQueryStatistics(ByVal mandantGuid As String, ByVal jobQualificationSearchValue As String, ByVal clientLatitude As String, ByVal clientLongitude As String) Implements Contracts.IRepositoryService.UpdateCandidateQueryStatistics
        Dim mandantRecord As Stats_Mandants = GetMandantStats(mandantGuid)

        ' Always insert a query statistics record.
        Dim stat As Stats_Queries = New Stats_Queries()
        Dim geolocationCoordinate As Decimal
        stat.Client_Latitude = IIf(Decimal.TryParse(clientLatitude, geolocationCoordinate), geolocationCoordinate, 0)
        stat.Client_Longitude = IIf(Decimal.TryParse(clientLongitude, geolocationCoordinate), geolocationCoordinate, 0)
        stat.JobQualificationFilter = jobQualificationSearchValue
        stat.QueryDateTime = DateTime.Now
        mandantRecord.Stats_Queries.Add(stat)

        Me.spContractsContext.SaveChanges()
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Reads the visible vacancy columns.
    ''' </summary>
    ''' <param name="mdGuid">The mandant guid.</param>
    ''' <returns>List of visible vacancy columns.</returns>
    Private Function ReadVisibleVacancyColumns(ByVal mdGuid As String) As List(Of String)

        ' Read te visible vacancy columns.
        Dim visibleVacancyColumns As String = Me.spContractsContext.MySetting.Where(Function(setting As MySetting) setting.Vak_Guid = mdGuid) _
                                                                   .Select(Function(setting As MySetting) setting.Visible_Vacancy_Fields) _
                                                                   .FirstOrDefault()

        If Not String.IsNullOrEmpty(visibleVacancyColumns) Then
            Dim listOfVisibleVacancyFields As New List(Of String)

            ' Split the string with the separator character '#'

            listOfVisibleVacancyFields.AddRange(visibleVacancyColumns.Split("#"))

            ' Add the mandatory fields.
            listOfVisibleVacancyFields.Add(MANDATORY_COLUMN_USER_TELEPHONE)
            listOfVisibleVacancyFields.Add(MANDATORY_COLUMN_USER_EMAIL)
            listOfVisibleVacancyFields.Add(MANDATORY_COLUMN_CUSTOMER_TELEPHONE)
            listOfVisibleVacancyFields.Add(MANDATORY_COLUMN_CUSTOMER_EMAIL)

            ' Makes sure that no duplicates will be read.
            listOfVisibleVacancyFields = listOfVisibleVacancyFields.Distinct().ToList()

            Return listOfVisibleVacancyFields
        End If

        Return Nothing

    End Function


    ''' <summary>
    ''' Reads the visible candidate columns.
    ''' </summary>
    ''' <param name="mdGuid">The mandant guid.</param>
    ''' <returns>List of visible candidate columns.</returns>
    Private Function ReadVisibleCandidateColumns(ByVal mdGuid As String) As List(Of String)

        ' Read te visible vacancy columns.
        Dim visibleCandidateColumns As String = Me.spContractsContext.MySetting.Where(Function(setting As MySetting) setting.Vak_Guid = mdGuid) _
                                                                   .Select(Function(setting As MySetting) setting.Visible_Candidate_Fields) _
                                                                   .FirstOrDefault()

        If Not String.IsNullOrEmpty(visibleCandidateColumns) Then
            Dim listOfVisibleCandidateFields As New List(Of String)

            ' Split the string with the separator character '#'

            listOfVisibleCandidateFields.AddRange(visibleCandidateColumns.Split("#"))

            ' Add the mandatory fields.
            listOfVisibleCandidateFields.Add(MANDATORY_COLUMN_USER_TELEPHONE)
            listOfVisibleCandidateFields.Add(MANDATORY_COLUMN_USER_EMAIL)
            listOfVisibleCandidateFields.Add(MANDATORY_COLUMN_CUSTOMER_TELEPHONE)
            listOfVisibleCandidateFields.Add(MANDATORY_COLUMN_CUSTOMER_EMAIL)

            ' Makes sure that no duplicates will be read.
            listOfVisibleCandidateFields = listOfVisibleCandidateFields.Distinct().ToList()

            Return listOfVisibleCandidateFields
        End If

        Return Nothing

    End Function

    ''' <summary>Gets the main statistics record.
    ''' Checks whether statistics of a certain mandant alerady exists. Creates the record if it does not yet exist, and returns it.</summary>
    ''' <returns>The actual Stat_Mandants object (record).</returns>
    Private Function GetMandantStats(ByVal mandantGuid As String) As Stats_Mandants
        ' Mandant Stats Record should already exist...
        Dim mandantStats = (From statMandant In Me.spContractsContext.Stats_Mandants _
                          Where statMandant.MandantGuid = mandantGuid _
                          Select statMandant
                          Distinct)

        Dim mandantRecord As Stats_Mandants

        If mandantStats.Count() = 0 Then
            ' This happens only the first time.

            ' Insert mandant statistics record.
            Dim mandantStat As Stats_Mandants = New Stats_Mandants()
            mandantStat.MandantGuid = mandantGuid
            Me.spContractsContext.Stats_Mandants.AddObject(mandantStat)
            mandantRecord = mandantStat
        Else
            mandantRecord = mandantStats.First()
        End If
        Return mandantRecord
    End Function

#End Region

End Class