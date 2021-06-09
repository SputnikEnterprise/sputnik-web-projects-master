
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.DataTransferObject.UpdateInfo.DataObjects


Namespace UpdateInfo


	Partial Class UpdateInfoDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IUpdateInfoDatabaseAccess


		Function LoadUpdateInfoData(ByVal customerID As String) As IEnumerable(Of UpdateUtilitiesDTO) Implements IUpdateInfoDatabaseAccess.LoadUpdateInfoData
			Dim listOfSearchResultDTO As List(Of UpdateUtilitiesDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "SELECT TOP 1 ID, UpdateFileName, UpdateFileDate, UpdateFileTime FROM [SPUpdates].Dbo.SpUpdates WHERE FileDestPath NOT IN ('Query\', 'Documents\', 'Templates\' ) AND AllowedToDownload = 1 ORDER BY id desc"


			Dim reader = OpenReader(sql, Nothing, CommandType.Text)
			listOfSearchResultDTO = New List(Of UpdateUtilitiesDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New UpdateUtilitiesDTO

						data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
						data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
						data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)

						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadUpdateInfoData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function LoadFTPUpdateFileData(ByVal customerData As CustomerMDData) As IEnumerable(Of FTPUpdateFilesDTO) Implements IUpdateInfoDatabaseAccess.LoadFTPUpdateFileData
			Dim listOfSearchResultDTO As List(Of FTPUpdateFilesDTO) = Nothing
			m_customerID = customerData.CustomerID

			Dim sql As String
			sql = "SELECT ID, UpdateFileName, FileDestPath, UpdateFileVersion, UpdateFileDate, UpdateFileTime, UpdateFileSize, File_Guid "
			sql &= "FROM [SPUpdates].Dbo.SpUpdates WHERE AllowedToDownload = 1 And FileContent Is Not Null ORDER BY id desc"


			Dim reader = OpenReader(sql, Nothing, CommandType.Text)
			listOfSearchResultDTO = New List(Of FTPUpdateFilesDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FTPUpdateFilesDTO

						data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
						data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
						data.FileDestPath = SafeGetString(reader, "FileDestPath", String.Empty)
						data.FileDestVersion = SafeGetString(reader, "UpdateFileVersion", String.Empty)
						data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
						data.UpdateFileTime = SafeGetString(reader, "UpdateFileTime", String.Empty)
						data.UpdateFileSize = SafeGetDecimal(reader, "UpdateFileSize", 0)
						data.File_Guid = SafeGetString(reader, "File_Guid")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				Dim msgContent = String.Format("LocalIPAddress: {0} | LocalHostName: {1} | LocalDomainName: {2} | ExternalIPAddress: {3}",
																			 customerData.LocalIPAddress, customerData.LocalHostName, customerData.LocalDomainName, customerData.ExternalIPAddress)
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "UpdateInfo.UpdateInfoDatabaseAccess.LoadFTPUpdateFileData",
																.NotifyComments = msgContent, .NotifyArt = SPUtilities.NotifyArtEnum.FTPUPDATE, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFTPUpdateFileData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function LoadFTPFileContentData(ByVal customerID As String, ByVal recID As Integer) As FTPUpdateFilesDTO Implements IUpdateInfoDatabaseAccess.LoadFTPFileContentData
			Dim listOfSearchResultDTO As FTPUpdateFilesDTO = Nothing
			m_customerID = customerID

			Dim sql As String
			sql = "SELECT ID, UpdateFileName, FileDestPath, UpdateFileVersion, UpdateFileDate, UpdateFileTime, UpdateFileSize, File_Guid, FileContent "
			sql &= "FROM [SPUpdates].Dbo.SpUpdates "
			sql &= " WHERE AllowedToDownload = 1 And ID = @ID ORDER BY id Desc"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(recID, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)
			listOfSearchResultDTO = New FTPUpdateFilesDTO

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New FTPUpdateFilesDTO

					data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
					data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
					data.FileDestPath = SafeGetString(reader, "FileDestPath", String.Empty)
					data.FileDestVersion = SafeGetString(reader, "UpdateFileVersion", String.Empty)
					data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
					data.UpdateFileTime = SafeGetString(reader, "UpdateFileTime", String.Empty)
					data.UpdateFileSize = SafeGetDecimal(reader, "UpdateFileSize", 0)
					data.File_Guid = SafeGetString(reader, "File_Guid")
					data.FileContent = SafeGetByteArray(reader, "FileContent")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFTPFileContentData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function LoadProgramModuleFilesData(ByVal customerData As CustomerMDData, ByVal moduleName As String) As IEnumerable(Of ModuleFilesDTO) Implements IUpdateInfoDatabaseAccess.LoadProgramModuleFilesData
			Dim listOfSearchResultDTO As List(Of ModuleFilesDTO) = Nothing
			m_customerID = customerData.CustomerID

			Dim sql As String
			sql = "SELECT ID, UpdateFileName, ModuleName, UpdateFileVersion, UpdateFileDate, UpdateFileTime, UpdateFileSize, File_Guid "
			sql &= "FROM [SPUpdates].Dbo.tbl_ProgramModuleFiles WHERE ModuleName = @moduleName And FileContent Is Not Null ORDER BY id desc"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("moduleName", ReplaceMissing(moduleName, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)
			listOfSearchResultDTO = New List(Of ModuleFilesDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New ModuleFilesDTO

						data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
						data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
						data.ModuleName = SafeGetString(reader, "ModuleName", String.Empty)
						data.FileDestVersion = SafeGetString(reader, "UpdateFileVersion", String.Empty)
						data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
						data.UpdateFileTime = SafeGetString(reader, "UpdateFileTime", String.Empty)
						data.UpdateFileSize = SafeGetDecimal(reader, "UpdateFileSize", 0)
						data.File_Guid = SafeGetString(reader, "File_Guid")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadProgModuleFilesData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function LoadAssignedProgramModuleFileNameContentData(ByVal moduleName As String, ByVal fileName As String) As ModuleFilesDTO Implements IUpdateInfoDatabaseAccess.LoadAssignedProgramModuleFileNameContentData
			Dim listOfSearchResultDTO As ModuleFilesDTO = Nothing
			m_customerID = String.Empty

			Dim sql As String
			sql = "SELECT ID, UpdateFileName, ModuleName, UpdateFileVersion, UpdateFileDate, UpdateFileTime, UpdateFileSize, File_Guid, FileContent "
			sql &= "FROM [SPUpdates].Dbo.tbl_ProgramModuleFiles "
			sql &= " WHERE ModulName = @modulName And UpdateFileName = @UpdateFileName"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("moduleName", ReplaceMissing(moduleName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("UpdateFileName", ReplaceMissing(fileName, String.Empty)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)
			listOfSearchResultDTO = New ModuleFilesDTO

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ModuleFilesDTO

					data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
					data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
					data.ModuleName = SafeGetString(reader, "ModuleName", String.Empty)
					data.FileDestVersion = SafeGetString(reader, "UpdateFileVersion", String.Empty)
					data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
					data.UpdateFileTime = SafeGetString(reader, "UpdateFileTime", String.Empty)
					data.UpdateFileSize = SafeGetDecimal(reader, "UpdateFileSize", 0)
					data.File_Guid = SafeGetString(reader, "File_Guid")
					data.FileContent = SafeGetByteArray(reader, "FileContent")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedProgramModuleFileNameContentData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function LoadAssignedProgramModuleFileIDContentData(ByVal customerID As String, ByVal recID As Integer) As ModuleFilesDTO Implements IUpdateInfoDatabaseAccess.LoadAssignedProgramModuleFileIDContentData
			Dim listOfSearchResultDTO As ModuleFilesDTO = Nothing
			m_customerID = customerID

			Dim sql As String
			sql = "SELECT ID, UpdateFileName, ModuleName, UpdateFileVersion, UpdateFileDate, UpdateFileTime, UpdateFileSize, File_Guid, FileContent "
			sql &= "FROM [SPUpdates].Dbo.tbl_ProgramModuleFiles "
			sql &= " WHERE ID = @recID"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)
			listOfSearchResultDTO = New ModuleFilesDTO

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ModuleFilesDTO

					data.UpdateID = SafeGetInteger(reader, "ID", Nothing)
					data.UpdateFilename = SafeGetString(reader, "UpdateFileName", String.Empty)
					data.ModuleName = SafeGetString(reader, "ModuleName", String.Empty)
					data.FileDestVersion = SafeGetString(reader, "UpdateFileVersion", String.Empty)
					data.UpdateFileDate = SafeGetDateTime(reader, "UpdateFileDate", Nothing)
					data.UpdateFileTime = SafeGetString(reader, "UpdateFileTime", String.Empty)
					data.UpdateFileSize = SafeGetDecimal(reader, "UpdateFileSize", 0)
					data.File_Guid = SafeGetString(reader, "File_Guid")
					data.FileContent = SafeGetByteArray(reader, "FileContent")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedProgramModuleFileIDContentData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function

		Function AddNewUpdateFileForProgramModulesData(ByVal fileData As FTPUpdateFilesDTO) As Boolean Implements IUpdateInfoDatabaseAccess.AddNewUpdateFileForProgramModulesData
			Dim success As Boolean = True
			m_customerID = String.Empty

			Try

				Dim sql As String
				If fileData.FileDestPath = "FTP" OrElse fileData.FileDestPath = "Client" Then
					sql = "[Create New UpdateFile Into Program Modules Table]"
				Else
					sql = "[Create New UpdateFile Into Update Table]"
				End If

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileName", ReplaceMissing(fileData.UpdateFilename, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileSource", String.Empty))
				listOfParams.Add(New SqlClient.SqlParameter("ModuleName", ReplaceMissing(fileData.FileDestPath, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileVersion", ReplaceMissing(fileData.FileDestVersion, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileDate", ReplaceMissing(fileData.UpdateFileDate, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileTime", ReplaceMissing(fileData.UpdateFileTime, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UpdateFileSize", ReplaceMissing(fileData.UpdateFileSize, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("File_Guid", ReplaceMissing(fileData.File_Guid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("FileContent", ReplaceMissing(fileData.FileContent, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddNewUpdateFileForProgramModulesData", .MessageContent = msgContent})
			Finally

			End Try

			Return success
		End Function

	End Class


End Namespace
