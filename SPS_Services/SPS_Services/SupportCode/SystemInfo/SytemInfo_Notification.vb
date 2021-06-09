
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo


	Partial Class systeminfoDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ISystemInfoDatabaseAccess

		Function LoadPrividerData(ByVal customerID As String, ByVal providerName As String) As ProviderDataDTO Implements ISystemInfoDatabaseAccess.LoadPrividerData
			Dim listOfSearchResultDTO As ProviderDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Provider Login Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("ProviderName", ReplaceMissing(providerName, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New ProviderDataDTO

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New ProviderDataDTO

					data.Customer_ID = customerID
					data.ID = SafeGetInteger(reader, "ID", 0)
					data.ProviderName = SafeGetString(reader, "ProviderName")
					data.AccountName = SafeGetString(reader, "AccountName")
					data.UserName = SafeGetString(reader, "UserName")
					data.UserData = SafeGetString(reader, "UserPassword")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPrividerData", .MessageContent = msgContent})
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

		Function LoadJobPlattformCustomerData(ByVal customerID As String, ByVal userGuid As String, ByVal PlattformCustomerNumber As Integer) As JobplattformsCustomerData Implements ISystemInfoDatabaseAccess.LoadJobPlattformCustomerData
			Dim listOfSearchResultDTO As JobplattformsCustomerData = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Jobplattform Customer Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userGuid, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerNumber", ReplaceMissing(PlattformCustomerNumber, 0)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New JobplattformsCustomerData

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New JobplattformsCustomerData

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = SafeGetString(reader, "Customer_ID")
					data.CustomerNumber = SafeGetInteger(reader, "CustomerNumber", 0)
					data.PlattformLabel = SafeGetString(reader, "JobplattformLabel")
					data.CustomerName = SafeGetString(reader, "CustomerName")
					data.Advisorfullname = SafeGetString(reader, "AdvisorFullName")
					data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


					listOfSearchResultDTO = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobsCHCustomerData", .MessageContent = msgContent})
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

		Function LoadNotificationsData(ByVal customerID As String, ByVal notifyArt As NotifyArtEnum, ByVal excludeChecked As Boolean?) As IEnumerable(Of NotifyMessageData) Implements ISystemInfoDatabaseAccess.LoadNotificationsData
			Dim listOfSearchResultDTO As List(Of NotifyMessageData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Assigned Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyArt", ReplaceMissing(notifyArt, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("excludeChecked", ReplaceMissing(excludeCheckInteger, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of NotifyMessageData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New NotifyMessageData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.CustomerID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.NotifyHeader = SafeGetString(reader, "NotifyHeader", String.Empty)
						data.NotifyComments = SafeGetString(reader, "NotifyComments", String.Empty)
						data.NotifyArt = SafeGetInteger(reader, "NotifyArt", 0)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = SafeGetString(reader, "CheckedFrom", String.Empty)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadNotificationsData", .MessageContent = msgContent})
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

		Function LoadTODONotificationsData(ByVal customerID As String) As IEnumerable(Of NotifyMessageData) Implements ISystemInfoDatabaseAccess.LoadTODONotificationsData
			Dim listOfSearchResultDTO As List(Of NotifyMessageData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Assigned Notifications For TODO]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of NotifyMessageData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New NotifyMessageData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.CustomerID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.NotifyHeader = SafeGetString(reader, "NotifyHeader", String.Empty)
						data.NotifyComments = SafeGetString(reader, "NotifyComments", String.Empty)
						data.NotifyArt = SafeGetInteger(reader, "NotifyArt", 0)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = SafeGetString(reader, "CheckedFrom", String.Empty)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTODONotificationsData", .MessageContent = msgContent})
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

		Function LoadCustomerNotificationsData(ByVal userID As String, ByVal excludeChecked As Boolean?) As IEnumerable(Of CustomerNotificationDataDTO) Implements ISystemInfoDatabaseAccess.LoadCustomerNotificationsData
			Dim listOfSearchResultDTO As List(Of CustomerNotificationDataDTO) = Nothing
			m_customerID = userID

			Dim sql As String
			sql = "[List Customer Notifications]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("excludeChecked", ReplaceMissing(excludeChecked, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CustomerNotificationDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CustomerNotificationDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.User_ID = SafeGetString(reader, "User_ID", String.Empty)
						data.NotifyGroup = SafeGetString(reader, "NotifyGroup", String.Empty)
						data.NotifyHeader = SafeGetString(reader, "NotifyHeader", String.Empty)
						data.NotifyComments = SafeGetString(reader, "NotifyComments", String.Empty)
						'data.NotifyArt = SafeGetInteger(reader, "NotifyArt", 0)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.CheckedOn = SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = SafeGetString(reader, "CheckedFrom", String.Empty)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerNotificationsData", .MessageContent = msgContent})
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

		Function LoadCustomerDataForApplicationList() As IEnumerable(Of MandantData) Implements ISystemInfoDatabaseAccess.LoadCustomerDataForApplicationList
			Dim listOfSearchResultDTO As List(Of MandantData) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Get Customer Data For ApplicationJobs]"

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
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerDataForApplicationList", .MessageContent = msgContent})
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

		''' <summary>
		''' Logs the service usage in the database.
		''' </summary>
		Function AddCustomerPayableServiceUsage(ByVal customerID As String, ByVal userData As SystemUserData, ByVal serviceName As String, ByVal serviceArt As String) As Boolean Implements ISystemInfoDatabaseAccess.AddCustomerPayableServiceUsage
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String
				sql = "INSERT INTO [spSystemInfo].Dbo.[tblCustomerPayableServices] ("
				sql &= "[Customer_Guid]"
				sql &= ",[User_Guid]"
				sql &= ",[ServiceName]"
				sql &= ",[JobID]"
				sql &= ",[Servicedate]"
				sql &= ",[CreatedOn]"
				sql &= ",[CreatedFrom]"
				sql &= ",[t2]"
				sql &= ",[Validated]"
				sql &= ",[validatedon]) "
				sql &= "VALUES ("
				sql &= "@Customer_Guid"
				sql &= ",@User_Guid"
				sql &= ",@ServiceName"
				sql &= ",@ServiceArt"
				sql &= ",GetDate()"
				sql &= ",GetDate()"
				sql &= ",@CreatedFrom"
				sql &= ",1"
				sql &= ",1"
				sql &= ",GetDate()"
				sql &= " )"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, userData.UserMDGuid)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userData.UserGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceName", ReplaceMissing(serviceName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceArt", ReplaceMissing(serviceArt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(userData.UserFullName, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddCustomerPayableServiceUsage", .MessageContent = msgContent})

				Return False
			End Try


			Return True

		End Function

		Function UpdateAssignedNotificationData(ByVal customerID As String, ByVal recordID As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements ISystemInfoDatabaseAccess.UpdateAssignedNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Assigned Notification]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function AddCustomerNotificationData(ByVal customerID As String, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean Implements ISystemInfoDatabaseAccess.AddCustomerNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Add Customer Notification Content]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("notifyHeader", ReplaceMissing(notifyHeader, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyComment", ReplaceMissing(notifyComment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAssignedCustomerNotificatioContentData(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal userData As String) As Boolean Implements ISystemInfoDatabaseAccess.UpdateAssignedCustomerNotificatioContentData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Assigned Customer Notification Content]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyHeader", ReplaceMissing(notifyHeader, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyComment", ReplaceMissing(notifyComment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAssignedCustomerNotificationData(ByVal customerID As String, ByVal recordID As Integer, ByVal notifyHeader As String, ByVal notifyComment As String, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements ISystemInfoDatabaseAccess.UpdateAssignedCustomerNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Assigned Customer Notification]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyHeader", ReplaceMissing(notifyHeader, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("notifyComment", ReplaceMissing(notifyComment, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function



#Region "NLog entries"

		Function AddUserNLOGNotificationData(ByVal customerID As String, ByVal nlogEntry As NLOGData, ByVal nLogMessage As String) As Boolean Implements ISystemInfoDatabaseAccess.AddUserNLOGNotificationData
			Dim success As Boolean = True
			m_customerID = customerID


			Dim sql As String
			sql = "[InsertLog]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("level", ReplaceMissing(nlogEntry.level, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("callSite", ReplaceMissing(nlogEntry.callSite, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("type", ReplaceMissing(nlogEntry.type, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("message", ReplaceMissing(nlogEntry.message, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("stackTrace", ReplaceMissing(nlogEntry.stackTrace, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("innerException", ReplaceMissing(nlogEntry.innerException, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("additionalInfo", ReplaceMissing(nlogEntry.additionalInfo, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

#End Region

	End Class

End Namespace
