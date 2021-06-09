
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.JobPlatform.X28
Imports wsSPS_Services.SPUtilities


Namespace CVLizer '.Profilmatcher


	Partial Class CVLizerDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICVLizerDatabaseAccess

		Function LoadProfilmatcherQueryNotifications(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?) As IEnumerable(Of ProfilMatcherNotificationData) Implements ICVLizerDatabaseAccess.LoadProfilmatcherQueryNotifications
			Dim listOfSearchResultDTO As List(Of ProfilMatcherNotificationData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Profilmatcher Query Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of ProfilMatcherNotificationData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New ProfilMatcherNotificationData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.CustomerID = SafeGetString(reader, "Customer_ID")
						data.UserID = SafeGetString(reader, "User_ID")
						data.QueryName = SafeGetString(reader, "QueryName")
						data.Total = SafeGetInteger(reader, "Size", 0)
						data.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "EmployeeNumber", 0)
						data.Notify = SafeGetBoolean(reader, "Notify", False)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "CVLizer.CVLizerDatabaseAccess",
																.NotifyComments = String.Format("LoadProfilmatcherQueryNotifications"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadProfilmatcherQueryNotifications", .MessageContent = msgContent})
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

		Function LoadProfilmatcherAssignedQueryNotification(ByVal customerID As String, ByVal ID As Integer) As ProfilMatcherNotificationData Implements ICVLizerDatabaseAccess.LoadProfilmatcherAssignedQueryNotification
			Dim listOfSearchResultDTO As ProfilMatcherNotificationData = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Profilmatcher Assigned Query Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(ID, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New ProfilMatcherNotificationData

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ProfilMatcherNotificationData

					data.ID = SafeGetInteger(reader, "ID", 0)
						data.CustomerID = SafeGetString(reader, "Customer_ID")
						data.UserID = SafeGetString(reader, "User_ID")
						data.QueryName = SafeGetString(reader, "QueryName")
						data.QueryContent = SafeGetString(reader, "QueryContent")
						data.QueryResultContent = SafeGetString(reader, "QueryResultContent")
						data.Total = SafeGetInteger(reader, "Size", 0)
						data.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "EmployeeNumber", 0)
						data.Notify = SafeGetBoolean(reader, "Notify", False)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO = data

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "CVLizer.CVLizerDatabaseAccess",
																.NotifyComments = String.Format("LoadProfilmatcherAssignedQueryNotification"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadProfilmatcherAssignedQueryNotification", .MessageContent = msgContent})
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

		Function AddProfilmatcherQueryNotificationData(ByVal customerID As String, ByVal userID As String, ByVal customerNumber As Integer?, ByVal employeeNumber As Integer?, ByVal tplName As String,
																									 ByVal totalRecord As Integer, ByVal notify As Boolean, ByVal createdFrom As String,
																									 ByVal pmQueryContent As String, ByVal pmJobResultContent As String) As Boolean Implements ICVLizerDatabaseAccess.AddProfilmatcherQueryNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Add Profilmatcher Query Notification]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("customerNumber", ReplaceMissing(customerNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("employeeNumber", ReplaceMissing(employeeNumber, 0)))

			listOfParams.Add(New SqlClient.SqlParameter("QueryContent", ReplaceMissing(pmQueryContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("QueryResultContent", ReplaceMissing(pmJobResultContent, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("QueryName", ReplaceMissing(tplName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Size", ReplaceMissing(totalRecord, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Notify", ReplaceMissing(notify, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(createdFrom, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function DeleteProfilmatcherAssignedNotificationData(ByVal customerID As String, ByVal ID As Integer) As Boolean Implements ICVLizerDatabaseAccess.DeleteProfilmatcherAssignedNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Delete Profilmatcher Assigned Query Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ID", ReplaceMissing(ID, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


	End Class


End Namespace
