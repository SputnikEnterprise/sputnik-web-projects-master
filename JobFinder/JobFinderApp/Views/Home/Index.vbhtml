
@ModelType JobFinderApp.ViewModels.IndexViewModel

<!-- Render home partial view -->
@Html.Partial("Home", Model)

<!-- Render about partial view -->
@Html.Partial("About", Model)

<!-- Render results partial view -->
@Html.Partial("Results", Model)

<!-- Render vacancy details partial view -->
@Html.Partial("ResultDetail", Model)

<!-- The encrypted mandant guid -->    
<input id="hidMDGuid" type="hidden" value="@Model.EncryptedMandantGuid" /> 

<!-- The encrypted language-->    
<input id="hidLanguage" type="hidden" value="@Model.EncryptedLanguage" /> 