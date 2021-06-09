'------------------------------------
' File: GetAppController.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Domain
Imports JobFinderApp.Contracts

Namespace Controllers

    <HandleError(View:="ErrorView")> _
    Public Class AppRequestController
        Inherits System.Web.Mvc.Controller

#Region "Private Fields"

        ''' <summary>
        ''' The statistics.
        ''' </summary>
        Private statistics As IStatisticsService

#End Region

#Region "Constructor"

        Public Sub New(ByVal statistics As IStatisticsService)
            Me.statistics = statistics
        End Sub

#End Region

#Region "Public Methods (Actions)"

        Public Function Create(ByVal mandantGuid As String, ByVal language As String, ByVal role As String) As RedirectResult

            ' Check for mandatory parameter:
            If String.IsNullOrEmpty(mandantGuid) Then
                Utility.Utility.ThrowTimestampedException("Mandatory parameter ""MandantGuid"" was not provided.")
            End If

            'Check whether cookie exists, and if so, use it.
            Dim cookie As HttpCookie = Request.Cookies("goodjobsCookie")
            Dim appGuid As String = Nothing
            If cookie IsNot Nothing Then
                ' If it exist, pick the already existing application guid.
                appGuid = cookie("AppGuid")
            Else
                ' If cookie does not yet exist, create a new one that never expires.
                cookie = New HttpCookie("goodjobsCookie")
                cookie.Expires = DateTime.MaxValue
                appGuid = Guid.NewGuid().ToString("D").ToUpper()
                cookie("AppGuid") = appGuid
                Response.Cookies.Add(cookie)
            End If

            ' Updade statistics, ignore result: independently of result, the registration should continue.
            Me.statistics.UpdateDownloadStatistics(mandantGuid, appGuid)

            ' Validate language input.
            ' If something goes wrong use german as default language.
            If String.IsNullOrEmpty(language) Then
                ' If parameter is not provided, use browser language setting.
                language = Request.UserLanguages(0).Trim().Substring(0, 2)
            End If

            language = language.ToUpper()

            If Not (language.Equals("DE") OrElse language.Equals("FR") OrElse language.Equals("EN") OrElse language.Equals("IT")) Then
                ' If language param has an unsopported language, choose german as default language.
                language = "DE"
            End If

            ' Generate the necessasry serialized string array.
            Dim paramStream(3) As String
            paramStream(0) = mandantGuid
            paramStream(1) = appGuid
            paramStream(2) = language
            paramStream(3) = role
            Dim stream As String = Utility.Utility.SerializeParameterObject(paramStream, True)
            stream = HttpUtility.UrlEncode(stream)

            ' Redirect to App Home Controller with the needed parameters.
            Return Redirect(Utility.Utility.GetBaseUrl() & Constants.APP_RELATIVE_URL & stream)

        End Function

#End Region

    End Class
End Namespace
