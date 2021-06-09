'------------------------------------
' File: HomeController.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports JobFinderApp.ViewModels
Imports JobFinderApp.Domain

Namespace Controllers

    ''' <summary>
    ''' Home controller.
    ''' </summary>
    <HandleError(View:="ErrorView")> _
    Public Class HomeController
        Inherits System.Web.Mvc.Controller

#Region "Private Fields"

        ''' <summary>
        ''' The logging service.
        ''' </summary>
        Private loggingService As ILoggingService

        ''' <summary>
        ''' The configuration service
        ''' </summary>
        Private configService As IConfigService

        Private Const PATH_TO_MANDANT_CONTENT = "Content/Mandants"
        Private Const SETTING_FILE_NAME = "MandantSettings.xml"

#End Region

#Region "Constructor"

        ''' <summary>
        ''' The constructor.
        ''' </summary>
        ''' <param name="loggingService">The logging service.</param>
        Public Sub New(ByVal loggingService As ILoggingService, ByVal configService As IConfigService)
            Me.loggingService = loggingService
            Me.configService = configService
        End Sub
#End Region

#Region "Public Methods (Actions)"

        ''' <summary>
        ''' Main Controller Action
        ''' </summary>
        ''' <returns></returns>
        Function Index(ByVal stream As String) As ActionResult
            ' **********************************
            ' 1.) Parsing querystring parameter:

            ' The default language is 'DE'.
            Dim language As IsoLanguage = IsoLanguage.DE

            ' The mandant guid.
            Dim mandantGuid As String = Nothing

            ' The application guid
            Dim appGuid As String = Nothing

            ' The default role: Job Seeker
            Dim role As Contracts.Role = Contracts.Role.JobSeeker


            ' The about infos
            Dim mandant As Mandant = Nothing

            ' Read parameters from stream query string variable.
            If Not String.IsNullOrEmpty(stream) Then

                Try
                    ' Deserialize the URL encoded object.
                    Dim streamObject As Object = Utility.Utility.DeSerializeParameterObject(stream, True)

                    ' Check if object is of type 'String()'.
                    If GetType(String()).IsAssignableFrom(streamObject.GetType()) Then

                        ' Cast the generic object to a 'String()' object.
                        Dim queryStringSettings As String() = CType(streamObject, String())

                        ' Read the configuration data.
                        Dim mandantParam As String = Nothing
                        Dim appGuidParam As String = Nothing
                        Dim languageParam As String = Nothing
                        Dim roleParam As String = Nothing
                        
                        mandantParam = queryStringSettings(0)
                        appGuidParam = queryStringSettings(1)
                        languageParam = queryStringSettings(2)
                        roleParam = queryStringSettings(3)

                        ' Mandatory parameters: mandant guid
                        If Not String.IsNullOrEmpty(mandantParam) Then
                            mandantGuid = mandantParam
                        Else
                            Utility.Utility.ThrowTimestampedException("Mandatory parameter ""MandantGuid"" was not provided.")
                        End If

                        If Not String.IsNullOrEmpty(appGuidParam) Then
                            appGuid = appGuidParam
                        Else
                            Utility.Utility.ThrowTimestampedException("Mandatory parameter ""AppGuid"" was not provided.")
                        End If

            ' IOS 7 Safari Bug (Bookmarked App does not provide cookie)
            ' http://www.mobilexweb.com/blog/safari-ios7-html5-problems-apis-review
            'If Request.Browser.Cookies AndAlso Request.Cookies("goodjobsCookie") Is Nothing Then
            '    ' If cookies are supported, but App cookie does not exist then no valid app request was made.
            '    ' Use the AppRequestController for doing this.
            '    Utility.Utility.ThrowTimestampedException("Please submit a valid App request.")
            'End If

                        mandant = New Mandant(mandantGuid, DependencyResolver.Current.GetService(Of IRepositoryService)())

                        ' Other parameters:
                        ' Language:
                        If String.IsNullOrEmpty(languageParam) Then
                            ' If parameter is not provided, use browser language setting.
                            languageParam = Request.UserLanguages(0).Trim().Substring(0, 2)
                        End If

                        languageParam = languageParam.ToUpper()

                        If Not (languageParam.Equals("DE") OrElse languageParam.Equals("FR") OrElse languageParam.Equals("EN") OrElse languageParam.Equals("IT")) Then
                            ' If language param has an unsopported language, choose german as default language.
                            languageParam = "DE"
                        End If

                        language = [Enum].Parse(GetType(IsoLanguage), languageParam)

                        ' Search Object:
                        If Not String.IsNullOrEmpty(roleParam) Then
                            If Not (roleParam.Equals("JobSeeker") OrElse roleParam.Equals("CandidateSeeker")) Then
                                ' If the role has an unsopported value, choose Job Seeker as default role.
                                roleParam = "JobSeeker"
                            End If

                            role = [Enum].Parse(GetType(Role), roleParam)
                        End If
                    End If

                Catch ex As Exception
                    Me.loggingService.Log(String.Format("Deserializing of query string parameter 'stream' to an object failed. Requested url={0}. Exception={1}", _
                                                        Request.RawUrl, _
                                                        ex.ToString()), _
                                                        LogLevel.Debug_Level)
                    Throw ex
                End Try

            Else
                Me.loggingService.Log("The mandatory parameter stream is not given.", LogLevel.Error_Level)

#If DEBUG Then
                ' IMPORTANT: Use only Release Builds on the Web-Server, otherwise these test parameter are used.
                ' Use these parameter for debugging only!

                ' Create fantasy parameter, just for debugging.
                language = IsoLanguage.DE
                mandantGuid = "FA23AD10-FDCD-42f1-BC3E-53761773513F"
                appGuid = "FCA39D10-F990-42F3-GB34-345961337853"
                role = Contracts.Role.JobSeeker
                mandant = New Mandant(mandantGuid, DependencyResolver.Current.GetService(Of IRepositoryService)())
                mandant.AboutInfoUrl = String.Empty '"http://www.eupro.ch"

                ' The following stream in variable 'debugStream' had could be used:
                Dim paramStream(4) As String
                paramStream(0) = mandantGuid
                paramStream(1) = appGuid
                paramStream(2) = "DE"
                paramStream(3) = "JobSeeker"
                paramStream(4) = "http://www.eupro.ch"

                ' Use this for debugging:
                Dim debugStream As String = Utility.Utility.SerializeParameterObject(paramStream, True)
                debugStream = HttpUtility.UrlEncode(debugStream)

#Else
                ' For release configuration: do not allow starting of the application,
                ' if mandatory parameter is not provided.
                Utility.Utility.ThrowTimestampedException("No parameter provided. Please submit a valid App request.")
#End If

            End If

            If Not mandant.IsValidMandantGuid Then
                Me.loggingService.Log("Provided mandant with guid '" & mandant.Guid & "' is not valid.", LogLevel.Error_Level)
                Utility.Utility.ThrowTimestampedException("The provided mandant guid is not valid.")
            End If

            ' *********************************************
            ' 2a.) Getting Translation Service out of cache:

            ' Get the translation service from the application cache.
            Dim translationService As ITranslationService = HttpRuntime.Cache(Constants.TRANSLATION_SERVICE_CACHE_KEY)

            ' If the translation service was not added to the application cache then something seriously went wrong.
            If (translationService Is Nothing) Then
                Me.loggingService.Log("Translation service could not be retrieved from application cache.", LogLevel.Error_Level)
                Utility.Utility.ThrowTimestampedException("The Translation service could not be retreived.")
            End If

            ' *********************************************
            ' 2b.) Load config service settings
            Dim settingFilePath = Server.MapPath("~/" & PATH_TO_MANDANT_CONTENT & "/" & mandant.Guid & "/" & SETTING_FILE_NAME)
            Me.configService.LoadSettings(settingFilePath)

            ' ****************************************************
            ' 3.) Set JavaScript relevant word fragment variables:

            ' Set strings required by the layout page.
            ViewData("MandantGuid") = mandantGuid
            ViewData("Title") = configService.GetSetting("APP_NAME")
            If String.IsNullOrEmpty(ViewData("Title")) Then
                ViewData("Title") = translationService.GetTranslation("TEXT_DEFAULT_APPLICATION_TITLE", language)
            End If
            ViewData("PleaseWaitMessage") = translationService.GetTranslation("TEXT_PLEASE_WAIT", language)
            ViewData("InstallMsg1") = translationService.GetTranslation("TEXT_INSTALL_MSG1", language)
            ViewData("InstallMsg2") = translationService.GetTranslation("TEXT_INSTALL_MSG2", language)
            ViewData("BackButton") = translationService.GetTranslation("TEXT_BACK_BUTTON", language)
            ViewData("applicationPath") = Utility.Utility.GetApplicationPath()

            ' *********************************************
            ' 4.) Build IndexViewModel to pass to the View:
            Dim indexViewModel As IndexViewModel = New IndexViewModel(mandant, translationService, configService, language, role)
            Return View(indexViewModel)

        End Function

#End Region

#Region "Public methods (Web Services)"

        ''' <summary>
        ''' Action to loads the branches by cantons.
        ''' </summary>
        ''' <param name="encryptedMandantGuid">The encrypted mandant guid.</param>
        ''' <param name="cantonAbbreviation">The canton abbreviation.</param>
        ''' <returns>List of branches as json objects.</returns>
        Public Function LoadBranchesByCantonAbbreviation(ByVal encryptedMandantGuid As String, ByVal cantonAbbreviation As String) As JsonResult

            If String.IsNullOrEmpty(encryptedMandantGuid) Then
                Throw New ArgumentNullException("encryptedMandantGuid")
            End If

            Dim mandantGuid As String = Utility.Utility.DecryptString(encryptedMandantGuid, My.Settings.CryptographyKey)

            ' Load branches by canton.
            Dim branches = DependencyResolver.Current.GetService(Of Branches).ReadBranches(mandantGuid, cantonAbbreviation)

            Dim listOfSelectOptions As New List(Of SelectListItem)

            ' Create the select box items.
            For Each branchKey As String In branches.AllKeys
                listOfSelectOptions.Add(New SelectListItem() With {.Text = branchKey, .Value = branchKey})
            Next

            Return Me.Json(listOfSelectOptions)

        End Function

        ''' <summary>
        ''' Action to searches vacancies by canton, branch and vacancy title.
        ''' </summary>
        ''' <param name="encryptedMandantGuid">The encrypted mandant guid.</param>
        ''' <param name="cantonAbbreviation">The canton abbreviation.</param>
        ''' <param name="branch">The branch.</param>
        ''' <param name="vacancyTitle">The vacancy title.</param>
        ''' <returns>List of vacancies.</returns>
        Public Function SearchVacancies(ByVal encryptedMandantGuid As String, ByVal cantonAbbreviation As String, ByVal branch As String, ByVal vacancyTitle As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As JsonResult

            If String.IsNullOrEmpty(encryptedMandantGuid) Then
                Throw New ArgumentNullException("encryptedMandantGuid")
            End If

            Dim mandantGuid As String = Utility.Utility.DecryptString(encryptedMandantGuid, My.Settings.CryptographyKey)

            ' Search for vacancies.
            Dim vacancies As List(Of SearchItem) = DependencyResolver.Current.GetService(Of Vacancies).ReadVacancies(mandantGuid, vacancyTitle, cantonAbbreviation, branch, clientLatitude, clientLongitude)

            ' Encrypt ID
            For Each vacancy In vacancies
                vacancy.EncryptedID = Utility.Utility.EncryptString(vacancy.ID.ToString, My.Settings.CryptographyKey)
            Next

            Return Json(vacancies)

        End Function

        ''' <summary>
        ''' Action to searches candidates by a job qualification title.
        ''' </summary>
        ''' <param name="encryptedMandantGuid">The encrypted mandant guid.</param>
        ''' <param name="jobQualificationSearchValue">The canton abbreviation.</param>
        ''' <returns>List of candidates.</returns>
        Public Function SearchCandidates(ByVal encryptedMandantGuid As String, ByVal jobQualificationSearchValue As String, ByVal clientLatitude As String, ByVal clientLongitude As String) As JsonResult

            If String.IsNullOrEmpty(encryptedMandantGuid) Then
                Throw New ArgumentNullException("encryptedMandantGuid")
            End If

            Dim mandantGuid As String = Utility.Utility.DecryptString(encryptedMandantGuid, My.Settings.CryptographyKey)

            ' Search for vacancies.
            Dim candidates As List(Of SearchItem) = DependencyResolver.Current.GetService(Of Candidates).ReadCandidates(mandantGuid, jobQualificationSearchValue, clientLatitude, clientLongitude)

            ' Encrypt ID
            For Each candidate In candidates
                candidate.EncryptedID = Utility.Utility.EncryptString(candidate.ID.ToString, My.Settings.CryptographyKey)
            Next

            Return Json(candidates)

        End Function

        ''' <summary>
        '''  Action to read the details of a vacancy.
        ''' </summary>
        ''' <param name="encryptedMandantGuid">The encrypted mandant guid.</param>
        ''' <param name="encryptedVacancyId">The encrypted vacancy id.</param>
        ''' <param name="encryptedLanguage">The encrypted language.</param>
        ''' <returns>The vacancy details.</returns>
        Public Function ReadVacancyDetails(ByVal encryptedMandantGuid As String, ByVal encryptedVacancyId As String, ByVal encryptedLanguage As String) As JsonResult

            Dim cryptographyKey As String = My.Settings.CryptographyKey
            Dim mandantGuid As String = Utility.Utility.DecryptString(encryptedMandantGuid, cryptographyKey)
            Dim vacancyId As String = Utility.Utility.DecryptString(encryptedVacancyId, cryptographyKey)
            Dim language As String = Utility.Utility.DecryptString(encryptedLanguage, cryptographyKey)

            Dim vacancyDetails As ISearchResultDetails
            Try
                vacancyDetails = DependencyResolver.Current.GetService(Of Vacancies).ReadVacancyDetails(mandantGuid, vacancyId, language)
            Catch ex As Exception
                Return Json(String.Empty)
            End Try

            If Not IsNothing(vacancyDetails) Then
                ' Serialize data in json fromat.
                Return Json(vacancyDetails)
            Else
                Return Json(String.Empty)
            End If

        End Function


        ''' <summary>
        '''  Action to read the details of a candidate.
        ''' </summary>
        ''' <param name="encryptedMandantGuid">The encrypted mandant guid.</param>
        ''' <param name="encryptedCandidateId">The encrypted candidate id.</param>
        ''' <param name="encryptedLanguage">The encrypted language.</param>
        ''' <returns>The vacancy details.</returns>
        Public Function ReadCandidateDetails(ByVal encryptedMandantGuid As String, ByVal encryptedCandidateId As String, ByVal encryptedLanguage As String) As JsonResult

            Dim cryptographyKey As String = My.Settings.CryptographyKey
            Dim mandantGuid As String = Utility.Utility.DecryptString(encryptedMandantGuid, cryptographyKey)
            Dim candidateId As String = Utility.Utility.DecryptString(encryptedCandidateId, cryptographyKey)
            Dim language As String = Utility.Utility.DecryptString(encryptedLanguage, cryptographyKey)

            Dim vacancyDetails As ISearchResultDetails
            Try
                vacancyDetails = DependencyResolver.Current.GetService(Of Candidates).ReadCandidateDetails(mandantGuid, candidateId, language)
            Catch ex As Exception
                Return Json(String.Empty)
            End Try

            If Not IsNothing(vacancyDetails) Then
                ' Serialize data in json fromat.
                Return Json(vacancyDetails)
            Else
                Return Json(String.Empty)
            End If

        End Function

        
#End Region

    End Class
End Namespace
