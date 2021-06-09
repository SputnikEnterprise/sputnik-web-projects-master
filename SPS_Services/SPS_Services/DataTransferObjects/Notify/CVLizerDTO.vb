

Namespace DataTransferObject.CVLizer.DataObjects


	<Serializable()>
	Public Class CVLizerCustomerDataDTO
		Public Property Customer_ID As String
		Public Property CustomerName As String
		Public Property CustomerNumber As Integer?
		Public Property CustomerGroupNumber As Integer?
		Public Property Location As String

	End Class

	<Serializable()>
  Public Class CVLizerProfileDataDTO
		Public Property ProfileID As Integer?
		Public Property PersonalID As Integer?
		Public Property ApplicationID As Integer?
		Public Property ApplicantID As Integer?
		Public Property GenderLabel As String
		Public Property FirstName As String
		Public Property LastName As String
		Public Property ApplicationLabel As String
		Public Property WorkID As Integer?
		Public Property EducationID As Integer?
		Public Property AdditionalID As Integer?
		Public Property ObjectiveID As Integer?
		Public Property DateOfBirth As Date?

		Public Property Customer_ID As String
		Public Property CustomerName As String
		Public Property CreatedOn As Date?
		Public Property CreatedFrom As String
		Public Property ChangedOn As Date?
		Public Property ChangedFrom As String

	End Class

	<Serializable()>
	Public Class CVLPersonalDataDTO

		Public Property PersonalID As Integer?
		Public Property FirstName As String
		Public Property LastName As String
		Public Property Gender As String

		Public Property GenderLabel As String
		Public Property IsCed As String
		Public Property IsCedLable As String
		Public Property DateOfBirth As Date?
		Public Property DateOfBirthPlace As String
		Public Property Nationality As String
		Public Property NationalityLable As String
		Public Property CivilState As String
		Public Property CivilStateLable As String

		'Public Property Email As List(Of String)
		'Public Property PhoneNumbers As List(Of String)
		'Public Property Homepage As List(Of String)
		'Public Property TelefaxNumber As List(Of String)
		Public Property PersonalPhoto As Byte()
		Public Property ISValid As Boolean?

		Public Property PersonalTitle As List(Of CVLListsDTO)
		Public Property PersonalEMail As List(Of CVLListsDTO)
		Public Property PersonalHomepage As List(Of CVLListsDTO)
		Public Property PersonalTelephone As List(Of CVLListsDTO)
		Public Property PersonalTelefax As List(Of CVLListsDTO)

		Public Property PersonalAddress As CVLAddressDTO

    'Public ReadOnly Property Fullname As String
    '	Get
    '		Return String.Format("{0} {1}", FirstName, LastName)
    '	End Get
    'End Property

    'Public ReadOnly Property DateOfBirthYear As Integer?
    '	Get
    '		Return Year(DateOfBirth)
    '	End Get
    'End Property


  End Class


  <Serializable()>
  Public Class CVLAddressDTO
		Public Property ID As Integer?
		Public Property Street As String
		Public Property Postcode As String
		Public Property City As String
		Public Property Country As String
		Public Property CountryLable As String
		Public Property State As String


    'Public ReadOnly Property AddressLable As String
    '	Get
    '		Return String.Format("{0} {1}-{2} {3}", Street, Country, Postcode, City)
    '	End Get
    'End Property

  End Class

	<Serializable()>
	Public Class DocumentData
		Public Property ID As Integer?

		Public Property DocClass As String
		Public Property Pages As Integer?
		Public Property Plaintext As String
		Public Property FileType As String
		Public Property DocBinary As Byte()
		Public Property DocID As Integer?
		Public Property DocSize As Integer?
		Public Property DocLanguage As String
		Public Property FileHashvalue As String
		Public Property DocXML As String

	End Class

	'<Serializable()>
	Public Class CVLListsDTO
		Public Property ID As Integer?
		Public Property PersonalID As Integer?
		Public Property Lable As String

	End Class

	<Serializable()>
	Public Class WorkPhaseViewDataDTO
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property WorkID As Integer?
		Public Property WorkPhaseID As Integer?

		Public Property Companies As List(Of CodeViewData)
		Public Property Functions As List(Of CodeViewData)
		Public Property Positions As List(Of CodeNameViewData)
		Public Property Project As Boolean?
		Public Property Employments As List(Of CodeNameViewData)
		Public Property WorkTimes As List(Of CodeNameViewData)

	End Class


	<Serializable()>
	Public Class EducationPhaseViewDataDTO
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property EducationID As Integer?
		Public Property EducationPhaseID As Integer?
		Public Property SchooolNames As List(Of CodeViewData)
		Public Property Graduations As List(Of CodeViewData)
		Public Property EducationTypes As List(Of CodeNameWeightViewData)
		Public Property Completed As Boolean?
		Public Property Score As Integer?
		Public Property IsCedCode As String
		Public Property IsCedCodeLable As String

	End Class


	<Serializable()>
	Public Class AdditionalInfoViewDataDTO

		Public Property ID As Integer?
		Public Property Languages As List(Of LanguageData)
		Public Property DrivingLicences As List(Of CodeViewData)
		Public Property MilitaryService As Boolean?
		Public Property Competences As String
		Public Property Interests As String
		Public Property Additionals As String
		Public Property UndatedSkills As List(Of CodeNameWeightViewData)
		Public Property UndatedOperationArea As List(Of CodeNameWeightViewData)
		Public Property UndatedIndustries As List(Of CodeNameWeightViewData)
		Public Property InternetResources As List(Of InternetResourceViewData)

	End Class


	<Serializable()>
	Public Class PersonalInformationDataDTO

    Public Property ID As Integer?
    Public Property FK_CVLID As Integer?
    Public Property FirstName As String
    Public Property LastName As String
    Public Property FK_GenderCode As String
    Public Property FK_IsCedCode As String
    Public Property DateOfBirth As DateTime?
    Public Property PlaceOfBirth As String
    Public Property Address As AddressData

    Public Property Title As List(Of PropertyListData)
    Public Property Nationality As List(Of PropertyListData)
    Public Property CivilState As List(Of PropertyListData)
    Public Property Email As List(Of PropertyListData)
    Public Property PhoneNumbers As List(Of PropertyListData)
    Public Property Homepage As List(Of PropertyListData)
    Public Property TelefaxNumber As List(Of PropertyListData)

  End Class


	<Serializable()>
  Public Class WPhaseDataDTO
    Public Property ID As Integer?
    Public Property WorkPhases As List(Of WorkPhaseDataDTO)
    Public Property AdditionalText As String

  End Class

  <Serializable()>
  Public Class EdPhaseDataDTO
    Public Property ID As Integer?
    Public Property EducationPhases As List(Of EducationPhaseDataDTO)
    Public Property AdditionalText As String

  End Class

  <Serializable()>
  Public Class PublicationDataDTO
    Inherits Phase

    Public Property PublicationPhaseID As Integer?
    Public Property Author As List(Of PropertyListData)
    Public Property Proceedings As String
    Public Property Institute As String

  End Class


  <Serializable()>
  Public Class Phase
    Public Property PhaseID As Integer?
    Public Property DateFrom As Date?
    Public Property DateTo As Date?
    Public Property DateFromFuzzy As String
    Public Property DateToFuzzy As String
    Public Property Duration As Integer?
    Public Property Current As Boolean?
    Public Property SubPhase As Boolean?
    Public Property Location As List(Of AddressData)
    Public Property Skill As List(Of CodeNameWeightedData)
    Public Property SoftSkill As List(Of CodeNameWeightedData)

    Public Property OperationAreas As List(Of CodeNameWeightedData)
    Public Property Industries As List(Of CodeNameWeightedData)
    Public Property CustomCodes As List(Of CodeNameWeightedData)

    Public Property Topic As List(Of CodeNameData)
    Public Property Comments As String
    Public Property PlainText As String
    Public Property InternetRosources As List(Of InternetResource)
    Public Property DocumentID As List(Of CodeIDData)

  End Class


  <Serializable()>
  Public Class WorkPhaseDataDTO
    Inherits Phase

    Public Property WorkPhaseID As Integer?

    Public Property Company As List(Of PropertyListData)
    Public Property Functions As List(Of PropertyListData)
    Public Property Positions As List(Of CodeNameData)
    Public Property Project As Boolean?
    Public Property Employments As List(Of CodeNameData)
    Public Property WorkTimes As List(Of CodeNameData)

  End Class


  <Serializable()>
  Public Class EducationPhaseDataDTO
    Inherits Phase

    Public Property EducationPhaseID As Integer?
    Public Property IsCed As String
    Public Property EducationType As List(Of CodeNameWeightedData)
    Public Property SchoolName As List(Of PropertyListData)
    Public Property Graduation As List(Of PropertyListData)
    Public Property Completed As Boolean?
    Public Property Score As Integer?

  End Class


  <Serializable()>
  Public Class OtherInformationDataDTO

    Public Property ID As Integer?
    Public Property Languages As List(Of LanguageData)
    Public Property DrivingLicence As List(Of PropertyListData)
    Public Property MilitaryService As Boolean?
    Public Property Competences As String
    Public Property Interests As String
    Public Property Additionals As String
    Public Property UndatedSkill As List(Of CodeNameWeightedData)
    Public Property UndatedOperationArea As List(Of CodeNameWeightedData)
    Public Property UndatedIndustry As List(Of CodeNameWeightedData)
    Public Property InternetRosources As List(Of InternetResource)

  End Class


	<Serializable()>
	Public Class PublicationViewDataDTO
		Inherits PhaseViewData

		Public Property ID As Integer?
		Public Property Author As List(Of CodeViewData)
		Public Property Proceedings As String
		Public Property Institute As String

	End Class


	<Serializable()>
  Public Class ObjectiveDataDTO
    Inherits WorkPhaseDataDTO

    Public Property ID As Integer?
    Public Property Salary As List(Of PropertyListData)
    Public Property AvailabilityDate As Date?

  End Class


	<Serializable()>
	Public Class DocumentViewDataDTO

		Public Property ID As Integer?
		Public Property DocClass As String
		Public Property Pages As Integer?
		Public Property Plaintext As String
		Public Property FileType As String
		Public Property DocBinary As Byte()
		Public Property DocID As Integer?
		Public Property DocSize As Integer?
		Public Property DocLanguage As String
		Public Property FileHashvalue As String
		Public Property DocXML As String


	End Class


	<Serializable()>
	Public Class PostcodeCityViewDataDTO

		Public Property Customer_ID As String
		Public Property Postcode As String
		Public Property City As String

	End Class

	<Serializable()>
	Public Class FunctionTitelsViewDataDTO
		Public Property Customer_ID As String
		Public Property FunctionLabel As String

	End Class

	<Serializable()>
	Public Class ExperiencesViewDataDTO
		Public Property Customer_ID As String
		Public Property Code As String
		Public Property ExperienceLabel As String

	End Class

	<Serializable()>
	Public Class LanguageViewDataDTO

		Public Property Customer_ID As String
		Public Property LanguageCode As String
		Public Property LanguageLabel As String

	End Class

	<Serializable()>
	Public Class CVLSearchResultDataDTO

		Public Property Customer_ID As String
		Public Property CVLProfileID As Integer?
		Public Property PersonalID As Integer?
		Public Property EmployeeID As Integer?
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property Postcode As String
		Public Property Street As String
		Public Property Location As String
		Public Property CountryCode As String
		Public Property DateOfBirth As DateTime?
		Public Property EmployeeAge As Integer?
		Public Property JobTitel As String
		Public Property CreatedOn As DateTime?

	End Class

	<Serializable()>
	Public Class CVLSearchHistoryDataDTO

		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property QueryName As String
		Public Property QueryContent As String
		Public Property QueryResultContent As String
		Public Property Notify As Boolean
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ResultCount As Integer?

	End Class

	Public Enum JoinENum
		UND
		ODER
	End Enum


	<Serializable()>
	Public Class EMailDataDTO
		Public Property ID As Integer?
		Public Property ApplicationID As Integer?
		Public Property Customer_ID As String
		Public Property EMailUidl As Integer?
		Public Property EMailFrom As String
		Public Property EMailTo As String
		Public Property EMailSubject As String
		Public Property EMailBody As String
		Public Property EMailPlainTextBody As String
		Public Property HasHtmlBody As Boolean?
		Public Property EMailDate As DateTime?
		Public Property EMailAttachment As List(Of EMailAttachment)
		Public Property EMailMime As String
		Public Property EMailContent As Byte()
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class

	Public Class EMailAttachment
		Public Property ID As Integer?
		Public Property FK_REID As Integer?
		Public Property DocumentCategoryNumber As Integer?
		Public Property AttachmentSize As Byte()
		Public Property AttachmentName As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String

	End Class


	<Serializable()>
  Public Class CVCodeSummaryDataDTO
    Inherits CodeNameWeightedData

    Public Property ID As Integer?
    Public Property Duration As Integer?
    Public Property Domain As String

  End Class

  <Serializable()>
  Public Class DocumentDataDTO
    Public Property ID As Integer?
    Public Property DocClass As String
    Public Property Pages As Integer?
    Public Property Plaintext As String
    Public Property FileType As String
    Public Property DocBinary As Byte()
    Public Property DocID As Integer?
    Public Property DocSize As Integer?
    Public Property DocLanguage As String

  End Class


  <Serializable()>
  Public Class CodeNameData
    Public Property CodeNameID As Integer?
    Public Property FK_ID As Integer?
    Public Property Code As String
    Public Property CodeName As String

  End Class

  <Serializable()>
  Public Class CodeIDData
    Public Property CodeID As Integer?
    Public Property FK_ID As Integer?
    Public Property Code As Integer?

  End Class

  <Serializable()>
  Public Class AddressData
    Public Property ID As Integer?
    Public Property FK_PersonalID As Integer?
    Public Property Street As String
    Public Property Postcode As String
    Public Property City As String
    Public Property FK_CountryCode As String
    Public Property State As String

  End Class

  <Serializable()>
  Public Class CodeNameWeightedData
    Public Property CodeNameWeightedID As Integer?
    Public Property FK_ID As Integer?
    Public Property Code As String
    Public Property Name As String
    Public Property Weight As Double?

  End Class

  <Serializable()>
  Public Class InternetResource
    Public Property InternetResourceID As Integer?
    Public Property FK_ID As Integer?
    Public Property URL As String
    Public Property Title As String
    Public Property Source As String
    Public Property Snippet As String

  End Class

  <Serializable()>
  Public Class LanguageData
    Inherits CodeNameData

    Public Property Level As CodeNameData

  End Class

  <Serializable()>
  Public Class PropertyListData
    Public Property ID As Integer?
    Public Property FK_ID As Integer?
    Public Property PropertyName As String

  End Class

  <Serializable()>
  Public Class PhaseViewData

    Public Property PhaseID As Integer?
    Public Property DateFrom As Date?
    Public Property DateTo As Date?
    Public Property DateFromFuzzy As String
    Public Property DateToFuzzy As String
    Public Property Duration As Integer?
    Public Property Current As Boolean?
    Public Property SubPhase As Boolean?
    Public Property Comments As String
    Public Property PlainText As String
    Public Property Locations As List(Of CVLAddressDTO)
    Public Property Skills As List(Of CodeNameWeightViewData)
    Public Property SoftSkills As List(Of CodeNameWeightViewData)
    Public Property OperationAreas As List(Of CodeNameWeightViewData)
    Public Property Industries As List(Of CodeNameWeightViewData)
    Public Property CustomCodes As List(Of CodeNameWeightViewData)
    Public Property Topic As List(Of CodeViewData)
    Public Property InternetResources As List(Of InternetResourceViewData)
    Public Property DocumentID As List(Of IDiewData)

  End Class


  Public Class CodeViewData
		Public Property ID As Integer?
		Public Property Lable As String

	End Class

	Public Class IDiewData
		Public Property ID As Integer?
		Public Property CodeNumber As Integer

	End Class

  <Serializable()>
  Public Class CodeNameWeightViewData
    Public Property ID As Integer?
    Public Property PhaseID As Integer?
    Public Property Code As String
    Public Property Name As String
    Public Property Weight As Double?

  End Class

  <Serializable()>
  Public Class CodeNameViewData
    Public Property ID As Integer?
    Public Property PhaseID As Integer?
    Public Property Code As String
    Public Property Name As String

  End Class

  <Serializable()>
  Public Class InternetResourceViewData
    Public Property ID As Integer?
    Public Property PhaseID As Integer?
    Public Property URL As String
    Public Property Title As String
    Public Property Source As String
    Public Property Snippet As String

  End Class


End Namespace

