Imports JobFinderApp

Imports Microsoft.Web.Infrastructure.DynamicModuleHelper
Imports Ninject
Imports Ninject.Web.Mvc

Imports JobFinderApp.Contracts
Imports JobFinderApp.DataAccess
Imports JobFinderApp.Domain.Services
Imports JobFinderApp.Services

<Assembly: WebActivator.PreApplicationStartMethod(GetType(JobFinderApp.App_Start.NinjectMVC3), "StartNinject")> 
<Assembly: WebActivator.ApplicationShutdownMethodAttribute(GetType(JobFinderApp.App_Start.NinjectMVC3), "StopNinject")> 

Namespace App_Start

    Public Module NinjectMVC3

        Private ReadOnly bootstrapper As New Bootstrapper()

        ''' <summary>
        ''' Starts the application
        ''' </summary>
        Public Sub StartNinject()
            DynamicModuleUtility.RegisterModule(GetType(OnePerRequestModule))
            DynamicModuleUtility.RegisterModule(GetType(HttpApplicationInitializationModule))
            bootstrapper.Initialize(AddressOf CreateKernel)
        End Sub

        ''' <summary>
        ''' Stops the application.
        ''' </summary>
        Public Sub StopNinject()
            bootstrapper.ShutDown()
        End Sub

        ''' <summary>
        ''' Creates the kernel that will manage your application.
        ''' </summary>
        ''' <returns>The created kernel.</returns>
        Private Function CreateKernel() As IKernel
            Dim kernel = New StandardKernel()
            RegisterServices(kernel)
            Return kernel
        End Function

        ''' <summary>
        ''' Load your modules or register your services here!
        ''' </summary>
        ''' <param name="kernel">The kernel.</param>
        Private Sub RegisterServices(ByVal kernel As IKernel)
            kernel.Bind(Of ILoggingService)().To(Of Log4NetLoggingService)()
            kernel.Bind(Of IRepositoryService)().To(Of RepositoryService)().WithConstructorArgument("connectionString", ConfigurationManager.ConnectionStrings("SpContractEntities").ConnectionString)
            kernel.Bind(Of ITranslationService)().To(Of XmlTranslationService)()
            kernel.Bind(Of IConfigService)().To(Of XmlConfigService)()
            kernel.Bind(Of IStatisticsService)().To(Of StatisticsService)()
        End Sub

    End Module
End Namespace