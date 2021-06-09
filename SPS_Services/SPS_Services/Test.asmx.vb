
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.PVLInfo
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects


' Wenn der Aufruf dieses Webdiensts aus einem Skript mithilfe von ASP.NET AJAX zulässig sein soll, heben Sie die Kommentarmarkierung für die folgende Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/test.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class Test
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "test"

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

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


End Class

