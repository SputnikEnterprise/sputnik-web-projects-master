
Imports System.Data.SqlClient


Namespace JobPlatform.AVAM

	Partial Class AVAMDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements iVacancyDatabaseAccess


		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal jobroomID As String, ByVal queryContent As String, ByVal resultContent As String,
										 ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?, ByVal Notify As Boolean?, ByVal syncFrom As String) As Boolean Implements iVacancyDatabaseAccess.AddAVAMNotifyResultData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql As String
			sql = "[Add AVAM Advertisment Query Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("customer_ID", customerID))
			listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
			listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(jobroomID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("VacancyNumber", ReplaceMissing(vacancyNumber, DBNull.Value)))
			listOfParams.Add(New SqlParameter("QueryContent", ReplaceMissing(queryContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(resultContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ReportingObligation", ReplaceMissing(ReportingObligation, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ReportingObligationEndDate", ReplaceMissing(ReportingObligationEndDate, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Notify", ReplaceMissing(Notify, DBNull.Value)))
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

		Function AddAVAMQueryResultData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal jobroomID As String, ByVal queryContent As String, ByVal resultContent As String,
										 ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?, ByVal syncFrom As String) As Boolean Implements iVacancyDatabaseAccess.AddAVAMQueryResultData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql As String
			sql = "[Add AVAM Query Result Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("customer_ID", customerID))
			listOfParams.Add(New SqlParameter("User_ID", ReplaceMissing(userid, DBNull.Value)))
			listOfParams.Add(New SqlParameter("JobroomID", ReplaceMissing(jobroomID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("QueryContent", ReplaceMissing(queryContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ResultContent", ReplaceMissing(resultContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ReportingObligation", ReplaceMissing(ReportingObligation, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ReportingObligationEndDate", ReplaceMissing(ReportingObligationEndDate, DBNull.Value)))
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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMQueryResultData", .MessageContent = msgContent})
			Finally

			End Try

			Return success
		End Function


	End Class


End Namespace
