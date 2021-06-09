@ModelType JobFinderApp.ViewModels.IndexViewModel

<div data-role="page" id = "about" data-add-back-btn="true">      
	<div data-role="header" data-id="aboutHeader" data-position="fixed">
		<h2>@Model.TranslationService.GetTranslation("TEXT_PAGE_ABOUT_TITLE", Model.Language)</h2>
	</div><!-- /header -->      
      
     <div data-role="content" id="AboutContentWrapper">
        <div id="aboutHeader" style="text-align: center">
            <strong>@Model.Mandant.Name</strong>
        </div>
        <div id="aboutIcon" style="text-align: center">
            <img src="@Model.Mandant.IconUrl" alt="Icon" />
        </div>
        <div id="aboutHomePage" style="text-align: center">
            <a href="@Model.Mandant.HomePage">@Model.Mandant.HomePage</a>
        </div>
        <div id="aboutEmailAddress" style="text-align: center">
            <a href="mailto:@Model.Mandant.EmailAddress">@Model.Mandant.EmailAddress</a>
        </div>
     </div>
	 <!-- /content -->

    <div data-role="footer" data-id="myfooter" data-position="fixed" data-theme="c">
        <h3>@Model.TranslationService.GetTranslation("TEXT_PAGE_ABOUT_COPYRIGHT", Model.Language)</h3>
    </div>
    <!-- /footer -->
</div>

<!-- The about info url-->    
<input id="hidAboutInfoUrl" type="hidden" value="@Model.Mandant.AboutInfoUrl" />