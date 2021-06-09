


Namespace DataTransferObject.Notification.DataObjects


	<Serializable()>
	Public Class ApplicantDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property EmployeeID As Integer
		Public Property Lastname As String
		Public Property Firstname As String
		Public Property Gender As String
		Public Property Street As String
		Public Property PostOfficeBox As String
		Public Property Postcode As String
		Public Property Latitude As Double?
		Public Property Longitude As Double?
		Public Property Location As String
		Public Property Canton As String
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
		Public Property CivilStateLabel As String
		Public Property Language As String
		Public Property LanguageLevel As Integer
		Public Property ApplicantLifecycle As Integer
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String
		Public Property CVLProfileID As Integer?


		Public Property APID As Integer
		Public Property ApplicationLabel As String
		Public Property VacancyNumber As Integer
		Public Property Advisor As String
		Public Property BusinessBranch As String
		Public Property Dismissalperiod As String
		Public Property Availability As String
		Public Property Comment As String
		Public Property APCreatedOn As DateTime?
		Public Property ApplicationLifecycle As Integer


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

		''' <summary>
		''' deprecated!!!
		''' </summary>
		''' <returns></returns>
		Public ReadOnly Property Genderlable As String
			Get
				If Gender = "m" Then Return "männlich" Else Return "weiblich"
			End Get
		End Property

		Public ReadOnly Property Genderlabel As String
			Get
				If Gender = "m" Then Return "männlich" Else Return "weiblich"
			End Get
		End Property


	End Class


	<Serializable()>
	Public Class ApplicationDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property FK_ApplicantID As Integer
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

	<Serializable()>
	Public Class ApplicantDocumentDataDTO

		Public Property ID As Integer
		Public Property FK_ApplicantID As Integer?
		Public Property Type As Integer?
		Public Property Flag As Integer?
		Public Property Title As String
		Public Property FileExtension As String
		Public Property Content As Byte()
		Public Property Hashvalue As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

	End Class


End Namespace





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
	Public Class TKDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property KontoName As String
		Public Property UserName As String
		Public Property UserData As String

	End Class






	'<Serializable()>
	'Public Class ScanDTO

	'	Public Property ID As Integer
	'	Public Property Customer_ID As String
	'	Public Property FoundedCodeValue As String
	'	Public Property ModulNumber As ScannModulEnum
	'	Public Property RecordNumber As Integer
	'	Public Property DocumentCategoryNumber As Integer?
	'	Public Property IsValid As Boolean?

	'	Public Property ReportYear As Integer?
	'	Public Property ReportMonth As Integer?
	'	Public Property ReportWeek As Integer?
	'	Public Property ReportFirstDay As Integer?
	'	Public Property ReportLastDay As Integer?

	'	Public Property ScanContent As Byte()
	'	Public Property ImportedFileGuid As String
	'	Public Property CreatedOn As DateTime?
	'	Public Property CreatedFrom As String
	'	Public Property CheckedOn As DateTime?
	'	Public Property CheckedFrom As String

	'	Public Enum ScannModulEnum
	'		Employee
	'		Customer
	'		Employment
	'		Report
	'		Invoice
	'		Payroll
	'		NotDefined
	'	End Enum

	'End Class


	'<Serializable()>
	'Public Class ScanDropInDTO

	'	Public Property ID As Integer
	'	Public Property Customer_ID As String
	'	Public Property BusinessBranch As String
	'	Public Property ModulNumber As Integer?
	'	Public Property DocumentCategoryNumber As Integer?
	'	Public Property ScanContent As Byte()
	'	Public Property FileExtension As String
	'	Public Property CreatedOn As DateTime?
	'	Public Property CreatedFrom As String
	'	Public Property CheckedOn As DateTime?
	'	Public Property CheckedFrom As String

	'	Public Enum ScannModulEnum
	'		Employee
	'		Customer
	'		Employment
	'		Report
	'		Invoice
	'		Payroll
	'		NotDefined
	'	End Enum

	'End Class







#Region "Applicationdata"


#End Region


#Region "Applicantdata"


#End Region


#Region "Applicant Documentdata"


#End Region



End Namespace