
Imports System.Web.Services
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.Notify
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.CVLizer
Imports wsSPS_Services.Notification
Imports wsSPS_Services.DataTransferObject.Notification.DataObjects
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.Logging



' Wenn der Aufruf dieses Webdiensts aus einem Skript zulässig sein soll, heben Sie mithilfe von ASP.NET AJAX die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPApplication.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPApplication
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPApplication"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_CVLizer As CVLizerDatabaseAccess
	Private m_Notification As NotificationDatabaseAccess
	Private m_PublicData As PublicDataDatabaseAccess



	Public Sub New()

		m_utility = New ClsUtilities
		m_CVLizer = New CVLizerDatabaseAccess(My.Settings.ConnStr_cvLizer, Language.German)
		m_Notification = New NotificationDatabaseAccess(My.Settings.ConnStr_Applicant, Language.German)
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


#Region "upload application, applicant and documents"

	<WebMethod(Description:="List new application data for notification")>
	Function GetApplicationNotifications(ByVal customerID As String) As ApplicationDataDTO()

		Dim result As List(Of ApplicationDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of ApplicationDataDTO)
			result = m_Notification.LoadApplicationNotifications(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetApplicationNotifications", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new applicant-Data for notification")>
	Function GetApplicantNotifications(ByVal customerID As String) As ApplicantDataDTO()

		Dim result As List(Of ApplicantDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID
		If String.IsNullOrWhiteSpace(customerID) Then Return Nothing

		'm_Logger.LogInfo(String.Format("calling GetApplicantNotifications: customerID: {0}", customerID))
		Try
			result = New List(Of ApplicantDataDTO)
			result = m_Notification.LoadApplicantNotifications(customerID)
			If Not result Is Nothing AndAlso result.Count > 0 Then m_Logger.LogInfo(String.Format("customerID: {0} >>> founded items: {1}", customerID, result.Count))


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetApplicantNotifications", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List cvlizer customer data")>
	Function LoadCVLCustomerData(ByVal customerID As String) As CVLizerCustomerDataDTO()
		Dim result As List(Of CVLizerCustomerDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of CVLizerCustomerDataDTO)
			result = m_CVLizer.LoadCVLCustomerData(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLCustomerData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


	<WebMethod(Description:="List new applicant document data for notification")>
	Function GetApplicantDocumentNotifications(ByVal customerID As String) As ApplicantDocumentDataDTO()

		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of ApplicantDocumentDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing
		Dim excludeCheckInteger As Integer = 1

		Try

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[List New Applicant Document For Notifications]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																					cmd.Parameters(i).ParameterName,
																					cmd.Parameters(i).DbType,
																					cmd.Parameters(i).Size,
																					cmd.Parameters(i).Value,
																					ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of ApplicantDocumentDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New ApplicantDocumentDataDTO With {
						.ID = m_utility.SafeGetInteger(reader, "ID", 0),
						.FK_ApplicantID = m_utility.SafeGetInteger(reader, "FK_ApplicantID", 0),
						.Type = m_utility.SafeGetInteger(reader, "Type", 0),
						.Flag = m_utility.SafeGetInteger(reader, "Flag", 0),
						.Title = m_utility.SafeGetString(reader, "Title"),
						.FileExtension = m_utility.SafeGetString(reader, "FileExtension"),
						.Hashvalue = m_utility.SafeGetString(reader, "Hashvalue"),
						.CreatedOn = m_utility.SafeGetDateTime(reader, "CreatedOn", Nothing),
						.CreatedFrom = m_utility.SafeGetString(reader, "CreatedFrom"),
						.CheckedOn = m_utility.SafeGetDateTime(reader, "CheckedOn", Nothing),
						.CheckedFrom = m_utility.SafeGetString(reader, "CheckedFrom")
					}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetApplicantDocumentNotifications", .MessageContent = msgContent})
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
		Return listOfSearchResultDTO.ToArray()
	End Function


	<WebMethod(Description:="List application data for assigned applicant")>
	Function GetAssignedApplicationsForApplicant(ByVal customerID As String, applicantID As Integer) As ApplicationDataDTO()

		Dim result As List(Of ApplicationDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling GetAssignedApplicationsForApplicant: customerID: {0} | applicantID: {1}", customerID, applicantID))
		Try
			result = New List(Of ApplicationDataDTO)
			result = m_Notification.LoadAssignedApplicationsForApplicant(customerID, applicantID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetAssignedApplicationsForApplicant", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="update application notification record as checked")>
	Function UpdateAssignedApplicationAsChecked(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicationNumber As Integer?, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling UpdateAssignedApplicationAsChecked: customerID: {0} | recordID: {1} | destApplicationNumber: {2} | destApplicantNumber: {3} | checked: {4} | UserID: {5} | userData: {6}",
									   customerID, recordID, destApplicationNumber, destApplicantNumber, checked, UserID, userData))
		Try
			result = result AndAlso m_Notification.UpdateAssignedApplicationDataAsChecked(customerID, recordID, destApplicationNumber, destApplicantNumber, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedApplicationAsChecked", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update application notification record as checked")>
	Function UpdateAssignedApplicationData(ByVal customerID As String, ByVal applicationData As ApplicationDataDTO) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("Calling UpdateAssignedApplicationData: customerID: {0}", customerID))
		Try
			result = result AndAlso m_Notification.UpdateAssignedApplicationData(customerID, applicationData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedApplicationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update applicant notification record as checked")>
	Function UpdateAssignedApplicantAsChecked(ByVal customerID As String, ByVal recordID As Integer, ByVal destApplicantNumber As Integer?, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling UpdateAssignedApplicantAsChecked: customerID: {0} | recordID: {1} | destApplicantNumber: {2} | checked: {3} | UserID: {4} | userData: {5}",
									   customerID, recordID, destApplicantNumber, checked, UserID, userData))
		Try
			result = result AndAlso m_Notification.UpdateAssignedApplicantData(customerID, recordID, destApplicantNumber, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedApplicantAsChecked", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update applicant record with employee data")>
	Function UpdateAssignedApplicantWithExistingEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal existingEmployeeNumber As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_Notification.UpdateAssignedApplicantWithExistingEmployeeData(customerID, employeeID, existingEmployeeNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedApplicantWithExistingEmployeeData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update applicant record data")>
	Function UpdateAssignedApplicantCVLDataWithEmployeeData(ByVal customerID As String, ByVal employeeID As Integer, ByVal applicantID As Integer, ByVal applicantData As ApplicantDataDTO) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID
		If applicantData.CVLProfileID.GetValueOrDefault(0) = 0 Then Return True

		Try
			result = result AndAlso m_Notification.UpdateApplicantCVLPersonalWithEmployeeData(customerID, employeeID, applicantID, applicantData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedApplicantCVLDataWithEmployeeData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="update all application, applicant and document records as checked")>
	Function UpdateAllDataForAssignedApplicantAsChecked(ByVal customerID As String, ByVal applicantID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_Notification.UpdateAllDataForAssignedApplicantData(customerID, applicantID, destDocumentID, destApplicationNumber, destApplicantNumber, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAllDataForAssignedApplicantAsChecked", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List document data for assigned applicant")>
	Function GetAssignedDocumentsForApplicant(ByVal customerID As String, applicantID As Integer) As ApplicantDocumentDataDTO()
		Dim result As List(Of ApplicantDocumentDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("Calling GetAssignedDocumentsForApplicant: customerID: {0} | applicantID: {1}", customerID, applicantID))
		Try
			result = New List(Of ApplicantDocumentDataDTO)
			result = m_Notification.LoadAssignedDocumentsForApplicant(customerID, applicantID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetAssignedDocumentsForApplicant", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="update application document notification record as checked")>
	Function UpdateAssignedDocumentAsChecked(ByVal customerID As String, ByVal recordID As Integer, ByVal destDocumentID As Integer, ByVal destApplicationNumber As Integer, ByVal destApplicantNumber As Integer, ByVal checked As Boolean, ByVal UserID As String, ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_Notification.UpdateAssignedDocumentData(customerID, recordID, destDocumentID, destApplicationNumber, destApplicantNumber, checked, UserID, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedDocumentAsChecked", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


#End Region


#Region "for deleting..."

#Region "personal information data"

	Private Function LoadCVLPersonalInformation(ByVal profileID As Integer, ByVal personalID As Integer) As PersonalInformationDataDTO
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As PersonalInformationDataDTO = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New PersonalInformationDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PersonalInformationDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_CVLID = m_utility.SafeGetInteger(reader, "FK_CVLID", 0),
											.FirstName = m_utility.SafeGetString(reader, "FirstName"),
											.LastName = m_utility.SafeGetString(reader, "LastName"),
											.FK_GenderCode = m_utility.SafeGetString(reader, "FK_GenderCode"),
											.FK_IsCedCode = m_utility.SafeGetString(reader, "FK_IsCedCode"),
											.DateOfBirth = m_utility.SafeGetDateTime(reader, "DateOfBirth", Nothing),
											.PlaceOfBirth = m_utility.SafeGetString(reader, "PlaceOfBirth")
									}

				' load personal prpoerties
				Dim addressData = New AddressData
				addressData = LoadCVLPersonalAddressData(dto.ID)
				dto.Address = addressData

				Dim propertyData = New List(Of PropertyListData)
				propertyData = LoadCVLPersonalTitleData(dto.ID)
				dto.Title = propertyData
				propertyData = LoadCVLPersonalNationalityData(dto.FK_CVLID.GetValueOrDefault(0), dto.ID)
				dto.Nationality = propertyData
				propertyData = LoadCVLPersonalCivilStateData(dto.FK_CVLID.GetValueOrDefault(0), dto.ID)
				dto.CivilState = propertyData
				propertyData = LoadCVLPersonalEMailData(dto.ID)
				dto.Email = propertyData
				propertyData = LoadCVLPersonalHomepageData(dto.ID)
				dto.Homepage = propertyData
				propertyData = LoadCVLPersonalTelefonnumberData(dto.ID)
				dto.PhoneNumbers = propertyData
				propertyData = LoadCVLPersonalTelefaxnumberData(dto.ID)
				dto.TelefaxNumber = propertyData


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalInformation", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalAddressData(ByVal personalID As Integer) As AddressData
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As AddressData = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Address Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New AddressData
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New AddressData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_PersonalID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.Street = m_utility.SafeGetString(reader, "Street"),
											.Postcode = m_utility.SafeGetString(reader, "PostCode"),
											.City = m_utility.SafeGetString(reader, "City"),
											.FK_CountryCode = m_utility.SafeGetString(reader, "FK_CountryCode"),
											.State = m_utility.SafeGetString(reader, "State")
									}

				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalAddressData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalTitleData(ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Title Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "Title")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTitleData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalNationalityData(ByVal cvlPrifleID As Integer?, ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Nationality Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			'listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", cvlPrifleID))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "FK_NationalityCode")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalNationalityData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalCivilStateData(ByVal cvlPrifleID As Integer?, ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal CivilState Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			'listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", cvlPrifleID))
			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "FK_CivilStateCode")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalCivilStateData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalEMailData(ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal EMail Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "EMailAddress")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalEMailData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalHomepageData(ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Homepage Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "Homepage")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalHomepageData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalTelefonnumberData(ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Telefonnumber Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "PhoneNumber")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTelefonnumberData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPersonalTelefaxnumberData(ByVal personalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Personal Telefaxnumber Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("PersonalID", personalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PersonalID", 0),
											.PropertyName = m_utility.SafeGetString(reader, "TelefaxNumber")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTelefaxnumberData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

#End Region

#Region "other information data"

	Private Function LoadCVLAdditionalInformationData(ByVal profileID As Integer, ByVal additionalID As Integer) As OtherInformationDataDTO
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As OtherInformationDataDTO = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Information Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New OtherInformationDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New OtherInformationDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.MilitaryService = m_utility.SafeGetBoolean(reader, "MilitaryService", False),
											.Competences = m_utility.SafeGetString(reader, "Competences"),
											.Additionals = m_utility.SafeGetString(reader, "Additionals"),
											.Interests = m_utility.SafeGetString(reader, "Interests")
									}

				' load prpoerties
				dto.Languages = LoadCVLAdditionalLanguageData(dto.ID)
				dto.DrivingLicence = LoadCVLAdditionalDrivingLicenceData(dto.ID)
				dto.UndatedSkill = LoadCVLAdditionalUndatedSkillData(dto.ID)
				dto.UndatedOperationArea = LoadCVLAdditionalUndatedOperationAreaData(dto.ID)
				dto.UndatedIndustry = LoadCVLAdditionalUndatedIndustryData(dto.ID)
				dto.InternetRosources = LoadCVLAdditionalInternetresourceData(dto.ID)


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInformationData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalLanguageData(ByVal additionalID As Integer) As List(Of LanguageData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of LanguageData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Language Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of LanguageData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New LanguageData
				'With {
				'							.LanguageID = m_utility.SafeGetInteger(reader, "ID", 0),
				'							.Code = m_utility.SafeGetString(reader, "FK_LanguageCode"),
				'							.Level = m_utility.SafeGetString(reader, "FK_LanguageLevelCode")
				'					}


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalLanguageData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalDrivingLicenceData(ByVal additionalID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional DrivingLicence Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetString(reader, "FK_AddID"),
											.PropertyName = m_utility.SafeGetString(reader, "DrivingLicence")
									}


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalDrivingLicenceData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalUndatedSkillData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated Skill Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedSkillData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalUndatedOperationAreaData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated OperationArea Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedOperationAreaData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalUndatedIndustryData(ByVal additionalID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional Undated Industry Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_AddID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalUndatedIndustryData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLAdditionalInternetresourceData(ByVal additionalID As Integer) As List(Of InternetResource)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of InternetResource) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Additional InternetResource Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("AddID", additionalID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of InternetResource)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New InternetResource With {
											.InternetResourceID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_AddID", 0),
											.URL = m_utility.SafeGetString(reader, "URL"),
											.Title = m_utility.SafeGetString(reader, "Title"),
											.Source = m_utility.SafeGetString(reader, "Source"),
											.Snippet = m_utility.SafeGetDecimal(reader, "Snippet", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInternetresourceData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "objective data"

	Private Function LoadCVLObjectiveData(ByVal profileID As Integer, ByVal objID As Integer) As ObjectiveDataDTO
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As ObjectiveDataDTO = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
			listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New ObjectiveDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New ObjectiveDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.AvailabilityDate = m_utility.SafeGetDateTime(reader, "AvailabilityDate", Nothing)
									}

				' load prpoerties
				dto.Salary = LoadCVLObjSalaryData(dto.ID)


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjSalaryData(ByVal objID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Salary Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Salary")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjSalaryData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectivePhaseData(ByVal objID As Integer) As List(Of WorkPhaseDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of WorkPhaseDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Phase Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("objID", objID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of WorkPhaseDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New WorkPhaseDataDTO With {
											.WorkPhaseID = m_utility.SafeGetInteger(reader, "ID", 0),
											.PhaseID = m_utility.SafeGetString(reader, "FK_PhasesID"),
											.Project = m_utility.SafeGetBoolean(reader, "Project", False)
									}
				dto.Company = LoadCVLObjectiveCompanyData(objID)
				dto.Functions = LoadCVLObjectiveFunctionData(objID)
				dto.Positions = LoadCVLObjectivePositionData(objID)
				dto.Employments = LoadCVLObjectiveEmploymentData(objID)
				dto.WorkTimes = LoadCVLObjectiveWorktimeData(objID)

				Dim phase = LoadAssignedCVLPhaseData(dto.PhaseID)

				If Not phase Is Nothing Then
					dto.PhaseID = phase.PhaseID
					dto.DateFrom = phase.DateFrom
					dto.DateTo = phase.DateTo
					dto.DateFromFuzzy = phase.DateFromFuzzy
					dto.DateToFuzzy = phase.DateToFuzzy
					dto.Duration = phase.Duration
					dto.Current = phase.Current
					dto.SubPhase = phase.SubPhase
					dto.Comments = phase.Comments
					dto.PlainText = phase.PlainText
					dto.Location = phase.Location
					dto.Skill = phase.Skill
					dto.SoftSkill = phase.SoftSkill
					dto.OperationAreas = phase.OperationAreas
					dto.Industries = phase.Industries
					dto.CustomCodes = phase.CustomCodes
					dto.Topic = phase.Topic
					dto.InternetRosources = phase.InternetRosources
					dto.DocumentID = phase.DocumentID
				End If


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectivePhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectiveCompanyData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Company Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Company")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveCompanyData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectiveFunctionData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Function Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Function")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveFunctionData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectivePositionData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Position Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectivePositionData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectiveEmploymentData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Employment Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveEmploymentData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLObjectiveWorktimeData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Objective Worktime Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_ObjPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLObjectiveWorktimeData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function



#End Region

#Region "phase data"

	Private Function LoadAssignedCVLPhaseData(ByVal phaseID As Integer) As Phase
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As Phase = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New Phase
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New Phase With {
											.PhaseID = m_utility.SafeGetInteger(reader, "ID", 0),
											.DateFrom = m_utility.SafeGetDateTime(reader, "DateFrom", Nothing),
											.DateTo = m_utility.SafeGetDateTime(reader, "DateTo", Nothing),
											.DateFromFuzzy = m_utility.SafeGetString(reader, "DateFromFuzzy"),
											.DateToFuzzy = m_utility.SafeGetString(reader, "DateToFuzzy"),
											.Duration = m_utility.SafeGetString(reader, "Duration", Nothing),
											.Current = m_utility.SafeGetString(reader, "Current", Nothing),
											.SubPhase = m_utility.SafeGetString(reader, "SubPhase", Nothing),
											.Comments = m_utility.SafeGetString(reader, "Comments"),
											.PlainText = m_utility.SafeGetString(reader, "PlainText")
									}

				' load personal prpoerties
				Dim addressData = New List(Of AddressData)
				addressData = LoadCVLPhaseLocationData(dto.PhaseID)
				dto.Location = addressData

				Dim phaseProperty = New List(Of CodeNameWeightedData)
				phaseProperty = LoadCVLPhaseSkillData(dto.PhaseID)
				dto.Skill = phaseProperty

				phaseProperty = LoadCVLPhaseSoftSkillData(dto.PhaseID)
				dto.SoftSkill = phaseProperty

				phaseProperty = LoadCVLPhaseOperationAreaData(dto.PhaseID)
				dto.OperationAreas = phaseProperty

				phaseProperty = LoadCVLPhaseIndustryData(dto.PhaseID)
				dto.Industries = phaseProperty

				phaseProperty = LoadCVLPhaseCustomCodeData(dto.PhaseID)
				dto.CustomCodes = phaseProperty

				Dim propertyData = New List(Of CodeNameData)
				propertyData = LoadCVLPhaseTopicData(dto.PhaseID)
				dto.Topic = propertyData

				Dim internetData = New List(Of InternetResource)
				internetData = LoadCVLPhaseInternetresourceData(dto.PhaseID)
				dto.InternetRosources = internetData

				Dim documentData = New List(Of CodeIDData)
				documentData = LoadCVLPhaseDocumentIDData(dto.PhaseID)
				dto.DocumentID = documentData


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseLocationData(ByVal phaseID As Integer) As List(Of AddressData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of AddressData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Location Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of AddressData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New AddressData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_PersonalID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Street = m_utility.SafeGetString(reader, "Street"),
											.Postcode = m_utility.SafeGetString(reader, "PostCode"),
											.City = m_utility.SafeGetString(reader, "City"),
											.FK_CountryCode = m_utility.SafeGetString(reader, "FK_CountryCode"),
											.State = m_utility.SafeGetString(reader, "State")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseLocationData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseSkillData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Skill Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseSkillData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseSoftSkillData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase SoftSkill Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseSoftSkillData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseOperationAreaData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase OperationArea Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseOperationAreaData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseIndustryData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Industry Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseIndustryData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseCustomCodeData(ByVal phaseID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase CustomCode Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseCustomCodeData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseTopicData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase Topic Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.CodeName = m_utility.SafeGetString(reader, "Name")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPersonalTitleData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseInternetresourceData(ByVal phaseID As Integer) As List(Of InternetResource)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of InternetResource) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase InternetResource Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of InternetResource)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New InternetResource With {
											.InternetResourceID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.URL = m_utility.SafeGetString(reader, "URL"),
											.Title = m_utility.SafeGetString(reader, "Title"),
											.Source = m_utility.SafeGetString(reader, "Source"),
											.Snippet = m_utility.SafeGetDecimal(reader, "Snippet", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseInternetresourceData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPhaseDocumentIDData(ByVal phaseID As Integer) As List(Of CodeIDData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeIDData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Phase DocumentID Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeIDData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeIDData With {
											.CodeID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PhasesID", 0),
											.Code = m_utility.SafeGetInteger(reader, "Code", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPhaseDocumentIDData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "publication"

	Private Function LoadCVLPublicationPhaseData(ByVal profileID As Integer) As List(Of PublicationDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PublicationDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Publication Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PublicationDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PublicationDataDTO With {
											.PublicationPhaseID = m_utility.SafeGetInteger(reader, "ID", 0),
											.Proceedings = m_utility.SafeGetString(reader, "Proceedings"),
											.Institute = m_utility.SafeGetString(reader, "Institute")
									}

				dto.Author = LoadCVLPublicationAutorData(dto.PublicationPhaseID)

				Dim phase = LoadAssignedCVLPhaseData(dto.PhaseID)

				If Not phase Is Nothing Then
					dto.PhaseID = phase.PhaseID
					dto.DateFrom = phase.DateFrom
					dto.DateTo = phase.DateTo
					dto.DateFromFuzzy = phase.DateFromFuzzy
					dto.DateToFuzzy = phase.DateToFuzzy
					dto.Duration = phase.Duration
					dto.Current = phase.Current
					dto.SubPhase = phase.SubPhase
					dto.Comments = phase.Comments
					dto.PlainText = phase.PlainText
					dto.Location = phase.Location
					dto.Skill = phase.Skill
					dto.SoftSkill = phase.SoftSkill
					dto.OperationAreas = phase.OperationAreas
					dto.Industries = phase.Industries
					dto.CustomCodes = phase.CustomCodes
					dto.Topic = phase.Topic
					dto.InternetRosources = phase.InternetRosources
					dto.DocumentID = phase.DocumentID
				End If


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLPublicationAutorData(ByVal publicationID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Publication Authors Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("publicationID", publicationID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_PubPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Authors")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationAutorData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "Statistic"

	Private Function LoadCVLStatisticPhaseData(ByVal profileID As Integer) As List(Of CVCodeSummaryDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CVCodeSummaryDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Statistic Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CVCodeSummaryDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CVCodeSummaryDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing),
											.Duration = m_utility.SafeGetInteger(reader, "Duration", Nothing),
											.Domain = m_utility.SafeGetString(reader, "Domain")
									}


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLStatisticPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "work data"

	Private Function LoadCVLWorkData(ByVal profileID As Integer, ByVal workID As Integer) As WPhaseDataDTO
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As WPhaseDataDTO = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
			listOfParams.Add(New SqlClient.SqlParameter("WorkID", workID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New WPhaseDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New WPhaseDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.AdditionalText = m_utility.SafeGetString(reader, "AdditionalText")
									}

				' load personal prpoerties
				Dim wphaseData = New List(Of WorkPhaseDataDTO)
				wphaseData = LoadCVLWorkPhaseData(dto.ID)
				dto.WorkPhases = wphaseData


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkPhaseData(ByVal workID As Integer) As List(Of WorkPhaseDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of WorkPhaseDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL WorkPhase Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("WorkID", workID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of WorkPhaseDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New WorkPhaseDataDTO With {
											.WorkPhaseID = m_utility.SafeGetInteger(reader, "ID", 0),
											.PhaseID = m_utility.SafeGetString(reader, "FK_PhasesID"),
											.Project = m_utility.SafeGetBoolean(reader, "Project", False)
									}
				dto.Company = LoadCVLWorkCompanyData(workID)
				dto.Functions = LoadCVLWorkFunctionData(workID)
				dto.Positions = LoadCVLWorkPositionData(workID)
				dto.Employments = LoadCVLWorkEmploymentData(workID)
				dto.WorkTimes = LoadCVLWorkWorktimeData(workID)

				Dim phase = LoadAssignedCVLPhaseData(dto.PhaseID)

				If Not phase Is Nothing Then
					dto.PhaseID = phase.PhaseID
					dto.DateFrom = phase.DateFrom
					dto.DateTo = phase.DateTo
					dto.DateFromFuzzy = phase.DateFromFuzzy
					dto.DateToFuzzy = phase.DateToFuzzy
					dto.Duration = phase.Duration
					dto.Current = phase.Current
					dto.SubPhase = phase.SubPhase
					dto.Comments = phase.Comments
					dto.PlainText = phase.PlainText
					dto.Location = phase.Location
					dto.Skill = phase.Skill
					dto.SoftSkill = phase.SoftSkill
					dto.OperationAreas = phase.OperationAreas
					dto.Industries = phase.Industries
					dto.CustomCodes = phase.CustomCodes
					dto.Topic = phase.Topic
					dto.InternetRosources = phase.InternetRosources
					dto.DocumentID = phase.DocumentID
				End If


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkCompanyData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Company Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Company")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkCompanyData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkFunctionData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Function Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Function")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkFunctionData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkPositionData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Position Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPositionData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkEmploymentData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Employment Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkEmploymentData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLWorkWorktimeData(ByVal phaseID As Integer) As List(Of CodeNameData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Work Worktime Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameData With {
											.CodeNameID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.CodeName = m_utility.SafeGetString(reader, "CodeName")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkWorktimeData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "education data"

	Private Function LoadCVLEducationData(ByVal profileID As Integer, ByVal educationID As Integer) As EdPhaseDataDTO
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As EdPhaseDataDTO = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))
			listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New EdPhaseDataDTO
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New EdPhaseDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.AdditionalText = m_utility.SafeGetString(reader, "AdditionalText")
									}

				' load personal prpoerties
				Dim edPhaseData = New List(Of EducationPhaseDataDTO)
				edPhaseData = LoadCVLEducationPhaseData(dto.ID)
				dto.EducationPhases = edPhaseData


				listOfSearchResultDTO = dto

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLEducationPhaseData(ByVal educationID As Integer) As List(Of EducationPhaseDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of EducationPhaseDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL EducationPhase Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of EducationPhaseDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New EducationPhaseDataDTO With {
											.EducationPhaseID = m_utility.SafeGetInteger(reader, "ID", 0),
											.PhaseID = m_utility.SafeGetString(reader, "FK_PhasesID"),
											.IsCed = m_utility.SafeGetString(reader, "FK_IsCedCode"),
											.Completed = m_utility.SafeGetString(reader, "Completed", Nothing),
											.Score = m_utility.SafeGetInteger(reader, "Score", Nothing)
									}

				dto.EducationType = LoadCVLEducationTypeData(educationID)
				dto.SchoolName = LoadCVLEducationSchoolnameData(educationID)
				dto.Graduation = LoadCVLEducationGraduationData(educationID)

				Dim phase = LoadAssignedCVLPhaseData(dto.PhaseID)

				If Not phase Is Nothing Then
					dto.PhaseID = phase.PhaseID
					dto.DateFrom = phase.DateFrom
					dto.DateTo = phase.DateTo
					dto.DateFromFuzzy = phase.DateFromFuzzy
					dto.DateToFuzzy = phase.DateToFuzzy
					dto.Duration = phase.Duration
					dto.Current = phase.Current
					dto.SubPhase = phase.SubPhase
					dto.Comments = phase.Comments
					dto.PlainText = phase.PlainText
					dto.Location = phase.Location
					dto.Skill = phase.Skill
					dto.SoftSkill = phase.SoftSkill
					dto.OperationAreas = phase.OperationAreas
					dto.Industries = phase.Industries
					dto.CustomCodes = phase.CustomCodes
					dto.Topic = phase.Topic
					dto.InternetRosources = phase.InternetRosources
					dto.DocumentID = phase.DocumentID
				End If


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLEducationTypeData(ByVal educationID As Integer) As List(Of CodeNameWeightedData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of CodeNameWeightedData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL EducationType Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("educationID", educationID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of CodeNameWeightedData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New CodeNameWeightedData With {
											.CodeNameWeightedID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_EducPhasesID", 0),
											.Code = m_utility.SafeGetString(reader, "Code"),
											.Name = m_utility.SafeGetString(reader, "Name"),
											.Weight = m_utility.SafeGetDecimal(reader, "Weight", Nothing)
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationTypeData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLEducationSchoolnameData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Schoolname Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_EducPhasesID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Schoolname")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationSchoolnameData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function

	Private Function LoadCVLEducationGraduationData(ByVal phaseID As Integer) As List(Of PropertyListData)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of PropertyListData) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Education Graduation Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("phaseID", phaseID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of PropertyListData)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New PropertyListData With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.FK_ID = m_utility.SafeGetInteger(reader, "FK_WorkPhaseID", Nothing),
											.PropertyName = m_utility.SafeGetString(reader, "Graduations")
									}

				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationGraduationData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region

#Region "Document"

	Private Function LoadCVLDocumentPhaseData(ByVal profileID As Integer) As List(Of DocumentDataDTO)
		Dim connString As String = My.Settings.ConnStr_Applicant
		Dim listOfSearchResultDTO As List(Of DocumentDataDTO) = Nothing
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim m_utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Document Data For Notification]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																									cmd.Parameters(i).ParameterName,
																																									cmd.Parameters(i).DbType,
																																									cmd.Parameters(i).Size,
																																									cmd.Parameters(i).Value,
																																									ControlChars.NewLine))
			Next

			listOfSearchResultDTO = New List(Of DocumentDataDTO)
			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				Dim dto As New DocumentDataDTO With {
											.ID = m_utility.SafeGetInteger(reader, "ID", 0),
											.DocClass = m_utility.SafeGetString(reader, "DocClass"),
											.Pages = m_utility.SafeGetInteger(reader, "Pages", 0),
											.Plaintext = m_utility.SafeGetString(reader, "PlainText"),
											.FileType = m_utility.SafeGetString(reader, "FileType"),
											.DocBinary = m_utility.SafeGetByteArray(reader, "DocBinary"),
											.DocID = m_utility.SafeGetInteger(reader, "DocID", Nothing),
											.DocSize = m_utility.SafeGetInteger(reader, "DocSize", Nothing),
											.DocLanguage = m_utility.SafeGetString(reader, "DocLanguage")
									}


				listOfSearchResultDTO.Add(dto)

			End While

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLDocumentPhaseData", .MessageContent = msgContent})
		Finally
			If Not reader Is Nothing Then
				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		Return listOfSearchResultDTO
	End Function


#End Region


#End Region


#Region "cvLizer upload"

	<WebMethod(Description:="List new cvlizer profile data")>
	Function LoadALLCVLProfileViewData() As CVLizerProfileDataDTO()
		Dim result As List(Of CVLizerProfileDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = Nothing

		Try
			result = New List(Of CVLizerProfileDataDTO)
			result = m_CVLizer.LoadALLCVLProfileData(Nothing)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = "Admin", .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadALLCVLProfileViewData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer profile data for one day")>
	Function LoadALLAssignedDayCVLProfileViewData(ByVal assignedDate As DateTime?) As CVLizerProfileDataDTO()
		Dim result As List(Of CVLizerProfileDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = Nothing

		Try
			result = New List(Of CVLizerProfileDataDTO)
			result = m_CVLizer.LoadALLCVLProfileData(assignedDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = "Admin", .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadALLAssignedDayCVLProfileViewData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer profile data")>
	Function LoadAssignedCVLProfileViewData(ByVal customerID As String, ByVal profileID As Integer) As CVLizerProfileDataDTO
		Dim result As CVLizerProfileDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New CVLizerProfileDataDTO
			Dim data = m_CVLizer.LoadAssignedCVLProfileData(customerID, profileID, Nothing)
			If Not data Is Nothing Then result = data(0)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLProfileViewData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return result
	End Function


	<WebMethod(Description:="List new cvlizer personal data")>
	Function LoadAssignedCVLPersonalViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal cvlPersonalID As Integer?) As CVLPersonalDataDTO
		Dim result As CVLPersonalDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("LoadAssignedCVLPersonalViewData: customerID: {0} | cvlPrifleID: {1} | cvlPersonalID: {2}", customerID, cvlPrifleID, cvlPersonalID))
		Try
			result = New CVLPersonalDataDTO
			result = m_CVLizer.LoadAssignedCVLPersonalData(customerID, cvlPrifleID, cvlPersonalID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLPersonalViewData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return result
	End Function

	<WebMethod(Description:="List new cvlizer work phase data")>
	Function LoadCVLWorkPhaseViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal workID As Integer) As WorkPhaseViewDataDTO()
		Dim result As List(Of WorkPhaseViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of WorkPhaseViewDataDTO)
			result = m_CVLizer.LoadCVLWorkPhaseData(customerID, cvlPrifleID, workID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLWorkPhaseViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer education phase data")>
	Function LoadCVLEducationPhaseViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal educationID As Integer) As EducationPhaseViewDataDTO()
		Dim result As List(Of EducationPhaseViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of EducationPhaseViewDataDTO)
			result = m_CVLizer.LoadCVLEducationPhaseData(customerID, cvlPrifleID, educationID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLEducationPhaseViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer additional information data")>
	Function LoadCVLAdditionalInfoViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?, ByVal addID As Integer) As AdditionalInfoViewDataDTO
		Dim result As AdditionalInfoViewDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New AdditionalInfoViewDataDTO
			result = m_CVLizer.LoadCVLAdditionalInfoData(customerID, cvlPrifleID, addID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLAdditionalInfoViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List new cvlizer publication data")>
	Function LoadCVLPublicationViewData(ByVal customerID As String, ByVal cvlPrifleID As Integer?) As PublicationViewDataDTO()
		Dim result As List(Of PublicationViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of PublicationViewDataDTO)
			result = m_CVLizer.LoadCVLPublicationData(customerID, cvlPrifleID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLPublicationViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer document data")>
	Function LoadALLCVLDocumentData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO()
		Dim result As List(Of DocumentViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of DocumentViewDataDTO)
			result = m_CVLizer.LoadApplicantDocumentsFromCVLData(customerID, cvlPrifleID, False)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLDocumentData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer document data")>
	Function LoadAssignedApplicantDocumentFromCVLData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO()
		Dim result As List(Of DocumentViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling LoadAssignedApplicantDocumentFromCVLData: customerID: {0} | cvlPrifleID: {1}", customerID, cvlPrifleID))
		Try
			result = New List(Of DocumentViewDataDTO)
			result = m_CVLizer.LoadApplicantDocumentsFromCVLData(customerID, cvlPrifleID, True)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedApplicantDocumentFromCVLData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer assigned document data")>
	Function LoadAssignedApplicantPictureFromCVLData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO
		Dim result As DocumentViewDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling LoadAssignedApplicantPictureFromCVLData: customerID: {0} | cvlPrifleID: {1}", customerID, cvlPrifleID))
		Try
			result = New DocumentViewDataDTO
			result = m_CVLizer.LoadCVLApplicantPictureData(customerID, cvlPrifleID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedApplicantPictureFromCVLData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Get email data for assigned application")>
	Function LoadApplicationEMailData(ByVal customerID As String, ByVal applicationNumber As Integer, ByVal withAttachments As Boolean?) As EMailDataDTO
		Dim result As EMailDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		m_Logger.LogInfo(String.Format("calling LoadApplicationEMailData: customerID: {0} | applicationNumber: {1} | withAttachments: {2}", customerID, applicationNumber, withAttachments))
		Try
			result = New EMailDataDTO
			result = m_CVLizer.LoadAssignedApplicationEMailData(customerID, applicationNumber, withAttachments)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadApplicationEMailData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


	<WebMethod(Description:="List new cvlizer document data")>
	Function LoadAssignedCVLDocumentData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO()
		Dim result As List(Of DocumentViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of DocumentViewDataDTO)
			result = m_CVLizer.LoadCVLDocumentData(customerID, cvlPrifleID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLDocumentData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer document data")>
	Function LoadAssignedCVLDocumenViewtData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO()
		Dim result As List(Of DocumentViewDataDTO) = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New List(Of DocumentViewDataDTO)
			result = m_CVLizer.LoadCVLDocumentData(customerID, cvlPrifleID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCVLDocumenViewtData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List new cvlizer assigned document data")>
	Function LoadAssignedDocumentData(ByVal customerID As String, ByVal id As Integer) As DocumentViewDataDTO
		Dim result As DocumentViewDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New DocumentViewDataDTO
			result = m_CVLizer.LoadAssignedDocumentData(customerID, id)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedDocumentData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List new cvlizer assigned document data")>
	Function LoadAssignedDocumentViewData(ByVal customerID As String, ByVal id As Integer) As DocumentViewDataDTO
		Dim result As DocumentViewDataDTO = Nothing
		Dim excludeCheckInteger As Integer = 1
		m_customerID = customerID

		Try
			result = New DocumentViewDataDTO
			result = m_CVLizer.LoadCVLDocumentData(customerID, id)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedDocumentData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

#End Region


#Region "geo data"

	<WebMethod(Description:="load geo coordinates data for country")>
	Function LoadGeoCoordinationCountryData(ByVal customerID As String, ByVal countryCode As String) As LocationGoordinateDataDTO()
		Dim result As List(Of LocationGoordinateDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of LocationGoordinateDataDTO)
			result = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, String.Empty, String.Empty)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationCountryData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load geo data for postfach data")>
	Function LoadGeoCoordinationPostcodeData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String) As LocationGoordinateDataDTO
		Dim result As LocationGoordinateDataDTO = Nothing
		m_customerID = customerID

		Try
			result = New LocationGoordinateDataDTO
			result = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, String.Empty)(0)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationPostcodeData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)
	End Function

	<WebMethod(Description:="load distance between two postfach data")>
	Function LoadGeoCoordinationTwoPostcodeData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String, ByVal v_plz2 As String) As LocationGoordinateDataDTO()
		Dim result As List(Of LocationGoordinateDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of LocationGoordinateDataDTO)
			result = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, v_plz2)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationTwoPostcodeData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load distance between two postfach data")>
	Function LoadGeoDistancesData(ByVal customerID As String, ByVal countryCode As String, ByVal v_plz1 As String, ByVal v_plz2 As String, ByVal unit As String) As Double
		Dim result As Double = 0
		m_customerID = customerID

		Try
			Dim geoData = New List(Of LocationGoordinateDataDTO)
			geoData = m_PublicData.LoadGeoCoordinationData(m_customerID, countryCode, v_plz1, v_plz2)

			If String.IsNullOrWhiteSpace(unit) Then unit = "K"
			result = CalcDistance(geoData(0).Latitude, geoData(0).Longitude, geoData(1).Latitude, geoData(1).Longitude, unit)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoDistancesData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return result
	End Function


	''' <summary>
	''' Calculate distances between to points.
	''' Standards: 
	''' unit = "M"	miles
	''' unit = "K"	Kilometers
	''' unit = "N"	Nautical miles
	''' </summary>
	''' <returns></returns>
	Private Function CalcDistance(ByVal lat1 As Double, ByVal lon1 As Double, ByVal lat2 As Double, ByVal lon2 As Double, ByVal unit As Char) As Double
		Dim theta As Double = lon1 - lon2
		Dim dist As Double = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta))

		dist = Math.Acos(dist)
		dist = rad2deg(dist)
		dist = dist * 60 * 1.1515

		If unit = "K" Then
			dist = dist * 1.609344
		ElseIf unit = "N" Then
			dist = dist * 0.8684
		End If


		Return dist
	End Function

	Private Function deg2rad(ByVal deg As Double) As Double
		Return (deg * Math.PI / 180.0)
	End Function

	Private Function rad2deg(ByVal rad As Double) As Double
		Return rad / Math.PI * 180.0
	End Function


#End Region



#Region "searching for skills and operation areas"


	<WebMethod(Description:="List customer postcode and city")>
	Function LoadPostcodeCityViewData(ByVal customerID As String) As PostcodeCityViewDataDTO()
		Dim result As List(Of PostcodeCityViewDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of PostcodeCityViewDataDTO)
			result = m_CVLizer.LoadCVLPostcodeCityData(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPostcodeCityViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List customer workphase job groups (operationarea)")>
	Function LoadJobGroupsViewData(ByVal customerID As String) As ExperiencesViewDataDTO()
		Dim result As List(Of ExperiencesViewDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of ExperiencesViewDataDTO)
			result = m_CVLizer.LoadCVLJobGroupsData(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadJobGroupsViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List customer experiences")>
	Function LoadExperiencesViewData(ByVal customerID As String, ByVal id As Integer) As ExperiencesViewDataDTO()
		Dim result As List(Of ExperiencesViewDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of ExperiencesViewDataDTO)
			result = m_CVLizer.LoadCVLExperiencesData(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadExperiencesViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List customer postcode and city")>
	Function LoadLanguageViewData(ByVal customerID As String) As LanguageViewDataDTO()
		Dim result As List(Of LanguageViewDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of LanguageViewDataDTO)
			result = m_CVLizer.LoadCVLLanguageData(customerID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPostcodeCityViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List cvl search result")>
	Function LoadSearchResultViewData(ByVal customerID As String, ByVal postfachCityData As PostcodeCityViewDataDTO, ByVal redius As Integer,
									  ByVal JobTitelsData As ExperiencesViewDataDTO(),
									  ByVal operationAreasData As ExperiencesViewDataDTO(),
									  ByVal skillsData As ExperiencesViewDataDTO(),
									  ByVal languageData As LanguageViewDataDTO()) As CVLSearchResultDataDTO()
		Dim result As List(Of CVLSearchResultDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CVLSearchResultDataDTO)
			result = m_CVLizer.LoadCVLSearchData(customerID, String.Empty, postfachCityData, redius,
												 JobTitelsData.ToList(), operationAreasData.ToList(), JoinENum.ODER,
												 skillsData.ToList(), JoinENum.ODER,
												 languageData.ToList(), JoinENum.ODER, String.Empty, False)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadSearchResultViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List cvl search result with saving search criteria")>
	Function LoadSearchResultAndSaveCriteriaViewData(ByVal customerID As String, ByVal userID As String, ByVal postfachCityData As PostcodeCityViewDataDTO, ByVal redius As Integer,
													 ByVal JobTitelsData As ExperiencesViewDataDTO(),
													 ByVal operationAreaData As ExperiencesViewDataDTO(), ByVal operationAreaJoin As JoinENum,
													 ByVal skillData As ExperiencesViewDataDTO(), ByVal skillJoin As JoinENum,
													 ByVal languageData As LanguageViewDataDTO(), ByVal languageJoin As JoinENum,
													 ByVal searchLabel As String, ByVal setNotification As Boolean) As CVLSearchResultDataDTO()
		Dim result As List(Of CVLSearchResultDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CVLSearchResultDataDTO)
			If String.IsNullOrWhiteSpace(searchLabel) Then setNotification = False
			result = m_CVLizer.LoadCVLSearchData(customerID, userID, postfachCityData, redius, JobTitelsData.ToList(),
												 operationAreaData.ToList(), operationAreaJoin, skillData.ToList(), skillJoin,
												 languageData.ToList(), languageJoin, searchLabel, setNotification)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
								   .MessageHeader = "LoadSearchResultAndSaveCriteriaViewData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List user saved cvl search history")>
	Function LoadCVLSearchHistoryData(ByVal customerID As String, ByVal userID As String) As CVLSearchHistoryDataDTO()
		Dim result As List(Of CVLSearchHistoryDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CVLSearchHistoryDataDTO)
			result = m_CVLizer.LoadUserCVLSearchHistoryData(customerID, userID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
														 .MessageHeader = "LoadCVLSearchHistoryData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List user saved cvl search history result")>
	Function LoadAssignedCVLSearchHistoryResultData(ByVal customerID As String, ByVal searchID As Integer) As CVLSearchResultDataDTO()
		Dim result As List(Of CVLSearchResultDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CVLSearchResultDataDTO)
			result = m_CVLizer.LoadAssignedCVLSearchHistoryResultData(customerID, searchID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
														 .MessageHeader = "LoadAssignedCVLSearchHistoryResultData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="update saved cvl search history notifier state")>
	Function UpdateAssignedCVLSearchHistoryNotifierStateData(ByVal customerID As String, ByVal searchID As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = m_CVLizer.UpdateAssignedCVLSearchHistoryNotifierData(customerID, searchID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
														 .MessageHeader = "UpdateAssignedCVLSearchHistoryNotifierStateData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="delete user saved cvl search history")>
	Function DeleteAssignedCVLSearchHistoryData(ByVal customerID As String, ByVal searchID As Integer) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = m_CVLizer.DeleteAssignedCVLSearchHistoryData(customerID, searchID)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
														 .MessageHeader = "DeleteAssignedCVLSearchHistoryData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function


#End Region


End Class