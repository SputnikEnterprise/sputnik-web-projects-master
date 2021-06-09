'------------------------------------
' File: IndexViewModel.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts
Imports JobFinderApp.Domain

Namespace ViewModels

    ''' <summary>
    ''' Index view model for index page.
    ''' </summary>
    Public Class IndexViewModel

#Region "Private fields"
        Private iTranslationService As ITranslationService
        Private iConfigService As IConfigService
#End Region

#Region "Public Properties"

        ''' <summary>
        ''' The encrypted mandant guid.
        ''' </summary>
        Public Property EncryptedMandantGuid As String

        ''' <summary>
        ''' The canton select list items for the cantons filter.
        ''' </summary>
        Public Property CantonsSelectListItems As List(Of SelectListItem)

        ''' <summary>
        ''' The select list items for the branch filter.
        ''' </summary>
        Public Property BranchesSelectListItems As List(Of SelectListItem)

        ''' <summary>
        ''' The select list items for the job qualification filter.
        ''' </summary>
        Public Property JobQualificationSelectListItems As List(Of SelectListItem)

        ''' <summary>
        ''' The iso language.
        ''' </summary>
        Public Language As IsoLanguage

        ''' <summary>
        ''' The Role.
        ''' </summary>
        Public Role As Role

        ''' <summary>
        ''' The encrypted language.
        ''' </summary>
        Public EncryptedLanguage As String

        ''' <summary>
        ''' The translation service.
        ''' </summary>
        Public ReadOnly Property TranslationService As ITranslationService
            Get
                Return Me.iTranslationService
            End Get
        End Property

        ''' <summary>
        ''' The config service.
        ''' </summary>
        Public ReadOnly Property ConfigurationService As IConfigService
            Get
                Return Me.iConfigService
            End Get
        End Property

        ''' <summary>
        ''' Details for about page.
        ''' </summary>
        Public Property Mandant As Mandant
#End Region


#Region "Constructor"

        Public Sub New(ByVal mandant As Mandant, ByVal translationService As ITranslationService, ByVal configurationService As IConfigService, ByVal language As IsoLanguage, ByVal role As Role)
            Dim cryptographyKey As String = My.Settings.CryptographyKey
            Me.EncryptedMandantGuid = Utility.Utility.EncryptString(mandant.Guid, cryptographyKey)
            Me.Mandant = mandant
            Me.iTranslationService = translationService
            Me.iConfigService = configurationService
            Me.Language = language
            Me.Role = role
            Me.EncryptedLanguage = Utility.Utility.EncryptString(language.ToString(), cryptographyKey)

            ' The canton select list items.
            Dim cantonSelectListItems As New List(Of SelectListItem)
            Dim cantons = DependencyResolver.Current.GetService(Of Cantons).ReadCantons(mandant.Guid)
            For Each cantonKey In cantons.AllKeys
                cantonSelectListItems.Add(New SelectListItem() With {.Text = cantons(cantonKey), .Value = cantonKey})
            Next

            ' Sort the select list
            cantonSelectListItems.OrderBy(Function(item As SelectListItem) item.Text)

            ' Add default option at first position (this is required by client side javacript code).
            cantonSelectListItems.Insert(0, New SelectListItem() With {.Text = translationService.GetTranslation("TEXT_SELECT_OPTION_ALL", language), .Value = Constants.SELECT_ALL_WILDCARD})


            ' The initial branch select list items (all the existing branches).
            Dim branchesSelectListItems As New List(Of SelectListItem)
            Dim branches = DependencyResolver.Current.GetService(Of Branches).ReadBranches(mandant.Guid)
            For Each branchKey In branches.AllKeys
                branchesSelectListItems.Add(New SelectListItem() With {.Text = branches(branchKey), .Value = branchKey})
            Next

            branchesSelectListItems.OrderBy(Function(item As SelectListItem) item.Text)

            '  Add default option at first position.
            branchesSelectListItems.Insert(0, New SelectListItem() With {.Text = translationService.GetTranslation("TEXT_SELECT_OPTION_ALL", language), .Value = Constants.SELECT_ALL_WILDCARD})

            ' The initial job qualification select list items (all the existing job titles).
            Dim jobQualificationSelectListItems As New List(Of SelectListItem)
            Dim jobQualifications = DependencyResolver.Current.GetService(Of Candidates).ReadJobQualifications(mandant.Guid)
            For Each jobQualificationKey In jobQualifications.AllKeys
                jobQualificationSelectListItems.Add(New SelectListItem() With {.Text = jobQualifications(jobQualificationKey), .Value = jobQualificationKey})
            Next

            jobQualificationSelectListItems.OrderBy(Function(item As SelectListItem) item.Text)

            '  Add default option at first position.
            jobQualificationSelectListItems.Insert(0, New SelectListItem() With {.Text = translationService.GetTranslation("TEXT_SELECT_OPTION_ALL", language), .Value = Constants.SELECT_ALL_WILDCARD})


            Me.CantonsSelectListItems = cantonSelectListItems
            Me.BranchesSelectListItems = branchesSelectListItems
            Me.JobQualificationSelectListItems = jobQualificationSelectListItems
        End Sub

#End Region

    End Class

End Namespace
