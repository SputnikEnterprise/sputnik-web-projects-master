
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace DocumentScan


	Partial Class ScanDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IScanDatabaseAccess

		Function AddAssignedScanJob(ByVal customerID As String, ByVal scanID As String) As ScanDTO Implements IScanDatabaseAccess.AddAssignedScanJob
			Dim listOfSearchResultDTO As ScanDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Assigned ScanJobs]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanID", ReplaceMissing(scanID, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New ScanDTO

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ScanDTO

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = SafeGetString(reader, "Customer_ID")
					data.FoundedCodeValue = SafeGetString(reader, "FoundedCodeValue")
					data.ModulNumber = SafeGetInteger(reader, "ModulNumber", 0)
					data.RecordNumber = SafeGetInteger(reader, "RecordNumber", 0)
					data.DocumentCategoryNumber = SafeGetInteger(reader, "DocumentCategoryNumber", 0)
					If SafeGetInteger(reader, "IsValid", 0) = 1 Then
						data.IsValid = True
					Else
						data.IsValid = False
					End If
					data.ReportYear = SafeGetInteger(reader, "ReportYear", Nothing)
					data.ReportMonth = SafeGetInteger(reader, "ReportMonth", Nothing)
					data.ReportWeek = SafeGetInteger(reader, "ReportWeek", Nothing)
					data.ReportFirstDay = SafeGetInteger(reader, "ReportFirstDay", Nothing)
					data.ReportLastDay = SafeGetInteger(reader, "ReportLastDay", Nothing)
					data.ReportLineID = SafeGetInteger(reader, "ReportLineID", Nothing)
					data.ScanContent = SafeGetByteArray(reader, "ScanContent")
					data.ImportedFileGuid = SafeGetString(reader, "ImportedFileGuid")
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
					data.CheckedFrom = SafeGetString(reader, "CheckedFrom")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAssignedScanJob", .MessageContent = msgContent})
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

		Function LoadScanJobData(ByVal customerID As String, ByVal scanID As Integer?, ByVal assignedDate As DateTime?) As IEnumerable(Of ScanAttachmentDTO) Implements IScanDatabaseAccess.LoadScanJobData
			Dim listOfSearchResultDTO As List(Of ScanAttachmentDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load ALL Scanjob Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanID", ReplaceMissing(scanID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("assignedDate", ReplaceMissing(assignedDate, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					listOfSearchResultDTO = New List(Of ScanAttachmentDTO)

					While reader.Read
						Dim data = New ScanAttachmentDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.CustomerName = SafeGetString(reader, "CustomerName")
						data.BusinessBranchNumber = SafeGetInteger(reader, "BusinessBranchNumber", 0)
						data.ModulNumber = SafeGetInteger(reader, "ModulNumber", 0)
						data.DocumentCategoryNumber = SafeGetInteger(reader, "DocumentCategoryNumber", 0)
						data.ScanContent = SafeGetByteArray(reader, "ScanContent")
						data.ImportedFileGuid = SafeGetString(reader, "ImportedFileGuid")

						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = SafeGetString(reader, "CheckedFrom")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadScanJobData", .MessageContent = msgContent})
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

		Function LoadAssignedScanJobData(ByVal customerID As String, ByVal scanID As Integer?) As ScanAttachmentDTO Implements IScanDatabaseAccess.LoadAssignedScanJobData
			Dim listOfSearchResultDTO As ScanAttachmentDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Assigned Scanjob Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanID", ReplaceMissing(scanID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				listOfSearchResultDTO = New ScanAttachmentDTO

				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ScanAttachmentDTO

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = SafeGetString(reader, "Customer_ID")
					data.CustomerName = SafeGetString(reader, "CustomerName")
					data.BusinessBranchNumber = SafeGetInteger(reader, "BusinessBranchNumber", 0)
					data.ModulNumber = SafeGetInteger(reader, "ModulNumber", 0)
					data.DocumentCategoryNumber = SafeGetInteger(reader, "DocumentCategoryNumber", 0)
					data.ScanContent = SafeGetByteArray(reader, "ScanContent")
					data.ImportedFileGuid = SafeGetString(reader, "ImportedFileGuid")

					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
					data.CheckedFrom = SafeGetString(reader, "CheckedFrom")


					listOfSearchResultDTO = (data)

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedScanJobData", .MessageContent = msgContent})
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

		Function UpdateAssignedScanJob(ByVal customerID As String, ByVal scanID As String, ByVal userData As String) As Boolean Implements IScanDatabaseAccess.UpdateAssignedScanJob
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Update Assigned ScanJobs]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanID", ReplaceMissing(scanID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddReportDropInJob(ByVal customerID As String, ByVal scanData As ScanDropInDTO) As Boolean Implements IScanDatabaseAccess.AddReportDropInJob
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Create New Report ScanJob DropIn]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(scanData.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(scanData.ModulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocumentCategoryNumber", ReplaceMissing(scanData.DocumentCategoryNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Content", ReplaceMissing(scanData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(scanData.FileExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanFileName", ReplaceMissing(scanData.ScanFileName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(scanData.CreatedFrom, DBNull.Value)))

			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCVDropInJob(ByVal customerID As String, ByVal UserData As SystemUserData, ByVal scanData As ScanDropInDTO) As Boolean Implements IScanDatabaseAccess.AddCVDropInJob
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Create New CV ScanJob DropIn]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(scanData.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ModulNumber", ReplaceMissing(scanData.ModulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("DocumentCategoryNumber", ReplaceMissing(scanData.DocumentCategoryNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Content", ReplaceMissing(scanData.ScanContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("FileExtension", ReplaceMissing(scanData.FileExtension, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ScanFileName", ReplaceMissing(scanData.ScanFileName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(scanData.CreatedFrom, DBNull.Value)))

			success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


#Region "Helpers"




#End Region

	End Class

End Namespace
