
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects

Namespace PVLInfo


	''' <summary>
	''' Interface for PVLinfo database access.
	''' </summary>
	Public Interface IPVLDatabaseAccess

		Function LoadArchivePVLDatabases(ByVal customerID As String) As IEnumerable(Of GAVPVLArchiveDatabaseDTO)
		Function LoadCurrentMetaInfo(ByVal customerID As String, ByVal canton As String, ByVal contractNumber As Integer) As GAVNameResultDTO
		Function LoadGAVVersionChangingData(ByVal customerID As String, ByVal gavNumber As Integer) As GAVVersionDataDTO
		Function LoadPVLAddressData(ByVal customerID As String, ByVal gavnumber As Integer?, ByVal canton As String, ByVal gruppe0 As String, ByVal organ As String) As GAVAddressDataDTO


#Region "FL GAV"

		Function LoadFLGAVGruppe0Info(ByVal customerID As String, ByVal canton As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe0ResultDTO)
		Function LoadFLGAVGruppe1Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe1ResultDTO)
		Function LoadFLGAVGruppe2Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe2ResultDTO)
		Function LoadFLGAVGruppe3Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe3ResultDTO)
		Function LoadFLGAVTextInfo(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal language As String) As IEnumerable(Of FLGAVTextResultDTO)
		Function LoadFLGAVSalaryInfo(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal gavtext As String, ByVal language As String) As FLGAVSalaryResultDTO


#End Region

#Region "tempdata version"

		Function LoadAssignedAdvisorPublicationInfoData(ByVal customerID As String, ByVal userID As String) As IEnumerable(Of PVLPublicationViewDataDTO)
		Function UpdateAssignedAdvisorPublicationViewedData(ByVal customerID As String, ByVal userID As String, ByVal recID As Integer, ByVal ContractNumber As String, ByVal VersionNumber As Integer,
																	ByVal PublicationDate As DateTime?, ByVal Title As String, ByVal checked As Boolean?,
																	ByVal userData As String) As Boolean
#End Region


	End Interface

	''' <summary>
	''' Interface for Current PVLinfo database access.
	''' </summary>
	Public Interface ICurrentPVLDatabaseAccess

		Function LoadCurrentPVLInfo(ByVal customerID As String, ByVal canton As String, ByVal postcode As String, ByVal language As String) As IEnumerable(Of GAVNameResultDTO)
		Function LoadPVLCriteriasInfo(ByVal customerID As String, ByVal metaID As Integer, ByVal language As String) As IEnumerable(Of GAVCriteriasResultDTO)
		Function LoadPVLCriteriaValueInfo(ByVal customerID As String, ByVal criteriaID As Integer, ByVal language As String) As IEnumerable(Of GAVCriteriaValueResultDTO)
		Function LoadPVLTaxonomyInfo(ByVal customerID As String, ByVal metaID As Integer) As IEnumerable(Of GAVTaxonomyDTO)
		Function LoadPVLCalculationData(ByVal customerID As String, ByVal categoryValues As String) As GAVCalculationDTO
		Function LoadPVLCategoryValueWithBaseValueInfo(ByVal customerID As String, ByVal categoryID As Integer, ByVal baseCategoryValueID As Integer?, ByVal language As String) As IEnumerable(Of GAVCategoryValueDTO)
		Function LoadPVLCategoryInfo(ByVal customerID As String, ByVal metaID As Integer, ByVal language As String) As IEnumerable(Of GAVCategoryDTO)
		Function LoadPVLWarningData(ByVal customerID As String, ByVal gavNumber As Integer) As GAVNotificationDTO
		Function LoadPVLAnhang1Data(ByVal customerID As String) As IEnumerable(Of GAVNameResultDTO)






	End Interface

End Namespace
