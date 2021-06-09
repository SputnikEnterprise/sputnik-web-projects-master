
Namespace DataTransferObject.DocumentScan.DataObjects


	<Serializable()>
	Public Class ScanAttachmentDTO
		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property CustomerName As String
		Public Property BusinessBranchNumber As Integer?
		Public Property ModulNumber As AttachmentModulEnum
		Public Property DocumentCategoryNumber As Integer?

		Public Property ScanContent As Byte()
		Public Property ImportedFileGuid As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public Enum AttachmentModulEnum
			Employee
			Customer
			Employment
			Report
			Invoice
			Payroll
			NotDefined
		End Enum


	End Class


	<Serializable()>
	Public Class ScanDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property FoundedCodeValue As String
		Public Property ModulNumber As ScannModulEnum
		Public Property RecordNumber As Integer
		Public Property DocumentCategoryNumber As Integer?
		Public Property IsValid As Boolean?

		Public Property ReportYear As Integer?
		Public Property ReportMonth As Integer?
		Public Property ReportWeek As Integer?
		Public Property ReportFirstDay As Integer?
		Public Property ReportLastDay As Integer?
		Public Property ReportLineID As Integer?

		Public Property ScanContent As Byte()
		Public Property ImportedFileGuid As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public Enum ScannModulEnum
			Employee
			Customer
			Employment
			Report
			Invoice
			Payroll
			NotDefined
		End Enum

	End Class


	<Serializable()>
	Public Class ScanDropInDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property BusinessBranch As String
		Public Property ModulNumber As Integer?
		Public Property DocumentCategoryNumber As Integer?
		Public Property ScanContent As Byte()
		Public Property FileExtension As String
		Public Property ScanFileName As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public Enum ScannModulEnum
			Employee
			Customer
			Employment
			Report
			Invoice
			Payroll
			NotDefined
		End Enum

	End Class

	Public Class EMailNotificationData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property Customer_Name As String
		Public Property Recipients As String
		Public Property Report_Recipients As String
		Public Property BCCAddresses As String
		Public Property MailSender As String
		Public Property MailUserName As String
		Public Property MailPassword As String
		Public Property SmtpServer As String
		Public Property SmtpPort As Integer
		Public Property ActivateSSL As Boolean
		Public Property TemplateFolder As String


	End Class


End Namespace
