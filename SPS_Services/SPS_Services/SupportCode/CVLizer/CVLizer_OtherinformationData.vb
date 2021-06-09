
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess


#Region "other information data"

		Private Function LoadCVLAdditionalInformationData(ByVal profileID As Integer, ByVal additionalID As Integer) As OtherInformationDataDTO
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As OtherInformationDataDTO = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Information Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New OtherInformationDataDTO
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New OtherInformationDataDTO With {
											.ID = utility.SafeGetInteger(reader, "ID", 0),
											.MilitaryService = utility.SafeGetBoolean(reader, "MilitaryService", False),
											.Competences = utility.SafeGetString(reader, "Competences"),
											.Additionals = utility.SafeGetString(reader, "Additionals"),
											.Interests = utility.SafeGetString(reader, "Interests")
									}

					' load prpoerties
					dto.Languages = LoadCVLAdditionalLanguageData(dto.ID)
					dto.DrivingLicence = LoadCVLAdditionalDrivingLicenceData(dto.ID)
					dto.UndatedSkill = LoadCVLAdditionalUndatedSkillData(dto.ID)
					dto.UndatedOperationArea = LoadCVLAdditionalUndatedOperationAreaData(dto.ID)
					dto.UndatedIndustry = LoadCVLAdditionalUndatedIndustryData(dto.ID)
					dto.InternetRosources = LoadCVLAdditionalInternetresourceData(dto.ID)


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInformationData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalLanguageData(ByVal additionalID As Integer) As List(Of LanguageData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of LanguageData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Language Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of LanguageData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New LanguageData
					'With {
					'						. = utility.SafeGetInteger(reader, "ID", 0),
					'						.Code = utility.SafeGetString(reader, "FK_LanguageCode"),
					'						.Level = utility.SafeGetString(reader, "FK_LanguageLevelCode")
					'				}


					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalLanguageData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalDrivingLicenceData(ByVal additionalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional DrivingLicence Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of PropertyListData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New PropertyListData With {
											.ID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetString(reader, "FK_AddID"),
											.PropertyName = utility.SafeGetString(reader, "DrivingLicence")
									}


					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalDrivingLicenceData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalUndatedSkillData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated Skill Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = utility.SafeGetString(reader, "Code"),
											.Name = utility.SafeGetString(reader, "Name"),
											.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedSkillData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalUndatedOperationAreaData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated OperationArea Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = utility.SafeGetString(reader, "Code"),
											.Name = utility.SafeGetString(reader, "Name"),
											.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedOperationAreaData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalUndatedIndustryData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated Industry Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = utility.SafeGetString(reader, "Code"),
											.Name = utility.SafeGetString(reader, "Name"),
											.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedIndustryData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function

		Private Function LoadCVLAdditionalInternetresourceData(ByVal additionalID As Integer) As List(Of InternetResource)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of InternetResource) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional InternetResource Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of InternetResource)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New InternetResource With {
											.InternetResourceID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_AddID", 0),
											.URL = utility.SafeGetString(reader, "URL"),
											.Title = utility.SafeGetString(reader, "Title"),
											.Source = utility.SafeGetString(reader, "Source"),
											.Snippet = utility.SafeGetDecimal(reader, "Snippet", Nothing)
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInternetresourceData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function


#End Region


		Function LoadCVLAdditionalInfoData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal addID As Integer) As AdditionalInfoViewDataDTO Implements ICVLizerDatabaseAccess.LoadCVLAdditionalInfoData
			Dim result As AdditionalInfoViewDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned CVL Additional Information Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New AdditionalInfoViewDataDTO

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)

						result.MilitaryService = SafeGetBoolean(reader, "MilitaryService", Nothing)
						result.Competences = SafeGetString(reader, "Competences")
						result.Additionals = SafeGetString(reader, "Additionals")
						result.Interests = SafeGetString(reader, "Interests")

						result.Languages = LoadAssignedCVLAddtioinalLanguageData(addID)
						result.DrivingLicences = LoadAssignedCVLAdditionalDrivingLicenceViewData(addID)
						result.UndatedSkills = LoadAssignedCVLAddtioinalUndatedSkillViewData(addID)
						result.UndatedOperationArea = LoadAssignedCVLAddtioinalUndatedOperationAreaViewData(addID)
						result.UndatedIndustries = LoadAssignedCVLAddtioinalUndatedIndustryViewData(addID)

						result.InternetResources = LoadAssignedCVLAddtioinalInternetResourceViewData(addID)


					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInfoData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAdditionalDrivingLicenceViewData(ByVal addID As Integer) As List(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional DrivingLicence Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "DrivingLicence")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAdditionalDrivingLicenceViewData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAddtioinalUndatedSkillViewData(ByVal addID As Integer) As List(Of CodeNameWeightViewData)
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated Skill Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameWeightViewData)

					While reader.Read
						Dim data = New CodeNameWeightViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")
						data.Weight = SafeGetDecimal(reader, "Weight", Nothing)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAddtioinalUndatedSkillViewData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAddtioinalUndatedOperationAreaViewData(ByVal addID As Integer) As List(Of CodeNameWeightViewData)
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated OperationArea Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameWeightViewData)

					While reader.Read
						Dim data = New CodeNameWeightViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")
						data.Weight = SafeGetDecimal(reader, "Weight", Nothing)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAddtioinalUndatedOperationAreaViewData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAddtioinalUndatedIndustryViewData(ByVal addID As Integer) As List(Of CodeNameWeightViewData)
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Undated Industry Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameWeightViewData)

					While reader.Read
						Dim data = New CodeNameWeightViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")
						data.Weight = SafeGetDecimal(reader, "Weight", Nothing)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAddtioinalUndatedIndustryViewData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAddtioinalInternetResourceViewData(ByVal addID As Integer) As List(Of InternetResourceViewData)
			Dim result As List(Of InternetResourceViewData) = Nothing

			Dim sql = "[Load Assigned CVL Additional InternetResource Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of InternetResourceViewData)

					While reader.Read
						Dim data = New InternetResourceViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.URL = SafeGetString(reader, "URL")
						data.Title = SafeGetString(reader, "Title")
						data.Source = SafeGetString(reader, "Source")
						data.Snippet = SafeGetString(reader, "Snippet")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAddtioinalInternetResourceViewData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedCVLAddtioinalLanguageData(ByVal addID As Integer) As List(Of LanguageData)
			Dim result As List(Of LanguageData) = Nothing

			Dim sql = "[Load Assigned CVL Additional Language Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@AddID", ReplaceMissing(addID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of LanguageData)

					While reader.Read
						Dim data = New LanguageData

						data.Code = SafeGetString(reader, "FK_LanguageCode")
						data.CodeName = SafeGetString(reader, "LanguageLable")

						Dim levelCode As String = String.Empty
						Dim levelName As String = String.Empty

						levelCode = SafeGetString(reader, "FK_LanguageLevelCode")
						levelName = SafeGetString(reader, "LanguageLevelLable")
						data.Level = New CodeNameData With {.Code = levelCode, .CodeName = levelName}


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLAddtioinalLanguageData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function


#Region "searching for experiences"

		Function LoadCVLPostcodeCityData(ByVal customerID As String) As IEnumerable(Of PostcodeCityViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLPostcodeCityData
			Dim result As List(Of PostcodeCityViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Customer Experiences Postcode And Cities]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of PostcodeCityViewDataDTO)

					While reader.Read
						Dim data = New PostcodeCityViewDataDTO

						data.Customer_ID = customerID
						data.Postcode = SafeGetString(reader, "Postcode")
						data.City = SafeGetString(reader, "City")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPostcodeCityData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadCVLJobGroupsData(ByVal customerID As String) As IEnumerable(Of ExperiencesViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLJobGroupsData
			Dim result As List(Of ExperiencesViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Customer Experiences Job Groups]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of ExperiencesViewDataDTO)

					While reader.Read
						Dim data = New ExperiencesViewDataDTO

						data.Customer_ID = customerID
						data.Code = SafeGetString(reader, "code")
						data.ExperienceLabel = SafeGetString(reader, "ExperienceLabel")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLJobGroupsData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadCVLExperiencesData(ByVal customerID As String) As IEnumerable(Of ExperiencesViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLExperiencesData
			Dim result As List(Of ExperiencesViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Customer Experiences]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of ExperiencesViewDataDTO)

					While reader.Read
						Dim data = New ExperiencesViewDataDTO

						data.Customer_ID = customerID
						data.Code = SafeGetString(reader, "code")
						data.ExperienceLabel = SafeGetString(reader, "ExperienceLabel")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLExperiencesData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadCVLLanguageData(ByVal customerID As String) As IEnumerable(Of LanguageViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLLanguageData
			Dim result As List(Of LanguageViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Customer Experiences Language]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of LanguageViewDataDTO)

					While reader.Read
						Dim data = New LanguageViewDataDTO

						data.Customer_ID = customerID
						data.LanguageCode = SafeGetString(reader, "LanguageCode")
						data.LanguageLabel = SafeGetString(reader, "LanguageLabel")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLLanguageData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

    Function LoadCVLSearchData(ByVal customerID As String, ByVal userID As String, ByVal postfachCityData As PostcodeCityViewDataDTO,
                               ByVal radius As Integer, ByVal jobTitelsData As List(Of ExperiencesViewDataDTO), ByVal opAreaData As List(Of ExperiencesViewDataDTO), ByVal operationAreaJoin As JoinENum,
                               ByVal skillData As List(Of ExperiencesViewDataDTO), ByVal skillJoin As JoinENum,
                               ByVal languageData As List(Of LanguageViewDataDTO), ByVal languageJoin As JoinENum,
                               ByVal searchLabel As String, ByVal setNotification As Boolean) As IEnumerable(Of CVLSearchResultDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLSearchData
      Dim result As List(Of CVLSearchResultDataDTO) = Nothing
      Dim excludeCheckInteger As Integer = 1
      m_customerID = customerID

      Dim Jobtitels As String = String.Empty
      For Each job In jobTitelsData
        Jobtitels = Jobtitels & IIf(Jobtitels <> "", ", ", "") & job.Code
      Next

      Dim opArea As String = String.Empty
      For Each functionTitel In opAreaData
        opArea = opArea & IIf(opArea <> "", ", ", "") & functionTitel.Code
      Next
      Dim experiencesBuffer As String = String.Empty
      For Each experience In skillData
        experiencesBuffer = experiencesBuffer & IIf(experiencesBuffer <> "", ", ", "") & experience.Code
      Next
      Dim languagesBuffer As String = String.Empty
      For Each language In languageData
        languagesBuffer = languagesBuffer & IIf(languagesBuffer <> "", ", ", "") & language.LanguageCode
      Next
      If postfachCityData Is Nothing OrElse String.IsNullOrWhiteSpace(postfachCityData.Postcode) Then radius = 0


      Dim sql = "[Load CVL Search Result Data]"

      Dim listOfParams = New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("postcode", ReplaceMissing(postfachCityData.Postcode, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("radius", ReplaceMissing(radius, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("JobTitels", ReplaceMissing(Jobtitels, DBNull.Value)))

      listOfParams.Add(New SqlClient.SqlParameter("opAreaTitels", ReplaceMissing(opArea, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("competences", ReplaceMissing(experiencesBuffer, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("languages", ReplaceMissing(languagesBuffer, DBNull.Value)))


      Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

      Try

        If reader IsNot Nothing Then

          result = New List(Of CVLSearchResultDataDTO)

          While reader.Read
            Dim data = New CVLSearchResultDataDTO

            data.Customer_ID = customerID
            data.CVLProfileID = SafeGetInteger(reader, "CVLProfileID", 0)
            data.PersonalID = SafeGetInteger(reader, "PersonalID", 0)
            data.EmployeeID = SafeGetInteger(reader, "EmployeeID", 0)

            data.Firstname = SafeGetString(reader, "Firstname")
            data.Lastname = SafeGetString(reader, "Lastname")
            data.Postcode = SafeGetString(reader, "Postcode")
            data.Street = SafeGetString(reader, "Street")
            data.Location = SafeGetString(reader, "Location")
            data.CountryCode = SafeGetString(reader, "CountryCode")
            data.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
            data.EmployeeAge = SafeGetInteger(reader, "EmployeeAge", 0)
            data.JobTitel = SafeGetString(reader, "JobTitel")
            data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


            result.Add(data)

          End While

        End If
        Dim queryContent As String

        queryContent = String.Format("luePostcodeCity: {0}", ReplaceMissing(postfachCityData.Postcode, DBNull.Value))

        If Not String.IsNullOrWhiteSpace(searchLabel) Then AddUserCVLSeachResultData(customerID, userID, String.Empty, searchLabel, setNotification, result)


      Catch ex As Exception
        Dim msgContent = ex.ToString
        m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLSearchData", .MessageContent = msgContent})
      Finally
        If Not reader Is Nothing Then
          Try
            reader.Close()
          Catch
            ' Do nothing
          End Try

        End If
      End Try


      Return result
    End Function

    Function LoadUserCVLSearchHistoryData(ByVal customerID As String, ByVal userID As String) As IEnumerable(Of CVLSearchHistoryDataDTO) Implements ICVLizerDatabaseAccess.LoadUserCVLSearchHistoryData
			Dim result As List(Of CVLSearchHistoryDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load CVLSearch Query Notifications]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of CVLSearchHistoryDataDTO)

					While reader.Read
						Dim data = New CVLSearchHistoryDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = customerID
						data.User_ID = SafeGetString(reader, "User_ID")
						data.QueryName = SafeGetString(reader, "QueryName")
						data.QueryContent = SafeGetString(reader, "QueryContent")
						data.QueryResultContent = SafeGetString(reader, "QueryResultContent")
						data.Notify = SafeGetBoolean(reader, "Notify", False)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.ResultCount = SafeGetInteger(reader, "ResultCount", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadUserCVLSearchHistoryData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadAssignedCVLSearchHistoryResultData(ByVal customerID As String, ByVal searchID As Integer) As IEnumerable(Of CVLSearchResultDataDTO) Implements ICVLizerDatabaseAccess.LoadAssignedCVLSearchHistoryResultData
			Dim result As List(Of CVLSearchResultDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load CVLSearch Assigned Query Result Notifications]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("searchID", ReplaceMissing(searchID, DBNull.Value)))


			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of CVLSearchResultDataDTO)

					While reader.Read
						Dim data = New CVLSearchResultDataDTO

						data.Customer_ID = customerID
						data.CVLProfileID = SafeGetInteger(reader, "CVLProfileID", 0)
						data.PersonalID = SafeGetInteger(reader, "PersonalID", 0)
						data.EmployeeID = SafeGetInteger(reader, "EmployeeID", 0)
						data.Firstname = SafeGetString(reader, "Firstname")
						data.Lastname = SafeGetString(reader, "Lastname")
						data.Postcode = SafeGetString(reader, "Postcode")
						data.Street = SafeGetString(reader, "Street")
						data.Location = SafeGetString(reader, "Location")
						data.CountryCode = SafeGetString(reader, "CountryCode")
						data.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
						data.EmployeeAge = SafeGetInteger(reader, "EmployeeAge", 0)
						data.JobTitel = SafeGetString(reader, "JobTitel")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLSearchHistoryResultData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function UpdateAssignedCVLSearchHistoryNotifierData(ByVal customerID As String, ByVal searchID As Integer) As Boolean Implements ICVLizerDatabaseAccess.UpdateAssignedCVLSearchHistoryNotifierData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql = "[Update CVLSearch Assigned Query Notifier State]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("searchID", ReplaceMissing(searchID, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

		Function DeleteAssignedCVLSearchHistoryData(ByVal customerID As String, ByVal searchID As Integer) As Boolean Implements ICVLizerDatabaseAccess.DeleteAssignedCVLSearchHistoryData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql = "[Delete CVLSearch Assigned Query Notifications]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("searchID", ReplaceMissing(searchID, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Return success
		End Function

#End Region



#Region "private methods"

		Function AddUserCVLSeachResultData(ByVal customerID As String, ByVal userID As String, ByVal queryContent As String, ByVal searchLabel As String, ByVal setNotification As Boolean, ByVal searchResult As IEnumerable(Of CVLSearchResultDataDTO)) As Boolean
			Dim success As Boolean = True
			If String.IsNullOrWhiteSpace(searchLabel) Then
				setNotification = False

				Return True
			End If

			Dim sql As String
			sql = "[Add CVLSearch Query Notification]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("QueryContent", ReplaceMissing(queryContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("QueryResultContent", DBNull.Value))
			listOfParams.Add(New SqlClient.SqlParameter("QueryName", ReplaceMissing(searchLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notify", ReplaceMissing(setNotification, False)))


			' New searchID
			Dim newIdParameter = New SqlClient.SqlParameter("@newID", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			If Not (success AndAlso Not newIdParameter.Value Is Nothing) Then
				success = False
			End If
			If Not success Then Return success

			For Each itm In searchResult
				sql = "[Add CVLSearch Query Result]"

				listOfParams.Clear()
				listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("FK_SearchQueryID", ReplaceMissing(newIdParameter.Value, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(itm.CVLProfileID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(itm.PersonalID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(itm.EmployeeID, False)))
				listOfParams.Add(New SqlClient.SqlParameter("Firstname", ReplaceMissing(itm.Firstname, False)))
				listOfParams.Add(New SqlClient.SqlParameter("Lastname", ReplaceMissing(itm.Lastname, False)))
				listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(itm.Postcode, False)))
				listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(itm.Street, False)))
				listOfParams.Add(New SqlClient.SqlParameter("Location", ReplaceMissing(itm.Location, False)))
				listOfParams.Add(New SqlClient.SqlParameter("CountryCode", ReplaceMissing(itm.CountryCode, False)))
				listOfParams.Add(New SqlClient.SqlParameter("DateOfBirth", ReplaceMissing(itm.DateOfBirth, False)))
				listOfParams.Add(New SqlClient.SqlParameter("EmployeeAge", ReplaceMissing(itm.EmployeeAge, False)))
				listOfParams.Add(New SqlClient.SqlParameter("JobTitel", ReplaceMissing(itm.JobTitel, False)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedOn", ReplaceMissing(itm.CreatedOn, False)))


				success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

			Next


			Return success

		End Function

		Private Function SaveToXml(Of T)(ByVal instance As T, ByVal filePath As String) As Boolean
			Dim objSerialize As System.Xml.Serialization.XmlSerializer
			Dim fs As System.IO.FileStream
			Try
				If instance Is Nothing Then
					Throw New ArgumentNullException("instance")
				End If
				objSerialize = New System.Xml.Serialization.XmlSerializer(instance.GetType())
				fs = New System.IO.FileStream(filePath, IO.FileMode.Create)
				objSerialize.Serialize(fs, instance)
				fs.Close()
				Return True
			Catch ex As Exception
				Throw
			End Try
		End Function


#End Region

	End Class

End Namespace
