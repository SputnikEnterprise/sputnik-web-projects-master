
Imports System.Data.SqlClient
Imports sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects
Imports sp_WebServiceUtility.JobPlatform


Namespace SystemInfo


	Partial Class SystemInfoDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ISystemInfoDatabaseAccess


		Function AddErrorMessage(ByVal customerID As String, ByVal msgData As ErrorMessageData) As Boolean Implements ISystemInfoDatabaseAccess.AddErrorMessage
			Dim success As Boolean = True

			Dim sql As String
			sql = "Insert Into [spSystemInfo].Dbo.[tbl_ErrorMessage] "
			sql &= "(Customer_ID, SourceModul, MessageHeader, MessageContent, CreatedOn, CreatedFrom "
			sql &= ") Values ("
			sql &= "@CustomerID, @SourceModul, @MessageHeader, @MessageContent, GetDate(), @CreatedFrom)"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlParameter("SourceModul", ReplaceMissing(msgData.SourceModul, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MessageHeader", ReplaceMissing(msgData.MessageHeader, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MessageContent", ReplaceMissing(msgData.MessageContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(msgData.CreatedFrom, DBNull.Value)))


			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

				success = False
			Finally

			End Try

			Return success
		End Function

		Function AddNotifyMessage(ByVal customerID As String, ByVal msgData As ErrorMessageData) As Boolean Implements ISystemInfoDatabaseAccess.AddNotifyMessage
			Dim success As Boolean = True

			Dim sql As String
			sql = "Insert Into [spSystemInfo].Dbo.[tbl_Notify] "
			sql &= "(Customer_ID, NotifyHeader, NotifyComments, NotifyArt, CreatedOn, CreatedFrom"
			sql &= ") Values ("
			sql &= "@CustomerID, @NotifyHeader, @NotifyComments, @NotifyArt, GetDate(), @CreatedFrom)"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlParameter("NotifyHeader", ReplaceMissing(msgData.MessageHeader, DBNull.Value)))
			listOfParams.Add(New SqlParameter("NotifyComments", ReplaceMissing(msgData.MessageContent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("NotifyArt", ReplaceMissing(msgData.MessageArt, DBNull.Value)))
			listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(msgData.CreatedFrom, DBNull.Value)))

			Try
				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString())

				success = False
			Finally

			End Try

			Return success
		End Function


	End Class


End Namespace
