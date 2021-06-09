
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess

#Region "personal informatio data"

		Private Function LoadCVLPersonalInformation(ByVal profileID As Integer, ByVal personalID As Integer) As PersonalInformationDataDTO
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As PersonalInformationDataDTO = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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

				listOfSearchResultDTO = New PersonalInformationDataDTO
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New PersonalInformationDataDTO With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_CVLID = utility.SafeGetInteger(reader, "FK_CVLID", 0),
												.FirstName = utility.SafeGetString(reader, "FirstName"),
												.LastName = utility.SafeGetString(reader, "LastName"),
												.FK_GenderCode = utility.SafeGetString(reader, "FK_GenderCode"),
												.FK_IsCedCode = utility.SafeGetString(reader, "FK_IsCedCode"),
												.DateOfBirth = utility.SafeGetDateTime(reader, "DateOfBirth", Nothing),
												.PlaceOfBirth = utility.SafeGetString(reader, "PlaceOfBirth")
										}

					' load personal prpoerties
					Dim addressData = New AddressData
					addressData = LoadCVLPersonalAddressData(dto.ID)
					dto.Address = addressData

					Dim propertyData = New List(Of PropertyListData)
					propertyData = LoadCVLPersonalTitleData(dto.ID)
					dto.Title = propertyData
					propertyData = LoadCVLPersonalNationalityData(dto.FK_CVLID.GetValueOrDefault(0), dto.ID)
					dto.Nationality = propertyData
					propertyData = LoadCVLPersonalCivilStateData(dto.FK_CVLID.GetValueOrDefault(0), dto.ID)
					dto.CivilState = propertyData
					propertyData = LoadCVLPersonalEMailData(dto.ID)
					dto.Email = propertyData
					propertyData = LoadCVLPersonalHomepageData(dto.ID)
					dto.Homepage = propertyData
					propertyData = LoadCVLPersonalTelefonnumberData(dto.ID)
					dto.PhoneNumbers = propertyData
					propertyData = LoadCVLPersonalTelefaxnumberData(dto.ID)
					dto.TelefaxNumber = propertyData


					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalInformation", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalAddressData(ByVal personalID As Integer) As AddressData
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As AddressData = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Address Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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

				listOfSearchResultDTO = New AddressData
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New AddressData With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_PersonalID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.Street = utility.SafeGetString(reader, "Street"),
												.Postcode = utility.SafeGetString(reader, "PostCode"),
												.City = utility.SafeGetString(reader, "City"),
												.FK_CountryCode = utility.SafeGetString(reader, "FK_CountryCode"),
												.State = utility.SafeGetString(reader, "State")
										}

					listOfSearchResultDTO = dto

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalAddressData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalTitleData(ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Title Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "Title")
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

		Private Function LoadCVLPersonalNationalityData(ByVal cvlPrifleID As Integer?, ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Nationality Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				'listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", cvlPrifleID))
				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "FK_NationalityCode")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalNationalityData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalCivilStateData(ByVal cvlPrifleID As Integer?, ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal CivilState Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				'listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", cvlPrifleID))
				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "FK_CivilStateCode")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalCivilStateData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalEMailData(ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal EMail Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "EMailAddress")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalEMailData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalHomepageData(ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Homepage Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "Homepage")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalHomepageData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalTelefonnumberData(ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Telefonnumber Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

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
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "PhoneNumber")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTelefonnumberData", .MessageContent = msgContent})
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

		Private Function LoadCVLPersonalTelefaxnumberData(ByVal personalID As Integer) As List(Of PropertyListData)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Telefaxnumber Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}", cmd.Parameters(i).ParameterName, cmd.Parameters(i).DbType, cmd.Parameters(i).Size, cmd.Parameters(i).Value, ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of PropertyListData)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New PropertyListData With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.FK_ID = utility.SafeGetInteger(reader, "FK_PersonalID", 0),
												.PropertyName = utility.SafeGetString(reader, "TelefaxNumber")
										}

					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTelefaxnumberData", .MessageContent = msgContent})
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

		Function LoadAssignedCVLPersonalData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer?) As CVLPersonalDataDTO Implements ICVLizerDatabaseAccess.LoadAssignedCVLPersonalData
			Dim listOfSearchResultDTO As CVLPersonalDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned CVL Personal Data]"
			Dim listOfParams = New List(Of SqlClient.SqlParameter)

			' Input Parameters
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			m_Logger.LogInfo(String.Format("{0}: {1} >>> {2}", sql, cvlPrifleID, cvlPersonalID))
			Try
				listOfSearchResultDTO = New CVLPersonalDataDTO

				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim Result = New CVLPersonalDataDTO

					Result.PersonalID = SafeGetInteger(reader, "ID", Nothing)
					Result.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
					Result.DateOfBirthPlace = SafeGetString(reader, "PlaceOfBirth")
					Result.FirstName = SafeGetString(reader, "FirstName")
					Result.Gender = SafeGetString(reader, "FK_GenderCode")
					Result.GenderLabel = SafeGetString(reader, "GenderLabel")

					Result.IsCed = SafeGetString(reader, "FK_IsCedCode")
					Result.IsCedLable = SafeGetString(reader, "FK_IsCedCode")
					Result.LastName = SafeGetString(reader, "LastName")

					Dim code As String = String.Empty
					Dim Lable As String = String.Empty
					If cvlPersonalID.GetValueOrDefault(0) = 0 Then cvlPersonalID = Result.PersonalID

					Dim nationality = LoadAssignedCVLPersonalNationalityViewData(cvlPrifleID, cvlPersonalID)
					If Not nationality Is Nothing AndAlso nationality.Count > 0 Then
						For Each itm In nationality
							code &= If(String.IsNullOrWhiteSpace(code), "", ", ") & itm.Code
							Lable &= If(String.IsNullOrWhiteSpace(Lable), "", ", ") & itm.CodeName
						Next
						Result.Nationality = code
						Result.NationalityLable = Lable
					End If

					code = String.Empty
					Lable = String.Empty

					Dim civilstate = LoadAssignedCVLPersonalCivilstateViewData(cvlPrifleID, cvlPersonalID)
					If Not civilstate Is Nothing AndAlso civilstate.Count > 0 Then
						For Each itm In civilstate
							code &= If(String.IsNullOrWhiteSpace(code), "", ", ") & itm.Code
							Lable &= If(String.IsNullOrWhiteSpace(Lable), "", ", ") & itm.CodeName
						Next
						Result.CivilState = code
						Result.CivilStateLable = Lable

					Else
						Result.CivilState = code
						Result.CivilStateLable = Lable

					End If

					Dim photoData = LoadAssignedCVLPersonalPhotoViewData(cvlPrifleID)
					Result.PersonalPhoto = photoData.DocBinary

					Result.PersonalTitle = LoadAssignedCVLPersonalTitleViewData(cvlPrifleID, cvlPersonalID)
					Result.PersonalEMail = LoadAssignedCVLPersonalEMailViewData(cvlPrifleID, cvlPersonalID)
					Result.PersonalHomepage = LoadAssignedCVLPersonalHomepageViewData(cvlPrifleID, cvlPersonalID)
					Result.PersonalTelephone = LoadAssignedCVLPersonalTelefonNumberViewData(cvlPrifleID, cvlPersonalID)
					Result.PersonalTelefax = LoadAssignedCVLPersonalTelefaxNumberViewData(cvlPrifleID, cvlPersonalID)
					Result.PersonalAddress = LoadAssignedCVLPersonalAddressViewData(cvlPrifleID, cvlPersonalID)

					listOfSearchResultDTO = Result
				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))

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


#Region "Helpers"

		Private Function LoadAssignedCVLPersonalAddressViewData(ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer) As CVLAddressDTO
			Dim result As CVLAddressDTO = Nothing

			Dim sql = "[Load Assigned CVL Personal Address Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			Try
				If reader IsNot Nothing Then

					result = New CVLAddressDTO

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.Street = SafeGetString(reader, "Street")
						result.Postcode = SafeGetString(reader, "Postcode")
						result.City = SafeGetString(reader, "City")
						result.Country = SafeGetString(reader, "FK_CountryCode")
						result.CountryLable = SafeGetString(reader, "CountryLable")
						result.State = SafeGetString(reader, "State")

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalAddressViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))
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

		Private Function LoadAssignedCVLPersonalNationalityViewData(ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer?) As IEnumerable(Of CodeNameData)
			Dim result As List(Of CodeNameData) = Nothing

			Dim sql = "[Load Assigned CVL Personal Nationality Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, 0)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1} >>> {2}", sql, cvlPrifleID, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameData)

					While reader.Read
						Dim data = New CodeNameData

						data.Code = SafeGetString(reader, "FK_NationalityCode")
						data.CodeName = SafeGetString(reader, "NationalityCodeLable")


						result.Add(data)

					End While


				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalNationalityViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalCivilstateViewData(ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer) As IEnumerable(Of CodeNameData)
			Dim result As List(Of CodeNameData) = Nothing

			Dim sql = "[Load Assigned CVL Personal CivilState Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1} >>> {2}", sql, cvlPrifleID, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CodeNameData)

					While reader.Read
						Dim data = New CodeNameData

						data.Code = SafeGetString(reader, "FK_CivilStateCode")
						data.CodeName = SafeGetString(reader, "CivilStateCodeLable")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalCivilstateViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalPhotoViewData(ByVal cvlPrifleID As Integer) As DocumentData
			Dim result As DocumentData = Nothing

			Dim sql = "[Load Assigned CVL Personal Photo]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1}", sql, cvlPrifleID))

			Try
				If reader IsNot Nothing Then

					result = New DocumentData

					While reader.Read

						result.ID = SafeGetInteger(reader, "ID", Nothing)
						result.DocClass = SafeGetString(reader, "DocClass")
						result.Pages = SafeGetInteger(reader, "Pages", Nothing)
						result.Plaintext = SafeGetString(reader, "Plaintext")
						result.FileType = SafeGetString(reader, "FileType")
						result.DocBinary = SafeGetByteArray(reader, "DocBinary")
						result.DocID = SafeGetInteger(reader, "DocID", Nothing)
						result.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						result.DocLanguage = SafeGetString(reader, "DocLanguage")
						result.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						result.DocXML = SafeGetString(reader, "DocXML")

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalPhotoViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2}{0}{3}", vbNewLine, sql, cvlPrifleID, msgContent))

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

		Private Function LoadAssignedCVLPersonalTitleViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As List(Of CVLListsDTO)
			Dim result As List(Of CVLListsDTO) = Nothing

			Dim sql = "[Load Assigned CVL Personal Title Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1}", sql, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLListsDTO)

					While reader.Read
						Dim data = New CVLListsDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "Title")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalTitleViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2}{0}{3}", vbNewLine, sql, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalEMailViewData(ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer) As List(Of CVLListsDTO)
			Dim result As List(Of CVLListsDTO) = Nothing

			Dim sql = "[Load Assigned CVL Personal EMail Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1} >>> {2}", sql, cvlPrifleID, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLListsDTO)

					While reader.Read
						Dim data = New CVLListsDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "EMailAddress")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalEMailViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalHomepageViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As List(Of CVLListsDTO)
			Dim result As List(Of CVLListsDTO) = Nothing

			Dim sql = "[Load Assigned CVL Personal Homepage Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1}", sql, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLListsDTO)

					While reader.Read
						Dim data = New CVLListsDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "Homepage")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalHomepageViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2}{0}{3}", vbNewLine, sql, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalTelefonNumberViewData(ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer) As List(Of CVLListsDTO)
			Dim result As List(Of CVLListsDTO) = Nothing

			Dim sql = "[Load Assigned CVL Personal Telefonnumber Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1} >>> {2}", sql, cvlPrifleID, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLListsDTO)

					While reader.Read
						Dim data = New CVLListsDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "PhoneNumber")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalTelefonNumberViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2} >>> {3}{0}{4}", vbNewLine, sql, cvlPrifleID, cvlPersonalID, msgContent))

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

		Private Function LoadAssignedCVLPersonalTelefaxNumberViewData(ByVal cvlPrifleID As Integer, ByVal cvlPersonalID As Integer) As List(Of CVLListsDTO)
			Dim result As List(Of CVLListsDTO) = Nothing

			Dim sql = "[Load Assigned CVL Personal Telefaxnumber Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@PersonalID", ReplaceMissing(cvlPersonalID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			m_Logger.LogInfo(String.Format("{0}: {1}", sql, cvlPersonalID))

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLListsDTO)

					While reader.Read
						Dim data = New CVLListsDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.PersonalID = SafeGetInteger(reader, "FK_PersonalID", Nothing)
						data.Lable = SafeGetString(reader, "TelefaxNumber")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalTelefaxNumberViewData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}: {2}{0}{3}", vbNewLine, sql, cvlPersonalID, msgContent))

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
