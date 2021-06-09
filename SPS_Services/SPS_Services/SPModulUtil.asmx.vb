Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.Notify

Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.Logging


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPModulUtil.asmx")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPModulUtil
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPModulUtil"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Dim _UserInfo As String = String.Empty


	Public Sub New()

		m_utility = New ClsUtilities

	End Sub

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Berufdaten auf der Client")>
	Function GetJobData(ByVal strUserID As String,
										ByVal strLanguage As String,
										ByVal strSex As String) As DataSet
		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get JobData]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		m_customerID = strUserID

		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn
		Dim rFoundedrec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Language",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Geschlecht",
																																									 Global.System.Data.SqlDbType.NVarChar,
																																									 0,
																																									 Global.System.Data.ParameterDirection.Input,
																																									 0, 0, "Geschlecht",
																																									 Global.System.Data.DataRowVersion.Original,
																																									 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(strSex.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Job_Online")


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetJobData", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der HBB-Berufdaten auf der Client")>
	Function Get_HBBJob_Data(ByVal strUserID As String,
										ByVal strLanguage As String,
										ByVal strSex As String) As DataSet
		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get HBB_JobData]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		m_customerID = strUserID

		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn
		Dim rFoundedrec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Language",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Geschlecht",
																																									 Global.System.Data.SqlDbType.NVarChar,
																																									 0,
																																									 Global.System.Data.ParameterDirection.Input,
																																									 0, 0, "Geschlecht",
																																									 Global.System.Data.DataRowVersion.Original,
																																									 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(strSex.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "JobHBB_Online")


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "Get_HBBJob_Data", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der BGB-Berufdaten auf der Client")>
	Function Get_BGBJob_Data(ByVal strUserID As String,
										ByVal strLanguage As String,
										ByVal strSex As String) As DataSet
		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get BGB_JobData]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		m_customerID = strUserID

		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn
		Dim rFoundedrec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Language",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Geschlecht",
																																									 Global.System.Data.SqlDbType.NVarChar,
																																									 0,
																																									 Global.System.Data.ParameterDirection.Input,
																																									 0, 0, "Geschlecht",
																																									 Global.System.Data.DataRowVersion.Original,
																																									 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(strSex.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "JobBGB_Online")


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "Get_BGBJob_Data", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der BGB und HBB-Berufdaten auf der Client")>
	Function Get_BGBHBBJob_Data(ByVal strUserID As String,
										ByVal strLanguage As String,
										ByVal strSex As String) As DataSet
		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get BGB_HBB_JobData]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		m_customerID = strUserID

		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn
		Dim rFoundedrec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Language",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Geschlecht",
																																									 Global.System.Data.SqlDbType.NVarChar,
																																									 0,
																																									 Global.System.Data.ParameterDirection.Input,
																																									 0, 0, "Geschlecht",
																																									 Global.System.Data.DataRowVersion.Original,
																																									 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(strSex.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "JobBGB_HBB_Online")


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "Get_BGBHBBJob_Data", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Regionsdaten auf der Client")>
	Function GetRegionsData(ByVal strUserID As String,
													ByVal strLanguage As String,
													ByVal strRegion As String) As DataSet
		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get RegionsData]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
		m_customerID = strUserID

		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn
		Dim rFoundedrec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Language",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Regions",
																																									 Global.System.Data.SqlDbType.NVarChar,
																																									 0,
																																									 Global.System.Data.ParameterDirection.Input,
																																									 0, 0, "Regions",
																																									 Global.System.Data.DataRowVersion.Original,
																																									 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(strRegion.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Regions_Online")


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetRegionsData", .MessageContent = msgContent})

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function


#Region "Sonstige Funktionen..."


	'Function SaveErrToDb(ByVal strErrorMessage As String) As String
	'	Dim strResult As String = "Erfolgreich..."
	'	Dim connString As String = My.Settings.ConnStr_New_spContract
	'	Dim strSQL As String = String.Empty
	'	strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'	strSQL &= "CreatedOn) Values ("
	'	strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

	'	Dim Conn As SqlConnection = New SqlConnection(connString)
	'	Conn.Open()

	'	Try
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPModulUtil.asmx")
	'		param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
	'		param = cmd.Parameters.AddWithValue("@RequestParam", Me._UserInfo)
	'		param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

	'		cmd.ExecuteNonQuery()

	'	Catch ex As Exception
	'		'     strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)
	'	Finally

	'		If Not Conn Is Nothing Then
	'			Conn.Close()
	'			Conn.Dispose()
	'		End If

	'	End Try

	'	Return strResult
	'End Function

#End Region

End Class