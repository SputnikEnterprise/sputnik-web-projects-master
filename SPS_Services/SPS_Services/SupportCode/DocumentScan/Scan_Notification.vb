
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace DocumentScan


	Partial Class ScanDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IScanDatabaseAccess

		Function LoadScanJobsNotificationsData(ByVal customerID As String, ByVal excludeChecked As Boolean?) As IEnumerable(Of ScanDTO) Implements IScanDatabaseAccess.LoadScanJobsNotificationsData
			Dim listOfSearchResultDTO As List(Of ScanDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Scanjobs Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("excludeChecked", ReplaceMissing(excludeCheckInteger, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of ScanDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New ScanDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.ModulNumber = SafeGetInteger(reader, "ModulNumber", 6)
						data.DocumentCategoryNumber = SafeGetInteger(reader, "DocumentCategoryNumber", 0)
						data.FoundedCodeValue = SafeGetString(reader, "FoundedCodeValue", String.Empty)
						data.ImportedFileGuid = SafeGetString(reader, "ImportedFileGuid", String.Empty)

						If SafeGetInteger(reader, "IsValid", 0) = 1 Then
							data.IsValid = True
						Else
							data.IsValid = False
						End If

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadScanJobsNotificationsData", .MessageContent = msgContent})
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

		Function LoadCustomerDataForScanjobList() As IEnumerable(Of MandantData) Implements IScanDatabaseAccess.LoadCustomerDataForScanjobList
			Dim listOfSearchResultDTO As List(Of MandantData) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Get Customer Data For ScanJobs]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			'listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			'listOfParams.Add(New SqlClient.SqlParameter("excludeChecked", ReplaceMissing(excludeCheckInteger, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of MandantData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New MandantData

						data.CustomerID = SafeGetString(reader, "Customer_ID", String.Empty)

						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerScanjobsData", .MessageContent = msgContent})
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
