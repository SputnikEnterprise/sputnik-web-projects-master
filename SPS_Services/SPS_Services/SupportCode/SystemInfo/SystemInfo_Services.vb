
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo


	Partial Class systeminfoDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ISystemInfoDatabaseAccess


		Function LoadCustomerServices() As IEnumerable(Of CustomerSearchResultDTO) Implements ISystemInfoDatabaseAccess.LoadCustomerServices
			Dim listOfSearchResultDTO As List(Of CustomerSearchResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Get Customer for Payment Data]"

			Dim reader = OpenReader(sql, Nothing, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CustomerSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CustomerSearchResultDTO

						data.customer_ID = SafeGetString(reader, "Customer_ID")
						data.customer_Name = SafeGetString(reader, "Customer_Name", String.Empty)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerServices", .MessageContent = msgContent})
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

		Function LoadCustomerDeniedServices(ByVal customerID As String) As List(Of String) Implements ISystemInfoDatabaseAccess.LoadCustomerDeniedServices
			Dim listOfSearchResultDTO As List(Of String) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Load Customer Denied Services Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", customerID))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of String)

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim serviceName = SafeGetString(reader, "DeniedServiceName", String.Empty).Split(New String() {",", ";", "|", "#"}, StringSplitOptions.RemoveEmptyEntries).ToList()

					For Each itm In serviceName
						listOfSearchResultDTO.Add(itm.ToLower)
					Next

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerDeniedServices", .MessageContent = msgContent})
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

		Function LoadCustomerServicesAdvisor(ByVal customerID As String) As IEnumerable(Of CustomerUserNameSearchResultDTO) Implements ISystemInfoDatabaseAccess.LoadCustomerServicesAdvisor
			Dim listOfSearchResultDTO As List(Of CustomerUserNameSearchResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Customer UserName for Payment Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerGuid", customerID))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CustomerUserNameSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CustomerUserNameSearchResultDTO

						data.UserName = SafeGetString(reader, "UserName")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCustomerServicesAdvisor", .MessageContent = msgContent})
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

		Function LoadCurrentServicesData(ByVal customerID As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As IEnumerable(Of PaymentSearchResultDTO) Implements ISystemInfoDatabaseAccess.LoadCurrentServicesData
			Dim listOfSearchResultDTO As List(Of PaymentSearchResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			If serviceName = "SOLVENCY_QUICK_CHECK" OrElse serviceName = "SOLVENCY_BUSINESS_CHECK" Then
				sql = "[Get Search DeltaVista Payment Data]"

			ElseIf serviceName = "CVLIZER_SCAN" Then
				sql = "[Get Search CVLizer Data]"

			Else
				sql = "[Get Search Payment Data]"

			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerGuid", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("userName", ReplaceMissing(userName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("serviceName", ReplaceMissing(serviceName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("serviceDate", ReplaceMissing(serviceDate, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchMonth, Now.Month)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of PaymentSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PaymentSearchResultDTO

						data.RecID = SafeGetInteger(reader, "ID", 0)
						data.CustomerGuid = SafeGetString(reader, "Customer_Guid")
						data.UserGuid = SafeGetString(reader, "User_Guid")
						data.ServiceName = SafeGetString(reader, "ServiceName")
						data.ServiceDate = SafeGetDateTime(reader, "ServiceDate", Nothing)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.AuthorizedItems = SafeGetDecimal(reader, "AuthorizedItems", 0)
						data.AuthorizedCredit = SafeGetDecimal(reader, "AuthorizedCredit", 0)
						data.JobID = SafeGetString(reader, "JobID")
						data.Validated = SafeGetBoolean(reader, "Validated", False)
						data.BookedPayment = SafeGetBoolean(reader, "Fakturiert", False)
						data.BookedDate = SafeGetDateTime(reader, "Fak_Date", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCurrentServicesData", .MessageContent = msgContent})
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

		Function LoadPaidDataServices(ByVal customerID As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As IEnumerable(Of PaidSearchResultDTO) Implements ISystemInfoDatabaseAccess.LoadPaidDataServices
			Dim listOfSearchResultDTO As List(Of PaidSearchResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			If serviceName = "SOLVENCY_QUICK_CHECK" Or serviceName = "SOLVENCY_BUSINESS_CHECK" Then
				sql = "[Get Paid DeltaVista Service Data]"
			Else
				sql = "[Get Paid Service Data]"

			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)
			If String.IsNullOrWhiteSpace(serviceDate) Then
				If searchYear = 0 Then searchYear = Now.Year
				'If searchMonth = 0 Then searchMonth = Now.Month
			End If
			listOfParams.Add(New SqlClient.SqlParameter("customerGuid", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("userName", ReplaceMissing(userName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("serviceName", ReplaceMissing(serviceName, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("serviceDate", ReplaceMissing(serviceDate, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("jahr", ReplaceMissing(searchYear, Now.Year)))
			listOfParams.Add(New SqlClient.SqlParameter("monat", ReplaceMissing(searchMonth, Now.Month)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of PaidSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PaidSearchResultDTO

						data.RecID = SafeGetInteger(reader, "ID", 0)
						data.CustomerGuid = SafeGetString(reader, "Customer_Guid")
						data.ServiceDate = SafeGetDateTime(reader, "ServiceDate", Nothing)
						data.Content = SafeGetString(reader, "Content")
						data.Recipient = SafeGetString(reader, "Recipient")
						data.Sender = SafeGetString(reader, "Sender")
						data.Status = SafeGetInteger(reader, "Status", 0)
						data.ServiceName = SafeGetString(reader, "ServiceName")
						data.UserData = SafeGetString(reader, "UserData")
						data.AuthorizedItems = SafeGetDecimal(reader, "AuthorizedItems", 0)
						data.BookedPayment = SafeGetBoolean(reader, "Fakturiert", False)
						data.BookedDate = SafeGetDateTime(reader, "Fak_Date", Nothing)
						data.ResultCode = SafeGetInteger(reader, "ResultCode", 0)
						data.ResultMessage = SafeGetString(reader, "ResultMessage")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPaidDataServices", .MessageContent = msgContent})
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

		Function LoadECallDataForAssignedJob(ByVal customerID As String, ByVal JobGuid As String) As IEnumerable(Of PaidSearchResultDTO) Implements ISystemInfoDatabaseAccess.LoadECallDataForAssignedJob
			Dim listOfSearchResultDTO As List(Of PaidSearchResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String

			sql = "[Get Paid Service Data For Selected JobGuid]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerGuid", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("JobGuid", ReplaceMissing(JobGuid, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of PaidSearchResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PaidSearchResultDTO

						data.RecID = SafeGetInteger(reader, "ID", 0)
						data.CustomerGuid = SafeGetString(reader, "Customer_Guid")
						data.ServiceDate = SafeGetDateTime(reader, "ServiceDate", Nothing)
						data.Content = SafeGetString(reader, "Content")
						data.Recipient = SafeGetString(reader, "Recipient")
						data.Sender = SafeGetString(reader, "Sender")
						data.Status = SafeGetInteger(reader, "Status", 0)
						data.ServiceName = SafeGetString(reader, "ServiceName")
						data.UserData = SafeGetString(reader, "UserData")
						data.AuthorizedItems = SafeGetDecimal(reader, "AuthorizedItems", 0)
						data.BookedPayment = SafeGetBoolean(reader, "Fakturiert", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadECallDataForAssignedJob", .MessageContent = msgContent})
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
