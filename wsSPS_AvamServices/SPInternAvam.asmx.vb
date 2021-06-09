
Imports System.Web.Services
Imports System.ComponentModel

Imports wsSPS_AvamServices.SPUtilities
Imports wsSPS_AvamServices.DatabaseAccessBase
Imports wsSPS_AvamServices.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_AvamServices.JobPlatform.AVAM
Imports wsSPS_AvamServices.Logging


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://avam.sputnik-it.com/wsSPS_AvamServices/SPInternAvam.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPInternAvam
	Inherits System.Web.Services.WebService


#Region "private consts"

	Private Const ASMX_SERVICE_NAME As String = "SPInternAvam"

#End Region

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_AVAMData As AVAMDatabaseAccess



	Public Sub New()

		m_utility = New ClsUtilities
		m_AVAMData = New AVAMDatabaseAccess(My.Settings.ConnStr_spJobplattforms, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


#Region "AVAM transmitting"

	<WebMethod()>
	Function TransmittVacancyToAVAMProtal_0(ByVal customerID As String, ByVal userID As String, ByVal asStaging As Boolean, ByVal vacancyData As sp_WebServiceUtility.VacancyMasterData,
																							ByVal vacancyJobCHData As sp_WebServiceUtility.VacancyInseratJobCHData, ByVal vacancyStmpData As sp_WebServiceUtility.VacancyStmpSettingData,
																							ByVal vacancyStmpLanguageData As List(Of sp_WebServiceUtility.VacancyJobCHLanguageData),
																							ByVal MDData As sp_WebServiceUtility.MandantData,
																							ByVal userData As AdvisorData, ByVal employerData As sp_WebServiceUtility.CustomerMasterData,
																							ByVal jobNumber As Integer?, ByVal language As String) As AVAMJobCreationData ' sp_WebServiceUtility.JobPlatform.SPAVAMJobCreationData
		Dim result As AVAMJobCreationData = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("*** starting customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))
			result = New AVAMJobCreationData
			result = m_AVAMData.CreateAVAMAdvertisement(m_customerID, userID, asStaging, vacancyData,
																							vacancyJobCHData, vacancyStmpData,
																							vacancyStmpLanguageData,
																							MDData,
																							userData, employerData, jobNumber.GetValueOrDefault(0), language)
			m_Logger.LogInfo(String.Format("*** ending customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "TransmittVacancyToAVAMProtal", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally
		End Try


		Return (result)
	End Function

	<WebMethod>
	Public Function BeginGetAuthorRoyalties(ByVal customerID As String, ByVal userID As String,
											ByVal asStaging As Boolean, ByVal vacancyData As sp_WebServiceUtility.VacancyMasterData,
											ByVal vacancyJobCHData As sp_WebServiceUtility.VacancyInseratJobCHData, ByVal vacancyStmpData As sp_WebServiceUtility.VacancyStmpSettingData,
											ByVal vacancyStmpLanguageData As List(Of sp_WebServiceUtility.VacancyJobCHLanguageData),
											ByVal MDData As sp_WebServiceUtility.MandantData,
											ByVal userData As AdvisorData, ByVal employerData As sp_WebServiceUtility.CustomerMasterData,
											ByVal jobNumber As Integer?, ByVal language As String,
											ByVal callback As AsyncCallback, ByVal asyncState As Object) As IAsyncResult

		Dim state = New AVAMJobCreationData
		state.originalState = asyncState

		state.OriginalCallback = callback
		Dim chainedCallback As AsyncCallback = New AsyncCallback(AddressOf AuthorRoyaltiesCallback)

		Return remoteService.BeginGetAuthors(chainedCallback, state)
	End Function

	Public Sub AuthorRoyaltiesCallback(ByVal ar As IAsyncResult)
		Dim state = CType(ar.AsyncState, AVAMJobCreationData)
		Dim result As AVAMJobCreationData = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("*** starting customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))
			result = New AVAMJobCreationData
			result = m_AVAMData.CreateAVAMAdvertisement(m_customerID, userID, asStaging, vacancyData,
																							vacancyJobCHData, vacancyStmpData,
																							vacancyStmpLanguageData,
																							MDData,
																							userData, employerData, jobNumber.GetValueOrDefault(0), language)
			m_Logger.LogInfo(String.Format("*** ending customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "TransmittVacancyToAVAMProtal", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally

		End Try

		'Return (result)
	End Sub

	<WebMethod>
	Public Function EndGetAuthorRoyalties(ByVal asyncResult As IAsyncResult) As AVAMJobCreationData
		Return remoteService.EndReturnedStronglyTypedDS(asyncResult)
	End Function



	<WebMethod()>
	Function AddAVAMDataToDataBase(ByVal customerID As String, ByVal userID As String, ByVal asStaging As Boolean, ByVal vacancyData As sp_WebServiceUtility.VacancyMasterData,
																							ByVal vacancyJobCHData As sp_WebServiceUtility.VacancyInseratJobCHData, ByVal vacancyStmpData As sp_WebServiceUtility.VacancyStmpSettingData,
																							ByVal vacancyStmpLanguageData As List(Of sp_WebServiceUtility.VacancyJobCHLanguageData),
																							ByVal MDData As sp_WebServiceUtility.MandantData,
																							ByVal userData As AdvisorData, ByVal employerData As sp_WebServiceUtility.CustomerMasterData,
																							ByVal jobNumber As Integer?, ByVal language As String) As System.IAsyncResult
		Dim result As AVAMJobCreationData = Nothing
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("*** starting customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))
			result = New AVAMJobCreationData
			result = m_AVAMData.AddAVAMNotifyResultData(m_customerID, userID, asStaging, vacancyData,
																							vacancyJobCHData, vacancyStmpData,
																							vacancyStmpLanguageData,
																							MDData,
																							userData, employerData, jobNumber.GetValueOrDefault(0), language)
			m_Logger.LogInfo(String.Format("*** ending customerID: {0} | userID: {1} | asStaging: {2} | VakNr: {3}", customerID, userID, asStaging, vacancyData.VakNr))

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddAVAMDataToDataBase", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally
		End Try


		Return (result)
	End Function


	<WebMethod()>
	Function CacelAssignedAdvertismentData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData, ByVal jobroomID As String, ByVal reasonEnum As AVAMAdvertismentCancelReasonENUM) As WebServiceResult
		Dim result As New WebServiceResult With {.JobResult = True, .JobResultMessage = String.Empty}
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("customerID: {0} | UserFullname: {1} | asStaging: {2} | jobroomID: {3}", customerID, userData.UserFullname, asStaging, jobroomID))
			result = m_AVAMData.CancelAssignedAdvertisementData(m_customerID, asStaging, userData, jobroomID, reasonEnum)


		Catch ex As Exception
			Dim msgContent = String.Format("{0} | m_customerID: {1} | jobroomID: {2} | reasonEnum: {3}", ex.ToString, m_customerID, jobroomID, reasonEnum)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "CacelAssignedAdvertismentData", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally
		End Try

		Return (result)
	End Function

	<WebMethod()>
	Function LoadAssignedAVAMQueryResultData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData, ByVal jobroomID As String) As AVAMJobCreationData
		Dim result As AVAMJobCreationData = Nothing
		m_customerID = customerID
		Dim msgContent As String = String.Format("Customer_ID: {0} | JobroomID: {1}", m_customerID, jobroomID)

		Try
			m_Logger.LogInfo(String.Format("customerID: {0} | UserFullname: {1} | asStaging: {2} | jobroomID: {3}", customerID, userData.UserFullname, asStaging, jobroomID))
			m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyArt = 99, .NotifyHeader = String.Format("{0}: LoadAssignedAVAMQueryResultData", ASMX_SERVICE_NAME), .NotifyComments = msgContent, .CreatedFrom = userData.UserFullname})
			result = New AVAMJobCreationData
			result = m_AVAMData.LoadAssignedAdvertisementQueryResultData(m_customerID, asStaging, userData, jobroomID)


		Catch ex As Exception
			msgContent = String.Format("{0} | {1} >>> {2}", ex.ToString, m_customerID, jobroomID)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAVAMQueryResultData", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally
		End Try

		Return (result)
	End Function


	<WebMethod()>
	Function LoadAVAMQueryResultData(ByVal customerID As String, ByVal asStaging As Boolean, ByVal userData As AdvisorData) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			m_Logger.LogInfo(String.Format("customerID: {0} | UserFullname: {1} | asStaging: {2}", customerID, userData.UserFullname, asStaging))
			result = m_AVAMData.LoadAdvertisementsQueryResultData(m_customerID, asStaging, userData)


		Catch ex As Exception
			Dim msgContent = String.Format("{0} | {1}", ex.ToString, m_customerID)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAVAMQueryResultData", .MessageContent = msgContent, .CreatedFrom = userData.UserFullname})
		Finally
		End Try

		Return (result)
	End Function


#End Region





End Class

