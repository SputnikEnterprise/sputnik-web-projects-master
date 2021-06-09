@ModelType JobFinderApp.ViewModels.IndexViewModel    
    
@Code
    Dim headerTitle As String = "App"
    Dim footerHomeTitle As String = "Home"
    Dim footerResultTitle As String = "Results"
    Dim footerSearchDetailsTitle As String = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_DETAILS", Model.Language)
    If Model.Role = JobFinderApp.Contracts.Role.JobSeeker Then
        headerTitle = Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_JOBSEEKER_TITLE", Model.Language)
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBSEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBS", Model.Language)
    ElseIf Model.Role = JobFinderApp.Contracts.Role.CandidateSeeker Then
        headerTitle = Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_CANDIDATESEEKER_TITLE", Model.Language)
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATESEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATES", Model.Language)
    End If    
End Code

<div data-role="page" id="home">
    <div class="ui-bar ui-bar-a header" data-position="fixed">
        <div class="headerDiv">
            <img class="headerIcon" src="@Model.Mandant.IconUrl" alt="Icon" />
        </div>
        <div class="headerTextDiv" >           
            <h2>@headerTitle</h2>
        </div>
        <a href="#about"style="float: right" class="ui-btn-right" data-icon="info" data-transition="flip">@Model.TranslationService.GetTranslation("TEXT_HEADER_INFO", Model.Language)</a>
    </div>
    <!-- /header -->
    <div data-role="content">
     
        @If Model.Role = JobFinderApp.Contracts.Role.JobSeeker Then
            @<!-- Cantons selection -->
            @<label for="cmbCantons">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_CANTON", Model.Language)</label>
            @Html.DropDownList("cmbCantons", CType(Model.CantonsSelectListItems, IEnumerable(Of SelectListItem)))
       
            @<!-- Branch selection -->
            @<label for="cmbBranches">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_BRANCH", Model.Language)</label>
            @Html.DropDownList("cmbBranches", CType(Model.BranchesSelectListItems, IEnumerable(Of SelectListItem)))       
       
            @<!-- Vacancy title free text. -->
            @<label for="vacancyTitle">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_IAMLOOKINGFOR_JOB", Model.Language)</label>
            @<input type="search" name="vacancyTitle" id="vacancyTitle" value=""/>
                
            @<br />
            @<br />

            @<!-- Search button  -->
            @<a href="#" id="btnSearchVacancies" data-role="button" data-icon="gear">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_STARTSEARCH", Model.Language)</a>
        ElseIf Model.Role = JobFinderApp.Contracts.Role.CandidateSeeker Then
            
            @<!-- Job title selection -->
            @<label for="cmbJobQualifications">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_JOB_QUALIFICATION", Model.Language)</label>
            @Html.DropDownList("cmbJobQualifications", CType(Model.JobQualificationSelectListItems, IEnumerable(Of SelectListItem)))       
       
            @<br />
            @<br />

            @<!-- Search button  -->
            @<a href="#" id="btnSearchCandidates" data-role="button" data-icon="gear">@Model.TranslationService.GetTranslation("TEXT_PAGE_HOME_STARTSEARCH", Model.Language)</a>
        End If    
    </div>
    <!-- /content -->
    <div data-role="footer" data-id="myfooter" data-position="fixed" data-theme="c">
        <div data-role="navbar" data-iconpos="bottom">
            <ul>
                <li><a href="#home" data-icon="home" class="ui-btn-active ui-state-persist" data-transition="flip">@footerHomeTitle</a></li>
                <li><a href="#results" data-icon="search" class="ui-state-persist" data-transition="flip">@footerResultTitle</a></li>
                <li><a href="#resultDetail" data-icon="info" class="ui-state-persist" data-transition="flip">@footerSearchDetailsTitle</a></li>
            </ul>
        </div>
    </div>
    <!-- /footer -->
</div>
