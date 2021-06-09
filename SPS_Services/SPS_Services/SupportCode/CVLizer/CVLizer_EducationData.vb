

Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

#Region "education data"

		Private Function LoadCVLEducationData(ByVal profileID As Integer, ByVal educationID As Integer) As EdPhaseDataDTO
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As EdPhaseDataDTO = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
				listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

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

				listOfSearchResultDTO = New EdPhaseDataDTO
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New EdPhaseDataDTO With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.AdditionalText = utility.SafeGetString(reader, "AdditionalText")
										}

					' load personal prpoerties
					Dim edPhaseData = New List(Of EducationPhaseDataDTO)
					edPhaseData = LoadCVLEducationPhaseData(dto.ID)
					dto.EducationPhases = edPhaseData


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationData", .MessageContent = msgContent})
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

		Private Function LoadCVLEducationPhaseData(ByVal educationID As Integer) As List(Of EducationPhaseDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of EducationPhaseDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL EducationPhase Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

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

				listOfSearchResultDTO = New List(Of EducationPhaseDataDTO)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New EducationPhaseDataDTO With {
												.EducationPhaseID = utility.SafeGetInteger(reader, "ID", 0),
												.PhaseID = utility.SafeGetString(reader, "FK_PhasesID"),
												.IsCed = utility.SafeGetString(reader, "FK_IsCedCode"),
												.Completed = utility.SafeGetString(reader, "Completed", Nothing),
												.Score = utility.SafeGetInteger(reader, "Score", Nothing)
										}

					dto.EducationType = LoadCVLEducationTypeData(educationID)
					dto.SchoolName = LoadCVLEducationSchoolnameData(educationID)
					dto.Graduation = LoadCVLEducationGraduationData(educationID)

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
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationPhaseData", .MessageContent = msgContent})
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

		Private Function LoadCVLEducationTypeData(ByVal educationID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL EducationType Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_EducPhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationTypeData", .MessageContent = msgContent})
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

		Private Function LoadCVLEducationSchoolnameData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Schoolname Data For Notification]", conn)
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
												.FK_ID = utility.SafeGetInteger(reader, "FK_EducPhasesID", Nothing),
												.PropertyName = utility.SafeGetString(reader, "Schoolname")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationSchoolnameData", .MessageContent = msgContent})
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

		Private Function LoadCVLEducationGraduationData(ByVal phaseID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Graduation Data For Notification]", conn)
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
												.PropertyName = utility.SafeGetString(reader, "Graduations")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationGraduationData", .MessageContent = msgContent})
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


		Function LoadCVLEducationPhaseData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal educationID As Integer) As IEnumerable(Of EducationPhaseViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLEducationPhaseData
			Dim result As List(Of EducationPhaseViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned CVL Education Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("@EducationID", ReplaceMissing(educationID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of EducationPhaseViewDataDTO)

					While reader.Read
						Dim data = New EducationPhaseViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.EducationPhaseID = SafeGetInteger(reader, "FK_EducationID", Nothing)
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

						data.IsCedCodeLable = SafeGetString(reader, "IsCedCodeLable")
						data.Completed = SafeGetBoolean(reader, "Completed", Nothing)
						data.Score = SafeGetInteger(reader, "Score", Nothing)
						data.SchooolNames = LoadAssignedCVLEducationPhaseSchoolnameViewData(data.ID)
						data.Graduations = LoadAssignedCVLEducationPhaseGraduationViewData(data.ID)
						data.EducationTypes = LoadAssignedCVLEducationPhaseEducationTypeViewData(data.ID)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationPhaseData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLEducationPhaseSchoolnameViewData(ByVal educationPhaseID As Integer) As List(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Schoolname Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Schoolname")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLEducationPhaseSchoolnameViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLEducationPhaseGraduationViewData(ByVal educationPhaseID As Integer) As List(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase Graduation Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Graduations")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLEducationPhaseGraduationViewData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLEducationPhaseEducationTypeViewData(ByVal educationPhaseID As Integer) As List(Of CodeNameWeightViewData)
			Dim result As List(Of CodeNameWeightViewData) = Nothing

			Dim sql = "[Load Assigned CVL Phase EducationType Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@EducationPhaseID", ReplaceMissing(educationPhaseID, DBNull.Value)))

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLEducationPhaseEducationTypeViewData", .MessageContent = msgContent})
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

	End Class

End Namespace
