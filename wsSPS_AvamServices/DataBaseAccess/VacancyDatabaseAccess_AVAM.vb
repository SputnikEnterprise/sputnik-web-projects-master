Imports System.Data.SqlClient
Imports wsSPS_AvamServices.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_AvamServices.SPUtilities


Namespace JobPlatform.AVAM

	Partial Class AVAMDatabaseAccess
		Inherits DatabaseAccessBase
		Implements iVacancyDatabaseAccess



		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal jobroomID As String, ByVal resultContent As String, ByVal syncFrom As String) Implements iVacancyDatabaseAccess.AddAVAMNotifyResultData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql As String
			sql = "[Add AVAM Query Result]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("customer_ID", customerID))
			listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
			listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(jobroomID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(resultContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(syncFrom, DBNull.Value)))

			Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
			newIdParameter.Direction = ParameterDirection.Output
			listOfParams.Add(newIdParameter)

			Try

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If Not newIdParameter.Value Is Nothing Then
					success = True
				Else
					success = False
				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMNotifyResultData", .MessageContent = msgContent})
			Finally

			End Try

			Return success
		End Function

		'Function LoadAssignedQueryData(ByVal customerID As String, ByVal jobRoomID As String) As SPAVAMQueryResultData Implements iVacancyDatabaseAccess.LoadAssignedQueryData
		'	Dim result As SPAVAMQueryResultData = Nothing
		'	Dim excludeCheckInteger As Integer = 1
		'	m_customerID = customerID

		'	Dim sql = "[Load Assigned AVAM Query Result]"

		'	Dim listOfParams = New List(Of SqlClient.SqlParameter)
		'	listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
		'	listOfParams.Add(New SqlClient.SqlParameter("JobroomID", ReplaceMissing(jobRoomID, DBNull.Value)))

		'	Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
		'	Try
		'		If reader IsNot Nothing Then

		'			result = New SPAVAMQueryResultData

		'			While reader.Read
		'				Dim data = New SPAVAMQueryResultData

		'				data.ID = SafeGetInteger(reader, "ID", 0)
		'				data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
		'				data.User_ID = SafeGetString(reader, "User_ID", 0)
		'				data.Advertisment_ID = SafeGetInteger(reader, "Advertisment_ID", 0)
		'				data.JobroomID = SafeGetString(reader, "JobroomID")
		'				data.ResultContent = SafeGetString(reader, "ResultContent")
		'				data.ReportingObligation = SafeGetBoolean(reader, "ReportingObligation", Nothing)
		'				data.ReportingObligationEndDate = SafeGetDateTime(reader, "ReportingObligationEndDate", Nothing)
		'				data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
		'				data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


		'				result = data

		'			End While

		'		End If

		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedQueryData", .MessageContent = msgContent})
		'	Finally
		'		If Not reader Is Nothing Then
		'			Try
		'				reader.Close()
		'			Catch
		'				' Do nothing
		'			End Try

		'		End If
		'	End Try


		'	Return result
		'End Function

		'Function AddAVAMAdvertismentData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal notify As Boolean, ByVal avamData As SPAVAMJobCreationData) As Boolean Implements iVacancyDatabaseAccess.AddAVAMAdvertismentData
		'	Dim success As Boolean = True
		'	m_customerID = customerID

		'	Dim sql As String
		'	sql = "[Add AVAM Advertisment Query Data]"

		'	Dim listOfParams As New List(Of SqlParameter)

		'	listOfParams.Add(New SqlParameter("customer_ID", customerID))
		'	listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(avamData.JobroomID, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("VacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("QueryContent", ReplaceMissing(avamData.QueryContent, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(avamData.ResultContent, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ReportingObligation", ReplaceMissing(avamData.ReportingObligation, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ReportingObligationEndDate", ReplaceMissing(avamData.reportingObligationEndDate, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("Notify", ReplaceMissing(notify, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(avamData.CreatedFrom, DBNull.Value)))

		'	Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
		'	newIdParameter.Direction = ParameterDirection.Output
		'	listOfParams.Add(newIdParameter)

		'	Try

		'		success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

		'		If Not newIdParameter.Value Is Nothing Then
		'			success = True
		'		Else
		'			success = False
		'		End If


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMAdvertismentData", .MessageContent = msgContent})

		'		success = False
		'	Finally

		'	End Try

		'	Return success
		'End Function

		'Function UpdateAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal avamData As SPAVAMJobCreationData) As Boolean Implements iVacancyDatabaseAccess.UpdateAVAMNotifyResultData
		'	Dim success As Boolean = True
		'	m_customerID = customerID

		'	Dim sql As String
		'	sql = "[Update AVAM Query Result]"

		'	Dim listOfParams As New List(Of SqlParameter)

		'	listOfParams.Add(New SqlParameter("customer_ID", customerID))
		'	listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(avamData.JobroomID, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(avamData.ResultContent, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ReportingObligation", ReplaceMissing(avamData.ReportingObligation, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("ReportingObligationEndDate", ReplaceMissing(avamData.reportingObligationEndDate, DBNull.Value)))
		'	listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(avamData.SyncFrom, DBNull.Value)))

		'	Try

		'		success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


		'	Catch ex As Exception
		'		Dim msgContent = ex.ToString
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAVAMNotifyResultData", .MessageContent = msgContent})

		'		success = False
		'	Finally

		'	End Try

		'	Return success
		'End Function




	End Class


End Namespace
