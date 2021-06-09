
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace WOSInfo


	Partial Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess

		Function LoadWOSAvailableEmployeeData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal qualification As String, ByVal location As String, ByVal canton As String, ByVal brunchOffice As String) As IEnumerable(Of AvailableEmployeeDTO) Implements IWOSDatabaseAccess.LoadWOSAvailableEmployeeData
			Dim listOfSearchResultDTO As List(Of AvailableEmployeeDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[List Available Employee For WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("Beruf", ReplaceMissing(qualification, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Ort", ReplaceMissing(location, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kanton", ReplaceMissing(canton, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Filiale", ReplaceMissing(brunchOffice, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of AvailableEmployeeDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AvailableEmployeeDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.WOS_ID = SafeGetString(reader, "WOS_Guid", String.Empty)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.Advisor_ID = SafeGetString(reader, "Advisor_ID")
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.EmployeeAdvisor = SafeGetString(reader, "Berater", String.Empty)
						data.Firstname = SafeGetString(reader, "MA_Vorname")
						data.Lastname = SafeGetString(reader, "MA_Nachname", String.Empty)
						data.BrunchOffice = SafeGetString(reader, "MA_Filiale")
						data.Canton = SafeGetString(reader, "MA_Kanton", String.Empty)
						data.postcode = SafeGetString(reader, "MA_PLZ", String.Empty)
						data.Location = SafeGetString(reader, "MA_Ort", String.Empty)

						data.HowContact = SafeGetString(reader, "MA_Kontakt", String.Empty)
						data.FirstState = SafeGetString(reader, "MA_State1", String.Empty)
						data.SecondState = SafeGetString(reader, "MA_State2", String.Empty)
						data.Branches = SafeGetString(reader, "Branches", String.Empty)
						data.Qualifications = SafeGetString(reader, "MA_Beruf", String.Empty)
						data.JobProzent = SafeGetString(reader, "JobProzent", String.Empty)
						data.BirthDay = SafeGetDateTime(reader, "MAGebDat", Nothing)
						data.Gender = SafeGetString(reader, "MASex", String.Empty)
						data.DriverLicenses = SafeGetString(reader, "MAFSchein", String.Empty)
						data.Civilstate = SafeGetString(reader, "MAZivil", String.Empty)
						data.AvailableMobility = SafeGetString(reader, "MAAuto", String.Empty)
						data.Nationality = SafeGetString(reader, "MANationality", String.Empty)
						data.Permit = SafeGetString(reader, "Bewillig", String.Empty)
						data.Salutation = SafeGetString(reader, "BriefAnrede", String.Empty)
						data.ExistsTemplate = SafeGetBoolean(reader, "ExistsTemplate", False)

						data.DesiredWagesOld = SafeGetDecimal(reader, "DesiredWagesOld", 0)
						data.DesiredWagesNew = SafeGetDecimal(reader, "DesiredWagesNew", 0)
						data.DesiredWagesInMonth = SafeGetDecimal(reader, "DesiredWagesInMonth", 0)
						data.DesiredWagesInHour = SafeGetDecimal(reader, "DesiredWagesInHour", 0)


						Dim reserveData As New AvailableEmployeeReserveFields With {.EmployeeNumber = data.EmployeeNumber}

						reserveData.Reserve1 = SafeGetString(reader, "MA_Res1", String.Empty)
						reserveData.Reserve2 = SafeGetString(reader, "MA_Res2", String.Empty)
						reserveData.Reserve3 = SafeGetString(reader, "MA_Res3", String.Empty)
						reserveData.Reserve4 = SafeGetString(reader, "MA_Res4", String.Empty)
						reserveData.Reserve5 = SafeGetString(reader, "MA_Res5", String.Empty)
						data.EmployeeReserveFields = reserveData


						data.Transfer_UserID = SafeGetString(reader, "Transfered_User", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "Transfered_On", Nothing)
						data.Transfer_ID = SafeGetString(reader, "Transfered_Guid", String.Empty)
						data.MainLanguage = SafeGetString(reader, "MainLanguage", String.Empty)
						data.SpokenLanguages = SafeGetString(reader, "MA_MSprache", String.Empty)
						data.WritingLanguages = SafeGetString(reader, "MA_SSprache", String.Empty)
						data.Properties = SafeGetString(reader, "MA_Eigenschaft", String.Empty)


						Dim advisorData As New WOSAdvisorData With {.User_ID = data.Advisor_ID}
						advisorData.Lastname = SafeGetString(reader, "User_Nachname", String.Empty)
						advisorData.Firstname = SafeGetString(reader, "User_Vorname", String.Empty)
						advisorData.Telephon = SafeGetString(reader, "User_Telefon", String.Empty)
						advisorData.Telefax = SafeGetString(reader, "User_Telefax", String.Empty)
						advisorData.EMail = SafeGetString(reader, "User_eMail", String.Empty)
						advisorData.UserInitial = SafeGetString(reader, "User_Initial", String.Empty)

						data.EmployeeAdvisorData = advisorData


						Dim CustomerData As New CustomerSettingData With {.MA_Guid = customerWosGuid}
						CustomerData.Customer_Name = SafeGetString(reader, "Customer_Name", String.Empty)
						CustomerData.Customer_Strasse = SafeGetString(reader, "Customer_Strasse", String.Empty)
						CustomerData.Customer_Ort = SafeGetString(reader, "Customer_Ort", String.Empty)
						CustomerData.Customer_Telefon = SafeGetString(reader, "Customer_Telefon", String.Empty)
						CustomerData.Customer_eMail = SafeGetString(reader, "Customer_eMail", String.Empty)

						data.CustomerData = CustomerData


						'data.ExistsTemplate = False


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadWOSAvailableEmployeeData", .MessageContent = msgContent})
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

		Function LoadAssignedAvailableEmployeeData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal employeeNumber As Integer) As AvailableEmployeeDTO Implements IWOSDatabaseAccess.LoadAssignedAvailableEmployeeData
			Dim listOfSearchResultDTO As AvailableEmployeeDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Assigned Available Employee Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New AvailableEmployeeDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AvailableEmployeeDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "WOS_Guid", String.Empty)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.Advisor_ID = SafeGetString(reader, "Advisor_ID")
						data.EmployeeNumber = SafeGetInteger(reader, "MANr", 0)
						data.EmployeeAdvisor = SafeGetString(reader, "Berater", String.Empty)
						data.Firstname = SafeGetString(reader, "MA_Vorname")
						data.Lastname = SafeGetString(reader, "MA_Nachname", String.Empty)
						data.BrunchOffice = SafeGetString(reader, "MA_Filiale")
						data.Canton = SafeGetString(reader, "MA_Kanton", String.Empty)
						data.Location = SafeGetString(reader, "MA_Ort", String.Empty)

						data.HowContact = SafeGetString(reader, "MA_Kontakt", String.Empty)
						data.FirstState = SafeGetString(reader, "MA_State1", String.Empty)
						data.SecondState = SafeGetString(reader, "MA_State2", String.Empty)
						data.Branches = SafeGetString(reader, "Branches", String.Empty)
						data.Qualifications = SafeGetString(reader, "MA_Beruf", String.Empty)
						data.JobProzent = SafeGetString(reader, "JobProzent", String.Empty)
						data.BirthDay = SafeGetDateTime(reader, "MAGebDat", Nothing)
						data.Gender = SafeGetString(reader, "MASex", String.Empty)
						data.DriverLicenses = SafeGetString(reader, "MAFSchein", String.Empty)
						data.Civilstate = SafeGetString(reader, "MAZivil", String.Empty)
						data.AvailableMobility = SafeGetString(reader, "MAAuto", String.Empty)
						data.Nationality = SafeGetString(reader, "MANationality", String.Empty)
						data.Permit = SafeGetString(reader, "Bewillig", String.Empty)
						data.Salutation = SafeGetString(reader, "BriefAnrede", String.Empty)

						Dim reserveData As New AvailableEmployeeReserveFields With {.EmployeeNumber = data.EmployeeNumber}

						reserveData.Reserve1 = SafeGetString(reader, "MA_Res1", String.Empty)
						reserveData.Reserve2 = SafeGetString(reader, "MA_Res2", String.Empty)
						reserveData.Reserve3 = SafeGetString(reader, "MA_Res3", String.Empty)
						reserveData.Reserve4 = SafeGetString(reader, "MA_Res4", String.Empty)
						reserveData.Reserve5 = SafeGetString(reader, "MA_Res5", String.Empty)
						data.EmployeeReserveFields = reserveData


						data.Transfer_UserID = SafeGetString(reader, "Transfered_User", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "Transfered_On", Nothing)
						data.Transfer_ID = SafeGetString(reader, "Transfered_Guid", String.Empty)
						data.MainLanguage = SafeGetString(reader, "MainLanguage", String.Empty)
						data.SpokenLanguages = SafeGetString(reader, "MA_MSprache", String.Empty)
						data.WritingLanguages = SafeGetString(reader, "MA_SSprache", String.Empty)
						data.Properties = SafeGetString(reader, "MA_Eigenschaft", String.Empty)
						data.ExistsTemplate = SafeGetBoolean(reader, "ExistsTemplate", False)


						data.DesiredWagesOld = SafeGetDecimal(reader, "DesiredWagesOld", 0)
						data.DesiredWagesNew = SafeGetDecimal(reader, "DesiredWagesNew", 0)
						data.DesiredWagesInMonth = SafeGetDecimal(reader, "DesiredWagesInMonth", 0)
						data.DesiredWagesInHour = SafeGetDecimal(reader, "DesiredWagesInHour", 0)



						Dim advisorData As New WOSAdvisorData With {.User_ID = data.Advisor_ID}
						advisorData.Lastname = SafeGetString(reader, "User_Nachname", String.Empty)
						advisorData.Firstname = SafeGetString(reader, "User_Vorname", String.Empty)
						advisorData.Telephon = SafeGetString(reader, "User_Telefon", String.Empty)
						advisorData.Telefax = SafeGetString(reader, "User_Telefax", String.Empty)
						advisorData.EMail = SafeGetString(reader, "User_eMail", String.Empty)

						data.EmployeeAdvisorData = advisorData


						Dim CustomerData As New CustomerSettingData With {.MA_Guid = customerWosGuid}
						CustomerData.Customer_Name = SafeGetString(reader, "Customer_Name", String.Empty)
						CustomerData.Customer_Strasse = SafeGetString(reader, "Customer_Strasse", String.Empty)
						CustomerData.Customer_Ort = SafeGetString(reader, "Customer_Ort", String.Empty)
						CustomerData.Customer_Telefon = SafeGetString(reader, "Customer_Telefon", String.Empty)
						CustomerData.Customer_eMail = SafeGetString(reader, "Customer_eMail", String.Empty)


						data.CustomerData = CustomerData


						listOfSearchResultDTO = data

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeData", .MessageContent = msgContent})
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

		Function LoadAssignedAvailableEmployeeApplicationData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal employeeNumber As Integer) As AvailableEmployeeApplicationFields Implements IWOSDatabaseAccess.LoadAssignedAvailableEmployeeApplicationData
			Dim listOfSearchResultDTO As AvailableEmployeeApplicationFields = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Assigned Available Employee Application Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New AvailableEmployeeApplicationFields

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AvailableEmployeeApplicationFields

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.EmployeeNumber = SafeGetInteger(reader, "employeeNr", 0)
						'data.LL_Name = SafeGetString(reader, "LL_Name", String.Empty)
						data.ApplicationReserve0 = SafeGetString(reader, "ApplicationReserve0")
						data.ApplicationReserve1 = SafeGetString(reader, "ApplicationReserve1")
						data.ApplicationReserve2 = SafeGetString(reader, "ApplicationReserve2")
						data.ApplicationReserve3 = SafeGetString(reader, "ApplicationReserve3")
						data.ApplicationReserve4 = SafeGetString(reader, "ApplicationReserve4")
						data.ApplicationReserve5 = SafeGetString(reader, "ApplicationReserve5")
						data.ApplicationReserve6 = SafeGetString(reader, "ApplicationReserve6")
						data.ApplicationReserve7 = SafeGetString(reader, "ApplicationReserve7")
						data.ApplicationReserve8 = SafeGetString(reader, "ApplicationReserve8")
						data.ApplicationReserve9 = SafeGetString(reader, "ApplicationReserve9")
						data.ApplicationReserve10 = SafeGetString(reader, "ApplicationReserve10")
						data.ApplicationReserve11 = SafeGetString(reader, "ApplicationReserve11")
						data.ApplicationReserve12 = SafeGetString(reader, "ApplicationReserve12")
						data.ApplicationReserve13 = SafeGetString(reader, "ApplicationReserve13")
						data.ApplicationReserve14 = SafeGetString(reader, "ApplicationReserve14")
						data.ApplicationReserve15 = SafeGetString(reader, "ApplicationReserve15")

						data.ApplicationReserveRtf0 = SafeGetString(reader, "ApplicationReserveRtf0")
						data.ApplicationReserveRtf1 = SafeGetString(reader, "ApplicationReserveRtf1")
						data.ApplicationReserveRtf2 = SafeGetString(reader, "ApplicationReserveRtf2")
						data.ApplicationReserveRtf3 = SafeGetString(reader, "ApplicationReserveRtf3")
						data.ApplicationReserveRtf4 = SafeGetString(reader, "ApplicationReserveRtf4")
						data.ApplicationReserveRtf5 = SafeGetString(reader, "ApplicationReserveRtf5")
						data.ApplicationReserveRtf6 = SafeGetString(reader, "ApplicationReserveRtf6")
						data.ApplicationReserveRtf7 = SafeGetString(reader, "ApplicationReserveRtf7")
						data.ApplicationReserveRtf8 = SafeGetString(reader, "ApplicationReserveRtf8")
						data.ApplicationReserveRtf9 = SafeGetString(reader, "ApplicationReserveRtf9")
						data.ApplicationReserveRtf10 = SafeGetString(reader, "ApplicationReserveRtf10")
						data.ApplicationReserveRtf11 = SafeGetString(reader, "ApplicationReserveRtf11")
						data.ApplicationReserveRtf12 = SafeGetString(reader, "ApplicationReserveRtf12")
						data.ApplicationReserveRtf13 = SafeGetString(reader, "ApplicationReserveRtf13")
						data.ApplicationReserveRtf14 = SafeGetString(reader, "ApplicationReserveRtf14")
						data.ApplicationReserveRtf15 = SafeGetString(reader, "ApplicationReserveRtf15")


						listOfSearchResultDTO = data

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeApplicationData", .MessageContent = msgContent})
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

		Function AddWOSAvailableCandidatesData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult Implements IWOSDatabaseAccess.AddWOSAvailableCandidatesData
			Dim result As WebServiceResult = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Create Assigned Available Employee For WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(employeeData.WOS_ID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Employee_Advisor", ReplaceMissing(employeeData.EmployeeAdvisor, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Employee_Filiale", ReplaceMissing(employeeData.BrunchOffice, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Employee_Canton", ReplaceMissing(employeeData.Canton, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Postcode", ReplaceMissing(employeeData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Location", ReplaceMissing(employeeData.Location, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Firstname", ReplaceMissing(employeeData.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Lastname", ReplaceMissing(employeeData.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlParameter("HowContact", ReplaceMissing(employeeData.HowContact, DBNull.Value)))
			listOfParams.Add(New SqlParameter("FirstState", ReplaceMissing(employeeData.FirstState, DBNull.Value)))
			listOfParams.Add(New SqlParameter("SecondState", ReplaceMissing(employeeData.SecondState, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Qualification", ReplaceMissing(employeeData.Qualifications, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Branches", ReplaceMissing(employeeData.Branches, DBNull.Value)))
			listOfParams.Add(New SqlParameter("JobProzent", ReplaceMissing(employeeData.JobProzent, DBNull.Value)))
			listOfParams.Add(New SqlParameter("BirthDate", ReplaceMissing(employeeData.BirthDay, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Gender", ReplaceMissing(employeeData.Gender, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Civilstate", ReplaceMissing(employeeData.Civilstate, DBNull.Value)))
			listOfParams.Add(New SqlParameter("FSchein", ReplaceMissing(employeeData.DriverLicenses, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Auto", ReplaceMissing(employeeData.AvailableMobility, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Nationality", ReplaceMissing(employeeData.Nationality, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Permission", ReplaceMissing(employeeData.Permit, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Salutation", ReplaceMissing(employeeData.Salutation, DBNull.Value)))

			listOfParams.Add(New SqlParameter("MA_Res1", ReplaceMissing(employeeData.EmployeeReserveFields.Reserve1, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_Res2", ReplaceMissing(employeeData.EmployeeReserveFields.Reserve2, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_Res3", ReplaceMissing(employeeData.EmployeeReserveFields.Reserve3, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_Res4", ReplaceMissing(employeeData.EmployeeReserveFields.Reserve4, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_Res5", ReplaceMissing(employeeData.EmployeeReserveFields.Reserve5, DBNull.Value)))

			listOfParams.Add(New SqlParameter("LL_Name", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.LL_Name, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve0", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve0, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve1", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve1, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve2", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve2, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve3", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve3, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve4", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve4, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve5", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve5, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve6", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve6, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve7", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve7, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve8", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve8, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve9", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve9, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve10", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve10, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve11", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve11, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve12", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve12, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve13", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve13, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve14", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve14, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserve15", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserve15, DBNull.Value)))

			listOfParams.Add(New SqlParameter("ApplicationReserveRtf0", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf0, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf1", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf1, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf2", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf2, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf3", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf3, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf4", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf4, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf5", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf5, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf6", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf6, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf7", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf7, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf8", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf8, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf9", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf9, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf10", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf10, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf11", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf11, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf12", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf12, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf13", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf13, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf14", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf14, DBNull.Value)))
			listOfParams.Add(New SqlParameter("ApplicationReserveRtf15", ReplaceMissing(employeeData.EmployeeApplicationReserveFields.ApplicationReserveRtf15, DBNull.Value)))


			listOfParams.Add(New SqlParameter("MainLanguage", ReplaceMissing(employeeData.MainLanguage, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_MLanguage", ReplaceMissing(employeeData.SpokenLanguages, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_SLanguage", ReplaceMissing(employeeData.WritingLanguages, DBNull.Value)))
			listOfParams.Add(New SqlParameter("MA_Eigenschaft", ReplaceMissing(employeeData.Properties, DBNull.Value)))
			listOfParams.Add(New SqlParameter("@Transfer_UserID", ReplaceMissing(employeeData.Transfer_UserID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Transfered_Guid", ReplaceMissing(employeeData.Transfer_ID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Advisor_ID", ReplaceMissing(employeeData.Advisor_ID, DBNull.Value)))

			listOfParams.Add(New SqlParameter("DesiredWagesOld", ReplaceMissing(employeeData.DesiredWagesOld, DBNull.Value)))
			listOfParams.Add(New SqlParameter("DesiredWagesNew", ReplaceMissing(employeeData.DesiredWagesNew, DBNull.Value)))
			listOfParams.Add(New SqlParameter("DesiredWagesInMonth", ReplaceMissing(employeeData.DesiredWagesInMonth, DBNull.Value)))
			listOfParams.Add(New SqlParameter("DesiredWagesInHour", ReplaceMissing(employeeData.DesiredWagesInHour, DBNull.Value)))


			Try
				Dim success As Boolean = True

				' New ID of kandidaten_online
				Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso
				Not newIdParameter.Value Is Nothing Then
					employeeData.ID = CType(newIdParameter.Value, Integer)
				Else
					success = False
				End If
				result = New WebServiceResult With {.JobResult = success, .JobResultMessage = String.Empty}
				If Not employeeData.AvailableEmployeeTemplates Is Nothing Then
					'For Each docItem In employeeData.AvailableEmployeeTemplates
					success = success AndAlso AddWOSAvailableCandidatesTemplateData(customerID, wosID, employeeData, employeeData.AvailableEmployeeTemplates).JobResult
					'Next
				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddWOSAvailableCandidatesData", .MessageContent = msgContent})

				result = New WebServiceResult With {.JobResult = False, .JobResultMessage = msgContent}

			Finally

			End Try

			Return result
		End Function

		Private Function AddWOSAvailableCandidatesTemplateData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO, ByVal docItem As AvailableEmployeeTemplateData) As WebServiceResult
			Dim result As WebServiceResult = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Create Assigned Available Employee Template For WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(employeeData.WOS_ID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeData.EmployeeNumber, DBNull.Value)))

			listOfParams.Add(New SqlParameter("ScanDoc", ReplaceMissing(docItem.ScanDoc, DBNull.Value)))
			listOfParams.Add(New SqlParameter("CreatedFrom", ReplaceMissing(employeeData.EmployeeNumber, DBNull.Value)))


			Try
				Dim success As Boolean = True

				' New ID of kandidaten_online
				Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso Not newIdParameter.Value Is Nothing Then
					success = True
				Else
					success = False
				End If
				result = New WebServiceResult With {.JobResult = success, .JobResultMessage = String.Empty}

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddWOSAvailableCandidatesTemplateData", .MessageContent = msgContent})

				result = New WebServiceResult With {.JobResult = False, .JobResultMessage = msgContent}

			Finally

			End Try

			Return result
		End Function

		Function RemoveWOSAvailableCandidatesData(ByVal customerID As String, ByVal wosID As String, ByVal employeeData As AvailableEmployeeNewDTO) As WebServiceResult Implements IWOSDatabaseAccess.RemoveWOSAvailableCandidatesData
			Dim result As WebServiceResult = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Delete Assigned Available Employee From WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(employeeData.WOS_ID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeData.EmployeeNumber, DBNull.Value)))
			listOfParams.Add(New SqlParameter("UserID", ReplaceMissing(employeeData.Transfer_UserID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Transfered_Guid", ReplaceMissing(employeeData.Transfer_ID, DBNull.Value)))


			Try
				Dim success As Boolean = True

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				result = New WebServiceResult With {.JobResult = success, .JobResultMessage = String.Empty}

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddWOSAvailableCandidatesData", .MessageContent = msgContent})

				result = New WebServiceResult With {.JobResult = False, .JobResultMessage = msgContent}

			Finally

			End Try

			Return result
		End Function


		Function AddWOSEmployeeDocumentData(ByVal customerID As String, ByVal employeeWosGuid As String, ByVal employeeWosData As EmployeeWOSData) As Boolean Implements IWOSDatabaseAccess.AddWOSEmployeeDocumentData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Try
				Dim sql As String
				sql = "[Create New Employee Document For WOS]"

				' Extract parameter data from data row.
				Dim maNr As Integer? = employeeWosData.EmployeeNumber
				Dim ESNr As Integer? = employeeWosData.EmploymentNumber
				Dim ESLohnNr As Integer? = employeeWosData.EmploymentLineNumber
				Dim PayrollNr As Integer? = employeeWosData.PayrollNumber
				Dim RPNr As Integer? = employeeWosData.ReportNumber
				Dim RPLNr As Integer? = employeeWosData.ReportLineNumber
				Dim RPDocNr As Integer? = employeeWosData.ReportDocNumber

				Dim LogedUser_Guid As String = employeeWosData.LogedUserID
				Dim Customer_ID As String = customerID
				Dim wosGuid As String = employeeWosGuid

				Dim MA_Vorname As String = employeeWosData.MA_Vorname
				Dim MA_Nachname As String = employeeWosData.MA_Nachname
				Dim MA_Filiale As String = employeeWosData.MA_Filiale

				Dim MA_Postfach As String = employeeWosData.MA_Postfach
				Dim MA_Strasse As String = employeeWosData.MA_Strasse
				Dim MA_PLZ As String = employeeWosData.MA_PLZ
				Dim MA_Ort As String = employeeWosData.MA_Ort
				Dim MA_AGB_Wos As String = employeeWosData.MA_AGB_Wos
				Dim MASex As String = employeeWosData.MA_Gender
				Dim MA_Briefanrede As String = employeeWosData.MA_BriefAnrede

				Dim MA_EMail As String = employeeWosData.MA_Email
				Dim myArray = MA_EMail.Split("#"c)
				MA_EMail = String.Join(",", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim MA_Guid As String = employeeWosData.MATransferedGuid
				Dim Doc_Guid As String = employeeWosData.AssignedDocumentGuid
				Dim Doc_Art As String = employeeWosData.AssignedDocumentArtName
				Dim Doc_Info As String = employeeWosData.AssignedDocumentInfo
				Dim Result As String = String.Empty
				Dim MA_Berater As String = employeeWosData.MA_Berater
				Dim MA_Zivil As String = employeeWosData.MA_Zivil
				Dim MA_Nationality As String = employeeWosData.MA_Nationality

				Dim MA_Beruf As String = employeeWosData.MA_Beruf
				myArray = MA_Beruf.Split("#"c)
				MA_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim MA_Branche As String = employeeWosData.MA_Branche
				myArray = MA_Branche.Split("#"c)
				MA_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim MA_GebDat As DateTime? = employeeWosData.MA_GebDat
				Dim TransferedUser As String = String.Empty

				Dim US_Nachname As String = employeeWosData.UserName
				Dim US_Vorname As String = employeeWosData.UserVorname
				Dim US_Telefon As String = employeeWosData.UserTelefon
				Dim US_Telefax As String = employeeWosData.UserTelefax
				Dim US_eMail As String = employeeWosData.UserMail

				Dim MD_Telefon As String = employeeWosData.MDTelefon
				Dim MD_DTelefon As String = employeeWosData.MD_DTelefon
				Dim MD_Telefax As String = employeeWosData.MDTelefax
				Dim MD_eMail As String = employeeWosData.MDMail

				If String.IsNullOrWhiteSpace(MD_Telefon) Then MD_Telefon = US_Telefon
				If String.IsNullOrWhiteSpace(MD_DTelefon) Then MD_DTelefon = US_Telefon
				If String.IsNullOrWhiteSpace(MD_Telefax) Then MD_Telefax = US_Telefax
				If String.IsNullOrWhiteSpace(MD_eMail) Then MD_eMail = US_eMail


				Dim MA_Language As String = employeeWosData.MA_Language

				Dim MA_FSchein As String = employeeWosData.MA_FSchein
				Dim MA_Auto As String = employeeWosData.MA_Auto
				Dim MA_Kontakt As String = employeeWosData.MA_Kontakt
				Dim MA_State1 As String = employeeWosData.MA_State1
				Dim MA_State2 As String = employeeWosData.MA_State2
				Dim MA_Eigenschaft As String = employeeWosData.MA_Eigenschaft
				Dim MA_SSprache As String = employeeWosData.MA_SSprache
				Dim MA_MSprache As String = employeeWosData.MA_MSprache
				Dim AHV_Nr As String = employeeWosData.AHV_Nr
				Dim MA_Canton As String = employeeWosData.MA_Canton


				Dim DocFilename As String = employeeWosData.ScanDocName
				Dim DocScan As Byte() = employeeWosData.ScanDoc

				Dim User_Initial As String = employeeWosData.UserInitial
				Dim User_Sex As String = employeeWosData.UserSex
				Dim User_Filiale As String = employeeWosData.UserFiliale
				Dim UserSign As Byte() = employeeWosData.UserSign
				Dim UserPicture As Byte() = employeeWosData.UserPicture


				Dim listOfParams As New List(Of SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(Customer_ID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", ReplaceMissing(wosGuid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(maNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(ESNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(ESLohnNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("LONr", ReplaceMissing(PayrollNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(RPNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("RPLNr", ReplaceMissing(RPLNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("RPDocNr", ReplaceMissing(RPDocNr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("LogedUser_Guid", ReplaceMissing(LogedUser_Guid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Berater", ReplaceMissing(MA_Berater, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("MA_Vorname", ReplaceMissing(MA_Vorname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Nachname", ReplaceMissing(MA_Nachname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Filiale", ReplaceMissing(MA_Filiale, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Postfach", ReplaceMissing(MA_Postfach, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Strasse", ReplaceMissing(MA_Strasse, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_PLZ", ReplaceMissing(MA_PLZ, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Ort", ReplaceMissing(MA_Ort, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MASex", ReplaceMissing(MASex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MAZivil", ReplaceMissing(MA_Zivil, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Briefanrede", ReplaceMissing(MA_Briefanrede, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("AGB_WOS", ReplaceMissing(MA_AGB_Wos, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Beruf", ReplaceMissing(MA_Beruf, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Branche", ReplaceMissing(MA_Branche, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_EMail", ReplaceMissing(MA_EMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_GebDat", ReplaceMissing(MA_GebDat, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Language", ReplaceMissing(MA_Language, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Nationality", ReplaceMissing(MA_Nationality, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Transfered_User", ReplaceMissing(TransferedUser, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Owner_Guid", ReplaceMissing(MA_Guid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("MA_FSchein", ReplaceMissing(MA_FSchein, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Auto", ReplaceMissing(MA_Auto, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Kontakt", ReplaceMissing(MA_Kontakt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_State1", ReplaceMissing(MA_State1, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_State2", ReplaceMissing(MA_State2, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Eigenschaft", ReplaceMissing(MA_Eigenschaft, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_SSprache", ReplaceMissing(MA_SSprache, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_MSprache", ReplaceMissing(MA_MSprache, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("AHV_Nr", ReplaceMissing(AHV_Nr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MA_Canton", ReplaceMissing(MA_Canton, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(Result, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("US_Nachname", ReplaceMissing(US_Nachname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Vorname", ReplaceMissing(US_Vorname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Telefon", ReplaceMissing(MD_Telefon, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Telefax", ReplaceMissing(MD_Telefax, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_eMail", ReplaceMissing(MD_eMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(DocFilename, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(DocScan, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(User_Initial, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Sex", ReplaceMissing(User_Sex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(User_Filiale, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(UserPicture, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(UserSign, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("SignTransferedDocument", ReplaceMissing(employeeWosData.SignTransferedDocument.GetValueOrDefault(False), False)))

				' New ID of ZG
				Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso
					Not newIdParameter.Value Is Nothing Then
					employeeWosData.ID = CType(newIdParameter.Value, Integer)
				Else
					success = False
				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddWOSEmployeeDocumentData", .MessageContent = msgContent})

				Return False
			End Try


			Return success
		End Function

		Function LoadWOSAvailableEmployeeDocumentData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal employeeNumber As Integer) As IEnumerable(Of AvailableEmployeeTemplateData) Implements IWOSDatabaseAccess.LoadWOSAvailableEmployeeDocumentData
			Dim listOfSearchResultDTO As List(Of AvailableEmployeeTemplateData) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[List Available Employee Document Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(customerWosGuid, DBNull.Value)))
			listOfParams.Add(New SqlParameter("EmployeeNumber", ReplaceMissing(employeeNumber, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of AvailableEmployeeTemplateData)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New AvailableEmployeeTemplateData

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID", String.Empty)
						data.WOS_ID = SafeGetString(reader, "WOS_Guid", String.Empty)
						data.EmployeeNumber = SafeGetInteger(reader, "EmployeeNr", 0)

						data.CreatedFrom = SafeGetString(reader, "CreatedFrom", String.Empty)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.ScanDoc = SafeGetByteArray(reader, "ScanDoc")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadWOSAvailableEmployeeDocumentData", .MessageContent = msgContent})
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
