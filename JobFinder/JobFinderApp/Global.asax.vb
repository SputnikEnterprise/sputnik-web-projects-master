'------------------------------------
' File: Global.asax.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

' Note: For instructions on enabling IIS6 or IIS7 classic mode, 
' visit http://go.microsoft.com/?LinkId=9394802

Imports JobFinderApp.Contracts
Imports JobFinderApp.Services
Imports JobFinderApp.Domain

Namespace jQueryMobileTemplate

    Public Class MvcApplication
        Inherits System.Web.HttpApplication

#Region "Private Consts"
        ''' <summary>
        ''' The translations xml file.
        ''' </summary>
        Public Const TRANSLATIONS_XML = "~/Config/strings.xml"
#End Region

#Region "Public properties"

        ''' <summary>
        ''' Value indicating whether this app should be in debug mode.
        ''' </summary>
        Public Shared ReadOnly Property IsDebug As Boolean
            Get

#If DEBUG Then
                Dim buildIsDebug As Boolean = True
#Else
                Dim buildIsDebug As Boolean = False
#End If
                Return buildIsDebug

            End Get
        End Property

#End Region


        Shared Sub RegisterGlobalFilters(ByVal filters As GlobalFilterCollection)
            filters.Add(New HandleErrorAttribute())
        End Sub

        Shared Sub RegisterRoutes(ByVal routes As RouteCollection)
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}")

            ' MapRoute takes the following parameters, in order:
            ' (1) Route name
            ' (2) URL with parameters
            ' (3) Parameter defaults
            ' (4) Parameter constraints
            routes.MapRoute( _
                "Default", _
                "{controller}/{action}/{stream}", _
                New With {.controller = "Home", .action = "Index", .stream = UrlParameter.Optional}, _
                New With {.controller = "Home"})

            routes.MapRoute( _
                "GetApp", _
                "{controller}/{action}/{mandantGuid}/{language}/{role}",
                New With {.controller = "AppRequest", .action = "Create", .mandantGuid = "", .language = "DE", .role = "JobSeeker"}, _
                New With {.controller = "AppRequest"})

            routes.MapRoute( _
                "GetFiles", _
                "{controller}/{action}/{mandantGuid}",
                New With {.controller = "File", .action = "GetMandantIcon", .mandantGuid = UrlParameter.Optional}, _
                New With {.controller = "File"})

        End Sub

        Sub Application_Start()
            AreaRegistration.RegisterAllAreas()

            RegisterGlobalFilters(GlobalFilters.Filters)
            RegisterRoutes(RouteTable.Routes)

            ' Load the translations form an xml file.
            Dim translationSerivce As ITranslationService = DependencyResolver.Current.GetService(Of ITranslationService)()
            translationSerivce.LoadTranslations(Server.MapPath(TRANSLATIONS_XML))

            ' Add the translation service to the application cache.
            HttpRuntime.Cache.Add(Constants.TRANSLATION_SERVICE_CACHE_KEY, translationSerivce, Nothing, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.High, Nothing)

            ' Inject Logger into Utility.
            JobFinderApp.Domain.Utility.Utility.Logger = DependencyResolver.Current.GetService(Of ILoggingService)()
        End Sub

    End Class

End Namespace
