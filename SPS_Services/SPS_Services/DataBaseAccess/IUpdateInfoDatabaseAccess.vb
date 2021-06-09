
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.DataTransferObject.UpdateInfo.DataObjects

Namespace UpdateInfo


	''' <summary>
	''' Interface for Updateinfo database access.
	''' </summary>
	Public Interface IUpdateInfoDatabaseAccess


#Region "FTP Update"

		Function LoadUpdateInfoData(ByVal customerID As String) As IEnumerable(Of UpdateUtilitiesDTO)

		Function LoadFTPUpdateFileData(ByVal customerID As CustomerMDData) As IEnumerable(Of FTPUpdateFilesDTO)
		Function LoadFTPFileContentData(ByVal customerID As String, ByVal recID As Integer) As FTPUpdateFilesDTO
		Function LoadProgramModuleFilesData(ByVal customerData As CustomerMDData, ByVal moduleName As String) As IEnumerable(Of ModuleFilesDTO)
		Function LoadAssignedProgramModuleFileNameContentData(ByVal customerID As String, ByVal fileName As String) As ModuleFilesDTO
		Function LoadAssignedProgramModuleFileIDContentData(ByVal customerID As String, ByVal recID As Integer) As ModuleFilesDTO


#End Region

		Function AddNewUpdateFileForProgramModulesData(ByVal fileData As FTPUpdateFilesDTO) As Boolean

#Region "Station update"

#End Region

	End Interface


End Namespace
