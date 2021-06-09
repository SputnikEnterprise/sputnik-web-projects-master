'---------------------------------------------------------
' Utility.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.IO
Imports System.Net.Mail
Imports System.Net

''' <summary>
''' Utility class.
''' </summary>
Public Class Utility

	''' <summary>
	''' Sends a file to the customers browser.
	''' </summary>
	Public Shared Sub SendFileToCustomer(ByVal response As HttpResponse, ByVal applicationType As String, ByVal data As Byte(), ByVal sendAsAttachement As Boolean, ByVal fileNameWithExtension As String)

		Try
			response.ContentType = applicationType
			response.Clear()
			response.ClearContent()
			response.ClearHeaders()
			response.AddHeader("content-disposition", "filename=" + fileNameWithExtension)

			response.BinaryWrite(data)
			response.Flush()
			response.End()
		Catch ex As Exception
			'response.Write(ex.ToString())
		End Try
	End Sub

	' Get an email template 
	Public Shared Function GetEmailTemplate(ByVal templateName As String) As String

		Dim emailTemplateFileName As String = HttpContext.Current.Server.MapPath("~/MailTemplates/" + templateName)

		'Get a StreamReader class that can be used to read the file
		Dim objStreamReader As StreamReader
		objStreamReader = File.OpenText(emailTemplateFileName)

		Dim emailText As String = objStreamReader.ReadToEnd()

		objStreamReader.Close()

		Return emailText

	End Function


	Public Shared Function CreateSMTPClient() As SmtpClient
		Dim result As SmtpClient
		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		result = New System.Net.Mail.SmtpClient(appInfo.SmtpAddress, appInfo.SmtpServerPort)
		result.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
		result.EnableSsl = appInfo.SmtpTLS.GetValueOrDefault(False)

		If Not String.IsNullOrWhiteSpace(appInfo.SmtpUser) Then
			result.Credentials = New NetworkCredential(appInfo.SmtpUser, appInfo.SmtpUserPassword)
		End If

		Return result

	End Function

	' Sends a mail without an attachement
	Public Shared Sub SendMail(ByVal fromEmailAddress As String, ByVal toEmailAddress As String, ByVal ccEmailAddress As String, ByVal subject As String, ByVal body As String, ByVal isBodyHTML As Boolean)

		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		' Replace with test email receiver if test mode is on
		If appInfo.EmailTestModeEnabled Then
			toEmailAddress = appInfo.EmailTestModeReceiver

			If Not String.IsNullOrEmpty(ccEmailAddress) Then
				ccEmailAddress = appInfo.EmailTestModeReceiver
			End If
		End If

		' Create email object
		Dim mailMessage As MailMessage = New MailMessage(fromEmailAddress, toEmailAddress, subject, body)
		mailMessage.IsBodyHtml = isBodyHTML

		' Add a CC-Receiver if specified
		If Not String.IsNullOrEmpty(ccEmailAddress) Then
			mailMessage.CC.Add(ccEmailAddress)
		End If

		' Send the mail with attachement
		Dim smtpClient As SmtpClient = CreateSMTPClient()	' New SmtpClient(appInfo.SmtpAddress)
		smtpClient.Send(mailMessage)

	End Sub

	' Sends a mail without an attachement
	Public Shared Sub SendMail(ByVal fromEmailAddress As String, ByVal toEmailAddress As String, ByVal subject As String, ByVal body As String, ByVal isBodyHTML As Boolean)
		SendMail(fromEmailAddress, toEmailAddress, Nothing, subject, body, isBodyHTML)
	End Sub

	' Sends an email with an attachement
	Public Shared Sub SendMailWithAttachment(ByVal fromEmailAddress As String, ByVal toEmailAddress As String, _
																					 ByVal ccEmailAddress As String, ByVal subject As String, ByVal body As String, _
																					 ByVal isBodyHTML As Boolean, ByVal attachementName As String, ByVal data As Byte(), _
																					 ByVal attachementType As String)

		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		' Replace with test email receiver if test mode is on
		If appInfo.EmailTestModeEnabled Then
			toEmailAddress = appInfo.EmailTestModeReceiver

			If Not String.IsNullOrEmpty(ccEmailAddress) Then
				ccEmailAddress = appInfo.EmailTestModeReceiver
			End If
		End If

		' Create email object
#If DEBUG Then
		toEmailAddress = "m.schneider@sputnik-it.com"
#End If
		Dim mailMessage As MailMessage = New MailMessage(fromEmailAddress, toEmailAddress, subject, body)
		mailMessage.IsBodyHtml = isBodyHTML

		' Add a CC-Receiver if specified
		If Not String.IsNullOrEmpty(ccEmailAddress) Then
			mailMessage.CC.Add(ccEmailAddress)
		End If

		' Write the attachement data to a memory stream
		Dim ms As MemoryStream = New MemoryStream()
		ms.Write(data, 0, data.Length)
		ms.Flush()
		ms.Position = 0

		' Create the final attachement object
		Dim emailAttachement As Attachment = New Attachment(ms, attachementName, attachementType)
		mailMessage.Attachments.Add(emailAttachement)

		' Send the mail with attachement
		Dim smtpClient As SmtpClient = CreateSMTPClient() ' New SmtpClient(appInfo.SmtpAddress)
		smtpClient.Send(mailMessage)

	End Sub

	' Sends an email with an attachement
	Public Shared Sub SendMailWithAttachment(ByVal fromEmailAddress As String, ByVal toEmailAddress As String, _
																					 ByVal subject As String, ByVal body As String, ByVal isBodyHTML As Boolean, _
																					 ByVal attachementName As String, ByVal data As Byte(), ByVal attachementType As String)
		SendMailWithAttachment(fromEmailAddress, toEmailAddress, Nothing, subject, body, isBodyHTML, attachementName, data, attachementType)
	End Sub

	' Helps extracting a column value form a data reader.
	Public Shared Function GetColumnTextStr(ByVal dr As SqlDataReader, ByVal columnName As String, ByVal replacementOnNull As String) As String

		If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
			If String.IsNullOrEmpty(dr(columnName)) Then
				Return replacementOnNull
			End If
			Return dr(columnName)
		End If

		Return replacementOnNull
	End Function

	' Helps extracting a column value form a data reader.
	Public Shared Function GetColumnInteger(ByVal dr As SqlDataReader, ByVal columnName As String, ByVal replacementOnNull As Integer) As Integer

		If Not dr.IsDBNull(dr.GetOrdinal(columnName)) Then
			Return dr(columnName)
		End If

		Return replacementOnNull
	End Function

	''' <summary>
	''' Returns a boolean or the default value if its nothing.
	''' </summary>
	''' <param name="reader">The reader.</param>
	''' <param name="columnName">The column name.</param>
	''' <param name="defaultValue">The default value.</param>
	''' <returns>Value or default value if the value is nothing</returns>
	Public Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

		Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		If (Not reader.IsDBNull(columnIndex)) Then
			Return reader.GetBoolean(columnIndex)
		Else
			Return defaultValue
		End If
	End Function



	' Helps extracting a column value form a data reader.
	Public Shared Function GetColumnTextStr(ByVal dataRow As DataRow, ByVal columnName As String, ByVal replacementOnNull As String) As String

		If Not dataRow.IsNull(columnName) Then
			Return dataRow(columnName)
		End If

		Return replacementOnNull
	End Function


	Public Shared ERROR_TYPE_PARAMETER As String = "et"
	Public Shared ERROR_TYPE_DOCUMENT As String = "doc"

	Public Shared ERROR_MESSAGE_PARAMETER As String = "m"
End Class
