@ModelType JobFinderApp.ViewModels.IndexViewModel

    
@Code
    Dim headerTitle As String = Model.TranslationService.GetTranslation("TEXT_PAGE_RESULTDETAIL_TITLE", Model.Language)
    Dim footerHomeTitle As String = "Home"
    Dim footerResultTitle As String = "Results"
    Dim footerSearchDetailsTitle As String = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_DETAILS", Model.Language)
    If Model.Role = JobFinderApp.Contracts.Role.JobSeeker Then
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBSEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_JOBS", Model.Language)
    ElseIf Model.Role = JobFinderApp.Contracts.Role.CandidateSeeker Then
        footerHomeTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATESEARCH", Model.Language)
        footerResultTitle = Model.TranslationService.GetTranslation("TEXT_FOOTER_TAB_CANDIDATES", Model.Language)
    End If    
End Code

<div data-role="page" id="resultDetail" >      
	<div class="ui-bar ui-bar-a header" data-position="fixed">
        <div class="headerDiv">
            <img class="headerIcon" src="@Model.Mandant.IconUrl" alt="Icon" />
        </div>
        <div class="headerTextDiv" >
            <h2>@headerTitle</h2>
        </div>
        <a href="#about"style="float: right" class="ui-btn-right" data-icon="info" data-transition="flip">@Model.TranslationService.GetTranslation("TEXT_HEADER_INFO", Model.Language)</a>
    </div>
      
	 <div data-role="content">	
	 	<em><strong id="ResultTitle"></strong></em>
        <br /> <br /> 
        <!-- Regular vacancy values -->      
        <div id="resultDetailsRegularValuesWrapper"></div>
        <div id="resultContactPicture" style="display:none"></div>
  
        <!-- Contact data -->      
        <div id="resultDetailsContactWrapper">
            <hr  style="display:none"/>
            <strong id="resultContactTitle"></strong>
            <br /> <br />
            <!-- Telephone -->    
            <div id="resultContactTelephone">
                <span id="resultContactTelephoneTitle"></span>
                <a id="resultContactTelephoneValue" href="#"></a>
            </div>
            <br/>
            <!-- Email -->  
            <div id="resultContactEmail">
                <span id="resultContactEmailTitle"></span>
                <a id="resultContactEmailValue" href="#"></a>
            </div>
        </div>
	 </div><!-- /content -->

    <div data-role="footer" data-id="myfooter" data-position="fixed" data-theme="c">
        <div data-role="navbar" data-iconpos="bottom">
            <ul>
                <li><a href="#home" data-icon="home" class="ui-state-persist" data-transition="flip">@footerHomeTitle</a></li>
                <li><a href="#results" data-icon="search" class="ui-state-persist" data-transition="slide" data-direction="reverse">@footerResultTitle</a></li>
                <li><a href="#resultDetail" data-icon="info" class="ui-btn-active ui-state-persist" data-transition="flip">@footerSearchDetailsTitle</a></li>
            </ul>
        </div>
    </div>
    <!-- /footer -->
</div>