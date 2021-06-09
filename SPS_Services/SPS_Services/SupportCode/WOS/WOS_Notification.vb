
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace WOSInfo


	Partial Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess

		Function LoadWOSEMailNotificationsData(ByVal customerWosGuid As String, ByVal year As Integer?, ByVal month As Integer?) As IEnumerable(Of WOSNotificationDTO) Implements IWOSDatabaseAccess.LoadWOSEMailNotificationsData
			Dim listOfSearchResultDTO As List(Of WOSNotificationDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerWosGuid

			Dim sql As String
			sql = "[List WOS Notification Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("jahr", ReplaceMissing(year, 0)))
			listOfParams.Add(New SqlParameter("monat", ReplaceMissing(month, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of WOSNotificationDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New WOSNotificationDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.MailFrom = SafeGetString(reader, "MailFrom", String.Empty)
						data.MailTo = SafeGetString(reader, "MailTo", String.Empty)
						data.Result = SafeGetString(reader, "Result")
						data.MailSubject = SafeGetString(reader, "Subject", String.Empty)
						data.MailBody = SafeGetString(reader, "MailBody")
						data.DocLink = SafeGetString(reader, "DocLink", String.Empty)
						data.RecipientGuid = SafeGetString(reader, "Recipient_Guid", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadWOSEMailNotificationsData", .MessageContent = msgContent})
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

		Function LoadMailNotificationData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of EMailNotificationDTO) Implements IWOSDatabaseAccess.LoadMailNotificationData
			Dim listOfSearchResultDTO As List(Of EMailNotificationDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[List Mail Notification Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("assignedDate", ReplaceMissing(assignedDate, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of EMailNotificationDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New EMailNotificationDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.CustomerName = SafeGetString(reader, "Customer_Name", String.Empty)
						data.CustomerLocation = SafeGetString(reader, "Customer_Ort", String.Empty)
						data.MailFrom = SafeGetString(reader, "MailFrom", String.Empty)
						data.MailTo = SafeGetString(reader, "MailTo", String.Empty)
						data.Result = SafeGetString(reader, "Result")
						data.MailSubject = SafeGetString(reader, "Subject", String.Empty)
						data.MailBody = SafeGetString(reader, "MailBody")
						data.DocLink = SafeGetString(reader, "DocLink", String.Empty)
						data.RecipientGuid = SafeGetString(reader, "Recipient_Guid", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMailNotificationData", .MessageContent = msgContent})
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

		Function LoadWOSModulEMailNotificationsData(ByVal customerWosGuid As String, ByVal modulNumber As Integer?, ByVal number As Integer?) As IEnumerable(Of WOSNotificationDTO) Implements IWOSDatabaseAccess.LoadWOSModulEMailNotificationsData
			Dim listOfSearchResultDTO As List(Of WOSNotificationDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerWosGuid

			Dim sql As String

			If modulNumber = ModulNumberEnum.INVOICE Then
				sql = "[List Customer WOS Invoice Notification Data]"

			Else
				Return Nothing

			End If

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("modulNumber", ReplaceMissing(modulNumber, 0)))
			listOfParams.Add(New SqlParameter("number", ReplaceMissing(number, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of WOSNotificationDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New WOSNotificationDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.MailFrom = SafeGetString(reader, "MailFrom", String.Empty)
						data.MailTo = SafeGetString(reader, "MailTo", String.Empty)
						data.Result = SafeGetString(reader, "Result")
						data.MailSubject = SafeGetString(reader, "Subject", String.Empty)
						data.MailBody = SafeGetString(reader, "MailBody")
						data.DocLink = SafeGetString(reader, "DocLink", String.Empty)
						data.RecipientGuid = SafeGetString(reader, "Recipient_Guid", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadWOSModulEMailNotificationsData", .MessageContent = msgContent})
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

		Function UpdateWOSCustomerUserData(ByVal customerID As String, ByVal userData As SystemUserData) As Boolean Implements IWOSDatabaseAccess.UpdateWOSCustomerUserData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Try
				Dim sql As String
				sql = "[Update WOS Customer User Data]"

				Dim listOfParams As New List(Of SqlParameter)

				listOfParams.Add(New SqlParameter("LogedUser_Guid", userData.UserGuid))
				listOfParams.Add(New SqlParameter("WOSCustomerID", ReplaceMissing(userData.EmployeeWOSID, DBNull.Value)))
				listOfParams.Add(New SqlParameter("US_Nachname", ReplaceMissing(userData.UserLName, DBNull.Value)))

				listOfParams.Add(New SqlParameter("US_Vorname", ReplaceMissing(userData.UserFName, DBNull.Value)))
				listOfParams.Add(New SqlParameter("US_Telefon", ReplaceMissing(userData.UserMDTelefon, DBNull.Value)))
				listOfParams.Add(New SqlParameter("US_Telefax", ReplaceMissing(userData.UserMDTelefax, DBNull.Value)))
				listOfParams.Add(New SqlParameter("US_eMail", ReplaceMissing(userData.UserMDeMail, DBNull.Value)))
				listOfParams.Add(New SqlParameter("Customer_Name", ReplaceMissing(userData.UserMDName, DBNull.Value)))
				listOfParams.Add(New SqlParameter("User_Initial", ReplaceMissing(userData.UserKST, DBNull.Value)))
				listOfParams.Add(New SqlParameter("User_Sex", ReplaceMissing(userData.UserSalutation, DBNull.Value)))
				listOfParams.Add(New SqlParameter("User_Filiale", ReplaceMissing(userData.UserBranchOffice, DBNull.Value)))


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateWOSCustomerUserData", .MessageContent = msgContent})

				Return False
			End Try


			Return True
		End Function

		Function LoadJobplattformCounterData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal jobsCHAccountNumber As Integer, ByVal ostJobAccountNumber As String) As JobplattformCounterDataDTO Implements IWOSDatabaseAccess.LoadJobplattformCounterData
			Dim listOfSearchResultDTO As JobplattformCounterDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Jobplattform Counter Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("CustomerID", m_customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("JobsCHAccountNumber", ReplaceMissing(jobsCHAccountNumber, 0)))
			listOfParams.Add(New SqlParameter("OstJobAccountNumber", ReplaceMissing(ostJobAccountNumber, String.Empty)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New JobplattformCounterDataDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New JobplattformCounterDataDTO

						data.Customer_ID = customerID
						data.WOS_ID = customerWosGuid
						data.OwnCounter = SafeGetInteger(reader, "OwnCounter", 0)
						data.OstJobCounter = SafeGetInteger(reader, "OstJobCounter", 0)
						data.JobsCHCounter = SafeGetInteger(reader, "JobsCHCounter", 0)
						data.JobChannelPriorityCounter = SafeGetInteger(reader, "JobChannelPriorityCounter", 0)


						listOfSearchResultDTO = (data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobplattformCounterData", .MessageContent = msgContent})
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
