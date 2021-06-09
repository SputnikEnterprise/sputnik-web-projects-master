
Imports System.Web.Services
Imports System.ComponentModel

Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DocumentScan
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports System.Net.Mail
Imports System.Net
Imports wsSPS_Services.WOSInfo
Imports wsSPS_Services.Logging


' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPNotification.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPNotification
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPNotification"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess
	Private m_Scan As ScanDatabaseAccess
	Private m_WOS As WOSDatabaseAccess
	Private m_PublicData As PublicDataDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities

		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_Scan = New ScanDatabaseAccess(My.Settings.ConnStr_Scanning, Language.German)
		m_WOS = New WOSDatabaseAccess(My.Settings.ConnStr_New_spContract, Language.German)
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


	<WebMethod(Description:="get data for provider")>
	Function GetProviderData(ByVal customerID As String, ByVal providerName As String) As ProviderDataDTO
		Dim result As ProviderDataDTO = Nothing
		m_customerID = customerID

		Try
			result = New ProviderDataDTO
			result = m_SysInfo.LoadPrividerData(customerID, providerName)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetProviderData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="get data for job plattforms")>
	Function GetJobplattformCounterData(ByVal customerID As String, ByVal wos_id As String, ByVal jobsCHAccountNumber As Integer, ByVal ostjobAccountNumber As String) As JobplattformCounterDataDTO
		Dim result As JobplattformCounterDataDTO = Nothing
		m_customerID = customerID

		Try
			result = New JobplattformCounterDataDTO
			result = m_WOS.LoadJobplattformCounterData(customerID, wos_id, jobsCHAccountNumber, ostjobAccountNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetJobplattformCounterData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="adds user informations with remote data.")>
	Function AddUserData(ByVal customerGuid As String, userData As SystemUserData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerGuid

		Try
			result = m_SysInfo.AddSputnikUserData(m_customerID, userData)

			If Not String.IsNullOrWhiteSpace(userData.EmployeeWOSID) Then
				result = m_WOS.UpdateWOSCustomerUserData(m_customerID, userData)
			End If

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddUserData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List Notification-Data for todo in client")>
	Function LoadAssignedNotificationForTODO(ByVal customerID As String) As NotifyMessageData()

		Dim result As List(Of NotifyMessageData) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of NotifyMessageData)
			result = m_SysInfo.LoadTODONotificationsData(customerID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedNotificationForTODO", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List Notification-Data in client")>
	Function LoadAssignedNotificationArt(ByVal customerID As String, ByVal notifyArt As NotifyArtEnum, ByVal excludeChecked As Boolean?) As NotifyMessageData()

		Dim result As List(Of NotifyMessageData) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of NotifyMessageData)
			result = m_SysInfo.LoadNotificationsData(customerID, notifyArt, excludeChecked)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedNotificationArt", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="saves data into loging table")>
	Function AddNLogEntries(ByVal logEntry As String, ByVal nlogData As String, ByVal notificationMessage As String) As Boolean

		Dim result As Boolean = True
		m_customerID = String.Empty

		Try
			result = True
			Dim ndata As New NLOGData With {.callSite = logEntry, .Customer_ID = m_customerID, .innerException = nlogData, .level = "level", .stackTrace = notificationMessage, .type = "type", .message = "message", .additionalInfo = "additionalInfo"}
			result = m_SysInfo.AddUserNLOGNotificationData(m_customerID, ndata, notificationMessage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddNLogEntries", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return result
	End Function

	<WebMethod(Description:="List customer notification-Data in client")>
	Function GetCustomerNotifications(ByVal customerID As String, ByVal userID As String, ByVal excludeChecked As Boolean?) As CustomerNotificationDataDTO()

		Dim result As List(Of CustomerNotificationDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CustomerNotificationDataDTO)
			result = m_SysInfo.LoadCustomerNotificationsData(userID, excludeChecked)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerNotifications", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


	<WebMethod(Description:="List customer data for application listing in client")>
	Function GetCustomerDataForApplicationJoblist() As MandantData()

		Dim result As List(Of MandantData) = Nothing

		Try
			result = New List(Of MandantData)
			result = m_SysInfo.LoadCustomerDataForApplicationList()

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerDataForApplicationJoblist", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List customer data for scanjob listing in client")>
	Function GetCustomerDataForScanJoblist() As MandantData()

		Dim result As List(Of MandantData) = Nothing

		Try
			result = New List(Of MandantData)
			result = m_Scan.LoadCustomerDataForScanjobList()

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerDataForScanJoblist", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List Notification-Data for scan jobs")>
	Function GetScanJobsNotifications(ByVal customerID As String, ByVal excludeChecked As Boolean?) As ScanDTO()
		Dim result As List(Of ScanDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of ScanDTO)
			result = m_Scan.LoadScanJobsNotificationsData(customerID, excludeChecked)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetScanJobsNotifications", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


	<WebMethod(Description:="update notification record as checked")> _
	Function UpdateAssignedNotification(ByVal customerID As String, ByVal recordID As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_SysInfo.UpdateAssignedNotificationData(customerID, recordID, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedNotification", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="add customer notification record")>
	Function AddCustomerNotification(ByVal customerID As String, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_SysInfo.AddCustomerNotificationData(customerID, notifyHeader, notifyComment, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddCustomerNotification", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update customer notification record as checked")>
	Function UpdateAssignedCustomerNotificationContent(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_SysInfo.UpdateAssignedCustomerNotificatioContentData(customerID, recordID, notifyHeader, notifyComment, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedCustomerNotificationContent", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update customer notification record as checked")>
	Function UpdateAssignedCustomerNotification(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_SysInfo.UpdateAssignedCustomerNotificationData(customerID, recordID, notifyHeader, notifyComment, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedCustomerNotification", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update customer payable service.")>
	Function UpdateAssignedCustomerPayableService(ByVal customerID As String, ByVal userData As SystemUserData, ByVal serviceName As String, ByVal jobID As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_SysInfo.AddCustomerPayableServiceUsage(customerID, userData, serviceName, jobID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedCustomerPayableService", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


	<WebMethod(Description:="sends notification mail to user")>
	Function SendReportNotificationsWithEMail(ByVal customerID As String, ByVal notFoundedReportQuantity As Integer) As Boolean
		Dim result As Boolean = True
		Dim eMailData = m_Scan.LoadNotificationEMailData(customerID)
		If eMailData Is Nothing Then Return False
		Dim bodyMessage = System.IO.File.ReadAllText(System.IO.Path.Combine(eMailData.TemplateFolder, "NotFoundedReport.txt"), System.Text.Encoding.Default)
		m_customerID = customerID

		Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
		Dim mailmsg As New System.Net.Mail.MailMessage
		Dim strToAdresses As String() = eMailData.Report_Recipients.Split(CChar(";"))

		With mailmsg
			.IsBodyHtml = True

			.To.Clear()
			.ReplyToList.Clear()

			.From = New MailAddress(eMailData.MailSender)
			.To.Add(New MailAddress(strToAdresses(0).Trim))
			If strToAdresses.Length > 1 Then
				For i As Integer = 1 To strToAdresses.Length - 1
					.CC.Add(New MailAddress(strToAdresses(i).Trim))
				Next
			End If
			.ReplyToList.Add(.From)

			.Subject = "Benachrichtigung"
			.Body = String.Format(bodyMessage, notFoundedReportQuantity, customerID, eMailData.Customer_Name)
			.Priority = Net.Mail.MailPriority.High
		End With

		Dim strEx_UserName As String = eMailData.MailUserName
		Dim strEx_UserPW As String = eMailData.MailPassword
		Dim iSmtpPort As Integer = eMailData.SmtpPort

		Try
			If Not String.IsNullOrWhiteSpace(strEx_UserName) Then
				Dim mailClient As New System.Net.Mail.SmtpClient(eMailData.SmtpServer, eMailData.SmtpPort)
				mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
				mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
				mailClient.EnableSsl = eMailData.ActivateSSL

				mailClient.Send(mailmsg)

			Else
				obj.Host = eMailData.SmtpServer
				obj.Send(mailmsg)

			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("SendReportNotificationsWithEMail:{0}MailUserName: {1}{0}MailPassword: {2}{0}SmtpPort: {3}{0}ActivateSSL: {4}{0}customerID: {5}{0}{6}",
										   vbNewLine, strEx_UserName, strEx_UserPW, iSmtpPort, eMailData.ActivateSSL, customerID, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "SendReportNotificationsWithEMail", .MessageContent = msgContent})

		Finally
			obj = Nothing
			mailmsg.Attachments.Dispose()
			mailmsg.Dispose()

		End Try

		' Return search data as an array.
		Return result
	End Function


#Region "WOS Notification"

	<WebMethod(Description:="list mail notification for WOS")>
	Function GetWOSMailNotifications(ByVal customerWosGuid As String, ByVal year As Integer?, ByVal month As Integer?) As WOSNotificationDTO()

		Dim result As List(Of WOSNotificationDTO) = Nothing
		m_customerID = customerWosGuid

		Try
			result = New List(Of WOSNotificationDTO)
			result = m_WOS.LoadWOSEMailNotificationsData(customerWosGuid, year, month)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetWOSMailNotifications", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="list mail notification for WOS modul")>
	Function GetWOSModulMailNotifications(ByVal customerWosGuid As String, ByVal modulNumber As Integer?, ByVal number As Integer?) As WOSNotificationDTO()

		Dim result As List(Of WOSNotificationDTO) = Nothing
		m_customerID = customerWosGuid

		Try
			result = New List(Of WOSNotificationDTO)
			result = m_WOS.LoadWOSModulEMailNotificationsData(customerWosGuid, modulNumber, number)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetWOSModulMailNotifications", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function



#End Region


#Region "qualification data"

	<WebMethod(Description:="load geo data for postfach data")>
	Function LoadQualificationData(ByVal customerID As String, ByVal gender As String, ByVal language As String, ByVal qualificationModul As String) As QualificationDTO()
		Dim result As List(Of QualificationDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of QualificationDTO)
			result = m_PublicData.LoadQualificationData(m_customerID, gender, language, qualificationModul)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadQualificationData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


#End Region

#Region "geo data"

	<WebMethod(Description:="load geo data for postfach data")>
	Function LoadGeoCoordinationPostcodeData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String) As LocationGoordinateDataDTO
		Dim result As LocationGoordinateDataDTO = Nothing
		m_customerID = customerID

		Try
			result = New LocationGoordinateDataDTO
			result = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, 0)(0)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationPostcodeData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)
	End Function

	<WebMethod(Description:="load distance between two postfach data")>
	Function LoadGeoCoordinationTwoPostcodeData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String, ByVal v_plz2 As String) As LocationGoordinateDataDTO()
		Dim result As List(Of LocationGoordinateDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of LocationGoordinateDataDTO)
			result = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, v_plz2)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationTwoPostcodeData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load distance between two postfach data")>
	Function LoadGeoDistancesData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String, ByVal v_plz2 As String, ByVal unit As String) As Double
		Dim result As Double = 0
		m_customerID = customerID

		Try
			Dim geoData = New List(Of LocationGoordinateDataDTO)
			geoData = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, v_plz2)

			If String.IsNullOrWhiteSpace(unit) Then unit = "K"
			result = CalcDistance(geoData(0).Latitude, geoData(0).Longitude, geoData(1).Latitude, geoData(1).Longitude, "K")

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoDistancesData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return result
	End Function



	''' <summary>
	''' Calculate distances between to points.
	''' Standards: 
	''' unit = "M"	miles
	''' unit = "K"	Kilometers
	''' unit = "N"	Nautical miles
	''' </summary>
	''' <returns></returns>
	Private Function CalcDistance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double, ByVal unit As Char) As Double
		Dim theta As Double = lon1 - lon2
		Dim dist As Double = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta))

		dist = Math.Acos(dist)
		dist = rad2deg(dist)
		dist = dist * 60 * 1.1515

		If unit = "K" Then
			dist = dist * 1.609344
		ElseIf unit = "N" Then
			dist = dist * 0.8684
		End If


		Return dist
	End Function

	Private Function deg2rad(ByVal deg As Double) As Double
		Return (deg * Math.PI / 180.0)
	End Function

	Private Function rad2deg(ByVal rad As Double) As Double
		Return rad / Math.PI * 180.0
	End Function


#End Region


End Class