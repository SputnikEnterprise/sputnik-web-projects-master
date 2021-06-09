'------------------------------------
' File: PeerCertificateValidator.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IdentityModel.Selectors
Imports System.Security.Cryptography.X509Certificates
Imports System.IdentityModel.Tokens

''' <summary>
''' Validates the certificate of the upload service.
''' </summary>
Public Class PeerCertificateValidator
    Inherits X509CertificateValidator

    Public Overrides Sub Validate(ByVal certificate As X509Certificate2)
        If Not My.Settings.ServerCertThumbprint = certificate.Thumbprint Then
            Throw New SecurityTokenValidationException("Service certificate is not valid.")
        End If
    End Sub
End Class
