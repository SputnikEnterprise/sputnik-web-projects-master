
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer


	Partial Class CVLizerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess

		Function LoadALLCVLProfileData(ByVal assignedDate As DateTime?) As IEnumerable(Of CVLizerProfileDataDTO) Implements ICVLizerDatabaseAccess.LoadALLCVLProfileData
			Dim listOfSearchResultDTO As List(Of CVLizerProfileDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = Nothing

			Dim sql As String
			sql = "[Load ALL CVL Profile Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("assignedDate", ReplaceMissing(assignedDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CVLizerProfileDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CVLizerProfileDataDTO

						data.ProfileID = SafeGetInteger(reader, "ID", Nothing)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.CustomerName = SafeGetString(reader, "CustomerName")
						data.PersonalID = SafeGetInteger(reader, "PersonalID", Nothing)
						data.WorkID = SafeGetInteger(reader, "WorkID", Nothing)
						data.EducationID = SafeGetInteger(reader, "EducationID", Nothing)
						data.AdditionalID = SafeGetInteger(reader, "AdditionalID", Nothing)
						data.ObjectiveID = SafeGetInteger(reader, "ObjectiveID", Nothing)
						data.ApplicationID = SafeGetInteger(reader, "ApplicationID", Nothing)
						data.ApplicantID = SafeGetInteger(reader, "ApplicantID", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.DateOfBirth = SafeGetDateTime(reader, "DateOfBirth", Nothing)
						data.FirstName = SafeGetString(reader, "FirstName")
						data.LastName = SafeGetString(reader, "LastName")
						data.GenderLabel = SafeGetString(reader, "GenderLabel")
						data.ApplicationLabel = SafeGetString(reader, "ApplicationLabel")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = "Admin", .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadALLCVLProfileData", .MessageContent = msgContent})
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


		Function LoadCVLCustomerData(ByVal customerID As String) As IEnumerable(Of CVLizerCustomerDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLCustomerData
			Dim result As List(Of CVLizerCustomerDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load CVL Customer Data]"


			' Input Parameters
			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then

					result = New List(Of CVLizerCustomerDataDTO)

					While reader.Read
						Dim data = New CVLizerCustomerDataDTO

						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.CustomerName = SafeGetString(reader, "CustomerName")
						data.Location = SafeGetString(reader, "Location")
						data.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", 0)
						data.CustomerGroupNumber = SafeGetInteger(reader, "CustomerGroupNumber", 0)


						result.Add(data)

					End While


				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLCustomerData", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("{1}{0}{2}", vbNewLine, sql, msgContent))

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

		Function LoadAssignedCVLProfileData(ByVal customerID As String, ByVal profileID As Integer?, ByVal showAllData As Boolean?) As IEnumerable(Of CVLizerProfileDataDTO) Implements ICVLizerDatabaseAccess.LoadAssignedCVLProfileData
			Dim listOfSearchResultDTO As List(Of CVLizerProfileDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load CVL Profile Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(profileID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CVLizerProfileDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read()

						Dim Result = New CVLizerProfileDataDTO

						Result.ProfileID = SafeGetInteger(reader, "ID", Nothing)
						Result.Customer_ID = SafeGetString(reader, "Customer_ID")
						Result.PersonalID = SafeGetInteger(reader, "PersonalID", Nothing)
						Result.WorkID = SafeGetInteger(reader, "WorkID", Nothing)
						Result.EducationID = SafeGetInteger(reader, "EducationID", Nothing)
						Result.AdditionalID = SafeGetInteger(reader, "AdditionalID", Nothing)
						Result.ObjectiveID = SafeGetInteger(reader, "ObjectiveID", Nothing)
						Result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						Result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						Result.GenderLabel = SafeGetString(reader, "GenderLabel")
						Result.FirstName = SafeGetString(reader, "FirstName")
						Result.LastName = SafeGetString(reader, "LastName")


						listOfSearchResultDTO.Add(Result)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLProfileData", .MessageContent = msgContent})
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
