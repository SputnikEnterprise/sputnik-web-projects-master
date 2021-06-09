

Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects

Namespace JobPlatform.AVAM

	' AVAM vacancy database access.
	Partial Class AVAMDataDatabaseAccess


#Region "private consts"

		Private Const JOBROOM_USER As String = "sputnik"
		Private Const JOBROOM_PASSWORD As String = "54oy3hhn5x"
		Private Const JOBROOM_URI As String = "https://api.job-room.ch/jobAdvertisements/v1"
		Private Const JOBROOM_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const JOBROOM_SINGLE_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}"


		Private Const STAGING_JOBROOM_USER As String = "Sputnik"
		Private Const STAGING_JOBROOM_PASSWORD As String = "dqz5kkvzlt"
		Private Const STAGING_JOBROOM_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1"
		Private Const STAGING_JOBROOM_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const STAGING_JOBROOM_SINGLE_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}"

		Private Const CHILKAT_COMPONENT_CODE As String = "MFSPUT.CB1102020_Q8aw6Ch8jR3W"

#End Region

		Private m_SearchResultData As JobroomSearchResultData

		Private m_TransmittedSTMPid As String

		Private m_APIResponse As String
		Private m_ReportingObligation As Boolean?
		Private m_ReportingObligationEndDate As DateTime?
		Private m_ResultContent As String

		Private m_UserName As String
		Private m_Password As String
		Private m_JobroomURI As String
		Private m_JobroomAllRecordURI As String
		Private m_JobroomSingleRecordURI As String



#Region "Public Methods"

		'Public Function CreateAVAMAdvertisement(ByVal customerID As String, ByVal userID As String, ByVal asStaging As Boolean, ByVal vacancyData As sp_WebServiceUtility.VacancyMasterData,
		'																					ByVal vacancyJobCHData As sp_WebServiceUtility.VacancyInseratJobCHData, ByVal vacancyStmpData As sp_WebServiceUtility.VacancyStmpSettingData,
		'																					ByVal vacancyStmpLanguageData As List(Of sp_WebServiceUtility.VacancyJobCHLanguageData),
		'																					ByVal MDData As sp_WebServiceUtility.MandantData,
		'																					ByVal userData As AdvisorData, ByVal employerData As sp_WebServiceUtility.CustomerMasterData,
		'																					ByVal jobNumber As Integer?, ByVal language As String) As AVAMJobCreationData ' sp_WebServiceUtility.JobPlatform.SPAVAMJobCreationData
		'	Dim avamObj As New sp_WebServiceUtility.JobPlatform.SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
		'	Dim result As New AVAMJobCreationData With {.JobroomID = String.Empty, .State = False}

		'	m_customerID = customerID

		'	m_TransmittedSTMPid = String.Empty
		'	m_ReportingObligation = False

		'	m_UserName = JOBROOM_USER
		'	m_Password = JOBROOM_PASSWORD

		'	m_JobroomURI = JOBROOM_URI
		'	m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
		'	m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI

		'	If asStaging Then
		'		m_UserName = STAGING_JOBROOM_USER
		'		m_Password = STAGING_JOBROOM_PASSWORD

		'		m_JobroomURI = STAGING_JOBROOM_URI
		'		m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI
		'		m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI

		'	End If

		'	Try

		'		m_TransmittedSTMPid = String.Empty

		'		If vacancyData Is Nothing Then
		'			Return result
		'		End If

		'		If vacancyJobCHData Is Nothing Then
		'			Return result
		'		End If

		'		If vacancyStmpData Is Nothing Then
		'			Return result
		'		End If

		'		If MDData Is Nothing Then
		'			Return result
		'		End If

		'		If userData Is Nothing Then
		'			Return result
		'		End If

		'		If employerData Is Nothing Then
		'			Return result
		'		End If

		'		If vacancyStmpLanguageData Is Nothing Then
		'			Return result
		'		End If


		'		Dim msgContent = String.Format("library ist starting. >>> {0} | {1}", DBConnectionstring, language)
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CreateAVAMAdvertisement", .MessageContent = msgContent})

		'		Dim transferObj = New sp_WebServiceUtility.JobPlatform.VacancyUtilities
		'		Dim userObj As New sp_WebServiceUtility.SPAdvisorData With {.Firstname = userData.Firstname}
		'		userObj.Salutation = userData.Salutation
		'		userObj.Lastname = userData.Lastname
		'		userObj.UserGuid = userData.UserGuid
		'		userObj.UsereMail = userData.UsereMail
		'		userObj.MDCanton = userData.MDCanton
		'		userObj.UserLanguage = userData.UserLanguage
		'		userObj.UserMDTelefon = userData.UserMDTelefon
		'		userObj.UserMDeMail = userData.UserMDeMail


		'		avamObj = transferObj.AddAVAMAdvertisementToRAV(customerID, userID, asStaging, vacancyData, vacancyJobCHData, vacancyStmpData, vacancyStmpLanguageData, MDData, userObj, employerData, jobNumber, language)
		'		If avamObj Is Nothing OrElse String.IsNullOrWhiteSpace(avamObj.JobroomID) Then Return Nothing

		'		result.QueryContent = avamObj.QueryContent
		'		result.ResultContent = avamObj.ResultContent
		'		result.AVAMRecordState = avamObj.State
		'		result.JobroomID = avamObj.JobroomID
		'		result.State = Not String.IsNullOrWhiteSpace(avamObj.JobroomID)
		'		result.ReportingObligation = avamObj.ReportingObligation
		'		result.reportingObligationEndDate = avamObj.reportingObligationEndDate
		'		result.CreatedOn = avamObj.CreatedOn
		'		result.CreatedFrom = avamObj.CreatedFrom

		'		If Not avamObj.ErrorMessage Is Nothing Then
		'			result.ErrorMessage = New ErrorData With {.Detail = avamObj.ErrorMessage.Detail, .Content = avamObj.ErrorMessage.Content, .Status = avamObj.ErrorMessage.Status, .Message = avamObj.ErrorMessage.Message, .Title = avamObj.ErrorMessage.Title}
		'		End If


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CreateAVAMAdvertisement", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})

		'		m_Logger.LogError(msgContent)

		'	End Try

		'	Return result

		'End Function

		'Public Function CancelAssignedAdvertisementData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData, ByVal jobroomID As String, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM) As WebServiceResult
		'	Dim result As New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}

		'	m_customerID = customerID

		'	Try

		'		If userData Is Nothing OrElse userData.UserGuid Is Nothing Then
		'			Dim msgContent = String.Format("CancelAssignedAdvertisementData ist starting. >>> {0} | {1}", DBConnectionstring, jobroomID)
		'			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedAdvertisementData", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})

		'			m_Logger.LogWarning(msgContent)
		'		End If

		'		Dim avamObj = New sp_WebServiceUtility.JobPlatform.VacancyUtilities
		'		Dim userObj As New sp_WebServiceUtility.SPAdvisorData With {.Firstname = userData.Firstname}
		'		userObj.Salutation = userData.Salutation
		'		userObj.Lastname = userData.Lastname
		'		userObj.UserGuid = userData.UserGuid
		'		userObj.UsereMail = userData.UsereMail
		'		userObj.MDCanton = userData.MDCanton
		'		userObj.UserLanguage = userData.UserLanguage
		'		userObj.UserMDTelefon = userData.UserMDTelefon
		'		userObj.UserMDeMail = userData.UserMDeMail
		'		userObj.UserNumber = userData.UserNumber

		'		Dim reasonObj = New sp_WebServiceUtility.AVAMAdvertismentCancelReasonENUM
		'		Dim resultObj As New sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects.WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}
		'		reasonObj = reasonEnum

		'		resultObj = avamObj.CancelAssignedJobAdvertisementData(customerID, asStaging, userObj, jobroomID, reasonObj)
		'		result.JobResult = resultObj.JobResult
		'		result.JobResultMessage = resultObj.JobResultMessage

		'		Return result


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedAdvertisementData", .MessageContent = msgContent})
		'		result = Nothing

		'		m_Logger.LogError(msgContent)

		'	End Try

		'	Return result

		'End Function

		'Public Function LoadAssignedAdvertisementQueryResultData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData, ByVal jobroomID As String) As AVAMJobCreationData
		'	Dim resultObj As New sp_WebServiceUtility.JobPlatform.SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
		'	Dim result As New AVAMJobCreationData With {.JobroomID = String.Empty, .State = False}

		'	m_customerID = customerID

		'	Try

		'		If userData Is Nothing OrElse userData.UserGuid Is Nothing Then
		'			Dim msgContent = String.Format("LoadAssignedAdvertisementQueryResultData >>> {0} | {1}", DBConnectionstring, jobroomID)
		'			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAdvertisementQueryResultData", .MessageContent = msgContent})

		'			m_Logger.LogWarning(msgContent)
		'		End If

		'		Dim avamObj = New sp_WebServiceUtility.JobPlatform.VacancyUtilities
		'		Dim userObj As New sp_WebServiceUtility.SPAdvisorData With {.Firstname = userData.Firstname}
		'		userObj.Salutation = userData.Salutation
		'		userObj.Lastname = userData.Lastname
		'		userObj.UserGuid = userData.UserGuid
		'		userObj.UsereMail = userData.UsereMail
		'		userObj.MDCanton = userData.MDCanton
		'		userObj.UserLanguage = userData.UserLanguage
		'		userObj.UserMDTelefon = userData.UserMDTelefon
		'		userObj.UserMDeMail = userData.UserMDeMail
		'		userObj.UserNumber = userData.UserNumber

		'		m_Logger.LogInfo(String.Format("CustomerID: {0} | JobroomID: {1} | asStaging: {2} | UserFullname: {3}", customerID, jobroomID, asStaging, userData.UserFullname))


		'		resultObj = avamObj.LoadAssignedJobAdvertisementQueryResultData(customerID, asStaging, userObj, jobroomID)
		'		If resultObj Is Nothing OrElse String.IsNullOrWhiteSpace(resultObj.JobroomID) Then Return Nothing

		'		result.AVAMRecordState = resultObj.AVAMRecordState
		'		result.CreatedFrom = resultObj.CreatedFrom
		'		result.CreatedOn = resultObj.CreatedOn
		'		result.JobroomID = resultObj.JobroomID
		'		result.QueryContent = resultObj.QueryContent
		'		result.ReportingObligation = resultObj.ReportingObligation
		'		result.reportingObligationEndDate = resultObj.reportingObligationEndDate
		'		result.ResultContent = resultObj.ResultContent
		'		result.State = resultObj.State
		'		result.SyncDate = resultObj.SyncDate
		'		result.SyncFrom = resultObj.SyncFrom
		'		If Not resultObj.ErrorMessage Is Nothing Then
		'			result.ErrorMessage = New ErrorData With {.Detail = resultObj.ErrorMessage.Detail, .Content = resultObj.ErrorMessage.Content, .Status = resultObj.ErrorMessage.Status, .Message = resultObj.ErrorMessage.Message, .Title = resultObj.ErrorMessage.Title}
		'		End If


		'	Catch ex As Exception
		'		m_Logger.LogInfo(String.Format("Error: {4} | CustomerID: {0} | JobroomID: {1} | asStaging: {2} | UserFullname: {3}", customerID, jobroomID, asStaging, userData.UserFullname, ex.ToString))
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAdvertisementQueryResultData", .MessageContent = msgContent})
		'		result = Nothing

		'		m_Logger.LogError(msgContent)

		'	End Try

		'	Return result

		'End Function

		'Public Function LoadAdvertisementsQueryResultData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData) As Boolean
		'	Dim result As Boolean = True

		'	m_customerID = customerID

		'	Try

		'		If userData Is Nothing OrElse userData.UserGuid Is Nothing Then
		'			Dim msgContent = String.Format("LoadAdvertisementsQueryResultData ist starting. >>> {0}", DBConnectionstring)
		'			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvertisementsQueryResultData", .MessageContent = msgContent})

		'			m_Logger.LogWarning(msgContent)
		'		End If

		'		Dim avamObj = New sp_WebServiceUtility.JobPlatform.VacancyUtilities
		'		Dim userObj As New sp_WebServiceUtility.SPAdvisorData With {.Firstname = userData.Firstname}
		'		userObj.Salutation = userData.Salutation
		'		userObj.Lastname = userData.Lastname
		'		userObj.UserGuid = userData.UserGuid
		'		userObj.UsereMail = userData.UsereMail
		'		userObj.MDCanton = userData.MDCanton
		'		userObj.UserLanguage = userData.UserLanguage
		'		userObj.UserMDTelefon = userData.UserMDTelefon
		'		userObj.UserMDeMail = userData.UserMDeMail
		'		userObj.UserNumber = userData.UserNumber

		'		result = avamObj.LoadAllJobAdvertisement(customerID, asStaging, userObj)


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvertisementsQueryResultData", .MessageContent = msgContent})
		'		result = Nothing

		'		m_Logger.LogError(msgContent)

		'	End Try

		'	Return result

		'End Function




#End Region



		'Private Function MapObjectToVacancyMasterData(ByVal vacancyData As VacancyMasterData)
		'	Dim result = New sp_WebServiceUtility.VacancyMasterData

		'	result.Anforderung = vacancyData.Anforderung
		'	result.Anstellung = vacancyData.Anstellung
		'	result.Ausbildung = vacancyData.Ausbildung
		'	result.Beginn = vacancyData.Beginn
		'	result.Bemerkung = vacancyData.Bemerkung
		'	result.Berater = vacancyData.Berater
		'	result.Bezeichnung = vacancyData.Bezeichnung
		'	result.ChangedFrom = vacancyData.ChangedFrom
		'	result.ChangedOn = vacancyData.ChangedOn
		'	result.CreatedFrom = vacancyData.CreatedFrom
		'	result.CreatedOn = vacancyData.CreatedOn
		'	result.Customer_Guid = vacancyData.Customer_Guid
		'	result.Dauer = vacancyData.Dauer
		'	result.EDVKennt = vacancyData.EDVKennt
		'	result.ExistLink = vacancyData.ExistLink
		'	result.Filiale = vacancyData.Filiale
		'	result.Gruppe = vacancyData.Gruppe
		'	result.ID = vacancyData.ID
		'	result.IEExport = vacancyData.IEExport
		'	result.JobOrt = vacancyData.JobOrt
		'	result.JobPLZ = vacancyData.JobPLZ
		'	result.JobProzent = vacancyData.JobProzent
		'	result.Jobtime = vacancyData.Jobtime
		'	result.KDBeschreibung = vacancyData.KDBeschreibung
		'	result.KDBietet = vacancyData.KDBietet
		'	result.KDNr = vacancyData.KDNr
		'	result.KDZHDNr = vacancyData.KDZHDNr
		'	result.MAAge = vacancyData.MAAge
		'	result.MAAuto = vacancyData.MAAuto
		'	result.MAFSchein = vacancyData.MAFSchein
		'	result.MALohn = vacancyData.MALohn
		'	result.MANationality = vacancyData.MANationality
		'	result.MASex = vacancyData.MASex
		'	result.MAZivil = vacancyData.MAZivil
		'	result.MDNr = vacancyData.MDNr
		'	result.Reserve1 = vacancyData.Reserve1
		'	result.Reserve2 = vacancyData.Reserve2
		'	result.Reserve3 = vacancyData.Reserve3
		'	result.SBeschreibung = vacancyData.SBeschreibung
		'	result.SBNNumber = vacancyData.SBNNumber
		'	result.SBNPublicationDate = vacancyData.SBNPublicationDate
		'	result.Result = vacancyData.Result
		'	result.SBNPublicationFrom = vacancyData.SBNPublicationFrom
		'	result.SBNPublicationState = vacancyData.SBNPublicationState
		'	result.ShortDescription = vacancyData.ShortDescription
		'	result.SKennt = vacancyData.SKennt
		'	result.Slogan = vacancyData.Slogan
		'	result.SubGroup = vacancyData.SubGroup
		'	result.Taetigkeit = vacancyData.Taetigkeit
		'	result.TitelForSearch = vacancyData.TitelForSearch
		'	result.Transfered_Guid = vacancyData.Transfered_Guid
		'	result.Transfered_On = vacancyData.Transfered_On
		'	result.Transfered_User = vacancyData.Transfered_User
		'	result.UserEMail = vacancyData.UserEMail
		'	result.UserKontakt = vacancyData.UserKontakt
		'	result.VacancyNumberOffset = vacancyData.VacancyNumberOffset
		'	result.VakKontakt = vacancyData.VakKontakt
		'	result.VakKontakt_Value = vacancyData.VakKontakt_Value
		'	result.VakLink = vacancyData.VakLink
		'	result.VakNr = vacancyData.VakNr
		'	result.VakState = vacancyData.VakState
		'	result.VakState_Value = vacancyData.VakState_Value
		'	result.Vak_Kanton = vacancyData.Vak_Kanton
		'	result.Vak_Region = vacancyData.Vak_Region
		'	result.Weiterbildung = vacancyData.Weiterbildung


		'	Return result

		'End Function

		'Private Function MapObjectToVacancyInseratJobCHData(ByVal vacancyData As VacancyInseratJobCHData)
		'	Dim result = New sp_WebServiceUtility.VacancyInseratJobCHData

		'	result.Anforderung = vacancyData.Anforderung
		'	result.Aufgabe = vacancyData.Aufgabe
		'	result.Bezeichnung = vacancyData.Bezeichnung
		'	result.Vorspann = vacancyData.Vorspann
		'	result.Wirbieten = vacancyData.Wirbieten


		'	Return result

		'End Function

		'Private Function MapObjectToVacancyStmpSettingData(ByVal vacancyData As VacancyStmpSettingData)
		'	Dim result = New sp_WebServiceUtility.VacancyStmpSettingData

		'	result.EducationCode = vacancyData.EducationCode
		'	result.EndDate = vacancyData.EndDate
		'	result.EuresDisplay = vacancyData.EuresDisplay
		'	result.Home_Work = vacancyData.Home_Work
		'	result.ID = vacancyData.ID
		'	result.Immediately = vacancyData.Immediately
		'	result.IsOnline = vacancyData.IsOnline
		'	result.JobroomID = vacancyData.JobroomID
		'	result.Less_One_Year = vacancyData.Less_One_Year
		'	result.More_One_Year = vacancyData.More_One_Year
		'	result.More_Three_Years = vacancyData.More_Three_Years
		'	result.Night_Work = vacancyData.Night_Work
		'	result.Permanent = vacancyData.Permanent
		'	result.PublicDisplay = vacancyData.PublicDisplay
		'	result.ReportingDate = vacancyData.ReportingDate
		'	result.ReportingFrom = vacancyData.ReportingFrom
		'	result.ReportingObligation = vacancyData.ReportingObligation
		'	result.ReportingObligationEndDate = vacancyData.ReportingObligationEndDate
		'	result.ReportToAvam = vacancyData.ReportToAvam
		'	result.Shift_Work = vacancyData.Shift_Work
		'	result.ShortEmployment = vacancyData.ShortEmployment
		'	result.StartDate = vacancyData.StartDate
		'	result.Sunday_and_Holidays = vacancyData.Sunday_and_Holidays
		'	result.Surrogate = vacancyData.Surrogate
		'	result.VakNr = vacancyData.VakNr


		'	Return result

		'End Function

		'Private Function MapObjectToMandantData(ByVal vacancyData As MandantData)
		'	Dim result = New sp_WebServiceUtility.MandantData

		'	result.CustomerID = vacancyData.CustomerID
		'	result.EMail = vacancyData.EMail
		'	result.Homepage = vacancyData.Homepage
		'	result.ID = vacancyData.ID
		'	result.Location = vacancyData.Location
		'	result.MandantCanton = vacancyData.MandantCanton
		'	result.MandantDbConnection = vacancyData.MandantDbConnection
		'	result.MandantName1 = vacancyData.MandantName1
		'	result.MandantName2 = vacancyData.MandantName2
		'	result.MandantNumber = vacancyData.MandantNumber
		'	result.Postcode = vacancyData.Postcode
		'	result.Street = vacancyData.Street
		'	result.Telefax = vacancyData.Telefax
		'	result.Telephon = vacancyData.Telephon


		'	Return result

		'End Function

		'Private Function MapObjectToAdvisorData(ByVal vacancyData As AdvisorData)
		'	Dim result = New sp_WebServiceUtility.SPAdvisorData

		'	result.Firstname = vacancyData.Firstname
		'	result.KST = vacancyData.KST
		'	result.KST1 = vacancyData.KST1
		'	result.KST2 = vacancyData.KST2
		'	result.Lastname = vacancyData.Lastname
		'	result.MDCanton = vacancyData.MDCanton
		'	result.Salutation = vacancyData.Salutation
		'	result.UserBusinessBranch = vacancyData.UserBusinessBranch
		'	result.UsereMail = vacancyData.UsereMail
		'	result.UserFiliale = vacancyData.UserFiliale
		'	result.UserFTitel = vacancyData.UserFTitel
		'	result.UserGuid = vacancyData.UserGuid
		'	result.UserLanguage = vacancyData.UserLanguage
		'	result.UserLoginname = vacancyData.UserLoginname
		'	result.UserLoginPassword = vacancyData.UserLoginPassword
		'	result.UserMDDTelefon = vacancyData.UserMDDTelefon
		'	result.UserMDeMail = vacancyData.UserMDeMail
		'	result.UserMDGuid = vacancyData.UserMDGuid
		'	result.UserMDHomepage = vacancyData.UserMDHomepage
		'	result.UserMDLand = vacancyData.UserMDLand
		'	result.UserMDName = vacancyData.UserMDName
		'	result.UserMDName2 = vacancyData.UserMDName2
		'	result.UserMDName3 = vacancyData.UserMDName3
		'	result.UserMDNr = vacancyData.UserMDNr
		'	result.UserMDOrt = vacancyData.UserMDOrt
		'	result.UserMDPLZ = vacancyData.UserMDPLZ
		'	result.UserMDPostfach = vacancyData.UserMDPostfach
		'	result.UserMDStrasse = vacancyData.UserMDStrasse
		'	result.UserMDTelefax = vacancyData.UserMDTelefax
		'	result.UserMDTelefon = vacancyData.UserMDTelefon
		'	result.UserMobile = vacancyData.UserMobile
		'	result.UserNumber = vacancyData.UserNumber
		'	result.UserSTitel = vacancyData.UserSTitel
		'	result.UserTelefax = vacancyData.UserTelefax
		'	result.UserTelefon = vacancyData.UserTelefon


		'	Return result

		'End Function

		'Private Function MapObjectToCustomerMasterData(ByVal vacancyData As CustomerMasterData)
		'	Dim result = New sp_WebServiceUtility.CustomerMasterData

		'	result.BillTypeCode = vacancyData.BillTypeCode
		'	result.CanteenAvailable = vacancyData.CanteenAvailable
		'	result.ChangedFrom = vacancyData.ChangedFrom
		'	result.ChangedOn = vacancyData.ChangedOn
		'	result.Comment = vacancyData.Comment
		'	result.Company1 = vacancyData.Company1
		'	result.Company2 = vacancyData.Company2
		'	result.Company3 = vacancyData.Company3
		'	result.CountryCode = vacancyData.CountryCode
		'	result.CreatedFrom = vacancyData.CreatedFrom
		'	result.CreatedOn = vacancyData.CreatedOn
		'	result.CreditLimit1 = vacancyData.CreditLimit1
		'	result.CreditLimit2 = vacancyData.CreditLimit2
		'	result.CreditLimitsFromDate = vacancyData.CreditLimitsFromDate
		'	result.CreditLimitsToDate = vacancyData.CreditLimitsToDate
		'	result.CreditWarning = vacancyData.CreditWarning
		'	result.CurrencyCode = vacancyData.CurrencyCode
		'	result.CustomerMandantNumber = vacancyData.CustomerMandantNumber
		'	result.CustomerNumber = vacancyData.CustomerNumber
		'	result.CustomerState1 = vacancyData.CustomerState1
		'	result.CustomerState2 = vacancyData.CustomerState2
		'	result.DoNotShowContractInWOS = vacancyData.DoNotShowContractInWOS
		'	result.EMail = vacancyData.EMail
		'	result.Email_Mailing = vacancyData.Email_Mailing
		'	result.facebook = vacancyData.facebook
		'	result.FirstProperty = vacancyData.FirstProperty
		'	result.Hompage = vacancyData.Hompage
		'	result.HowContact = vacancyData.HowContact
		'	result.InvoiceOption = vacancyData.InvoiceOption
		'	result.KD_UmsMin = vacancyData.KD_UmsMin
		'	result.KST = vacancyData.KST
		'	result.Language = vacancyData.Language
		'	result.Latitude = vacancyData.Latitude
		'	result.Location = vacancyData.Location
		'	result.Longitude = vacancyData.Longitude
		'	result.mwstpflicht = vacancyData.mwstpflicht
		'	result.NotPrintReports = vacancyData.NotPrintReports
		'	result.NoUse = vacancyData.NoUse
		'	result.NoUseComment = vacancyData.NoUseComment
		'	result.NumberOfCopies = vacancyData.NumberOfCopies
		'	result.NumberOfEmployees = vacancyData.NumberOfEmployees
		'	result.OpenInvoiceAmount = vacancyData.OpenInvoiceAmount
		'	result.OPShipment = vacancyData.OPShipment
		'	result.Postcode = vacancyData.Postcode
		'	result.PostOfficeBox = vacancyData.PostOfficeBox
		'	result.ReferenceNumber = vacancyData.ReferenceNumber
		'	result.Reserve1 = vacancyData.Reserve1
		'	result.Reserve2 = vacancyData.Reserve2
		'	result.Reserve3 = vacancyData.Reserve3
		'	result.Reserve4 = vacancyData.Reserve4
		'	result.SalaryPerHour = vacancyData.SalaryPerHour
		'	result.SalaryPerMonth = vacancyData.SalaryPerMonth
		'	result.sendToWOS = vacancyData.sendToWOS
		'	result.ShowHoursInNormal = vacancyData.ShowHoursInNormal
		'	result.SolvencyDecisionID = vacancyData.SolvencyDecisionID
		'	result.SolvencyInfo = vacancyData.SolvencyInfo
		'	result.Street = vacancyData.Street
		'	result.Telefax = vacancyData.Telefax
		'	result.Telefax_Mailing = vacancyData.Telefax_Mailing
		'	result.Telephone = vacancyData.Telephone
		'	result.TermsAndConditions_WOS = vacancyData.TermsAndConditions_WOS
		'	result.Transfered_Guid = vacancyData.Transfered_Guid
		'	result.TransportationOptions = vacancyData.TransportationOptions
		'	result.ValueAddedTaxNumber = vacancyData.ValueAddedTaxNumber
		'	result.WOSGuid = vacancyData.WOSGuid
		'	result.xing = vacancyData.xing


		'	Return result

		'End Function

		'Private Function MapObjectToVacancyJobCHLanguageData(ByVal vacancyData As List(Of VacancyJobCHLanguageData)) As List(Of sp_WebServiceUtility.VacancyJobCHLanguageData)
		'	Dim result = New List(Of sp_WebServiceUtility.VacancyJobCHLanguageData)

		'	For Each lang In vacancyData
		'		Dim data As New sp_WebServiceUtility.VacancyJobCHLanguageData

		'		data.Bezeichnung = lang.Bezeichnung
		'		data.Bezeichnung_Value = lang.Bezeichnung_Value
		'		data.ID = lang.ID
		'		data.LanguageNiveau = lang.LanguageNiveau
		'		data.LanguageNiveau_Value = lang.LanguageNiveau_Value
		'		data.VakNr = lang.VakNr

		'		result.Add(data)

		'	Next


		'	Return result

		'End Function


	End Class

End Namespace