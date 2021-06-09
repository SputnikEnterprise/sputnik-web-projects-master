
Namespace DataTransferObject.SystemInfo.DataObjects


	Public Class WebServiceResult

		Public Property JobResult As Boolean
		Public Property JobResultMessage As String

	End Class

	Public Class NumberOfVacancyData

		Public Property CustomerID As String
		Public Property WOS_Guid As String
		Public Property NumberOfInternal As Integer?
		Public Property NumberOfJobChannel As Integer?
		Public Property NumberOfJobCH As Integer?
		Public Property NumberOfOstJob As Integer?

	End Class

	''' <summary>
	''' Mandant data.
	''' </summary>
	<Serializable()>
	Public Class CustomerSettingData
		Public Property CustomerID As String
		Public Property ID As Integer
		Public Property UserKey As String
		Public Property Passwort As String
		Public Property eMail As String
		Public Property Customer_Name As String
		Public Property KD_ZHD As String
		Public Property ModulName As String
		Public Property Vak_Guid As String
		Public Property MA_Guid As String
		Public Property KD_Guid As String
		Public Property DP_Guid As String
		Public Property Verleih_Guid As String
		Public Property Customer_Ort As String
		Public Property Customer_Telefon As String
		Public Property Customer_Telefax As String
		Public Property Customer_eMail As String
		Public Property Customer_Strasse As String
		Public Property Customer_Homepage As String

	End Class

	<Serializable()>
	Public Class NLOGData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property level As String
		Public Property callSite As String
		Public Property type As String
		Public Property message As String
		Public Property stackTrace As String
		Public Property innerException As String
		Public Property additionalInfo As String

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
	Public Class JobplattformCounterDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property WOS_ID As String
		Public Property OwnCounter As Integer
		Public Property JobsCHCounter As Integer
		Public Property OstJobCounter As Integer
		Public Property JobChannelPriorityCounter As Integer

	End Class

	Public Class JobplattformsCustomerData

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property CustomerNumber As Integer
		Public Property PlattformLabel As String
		Public Property CustomerName As String
		Public Property Advisorfullname As String
		Public Property CreatedOn As DateTime
		Public Property CreatedFrom As String

	End Class

	<Serializable()>
	Public Class SystemUserData

		Public Property UserNr As Integer
		Public Property UserGuid As String
		Public Property UserLoginname As String
		Public Property UserLoginPassword As String
		Public Property UserSalutation As String
		Public Property UserLName As String
		Public Property UserFName As String

		Public Property Birthdate As Date?

		Public Property UserKST As String
		Public Property UserKST_1 As String
		Public Property UserKST_2 As String

		Public Property UserBranchOffice As String
		Public Property UserFiliale As String
		Public Property UserFTitel As String
		Public Property UserSTitel As String

		Public Property UserTelefon As String
		Public Property UserTelefax As String
		Public Property UserMobile As String
		Public Property UsereMail As String
		Public Property UserLanguage As String

		Public Property UserMDTelefon As String
		Public Property UserMDDTelefon As String
		Public Property UserMDTelefax As String
		Public Property UserMDeMail As String
		Public Property UserMDGuid As String

		Public Property UserMDName As String
		Public Property UserMDName2 As String
		Public Property UserMDName3 As String
		Public Property UserMDPostfach As String
		Public Property UserMDStrasse As String
		Public Property UserMDPLZ As String
		Public Property UserMDOrt As String
		Public Property UserMDCanton As String
		Public Property UserMDLand As String
		Public Property UserMDHomepage As String

		Public Property UserFullNameWithComma As String
		Public Property UserFullName As String

		Public Property EMail_UserName As String
		Public Property EMail_UserPW As String
		Public Property EMail_SMTP As String
		Public Property Deactivated As Boolean?
		Public Property Customer_ID As String
		Public Property jch_layoutID As Integer?
		Public Property jch_logoID As Integer
		Public Property OstJob_ID As String
		Public Property ostjob_Kontingent As Integer?
		Public Property JCH_SubID As Integer?

		Public Property UserPicture As Byte()
		Public Property UserSign As Byte()
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?
		Public Property ChangedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property AsCostCenter As Boolean?
		Public Property LogonMorePlaces As Boolean?

		Public Property EmployeeWOSID As String
		Public Property CustomerWOSID As String

		Public ReadOnly Property UserFullname_1
			Get
				Return String.Format("{0}, {1}", UserLName, UserFName)
			End Get
		End Property

	End Class


	'<Serializable()>
	'Public Class NotifyDTO
	'	Public Property ID As Integer
	'	Public Property Customer_ID As String
	'	Public Property NotifyHeader As String
	'	Public Property NotifyComments As String
	'	Public Property NotifyArt As NotifyEnum
	'	Public Property CreatedOn As DateTime?
	'	Public Property CreatedFrom As String
	'	Public Property CheckedOn As DateTime?
	'	Public Property CheckedFrom As String

	'	Public Enum NotifyEnum
	'		NotDefined
	'		AsError
	'		AsImportant
	'		AsInfo
	'		AsScanJobImportInfo
	'		AsScanJobErrorInfo
	'	End Enum

	'End Class

	<Serializable()>
	Public Class CustomerNotificationDataDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property User_ID As String
		Public Property NotifyGroup As String
		Public Property NotifyHeader As String
		Public Property NotifyComments As String
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

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
	Public Class EMailNotificationDTO

		Public Property ID As Integer
		Public Property Customer_ID As String
		Public Property CustomerName As String
		Public Property CustomerLocation As String
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
	Public Class AdvisorLoginData
		Public Property Customer_ID As String
		Public Property CustomerName As String
		Public Property UserCount As Integer?
		Public Property Advisorname As String
		Public Property LogYear As Integer?
		Public Property LogMonth As Integer?
		Public Property CreatedOn As Date?

	End Class

	<Serializable()>
	Public Class StationData
		Public Property LocalIPAddress As String
		Public Property ExternalIPAddress As String
		Public Property LocalHostName As String
		Public Property LocalUserName As String
		Public Property LocalDomainName As String

	End Class

	<Serializable()>
	Public Class CustomerMDData
		Public Property CustomerID As String
		Public Property LocalIPAddress As String
		Public Property ExternalIPAddress As String
		Public Property LocalHostName As String
		Public Property LocalDomainName As String

	End Class


	<Serializable()>
	Public Class SBN2000GroupHeaderData
		Public Property GroupHeaderNumber As Integer
		Public Property GroupHeader_1 As Integer?
		Public Property GroupHeader_2 As Integer?
		Public Property GroupHeader_3 As Decimal?
		Public Property JobTitle As String

	End Class

	<Serializable()>
	Public Class STMPJobData
		Public Property ID As Integer?
		Public Property GroupNumber As Integer?
		Public Property TitleNumber As Integer?
		Public Property Bez_DE As String
		Public Property Bez_FR As String
		Public Property Bez_IT As String
		Public Property Bez_Translated As String
		Public Property Group_DE As String
		Public Property Group_FR As String
		Public Property Group_IT As String
		Public Property Group_Translated As String
		Public Property Notifiable As Boolean

	End Class

	<Serializable()>
	Public Class STMPMappingData
		Public Property OLD_AVAMNumber As Integer?
		Public Property New_AVAMNumber As Integer?
		Public Property OLD_Bez_Translated As String
		Public Property New_Bez_Translated As String

	End Class


	<Serializable()>
	Public Class SBN2000Data
		Public Property GroupHeaderNumber As Integer
		Public Property JobData As List(Of SBN2000Group1Data)

	End Class

	<Serializable()>
	Public Class LocationGoordinateDataDTO
		Public Property ID As Integer?
		Public Property CountryCode As String
		Public Property Postcode As String
		Public Property PlaceName As String
		Public Property AdminName1 As String
		Public Property AdminCode1 As String
		Public Property AdminName2 As String
		Public Property AdminCode2 As String
		Public Property AdminName3 As String
		Public Property AdminCode3 As String
		Public Property Longitude As Double?
		Public Property Latitude As Double?
		Public Property Accuracy As Integer?

	End Class

	Public Class SBN2000Group1Data
		Public Property JobGroup_1 As Integer
		Public Property JobData As List(Of SBN2000Group2Data)

	End Class

	Public Class SBN2000Group2Data
		Public Property JobGroup_2 As Integer
		Public Property JobData As List(Of SBN2000Group3Data)

	End Class

	Public Class SBN2000Group3Data
		Public Property JobGroup_3 As Decimal
		Public Property JobLabel As String

	End Class

	<Serializable()>
	Public Class CVLBaseDataDTO

		Public Property ID As Integer
		Public Property Code As String
		Public Property Bez_DE As String
		Public Property Translated_Value As String

	End Class

  <Serializable()>
  Public Class UserFormControlTemplateDTO

    Public Property ID As Integer
    Public Property Customer_ID As String
    Public Property User_ID As String
    Public Property TemplateName As String
    Public Property FieldName As String
    Public Property FieldData As String
    Public Property CreatedOn As DateTime?
    Public Property CreatedFrom As String

  End Class

	<Serializable()>
	Public Class VacancyJobCHPeripheryDTO
		Public Property ID As Integer
		Public Property ID_Parent As Integer
		Public Property RecNr As Integer
		Public Property TranslatedLabel As String

	End Class


	<Serializable()>
	Public Class AvailableEmployeeDTO
		Public Property ID As Integer
		Public Property WOS_ID As String
		Public Property Customer_ID As String
		Public Property EmployeeNumber As Integer
		Public Property CustomerData As CustomerSettingData
		Public Property EmployeeAdvisor As String
		Public Property Advisor_ID As String
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property BrunchOffice As String
		Public Property Canton As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property MainLanguage As String
		Public Property HowContact As String
		Public Property FirstState As String
		Public Property SecondState As String
		Public Property Qualifications As String
		Public Property Branches As String
		Public Property JobProzent As String
		Public Property BirthDay As DateTime?
		Public Property Gender As String
		Public Property DriverLicenses As String
		Public Property Civilstate As String
		Public Property AvailableMobility As String
		Public Property Nationality As String
		Public Property Permit As String
		Public Property Salutation As String

		Public Property Transfer_UserID As String

		Public Property CreatedOn As DateTime?
		Public Property Transfer_ID As String
		Public Property SpokenLanguages As String
		Public Property WritingLanguages As String
		Public Property Properties As String
		Public Property EmployeeReserveFields As AvailableEmployeeReserveFields
		Public Property EmployeeApplicationReserveFields As AvailableEmployeeApplicationFields
		Public Property EmployeeAdvisorData As WOSAdvisorData
		Public Property AvailableEmployeeTemplates As AvailableEmployeeTemplateData
		Public Property ExistsTemplate As Boolean?

		Public Property DesiredWagesOld As Decimal?
		Public Property DesiredWagesNew As Decimal?
		Public Property DesiredWagesInMonth As Decimal?
		Public Property DesiredWagesInHour As Decimal?

	End Class

	<Serializable()>
	Public Class AvailableEmployeeNewDTO
		Public Property ID As Integer
		Public Property WOS_ID As String
		Public Property Customer_ID As String
		Public Property EmployeeNumber As Integer
		Public Property CustomerData As CustomerSettingData
		Public Property EmployeeAdvisor As String
		Public Property Advisor_ID As String
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property BrunchOffice As String
		Public Property Canton As String
		Public Property Postcode As String
		Public Property Location As String
		Public Property MainLanguage As String
		Public Property HowContact As String
		Public Property FirstState As String
		Public Property SecondState As String
		Public Property Qualifications As String
		Public Property Branches As String
		Public Property JobProzent As String
		Public Property BirthDay As DateTime?
		Public Property Gender As String
		Public Property DriverLicenses As String
		Public Property Civilstate As String
		Public Property AvailableMobility As String
		Public Property Nationality As String
		Public Property Permit As String
		Public Property Salutation As String

		Public Property Transfer_UserID As String

		Public Property CreatedOn As DateTime?
		Public Property Transfer_ID As String
		Public Property SpokenLanguages As String
		Public Property WritingLanguages As String
		Public Property Properties As String
		Public Property EmployeeReserveFields As AvailableEmployeeReserveFields
		Public Property EmployeeApplicationReserveFields As AvailableEmployeeApplicationFields
		Public Property EmployeeAdvisorData As WOSAdvisorData
		Public Property AvailableEmployeeTemplates As AvailableEmployeeTemplateData
		Public Property ExistsTemplate As Boolean?

		Public Property DesiredWagesOld As Decimal?
		Public Property DesiredWagesNew As Decimal?
		Public Property DesiredWagesInMonth As Decimal?
		Public Property DesiredWagesInHour As Decimal?

	End Class


	<Serializable()>
	Public Class AvailableEmployeeTemplateData
		Public Property ID As Integer?
		Public Property Customer_ID As String
		Public Property WOS_ID As String
		Public Property EmployeeNumber As Integer
		Public Property ScanDoc As Byte()
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?

	End Class

	<Serializable()>
	Public Class AvailableEmployeeReserveFields
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer
		Public Property Reserve1 As String
		Public Property Reserve2 As String
		Public Property Reserve3 As String
		Public Property Reserve4 As String
		Public Property Reserve5 As String
		Public Property Reserve6 As String

	End Class


	<Serializable()>
	Public Class AvailableEmployeeApplicationFields
		Public Property ID As Integer
		Public Property EmployeeNumber As Integer
		Public Property LL_Name As String
		Public Property ApplicationReserve0 As String
		Public Property ApplicationReserve1 As String
		Public Property ApplicationReserve2 As String
		Public Property ApplicationReserve3 As String
		Public Property ApplicationReserve4 As String
		Public Property ApplicationReserve5 As String
		Public Property ApplicationReserve6 As String
		Public Property ApplicationReserve7 As String
		Public Property ApplicationReserve8 As String
		Public Property ApplicationReserve9 As String
		Public Property ApplicationReserve10 As String
		Public Property ApplicationReserve11 As String
		Public Property ApplicationReserve12 As String
		Public Property ApplicationReserve13 As String
		Public Property ApplicationReserve14 As String
		Public Property ApplicationReserve15 As String
		Public Property ApplicationReserveRtf0 As String
		Public Property ApplicationReserveRtf1 As String
		Public Property ApplicationReserveRtf2 As String
		Public Property ApplicationReserveRtf3 As String
		Public Property ApplicationReserveRtf4 As String
		Public Property ApplicationReserveRtf5 As String
		Public Property ApplicationReserveRtf6 As String
		Public Property ApplicationReserveRtf7 As String
		Public Property ApplicationReserveRtf8 As String
		Public Property ApplicationReserveRtf9 As String
		Public Property ApplicationReserveRtf10 As String
		Public Property ApplicationReserveRtf11 As String
		Public Property ApplicationReserveRtf12 As String
		Public Property ApplicationReserveRtf13 As String
		Public Property ApplicationReserveRtf14 As String
		Public Property ApplicationReserveRtf15 As String

	End Class

	<Serializable()>
	Public Class WOSAdvisorData
		Public Property Customer_ID As String
		Public Property WOS_ID As String
		Public Property User_ID As String
		Public Property Firstname As String
		Public Property Lastname As String
		Public Property Telephon As String
		Public Property Telefax As String
		Public Property EMail As String
		Public Property UserInitial As String

	End Class


	<Serializable()>
	Public Class MainViewSettingFileDTO

		Public Property Filename As String
		Public Property FileDate As DateTime?
		Public Property UpdateFileTime As String
		Public Property UpdateFileSize As Long
		Public Property FileContent As Byte()


	End Class

	Public Enum ModulNumberEnum
		COMMON
		EMPLOYEE
		CUSTOMER
		VACANCY
		PROPOSE
		EMPLOYMENT
		REPORT
		INVOICE
		ADVANCEDPAYMENT
		PAYROLL
	End Enum


End Namespace
