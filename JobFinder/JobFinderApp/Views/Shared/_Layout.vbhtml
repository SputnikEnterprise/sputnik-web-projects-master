<!DOCTYPE html>
<html>
    <head>
        <title>@ViewData("Title")</title>

        <meta charset="utf-8" />
        <meta name="description" content="goodjobs App for mobile devices" />
        <meta name="author" content="Sputnik Informatik GmbH" />
        <!--  Mobile viewport optimized: j.mp/bplateviewport -->
        <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />

        <link rel="shortcut icon" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder57.png")" />
                
        <!-- Region Apple support-->
        <meta name="apple-mobile-web-app-capable" content="yes" />

        <link rel="apple-touch-startup-image" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder57.png")" />
        <link rel="apple-touch-icon-precomposed" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder57.png")" />

        <!-- Home Screen Icons: 
            57×57 = iphone3
            114×114 = iphone4
            72×72 = ipad
        -->
        <link rel="apple-touch-icon" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder57.png")" />
        <link rel="apple-touch-icon" sizes="72x72" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder72.png")" />
        <link rel="apple-touch-icon" sizes="114x114" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/jobFinder114.png")" />
        <!-- End region apple support -->

        <!-- CSS Loading -->
        @* ' Reference the minified and combined of css based on whether we are in debug mode. *@
        @If JobFinderApp.jQueryMobileTemplate.MvcApplication.IsDebug Then
            @:<link rel="stylesheet" href="@Url.Content("~/Content/jquery.mobile-1.0.css")" />
        Else
            @:<link rel="stylesheet" href="@Url.Content("~/Content/CombinedStyleSheet.css")"/>
        End If
        @* ' Alternatively load jQuery Mobile Latest Styles
        <link rel="stylesheet" href="http://code.jquery.com/mobile/1.0/jquery.mobile-1.0.min.css" />
        *@

        <!-- Include Mandant Specific CSS -->
        <link rel="stylesheet" href="@Url.Content("~/Content/Mandants/" + ViewData("MandantGuid") + "/MandantStyle.css")" />
            
        <!-- Instantiate global variables. -->
        <script type="text/javascript">

            // Translations used by javasscript code.
            // <![CDATA[
            var clientSideTranslations = {
                "TEXT_PLEASE_WAIT": '@ViewData("PleaseWaitMessage")',
                "TEXT_INSTALL_MSG1": '@Html.Raw(ViewData("InstallMsg1"))',
                "TEXT_INSTALL_MSG2": '@Html.Raw(ViewData("InstallMsg2"))',
                "TEXT_BACK_BUTTON": '@Html.Raw(ViewData("BackButton"))'
            };

            // Needed for Ajax-Calls:
            var applicationPath = '@ViewData("applicationPath")'

            // ]]>

        </script>

        @* ' Reference the minified and combined js based on whether we are in debug mode. *@
        @If JobFinderApp.jQueryMobileTemplate.MvcApplication.IsDebug Then
            ' Load jQuery from local site
            ' We put these at the top because jquery mobile applies styles before the page finishes loading
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/jquery-1.6.4.js")"></script>
            @*
            <!-- Optionally grab Google CDN's jQuery. fall back to local if necessary -->
            <script type="text/javascript" src="http://code.jquery.com/jquery-1.6.4.js"></script>
            *@
    
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/bookmark_bubble.js")"></script>
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/example.js")"></script>
        
            ' Load  jQuery Mobile from local site 
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/jquery.mobile-1.0.js")"></script>
            @* 
            <!-- Optionally from jquery CDN, get latest builds -->
            <script type="text/javascript" src="http://code.jquery.com/mobile/1.0/jquery.mobile-1.0.js"></script>
            *@
            
            ' Load common scripts of job finder app page. 
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/jobFinderApp.js")"></script>           

        Else
            @:<script type="text/javascript" src="@Url.Content("~/Scripts/CombinedJavaScript.js")"></script>
        End If
    </head>
    <body>
        @RenderBody()
    </body>
</html>
