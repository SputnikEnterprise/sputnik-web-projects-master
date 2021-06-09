
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.PVLInfo
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects
Imports wsSPS_Services.Logging



' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPPVLGAVUtil.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPPVLGAVUtil
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPPVLGAVUtil"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_SysInfo As SystemInfoDatabaseAccess
	Private m_PVLInfo As PVLDatabaseAccess
	Private m_CurrentPVLInfo As CurrentPVLDatabaseAccess


	Public Sub New()

		m_utility = New ClsUtilities
		m_SysInfo = New SystemInfoDatabaseAccess(My.Settings.Connstr_spSystemInfo_2016, Language.German)
		m_PVLInfo = New PVLDatabaseAccess(My.Settings.ConnStr_PVLPublicInfo, Language.German)
		m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(My.Settings.ConnStr_SputnikCurrentPVLDb, Language.German)

	End Sub


	<WebMethod(Description:="Zur Auflistung der aktuellen PVL GAV-Daten auf der Client")>
	Function GetPVLArchiveDbData(ByVal customerID As String) As GAVPVLArchiveDatabaseDTO()

		Dim result As List(Of GAVPVLArchiveDatabaseDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of GAVPVLArchiveDatabaseDTO)
			result = m_PVLInfo.LoadArchivePVLDatabases(customerID)

		Catch ex As Exception
			Dim msgContent = String.Format("{0}", ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLArchiveDbData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	''' <summary>
	''' Gets the parifond hour data.
	''' </summary>
	<WebMethod(Description:="loads parifond hour data")>
	Function LoadCurrentMetaInfo(ByVal customerID As String, ByVal canton As String, ByVal contractNumber As Integer) As GAVNameResultDTO
		Dim result As GAVNameResultDTO = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		Try
			result = New GAVNameResultDTO
			result = m_PVLInfo.LoadCurrentMetaInfo(customerID, canton, contractNumber)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, customerID, contractNumber, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCurrentMetaInfo", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return result
	End Function

	''' <summary>
	''' Gets the PVL data.
	''' </summary>
	''' <returns>Array containing search results.</returns>
	<WebMethod(Description:="Zur Auflistung der aktuellen PVL GAV-Daten auf der Client")>
	Function GetCurrentPVLData(ByVal customerID As String, ByVal archiveDbName As String, ByVal canton As String, ByVal postcode As String, ByVal language As String) As GAVNameResultDTO()

		Dim result As List(Of GAVNameResultDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVNameResultDTO)
			result = m_CurrentPVLInfo.LoadCurrentPVLInfo(customerID, canton, postcode, language)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCurrentPVLData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Zur Auflistung der aktuelle PVL GAV-Daten auf der Client")> <Obsolete("is obsolated! use GetCurrentPVLData instead.")>
	Function GetPVLDataforMandant(ByVal archiveDbName As String) As GAVNameResultDTO()

		Dim result As List(Of GAVNameResultDTO) = Nothing
		Dim dbConnection As String = String.Empty
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVNameResultDTO)
			result = m_CurrentPVLInfo.LoadCurrentPVLInfo(String.Empty, String.Empty, String.Empty, String.Empty)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLDataforMandant", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Zur Auflistung der GAV-Details von Criterion-Db anhand ID_Meta")>
	Function GetPVLCriteriasData(ByVal customerID As String, ByVal archiveDbName As String, ByVal metaID As Integer, ByVal strLanguage As String) As GAVCriteriasResultDTO()
		Dim result As List(Of GAVCriteriasResultDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVCriteriasResultDTO)
			result = m_CurrentPVLInfo.LoadPVLCriteriasInfo(customerID, metaID, strLanguage)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLCriteriasData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Zur Auflistung der GAV-Details von CriterionValue-Db anhand ID_Criterion")>
	Function GetPVLCriteriaValuesByIDData(ByVal customerID As String, ByVal archiveDbName As String, ByVal criteriaID As Integer, ByVal strLanguage As String) As GAVCriteriaValueResultDTO()
		Dim result As List(Of GAVCriteriaValueResultDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVCriteriaValueResultDTO)
			result = m_CurrentPVLInfo.LoadPVLCriteriaValueInfo(customerID, criteriaID, strLanguage)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLCriteriaValuesByIDData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


	<WebMethod(Description:="Zur Auflistung der GAV-Kantone von TaxonomyEntry-Db anhand ID_Meta")>
	Function GetGAVTaxonomyEntryValueData(ByVal customerID As String, ByVal archiveDbName As String, ByVal metaID As Integer) As GAVTaxonomyDTO()
		Dim result As List(Of GAVTaxonomyDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVTaxonomyDTO)
			result = m_CurrentPVLInfo.LoadPVLTaxonomyInfo(customerID, metaID)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVTaxonomyEntryValueData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Zur Auflistung der Wert von Calculation-Db anhand Kategorievalues")>
	Function GetGAVCalculationValueData(ByVal customerID As String, ByVal archiveDbName As String, ByVal categoryValues As String) As GAVCalculationDTO
		Dim result As GAVCalculationDTO = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New GAVCalculationDTO
			result = m_CurrentPVLInfo.LoadPVLCalculationData(customerID, categoryValues)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVCalculationValueData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (result)
	End Function

	<WebMethod(Description:="Zur Auflistung der Werte aller Kategorievalues einer BaseCategoryValue")>
	Function GetGAVCategoryValuesWithBaseValueData(ByVal customerID As String, ByVal archiveDbName As String, ByVal categoryID As Integer, ByVal baseCategoryValueID As Integer?, ByVal strLanguage As String) As GAVCategoryValueDTO()
		Dim result As List(Of GAVCategoryValueDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVCategoryValueDTO)
			result = m_CurrentPVLInfo.LoadPVLCategoryValueWithBaseValueInfo(customerID, categoryID, baseCategoryValueID, strLanguage)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVCategoryValuesWithBaseValueData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Name aller Kategorien einer MetaNr")>
	Function GetGAVCategoryLabelData(ByVal customerID As String, ByVal archiveDbName As String, ByVal metaID As Integer, ByVal strLanguage As String) As GAVCategoryDTO()
		Dim result As List(Of GAVCategoryDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVCategoryDTO)
			result = m_CurrentPVLInfo.LoadPVLCategoryInfo(customerID, metaID, strLanguage)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVCategoryLabelData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Gibt die kritischen Infos über eine GAV-Number zurück. Das pflegen wir selbst ein!!!")>
	Function GetPVLWarningData(ByVal customerID As String, ByVal archiveDbName As String, ByVal iGAVNr As Integer) As GAVNotificationDTO
		Dim result As GAVNotificationDTO = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New GAVNotificationDTO
			result = m_CurrentPVLInfo.LoadPVLWarningData(customerID, iGAVNr)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLWarningData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (result)
	End Function

	<WebMethod(Description:="Zur Auflistung der GAV-Berufe im Anhang1")>
	Function GetPVLAnhang1Data(ByVal customerID As String, ByVal archiveDbName As String) As GAVNotificationDTO()
		Dim result As List(Of GAVNotificationDTO) = Nothing
		Dim dbConnection As String = String.Empty

		m_customerID = customerID
		If String.IsNullOrWhiteSpace(archiveDbName) Then
			dbConnection = My.Settings.ConnStr_SputnikCurrentPVLDb
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		Else
			dbConnection = String.Format(My.Settings.ConnStr_CurrentPVLArchiveDbName, archiveDbName) '"Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog={0};Data Source=192.168.19.55;Current Language=German"
			If Not String.IsNullOrWhiteSpace(archiveDbName) Then m_CurrentPVLInfo = New CurrentPVLDatabaseAccess(dbConnection, DatabaseAccessBase.Language.German)

		End If

		Try
			result = New List(Of GAVNotificationDTO)
			result = m_CurrentPVLInfo.LoadPVLAnhang1Data(customerID)

		Catch ex As Exception
			Dim msgContent = String.Format("{1}{0}{2}{0}{3}", vbNewLine, archiveDbName, dbConnection, ex.ToString)
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLAnhang1Data", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function



#Region "FL GAV"


	<WebMethod(Description:="Datasetversion: FL-GAV Gruppe0")>
	Function GetFLGAVGruppe0Data(ByVal customerID As String, ByVal strLanguage As String) As FLGAVGruppe0ResultDTO()
		Dim result As List(Of FLGAVGruppe0ResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of FLGAVGruppe0ResultDTO)
			result = m_PVLInfo.LoadFLGAVGruppe0Info(customerID, "FL", strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVGruppe0Data", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: FL-GAV Gruppe1")>
	Function GetFLGAVGruppe1Data(ByVal customerID As String, ByVal gruppe0 As String, ByVal strLanguage As String) As FLGAVGruppe1ResultDTO()
		Dim result As List(Of FLGAVGruppe1ResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of FLGAVGruppe1ResultDTO)
			result = m_PVLInfo.LoadFLGAVGruppe1Info(customerID, "FL", gruppe0, strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVGruppe1Data", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: FL-GAV Gruppe2")>
	Function GetFLGAVgruppe2Data(ByVal customerID As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal strLanguage As String) As FLGAVGruppe2ResultDTO()
		Dim result As List(Of FLGAVGruppe2ResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of FLGAVGruppe2ResultDTO)
			result = m_PVLInfo.LoadFLGAVGruppe2Info(customerID, "FL", gruppe0, gruppe1, strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVgruppe2Data", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: FL-GAV gruppe3")>
	Function GetFLGAVgruppe3Data(ByVal customerID As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal strLanguage As String) As FLGAVGruppe3ResultDTO()
		Dim result As List(Of FLGAVGruppe3ResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of FLGAVGruppe3ResultDTO)
			result = m_PVLInfo.LoadFLGAVGruppe3Info(customerID, "FL", gruppe0, gruppe1, gruppe2, strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVgruppe3Data", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: FL-GAV Text Data")>
	Function GetFLGAVTextData(ByVal customerID As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal strLanguage As String) As FLGAVTextResultDTO()
		Dim result As List(Of FLGAVTextResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of FLGAVTextResultDTO)
			result = m_PVLInfo.LoadFLGAVTextInfo(customerID, "FL", gruppe0, gruppe1, gruppe2, gruppe3, strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVTextData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="Datasetversion: FL-GAV salary Data")>
	Function GetFLGAVSalaryData(ByVal customerID As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal gavtext As String, ByVal strLanguage As String) As FLGAVSalaryResultDTO
		Dim result As FLGAVSalaryResultDTO = Nothing
		m_customerID = customerID

		Try
			result = New FLGAVSalaryResultDTO
			result = m_PVLInfo.LoadFLGAVSalaryInfo(customerID, "FL", gruppe0, gruppe1, gruppe2, gruppe3, gavtext, strLanguage)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetFLGAVSalaryData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data.
		Return (result)
	End Function


#End Region


	<WebMethod(Description:="Datasetversion: FL-GAV salary Data")>
	Function GetPVLAddressData(ByVal customerID As String, ByVal gavnumber As Integer, ByVal canton As String, ByVal gruppe0 As String, ByVal organ As String) As GAVAddressDataDTO
		Dim result As GAVAddressDataDTO = Nothing
		m_customerID = customerID

		Try
			result = New GAVAddressDataDTO
			result = m_PVLInfo.LoadPVLAddressData(customerID, gavnumber, canton, gruppe0, organ)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("{0} >>> {1}", msgContent, ex.ToString))
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetPVLAddressData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data.
		Return (result)
	End Function

	<WebMethod(Description:="List of viewed publications")>
	Function LoadPublicationInfoData(ByVal customerID As String, ByVal userID As String) As PVLPublicationViewDataDTO()
		Dim result As List(Of PVLPublicationViewDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of PVLPublicationViewDataDTO)
			result = m_PVLInfo.LoadAssignedAdvisorPublicationInfoData(customerID, userID)

			m_Logger.LogInfo(String.Format("LoadPublicationInfoData:{0}customerID: {1} >>> userID: {2} | Recordcount: {3}", vbNewLine, m_customerID, userID, result.Count))

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("LoadPublicationInfoData:{0}customerID: {1} >>> userID: {2}{3}", vbNewLine, m_customerID, userID, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPublicationInfoData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="update publication viewed record as done")>
	Function UpdateViewedPublicationData(ByVal customerID As String, ByVal userID As String, ByVal recID As Integer, ByVal ContractNumber As String, ByVal VersionNumber As Integer,
										 ByVal PublicationDate As DateTime?, ByVal Title As String, ByVal checked As Boolean?,
										 ByVal userData As String) As Boolean
		Dim result As Boolean = True
		m_customerID = customerID

		Try
			result = result AndAlso m_PVLInfo.UpdateAssignedAdvisorPublicationViewedData(customerID, userID, recID, ContractNumber, VersionNumber, PublicationDate, Title, checked, userData)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_Logger.LogError(String.Format("UpdateViewedPublicationData:{0}customerID: {1} >>> userID: {2}{0}userData: {3}{0}infoID: {4}{0}{5}", vbNewLine, m_customerID, userID, recID, userData, ex.ToString))

			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateViewedPublicationData", .MessageContent = msgContent})
			result = Nothing
		Finally
		End Try


		Return result
	End Function

End Class