@ModelType JobFinderApp.ViewModels.IndexViewModel

@Code
    Dim headerTitle As String = "App"
    Dim footerHomeTitle As String = "Home"
    Dim footerResultTitle As String = "Results"
    Dim footerSearchDetailsTitle As String = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_DETAILS", Model.Language)
    If Model.Role = JobFinderApp.Contracts.Role.JobSeeker Then
        headerTitle = Model.TranslationService.GetTranslation("TEXT_PAGE_JOBRESULTS_TITLE", Model.Language)
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBSEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBS", Model.Language)
    ElseIf Model.Role = JobFinderApp.Contracts.Role.CandidateSeeker Then
        headerTitle = Model.TranslationService.GetTranslation("TEXT_PAGE_CANDIDATERESULTS_TITLE", Model.Language)
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATESEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATES", Model.Language)
    End If    
End Code

<div data-role="page" id="results" >      
	<div class="ui-bar ui-bar-a header" data-position="fixed">
        <div class="headerDiv">
            <img class="headerIcon" src="@Model.Mandant.IconUrl" alt="Icon" />
        </div>
        <div class="headerTextDiv" >
            <h2>@headerTitle</h2>
        </div>
        <a href="#about"style="float: right" class="ui-btn-right" data-icon="info" data-transition="flip">@Model.TranslationService.GetTranslation("TEXT_HEADER_INFO", Model.Language)</a>
    </div>    
    <div id="paginationNavBar" data-role="navbar">
        <ul>
            <li><a id="btnBack" href="#" data-icon="arrow-l">Back</a></li>
            <li><a id="btnForward" href="#" data-icon="arrow-r">Forward</a></li>
        </ul>
    </div>
    <!-- /navbar -->     
	 <div data-role="content">	
	 	<div id="searchResults">
       </div> 
	 </div><!-- /content -->

    <div data-role="footer" data-id="myfooter" data-position="fixed" data-theme="c">
        <div data-role="navbar" data-iconpos="bottom">
            <ul>
                <li><a href="#home" data-icon="home" class="ui-state-persist" data-transition="flip">@footerHomeTitle</a></li>
                <li><a href="#results" data-icon="search" class="ui-btn-active ui-state-persist" data-transition="flip">@footerResultTitle</a></li>
                <li><a href="#resultDetail" data-icon="info" class="ui-state-persist" data-transition="slide">@footerSearchDetailsTitle</a></li>
            </ul>
        </div>
    </div>
    <!-- /footer -->
</div>
