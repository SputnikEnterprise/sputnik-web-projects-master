
Imports System.IO
Imports System.Text

Imports Newtonsoft.Json
Imports System.Net
Imports sp_WebServiceUtility.WOSInfo
Imports sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects
Imports System.Net.Http
Imports Newtonsoft.Json.Linq
Imports System.Net.Http.Headers
Imports System.Threading.Tasks
Imports System.Threading

Namespace JobPlatform

	Partial Class VacancyUtilities


#Region "private consts"

		Private Const JOBROOM_USER As String = "mfSputnikInformatikGmbH" ' "sputnik"
		Private Const JOBROOM_PASSWORD As String = "552vq0nckl" ' "54oy3hhn5x"
		Private Const JOBROOM_URI As String = "https://api.job-room.ch/jobAdvertisements/v1"
		Private Const JOBROOM_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const JOBROOM_SINGLE_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}"
		Private Const JOBROOM_CANCEL_RECORDS_URI As String = "https://api.job-room.ch/jobAdvertisements/v1/{0}/cancel"

		Private Const STAGING_JOBROOM_USER As String = "sputnik"
		Private Const STAGING_JOBROOM_PASSWORD As String = "ygxh8cvfqn"     ' "dqz5kkvzlt"
		Private Const STAGING_JOBROOM_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1"
		Private Const STAGING_JOBROOM_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1?page={0}&size{1}"
		Private Const STAGING_JOBROOM_SINGLE_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}"
		Private Const STAGING_JOBROOM_CANCEL_RECORDS_URI As String = "https://staging.job-room.ch/jobAdvertisements/v1/{0}/cancel"

		Private Const CHILKAT_COMPONENT_CODE As String = "MFSPUT.CB1102020_Q8aw6Ch8jR3W"

#End Region

		Private m_SearchResultData As JobroomSearchResultData

		Private m_TransmittedSTMPid As String


		Private m_APIResponse As String
		Private m_ReportingObligation As Boolean?
		Private m_QueryResultData As SPAVAMQueryResultData

		Private m_ResultContent As String
		Private m_UserData As SPAdvisorData
		Private m_MDData As MandantData

		Private m_UserName As String
		Private m_Password As String
		Private m_JobroomURI As String
		Private m_JobroomAllRecordURI As String
		Private m_JobroomSingleRecordURI As String



#Region "Public Methods"

		Public Function AddAVAMAdvertisementToRAV(ByVal customerID As String, ByVal userID As String, ByVal asStaging As Boolean, ByVal vacancyData As VacancyMasterData,
																								ByVal vacancyJobCHData As VacancyInseratJobCHData, ByVal vacancyStmpData As VacancyStmpSettingData,
																								ByVal vacancyStmpLanguageData As List(Of VacancyJobCHLanguageData),
																								ByVal MDData As MandantData,
																								ByVal userData As SPAdvisorData, ByVal employerData As CustomerMasterData,
																								ByVal jobNumber As Integer?, ByVal language As String) As Task(Of SPAVAMJobCreationData)
			'ByVal jobNumber As Integer?, ByVal language As String) As Threading.Tasks.Task(Of SPAVAMJobCreationData)
			m_customerID = customerID

			m_MDData = MDData
			m_UserData = userData

			Dim msgContent = "library is started..."
			Dim result As New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
			Dim htmlToMarkdown As New Html2Markdown.Converter
			Dim userFullname As String = "System"

			m_TransmittedSTMPid = String.Empty
			m_ReportingObligation = False

			m_TransmittedSTMPid = String.Empty

			Dim jasonString = BuildJasonstring(m_customerID, userID, vacancyData, vacancyJobCHData, vacancyStmpData, vacancyStmpLanguageData, m_MDData, m_UserData, employerData, jobNumber, language)


			Dim success As Boolean = True
			Try
				Dim baseUri As Uri = New Uri(m_JobroomURI)

				msgContent = String.Format("{1}: {2} >>> {3} | {4}{0}VacancyNr: {5}{0}{6}", vbNewLine, baseUri, success, m_UserName, m_Password, vacancyData.VakNr, jasonString.ToString())
				' TODO: To remove after a while
				m_SysInfo.AddNotifyMessage(m_customerID, New ErrorMessageData With {.CustomerID = m_customerID, .MessageHeader = "AddAVAMAdvertisementToRAV.WebserviceResponse",
																	 .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname, .MessageArt = 99})
				m_Logger.LogInfo(msgContent)

				'success = success AndAlso webserviceResponse(jasonString.ToString(), baseUri, "Post", m_UserName, m_Password)
				'If Not success Then Return Nothing

				Dim response As New HttpResponseMessage
				response = Await webserviceResponse(jasonString, baseUri, "Post", m_UserName, m_Password)

				Try

					If response Is Nothing Then
						result.ErrorMessage = m_SearchResultData.ErrorMessage

						Return Nothing
					End If

					Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)
					Dim jsonContent As JObject = JObject.Parse(myStreamReader.ReadToEnd)

					Dim jobRoomContent As New SPJobroomData
					jobRoomContent = m_SearchResultData.Content(0)
					m_TransmittedSTMPid = jobRoomContent.ID

					result.QueryContent = jasonString.ToString()
					result.ResultContent = m_ResultContent
					result.AVAMRecordState = jobRoomContent.Status
					result.JobroomID = jobRoomContent.ID
					result.Content = jobRoomContent.JobContent

					result.State = Not String.IsNullOrWhiteSpace(m_TransmittedSTMPid)
					result.ReportingObligation = jobRoomContent.ReportingObligation.GetValueOrDefault(False)
					result.reportingObligationEndDate = jobRoomContent.ReportingObligationEndDate
					result.CreatedOn = Now
					result.CreatedFrom = m_UserData.UserFullname

					If Not result Is Nothing AndAlso Not String.IsNullOrWhiteSpace(result.JobroomID) Then success = success AndAlso AddAVAMAdvertismentData(m_customerID, m_UserData.UserGuid, vacancyData.VakNr, False, result)

				Catch ex As Exception
					m_Logger.LogError(ex.ToString)
					result = Nothing

				End Try

			Catch ex As Exception
				msgContent = ex.ToString
				m_SysInfo.AddErrorMessage(m_customerID, New ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = "AddAVAMAdvertisementToRAV.WebserviceResponse", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				m_Logger.LogError(ex.ToString)
				m_TransmittedSTMPid = Nothing
				result = Nothing

			End Try


			Return result

		End Function

		Public Function CancelAssignedJobAdvertisementData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As SPAdvisorData, ByVal v_id As String, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM) As WebServiceResult
			Dim result As New WebServiceResult With {.JobResult = False, .JobResultMessage = String.Empty} 'New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
			Dim success As Boolean = True
			Dim msgContent As String

			m_customerID = customerID
			m_UserData = userData

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
				If asStaging Then m_UserName = STAGING_JOBROOM_USER Else m_UserName = JOBROOM_USER
			Else
				m_UserName = My.Settings.AVAM_UserName
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
				If asStaging Then m_Password = STAGING_JOBROOM_PASSWORD Else m_Password = JOBROOM_PASSWORD
			Else
				m_Password = My.Settings.AVAM_Password
			End If

			If asStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_CANCEL_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_CANCEL_RECORDS_URI

			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try

				Using writer As JsonWriter = New JsonTextWriter(sw)

					writer.WriteStartObject()

					writer.WritePropertyName("code")
					Select Case reasonEnum
						Case AVAMAdvertismentCancelReasonENUM.CHANGE_OR_REPOSE
							writer.WriteValue("CHANGE_OR_REPOSE")
						Case AVAMAdvertismentCancelReasonENUM.NOT_OCCUPIED
							writer.WriteValue("NOT_OCCUPIED")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_AGENCY
							writer.WriteValue("OCCUPIED_AGENCY")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBCENTER
							writer.WriteValue("OCCUPIED_JOBCENTER")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_JOBROOM
							writer.WriteValue("OCCUPIED_JOBROOM")
						Case AVAMAdvertismentCancelReasonENUM.OCCUPIED_OTHER
							writer.WriteValue("OCCUPIED_OTHER")

						Case Else
							Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "no reason was defined!"}

					End Select

					writer.WriteEndObject()

				End Using

			Catch ex As Exception
				msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedJobAdvertisementData", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = ex.ToString}
			End Try

			Try
				'v_id = "f5c1c0fb-a537-11e8-9710-005056ac3479"
				Dim baseUri As Uri = New Uri(String.Format(m_JobroomSingleRecordURI, v_id))
				result = WebservicePATCHResponse(sb.ToString(), baseUri, m_UserName, m_Password)
				If Not result.JobResult Then Return result


				Dim resultData As New SPAVAMJobCreationData With {.CreatedFrom = userData.UserFullname, .CreatedOn = Now, .ResultContent = "WebservicePATCHResponse: successfull"}
				success = success AndAlso AddNotifyDataIntoDatabase(m_customerID, m_UserData.UserGuid, resultData)

			Catch ex As Exception
				msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CancelAssignedJobAdvertisementData", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				m_Logger.LogError(ex.ToString)
				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = ex.ToString}

			End Try

			Return result

		End Function

		Public Function LoadAssignedJobAdvertisementQueryResultData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As SPAdvisorData, ByVal v_id As String) As SPAVAMJobCreationData
			Dim result As New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
			Dim success As Boolean = True

			m_customerID = customerID
			m_UserData = userData

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
				If asStaging Then m_UserName = STAGING_JOBROOM_USER Else m_UserName = JOBROOM_USER

			Else
				m_UserName = My.Settings.AVAM_UserName
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
				If asStaging Then m_Password = STAGING_JOBROOM_PASSWORD Else m_Password = JOBROOM_PASSWORD
			Else
				m_Password = My.Settings.AVAM_Password
			End If

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomURI) Then
				m_JobroomURI = JOBROOM_URI
			Else
				m_JobroomURI = My.Settings.AVAM_JobroomURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomAllRecordURI) Then
				If asStaging Then m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI Else m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
			Else
				m_JobroomAllRecordURI = My.Settings.AVAM_JobroomAllRecordURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomSingleRecordURI) Then
				If asStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI
			Else
				m_JobroomSingleRecordURI = My.Settings.AVAM_JobroomSingleRecordURI
			End If

			Try
				Dim baseUri As Uri = New Uri(String.Format(m_JobroomSingleRecordURI, v_id))
				m_Logger.LogInfo(String.Format("CustomerID: {1} | JobroomID: {2} | asStaging: {3} | UserFullname: {4}{0}baseUri: {5} | m_UserName: {6} | m_Password: {7}{0}m_JobroomSingleRecordURI: {8}",
																			 vbNewLine, customerID, v_id, asStaging, userData.UserFullname, baseUri.ToString, m_UserName, m_Password, m_JobroomSingleRecordURI))

				Dim queryResult = WebserviceGetRequest(baseUri, m_UserName, m_Password)

				If String.IsNullOrWhiteSpace(queryResult) Then
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}queryResult is empty.", vbNewLine, customerID, v_id))
					result.ErrorMessage = New SPErrorData With {.Detail = "Leere Rückgabe!"}

					Return result
				Else
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}queryResult HAS value.", vbNewLine, customerID, v_id))
				End If
				If Not success Then
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}queryResult HAS value but is not succeeded.", vbNewLine, customerID, v_id))
				Else
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}going to parse queryresult.", vbNewLine, customerID, v_id))
				End If

				success = success AndAlso ParseJSonResult(queryResult)

				If Not m_SearchResultData.ErrorMessage Is Nothing Then
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}m_SearchResultData.ErrorMessage has value.", vbNewLine, customerID, v_id))
					result.ErrorMessage = m_SearchResultData.ErrorMessage

					Return result
				Else
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}m_SearchResultData.ErrorMessage is nothing.", vbNewLine, customerID, v_id))

				End If
				m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0}ParseJSonResult: {3}", vbNewLine, customerID, v_id, success))

				Dim jobRoomContent As New SPJobroomData
				Try
					m_Logger.LogWarning(String.Format("CustomerID: {1} | JobroomID: {2}{0} m_SearchResultData.Content: {3}", vbNewLine, customerID, v_id, m_SearchResultData.Content.Count))

					jobRoomContent = m_SearchResultData.Content(0)
					m_TransmittedSTMPid = jobRoomContent.ID

					result.AVAMRecordState = jobRoomContent.Status
					result.JobroomID = jobRoomContent.ID
					result.Content = jobRoomContent.JobContent

					result.State = Not String.IsNullOrWhiteSpace(m_TransmittedSTMPid)
					result.ReportingObligation = jobRoomContent.ReportingObligation.GetValueOrDefault(False)
					result.reportingObligationEndDate = jobRoomContent.ReportingObligationEndDate
					result.ResultContent = queryResult

					result.SyncDate = Now
					result.SyncFrom = userData.UserFullname
					result.CreatedFrom = userData.UserFullname

					If Not result Is Nothing AndAlso Not String.IsNullOrWhiteSpace(result.JobroomID) Then success = success AndAlso AddNotifyDataIntoDatabase(m_customerID, userData.UserGuid, result)

				Catch ex As Exception
					m_Logger.LogError(String.Format("CustomerID: {1} | JobroomID: {2}{0}jobRoomContent building object.{0}{3}", vbNewLine, customerID, v_id, ex.ToString))

					Return Nothing
				End Try


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

				Dim msgContent As String
				msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedJobAdvertisementQueryResultData", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				m_Logger.LogError(ex.ToString)

				Return Nothing
			End Try


			Return result

		End Function

		Public Function LoadAllJobAdvertisement(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As SPAdvisorData) As Boolean
			Dim result As Boolean = True
			Dim msgContent As String

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
				If asStaging Then m_UserName = STAGING_JOBROOM_USER Else m_UserName = JOBROOM_USER

			Else
				m_UserName = My.Settings.AVAM_UserName
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
				If asStaging Then m_Password = STAGING_JOBROOM_PASSWORD Else m_Password = JOBROOM_PASSWORD
			Else
				m_Password = My.Settings.AVAM_Password
			End If

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomURI) Then
				m_JobroomURI = JOBROOM_URI
			Else
				m_JobroomURI = My.Settings.AVAM_JobroomURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomAllRecordURI) Then
				If asStaging Then m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI Else m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
			Else
				m_JobroomAllRecordURI = My.Settings.AVAM_JobroomAllRecordURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomSingleRecordURI) Then
				If asStaging Then m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI Else m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI
			Else
				m_JobroomSingleRecordURI = My.Settings.AVAM_JobroomSingleRecordURI
			End If

			Dim baseUri As Uri = New Uri(String.Format(m_JobroomAllRecordURI, 0, 25))
			m_SearchResultData = New JobroomSearchResultData

			Try
				Dim queryResult = WebserviceGetRequest(baseUri, m_UserName, m_Password)

				result = result AndAlso ParseJSonResult(queryResult)

				For Each itm In m_SearchResultData.Content
					Dim v_id As String = itm.ID

					If String.IsNullOrWhiteSpace(itm.ID) Then
						msgContent = String.Format("{0} is empty.", v_id)
						m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllJobAdvertisement", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

						Continue For
					End If

					Dim queryData As New SPAVAMJobCreationData
					queryData.AVAMRecordState = itm.Status
					queryData.JobroomID = itm.ID
					queryData.Content = itm.JobContent

					queryData.State = Not String.IsNullOrWhiteSpace(v_id)
					queryData.ReportingObligation = itm.ReportingObligation.GetValueOrDefault(False)
					queryData.reportingObligationEndDate = itm.ReportingObligationEndDate
					queryData.ResultContent = queryResult

					queryData.SyncDate = Now
					queryData.SyncFrom = userData.UserFullname
					queryData.CreatedFrom = userData.UserFullname

					result = result AndAlso LoadAssignedQueryData(v_id)
					If m_QueryResultData Is Nothing OrElse m_QueryResultData.ID.GetValueOrDefault(0) = 0 Then
						If Not queryData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(queryData.JobroomID) Then result = result AndAlso AddNotifyDataIntoDatabase(m_customerID, userData.UserGuid, queryData)
					Else
						If Not queryData Is Nothing AndAlso Not String.IsNullOrWhiteSpace(queryData.JobroomID) Then result = result AndAlso UpdateNotifyDataIntoDatabase(customerID, userData.UserGuid, queryData)
					End If

				Next


			Catch ex As Exception
				msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllJobAdvertisement", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				m_Logger.LogError(ex.ToString)

				Return False

			End Try

			Return True

		End Function


#End Region


#Region "private methodes"

		Private Function BuildJasonstring(ByVal customerID As String, ByVal userID As String, ByVal vacancyData As VacancyMasterData,
																										ByVal vacancyJobCHData As VacancyInseratJobCHData, ByVal vacancyStmpData As VacancyStmpSettingData,
																										ByVal vacancyStmpLanguageData As List(Of VacancyJobCHLanguageData),
																										ByVal MDData As MandantData,
																										ByVal userData As SPAdvisorData, ByVal employerData As CustomerMasterData,
																										ByVal jobNumber As Integer?, ByVal language As String) As StringBuilder
			Dim msgContent = "library is started..."
			Dim result As New SPAVAMJobCreationData With {.JobroomID = String.Empty, .State = False}
			Dim htmlToMarkdown As New Html2Markdown.Converter
			Dim userFullname As String = "System"

			Try
				If m_UserData Is Nothing Then
					msgContent = "userData was null"
					Throw New Exception(msgContent)
				End If
				userFullname = userData.UserFullname

				If vacancyData Is Nothing Then
					msgContent = "vacancyData was null"
					Throw New Exception(msgContent)
				End If
				If vacancyJobCHData Is Nothing Then
					msgContent = "vacancyJobCHData was null"
					Throw New Exception(msgContent)
				End If
				If vacancyStmpData Is Nothing Then
					msgContent = "vacancyStmpData was null"
					Throw New Exception(msgContent)
				End If
				If m_MDData Is Nothing Then
					msgContent = "MDData was null"
					Throw New Exception(msgContent)
				End If
				If employerData Is Nothing Then
					msgContent = "employerData was null"
					Throw New Exception(msgContent)
				End If

			Catch ex As Exception
				m_SysInfo.AddErrorMessage(m_customerID, New ErrorMessageData With {.CustomerID = m_customerID, .MessageHeader = "AddAVAMAdvertisementToRAV.StringWriter",
																		 .MessageContent = ex.ToString, .CreatedFrom = userFullname})

				Return Nothing

			End Try

			Dim sb As New StringBuilder()
			Dim sw As New StringWriter(sb)

			Try

				Using writer As JsonWriter = New JsonTextWriter(sw)

					writer.WriteStartObject()

					'writer.WritePropertyName("externalUrl")
					'writer.WriteValue("externalUrl_Value")
					'writer.WritePropertyName("externalReference")
					'writer.WriteValue("externalReference_Value")
					writer.WritePropertyName("reportToAvam")
					If vacancyStmpData.ReportToAvam.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					If vacancyStmpData.NumberOfJobs.GetValueOrDefault(1) > 1 Then
						writer.WritePropertyName("numberOfJobs")
						writer.WriteValue(vacancyStmpData.NumberOfJobs.GetValueOrDefault(1))
					End If

					' contact
					' Provide an administrative contact (e. g. an HR employee); this contact is used for email notifications concerning the reporting obligation
					writer.WritePropertyName("contact")
					writer.WriteStartObject()

					writer.WritePropertyName("languageIsoCode")
					writer.WriteValue("de")
					writer.WritePropertyName("salutation")
					If userData.Salutation = "Herr" Then
						writer.WriteValue("MR")
					Else
						writer.WriteValue("MS")
					End If
					writer.WritePropertyName("firstName")
					writer.WriteValue(userData.Firstname)
					writer.WritePropertyName("lastName")
					writer.WriteValue(userData.Lastname)
					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(userData.UserMDTelefon))
					writer.WritePropertyName("email")
					writer.WriteValue(userData.UserMDeMail)

					writer.WriteEndObject()


					'' jobDescriptions
					' The text of the job advertisement; may be multilingual
					writer.WritePropertyName("jobDescriptions")
					writer.WriteStartArray()
					writer.WriteStartObject()

					writer.WritePropertyName("languageIsoCode")
					writer.WriteValue("de")
					writer.WritePropertyName("title")
					writer.WriteValue(vacancyData.Bezeichnung)
					writer.WritePropertyName("description")
					Dim anforderung = vacancyJobCHData.Anforderung
					Dim taetigkeit = vacancyJobCHData.Aufgabe
					If String.IsNullOrWhiteSpace(anforderung) Then
						If Not taetigkeit.Contains("Tätigkeit:") Then taetigkeit = String.Format("Tätigkeit:<br>{0}", taetigkeit)
						anforderung = taetigkeit
					Else

						If Not anforderung.Contains("Anforderung:") Then anforderung = String.Format("Anforderung:<br>{0}", anforderung)
						anforderung &= String.Format("<br>{0}", taetigkeit)

					End If
					writer.WriteValue(htmlToMarkdown.Convert(anforderung))

					writer.WriteEndObject()
					writer.WriteEndArray()

					'' company
					' The company that handles the recruitment. This information is published.
					writer.WritePropertyName("company")
					writer.WriteStartObject()

					writer.WritePropertyName("name")
					writer.WriteValue(MDData.MandantName1)

					Dim streetName As String = String.Empty
					Dim houseNumber As String = String.Empty

					If Not String.IsNullOrWhiteSpace(MDData.Street) Then
						Dim adressString = MDData.Street
						Dim i As Integer = 0
						Dim streetData = adressString.Split(" ")

						houseNumber = streetData(streetData.Length - 1)
						For Each itm In streetData
							If i < streetData.Length - 1 Then
								streetName = String.Format("{0}{1}{2}", streetName, If(String.IsNullOrWhiteSpace(streetName), "", " "), itm)
							End If
							i += 1
						Next

					End If

					writer.WritePropertyName("street")
					writer.WriteValue(streetName)

					writer.WritePropertyName("houseNumber")
					writer.WriteValue(houseNumber)

					'writer.WritePropertyName("postOfficeBoxNumber")
					'writer.WriteValue("postOfficeBoxNumber")
					'writer.WritePropertyName("postOfficeBoxPostalCode")
					'writer.WriteValue("postOfficeBoxPostalCode")
					'writer.WritePropertyName("postOfficeBoxCity")
					'writer.WriteValue("postOfficeBoxCity")

					writer.WritePropertyName("postalCode")
					writer.WriteValue(MDData.Postcode)

					writer.WritePropertyName("city")
					writer.WriteValue(MDData.Location)

					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue("CH")

					writer.WritePropertyName("website")
					writer.WriteValue(MDData.Homepage)

					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(MDData.Telephon))

					writer.WritePropertyName("email")
					writer.WriteValue(MDData.EMail)

					writer.WritePropertyName("surrogate")
					If vacancyStmpData.Surrogate.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					writer.WriteEndObject()

					'' employer
					' Must be provided if the company handling the recruitment is not the actual employer; will not be published.
					writer.WritePropertyName("employer")
					writer.WriteStartObject()

					writer.WritePropertyName("name")
					writer.WriteValue(employerData.Company1)
					writer.WritePropertyName("postalCode")
					writer.WriteValue(employerData.Postcode)
					writer.WritePropertyName("city")
					writer.WriteValue(employerData.Location)
					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue(employerData.CountryCode)

					writer.WriteEndObject()

					'' employment
					' Employment metadata
					writer.WritePropertyName("employment")
					writer.WriteStartObject()

					'If vacancyStmpData.StartDate.HasValue Then
					'	writer.WritePropertyName("startDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.StartDate.GetValueOrDefault(Now)))
					'End If
					'If vacancyStmpData.EndDate.HasValue Then
					'	writer.WritePropertyName("endDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.EndDate.GetValueOrDefault(Now)))
					'End If

					writer.WritePropertyName("shortEmployment")
					If vacancyStmpData.ShortEmployment.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("immediately")
					If vacancyStmpData.Immediately.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("permanent")
					If vacancyStmpData.Permanent.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If

					Dim jobProztent = vacancyData.JobProzent
					If String.IsNullOrWhiteSpace(jobProztent) Then
						jobProztent = "100#100"
					End If
					Dim minProzent = jobProztent.Split(CChar("#"))(0)
					Dim maxProzent = jobProztent.Split(CChar("#"))(1)
					minProzent = Math.Max(10, Val(minProzent))
					maxProzent = Math.Max(10, Val(maxProzent))

					writer.WritePropertyName("workloadPercentageMax")
					writer.WriteValue(CStr(maxProzent))
					writer.WritePropertyName("workloadPercentageMin")
					writer.WriteValue(CStr(minProzent))

					'workForms
					If Not vacancyStmpData Is Nothing AndAlso (vacancyStmpData.Sunday_and_Holidays.GetValueOrDefault(False) OrElse vacancyStmpData.Shift_Work.GetValueOrDefault(False) OrElse vacancyStmpData.Night_Work.GetValueOrDefault(False) OrElse
						vacancyStmpData.Home_Work.GetValueOrDefault(False)) Then
						writer.WritePropertyName("workForms")
						writer.WriteStartArray()

						Dim langName As String
						If vacancyStmpData.Sunday_and_Holidays.GetValueOrDefault(False) Then
							langName = "SUNDAY_AND_HOLIDAYS"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Shift_Work.GetValueOrDefault(False) Then
							langName = "SHIFT_WORK"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Night_Work.GetValueOrDefault(False) Then
							langName = "NIGHT_WORK"
							writer.WriteValue(langName)
						End If
						If vacancyStmpData.Home_Work.GetValueOrDefault(False) Then
							langName = "HOME_WORK"
							writer.WriteValue(langName)
						End If

						writer.WriteEnd()

					End If


					writer.WriteEndObject()

					'' location
					' The work location
					writer.WritePropertyName("location")
					writer.WriteStartObject()

					'writer.WritePropertyName("remarks")
					'writer.WriteValue("remarks")
					writer.WritePropertyName("postalCode")
					writer.WriteValue(vacancyData.JobPLZ)
					writer.WritePropertyName("city")
					writer.WriteValue(vacancyData.JobOrt)
					writer.WritePropertyName("countryIsoCode")
					writer.WriteValue("CH")

					writer.WriteEndObject()

					'' occupation
					' The ad must be coded to an occupation according ot the AVAM occupation list; this determines the reporting obligation.
					writer.WritePropertyName("occupation")
					writer.WriteStartObject()

					writer.WritePropertyName("avamOccupationCode")
					writer.WriteValue(CStr(vacancyData.SBNNumber))

					Dim experienceValue As String = String.Empty
					If vacancyStmpData.Less_One_Year.GetValueOrDefault(False) Then
						experienceValue = "LESS_THAN_1_YEAR"
					ElseIf vacancyStmpData.More_One_Year.GetValueOrDefault(False) Then
						experienceValue = "MORE_THAN_1_YEAR"
					ElseIf vacancyStmpData.More_Three_Years.GetValueOrDefault(False) Then
						experienceValue = "MORE_THAN_3_YEARS"
					End If
					If Not String.IsNullOrWhiteSpace(experienceValue) Then
						writer.WritePropertyName("workExperience")
						writer.WriteValue(experienceValue)
					End If

					If vacancyStmpData.EducationCode.GetValueOrDefault(0) > 0 Then
						writer.WritePropertyName("educationCode")
						writer.WriteValue(vacancyStmpData.EducationCode)
					End If

					writer.WriteEndObject()

					'' languageSkills
					If Not vacancyStmpLanguageData Is Nothing AndAlso vacancyStmpLanguageData.Count > 0 Then
						writer.WritePropertyName("languageSkills")
						writer.WriteStartArray()

						' TODO: if not exists, must be "de" and "NONE"
						For Each lang As VacancyJobCHLanguageData In vacancyStmpLanguageData
							writer.WriteStartObject()
							Dim langName As String = "de"
							Dim langNiveau As String = "BASIC"

							Select Case lang.Bezeichnung_Value.GetValueOrDefault(0)
								Case 1
									langName = "de"
								Case 2
									langName = "fr"
								Case 4
									langName = "it"
								Case 3
									langName = "en"
							End Select

							writer.WritePropertyName("languageIsoCode")
							writer.WriteValue(langName)

							Select Case lang.LanguageNiveau_Value.GetValueOrDefault(0)
								Case 1
									langNiveau = "NONE"
								Case 2
									langNiveau = "BASIC"
								Case 3
									langNiveau = "INTERMEDIATE"
								Case 4
									langNiveau = "PROFICIENT"
							End Select

							writer.WritePropertyName("spokenLevel")
							writer.WriteValue(langNiveau)
							writer.WritePropertyName("writtenLevel")
							writer.WriteValue(langNiveau)

							writer.WriteEndObject()
						Next
						writer.WriteEndArray()

					End If


					'' applyChannel
					' Provide at least one channel for applications.
					writer.WritePropertyName("applyChannel")
					writer.WriteStartObject()

					writer.WritePropertyName("mailAddress")
					writer.WriteValue(userData.UserMDeMail)
					writer.WritePropertyName("phoneNumber")
					writer.WriteValue(FormatPhoneNumber(userData.UserMDTelefon))

					writer.WriteEndObject()

					'' publicContact
					' Provide a public contact if you want to give applicants the opportunity to ask questions about the job.
					writer.WritePropertyName("publicContact")
					writer.WriteStartObject()

					writer.WritePropertyName("salutation")
					If userData.Salutation = "Herr" Then
						writer.WriteValue("MR")
					Else
						writer.WriteValue("MS")
					End If
					writer.WritePropertyName("firstName")
					writer.WriteValue(userData.Firstname)
					writer.WritePropertyName("lastName")
					writer.WriteValue(userData.Lastname)
					writer.WritePropertyName("phone")
					writer.WriteValue(FormatPhoneNumber(userData.UserMDTelefon))
					writer.WritePropertyName("email")
					writer.WriteValue(userData.UserMDeMail)

					writer.WriteEndObject()

					'' publication
					' If the ad falls under the reporting obligation, the ad will be restricted for five business days.
					' After that period, the ad will be published In the Job-Room Public area If the publicDisplay flag Is Set, otherwise Not.
					writer.WritePropertyName("publication")
					writer.WriteStartObject()

					writer.WritePropertyName("startDate")
					writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.StartDate.GetValueOrDefault(Now)))
					'If vacancyStmpData.EndDate.HasValue Then
					'	writer.WritePropertyName("endDate")
					'	writer.WriteValue(String.Format("{0: yyyy-MM-dd}", vacancyStmpData.EndDate.GetValueOrDefault(Now)))
					'End If
					writer.WritePropertyName("euresDisplay")
					If vacancyStmpData.EuresDisplay.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WritePropertyName("publicDisplay")
					If vacancyStmpData.PublicDisplay.GetValueOrDefault(False) Then
						writer.WriteValue("true")
					Else
						writer.WriteValue("false")
					End If
					writer.WriteEndObject()

					writer.WriteEndObject()

				End Using

			Catch ex As Exception
				msgContent = ex.ToString
				m_SysInfo.AddErrorMessage(m_customerID, New ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME,
																	.MessageHeader = "AddAVAMAdvertisementToRAV.StringWriter", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})
				m_Logger.LogDebug(msgContent)

				m_TransmittedSTMPid = Nothing
				Return Nothing

			End Try


			Return sb

		End Function

		Private Function LoadAssignedQueryData(ByVal v_id As String) As Boolean
			Dim result As Boolean = True

			Try
				m_QueryResultData = m_JobData.LoadAssignedQueryData(m_customerID, v_id)

				If m_QueryResultData Is Nothing Then Return False

			Catch ex As Exception
				Dim msgContent As String = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedQueryData", .MessageContent = msgContent, .CreatedFrom = "System"})

				result = False
			End Try


			Return result

		End Function

		Public Async Function webserviceResponse(ByVal sb As StringBuilder, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As Task(Of HttpResponseMessage)

			Dim client As HttpClient = New HttpClient()

			If String.IsNullOrWhiteSpace(My.Settings.AVAM_UserName) Then
				m_UserName = JOBROOM_USER
			Else
				m_UserName = My.Settings.AVAM_UserName
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_Password) Then
				m_Password = JOBROOM_PASSWORD
			Else
				m_Password = My.Settings.AVAM_Password
			End If


			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomURI) Then
				m_JobroomURI = JOBROOM_URI
			Else
				m_JobroomURI = My.Settings.AVAM_JobroomURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomAllRecordURI) Then
				m_JobroomAllRecordURI = JOBROOM_RECORDS_URI
			Else
				m_JobroomAllRecordURI = My.Settings.AVAM_JobroomAllRecordURI
			End If
			If String.IsNullOrWhiteSpace(My.Settings.AVAM_JobroomSingleRecordURI) Then
				m_JobroomSingleRecordURI = JOBROOM_SINGLE_RECORDS_URI
			Else
				m_JobroomSingleRecordURI = My.Settings.AVAM_JobroomSingleRecordURI
			End If


			If m_asStaging Then
				m_UserName = STAGING_JOBROOM_USER
				m_Password = STAGING_JOBROOM_PASSWORD

				m_JobroomURI = STAGING_JOBROOM_URI
				m_JobroomAllRecordURI = STAGING_JOBROOM_RECORDS_URI
				m_JobroomSingleRecordURI = STAGING_JOBROOM_SINGLE_RECORDS_URI
			End If
			Dim baseUri As Uri = New Uri(m_JobroomURI)



			client.BaseAddress = baseUri

			If String.IsNullOrEmpty(User) Then
				Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("None")
				client.DefaultRequestHeaders.Authorization = authHeader

			Else

				Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", User, Password))))
				client.DefaultRequestHeaders.Authorization = authHeader

			End If

			Dim timeout As TimeSpan = TimeSpan.FromMinutes(5)
			client.Timeout = timeout

			Dim content As New StringContent(sb.ToString, System.Text.Encoding.UTF8, "application/json")

			Dim resp As HttpResponseMessage = Nothing
			Dim cancellationToken As CancellationToken

			If Method = "Post" Then
				resp = Await client.PostAsync(baseUri, content, cancellationToken)

			ElseIf Method = "Put" Then
				resp = Await client.PutAsync(baseUri, content, cancellationToken)

			ElseIf Method = "Delete" Then
				resp = Await client.DeleteAsync(baseUri, cancellationToken)

			ElseIf Method = "Get" Then
				resp = Await client.GetAsync(baseUri, cancellationToken)

			End If
			If resp.StatusCode = HttpStatusCode.BadRequest Then Return Nothing


			Return resp
		End Function

		'Private Function WebserviceResponse(ByVal sb As String, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As Boolean
		'	Dim msgContent As String
		'	Dim success As Boolean = True
		'	Dim result As String = String.Empty

		'	Dim req As New Chilkat.HttpRequest
		'	Dim http As New Chilkat.Http
		'	success = success AndAlso http.UnlockComponent(CHILKAT_COMPONENT_CODE)

		'	If Not success Then
		'		msgContent = http.LastErrorText
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.UnlockComponent", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		Return False
		'	End If

		'	Dim ssLogin As New Chilkat.SecureString
		'	Dim ssPassword As New Chilkat.SecureString
		'	Dim crypt As New Chilkat.Crypt2

		'	Dim json As New Chilkat.JsonObject
		'	success = success AndAlso json.Load(sb)

		'	http.Login = User
		'	http.Password = Password
		'	http.BasicAuth = True

		'	Dim resp As Chilkat.HttpResponse
		'	ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
		'	resp = http.PostJson(baseUri.ToString(), sb)

		'	If (resp Is Nothing) Then
		'		msgContent = http.LastErrorText
		'		m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.HttpResponse", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		m_Logger.LogError(http.LastErrorText)

		'		Return False
		'	Else
		'		result = resp.BodyStr
		'		m_ResultContent = result

		'		msgContent = result
		'		m_utility.AddNotifingToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .MessageHeader = "WebserviceResponse.resp", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

		'		If Not m_ResultContent Is Nothing Then
		'			success = success AndAlso ParseJSonResult(result)
		'		End If

		'	End If

		'	Return success
		'End Function

		Private Function WebservicePATCHResponse(ByVal sb As String, ByVal baseUri As Uri, ByVal User As String, ByVal Password As String) As WebServiceResult
			Dim msgContent As String
			Dim success As Boolean = True
			Dim result As New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}

			Dim req As New Chilkat.HttpRequest
			Dim http As New Chilkat.Http
			success = success AndAlso http.UnlockComponent(CHILKAT_COMPONENT_CODE)

			If Not success Then
				msgContent = http.LastErrorText
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebservicePATCHResponse.UnlockComponent", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = "component is not licensed!"}
			End If

			Dim json As New Chilkat.JsonObject
			success = success AndAlso json.Load(sb)

			Dim rest As New Chilkat.Rest
			Dim url As New Chilkat.Url
			url.ParseUrl(baseUri.ToString())

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
			success = rest.Connect(baseUri.Host, baseUri.Port, url.Ssl, True)
			success = rest.SetAuthBasic(User, Password)
			success = rest.AddHeader("Content-Type", "application/json; charset=UTF-8")
			Dim jsonResponse As String = rest.FullRequestString("PATCH", url.Path, json.Emit())

			If (rest.LastMethodSuccess <> True) Then
				m_Logger.LogError(rest.LastErrorText)

				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = rest.LastErrorText}
			End If

			If (rest.ResponseStatusCode <> 204) Then
				m_Logger.LogError(String.Format("rest.ResponseStatusText: {0} | jsonResponse: {1}", rest.ResponseStatusText, jsonResponse))

				Return New WebServiceResult With {.JobResult = False, .JobResultMessage = rest.ResponseStatusText}
			End If

			Return result

		End Function

		Private Function WebserviceGetRequest(ByVal baseUri As Uri, ByVal User As String, ByVal Password As String) As String
			Dim msgContent As String
			Dim success As Boolean = True
			Dim result As String = String.Empty

			Dim req As New Chilkat.HttpRequest
			Dim http As New Chilkat.Http
			Dim rest As New Chilkat.Rest

			Try
				success = success AndAlso http.UnlockComponent(CHILKAT_COMPONENT_CODE)

				Dim ssLogin As New Chilkat.SecureString
				Dim ssPassword As New Chilkat.SecureString
				Dim crypt As New Chilkat.Crypt2

				http.Login = User
				http.Password = Password
				http.BasicAuth = True

				Dim url As New Chilkat.Url
				url.ParseUrl(baseUri.ToString())

				ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12
				success = rest.SetAuthBasic(User, Password)
				success = rest.Connect(baseUri.Host, baseUri.Port, url.Ssl, True)
				success = rest.SendReqNoBody("GET", url.Path)
				If (success <> True) Then
					m_Logger.LogWarning(String.Format("baseUri: {1}{0}rest.SendReqNoBody.LastErrorText: {2}", vbNewLine, baseUri.ToString, rest.LastErrorText))

					msgContent = rest.LastErrorText
					m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.WebserviceGetRequest", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

					Return result
				End If


				Dim responseStatusCode As Integer = rest.ReadResponseHeader()
				If (responseStatusCode < 0) Then
					m_Logger.LogWarning(String.Format("baseUri: {1}{0}rest.ReadResponseHeader.LastErrorText: {2}", vbNewLine, baseUri.ToString, rest.LastErrorText))

					msgContent = rest.LastErrorText
					m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.WebserviceGetRequest", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

					Return result
				End If

				'  We expect a 200 response status.
				If (responseStatusCode <> 200) Then
					m_Logger.LogWarning(String.Format("baseUri: {1}{0}responseStatusCode: {2}", vbNewLine, baseUri.ToString, responseStatusCode))

					'  If the response status code is not 200, we could check for a redirect status code and
					'  then follow it, read the entire response (as shown here), or just call rest.Disconnect
					Dim errResponse As String = rest.ReadRespBodyString()
					If (Not rest.LastMethodSuccess) Then
						m_Logger.LogWarning(String.Format("baseUri: {1}{0}rest.ReadRespBodyString.LastErrorText: {2}", vbNewLine, baseUri.ToString, rest.LastErrorText))
						msgContent = rest.LastErrorText
						m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.WebserviceGetRequest", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})
					Else
						m_Logger.LogWarning(String.Format("baseUri: {1}{0}errResponse: {2}", vbNewLine, baseUri.ToString, errResponse))
						msgContent = errResponse
						m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.WebserviceGetRequest", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})
					End If

					Return result
				End If

				Dim bodyStream As New Chilkat.Stream
				'  Set a 10 second read timeout for the stream.
				'  (Give up if no data arrives within 10 seconds after calling a read method.)
				bodyStream.ReadTimeoutMs = 10000

				'  Create a background thread task to read the response body (which feeds
				'  it to the bodyStream object.)
				Dim readResponseBodyTask As Chilkat.Task = rest.ReadRespBodyStreamAsync(bodyStream, True)

				'  Start the task.
				success = readResponseBodyTask.Run()

				'  Read the HTTP response body until the "</head>" is seen, or until
				'  the end-of-stream is reached.
				Dim sbBody As New Chilkat.StringBuilder
				Dim exitLoop As Boolean = False
				While Not exitLoop And (bodyStream.EndOfStream <> True)

					Dim bodyText As String = bodyStream.ReadString()
					If (bodyStream.LastMethodSuccess = True) Then
						sbBody.Append(bodyText)
						If (sbBody.Contains("</head>", False)) Then
							exitLoop = True
						End If

					Else
						exitLoop = True
					End If

				End While

				'  Cancel the remainder of the task...
				readResponseBodyTask.Cancel()

				Dim maxWaitMs As Integer = 50
				rest.Disconnect(maxWaitMs)

				m_ResultContent = sbBody.GetAsString()

				result = sbBody.GetAsString()

			Catch ex As Exception
				m_Logger.LogWarning(String.Format("baseUri: {1}{0}Error: {2}", vbNewLine, baseUri.ToString, ex.ToString))

				msgContent = ex.ToString()
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "WebserviceResponse.WebserviceGetRequest", .MessageContent = msgContent, .CreatedFrom = m_UserData.UserFullname})

			End Try

			Return result

		End Function

		Private Function ParseJSonResult(ByVal jsonString As String) As Boolean
			Dim result As Boolean = True
			Dim searchResult As New JobroomSearchResultData
			Dim singleSearchResult As Boolean = True
			Dim contentArrayString As String = "content[i]."

			m_SearchResultData = New JobroomSearchResultData

			Dim json As New Chilkat.JsonObject
			Dim success As Boolean = True

			Try
				success = success AndAlso json.Load(jsonString)
				If Not success Then
					m_Logger.LogWarning(String.Format("loading jsonString is not succeeded: {0}", jsonString.ToString))

					Return False
				Else
					m_Logger.LogWarning(String.Format("loading jsonString was succeeded: {0}", jsonString.ToString))

				End If

				If ParseJSonError(json) Then
					m_SearchResultData.ErrorMessage.Content = jsonString

					m_Logger.LogWarning(String.Format("ParseJSonError json object is succeeded:{0}{1}", vbNewLine, jsonString.ToString))
					Return True
				Else
					m_Logger.LogWarning(String.Format("ParseJSonError json object was not succeeded. going to parse data.{0}{1}", vbNewLine, jsonString.ToString))
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("ParseJSonError json object was with error:{0}{1}", vbNewLine, ex.ToString))

				Return False
			End Try

			Dim totalElements As String = String.Empty
			Dim totalPages As String = String.Empty
			Dim currentPage As String = String.Empty
			Dim currentSize As String = String.Empty
			Dim first As Boolean = True
			Dim last As Boolean = True

			Try
				totalElements = json.StringOf("totalElements")
				totalPages = json.StringOf("totalPages")
				currentPage = json.StringOf("currentPage")
				currentSize = json.StringOf("currentSize")
				first = json.BoolOf("first")
				last = json.BoolOf("last")

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try


			singleSearchResult = Val(totalElements) = 0
			If singleSearchResult Then contentArrayString = String.Empty

			searchResult.CurrentPage = Val(currentPage)
			searchResult.CurrentSize = Val(currentSize)
			searchResult.First = first
			searchResult.Last = last
			searchResult.TotalElements = Math.Max(1, Val(totalElements))
			searchResult.TotalPages = Val(totalPages)

			searchResult.Content = New List(Of SPJobroomData)
			m_Logger.LogWarning(String.Format("jason parsing:{0}totalElements: {1}{0}first: {2}{0}last: {3}{0}totalPages: {4}{0}currentPage: {5}{0}currentSize: {6}{0}contentArrayString: {7}",
																				vbNewLine, Val(totalElements), first, last, totalPages, currentPage, currentSize, contentArrayString))
			Try

				If singleSearchResult Then
					Dim contentData = New SPJobroomData
					m_Logger.LogInfo(String.Format("ParseAssignedJsonResultContent: sending to parse single record."))

					contentData = ParseAssignedJsonResultContent(0, json, contentArrayString)
					If Not contentData Is Nothing Then m_Logger.LogInfo(String.Format("ParseAssignedJsonResultContent: parsing single record was succeeded."))

					searchResult.Content.Add(contentData)

				Else
					m_Logger.LogInfo(String.Format("ParseAssignedJsonResultContent: sending to to parse multiple records."))
					For i = 0 To Val(totalElements) - 1

						Dim contentData = New SPJobroomData
						m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: sending to to parse multiple records: {0}", i))
						contentData = ParseAssignedJsonResultContent(i, json, contentArrayString)
						If Not contentData Is Nothing Then m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: sending to to parse multiple records: {0} was succeeded", i))

						searchResult.Content.Add(contentData)

					Next
					m_Logger.LogInfo(String.Format("ParseAssignedJsonResultContent: parsing multiple record was succeeded."))

				End If

			Catch ex As Exception
				m_Logger.LogError(ex.ToString)

			End Try

			If searchResult Is Nothing Then m_Logger.LogError(String.Format("ParseAssignedJsonResultContent: searchResult was nothing."))
			m_SearchResultData = searchResult


			Return Not (searchResult Is Nothing)
		End Function

		Private Function ParseAssignedJsonResultContent(ByVal i As Integer, ByVal json As Chilkat.JsonObject, ByVal contentArrayString As String) As SPJobroomData
			Dim success As Boolean = True
			Dim result As New SPJobroomData

			Dim contentData = New SPJobroomData

			Try

				Try
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse ID."))

					contentData.ID = json.StringOf(String.Format("{0}id", contentArrayString))
					contentData.Status = json.StringOf(String.Format("{0}status", contentArrayString))
					contentData.SourceSystem = json.StringOf(String.Format("{0}sourceSystem", contentArrayString))
					contentData.ExternalReference = json.StringOf(String.Format("{0}externalReference", contentArrayString))
					contentData.StellennummerEgov = json.StringOf(String.Format("{0}stellennummerEgov", contentArrayString))
					contentData.StellennummerAvam = json.StringOf(String.Format("{0}stellennummerAvam", contentArrayString))
					contentData.Fingerprint = json.StringOf(String.Format("{0}fingerprint", contentArrayString))

				Catch ex As Exception
					m_Logger.LogError(String.Format("parsing ID: {0}", ex.ToString))

					Return Nothing
				End Try

				Dim booleanValue As Boolean
				Dim dateValue As Date
				Try
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse reportingObligation."))

					If Boolean.TryParse(json.StringOf(String.Format("{0}reportingObligation", contentArrayString)), booleanValue) Then
						contentData.ReportingObligation = booleanValue
					End If

					If DateTime.TryParse(json.StringOf(String.Format("{0}reportingObligationEndDate", contentArrayString)), dateValue) Then
						contentData.ReportingObligationEndDate = dateValue
					End If

					If Boolean.TryParse(json.StringOf(String.Format("{0}reportToAvam", contentArrayString)), booleanValue) Then
						contentData.ReportToAvam = booleanValue
					End If
					contentData.JobCenterCode = json.StringOf(String.Format("{0}jobCenterCode", contentArrayString))

					If DateTime.TryParse(json.StringOf(String.Format("{0}approvalDate", contentArrayString)), dateValue) Then
						contentData.ApprovalDate = dateValue
					End If
					If DateTime.TryParse(json.StringOf(String.Format("{0}rejectionDate", contentArrayString)), dateValue) Then
						contentData.RejectionDate = dateValue
					End If

					contentData.RejectionCode = json.StringOf(String.Format("{0}rejectionCode", contentArrayString))
					contentData.RejectionReason = json.StringOf(String.Format("{0}rejectionReason", contentArrayString))
					If DateTime.TryParse(json.StringOf(String.Format("{0}cancellationDate", contentArrayString)), dateValue) Then
						contentData.CancellationDate = dateValue
					End If
					contentData.CancellationCode = json.StringOf(String.Format("{0}cancellationCode", contentArrayString))

				Catch ex As Exception
					m_Logger.LogError(String.Format("parsing reportingObligation: {0}", ex.ToString))

					Return Nothing
				End Try


				Dim jobContent As New SPJobContentData

				Try
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse jobcontent."))

					jobContent.ExternalUrl = json.StringOf(String.Format("{0}jobContent.externalUrl", contentArrayString))


					Dim jobContentCompany As New SPCompanyData
					jobContentCompany.Name = json.StringOf(String.Format("{0}jobContent.company.name", contentArrayString))
					jobContentCompany.Street = json.StringOf(String.Format("{0}jobContent.company.street", contentArrayString))
					jobContentCompany.HouseNumber = json.StringOf(String.Format("{0}jobContent.company.houseNumber", contentArrayString))
					jobContentCompany.PostalCode = json.StringOf(String.Format("{0}jobContent.company.postalCode", contentArrayString))
					jobContentCompany.City = json.StringOf(String.Format("{0}jobContent.company.city", contentArrayString))
					jobContentCompany.CountryIsoCode = json.StringOf(String.Format("{0}jobContent.company.countryIsoCode", contentArrayString))
					jobContentCompany.PostOfficeBoxNumber = json.StringOf(String.Format("{0}jobContent.company.postOfficeBoxNumber", contentArrayString))
					jobContentCompany.PostOfficeBoxPostalCode = json.StringOf(String.Format("{0}jobContent.company.postOfficeBoxPostalCode", contentArrayString))
					jobContentCompany.PostOfficeBoxCity = json.StringOf(String.Format("{0}jobContent.company.postOfficeBoxCity", contentArrayString))
					jobContentCompany.Phone = json.StringOf(String.Format("{0}jobContent.company.phone", contentArrayString))
					jobContentCompany.Email = json.StringOf(String.Format("{0}jobContent.company.email", contentArrayString))
					jobContentCompany.Website = json.StringOf(String.Format("{0}jobContent.company.website", contentArrayString))
					If Boolean.TryParse(json.StringOf(String.Format("{0}jobContent.company.surrogate", contentArrayString)), booleanValue) Then
						jobContentCompany.Surrogate = booleanValue
					End If


					Dim jobContentEmployment As New SPEmploymentData
					If DateTime.TryParse(json.StringOf(String.Format("{0}jobContent.employment.startDate", contentArrayString)), dateValue) Then
						jobContentEmployment.StartDate = dateValue
					End If
					If DateTime.TryParse(json.StringOf(String.Format("{0}jobContent.employment.endDate", contentArrayString)), dateValue) Then
						jobContentEmployment.EndDate = dateValue
					End If
					If Boolean.TryParse(json.StringOf(String.Format("{0}jobContent.employment.shortEmployment", contentArrayString)), booleanValue) Then
						jobContentEmployment.ShortEmployment = booleanValue
					End If
					If Boolean.TryParse(json.StringOf(String.Format("{0}jobContent.employment.immediately", contentArrayString)), booleanValue) Then
						jobContentEmployment.Immediately = booleanValue
					End If
					If Boolean.TryParse(json.StringOf(String.Format("{0}jobContent.employment.permanent", contentArrayString)), booleanValue) Then
						jobContentEmployment.Permanent = booleanValue
					End If
					jobContentEmployment.WorkloadPercentageMin = json.StringOf(String.Format("{0}jobContent.employment.workloadPercentageMin", contentArrayString))
					jobContentEmployment.WorkloadPercentageMax = json.StringOf(String.Format("{0}jobContent.employment.workloadPercentageMax", contentArrayString))

				Catch ex As Exception
					m_Logger.LogError(String.Format("parsing jobcontent: {0}", ex.ToString))

					Return Nothing
				End Try


				'Dim jobContentLocation As New SPLocationData
				'Dim jobContentApplyChannel As New SPApplyChannelData
				'Dim jobContentApplyPublicContact As New SPPublicContactData

				'data.jobContentLocationRemarks As String = json.StringOf(String.Format("{0}jobContent.location.remarks", contentArrayString))
				'data.jobContentLocationCity As String = json.StringOf(String.Format("{0}jobContent.location.city", contentArrayString))
				'data.jobContentLocationPostalCode As String = json.StringOf(String.Format("{0}jobContent.location.postalCode", contentArrayString))
				'data.jobContentLocationCommunalCode As String = json.StringOf(String.Format("{0}jobContent.location.communalCode", contentArrayString))
				'data.jobContentLocationRegionCode As String = json.StringOf(String.Format("{0}jobContent.location.regionCode", contentArrayString))
				'data.jobContentLocationCantonCode As String = json.StringOf(String.Format("{0}jobContent.location.cantonCode", contentArrayString))
				'data.jobContentLocationCountryIsoCode As String = json.StringOf(String.Format("{0}jobContent.location.countryIsoCode", contentArrayString))
				'data.jobContentLocationCoordinatesLongitude As String = json.StringOf(String.Format("{0}jobContent.location.coordinates.longitude", contentArrayString))
				'data.jobContentLocationCoordinatesLatitude As String = json.StringOf(String.Format("{0}jobContent.location.coordinates.latitude", contentArrayString))
				'data.jobContentApplyChannelMailAddress As String = json.StringOf(String.Format("{0}jobContent.applyChannel.mailAddress", contentArrayString))
				'data.jobContentApplyChannelEmailAddress As String = json.StringOf(String.Format("{0}jobContent.applyChannel.emailAddress", contentArrayString))
				'data.jobContentApplyChannelPhoneNumber As String = json.StringOf(String.Format("{0}jobContent.applyChannel.phoneNumber", contentArrayString))
				'data.jobContentApplyChannelFormUrl As String = json.StringOf(String.Format("{0}jobContent.applyChannel.formUrl", contentArrayString))
				'data.jobContentApplyChannelAdditionalInfo As String = json.StringOf(String.Format("{0}jobContent.applyChannel.additionalInfo", contentArrayString))
				'data.jobContentPublicContactSalutation As String = json.StringOf(String.Format("{0}jobContent.publicContact.salutation", contentArrayString))
				'data.jobContentPublicContactFirstName As String = json.StringOf(String.Format("{0}jobContent.publicContact.firstName", contentArrayString))
				'data.jobContentPublicContactLastName As String = json.StringOf(String.Format("{0}jobContent.publicContact.lastName", contentArrayString))
				'data.jobContentPublicContactPhone As String = json.StringOf(String.Format("{0}jobContent.publicContact.phone", contentArrayString))
				'data.jobContentPublicContactEmail As String = json.StringOf(String.Format("{0}jobContent.publicContact.email", contentArrayString))
				'data.publicationStartDate As String = json.StringOf(String.Format("{0}publication.startDate", contentArrayString))
				'data.publicationEndDate As String = json.StringOf(String.Format("{0}publication.endDate", contentArrayString))
				'data.publicationEuresDisplay As Boolean = json.BoolOf(String.Format("{0}publication.euresDisplay", contentArrayString))
				'data.publicationEuresAnonymous As Boolean = json.BoolOf(String.Format("{0}publication.euresAnonymous", contentArrayString))
				'data.publicationPublicDisplay As Boolean = json.BoolOf(String.Format("{0}publication.publicDisplay", contentArrayString))
				'data.publicationPublicAnonymous As Boolean = json.BoolOf(String.Format("{0}publication.publicAnonymous", contentArrayString))
				'data.publicationRestrictedDisplay As Boolean = json.BoolOf(String.Format("{0}publication.restrictedDisplay", contentArrayString))
				'data.publicationRestrictedAnonymous As Boolean = json.BoolOf(String.Format("{0}publication.restrictedAnonymous", contentArrayString))
				'data.jobContentLocationCoordinates As String = json.StringOf(String.Format("{0}jobContent.location.coordinates", contentArrayString))

				Dim jobDescription As New List(Of SPJobDescriptionData)

				Dim j = 0
				Try
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse jobDescriptions."))

					Dim count_j = json.SizeOfArray(String.Format("{0}jobContent.jobDescriptions", contentArrayString))
					Dim languageIsoCode As String = String.Empty
					While j < count_j
						Dim desData As New SPJobDescriptionData

						json.J = j
						desData.LanguageIsoCode = json.StringOf(String.Format("{0}jobContent.jobDescriptions[j].languageIsoCode", contentArrayString))
						desData.Title = json.StringOf(String.Format("{0}jobContent.jobDescriptions[j].title", contentArrayString))
						desData.Description = json.StringOf(String.Format("{0}jobContent.jobDescriptions[j].description", contentArrayString))

						jobDescription.Add(desData)
						j = j + 1
					End While
					jobContent.JobDescriptions = jobDescription


					j = 0
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse jobContent.employment.workForms."))
					count_j = json.SizeOfArray(String.Format("{0}jobContent.employment.workForms", contentArrayString))
					While j < count_j
						json.J = j
						j = j + 1
					End While

					Dim jobOccupations As New List(Of SPOccupationsData)
					j = 0
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse jobContent.occupations."))
					count_j = json.SizeOfArray(String.Format("{0}jobContent.occupations", contentArrayString))
					While j < count_j
						Dim desData As New SPOccupationsData

						json.J = j
						desData.AvamOccupationCode = CInt(Val(json.StringOf(String.Format("{0}jobContent.occupations[j].avamOccupationCode", contentArrayString))))
						desData.WorkExperience = json.StringOf(String.Format("{0}jobContent.occupations[j].workExperience", contentArrayString))
						desData.EducationCode = json.StringOf(String.Format("{0}jobContent.occupations[j].educationCode", contentArrayString))

						jobOccupations.Add(desData)

						j = j + 1
					End While
					jobContent.Occupations = jobOccupations

					Dim jobLanguageSkills As New List(Of SPLanguageSkillsData)
					j = 0
					m_Logger.LogWarning(String.Format("ParseAssignedJsonResultContent: start to parse jobContent.languageSkills."))
					count_j = json.SizeOfArray(String.Format("{0}jobContent.languageSkills", contentArrayString))
					While j < count_j
						Dim desData As New SPLanguageSkillsData

						json.J = j
						desData.LanguageIsoCode = json.StringOf(String.Format("{0}jobContent.languageSkills[j].languageIsoCode", contentArrayString))
						desData.SpokenLevel = json.StringOf(String.Format("{0}jobContent.languageSkills[j].spokenLevel", contentArrayString))
						desData.WrittenLevel = json.StringOf(String.Format("{0}jobContent.languageSkills[j].writtenLevel", contentArrayString))

						jobLanguageSkills.Add(desData)
						j = j + 1
					End While
					jobContent.LanguageSkills = jobLanguageSkills
					contentData.JobContent = jobContent


				Catch ex As Exception
					m_Logger.LogError(String.Format("parsing jobDescription: {0}", ex.ToString))

					Return Nothing
				End Try


				result = contentData

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString()))

				Return Nothing

			End Try

			Return result

		End Function

		Private Function ParseJSonError(ByVal json As Chilkat.JsonObject) As Boolean
			Dim result As Boolean = True
			Dim searchResult As New JobroomSearchResultData
			Dim errordata As New SPErrorData With {.Content = String.Empty}

			Try
				Dim type As String = json.StringOf("type")
				Dim title As String = json.StringOf("title")
				Dim status As String = json.StringOf("status")
				Dim detail As String = json.StringOf("detail")
				Dim path As String = json.StringOf("path")
				Dim message As String = json.StringOf("message")

				If String.IsNullOrWhiteSpace(title) Then Return False

				errordata.Content = json.ToString
				errordata.Title = title
				errordata.Message = message
				errordata.Status = status
				errordata.Detail = detail

				m_SearchResultData.ErrorMessage = errordata


			Catch ex As Exception
				m_Logger.LogError(String.Format("{0}", ex.ToString))

				Return False
			End Try

			Return Not (errordata Is Nothing)

		End Function


		Private Function AddAVAMAdvertismentData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal notify As Boolean, ByVal avamData As SPAVAMJobCreationData) As Boolean
			Dim result As Boolean = True

			Try
				m_Logger.LogInfo(String.Format("CustomerID: {1} | userid: {2} | vacancyNumber: {3} | notify: {4}{0}adding advertisment data", vbNewLine, customerID, userid, vacancyNumber, notify))
				result = result AndAlso m_JobData.AddAVAMAdvertismentData(customerID, userid, vacancyNumber, notify, avamData)

			Catch ex As Exception
				m_Logger.LogError(String.Format("CustomerID: {1} | userid: {2} | vacancyNumber: {3} | notify: {4}{0}{5}", vbNewLine, customerID, userid, vacancyNumber, notify, ex.ToString))

				Dim msgContent As String = ex.ToString
				m_SysInfo.AddErrorMessage(customerID, New ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME,
															 .MessageHeader = "AddAVAMAdvertismentData", .MessageContent = msgContent, .CreatedFrom = userid})

				result = False

			End Try

			Return result

		End Function

		Private Function AddNotifyDataIntoDatabase(ByVal customerID As String, ByVal userid As String, ByVal avamData As SPAVAMJobCreationData) As Boolean
			Dim result As Boolean = True

			Try
				result = result AndAlso m_JobData.AddAVAMNotifyResultData(customerID, userid, avamData)
				m_Logger.LogInfo(String.Format("CustomerID: {1} | userid: {2}{0}added notify data. avamData.RecID: {3}", vbNewLine, customerID, userid, avamData.RecID))

			Catch ex As Exception
				m_Logger.LogError(String.Format("CustomerID: {1} | userid: {2}{0}{3}", vbNewLine, customerID, userid, ex.ToString))

				Dim msgContent As String = ex.ToString
				m_SysInfo.AddErrorMessage(customerID, New ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME,
																	.MessageHeader = "AddNotifyDataIntoDatabase", .MessageContent = msgContent, .CreatedFrom = userid})

				result = False
			End Try

			Return result

		End Function

		Private Function UpdateNotifyDataIntoDatabase(ByVal customerID As String, ByVal userid As String, ByVal avamData As SPAVAMJobCreationData) As Boolean
			Dim result As Boolean = True

			Try
				result = result AndAlso m_JobData.UpdateAVAMNotifyResultData(customerID, userid, avamData)

			Catch ex As Exception
				Dim msgContent As String = ex.ToString
				m_SysInfo.AddErrorMessage(customerID, New ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME,
																	.MessageHeader = "UpdateNotifyDataIntoDatabase", .MessageContent = msgContent, .CreatedFrom = userid})

				result = False
			End Try

			Return result

		End Function

#End Region


#Region "helpers"

		Private Function FormatPhoneNumber(ByRef phoneNumber As String) As String
			Dim result As String = phoneNumber
			Dim existsCountryCode As Boolean = False

			If String.IsNullOrWhiteSpace(phoneNumber) Then Return result
			result = result.Replace(" ", "")
			result = result.Replace("(0)", "")

			If result.StartsWith("00") OrElse result.StartsWith("++") Then
				result = String.Format("+{0}", result.Substring(2, Len(result) - 2))
				existsCountryCode = True
			End If
			If result.StartsWith("+") Then
				existsCountryCode = True
			End If

			If result.StartsWith("0") Then
				result = String.Format("{0}", result.Substring(1, Len(result) - 1))
			End If
			If Not existsCountryCode Then result = String.Format("+41{0}", result)


			Return result
		End Function

#End Region


	End Class

End Namespace