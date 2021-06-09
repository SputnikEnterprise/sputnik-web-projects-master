
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects


Namespace SystemInfo


	Partial Class systeminfoDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ISystemInfoDatabaseAccess


		Function LoadAdvisorLoginData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of AdvisorLoginData) Implements ISystemInfoDatabaseAccess.LoadAdvisorLoginData
			Dim listOfSearchResultDTO As List(Of AdvisorLoginData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[List Advisor Login By Assigned Date Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("assignedDate", ReplaceMissing(assignedDate, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of AdvisorLoginData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AdvisorLoginData

						data.UserCount = SafeGetInteger(reader, "UserCount", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.CustomerName = SafeGetString(reader, "Customername", String.Empty)
						data.Advisorname = SafeGetString(reader, "Advisor", String.Empty)
						data.LogYear = SafeGetInteger(reader, "logyear", 0)
						data.LogMonth = SafeGetInteger(reader, "logMonth", 0)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvisorLoginData", .MessageContent = msgContent})
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

		Function LoadAdvisorLoginMonthlyData(ByVal customerID As String, ByVal assignedDate As DateTime?) As IEnumerable(Of AdvisorLoginData) Implements ISystemInfoDatabaseAccess.LoadAdvisorLoginMonthlyData
			Dim listOfSearchResultDTO As List(Of AdvisorLoginData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[List Advisor Login Data By Assigned CustomerID And UserID For Month And Year]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("assignedDate", ReplaceMissing(assignedDate, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of AdvisorLoginData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AdvisorLoginData

						data.UserCount = SafeGetInteger(reader, "UserCount", 0)
						data.CustomerName = SafeGetString(reader, "Customername", String.Empty)
						data.Advisorname = SafeGetString(reader, "Advisor", String.Empty)
						data.LogYear = Year(assignedDate)
						data.LogMonth = Month(assignedDate)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvisorLoginMonthlyData", .MessageContent = msgContent})
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

		Function UpdateECallResponseDataForAssignedJob(ByVal customerID As String, ByVal JobGuid As String, ByVal AuthorizedCredit As Decimal?, ByVal AuthorizedItems As Decimal?) As Boolean Implements ISystemInfoDatabaseAccess.UpdateECallResponseDataForAssignedJob
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String

				sql = "UPDATE [spSystemInfo].Dbo.[tblCustomerPayableServices] "
				sql &= "Set Validated = 1"
				sql &= ", UsedPoints = @AuthorizedCredit"
				sql &= ", t2 = @AuthorizedItems"
				sql &= ", ValidatedOn = GetDate() "
				sql &= "Where Customer_Guid = @Customer_Guid "
				sql &= "And JobID = @JobID "


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobID", ReplaceMissing(JobGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("AuthorizedCredit", ReplaceMissing(AuthorizedCredit, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("AuthorizedItems", ReplaceMissing(AuthorizedItems, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateECallResponseDataForAssignedJob", .MessageContent = msgContent})

				Return False
			End Try


			Return True

		End Function

		Function AddSolvencyUsage(ByVal customerID As String, ByVal userGuid As String, ByVal userName As String, ByVal solvencyCheckType As String, ByVal serviceDate As DateTime) As Boolean Implements ISystemInfoDatabaseAccess.AddSolvencyUsage
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String

				sql = "INSERT INTO [spSystemInfo].Dbo.[tblCustomerPayableServices]([Customer_Guid],[User_Guid],[ServiceName],[Servicedate],[CreatedOn],[CreatedFrom], [t2], [Validated], [validatedon]) "
				sql &= "VALUES(@Customer_Guid, @User_Guid, @ServiceName, @Servicedate, GetDate(), @CreatedFrom, 1, 1, GetDate())"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceName", ReplaceMissing(solvencyCheckType, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Servicedate", ReplaceMissing(serviceDate, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(userName, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)



			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddSolvencyUsage", .MessageContent = msgContent})

				Return False
			End Try


			Return True

		End Function

		Function AddECallUsage(ByVal customerID As String, ByVal userGuid As String, ByVal userName As String, ByVal SMSCredit As String, ByVal JobID As String, ByVal UsedPoints As String, ByVal serviceDate As DateTime) As Boolean Implements ISystemInfoDatabaseAccess.AddECallUsage
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String

				sql = "INSERT INTO [spSystemInfo].Dbo.[tblCustomerPayableServices] ([Customer_Guid], [User_Guid], [ServiceName], [JobID], [UsedPoints], [Servicedate], [CreatedOn], [CreatedFrom]) "
				sql &= "VALUES (@Customer_Guid, @User_Guid, @ServiceName, @JobID, @UsedPoints, @Servicedate, GetDate(), @CreatedFrom)"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Guid", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ServiceName", ReplaceMissing(SMSCredit, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobID", ReplaceMissing(JobID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UsedPoints", ReplaceMissing(UsedPoints, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Servicedate", ReplaceMissing(serviceDate, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(userName, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.Text, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddECallUsage", .MessageContent = msgContent})

				Return False
			End Try


			Return True

		End Function

		Function AddSputnikLoginUsage(ByVal customerID As String, ByVal customerName As String, ByVal userGuid As String, ByVal userName As String, ByVal domainUsername As String, ByVal machineName As String, ByVal domainName As String) As Boolean Implements ISystemInfoDatabaseAccess.AddSputnikLoginUsage
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String

				sql = "[Insert Sputnik Login Data]"

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("@Modulname", "LogSputnikUsageInDb"))
				listOfParams.Add(New SqlClient.SqlParameter("@MDGuid", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@MDname", ReplaceMissing(customerName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@UserGuid", ReplaceMissing(userGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Username", ReplaceMissing(userName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@DomainUsername", ReplaceMissing(domainUsername, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Machinename", ReplaceMissing(machineName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("@Domainname", ReplaceMissing(domainName, DBNull.Value)))

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)



			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddSputnikLoginUsage", .MessageContent = msgContent})

				Return False
			End Try


			Return True

		End Function

		Function AddSputnikUserData(ByVal customerID As String, ByVal userData As SystemUserData) As Boolean Implements ISystemInfoDatabaseAccess.AddSputnikUserData
			Dim success As Boolean = True
			m_customerID = customerID

			Try

				Dim sql As String

				sql = "[Insert Sputnik User Data]"

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(userData.Customer_ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(userData.UserGuid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("UserNr", ReplaceMissing(userData.UserNr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("LoginName", ReplaceMissing(userData.UserLoginname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Lastname", ReplaceMissing(userData.UserLName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Firstname", ReplaceMissing(userData.UserFName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Salutation", ReplaceMissing(userData.UserSalutation, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Logindata", ReplaceMissing(userData.UserLoginPassword, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Postoffice", ReplaceMissing(userData.UserMDPostfach, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Street", ReplaceMissing(userData.UserMDStrasse, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Postcode", ReplaceMissing(userData.UserMDPLZ, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Location", ReplaceMissing(userData.UserMDOrt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Country", ReplaceMissing(userData.UserMDLand, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Telephone", ReplaceMissing(userData.UserMDTelefon, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Mobile", ReplaceMissing(userData.UserMobile, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(userData.UserMDeMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Birthdate", ReplaceMissing(userData.Birthdate, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Language", ReplaceMissing(userData.UserLanguage, "deutsch")))
				listOfParams.Add(New SqlClient.SqlParameter("Shortcut", ReplaceMissing(userData.UserKST, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Branchoffice", ReplaceMissing(userData.UserBranchOffice, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Firsttitle", ReplaceMissing(userData.UserFTitel, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Secondtitle", ReplaceMissing(userData.UserSTitel, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EMail_UserName", ReplaceMissing(userData.EMail_UserName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EMail_UserPW", ReplaceMissing(userData.EMail_UserPW, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EMail_SMTP", ReplaceMissing(userData.EMail_SMTP, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("jch_layoutID", ReplaceMissing(userData.jch_layoutID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("jch_logoID", ReplaceMissing(userData.jch_logoID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("OstJob_ID", ReplaceMissing(userData.OstJob_ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ostjob_Kontingent", ReplaceMissing(userData.ostjob_Kontingent, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JCH_SubID", ReplaceMissing(userData.JCH_SubID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Deactivated", ReplaceMissing(userData.Deactivated, False)))

				listOfParams.Add(New SqlClient.SqlParameter("AsCostCenter", ReplaceMissing(userData.AsCostCenter, False)))
				listOfParams.Add(New SqlClient.SqlParameter("LogonMorePlaces", ReplaceMissing(userData.LogonMorePlaces, False)))

				listOfParams.Add(New SqlClient.SqlParameter("CreatedFrom", ReplaceMissing(userData.CreatedFrom, DBNull.Value)))


				success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddSputnikUserData", .MessageContent = msgContent})

				Return False
			End Try


			Return success

		End Function

		Function AllowedStationToUpdate(ByVal stationData As StationData) As Boolean Implements ISystemInfoDatabaseAccess.AllowedStationToUpdate
			Dim success As Boolean = True
			m_customerID = String.Empty

			Dim sql As String
			sql = "[Station IS Allowed For Update]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("LocalIPAddress", ReplaceMissing(stationData.LocalIPAddress, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LocalHostName", ReplaceMissing(stationData.LocalHostName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LocalUserName", ReplaceMissing(stationData.LocalUserName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LocalDomainName", ReplaceMissing(stationData.LocalDomainName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ExternalIPAddress", ReplaceMissing(stationData.ExternalIPAddress, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then

					success = SafeGetBoolean(reader, "Allowed", False)

				End If
				Dim msgContent = String.Format("LocalIPAddress: {0} | LocalHostName: {1} | LocalUserName: {2} | LocalDomainName: {3} | ExternalIPAddress: {4} | Allowed: {5}",
																			 stationData.LocalIPAddress, stationData.LocalHostName, stationData.LocalUserName, stationData.LocalDomainName, stationData.ExternalIPAddress, success)
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "UpdateInfo.UpdateInfoDatabaseAccess.AllowedStationToUpdate",
																.NotifyComments = msgContent, .NotifyArt = SPUtilities.NotifyArtEnum.STATIONUPDATE, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AllowedStationToUpdate", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return success
		End Function

		Function AllowedCustomerToUpdate(ByVal customerdata As CustomerMDData) As Boolean Implements ISystemInfoDatabaseAccess.AllowedCustomerToUpdate
			Dim success As Boolean = True
			m_customerID = customerdata.CustomerID


			Dim sql As String
			sql = "[Customer IS Allowed For Update]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("LocalIPAddress", ReplaceMissing(customerdata.LocalIPAddress, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LocalHostName", ReplaceMissing(customerdata.LocalHostName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("LocalUserName", String.Empty))
			listOfParams.Add(New SqlClient.SqlParameter("LocalDomainName", ReplaceMissing(customerdata.LocalDomainName, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ExternalIPAddress", ReplaceMissing(customerdata.ExternalIPAddress, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then

					success = SafeGetBoolean(reader, "Allowed", False)

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AllowedCustomerToUpdate", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return success
		End Function

		Function SendNotificationForNewFileToSputnik(ByVal customerdata As CustomerMDData) As Boolean Implements ISystemInfoDatabaseAccess.SendNotificationForNewFileToSputnik
			Dim success As Boolean = True
			m_customerID = customerdata.CustomerID

			Try
				Dim sql As String
				sql = "[Send Notification For New Files To Sputnik]"

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("LocalIPAddress", ReplaceMissing(customerdata.LocalIPAddress, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("LocalHostName", ReplaceMissing(customerdata.LocalHostName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("LocalUserName", String.Empty))
				listOfParams.Add(New SqlClient.SqlParameter("LocalDomainName", ReplaceMissing(customerdata.LocalDomainName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ExternalIPAddress", ReplaceMissing(customerdata.ExternalIPAddress, DBNull.Value)))


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)



			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "SendNotificationForNewFileToSputnik", .MessageContent = msgContent})
			Finally
			End Try

			Return success
		End Function

	End Class


End Namespace
