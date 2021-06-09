Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObjects.TaxInfoService
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPEmployeeTaxInfoService.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPEmployeeTaxInfoService
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPEmployeeTaxInfoService"

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_PublicData As PublicDataDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities

		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub


	<WebMethod()>
	Public Function GetTaxInfo(ByVal canton As String, ByVal year As Integer) As TaxDataDTO
		Dim result As TaxDataDTO = Nothing
		m_customerID = String.Empty

		If canton Is Nothing OrElse String.IsNullOrEmpty(canton) OrElse canton.Length < 2 OrElse year < 2010 Then Return Nothing

		Try
			Dim data = m_PublicData.LoadTaxInfoData(m_customerID, canton, year)

			If data Is Nothing Then Return Nothing

			Dim taxDataWithTranslations = New TaxDataDTO(data.ToArray())
			result = taxDataWithTranslations


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetTaxInfo", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Public Function LoadTaxInfoData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As TaxDataDTO
		Dim result As TaxDataDTO = Nothing
		m_customerID = customerID

		If canton Is Nothing OrElse String.IsNullOrEmpty(canton) OrElse canton.Length < 2 OrElse year < 2010 Then Return Nothing
		Try

			If Not String.IsNullOrWhiteSpace(customerID) Then
				Dim data = m_PublicData.LoadTaxInfoData(m_customerID, canton, year)
				If data Is Nothing Then Return Nothing

				result = New TaxDataDTO(data.ToArray())
			End If

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxInfoData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="loads tax code data")>
	Public Function LoadTaxCodeData(ByVal customerID As String, ByVal userName As String, ByVal language As String) As TaxCodeDataDTO()
		Dim result As List(Of TaxCodeDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadTaxCodeData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxCodeData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="loads tax church code data")>
	Public Function LoadTaxChurchCodeData(ByVal customerID As String, ByVal userName As String, ByVal language As String) As TaxChurchCodeDataDTO()
		Dim result As List(Of TaxChurchCodeDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadTaxChurchCodeData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxChurchCodeData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="loads number of children for given canton, tax and church code data")>
	Public Function LoadTaxNumberOfChilderData(ByVal customerID As String, ByVal userName As String, ByVal language As String, ByVal canton As String, ByVal code As String, ByVal church As String) As Integer()
		Dim result As List(Of Integer) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadTaxNumberOfChildrenData(m_customerID, userName, language, canton, code, church)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxNumberOfChilderData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Quellensteuerdaten auf der Client")>
	Public Function GetQstData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal einkommen As Double, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As QstDataDTO()
		Dim result As List(Of QstDataDTO) = Nothing
		m_customerID = customerID

		Try
			' TODO: should look for allowed customers!

			result = m_PublicData.LoadQstData(m_customerID, canton, year, einkommen, childern, qstGroup, kirchsteuer, geschlecht)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetQstData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod(Description:="Test ob der eingetragene QST-Code korrekt ist")>
	Public Function LoadAllowedQstCode(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As QstDataAllowedDTO
		Dim result As QstDataAllowedDTO = Nothing
		m_customerID = customerID

		Try
			' TODO: Should be open for everybody?

			result = m_PublicData.LoadAllowedQstData(m_customerID, canton, year, childern, qstGroup, kirchsteuer, geschlecht)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllowedQstCode", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod(Description:="Test ob der eingetragene QST-Code korrekt ist")>
	Public Function GetIsAllowedQstCode(ByVal canton As String, ByVal year As Integer, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As QstDataAllowedDTO
		Dim result As QstDataAllowedDTO = Nothing
		m_customerID = String.Empty

		Try
			result = m_PublicData.LoadAllowedQstData(m_customerID, canton, year, childern, qstGroup, kirchsteuer, geschlecht)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetIsAllowedQstCode", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

	<WebMethod()>
	Public Function LoadCommunityData(ByVal customerID As String, ByVal userName As String, ByVal canton As String, ByVal language As String) As CommunityDataDTO()
		Dim result As List(Of CommunityDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadCommunityData(m_customerID, userName, canton, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCommunityData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function


#Region "employmenttype, permission and more"

	<WebMethod()>
	Public Function LoadEmploymentType(ByVal customerID As String, ByVal userName As String, ByVal language As String) As EmploymentTypeDataDTO()
		Dim result As List(Of EmploymentTypeDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadEmploymentTypeData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadEmploymentType", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod()>
	Public Function LoadOtherEmploymentType(ByVal customerID As String, ByVal userName As String, ByVal language As String) As EmploymentTypeDataDTO()
		Dim result As List(Of EmploymentTypeDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadOtherEmploymentTypeData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadOtherEmploymentType", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod()>
	Public Function LoadTypeOfStay(ByVal customerID As String, ByVal userName As String, ByVal language As String) As TypeOfStayDataDTO()
		Dim result As List(Of TypeOfStayDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadTypeOfStayData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTypeOfStay", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod()>
	Public Function LoadPermission(ByVal customerID As String, ByVal userName As String, ByVal language As String) As PermissionDataDTO()
		Dim result As List(Of PermissionDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadPermissionData(m_customerID, userName, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPermission", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function

	<WebMethod()>
	Public Function LoadForeignCategory(ByVal customerID As String, ByVal userName As String, ByVal code As String, ByVal language As String) As PermissionDataDTO()
		Dim result As List(Of PermissionDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadForeignCategoryData(m_customerID, userName, code, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPermission", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result.ToArray()
	End Function


#End Region


#Region "Ki-Au Zulagen"

	<WebMethod(Description:="Test ob der eingetragene QST-Code korrekt ist")>
	Public Function LoadChildEducationData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As ChildEducationDataDTO
		Dim result As ChildEducationDataDTO = Nothing
		m_customerID = customerID

		Try
			result = m_PublicData.LoadChildEducationData(m_customerID, canton, year)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadChildEducationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function


#End Region


#Region "Utility Methods"


#End Region

End Class
