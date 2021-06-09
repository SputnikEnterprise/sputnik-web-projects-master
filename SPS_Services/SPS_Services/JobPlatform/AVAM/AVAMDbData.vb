
Namespace JobPlatform.AVAM


	<Serializable()>
	Public Class AVAMJobCreationData
		Public Property AVAMRecordState As String
		Public Property JobroomID As String
		Public Property QueryContent As String
		Public Property ResultContent As String
		Public Property ReportingObligation As Boolean?
		Public Property reportingObligationEndDate As DateTime?
		Public Property State As Boolean?
		Public Property Content As JobContentData
		Public Property ErrorMessage As ErrorData
		Public Property SyncDate As DateTime?
		Public Property SyncFrom As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

	Public Class ErrorData
		Public Property Content As String
		Public Property Title As String
		Public Property Status As String
		Public Property Message As String
		Public Property Detail As String

	End Class

	<Serializable()>
	Public Class JobroomSearchResultData
		Public Property Content As List(Of JobroomData)
		Public Property TotalElements As Integer?
		Public Property TotalPages As Integer?
		Public Property CurrentPage As Integer?
		Public Property CurrentSize As Integer?
		Public Property First As Boolean?
		Public Property Last As Boolean?

	End Class


	Public Class JobroomData
		Public Property ID As String
		Public Property Status As String
		Public Property SourceSystem As String
		Public Property ExternalReference As String
		Public Property StellennummerEgov As String
		Public Property StellennummerAvam As String
		Public Property Fingerprint As String
		Public Property ReportingObligation As Boolean?
		Public Property ReportingObligationEndDate As Date?
		Public Property ReportToAvam As Boolean?
		Public Property JobCenterCode As String
		Public Property ApprovalDate As Date?
		Public Property RejectionDate As Date?
		Public Property RejectionCode As String
		Public Property RejectionReason As String
		Public Property CancellationDate As Date?
		Public Property CancellationCode As String
		Public Property JobContent As JobContentData
		Public Property Publication As PublicationData

	End Class


	Public Class JobContentData
		Public Property ExternalUrl As String
		Public Property JobDescriptions As List(Of JobDescriptionData)
		Public Property Company As CompanyData
		Public Property Employment As EmploymentData
		Public Property Location As LocationData
		Public Property Occupations As List(Of OccupationsData)
		Public Property LanguageSkills As List(Of LanguageSkillsData)
		Public Property ApplyChannel As ApplyChannelData
		Public Property PublicContact As PublicContactData

	End Class

	Public Class JobDescriptionData
		Public Property LanguageIsoCode As String
		Public Property Title As String
		Public Property Description As String

	End Class

	Public Class CompanyData
		Public Property Name As String
		Public Property Street As String
		Public Property HouseNumber As String
		Public Property PostalCode As String
		Public Property City As String
		Public Property CountryIsoCode As String
		Public Property PostOfficeBoxNumber As String
		Public Property PostOfficeBoxPostalCode As String
		Public Property PostOfficeBoxCity As String
		Public Property Phone As String
		Public Property Email As String
		Public Property Website As String
		Public Property Surrogate As Boolean?

	End Class

	Public Class EmploymentData
		Public Property StartDate As Date?
		Public Property EndDate As Date?
		Public Property ShortEmployment As Boolean?
		Public Property Immediately As Boolean?
		Public Property Permanent As Boolean?
		Public Property WorkloadPercentageMin As String
		Public Property WorkloadPercentageMax As String
		Public Property WorkForms As List(Of WorkFormsData)

	End Class

	Public Class LocationData
		Public Property Remarks As String
		Public Property City As String
		Public Property PostalCode As String
		Public Property CommunalCode As String
		Public Property RegionCode As String
		Public Property CantonCode As String
		Public Property CountryIsoCode As String
		Public Property Coordinates As String

	End Class

	Public Class WorkFormsData
		Public Property Value As String

	End Class

	Public Class OccupationsData
		Public Property AvamOccupationCode As Integer?
		Public Property WorkExperience As String
		Public Property EducationCode As String

	End Class

	Public Class LanguageSkillsData
		Public Property LanguageIsoCode As String
		Public Property SpokenLevel As String
		Public Property WrittenLevel As String

	End Class

	Public Class ApplyChannelData
		Public Property MailAddress As String
		Public Property EmailAddress As String
		Public Property PhoneNumber As String
		Public Property FormUrl As String
		Public Property AdditionalInfo As String

	End Class

	Public Class PublicContactData
		Public Property Salutation As String
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Phone As String
		Public Property Email As String

	End Class

	Public Class PublicationData
		Public Property StartDate As Date?
		Public Property EndDate As Date?
		Public Property EuresAnonymous As Boolean?
		Public Property PublicDisplay As Boolean?
		Public Property PublicAnonymous As Boolean?
		Public Property RestrictedDisplay As Boolean?
		Public Property RestrictedAnonymous As Boolean?

	End Class

	Public Enum AVAMAdvertismentCancelReasonENUM

		OCCUPIED_JOBCENTER
		OCCUPIED_AGENCY
		OCCUPIED_JOBROOM
		OCCUPIED_OTHER
		NOT_OCCUPIED
		CHANGE_OR_REPOSE

	End Enum


	<Serializable()>
	Public Class ProfilMatcherNotificationData
		Public Property ID As Integer
		Public Property CustomerID As String
		Public Property UserID As String
		Public Property QueryName As String
		Public Property QueryContent As String
		Public Property QueryResultContent As String
		Public Property Total As Integer
		Public Property CustomerNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property Notify As Boolean?
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?

	End Class


	<Serializable()>
	Public Class ProfilMatcherLocationData
		Public Property Location As String
		Public Property LocationDistances As Integer

	End Class


	<Serializable()>
	Public Class ProfilmatcherQueryResultData

		Public Property CustomerID As String
		Public Property Status As String
		Public Property Total As Integer
		Public Property Page As Integer
		Public Property Size As Integer
		Public Property ResultContent As String
		Public Property Jobs As List(Of ProfilmatcherQueryJob)

	End Class

	<Serializable()>
	Public Class ProfilmatcherQueryJob
		Public Property ID As Long
		Public Property Title As String
		Public Property Location As String
		Public Property Company As String
		Public Property URL As String
		Public Property DateCreated As DateTime

	End Class


	Public Class ProfilmatcherWebserviceResult

		Public Property HttpState As String
		Public Property APIResponse As String
		Public Property APIResult As String

	End Class


End Namespace