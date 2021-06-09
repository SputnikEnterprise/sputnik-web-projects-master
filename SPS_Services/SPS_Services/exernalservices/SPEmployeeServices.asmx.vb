
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.WOSUtilities
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.WOSUtilities.WOSDbAccess
Imports wsSPS_Services.WOSInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects


' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/externalservices/SPEmployeeServices.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPEmployeeServices
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPEmployeeServices"


	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_WOSData As WOSDatabaseAccess
	Private m_NewWOS As WOSDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_WOSData = New WOSDatabaseAccess(My.Settings.ConnStr_spContract, Language.German)
		m_NewWOS = New WOSDatabaseAccess(My.Settings.ConnStr_New_spContract, Language.German)

	End Sub

	<WebMethod(Description:="List available employee data")>
	Function LoadAvailableEmployee(ByVal customerID As String, ByVal customerWosID As String, ByVal qualification As String, ByVal location As String, ByVal canton As String, ByVal brunchOffice As String) As AvailableEmployeeDTO()
		Dim result As List(Of AvailableEmployeeDTO) = Nothing
		m_customerID = customerID
		If String.IsNullOrWhiteSpace(customerWosID) Then Return Nothing

		Try
			result = New List(Of AvailableEmployeeDTO)
			result = m_NewWOS.LoadWOSAvailableEmployeeData(customerID, customerWosID, qualification, location, canton, brunchOffice)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAvailableEmployee", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="List assigned available employee data")>
	Function LoadAssignedAvailableEmployeeData(ByVal customerID As String, ByVal customerWosID As String, ByVal employeeNumber As Integer) As AvailableEmployeeDTO
		Dim result As AvailableEmployeeDTO = Nothing
		m_customerID = customerID
		If String.IsNullOrWhiteSpace(customerWosID) Then Return Nothing

		Try
			result = New AvailableEmployeeDTO
			result = m_NewWOS.LoadAssignedAvailableEmployeeData(customerID, customerWosID, employeeNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="List assigned available employee application data")>
	Function LoadAssignedAvailableEmployeeApplicationData(ByVal customerID As String, ByVal customerWosID As String, ByVal employeeNumber As Integer) As AvailableEmployeeApplicationFields
		Dim result As AvailableEmployeeApplicationFields = Nothing
		m_customerID = customerID
		If String.IsNullOrWhiteSpace(customerWosID) Then Return Nothing

		Try
			result = New AvailableEmployeeApplicationFields
			result = m_NewWOS.LoadAssignedAvailableEmployeeApplicationData(customerID, customerWosID, employeeNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeApplicationData", .MessageContent = msgContent})
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="loads assigned available employee document data")>
	Function LoadAssignedAvailableEmployeeDocumentData(ByVal customerID As String, ByVal customerWosID As String, ByVal employeeNumber As Integer) As AvailableEmployeeTemplateData()
		Dim result As List(Of AvailableEmployeeTemplateData) = Nothing
		m_customerID = customerID
		If String.IsNullOrWhiteSpace(customerWosID) Then Return Nothing

		Try
			result = New List(Of AvailableEmployeeTemplateData)
			result = m_NewWOS.LoadWOSAvailableEmployeeDocumentData(customerID, customerWosID, employeeNumber)


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAvailableEmployeeDocumentData", .MessageContent = msgContent})
		Finally
		End Try


		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


#Region "Hilf-Funktionen..."


#End Region



End Class