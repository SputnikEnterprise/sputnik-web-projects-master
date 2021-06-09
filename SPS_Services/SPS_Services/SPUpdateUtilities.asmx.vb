
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.PVLInfo
Imports wsSPS_Services.UpdateInfo
Imports wsSPS_Services.DataTransferObject.UpdateInfo.DataObjects
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports System.IO


' Wenn der Aufruf dieses Webdiensts aus einem Skript zulässig sein soll, heben Sie mithilfe von ASP.NET AJAX die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPUpdateUtilities.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPUpdateUtilities
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPUpdateUtilities"

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess
	Private m_PVLInfo As PVLDatabaseAccess
	Private m_UpdateInfo As UpdateInfoDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_PVLInfo = New PVLDatabaseAccess(My.Settings.ConnStr_PVLPublicInfo, Language.German)
		m_UpdateInfo = New UpdateInfoDatabaseAccess(My.Settings.ConnStr_SputnikUpdateSystem, Language.German)

	End Sub


	''' <summary>
	''' Loads update info data.
	''' </summary>
	<WebMethod()>
	Public Function GetUpdateInfo(ByVal customerID As String) As UpdateUtilitiesDTO()
		Dim result As List(Of UpdateUtilitiesDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of UpdateUtilitiesDTO)
			result = m_UpdateInfo.LoadUpdateInfoData(customerID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetUpdateInfo", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	'Dim connString As String = My.Settings.ConnStr_SputnikUpdateSystem
	'Dim conn As SqlConnection = New SqlConnection(connString)
	'Dim utility As New ClsUtilities
	'Dim strMessage As New StringBuilder()
	'Dim reader As SqlClient.SqlDataReader = Nothing

	'Dim listOfUpdateInfoDTO As List(Of UpdateUtilitiesDTO) = Nothing
	'Try

	'	Dim sql As String = String.Empty

	'	sql = "SELECT TOP 1 ID, UpdateFileName, UpdateFileDate, UpdateFileTime FROM [SPUpdates].Dbo.SpUpdates WHERE FileDestPath NOT IN ('Query\', 'Documents\', 'Templates\' ) AND AllowedToDownload = 1 ORDER BY id desc"

	'	' Create command.
	'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
	'	cmd.CommandType = CommandType.Text

	'	' Open connection to database.
	'	conn.Open()
	'	For i As Integer = 0 To cmd.Parameters.Count - 1
	'		strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
	'																		cmd.Parameters(i).ParameterName,
	'																		cmd.Parameters(i).DbType,
	'																		cmd.Parameters(i).Size,
	'																		cmd.Parameters(i).Value,
	'																		ControlChars.NewLine))
	'	Next

	'	' Execute the data reader.
	'	reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

	'	listOfUpdateInfoDTO = New List(Of UpdateUtilitiesDTO)

	'	' Read all data.
	'	While (reader.Read())
	'		Dim dto = New UpdateUtilitiesDTO

	'		dto.UpdateID = SafeGetInteger(reader, "ID", Nothing)
	'		dto.UpdateFilename = SafeGetString(reader, "UpdateFileName")
	'		dto.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)

	'		listOfUpdateInfoDTO.Add(dto)

	'	End While

	'Catch ex As Exception
	'	Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
	'	utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetUpdateInfo", .MessageContent = msgContent})

	'Finally
	'	If Not reader Is Nothing Then

	'		Try
	'			reader.Close()
	'		Catch
	'			' Do nothing
	'		End Try

	'	End If
	'End Try

	'If Not listOfUpdateInfoDTO Is Nothing Then

	'	' Return search data as an array.
	'	Return listOfUpdateInfoDTO.ToArray()
	'Else
	'	Return Nothing
	'End If


	''' <summary>
	''' Loads update info data.
	''' </summary>
	<WebMethod()>
	Public Function GetUpdateNotification() As UpdateUtilitiesDTO
		Dim result As UpdateUtilitiesDTO = Nothing
		m_customerID = String.Empty

		Try
			result = New UpdateUtilitiesDTO
			result = m_UpdateInfo.LoadUpdateInfoData(String.Empty)(0)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetUpdateNotification", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


	'Dim connString As String = My.Settings.ConnStr_SputnikUpdateSystem
	'	Dim conn As SqlConnection = New SqlConnection(connString)
	'	Dim reader As SqlClient.SqlDataReader = Nothing
	'	Dim utility As New ClsUtilities
	'	Dim strMessage As New StringBuilder()

	'	Dim listOfUpdateInfoDTO As UpdateUtilitiesDTO = Nothing
	'	Try

	'		Dim sql As String = String.Empty

	'		sql = "SELECT TOP 1 ID, UpdateFileName, UpdateFileDate, UpdateFileTime FROM [SPUpdates].Dbo.SpUpdates WHERE FileDestPath NOT IN ('Query\', 'Documents\', 'Templates\' ) AND AllowedToDownload = 1 ORDER BY id desc"

	'		' Create command.
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, conn)
	'		cmd.CommandType = CommandType.Text

	'		' Open connection to database.
	'		conn.Open()
	'		For i As Integer = 0 To cmd.Parameters.Count - 1
	'			strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
	'																			cmd.Parameters(i).ParameterName,
	'																			cmd.Parameters(i).DbType,
	'																			cmd.Parameters(i).Size,
	'																			cmd.Parameters(i).Value,
	'																			ControlChars.NewLine))
	'		Next

	'		' Execute the data reader.
	'		reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)


	'		' Read all data.
	'		If (Not reader Is Nothing AndAlso reader.Read()) Then
	'			listOfUpdateInfoDTO = New UpdateUtilitiesDTO

	'			listOfUpdateInfoDTO.UpdateID = SafeGetInteger(reader, "ID", Nothing)
	'			listOfUpdateInfoDTO.UpdateFilename = SafeGetString(reader, "UpdateFileName")
	'			listOfUpdateInfoDTO.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
	'			'listOfUpdateInfoDTO.UpdateFileTime = SafeGetDateTime(reader, "UpdateFileTime", Nothing)

	'		End If

	'	Catch ex As Exception
	'		Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
	'		utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = String.Empty,
	'																																.SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetUpdateNotification", .MessageContent = msgContent})
	'	Finally
	'		If Not reader Is Nothing Then

	'			Try
	'				reader.Close()
	'			Catch
	'				' Do nothing
	'			End Try

	'		End If
	'	End Try

	'	Return listOfUpdateInfoDTO

	'End Function


#Region "PVL-Version checking"

	''' <summary>
	''' Gets GAV version data.
	''' </summary>
	''' <returns>The GAV number.</returns>
	<WebMethod()>
	Public Function GetGAVVersionNotificationData(ByVal customerID As String, ByVal gavNumber As Integer) As GAVVersionDataDTO
		Dim result As GAVVersionDataDTO = Nothing
		m_customerID = customerID

		Try
			If gavNumber = 0 Then
				Throw New Exception("GetGAVVersionNotificationData: no gav number was defined!")
			End If
			result = New GAVVersionDataDTO
			result = m_PVLInfo.LoadGAVVersionChangingData(customerID, gavNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVVersionNotificationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()> <Obsolete("is obsolated! use GetGAVVersionNotificationData instead.")>
	Public Function GetGAVersionData(ByVal gavNumber As Integer) As GAVVersionDataDTO
		Dim result As GAVVersionDataDTO = Nothing
		m_customerID = String.Empty

		Try
			If gavNumber = 0 Then
				Throw New Exception("GetGAVersionData: no gav number was defined!")
			End If
			result = New GAVVersionDataDTO
			result = m_PVLInfo.LoadGAVVersionChangingData(String.Empty, gavNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVersionData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


#End Region


#Region "FTP-Upload"

	''' <summary>
	''' Loads update info data.
	''' </summary>
	''' <returns>Array of tax info.</returns>
	<WebMethod()>
	Public Function GetFTPUpdateFiles(ByVal customerData As CustomerMDData) As FTPUpdateFilesDTO()

		Dim result As List(Of FTPUpdateFilesDTO) = Nothing

		Try
			result = New List(Of FTPUpdateFilesDTO)
			result = m_UpdateInfo.LoadFTPUpdateFileData(customerData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFTPUpdateFiles", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <summary>
	''' Loads file content.
	''' </summary>
	''' <returns>Array of tax info.</returns>
	<WebMethod()>
	Public Function GetFTPUpdateFileContent(ByVal recID As Integer) As FTPUpdateFilesDTO
		Dim result As FTPUpdateFilesDTO = Nothing
		m_customerID = String.Empty

		Try
			result = New FTPUpdateFilesDTO
			result = m_UpdateInfo.LoadFTPFileContentData(m_customerID, recID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFTPUpdateFileContent", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Public Function GetProgramModuleFilesList(ByVal customerData As CustomerMDData, ByVal modulName As String) As ModuleFilesDTO()

		Dim result As List(Of ModuleFilesDTO) = Nothing
		m_customerID = String.Empty

		Try
			result = New List(Of ModuleFilesDTO)
			result = m_UpdateInfo.LoadProgramModuleFilesData(customerData, modulName)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetProgramModuleFilesList", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))

	End Function

	<WebMethod()>
	Public Function GetProgramModuleFileNameContent(ByVal modulName As String, ByVal fileName As String) As ModuleFilesDTO
		Dim result As ModuleFilesDTO = Nothing
		m_customerID = String.Empty

		Try
			result = New ModuleFilesDTO
			result = m_UpdateInfo.LoadAssignedProgramModuleFileNameContentData(modulName, fileName)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetProgramModuleFileNameContent", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Public Function GetProgramModuleFileIDContent(ByVal modulName As String, ByVal recID As Integer) As ModuleFilesDTO
		Dim result As ModuleFilesDTO = Nothing
		m_customerID = String.Empty

		Try
			result = New ModuleFilesDTO
			result = m_UpdateInfo.LoadAssignedProgramModuleFileIDContentData(modulName, recID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetProgramModuleFileIDContent", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="upload update files")>
	Function UploadNewUpdateFile(ByVal fileData As FTPUpdateFilesDTO) As Boolean

		Dim result As Boolean = True
		m_customerID = String.Empty

		' Read all data.
		If fileData.FileContent Is Nothing Then Return False
		Try

			result = result AndAlso CreateFileContentIntoFileSystem(fileData)

			result = result AndAlso m_UpdateInfo.AddNewUpdateFileForProgramModulesData(fileData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UploadNewUpdateFile", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="send a notification an sputnik")>
	Function SendNewUpdateFileNotificationToSputnik(ByVal customerData As CustomerMDData) As Boolean

		Dim result As Boolean = True
		m_customerID = String.Empty

		' Read all data.
		If customerData Is Nothing Then Return False
		Try

			'result = result AndAlso m_SysInfo.SendNotificationForNewFileToSputnik(customerData)
			Dim body As String = String.Format("<table><tr><strong>customerID:</strong> {0}<br></tr>", customerData.CustomerID)
			body &= String.Format("<tr><strong>ip-address:</strong> {0}<br></tr>", customerData.LocalIPAddress)
			body &= String.Format("<tr><strong>LocalHostName:</strong> {0}<br></tr>", customerData.LocalHostName)
			body &= String.Format("<tr><strong>LocalDomainName:</strong> {0}<br></tr>", customerData.LocalDomainName)
			body &= String.Format("<tr><strong>external-address:</strong> {0}<br></tr></table>", customerData.ExternalIPAddress)

			m_utility.SendNotificationMails("info@sputnik-it.com", "notification@sputnik-it.com", "program must be updated!", body, Nothing)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "SendNewUpdateFileNotificationToSputnik", .MessageContent = msgContent})
			result = False
		Finally
		End Try


		Return result
	End Function


	''' <summary>
	''' Loads update for assigned station allowed.
	''' </summary>
	''' <returns>Array of tax info.</returns>
	<WebMethod()>
	Public Function IsStationUpdateAllowed(ByVal stationData As StationData) As Boolean
		Dim result As Boolean = True
		m_customerID = String.Empty

		Try
			result = result AndAlso m_SysInfo.AllowedStationToUpdate(stationData)

			If Not result Then
				Dim body = String.Format("LocalIPAddress: {0}<br>LocalHostName: {1}<br>LocalUserName: {2}<br>LocalDomainName: {3}<br>ExternalIPAddress: {4}<br>Allowed: {5}",
																			 stationData.LocalIPAddress, stationData.LocalHostName, stationData.LocalUserName,
																			 stationData.LocalDomainName, stationData.ExternalIPAddress, result)

				m_utility.SendNotificationMails("info@sputnik-it.com", "notification@sputnik-it.com", "Station-Update was blocked!", body, Nothing)
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "IsStationUpdateAllowed", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	''' <summary>
	''' Loads update for assigned mandant allowed.
	''' </summary>
	''' <returns>Array of tax info.</returns>
	<WebMethod()>
	Public Function IsMandantUpdateAllowed(ByVal customerData As CustomerMDData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerData.CustomerID

		Try
			result = result AndAlso m_SysInfo.AllowedCustomerToUpdate(customerData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "IsMandantUpdateAllowed", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


#End Region


#Region "Utility Methods"


	Private Function CreateFileContentIntoFileSystem(ByVal fileData As FTPUpdateFilesDTO) As Boolean
		Dim success As Boolean = True
		Dim tempPath As String = String.Empty

		Try
			tempPath = System.Configuration.ConfigurationManager.AppSettings("TemporaryFS_Rath")
			If String.IsNullOrWhiteSpace(tempPath) Then tempPath = "C:\Temp"
			Dim bytes() = fileData.FileContent

			Dim tempFileName = Path.Combine(tempPath, fileData.UpdateFilename)

			success = success AndAlso (Not bytes Is Nothing) AndAlso m_utility.WriteFileBytes(tempFileName, bytes)

		Catch ex As Exception
			Dim msgContent = String.Format("{0} >>> {1}", tempPath, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CreateFileContentIntoFileSystem", .MessageContent = msgContent})
			success = False

		End Try


		Return success

	End Function



#End Region


End Class