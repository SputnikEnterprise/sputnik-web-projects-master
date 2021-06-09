

Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities

Namespace CVLizer


	Partial Class CVLizerDatabaseAccess

#Region "publication"

		Private Function LoadCVLPublicationPhaseData(ByVal profileID As Integer) As List(Of PublicationDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PublicationDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Publication Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))

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

				listOfSearchResultDTO = New List(Of PublicationDataDTO)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New PublicationDataDTO With {
											.PublicationPhaseID = utility.SafeGetInteger(reader, "ID", 0),
											.Proceedings = utility.SafeGetString(reader, "Proceedings"),
											.Institute = utility.SafeGetString(reader, "Institute")
									}

					dto.Author = LoadCVLPublicationAutorData(dto.PublicationPhaseID)

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
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationPhaseData", .MessageContent = msgContent})
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

		Private Function LoadCVLPublicationAutorData(ByVal publicationID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Publication Authors Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("publicationID", publicationID))

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
											.FK_ID = utility.SafeGetInteger(reader, "FK_PubPhaseID", Nothing),
											.PropertyName = utility.SafeGetString(reader, "Authors")
									}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationAutorData", .MessageContent = msgContent})
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


		Function LoadCVLPublicationData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As IEnumerable(Of PublicationViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLPublicationData
			Dim result As List(Of PublicationViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned CVL Publication Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of PublicationViewDataDTO)

					While reader.Read
						Dim data = New PublicationViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
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

						data.Proceedings = SafeGetString(reader, "Proceedings")
						data.Institute = SafeGetString(reader, "Institute")

						data.Author = LoadAssignedCVLPublicationAuthorsViewData(data.ID)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationData", .MessageContent = msgContent})
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

		Private Function LoadAssignedCVLPublicationAuthorsViewData(ByVal publicationID As Integer) As List(Of CodeViewData)
			Dim result As List(Of CodeViewData) = Nothing

			Dim sql = "[Load Assigned CVL Publication Authors Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@publicationID", ReplaceMissing(publicationID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeViewData)

					While reader.Read
						Dim data = New CodeViewData

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Lable = SafeGetString(reader, "Authors")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPublicationAuthorsViewData", .MessageContent = msgContent})
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
