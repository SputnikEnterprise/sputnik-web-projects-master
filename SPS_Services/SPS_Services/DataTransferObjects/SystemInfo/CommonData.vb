

Public Class QualificationDTO

	Public Property Code As Integer?
	Public Property TranslatedValue As String
	Public Property MeldePflichtig As Boolean

End Class

Public Class ProgramSettings
	Public Property FileServerPath As String
	Public Property CVScanFolder As String
	Public Property ReportScanFolder As String
	Public Property ScanParserStartProgram As String
	Public Property EMailParserStartProgram As String

	Public Property Notificationintervalperiode As Decimal
	Public Property Notificationintervalperiodeforreport As Decimal
	Public Property CVLFolderTOWatch As String
	Public Property CVLFolderTOArchive As String
	Public Property CVLXMLFolder As String
	Public Property TemporaryFolder As String

	Public Property SmtpServer As String
	Public Property SmtpPort As String
	Public Property SmtpUser As String
	Public Property SmtpPassword As String
	Public Property SmtpUseTLS As Boolean
	Public Property StagingEMailFrom As String
	Public Property StagingEMailTo As String
	Public Property NotifyEMailFrom As String

	Public Property ReportMailbox As String
	Public Property ReportEmailUser As String
	Public Property ReportEmailPassword As String

	Public Property CVMailbox As String
	Public Property CVEmailUser As String
	Public Property CVEmailPassword As String

	Public Property FTPServer As String
	Public Property FTPFolder As String
	Public Property FTPUser As String
	Public Property FTPPassword As String


	Public Property ConnstringApplication As String
	Public Property ConnstringCVLizer As String
	Public Property ConnstringSysteminfo As String
	Public Property ConnstringScanjobs As String
	Public Property ConnstringEMail As String
	Public Property ConnstringWOS As String
	Public Property CVLParseAsDemo As Boolean
	Public Property ParseEMailAttachment As Boolean
	Public Property AskSendToCVLizer As Boolean


End Class

Public Class ProgramSettings_OLD
	Public Property FileServerPath As String
	Public Property CVScanFolder As String
	Public Property ReportScanFolder As String
	Public Property ScanParserStartProgram As String
	Public Property EMailParserStartProgram As String

	Public Property Notificationintervalperiode As Decimal
	Public Property Notificationintervalperiodeforreport As Decimal
	Public Property CVLFolderTOWatch As String
	Public Property CVLFolderTOArchive As String
	Public Property CVLXMLFolder As String
	Public Property TemporaryFolder As String

	Public Property SmtpServer As String
	Public Property ReportMailbox As String
	Public Property ReportEmailUser As String
	Public Property ReportEmailPassword As String

	Public Property CVMailbox As String
	Public Property CVEmailUser As String
	Public Property CVEmailPassword As String

	Public Property FTPServer As String
	Public Property FTPFolder As String
	Public Property FTPUser As String
	Public Property FTPPassword As String


	Public Property ConnstringApplication As String
	Public Property ConnstringCVLizer As String
	Public Property ConnstringSysteminfo As String
	Public Property ConnstringScanjobs As String
	Public Property ConnstringEMail As String
	Public Property CVLParseAsDemo As Boolean
	Public Property ParseEMailAttachment As Boolean
	Public Property AskSendToCVLizer As Boolean


End Class
