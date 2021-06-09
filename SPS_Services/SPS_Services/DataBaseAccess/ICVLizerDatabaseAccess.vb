
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.DataTransferObject.Notify
Imports wsSPS_Services.JobPlatform.X28

Namespace CVLizer


	''' <summary>
	''' Interface for CVLizer database access.
	''' </summary>
	Public Interface ICVLizerDatabaseAccess


		''' <summary>
		''' profilmatcher
		''' </summary>
		Function LoadProfilmatcherQueryNotifications(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?) As IEnumerable(Of ProfilMatcherNotificationData)
		Function LoadProfilmatcherAssignedQueryNotification(ByVal customerID As String, ByVal ID As Integer) As ProfilMatcherNotificationData
		Function AddProfilmatcherQueryNotificationData(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?, ByVal tplName As String,
																									 ByVal totalRecord As Integer, ByVal notify As Boolean, ByVal createdFrom As String,
																									 ByVal pmQueryContent As String, ByVal pmJobResultContent As String) As Boolean
		Function DeleteProfilmatcherAssignedNotificationData(ByVal customerID As String, ByVal ID As Integer) As Boolean


		Function LoadALLCVLProfileData(ByVal assignedDate As DateTime?) As IEnumerable(Of CVLizerProfileDataDTO)
		Function LoadCVLDocumentData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As IEnumerable(Of DocumentViewDataDTO)



		Function LoadCVLCustomerData(ByVal customerID As String) As IEnumerable(Of CVLizerCustomerDataDTO)
		Function LoadAssignedCVLProfileData(ByVal customerID As String, ByVal profileID As Integer?, ByVal showAllData As Boolean?) As IEnumerable(Of CVLizerProfileDataDTO)
		Function LoadAssignedCVLPersonalData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer?) As CVLPersonalDataDTO
		Function LoadCVLWorkPhaseData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal workID As Integer) As IEnumerable(Of WorkPhaseViewDataDTO)
		Function LoadCVLEducationPhaseData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal educationID As Integer) As IEnumerable(Of EducationPhaseViewDataDTO)
		Function LoadCVLAdditionalInfoData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal addID As Integer) As AdditionalInfoViewDataDTO
		Function LoadCVLPublicationData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As IEnumerable(Of PublicationViewDataDTO)
		Function LoadApplicantDocumentsFromCVLData(ByVal customerID As String, ByVal cvlPrifleID As Integer, ByVal showForApplicant As Boolean) As IEnumerable(Of DocumentViewDataDTO)
		Function LoadCVLApplicantPictureData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO
		Function LoadAssignedDocumentData(ByVal customerID As String, ByVal id As Integer) As DocumentViewDataDTO

		Function LoadAssignedApplicationEMailData(ByVal customerID As String, ByVal applicationNumber As Integer, ByVal withAttachments As Boolean?) As EMailDataDTO

#Region "search Experiences"

		Function LoadCVLPostcodeCityData(ByVal customerID As String) As IEnumerable(Of PostcodeCityViewDataDTO)
		Function LoadCVLJobGroupsData(ByVal customerID As String) As IEnumerable(Of ExperiencesViewDataDTO)
		Function LoadCVLExperiencesData(ByVal customerID As String) As IEnumerable(Of ExperiencesViewDataDTO)
		Function LoadCVLLanguageData(ByVal customerID As String) As IEnumerable(Of LanguageViewDataDTO)

    Function LoadCVLSearchData(ByVal customerID As String, ByVal userID As String, ByVal postfachCityData As PostcodeCityViewDataDTO,
                               ByVal radius As Integer, ByVal jobTitelsData As List(Of ExperiencesViewDataDTO),
                               ByVal opAreaData As List(Of ExperiencesViewDataDTO), ByVal operationAreaJoin As JoinENum,
                               ByVal skillData As List(Of ExperiencesViewDataDTO), ByVal skillJoin As JoinENum,
                               ByVal languageData As List(Of LanguageViewDataDTO), ByVal languageJoin As JoinENum,
                               ByVal searchLabel As String, ByVal setNotification As Boolean) As IEnumerable(Of CVLSearchResultDataDTO)

    Function LoadUserCVLSearchHistoryData(ByVal customerID As String, ByVal userID As String) As IEnumerable(Of CVLSearchHistoryDataDTO)
		Function LoadAssignedCVLSearchHistoryResultData(ByVal customerID As String, ByVal searchID As Integer) As IEnumerable(Of CVLSearchResultDataDTO)
		Function UpdateAssignedCVLSearchHistoryNotifierData(ByVal customerID As String, ByVal searchID As Integer) As Boolean
		Function DeleteAssignedCVLSearchHistoryData(ByVal customerID As String, ByVal searchID As Integer) As Boolean

#End Region


	End Interface


End Namespace
