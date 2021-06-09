

Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer


	Partial Class CVLizerDatabaseAccess


#Region "objective data"

		Private Function LoadCVLObjectiveData(ByVal profileID As Integer, ByVal objID As Integer) As ObjectiveDataDTO
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As ObjectiveDataDTO = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
				listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

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

				listOfSearchResultDTO = New ObjectiveDataDTO
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New ObjectiveDataDTO With {
											.ID = utility.SafeGetInteger(reader, "ID", 0),
											.AvailabilityDate = utility.SafeGetDateTime(reader, "AvailabilityDate", Nothing)
									}

					' load prpoerties
					dto.Salary = LoadCVLObjSalaryData(dto.ID)


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjSalaryData(ByVal objID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Salary Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Salary")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjSalaryData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectivePhaseData(ByVal objID As Integer) As List(Of WorkPhaseDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of WorkPhaseDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Phase Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

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
					dto.Company = LoadCVLObjectiveCompanyData(objID)
					dto.Functions = LoadCVLObjectiveFunctionData(objID)
					dto.Positions = LoadCVLObjectivePositionData(objID)
					dto.Employments = LoadCVLObjectiveEmploymentData(objID)
					dto.WorkTimes = LoadCVLObjectiveWorktimeData(objID)

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
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectivePhaseData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectiveCompanyData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Company Data For Notification]", conn)
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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Company")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveCompanyData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectiveFunctionData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Function Data For Notification]", conn)
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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Function")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveFunctionData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectivePositionData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Position Data For Notification]", conn)
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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectivePositionData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectiveEmploymentData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Employment Data For Notification]", conn)
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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveEmploymentData", .MessageContent = msgContent})
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

		Private Function LoadCVLObjectiveWorktimeData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Worktime Data For Notification]", conn)
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
											.FK_ID = utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = utility.SafeGetString(reader, "Code"),
											.CodeName = utility.SafeGetString(reader, "CodeName")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveWorktimeData", .MessageContent = msgContent})
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


	End Class

End Namespace
