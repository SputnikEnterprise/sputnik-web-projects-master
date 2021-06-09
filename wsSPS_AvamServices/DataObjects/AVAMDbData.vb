
Namespace JobPlatform.AVAM


	<Serializable()>
	Public Class AVAMJobCreationData

		Public originalCallback As AsyncCallback
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








	'''' <summary>
	'''' Gets boolean flag indicating if the data is valid for xml export.
	'''' </summary>
	'Public ReadOnly Property IsDataValidForXml As Boolean
	'		Get

	'			m_ValidationErrors.Clear()

	'			Dim valid = True

	'			'valid = valid And Check(OrganisationsID.HasValue, "OrganisationID must have value")
	'			'valid = valid And Check(InseratID.HasValue, "InseratID must have value")

	'			'valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Beruf)), "Beruf must have a value")
	'			'valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Text)), "(Vorspann / Anforderungen / Aufgaben / Tätigkeit) must have a value")


	'			'' Check Anstellugnsgrad 
	'			'If Anstellungsgrad.HasValue Then
	'			'	valid = valid And Check(Anstellungsgrad >= 1, "Anstellungsgrad must be >= 1")
	'			'	valid = valid And Check(Anstellungsgrad <= 100, "Anstellungsgrad must be <= 100")
	'			'End If

	'			'' Check Anstellugnsgrad_Bis 
	'			'If Anstellungsgrad_Bis.HasValue Then
	'			'	valid = valid And Check(Anstellungsgrad_Bis >= 1, "Anstellungsgrad_Bis must be >= 1")
	'			'	valid = valid And Check(Anstellungsgrad_Bis <= 100, "Anstellungsgrad_Bis must be <= 100")
	'			'End If

	'			'' Check  Anstellugnsgrad_Bis >= Anstellungsgrad
	'			'If Anstellungsgrad.HasValue AndAlso Anstellungsgrad_Bis.HasValue Then
	'			'	valid = valid And Check(Anstellungsgrad_Bis >= Anstellungsgrad, "Anstellugnsgrad_Bis must be >= Anstellungsgrad")
	'			'End If

	'			'' Check Sprache
	'			'If Not String.IsNullOrEmpty(Sprache) Then

	'			'	Dim lang = Sprache.Trim().ToLower()

	'			'	Select Case lang
	'			'		Case "de"
	'			'		Case "it"
	'			'		Case "fr"
	'			'			' ok
	'			'		Case Else
	'			'			valid = valid And Check(False, "Sprache must be in [de, it, fr]")
	'			'	End Select

	'			'End If

	'			'' Check RubrikID and Position
	'			'If Not String.IsNullOrEmpty(TrimString(RubrikID)) Or
	'			'	Not String.IsNullOrEmpty(TrimString(Position)) Then

	'			'	Dim tRubridId As String = NothingToEmptyString(TrimString(RubrikID))
	'			'	Dim tPoitionId As String = NothingToEmptyString(TrimString(Position))

	'			'	Dim count1 = tRubridId.Count(Function(c) c = ":")
	'			'	Dim count2 = tPoitionId.Count(Function(c) c = ":")

	'			'	valid = valid And Check(count1 = count2, "RubrikID and Position do not have same number of values.")

	'			'End If

	'			''Check Alter_Von
	'			'If Alter_Von.HasValue Then

	'			'	Select Case Alter_Von
	'			'		Case 0
	'			'		Case 25
	'			'		Case 35
	'			'		Case 45
	'			'			' ok
	'			'		Case Else
	'			'			valid = valid And Check(False, "Alter_Von must be in [0, 25, 35, 45]")
	'			'	End Select

	'			'End If

	'			''Check Alter_Bis
	'			'If Alter_Bis.HasValue Then

	'			'	Select Case Alter_Bis
	'			'		Case 0
	'			'		Case 24
	'			'		Case 34
	'			'		Case 44
	'			'		Case 65
	'			'		Case 100
	'			'			' ok
	'			'		Case Else
	'			'			valid = valid And Check(False, "Alter_Bis must be in [0, 24, 34, 44, 65, 100]")
	'			'	End Select

	'			'End If

	'			''Check Alter_Bis >= Alter_Von
	'			'If Alter_Von.HasValue AndAlso Alter_Bis.HasValue Then
	'			'	valid = valid And Check(Alter_Bis >= Alter_Von, "Alter_Bis must be >= Alter_Von")
	'			'End If

	'			'' Check Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau
	'			'If Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Kandidat)) Or
	'			'	Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Niveau)) Then

	'			'	Dim tSprachKentnis As String = NothingToEmptyString(TrimString(Sprachkenntniss_Kandidat))
	'			'	Dim tSprachNiveau As String = NothingToEmptyString(TrimString(Sprachkenntniss_Niveau))

	'			'	Dim count1 = tSprachKentnis.Count(Function(c) c = ":")
	'			'	Dim count2 = tSprachNiveau.Count(Function(c) c = ":")

	'			'	valid = valid And Check(count1 = count2, "Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau do not have same number of values")
	'			'	valid = valid And Check(count1 <= 4, "To many Sprachkenntniss_Kandidat values (max 3 are allowed)")
	'			'	valid = valid And Check(count2 <= 4, "To many Sprachkenntniss_Niveau values (max 3 are allowed)")

	'			'End If

	'			'' Check Bildungsniveau
	'			'If Not String.IsNullOrEmpty(TrimString(Bildungsniveau)) Then

	'			'	Dim tBildungsNiveau As String = NothingToEmptyString(TrimString(Bildungsniveau))

	'			'	Dim count = tBildungsNiveau.Count(Function(c) c = ":")

	'			'	valid = valid And Check(count <= 4, "To many Bildungsniveau values (max 3 are allowed)")

	'			'End If

	'			'' Check Berufserfahrung and Berufserfahrung_Position
	'			'If Not String.IsNullOrEmpty(TrimString(Berufserfahrung)) Or
	'			'	Not String.IsNullOrEmpty(TrimString(Berufserfahrung_Position)) Then

	'			'	Dim tBerufserfahrung As String = NothingToEmptyString(TrimString(Berufserfahrung))
	'			'	Dim tBerufserfahrung_Position As String = NothingToEmptyString(TrimString(Berufserfahrung_Position))

	'			'	Dim count1 = tBerufserfahrung.Count(Function(c) c = ":")
	'			'	Dim count2 = tBerufserfahrung_Position.Count(Function(c) c = ":")

	'			'	valid = valid And Check(count1 = count2, "Berufserfahrung and Berufserfahrung_Position do not have same number of values")
	'			'	valid = valid And Check(count1 <= 3, "To many Berufserfahrung values (max 2 are allowed)")
	'			'	valid = valid And Check(count2 <= 3, "To many Berufserfahrung_Position values (max 2 are allowed)")

	'			'End If

	'			''Check Angebot
	'			'If Angebot.HasValue Then

	''Select Case Angebot
	''Case 27 ' Classic Konto 
	''Case 28 ' Classic Kontingent
	''Case 29 ' Plus Konto 
	''Case 30 ' Plus Kontingent
	''Case 31 ' Premium Konto
	''Case 32 ' Premium Kontingent 
	''Case 35 ' Starter Konto 
	''Case 36 ' Starter Kontingent 
	''Case 37 ' Standard Konto 
	''Case 38 ' Standard Kontingent 
	''Case 39 ' Professional Konto 
	''Case 40 ' Professional Kontingent 
	''Case 41 ' Expert Konto 
	''Case 42 ' Expert Kontingent 

	''Case 0  ' empty value -> will not be written
	''' ok
	''Case Else
	''						valid = valid And Check(False, "Angebot must be in [0, 27, 28, 29, 30, 31, 32, 35, 36, 37, 38, 39, 40, 41]")
	''				End Select

	'			'End If

	'			Return valid

	'		End Get

	'	End Property

	'''' <summary>
	'''' Gets the validation errors.
	'''' </summary>
	'Public ReadOnly Property ValidationErrors As String
	'	Get
	'		Return m_ValidationErrors.ToString()
	'	End Get
	'End Property


	'End Class

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