
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.Logging
Imports wsSPS_Services.SPUtilities


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess


		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()



#Region "work data"

		Private Function LoadCVLWorkData(ByVal profileID As Integer, ByVal workID As Integer) As WPhaseDataDTO
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As WPhaseDataDTO = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
				listOfParams.Add(New SqlClient.SqlParameter("WorkID", workID))

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

				listOfSearchResultDTO = New WPhaseDataDTO
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New WPhaseDataDTO With {
											.ID = utility.SafeGetInteger(reader, "ID", 0),
											.AdditionalText = utility.SafeGetString(reader, "AdditionalText")
									}

					' load personal prpoerties
					Dim wphaseData = New List(Of WorkPhaseDataDTO)
					wphaseData = LoadCVLWorkPhaseData(dto.ID)
					dto.WorkPhases = wphaseData


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkPhaseData(ByVal workID As Integer) As List(Of WorkPhaseDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of WorkPhaseDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL WorkPhase Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("WorkID", workID))

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

				listOfSearchResultDTO = New List(Of WorkPhaseDataDTO)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New WorkPhaseDataDTO With {
											.WorkPhaseID = utility.SafeGetInteger(reader, "ID", 0),
											.PhaseID = utility.SafeGetString(reader, "FK_PhasesID"),
											.Project = utility.SafeGetBoolean(reader, "Project", False)
									}
					dto.Company = LoadCVLWorkCompanyData(workID)
					dto.Functions = LoadCVLWorkFunctionData(workID)
					dto.Positions = LoadCVLWorkPositionData(workID)
					dto.Employments = LoadCVLWorkEmploymentData(workID)
					dto.WorkTimes = LoadCVLWorkWorktimeData(workID)

					Dim phase = LoadAssignedCVLPhaseData(dto.PhaseID)

					If Not phase Is Nothing Then
						dto.PhaseID = phase.PhaseID
						dto.DateFrom = phase.DateFrom
						dto.DateTo = phase.DateTo
						dto.DateFromFuzzy = phase.DateFromFuzzy
						dto.DateToFuzzy = phase.DateToFuzzy
						dto.Duration = phase.Duration
						dto.Current = phase.Current
						dto.SubPhase = phase.SubPhase
						dto.Comments = phase.Comments
						dto.PlainText = phase.PlainText
						dto.Location = phase.Location
						dto.Skill = phase.Skill
						dto.SoftSkill = phase.SoftSkill
						dto.OperationAreas = phase.OperationAreas
						dto.Industries = phase.Industries
						dto.CustomCodes = phase.CustomCodes
						dto.Topic = phase.Topic
						dto.InternetRosources = phase.InternetRosources
						dto.DocumentID = phase.DocumentID
					End If


					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPhaseData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkCompanyData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Company Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

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
											.FK_ID = utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Company")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkCompanyData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkFunctionData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Function Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

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
											.FK_ID = utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Function")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkFunctionData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkPositionData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Position Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

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

				listOfSearchResultDTO = New List(Of CodeNameData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameData With {
											.CodeNameID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPositionData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkEmploymentData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Employment Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

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

				listOfSearchResultDTO = New List(Of CodeNameData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameData With {
											.CodeNameID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkEmploymentData", .MessageContent = msgContent})
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

		Private Function LoadCVLWorkWorktimeData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Worktime Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

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

				listOfSearchResultDTO = New List(Of CodeNameData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameData With {
											.CodeNameID = utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkWorktimeData", .MessageContent = msgContent})
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


		Function LoadCVLWorkPhaseData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal workID As Integer) As IEnumerable(Of WorkPhaseViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLWorkPhaseData
			Dim result As List(Of WorkPhaseViewDataDTO) = Nothing
			Dim reader As SqlClient.SqlDataReader = Nothing
			Dim phaseID As New List(Of Integer)
			m_customerID = customerID

			Dim sql = "[Load Assigned CVL Work Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@WorkID", ReplaceMissing(workID, 0)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of WorkPhaseViewDataDTO)

					While reader.Read
						Dim data = New WorkPhaseViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.WorkID = SafeGetInteger(reader, "FK_WorkID", Nothing)
						data.PhaseID = SafeGetInteger(reader, "FK_PhasesID", Nothing)

						data.DateFromFuzzy = SafeGetString(reader, "DateFromFuzzy")
						data.DateToFuzzy = SafeGetString(reader, "DateToFuzzy")

						data.Duration = SafeGetInteger(reader, "Duration", Nothing)
						data.Current = SafeGetBoolean(reader, "Current", Nothing)
						data.SubPhase = SafeGetBoolean(reader, "SubPhase", Nothing)
						data.Comments = SafeGetString(reader, "Comments")
						data.PlainText = SafeGetString(reader, "PlainText")

						data.DateFrom = SafeGetDateTime(reader, "DateFrom", Nothing)
						data.DateTo = SafeGetDateTime(reader, "DateTo", Nothing)

						data.Locations = LoadAssignedCVLWorkPhaseAddressViewData(data.PhaseID)
						data.Skills = LoadAssignedCVLWorkPhaseSkillViewData(data.PhaseID)
						data.SoftSkills = LoadAssignedCVLWorkPhaseSoftSkillViewData(data.PhaseID)
						data.OperationAreas = LoadAssignedCVLWorkPhaseOperationAreaViewData(data.PhaseID)
						data.Industries = LoadAssignedCVLWorkPhaseIndustryViewData(data.PhaseID)
						data.CustomCodes = LoadAssignedCVLWorkPhaseCustomCodeViewData(data.PhaseID)
						data.Topic = LoadAssignedCVLWorkPhaseTopicViewData(data.PhaseID)
						data.InternetResources = LoadAssignedCVLWorkPhaseInternetResourceViewData(data.PhaseID)
						data.DocumentID = LoadAssignedCVLWorkPhaseDocumentIDViewData(data.PhaseID)

						data.Companies = LoadAssignedCVLWorkPhaseCompanyViewData(data.ID)
						data.Functions = LoadAssignedCVLWorkPhaseFunctionViewData(data.ID)
						data.Positions = LoadAssignedCVLWorkPhasePositionViewData(data.ID)
						data.Project = SafeGetBoolean(reader, "Project", Nothing)
						data.Employments = LoadAssignedCVLWorkPhaseEmploymentViewData(data.ID)
						data.WorkTimes = LoadAssignedCVLWorkPhaseWorktimeViewData(data.ID)

						phaseID.Add(data.PhaseID)

						result.Add(data)

					End While

				End If
				m_Logger.LogInfo(String.Format("SQL: {0} | customerID: {1} | CVLProfileID: {2} | WorkID: {3} >>> phaseID: {4}", sql, customerID, cvlPrifleID, workID, String.Join(", ", phaseID)))


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPhaseData", .MessageContent = msgContent})
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


#Region "private methodes"

		Private Function LoadAssignedCVLWorkPhaseAddressViewData(ByVal phaseID As Integer) As IEnumerable(Of CVLAddressDTO)  ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseAddressViewData
			Dim result As List(Of CVLAddressDTO) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase Location Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CVLAddressDTO)

					While reader.Read
						Dim data = New CVLAddressDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Street = SafeGetString(reader, "Street")
						data.Postcode = SafeGetString(reader, "Postcode")
						data.City = SafeGetString(reader, "City")
						data.Country = SafeGetString(reader, "FK_CountryCode")
						data.CountryLable = SafeGetString(reader, "CountryLable")
						data.State = SafeGetString(reader, "State")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseAddressViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseSkillViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase Skill Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseSkillViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseSoftSkillViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseSoftSkillViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase SoftSkill Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseSoftSkillViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseOperationAreaViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseOperationAreaViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase OperationArea Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseOperationAreaViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseIndustryViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseIndustryViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase Industry Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseIndustryViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseCustomCodeViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeNameWeightViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseCustomCodeViewData
			Dim result As List(Of CodeNameWeightViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase CustomCode Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseCustomCodeViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseTopicViewData(ByVal phaseID As Integer) As IEnumerable(Of CodeViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseTopicViewData
			Dim result As List(Of CodeViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Phase Topic Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseTopicViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseInternetResourceViewData(ByVal phaseID As Integer) As IEnumerable(Of InternetResourceViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseInternetResourceViewData
			Dim result As List(Of InternetResourceViewData) = Nothing
			Dim reader As SqlDataReader = Nothing
			Dim strMessage As New StringBuilder()

			Dim sql = "[Load Assigned CVL Phase InternetResource Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseInternetResourceViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseDocumentIDViewData(ByVal phaseID As Integer) As IEnumerable(Of IDiewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseDocumentIDViewData
			Dim result As List(Of IDiewData) = Nothing
			Dim reader As SqlDataReader = Nothing
			Dim strMessage As New StringBuilder()

			Dim sql = "[Load Assigned CVL Phase DocumentID Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PhaseID", ReplaceMissing(phaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of IDiewData)

					While reader.Read
						Dim data = New IDiewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.CodeNumber = SafeGetInteger(reader, "Code", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseDocumentIDViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseCompanyViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseCompanyViewData
			Dim result As List(Of CodeViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Work Company Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Company")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseCompanyViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseFunctionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseFunctionViewData
			Dim result As List(Of CodeViewData) = Nothing
			Dim reader As SqlDataReader = Nothing
			Dim strMessage As New StringBuilder()

			Dim sql = "[Load Assigned CVL Work Function Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Function")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseFunctionViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhasePositionViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhasePositionViewData
			Dim result As List(Of CodeNameViewData) = Nothing
			Dim reader As SqlDataReader = Nothing
			Dim strMessage As New StringBuilder()

			Dim sql = "[Load Assigned CVL Work Position Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhasePositionViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseEmploymentViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseEmploymentViewData
			Dim result As List(Of CodeNameViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Work Employment Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseEmploymentViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLWorkPhaseWorktimeViewData(ByVal workPhaseID As Integer) As IEnumerable(Of CodeNameViewData) ' Implements ICVLizerDatabaseAccess.LoadAssignedCVLWorkPhaseWorktimeViewData
			Dim result As List(Of CodeNameViewData) = Nothing
			Dim reader As SqlDataReader = Nothing

			Dim sql = "[Load Assigned CVL Work Worktime Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@WorkPhaseID", ReplaceMissing(workPhaseID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of CodeNameViewData)

					While reader.Read
						Dim data = New CodeNameViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Code = SafeGetString(reader, "Code")
						data.Name = SafeGetString(reader, "Name")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLWorkPhaseWorktimeViewData", .MessageContent = msgContent})
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


#End Region

	End Class

End Namespace
