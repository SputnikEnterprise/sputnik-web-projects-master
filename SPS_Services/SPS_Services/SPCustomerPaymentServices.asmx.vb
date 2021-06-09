
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.SPUtilities

Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DocumentScan
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.WOSInfo


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPCustomerPaymentServices.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPCustomerPaymentServices
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPCustomerPaymentServices"


#Region "Constants"

	Private Const SOLVENCY_QUICK_CHECK = "SOLVENCY_QUICK_CHECK"
	Private Const SOLVENCY_BUSINESS_CHECK = "SOLVENCY_BUSINESS_CHECK"

	Private Const SMS_ECALL = "SMS_ECALL_SEND"
	Private Const CVL_Scan = "CVLIZER_SCAN"

#End Region


	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function



	<WebMethod(Description:="Zur Auflistung der Kunden-Liste auf der Client für PaymentService")>
	Function GetCustomerListOfServices() As CustomerSearchResultDTO()
		Dim result As List(Of CustomerSearchResultDTO) = Nothing

		Try
			result = New List(Of CustomerSearchResultDTO)
			result = m_SysInfo.LoadCustomerServices()

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerListOfServices", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="denieded service list of customers")>
	Function GetCustomerDeniedListOfServices(ByVal customerGuid As String) As String()
		Dim result As List(Of String) = Nothing

		Try
			result = New List(Of String)
			result = m_SysInfo.LoadCustomerDeniedServices(customerGuid)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerDeniedListOfServices", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Zur Auflistung der Kunden-UserName-Liste auf der Client für PaymentService")>
	Function GetCustomerUserNameListOfServices(ByVal customerGuid As String) As CustomerUserNameSearchResultDTO()
		Dim result As List(Of CustomerUserNameSearchResultDTO) = Nothing
		m_customerID = customerGuid

		Try
			result = New List(Of CustomerUserNameSearchResultDTO)
			result = m_SysInfo.LoadCustomerServicesAdvisor(m_customerID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCustomerUserNameListOfServices", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <param name="serviceDate">The service date.</param>
	<WebMethod(Description:="Zur Auflistung der Aktuellen Liste auf der Client")>
	Function GetCurrentListOfServices(ByVal customerGuid As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As PaymentSearchResultDTO()
		Dim result As List(Of PaymentSearchResultDTO) = Nothing
		m_customerID = customerGuid

		Try
			result = New List(Of PaymentSearchResultDTO)
			result = m_SysInfo.LoadCurrentServicesData(m_customerID, userName, serviceDate, serviceName, searchYear, searchMonth)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCurrentListOfServices", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <param name="serviceDate">The paid service date.</param>
	<WebMethod(Description:="Zur Auflistung der verrechnete Dienste auf der Client")>
	Function GetPaidListOfServices(ByVal customerGuid As String, ByVal userName As String, ByVal serviceDate As String, ByVal serviceName As String, ByVal searchYear As Integer, ByVal searchMonth As Integer) As PaidSearchResultDTO()
		Dim result As List(Of PaidSearchResultDTO) = Nothing
		m_customerID = customerGuid

		Try
			result = New List(Of PaidSearchResultDTO)
			result = m_SysInfo.LoadPaidDataServices(m_customerID, userName, serviceDate, serviceName, searchYear, searchMonth)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPaidListOfServices", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <param name="JobGuid">The paid JobID .</param>
	<WebMethod(Description:="Zur Auflistung der der definitiven Daten von eCall auf der Client")>
	Function GeteCallDataForSelectedJobID(ByVal customerGuid As String, ByVal JobGuid As String) As PaidSearchResultDTO()
		Dim result As List(Of PaidSearchResultDTO) = Nothing
		m_customerID = customerGuid

		Try
			result = New List(Of PaidSearchResultDTO)
			result = m_SysInfo.LoadECallDataForAssignedJob(m_customerID, JobGuid)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GeteCallDataForSelectedJobID", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <param name="JobGuid">The paid JobID .</param>
	<WebMethod(Description:="Save Data to eCall Db")>
	Function UpdateeCallResponseDataForSelectedJobID(ByVal customerGuid As String, ByVal JobGuid As String, ByVal AuthorizedCredit As Decimal?, ByVal AuthorizedItems As Decimal?) As Boolean
		Dim result As Boolean = True
		m_customerID = customerGuid

		Try
			result = m_SysInfo.UpdateECallResponseDataForAssignedJob(m_customerID, JobGuid, AuthorizedCredit, AuthorizedItems)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateeCallResponseDataForSelectedJobID", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Logt die Verwendung von Solvenzprüfungen.")>
	Function LogSolvencyCheckUsage(ByVal customerGuid As String, ByVal userGuid As String, ByVal userName As String, ByVal solvencyCheckType As String, ByVal serviceDate As DateTime) As Boolean
		Dim result As Boolean = True
		m_customerID = customerGuid

		Try
			result = m_SysInfo.AddSolvencyUsage(m_customerID, userGuid, userName, solvencyCheckType, serviceDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LogSolvencyCheckUsage", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Logt die Verwendung von eCall SMS-Nachrichten.")>
	Function LogeCallUsage(ByVal customerGuid As String, ByVal userGuid As String, ByVal userName As String, ByVal CheckType As String, ByVal JobID As String, ByVal UsedPoints As String, ByVal serviceDate As DateTime) As Boolean
		Dim result As Boolean = True
		m_customerID = customerGuid

		Try
			result = m_SysInfo.AddECallUsage(m_customerID, userGuid, userName, CheckType, JobID, UsedPoints, serviceDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LogeCallUsage", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Logt die Anmeldungen von Sputnik.")>
	Function LogSputnikLoginUsage(ByVal customerGuid As String, ByVal customerName As String,
															ByVal userGuid As String, ByVal userName As String, ByVal domainUsername As String,
															ByVal machineName As String, ByVal domainName As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerGuid

		Try
			result = m_SysInfo.AddSputnikLoginUsage(m_customerID, customerName, userGuid, userName, domainUsername, machineName, domainName)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LogSputnikLoginUsage", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


End Class
