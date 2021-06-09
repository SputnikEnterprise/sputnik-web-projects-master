
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo

	Partial Class PublicDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPublicDatabaseAccess

		Function LoadJobCHOccupationsData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHOccupationsData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Occupation Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.ID_Parent = SafeGetInteger(reader, "ID_Parent", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHOccupationsData", .MessageContent = msgContent})
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

		Function LoadJobCHFachbereichData(ByVal customerID As String, ByVal parentID As Integer, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHFachbereichData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Fachbereich Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("parentID", ReplaceMissing(parentID, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.ID_Parent = SafeGetInteger(reader, "ID_Parent", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHOccupationsData", .MessageContent = msgContent})
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

		Function LoadJobCHPositionData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHPositionData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Position Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHLanguagesData", .MessageContent = msgContent})
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

		Function LoadJobCHRegionData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHRegionData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Region Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.ID_Parent = SafeGetInteger(reader, "ID_Parent", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHRegionData", .MessageContent = msgContent})
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

		Function LoadJobCHBranchesData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHBranchesData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Branchen Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHBranchesData", .MessageContent = msgContent})
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

		Function LoadJobCHLanguagesData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHLanguagesData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Languages Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHLanguagesData", .MessageContent = msgContent})
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

		Function LoadJobCHLanguageNiveauData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHLanguageNiveauData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Languages Niveau Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHLanguageNiveauData", .MessageContent = msgContent})
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

		Function LoadJobCHBildungNiveauData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadJobCHBildungNiveauData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load JobCH Bildung Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.ID_Parent = SafeGetInteger(reader, "ID_Parent", 0)
						data.RecNr = SafeGetInteger(reader, "RecNr", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobCHBildungNiveauData", .MessageContent = msgContent})
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

		Function LoadAVAMEducationsuData(ByVal customerID As String, ByVal language As String) As IEnumerable(Of VacancyJobCHPeripheryDTO) Implements IPublicDatabaseAccess.LoadAVAMEducationsuData
			Dim listOfSearchResultDTO As List(Of VacancyJobCHPeripheryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim myLanguage As String = ReplaceMissing(language, "DE")
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
			sql = "[Load AVAM Education Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(language, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of VacancyJobCHPeripheryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New VacancyJobCHPeripheryDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.RecNr = SafeGetInteger(reader, "Code", 0)
						data.TranslatedLabel = SafeGetString(reader, "TranslatedLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAVAMEducationsuData", .MessageContent = msgContent})
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
