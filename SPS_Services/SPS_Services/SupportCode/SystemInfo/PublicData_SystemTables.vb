
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo

	Partial Class PublicDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPublicDatabaseAccess

		Function LoadCVLBaseTableData(ByVal customerID As String, ByVal TableKind As String, ByVal Language As String) As IEnumerable(Of CVLBaseDataDTO) Implements IPublicDatabaseAccess.LoadCVLBaseTableData
			Dim listOfSearchResultDTO As List(Of CVLBaseDataDTO) = Nothing
			m_customerID = customerID

			Dim myLanguage As String = ReplaceMissing(Language, "DE")
			Dim selLanguage As String = myLanguage
			Select Case myLanguage.ToLower().TrimEnd()
				Case "deutsch", "de", "d"
					selLanguage = "DE"
				Case "italienisch", "it", "i"
					selLanguage = "IT"
				Case "französisch", "fr", "f"
					selLanguage = "FR"
				Case "englisch", "en", "e"
					selLanguage = "EN"

				Case Else
					selLanguage = "DE"
			End Select

			Dim sql As String

			Select Case TableKind.ToUpper.TrimEnd()
				Case "Country".ToUpper.TrimEnd()
					sql = "[Load ISOCountry Data]"
				Case TableKind = "Skill".ToUpper.TrimEnd()
					sql = "[Load Skill Data]"
				Case TableKind = "Cerfs".ToUpper.TrimEnd()
					sql = "[Load CERFS Data]"
				Case TableKind = "Eduction".ToUpper.TrimEnd()
					sql = "[Load Education Data]"
				Case TableKind = "Experience".ToUpper.TrimEnd()
					sql = "[Load Experience Data]"
				Case TableKind = "ISCED".ToUpper.TrimEnd()
					sql = "[Load ISCED Data]"
				Case TableKind = "Language".ToUpper.TrimEnd()
					sql = "[Load ISOLanguage Data]"
				Case TableKind = "NACE".ToUpper.TrimEnd()
					sql = "[Load NACE Data]"
				Case TableKind = "OperationArea".ToUpper.TrimEnd()
					sql = "[Load OperationArea Data]"
				Case TableKind = "Position".ToUpper.TrimEnd()
					sql = "[Load Position Data]"

				Case Else
					Throw New Exception(String.Format("no tablekind was founded! {0} | {1}", customerID, myLanguage))

			End Select

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CVLBaseDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CVLBaseDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Code = SafeGetString(reader, "Code")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadCVLBaseTableData: TableKind: {0} | Language: {1}", TableKind, Language),
															 .MessageContent = msgContent})

				Return Nothing
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

		Function LoadQualificationData(ByVal customerID As String, ByVal gender As String, ByVal language As String, ByVal qualificationModul As String) As IEnumerable(Of QualificationDTO) Implements IPublicDatabaseAccess.LoadQualificationData
			Dim listOfSearchResultDTO As List(Of QualificationDTO) = Nothing
			m_customerID = customerID

			Dim sql As String

			Dim myLanguage As String = ReplaceMissing(language, "DE")
			Dim selLanguage As String = myLanguage
			Select Case myLanguage.ToLower().TrimEnd()
				Case "deutsch", "de", "d"
					selLanguage = "D"
				Case "italienisch", "it", "i"
					selLanguage = "I"
				Case "französisch", "fr", "f"
					selLanguage = "F"
				Case "englisch", "en", "e"
					selLanguage = "E"

				Case Else
					selLanguage = "D"
			End Select

			If qualificationModul.ToLower = "Seco".ToLower Then
				sql = "[Load All SECO Qualification Data]"

			ElseIf qualificationModul.ToLower = "bgb".ToLower Then
				sql = "[Load BGB Qualification Data]"

			ElseIf qualificationModul.ToLower = "hbb".ToLower Then
				sql = "[Load HBB Qualification Data]"

			ElseIf qualificationModul.ToLower = "jobroom".ToLower Then
				sql = "[Load Job Room Qualification Data]"

			Else
				Return Nothing

			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gender", ReplaceMissing(gender, "m")))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "D")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of QualificationDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New QualificationDTO

						data.Code = SafeGetInteger(reader, "Code", Nothing)
						data.TranslatedValue = SafeGetString(reader, "Bezeichnung")
						data.MeldePflichtig = SafeGetBoolean(reader, "mp", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadQualificationData"),
															 .MessageContent = msgContent})

				Return Nothing
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

		Function LoadALKData(ByVal customerID As String) As IEnumerable(Of ALKResultDTO) Implements IPublicDatabaseAccess.LoadALKData
			Dim listOfSearchResultDTO As List(Of ALKResultDTO) = Nothing
			m_customerID = customerID


			Dim sql As String

			sql = "[Load ALK Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of ALKResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New ALKResultDTO

						data.ALKNumber = SafeGetInteger(reader, "KassenNr", 0)
						data.ALKName = SafeGetString(reader, "KassenName")
						data.POBox = SafeGetString(reader, "Postfach")
						data.Street = SafeGetString(reader, "Strasse")
						data.Postcode = SafeGetInteger(reader, "PLZ", 0)
						data.Location = SafeGetString(reader, "Ort")
						data.Telephone = SafeGetString(reader, "Telefon")
						data.Telefax = SafeGetString(reader, "TeleFax")
						data.EMail = SafeGetString(reader, "EMail")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadALKData"),
															 .MessageContent = msgContent})

				Return Nothing
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

		Function LoadAssignedALKData(ByVal customerID As String, ByVal ALKNumber As Integer?) As ALKResultDTO Implements IPublicDatabaseAccess.LoadAssignedALKData
			Dim listOfSearchResultDTO As ALKResultDTO = Nothing
			m_customerID = customerID


			Dim sql As String

			sql = "[Load assigned ALK Data By ALKNumber]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ALKNumber", ReplaceMissing(ALKNumber, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New ALKResultDTO

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					Dim data = New ALKResultDTO

					data.ALKNumber = SafeGetInteger(reader, "KassenNr", 0)
					data.ALKName = SafeGetString(reader, "KassenName")
					data.POBox = SafeGetString(reader, "Postfach")
					data.Street = SafeGetString(reader, "Strasse")
					data.Postcode = SafeGetInteger(reader, "PLZ", 0)
					data.Location = SafeGetString(reader, "Ort")
					data.Telephone = SafeGetString(reader, "Telefon")
					data.Telefax = SafeGetString(reader, "TeleFax")
					data.EMail = SafeGetString(reader, "EMail")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadAssignedALKData"),
															 .MessageContent = msgContent})

				Return Nothing
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

		Function LoadBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As IEnumerable(Of BankSearchResultDTO) Implements IPublicDatabaseAccess.LoadBankData
			Dim listOfSearchResultDTO As List(Of BankSearchResultDTO) = Nothing
			m_customerID = customerID


			Dim sql As String

			sql = "[Get Bank Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ClearingNumber", ReplaceMissing(clearingNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BankName", ReplaceMissing(bankName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(bankPostcode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(bankLocation, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Swift", ReplaceMissing(swift, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of BankSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New BankSearchResultDTO

						data.ClearingNumber = reader.GetString(reader.GetOrdinal("ClearingNr"))
						data.BankName = reader.GetString(reader.GetOrdinal("BankName"))
						data.Postcode = reader.GetString(reader.GetOrdinal("BankPLZ"))
						data.Location = reader.GetString(reader.GetOrdinal("BankOrt"))
						data.Swift = reader.GetString(reader.GetOrdinal("Swift"))
						data.Telephone = reader.GetString(reader.GetOrdinal("Telefon"))
						data.Telefax = reader.GetString(reader.GetOrdinal("Telefax"))
						data.PostAccount = reader.GetString(reader.GetOrdinal("Postkonto"))


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadBankData"),
															 .MessageContent = msgContent})

				Return Nothing
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

		Function LoadAssignedBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankLocation As String) As BankSearchResultDTO Implements IPublicDatabaseAccess.LoadAssignedBankData
			Dim listOfSearchResultDTO As BankSearchResultDTO = Nothing
			m_customerID = customerID


			Dim sql As String

			sql = "[Get Assinged Bank Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("ClearingNumber", ReplaceMissing(clearingNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("bankName", ReplaceMissing(bankName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Ort", ReplaceMissing(bankLocation, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New BankSearchResultDTO

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					Dim data = New BankSearchResultDTO


					data.ClearingNumber = reader.GetString(reader.GetOrdinal("ClearingNr"))
					data.BankName = reader.GetString(reader.GetOrdinal("BankName"))
					data.Postcode = reader.GetString(reader.GetOrdinal("BankPLZ"))
					data.Location = reader.GetString(reader.GetOrdinal("BankOrt"))
					data.Swift = reader.GetString(reader.GetOrdinal("Swift"))
					data.Telephone = reader.GetString(reader.GetOrdinal("Telefon"))
					data.Telefax = reader.GetString(reader.GetOrdinal("Telefax"))
					data.PostAccount = reader.GetString(reader.GetOrdinal("Postkonto"))


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = String.Format("LoadAssignedBankData"),
															 .MessageContent = msgContent})

				Return Nothing
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


    Function AddUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String, ByVal fieldName As String, ByVal fieldData As String, ByVal createdFrom As String) As Boolean Implements IPublicDatabaseAccess.AddUserFormControlTemplateData
      Dim success As Boolean = True
      m_customerID = customerID

      Dim sql As String

      sql = "[Add User FormControl Template Data]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("TemplateName", ReplaceMissing(templateName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("FieldName", ReplaceMissing(fieldName, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("FieldData", ReplaceMissing(fieldData, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(createdFrom, DBNull.Value)))

      success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


      Return success
    End Function

    Function LoadAssignedUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As IEnumerable(Of UserFormControlTemplateDTO) Implements IPublicDatabaseAccess.LoadAssignedUserFormControlTemplateData
      Dim listOfSearchResultDTO As List(Of UserFormControlTemplateDTO) = Nothing
      m_customerID = customerID


      Dim sql As String

      sql = "[Load Assigned User FormControl Template Data]"

      Dim listOfParams As New List(Of SqlClient.SqlParameter)
      listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, DBNull.Value)))
      listOfParams.Add(New SqlClient.SqlParameter("TemplateName", ReplaceMissing(templateName, DBNull.Value)))

      Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
      listOfSearchResultDTO = New List(Of UserFormControlTemplateDTO)

      Try
        If (Not reader Is Nothing AndAlso reader.Read()) Then
          Dim data = New UserFormControlTemplateDTO

          data.ID = SafeGetInteger(reader, "ID", Nothing)
          data.Customer_ID = SafeGetString(reader, "Customer_ID")
          data.User_ID = SafeGetString(reader, "User_ID")
          data.TemplateName = SafeGetString(reader, "Template_Name")
          data.FieldName = SafeGetString(reader, "Field_Name")
          data.FieldData = SafeGetString(reader, "Field_Data")
          data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
          data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


          listOfSearchResultDTO.Add(data)

        End If


      Catch ex As Exception
        Dim msgContent = ex.ToString
        m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
                               .MessageHeader = String.Format("LoadAssignedUserFormControlTemplateData"),
                               .MessageContent = msgContent})

        Return Nothing
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


  End Class


End Namespace
