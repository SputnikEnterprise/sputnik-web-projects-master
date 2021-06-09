
Namespace DataTransferObject.Notify


	<Serializable()>
	Public Class UserLoginDTO

		Public Property UserNumber As Integer
		Public Property UserFirstName As String
		Public Property UserLastName As String
		Public Property UserGuid As String
		Public Property UserMachineName As String
		Public Property UserDomainName As String
		Public Property UserLoginName As String


		Public Property CustomerID As String
		Public Property CustomerName As String
		Public Property CustomerTelefon As String
		Public Property CustomerEMail As String
		Public Property CustomerLocation As String


	End Class




	<Serializable()>
 Public Class ProviderDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ProviderName As String
		Public Property AccountName As String
		Public Property UserName As String
		Public Property UserData As String

	End Class


	<Serializable()>
 Public Class TKDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property KontoName As String
		Public Property UserName As String
		Public Property UserData As String

	End Class


	<Serializable()>
 Public Class NotifyDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property NotifyHeader As String
		Public Property NotifyComments As String
		Public Property NotifyArt As NotifyEnum
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public Enum NotifyEnum
			NotDefined
			AsError
			AsImportant
			AsInfo
			AsScanJobImportInfo
			AsScanJobErrorInfo
		End Enum

	End Class


	<Serializable()>
 Public Class WOSNotificationDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property MailFrom As String
		Public Property MailTo As String
		Public Property Result As String
		Public Property MailSubject As String
		Public Property MailBody As String
		Public Property DocLink As String
		Public Property RecipientGuid As String

		Public Property CreatedOn As DateTime?


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




#Region "Applicationdata"

	<Serializable()>
	Public Class ApplicationDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ApplicationNumber As Integer
		Public Property ApplicantNumber As Integer
		Public Property VacancyNumber As Integer?
		Public Property ApplicationLabel As String
		Public Property BusinessBranch As String
		Public Property Advisor As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property ApplicationLifecycle As Integer?

	End Class

#End Region


#Region "Applicantdata"

	<Serializable()>
	Public Class ApplicantDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ApplicantNumber As Integer
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Gender As Integer
		Public Property Street As String
		Public Property PostOfficeBox As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property Country As String
		Public Property Nationality As String
		Public Property EMail As String
		Public Property Telephone As String
		Public Property MobilePhone As String
		Public Property Birthdate As Date?
		Public Property Permission As String
		Public Property Profession As String
		Public Property Auto As Boolean
		Public Property Motorcycle As Boolean
		Public Property Bicycle As Boolean
		Public Property DrivingLicence1 As String
		Public Property DrivingLicence2 As String
		Public Property DrivingLicence3 As String
		Public Property CivilState As Integer
		Public Property Language As String
		Public Property LanguageLevel As Integer
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

		Public ReadOnly Property ApplicantFullname As String
			Get
				Return String.Format("{0} {1}", Firstname, Lastname)
			End Get
		End Property

		Public ReadOnly Property Address As String
			Get
				Return String.Format("{0}, {1} {2}", Street, Postcode, Location)
			End Get
		End Property

		Public ReadOnly Property Genderlable As String
			Get
				If Gender = 0 Then Return "männlich" Else Return "weiblich"
			End Get
		End Property


	End Class

#End Region


#Region "Applicant Documentdata"

	<Serializable()>
 Public Class ApplicantDocumentDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property ApplicationNumber As Integer
		Public Property ApplicantNumber As Integer
		Public Property Type As Integer?
		Public Property Flag As Integer?
		Public Property Title As String
		Public Property FileExtension As String
		Public Property Content As Byte()
		Public Property TrXMLID As Integer?
		Public Property TrXMLResult As String
		Public Property ProfilePicture As Byte()
		Public Property TrXMLCreatedOn As DateTime?
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

	End Class

#End Region



End Namespace