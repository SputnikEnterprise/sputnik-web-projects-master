
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.Notification.DataObjects
'Imports wsSPS_Services.DataTransferObject.Notify
Imports wsSPS_Services.SPUtilities


Namespace Notification


	Partial Class NotificationDatabaseAccess
		Inherits DatabaseAccessBase
		Implements INotificationDatabaseAccess

		Function LoadApplicationNotifications(ByVal customerID As String) As IEnumerable(Of ApplicationDataDTO) Implements INotificationDatabaseAccess.LoadApplicationNotifications
			Dim result As List(Of ApplicationDataDTO) = Nothing
			Dim reader As SqlClient.SqlDataReader = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[List New Application For Notifications]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of ApplicationDataDTO)

					While reader.Read
						Dim data = New ApplicationDataDTO

						data.ID = m_utility.SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = m_utility.SafeGetString(reader, "Customer_ID", String.Empty)
						data.FK_ApplicantID = m_utility.SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.VacancyNumber = m_utility.SafeGetInteger(reader, "VacancyNumber", 0)
						data.ApplicationLabel = m_utility.SafeGetString(reader, "ApplicationLabel")
						data.Advisor = m_utility.SafeGetString(reader, "Advisor")
						data.BusinessBranch = m_utility.SafeGetString(reader, "BusinessBranch")
						data.Dismissalperiod = m_utility.SafeGetString(reader, "Dismissalperiod")
						data.Availability = m_utility.SafeGetString(reader, "Availability")
						data.Comment = m_utility.SafeGetString(reader, "Comment")
						data.CreatedOn = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = m_utility.SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = m_utility.SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = m_utility.SafeGetString(reader, "CheckedFrom")
						data.ApplicationLifecycle = m_utility.SafeGetInteger(reader, "ApplicationLifecycle", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadApplicationNotifications", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadApplicantNotifications(ByVal customerID As String) As IEnumerable(Of ApplicantDataDTO) Implements INotificationDatabaseAccess.LoadApplicantNotifications
			Dim result As List(Of ApplicantDataDTO) = Nothing
			Dim reader As SqlClient.SqlDataReader = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[List New Applicant For Notifications]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))

			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of ApplicantDataDTO)

					While reader.Read
						Dim data = New ApplicantDataDTO

						data.ID = m_utility.SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = m_utility.SafeGetString(reader, "Customer_ID", String.Empty)
						data.EmployeeID = m_utility.SafeGetInteger(reader, "EmployeeID", 0)
						data.Lastname = m_utility.SafeGetString(reader, "Lastname")
						data.Firstname = m_utility.SafeGetString(reader, "Firstname")
						data.Gender = m_utility.SafeGetString(reader, "Gender")
						data.Street = m_utility.SafeGetString(reader, "Street")
						data.PostOfficeBox = m_utility.SafeGetString(reader, "PostOfficeBox")
						data.Postcode = m_utility.SafeGetString(reader, "Postcode")
						data.Latitude = m_utility.SafeGetDouble(reader, "Latitude", 0)
						data.Longitude = m_utility.SafeGetDouble(reader, "Longitude", 0)
						data.Location = m_utility.SafeGetString(reader, "Location")
						data.Country = m_utility.SafeGetString(reader, "country")
						data.Nationality = m_utility.SafeGetString(reader, "Nationality")
						data.EMail = m_utility.SafeGetString(reader, "EMail")
						data.Telephone = m_utility.SafeGetString(reader, "Telephone")
						data.MobilePhone = m_utility.SafeGetString(reader, "MobilePhone")
						data.Birthdate = m_utility.SafeGetDateTime(reader, "Birthdate", Nothing)
						data.Permission = m_utility.SafeGetString(reader, "Permission")
						data.Profession = m_utility.SafeGetString(reader, "Profession")
						data.Auto = m_utility.SafeGetString(reader, "Auto", False)
						data.Motorcycle = m_utility.SafeGetString(reader, "Motorcycle", False)
						data.Bicycle = m_utility.SafeGetString(reader, "Bicycle", False)
						data.DrivingLicence1 = m_utility.SafeGetString(reader, "DrivingLicence1")
						data.DrivingLicence2 = m_utility.SafeGetString(reader, "DrivingLicence2")
						data.DrivingLicence3 = m_utility.SafeGetString(reader, "DrivingLicence3")
						data.CivilState = m_utility.SafeGetInteger(reader, "CivilState", 0)
						data.Language = m_utility.SafeGetString(reader, "Language")
						data.LanguageLevel = m_utility.SafeGetInteger(reader, "LanguageLevel", 0)
						data.ApplicantLifecycle = m_utility.SafeGetInteger(reader, "ApplicantLifecycle", 0)
						data.CreatedOn = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = m_utility.SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = m_utility.SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = m_utility.SafeGetString(reader, "CheckedFrom")
						data.CVLProfileID = m_utility.SafeGetInteger(reader, "CVLProfileID", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadApplicantNotifications", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadAssignedApplicationsForApplicant(ByVal customerID As String, ByVal applicantID As Integer) As IEnumerable(Of ApplicationDataDTO) Implements INotificationDatabaseAccess.LoadAssignedApplicationsForApplicant
			Dim result As List(Of ApplicationDataDTO) = Nothing
			Dim reader As SqlClient.SqlDataReader = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[List All Applications For Assigned Applicant]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantID", ReplaceMissing(applicantID, DBNull.Value)))


			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of ApplicationDataDTO)

					While reader.Read
						Dim data = New ApplicationDataDTO

						data.ID = m_utility.SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = m_utility.SafeGetString(reader, "Customer_ID", String.Empty)
						data.FK_ApplicantID = m_utility.SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.VacancyNumber = m_utility.SafeGetInteger(reader, "VacancyNumber", 0)
						data.ApplicationLabel = m_utility.SafeGetString(reader, "ApplicationLabel")
						data.Advisor = m_utility.SafeGetString(reader, "Advisor")
						data.BusinessBranch = m_utility.SafeGetString(reader, "BusinessBranch")
						data.Dismissalperiod = m_utility.SafeGetString(reader, "Dismissalperiod")
						data.Availability = m_utility.SafeGetString(reader, "Availability")
						data.Comment = m_utility.SafeGetString(reader, "Comment")
						data.CreatedOn = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = m_utility.SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = m_utility.SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = m_utility.SafeGetString(reader, "CheckedFrom")
						data.ApplicationLifecycle = m_utility.SafeGetString(reader, "ApplicationLifecycle", 0)


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedApplicationsForApplicant", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function UpdateAssignedApplicationDataAsChecked(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicationNumber As Integer?, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements INotificationDatabaseAccess.UpdateAssignedApplicationDataAsChecked
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Application For Notifications As Checked]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicationNumber", ReplaceMissing(destApplicationNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicantNumber", ReplaceMissing(destApplicantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAssignedApplicationData(ByVal customerID As String, ByVal applicationData As ApplicationDataDTO) As Boolean Implements INotificationDatabaseAccess.UpdateAssignedApplicationData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Assigned Application Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationID", ReplaceMissing(applicationData.ID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Advisor", ReplaceMissing(applicationData.Advisor, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLabel", ReplaceMissing(applicationData.ApplicationLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicationLifecycle", ReplaceMissing(applicationData.ApplicationLifecycle, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("BusinessBranch", ReplaceMissing(applicationData.BusinessBranch, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Comments", ReplaceMissing(applicationData.Comment, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAssignedApplicantData(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements INotificationDatabaseAccess.UpdateAssignedApplicantData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Applicant For Notifications As Checked]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicantNumber", ReplaceMissing(destApplicantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAssignedApplicantWithExistingEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal newExistingEmployeeNumber As Integer) As Boolean Implements INotificationDatabaseAccess.UpdateAssignedApplicantWithExistingEmployeeData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Applicant With Existing Employee Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(employeeID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("newExistingEmployeeNumber", ReplaceMissing(newExistingEmployeeNumber, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateApplicantCVLPersonalWithEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal applicantID As Integer, ByVal applicantData As ApplicantDataDTO) As Boolean Implements INotificationDatabaseAccess.UpdateApplicantCVLPersonalWithEmployeeData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim civilCVLLabel As String = applicantData.CivilStateLabel.ToString().ToUpper
			If String.IsNullOrWhiteSpace(civilCVLLabel) Then
				'applicantData.CivilState = 0
				civilCVLLabel = ""
			Else
				Select Case civilCVLLabel
					Case "V"
						applicantData.CivilState = 1
						civilCVLLabel = "m"

					Case "L"
						applicantData.CivilState = 2
						civilCVLLabel = "s"

					Case "W"
						applicantData.CivilState = 4
						civilCVLLabel = "w"

					Case "G"
						applicantData.CivilState = 5
						civilCVLLabel = "d"

					Case "P"
						applicantData.CivilState = 6
						civilCVLLabel = "p"

					Case Else
						applicantData.CivilState = 0
						civilCVLLabel = ""

				End Select
			End If


			Dim sql As String
			sql = "[Update Applicant And CVL PersonalInformation With Existing Employee Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("EmployeeID", ReplaceMissing(employeeID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(applicantData.CVLProfileID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("lastname", ReplaceMissing(applicantData.Lastname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("firstname", ReplaceMissing(applicantData.Firstname, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("street", ReplaceMissing(applicantData.Street, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("postcode", ReplaceMissing(applicantData.Postcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Latitude", ReplaceMissing(applicantData.Latitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Longitude", ReplaceMissing(applicantData.Longitude, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("location", ReplaceMissing(applicantData.Location, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Countrycode", ReplaceMissing(applicantData.Country, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("birthdate", ReplaceMissing(applicantData.Birthdate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("gender", ReplaceMissing(applicantData.Gender, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("Nationality", ReplaceMissing(applicantData.Nationality, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CivilState", ReplaceMissing(applicantData.CivilState, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("CivilStateLabel", ReplaceMissing(civilCVLLabel, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("EMail", ReplaceMissing(applicantData.EMail, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Telephone", ReplaceMissing(applicantData.Telephone, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Canton", ReplaceMissing(applicantData.Canton, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function

		Function UpdateAllDataForAssignedApplicantData(ByVal customerID As String, ByVal applicantID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements INotificationDatabaseAccess.UpdateAllDataForAssignedApplicantData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update All Applicant Data As Checked]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantID", ReplaceMissing(applicantID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicationNumber", ReplaceMissing(destApplicationNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicantNumber", ReplaceMissing(destApplicantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destDocumentID", ReplaceMissing(destDocumentID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))



			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


		Function LoadAssignedDocumentsForApplicant(ByVal customerID As String, ByVal applicantID As Integer) As IEnumerable(Of ApplicantDocumentDataDTO) Implements INotificationDatabaseAccess.LoadAssignedDocumentsForApplicant
			Dim result As List(Of ApplicantDocumentDataDTO) = Nothing
			Dim reader As SqlClient.SqlDataReader = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[List All Documents For Assigned Applicant]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(m_customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("ApplicantID", ReplaceMissing(applicantID, DBNull.Value)))


			Try
				reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				If reader IsNot Nothing Then

					result = New List(Of ApplicantDocumentDataDTO)

					While reader.Read
						Dim data = New ApplicantDocumentDataDTO

						data.ID = m_utility.SafeGetInteger(reader, "ID", 0)
						data.FK_ApplicantID = m_utility.SafeGetInteger(reader, "FK_ApplicantID", 0)
						data.Type = m_utility.SafeGetInteger(reader, "Type", 0)
						data.Flag = m_utility.SafeGetInteger(reader, "Flag", 0)
						data.Title = m_utility.SafeGetString(reader, "Title")
						data.FileExtension = m_utility.SafeGetString(reader, "FileExtension")
						data.Content = m_utility.SafeGetByteArray(reader, "Content")
						data.Hashvalue = m_utility.SafeGetString(reader, "Hashvalue")
						data.CreatedOn = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = m_utility.SafeGetString(reader, "CreatedFrom")
						data.CheckedOn = m_utility.SafeGetDateTime(reader, "CheckedOn", Nothing)
						data.CheckedFrom = m_utility.SafeGetString(reader, "CheckedFrom")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedDocumentsForApplicant", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function UpdateAssignedDocumentData(ByVal customerID As String, ByVal recordID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean Implements INotificationDatabaseAccess.UpdateAssignedDocumentData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Update Document For Notifications As Checked]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))
			listOfParams.Add(New SqlClient.SqlParameter("recordID", ReplaceMissing(recordID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destDocumentID", ReplaceMissing(destDocumentID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicationNumber", ReplaceMissing(destApplicationNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("destApplicantNumber", ReplaceMissing(destApplicantNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserID", ReplaceMissing(UserID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Checked", ReplaceMissing(checked, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function




	End Class

End Namespace
