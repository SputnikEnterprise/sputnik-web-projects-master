
'Namespace JobPlatform


Imports sp_WebServiceUtility

<Serializable()>
Public Class MandantData
	Public Property ID As Integer
	Public Property MandantNumber As Integer?
	Public Property MandantName1 As String
	Public Property MandantName2 As String
	Public Property MandantCanton As String
	Public Property Street As String
	Public Property Postcode As String
	Public Property Location As String
	Public Property Telephon As String
	Public Property Telefax As String
	Public Property EMail As String
	Public Property Homepage As String

	Public Property MandantDbConnection As String
	Public Property CustomerID As String

End Class


<Serializable()>
Public Class SPAdvisorData
	Public Property UserMDNr As Integer?
	Public Property UserNumber As Integer
	Public Property Firstname As String
	Public Property Lastname As String
	Public Property Salutation As String
	Public Property KST As String
	Public Property KST1 As String
	Public Property KST2 As String
	Public Property UserGuid As String


	Public Property UserLoginname As String
	Public Property UserLoginPassword As String
	Public Property UserBusinessBranch As String
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
	Public Property MDCanton As String
	Public Property UserMDLand As String
	Public Property UserMDHomepage As String

	Public ReadOnly Property UserFullname As String
		Get
			Return Strings.Trim(Lastname) + ", " + Strings.Trim(Firstname)
		End Get
	End Property
	Public ReadOnly Property UserFullnameReversedWithoutComma As String
		Get
			Return Strings.Trim(Firstname) + " " + Strings.Trim(Lastname)
		End Get
	End Property
End Class


<Serializable()>
Public Class CustomerMasterData
	Public Property CustomerMandantNumber As Integer
	Public Property CustomerNumber As Integer
	Public Property WOSGuid As String
	Public Property SolvencyDecisionID As Integer
	Public Property SolvencyInfo As String
	Public Property Company1 As String
	Public Property Company2 As String
	Public Property Company3 As String
	Public Property PostOfficeBox As String
	Public Property Street As String
	Public Property CountryCode As String
	Public Property Postcode As String
	Public Property Latitude As Double?
	Public Property Longitude As Double?
	Public Property Location As String
	Public Property Telephone As String
	Public Property Telefax As String
	Public Property Telefax_Mailing As Boolean
	Public Property EMail As String
	Public Property Email_Mailing As Boolean
	Public Property Hompage As String
	Public Property facebook As String
	Public Property xing As String

	Public Property KST As String
	Public Property FirstProperty As Decimal?
	Public Property Language As String
	Public Property HowContact As String
	Public Property CustomerState1 As String
	Public Property CustomerState2 As String
	Public Property NoUse As Boolean
	Public Property NoUseComment As String
	Public Property Comment As String
	Public Property SalaryPerMonth As Decimal?
	Public Property SalaryPerHour As Decimal?
	Public Property Reserve1 As String
	Public Property Reserve2 As String
	Public Property Reserve3 As String
	Public Property Reserve4 As String
	Public Property CreditLimit1 As Decimal?
	Public Property CreditLimit2 As Decimal?
	Public Property CreditLimitsFromDate As DateTime?
	Public Property CreditLimitsToDate As DateTime?
	Public Property OpenInvoiceAmount As Decimal?
	Public Property ReferenceNumber As String
	Public Property KD_UmsMin As Decimal?
	Public Property mwstpflicht As Boolean?
	Public Property NumberOfCopies As Short?
	Public Property ValueAddedTaxNumber As String
	Public Property CreditWarning As Boolean?
	Public Property OPShipment As String
	Public Property NotPrintReports As Boolean?
	Public Property TermsAndConditions_WOS As String
	Public Property sendToWOS As Boolean?
	Public Property DoNotShowContractInWOS As Boolean?
	Public Property CurrencyCode As String
	Public Property BillTypeCode As String
	Public Property NumberOfEmployees As String
	Public Property CanteenAvailable As Boolean?
	Public Property TransportationOptions As Boolean?
	Public Property InvoiceOption As String
	Public Property ShowHoursInNormal As Boolean?
	Public Property CreatedOn As DateTime?
	Public Property CreatedFrom As String
	Public Property ChangedOn As DateTime?
	Public Property ChangedFrom As String
	Public Property Transfered_Guid As String

	Public Function Clone() As CustomerMasterData
		Return DirectCast(Me.MemberwiseClone(), CustomerMasterData)
	End Function

	Public ReadOnly Property CustomerPostcodeLocation As String
		Get
			Return String.Format("{0} {1}", Postcode, Location)
		End Get
	End Property

End Class

<Serializable()>
Public Class CustomerUserData

	Public Customer_VacancyNr As String
	Public Customer_EmployeeNr As String
	Public Customer_CustomerNr As String

	Public Customer_Name As String
	Public Customer_Street As String
	Public Customer_Postcode As String
	Public Customer_City As String
	Public Customer_Country As String

	Public Customer_Telephone As String
	Public Customer_Telefax As String
	Public Customer_eMail As String
	Public Customer_Homepage As String

	Public User_Lastname As String
	Public User_Firstname As String
	Public User_Telephone As String
	Public User_Telefax As String
	Public User_eMail As String
	Public User_Homepage As String

	Public User_Filiale As String
	Public User_Initial As String
	Public User_Salution As String

	Public Loged_UserName As String
	Public Loged_UserGuid As String


End Class


<Serializable()>
Public Class VacancyMasterData

	Public Property ID As Integer?
	Public Property VakNr As Integer?
	Public Property Berater As String
	Public Property Filiale As String
	Public Property VakKontakt As String
	Public Property VakState As String
	Public Property VakKontakt_Value As Integer?
	Public Property VakState_Value As Integer?
	Public Property Bezeichnung As String
	Public Property Slogan As String
	Public Property Gruppe As String
	Public Property SubGroup As String
	Public Property KDNr As Integer?
	Public Property SBNNumber As Integer?
	Public Property SBNPublicationState As Integer?
	Public Property SBNPublicationDate As DateTime?
	Public Property SBNPublicationFrom As String
	Public Property KDZHDNr As Integer?
	Public Property ExistLink As Boolean?
	Public Property VakLink As String
	Public Property Beginn As String
	Public Property JobProzent As String
	Public Property Anstellung As String
	Public Property Dauer As String
	Public Property MAAge As String
	Public Property MASex As String
	Public Property MAZivil As String
	Public Property MALohn As String
	Public Property Jobtime As String
	Public Property JobOrt As String
	Public Property MAFSchein As String
	Public Property MAAuto As String
	Public Property MANationality As String
	Public Property IEExport As Boolean?
	Public Property KDBeschreibung As String
	Public Property KDBietet As String
	Public Property SBeschreibung As String
	Public Property Reserve1 As String
	Public Property Taetigkeit As String
	Public Property Anforderung As String
	Public Property Reserve2 As String
	Public Property Reserve3 As String
	Public Property Ausbildung As String
	Public Property Weiterbildung As String
	Public Property SKennt As String
	Public Property EDVKennt As String
	Public Property CreatedOn As DateTime?
	Public Property CreatedFrom As String
	Public Property ChangedOn As DateTime?
	Public Property ChangedFrom As String
	Public Property Result As String
	Public Property Vak_Region As String
	Public Property Transfered_User As String
	Public Property Transfered_On As DateTime?
	Public Property Transfered_Guid As String
	Public Property Vak_Kanton As String
	Public Property Customer_Guid As String
	Public Property Bemerkung As String
	Public Property MDNr As Integer?
	Public Property JobPLZ As String
	Public Property UserKontakt As String
	Public Property UserEMail As String
	Public Property TitelForSearch As String
	Public Property ShortDescription As String

	Public Property VacancyNumberOffset As Integer


End Class


<Serializable()>
Public Class VacancyJobCHMasterData

	Public Property ID As Integer?
	Public Property VakNr As Integer?
	Public Property UserKontakt As String
	Public Property UserEMail As String
	Public Property TitelForSearch As String
	Public Property ShortDescription As String
	Public Property Beginn As String
	Public Property JobProzent As String
	Public Property Anstellung As String
	Public Property Dauer As String
	Public Property MASex As String
	Public Property Vak_Kanton As String
	Public Property MAAge As String
	Public Property IEExport As Boolean?
	Public Property Firma1 As String
	Public Property KDzNachname As String
	Public Property KDzVorname As String
	Public Property USVorname As String
	Public Property USNachname As String
	Public Property BranchenValue As Integer?
	Public Property BranchenBez As String
	Public Property Position_Value As Integer?
	Public Property Position As String
	Public Property Organisation_ID As Integer?
	Public Property Organisation_SubID As Integer?
	Public Property Inserat_ID As String
	Public Property Our_URL As String
	Public Property Direkt_URL As String
	Public Property Branche As String
	Public Property Vak_Sprache As String
	Public Property Layout_ID As Integer?
	Public Property Logo_ID As Integer?
	Public Property Bewerben_URL As String
	Public Property Angebot_Value As Integer?
	Public Property Xing_Poster_URL As String
	Public Property Xing_Company_Profile_URL As String
	Public Property Xing_Company_Is_Poc As Boolean?
	Public Property StartDate As DateTime?
	Public Property EndDate As DateTime?
	Public Property IsOnline As Boolean?
	Public Property CreatedOn As DateTime?
	Public Property CreatedFrom As String
	Public Property ChangedOn As DateTime?
	Public Property ChangedFrom As String
	Public Property USJCHSub_ID As Integer?
	Public Property USJCHLayout_ID As Integer?
	Public Property USJCHLogo_ID As Integer?
	Public Property USJCHOur_URL As String
	Public Property DaysToAdd As Integer?
	Public Property USJCHDirekt_URL As String
	Public Property USJCHXing_Poster_URL As String
	Public Property USJCHXing_Company_Profile_URL As String
	Public Property USJCHXing_Company_Is_Poc As Boolean?

End Class


<Serializable()>
Public Class VacancyJobCHBerufData
	Public Property ID As Integer?
	Public Property VakNr As Integer?
	Public Property BerufGruppe_Value As Integer?
	Public Property BerufGruppe As String
	Public Property Fachrichtung_Value As Integer?
	Public Property Fachrichtung As String
	Public Property Position_Value As Integer?
	Public Property Position As String
	Public Property ForExperience As Boolean?

End Class


<Serializable()>
Public Class VacancyStmpSettingData
	Public Property ID As Integer?
	Public Property VakNr As Integer?
	Public Property EducationCode As Integer?
	Public Property StartDate As DateTime?
	Public Property EndDate As DateTime?
	Public Property NumberOfJobs As Integer?
	Public Property IsOnline As Boolean?
	Public Property Less_One_Year As Boolean?
	Public Property More_One_Year As Boolean?
	Public Property More_Three_Years As Boolean?
	Public Property Sunday_and_Holidays As Boolean?
	Public Property Shift_Work As Boolean?
	Public Property Night_Work As Boolean?
	Public Property Home_Work As Boolean?
	Public Property ReportToAvam As Boolean?
	Public Property ShortEmployment As Boolean?
	Public Property Immediately As Boolean?
	Public Property Surrogate As Boolean?
	Public Property Permanent As Boolean?
	Public Property EuresDisplay As Boolean?
	Public Property PublicDisplay As Boolean?

	Public Property JobroomID As String
	Public Property ReportingObligation As Boolean?
	Public Property ReportingObligationEndDate As DateTime?
	Public Property ReportingDate As DateTime?
	Public Property ReportingFrom As String

End Class

<Serializable()>
Public Class VacancyJobCHLanguageData
	Public Property ID As Integer?
	Public Property VakNr As Integer?
	Public Property LanguageNiveau_Value As Integer?
	Public Property Bezeichnung_Value As Integer?
	Public Property LanguageNiveau As String
	Public Property Bezeichnung As String

	Public ReadOnly Property LanguageViewData As String
		Get
			Return String.Format(String.Format("{0}: {1}", Bezeichnung, LanguageNiveau))
		End Get
	End Property

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
Public Class VacancyInseratJobCHData

	Public Property Bezeichnung As String
	Public Property Vorspann As String
	Public Property Aufgabe As String
	Public Property Anforderung As String
	Public Property Wirbieten As String

	Public ReadOnly Property Vorschau As String
		Get
			Return String.Format("{0}<h2>{1}</h2>{2}<br>{3}<br>{4}", Vorspann, Bezeichnung, Aufgabe, Anforderung, Wirbieten)
		End Get
	End Property

End Class


'End Namespace