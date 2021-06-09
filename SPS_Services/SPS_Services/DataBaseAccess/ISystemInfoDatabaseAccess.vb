
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.DataTransferObjects.TaxInfoService
Imports wsSPS_Services.SPUtilities

Namespace SystemInfo


	''' <summary>
	''' Interface for Systeminfo database access.
	''' </summary>
	Public Interface ISystemInfoDatabaseAccess

		Function LoadCustomerDataForApplicationList() As IEnumerable(Of MandantData)
		Function LoadPrividerData(ByVal customerID As String, ByVal providerName As String) As ProviderDataDTO
		Function LoadJobPlattformCustomerData(ByVal customerID As String, ByVal userGuid As String, ByVal PlattformCustomerNumber As Integer) As JobplattformsCustomerData
		Function LoadCustomerNotificationsData(ByVal userID As String, ByVal excludeChecked As Boolean?) As IEnumerable(Of CustomerNotificationDataDTO)

		Function AddCustomerPayableServiceUsage(ByVal customerGuid As String, ByVal userData As SystemUserData, ByVal serviceName As String, ByVal serviceArt As String) As Boolean
		Function UpdateAssignedNotificationData(ByVal customerID As String, ByVal recordID As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Function AddCustomerNotificationData(ByVal customerID As String, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean
		Function UpdateAssignedCustomerNotificatioContentData(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean
		Function UpdateAssignedCustomerNotificationData(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean


#Region "Services"

		Function LoadCustomerServices() As IEnumerable(Of CustomerSearchResultDTO)
		Function LoadCustomerDeniedServices(ByVal customerID As String) As List(Of String)
		Function LoadCustomerServicesAdvisor(ByVal customerID As String) As IEnumerable(Of CustomerUserNameSearchResultDTO)
		Function LoadCurrentServicesData(ByVal customerID As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As IEnumerable(Of PaymentSearchResultDTO)
		Function LoadNotificationsData(ByVal customerID As String, ByVal notifyArt As NotifyArtEnum, ByVal excludeChecked As Boolean?) As IEnumerable(Of NotifyMessageData)
		Function LoadTODONotificationsData(ByVal customerID As String) As IEnumerable(Of NotifyMessageData)
		Function LoadPaidDataServices(ByVal customerID As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As IEnumerable(Of PaidSearchResultDTO)
		Function LoadECallDataForAssignedJob(ByVal customerID As String, ByVal JobGuid As String) As IEnumerable(Of PaidSearchResultDTO)
		Function LoadAdvisorLoginData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of AdvisorLoginData)
		Function LoadAdvisorLoginMonthlyData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of AdvisorLoginData)
		Function UpdateECallResponseDataForAssignedJob(ByVal customerID As String, ByVal JobGuid As String, ByVal AuthorizedCredit As Decimal?, ByVal AuthorizedItems As Decimal?) As Boolean

#End Region


#Region "logins"

		Function AddSolvencyUsage(ByVal customerID As String, ByVal userGuid As String, ByVal userName As String, ByVal solvencyCheckType As String, ByVal serviceDate As DateTime) As Boolean
		Function AddECallUsage(ByVal customerID As String, ByVal userGuid As String, ByVal userName As String, ByVal SMSCredit As String, ByVal JobID As String, ByVal UsedPoints As String, ByVal serviceDate As DateTime) As Boolean
		Function AddSputnikLoginUsage(ByVal customerID As String, ByVal customerName As String, ByVal userGuid As String, ByVal userName As String, ByVal domainUsername As String, ByVal machineName As String, ByVal domainName As String) As Boolean
		Function AddSputnikUserData(ByVal customerID As String, ByVal userData As SystemUserData) As Boolean

		Function AllowedCustomerToUpdate(ByVal customerData As CustomerMDData) As Boolean
		Function AllowedStationToUpdate(ByVal stationData As StationData) As Boolean

#End Region

		Function SendNotificationForNewFileToSputnik(ByVal customerdata As CustomerMDData) As Boolean


#Region "nlog"
		Function AddUserNLOGNotificationData(ByVal customerID As String, ByVal nlogEntry As NLOGData, ByVal nLogMessage As String) As Boolean
#End Region


	End Interface


	Public Interface IPublicDatabaseAccess

		'Function LoadSBN2000GroupHeader_1Data(ByVal customerID As String, ByVal userID As String, ByVal groupHeaderNumber As Integer) As IEnumerable(Of SBN2000GroupHeaderData)
		'Function LoadSBN2000GroupHeader_2Data(ByVal customerID As String, ByVal userID As String, ByVal groupHeaderNumber As Integer, ByVal groupHeader_1 As Integer) As IEnumerable(Of SBN2000GroupHeaderData)
		'Function LoadSBN2000TitleData(ByVal customerID As String, ByVal userID As String, ByVal groupHeaderNumber As Integer?, ByVal groupHeader_1 As Integer?, ByVal groupHeader_2 As Integer?) As IEnumerable(Of SBN2000GroupHeaderData)
		'Function LoadSBN2000AllTitleData(ByVal customerID As String, ByVal userID As String) As IEnumerable(Of SBN2000GroupHeaderData)

		Function LoadJobCHOccupationsData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHFachbereichData(ByVal customerID As String, ByVal parentID As Integer, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHRegionData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHBranchesData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHLanguagesData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHLanguageNiveauData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHPositionData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadJobCHBildungNiveauData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)
		Function LoadAVAMEducationsuData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO)


		Function LoadCVLBaseTableData(ByVal customerID As String, ByVal TableKind As String, ByVal Language As String) As IEnumerable(Of CVLBaseDataDTO)
		Function LoadALKData(ByVal customerID As String) As IEnumerable(Of ALKResultDTO)
		Function LoadAssignedALKData(ByVal customerID As String, ByVal ALKNumber As Integer?) As ALKResultDTO

		Function LoadQualificationData(ByVal customerID As String, ByVal gender As String, ByVal language As String, ByVal qualificationModul As String) As IEnumerable(Of QualificationDTO)


#Region "tax info"

		Function LoadTaxInfoData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As IEnumerable(Of TaxDataItemDTO)
		Function LoadTaxCodeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TaxCodeDataDTO)
		Function LoadTaxChurchCodeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TaxChurchCodeDataDTO)
		Function LoadTaxNumberOfChildrenData(ByVal customerID As String, ByVal userID As String, ByVal language As String, ByVal canton As String, ByVal code As String, ByVal church As String) As IEnumerable(Of Integer)
		Function LoadQstData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal einkommen As Double, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As IEnumerable(Of QstDataDTO)
		Function LoadAllowedQstData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As QstDataAllowedDTO
		Function LoadCommunityData(ByVal customerID As String, ByVal userID As String, ByVal canton As String, ByVal language As String) As IEnumerable(Of CommunityDataDTO)

		Function LoadEmploymentTypeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of EmploymentTypeDataDTO)
		Function LoadOtherEmploymentTypeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of EmploymentTypeDataDTO)
		Function LoadTypeOfStayData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TypeOfStayDataDTO)

		Function LoadPermissionData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of PermissionDataDTO)
		Function LoadForeignCategoryData(ByVal customerID As String, ByVal userID As String, ByVal code As String, ByVal language As String) As IEnumerable(Of PermissionDataDTO)

#End Region



#Region "childern and educations data"

		Function LoadChildEducationData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As ChildEducationDataDTO

#End Region


#Region "Bank data"

		Function LoadBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As IEnumerable(Of BankSearchResultDTO)
		Function LoadAssignedBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankLocation As String) As BankSearchResultDTO

#End Region


#Region "Geo data"

		Function LoadGeoCoordinationData(ByVal customerID As String, ByVal countryCode As String, ByVal firstPostcode As String, ByVal secondPostcode As String) As IEnumerable(Of LocationGoordinateDataDTO)

#End Region

		Function LoadSTMPJobData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData)
		Function LoadSTMPJob2020Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData)
		Function LoadSTMPJob2021Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData)
		Function LoadSTMPMappingData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPMappingData)


#Region "saved form control data from user"

		Function AddUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String, ByVal fieldName As String, ByVal fieldData As String, ByVal createdFrom As String) As Boolean
		Function LoadAssignedUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As IEnumerable(Of UserFormControlTemplateDTO)

#End Region


	End Interface


End Namespace
