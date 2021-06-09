

Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer
	Partial Class CVLizerDatabaseAccess

#Region "phase data"

		Private Function LoadAssignedCVLPhaseData(ByVal phaseID As Integer) As Phase
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As Phase = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Data For Notification]", conn)
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

				listOfSearchResultDTO = New Phase
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New Phase With {
												.PhaseID = utility.SafeGetInteger(reader, "ID", 0),
												.DateFrom = utility.SafeGetDateTime(reader, "DateFrom", Nothing),
												.DateTo = utility.SafeGetDateTime(reader, "DateTo", Nothing),
												.DateFromFuzzy = utility.SafeGetString(reader, "DateFromFuzzy"),
												.DateToFuzzy = utility.SafeGetString(reader, "DateToFuzzy"),
												.Duration = utility.SafeGetString(reader, "Duration", Nothing),
												.Current = utility.SafeGetString(reader, "Current", Nothing),
												.SubPhase = utility.SafeGetString(reader, "SubPhase", Nothing),
												.Comments = utility.SafeGetString(reader, "Comments"),
												.PlainText = utility.SafeGetString(reader, "PlainText")
										}

					' load personal prpoerties
					Dim addressData = New List(Of AddressData)
					addressData = LoadCVLPhaseLocationData(dto.PhaseID)
					dto.Location = addressData

					Dim phaseProperty = New List(Of CodeNameWeightedData)
					phaseProperty = LoadCVLPhaseSkillData(dto.PhaseID)
					dto.Skill = phaseProperty

					phaseProperty = LoadCVLPhaseSoftSkillData(dto.PhaseID)
					dto.SoftSkill = phaseProperty

					phaseProperty = LoadCVLPhaseOperationAreaData(dto.PhaseID)
					dto.OperationAreas = phaseProperty

					phaseProperty = LoadCVLPhaseIndustryData(dto.PhaseID)
					dto.Industries = phaseProperty

					phaseProperty = LoadCVLPhaseCustomCodeData(dto.PhaseID)
					dto.CustomCodes = phaseProperty

					Dim propertyData = New List(Of CodeNameData)
					propertyData = LoadCVLPhaseTopicData(dto.PhaseID)
					dto.Topic = propertyData

					Dim internetData = New List(Of InternetResource)
					internetData = LoadCVLPhaseInternetresourceData(dto.PhaseID)
					dto.InternetRosources = internetData

					Dim documentData = New List(Of CodeIDData)
					documentData = LoadCVLPhaseDocumentIDData(dto.PhaseID)
					dto.DocumentID = documentData


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPhaseData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseLocationData(ByVal phaseID As Integer) As List(Of AddressData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of AddressData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Location Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of AddressData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New AddressData With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_PersonalID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Street = utility.SafeGetString(reader, "Street"),
												.Postcode = utility.SafeGetString(reader, "PostCode"),
												.City = utility.SafeGetString(reader, "City"),
												.FK_CountryCode = utility.SafeGetString(reader, "FK_CountryCode"),
												.State = utility.SafeGetString(reader, "State")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseLocationData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseSkillData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Skill Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
												.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseSkillData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseSoftSkillData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase SoftSkill Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
												.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseSoftSkillData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseOperationAreaData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase OperationArea Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
												.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseOperationAreaData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseIndustryData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Industry Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
												.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseIndustryData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseCustomCodeData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase CustomCode Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeNameWeightedData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeNameWeightedData With {
												.CodeNameWeightedID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetString(reader, "Code"),
												.Name = utility.SafeGetString(reader, "Name"),
												.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseCustomCodeData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseTopicData(ByVal phaseID As Integer) As List(Of CodeNameData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Topic Data For Notification]", conn)
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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.CodeName = utility.SafeGetString(reader, "Name")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTitleData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseInternetresourceData(ByVal phaseID As Integer) As List(Of InternetResource)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of InternetResource) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase InternetResource Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of InternetResource)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New InternetResource With {
												.InternetResourceID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.URL = utility.SafeGetString(reader, "URL"),
												.Title = utility.SafeGetString(reader, "Title"),
												.Source = utility.SafeGetString(reader, "Source"),
												.Snippet = utility.SafeGetDecimal(reader, "Snippet", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseInternetresourceData", .MessageContent = msgContent})
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

		Private Function LoadCVLPhaseDocumentIDData(ByVal phaseID As Integer) As List(Of CodeIDData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CodeIDData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase DocumentID Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CodeIDData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CodeIDData With {
												.CodeID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PhasesID", 0),
												.Code = utility.SafeGetInteger(reader, "Code", Nothing)
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseDocumentIDData", .MessageContent = msgContent})
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
