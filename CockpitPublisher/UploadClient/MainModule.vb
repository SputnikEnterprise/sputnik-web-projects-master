'------------------------------------
' File: Module1.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports UploadClient.CockpitWCFServices
Imports log4net.Config
Imports System.ServiceModel.Channels
Imports System.ServiceModel.Security.Tokens
Imports System.ServiceModel
Imports System.Security.Cryptography.X509Certificates
Imports System.ServiceModel.Description

Module MainModule

    ''' <summary>
    ''' Application entry point.
    ''' </summary>
    Sub Main()

        ' Configure log4net
        XmlConfigurator.Configure()

        ' Create helper objects
        Dim binding As Binding = CreateServiceBinding()
        Dim endpoint As EndpointAddress = CreateEndpointAddress()

        Dim uploadService As New TableDataUploadServiceClient(binding, endpoint)
        SetBehavior(uploadService)

        Dim logger As New DefaultLogger(uploadService)
        Dim databaseAccess As New DatabaseAccess(logger)

        ' Create the uploader component
        Dim tableUploader As New TableUploader(uploadService, databaseAccess, logger)

        ' Begin uploading tables
        tableUploader.UploadTables()

    End Sub

    ''' <summary>
    ''' Creates the service binding
    ''' </summary>
    Private Function CreateServiceBinding() As Binding
        Dim binding = New System.ServiceModel.WSHttpBinding()
        binding.CloseTimeout = New TimeSpan(0, 1, 0)
        binding.OpenTimeout = New TimeSpan(0, 1, 0)
        binding.ReceiveTimeout = New TimeSpan(0, 10, 0)
        binding.SendTimeout = New TimeSpan(0, 1, 0)

        binding.BypassProxyOnLocal = False
        binding.TransactionFlow = False
        binding.HostNameComparisonMode = ServiceModel.HostNameComparisonMode.StrongWildcard
        binding.MaxBufferPoolSize = 524288
        binding.MaxReceivedMessageSize = 65536
        binding.MessageEncoding = ServiceModel.WSMessageEncoding.Mtom
        binding.TextEncoding = System.Text.Encoding.UTF8
        binding.UseDefaultWebProxy = True
        binding.AllowCookies = False

        binding.ReaderQuotas.MaxDepth = 32
        binding.ReaderQuotas.MaxStringContentLength = 20000000
        binding.ReaderQuotas.MaxArrayLength = 20000000
        binding.ReaderQuotas.MaxBytesPerRead = 4096
        binding.ReaderQuotas.MaxNameTableCharCount = 16384

        binding.ReliableSession.Ordered = True
        binding.ReliableSession.InactivityTimeout = New TimeSpan(0, 10, 0)
        binding.ReliableSession.Enabled = True

        binding.Security.Mode = ServiceModel.SecurityMode.Message
        binding.Security.Message.ClientCredentialType = ServiceModel.MessageCredentialType.None
        binding.Security.Message.NegotiateServiceCredential = True
        binding.Security.Message.AlgorithmSuite = System.ServiceModel.Security.SecurityAlgorithmSuite.Default
        binding.Security.Message.EstablishSecurityContext = True


        Dim customBinding = New CustomBinding(binding)
        customBinding.Name = "WSHttpBinding_ITableDataUploadService"
        Dim security As SymmetricSecurityBindingElement = customBinding.Elements.Find(Of SymmetricSecurityBindingElement)()
        security.LocalClientSettings.MaxClockSkew = TimeSpan.MaxValue
        security.LocalClientSettings.DetectReplays = False
        security.LocalServiceSettings.MaxClockSkew = TimeSpan.MaxValue
        security.LocalServiceSettings.DetectReplays = False

        Dim secureTokenParams As SecureConversationSecurityTokenParameters = CType(security.ProtectionTokenParameters, SecureConversationSecurityTokenParameters)
        Dim bootstrapElement As SecurityBindingElement = secureTokenParams.BootstrapSecurityBindingElement
        bootstrapElement.LocalClientSettings.MaxClockSkew = New TimeSpan(24, 0, 0)
        bootstrapElement.LocalServiceSettings.MaxClockSkew = New TimeSpan(24, 0, 0)



        Return customBinding
    End Function

    ''' <summary>
    ''' Creates the service endpoint address
    ''' </summary>
    Private Function CreateEndpointAddress() As EndpointAddress

        ' Create EndpointIdentity
        Dim encodedCertificate = My.Settings.Certificate
        Dim certificateCollection As X509Certificate2Collection = New X509Certificate2Collection()
        certificateCollection.Import(Convert.FromBase64String(encodedCertificate))
        Dim certificate As X509Certificate2 = certificateCollection(0)
        certificateCollection.RemoveAt(0)
        Dim identity = EndpointIdentity.CreateX509CertificateIdentity(certificate)

        ' Create Address Uri
        Dim endpointUri = New Uri(My.Settings.EndpointAddress)

        ' Create EndpointAddress
        Dim endpointAddress As EndpointAddress = New EndpointAddress(endpointUri,
                                                                     identity,
                                                                     New AddressHeaderCollection())



        Return endpointAddress
    End Function

    ''' <summary>
    ''' Sets the behavior of the service.
    ''' </summary>
    Private Sub SetBehavior(ByRef serviceClient As TableDataUploadServiceClient)
        serviceClient.ClientCredentials.ServiceCertificate.Authentication.CertificateValidationMode = Security.X509CertificateValidationMode.Custom
        serviceClient.ClientCredentials.ServiceCertificate.Authentication.CustomCertificateValidator = New UploadClient.PeerCertificateValidator()
    End Sub


End Module
