Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports Imt.Common.Utility

<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ScriptService()> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class EmailService
    Inherits System.Web.Services.WebService

    <WebMethod(EnableSession:=True)> _
    Public Function SendMessageTo(ByVal ToEmailAdress As String, ByVal Subject As String, ByVal Message As String, ByVal ReplyToEmail As String) As Boolean
        If Message.Length = 0 Then
            Return False
        Else

            '#If DEBUG Then
            '                ToEmailAdress = "leanza@imt.ch"
            '#End If

            Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
			Imt.Common.Utility.MailHandler.SendMail(New System.Net.Mail.MailMessage(),
																										ToEmailAdress,
																										Message,
																										Subject,
																										appInfo.SmtpAddress,
																										25,
																										ReplyToEmail)
			Return True
        End If

    End Function
End Class