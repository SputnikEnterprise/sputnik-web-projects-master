Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo


	Partial Class PublicDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPublicDatabaseAccess


		Function LoadSTMPJobData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData) Implements IPublicDatabaseAccess.LoadSTMPJobData
			Dim listOfSearchResultDTO As List(Of STMPJobData) = Nothing
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
			sql = "[Load STMP Job Data 2018]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobNumber", ReplaceMissing(jobNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of STMPJobData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New STMPJobData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.GroupNumber = SafeGetInteger(reader, "GroupNumber", 0)
						data.TitleNumber = SafeGetInteger(reader, "TitleNumber", 0)
						data.Bez_DE = SafeGetString(reader, "Bez_DE")
						data.Bez_FR = SafeGetString(reader, "Bez_FR")
						data.Bez_IT = SafeGetString(reader, "Bez_IT")
						data.Bez_Translated = SafeGetString(reader, "Bez_Translated")
						data.Group_DE = SafeGetString(reader, "Group_DE")
						data.Group_FR = SafeGetString(reader, "Group_FR")
						data.Group_IT = SafeGetString(reader, "Group_IT")
						data.Group_Translated = SafeGetString(reader, "Group_Translated")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSTMPJobData", .MessageContent = msgContent})
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

		Function LoadSTMPJob2020Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData) Implements IPublicDatabaseAccess.LoadSTMPJob2020Data
			Dim listOfSearchResultDTO As List(Of STMPJobData) = Nothing
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
			sql = "[Load STMP Job Data 2020]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobNumber", ReplaceMissing(jobNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of STMPJobData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New STMPJobData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.GroupNumber = SafeGetInteger(reader, "GroupNumber", 0)
						data.TitleNumber = SafeGetInteger(reader, "TitleNumber", 0)
						data.Bez_Translated = SafeGetString(reader, "Bez_Translated")
						data.Group_Translated = SafeGetString(reader, "Group_Translated")
						data.Notifiable = SafeGetBoolean(reader, "Notifiable", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSTMPJob2020Data", .MessageContent = msgContent})
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

		Function LoadSTMPJob2021Data(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPJobData) Implements IPublicDatabaseAccess.LoadSTMPJob2021Data
			Dim listOfSearchResultDTO As List(Of STMPJobData) = Nothing
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
			sql = "[Load STMP Job Data 2021]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobNumber", ReplaceMissing(jobNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of STMPJobData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New STMPJobData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.GroupNumber = SafeGetInteger(reader, "GroupNumber", 0)
						data.TitleNumber = SafeGetInteger(reader, "TitleNumber", 0)
						data.Bez_Translated = SafeGetString(reader, "Bez_Translated")
						data.Group_Translated = SafeGetString(reader, "Group_Translated")
						data.Notifiable = SafeGetBoolean(reader, "Notifiable", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSTMPJob2020Data", .MessageContent = msgContent})
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

		Function LoadSTMPMappingData(ByVal customerID As String, ByVal userID As String, ByVal jobNumber As Integer?, ByVal language As String) As IEnumerable(Of STMPMappingData) Implements IPublicDatabaseAccess.LoadSTMPMappingData
			Dim listOfSearchResultDTO As List(Of STMPMappingData) = Nothing
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
			sql = "[Load STMP Mapping Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("JobNumber", ReplaceMissing(jobNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of STMPMappingData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New STMPMappingData

						data.OLD_AVAMNumber = SafeGetInteger(reader, "OLDNumber", 0)
						data.New_AVAMNumber = SafeGetInteger(reader, "NewNumber", 0)
						data.OLD_Bez_Translated = SafeGetString(reader, "Old_bez_translated")
						data.New_Bez_Translated = SafeGetString(reader, "New_bez_translated")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSTMPMappingData", .MessageContent = msgContent})
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
