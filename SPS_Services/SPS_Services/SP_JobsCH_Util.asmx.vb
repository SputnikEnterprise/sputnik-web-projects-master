Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.Logging
Imports wsSPS_Services.SPUtilities

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SP_JobsCH_Util.asmx")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SP_JobsCH_Util
	Inherits System.Web.Services.WebService

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

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Regionsdaten auf der Client")>
	Function GetRegionsData(ByVal strUserID As String, ByVal strLanguage As String) As DataSet

		Try
			If Not IsAllowedtoContinue(strUserID) Then
				m_Logger.LogWarning(String.Format("GetRegionsData: strUserID: {0} | language: {1}", strUserID, strLanguage))

				Return Nothing
			End If

		Catch ex As Exception
			m_Logger.LogError(String.Format("GetRegionsData: strUserID: {1} | language: {2}{0}{3}", vbNewLine, strUserID, strLanguage, ex.ToString()))

			Return Nothing

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get RegionsData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Regions_Online")


		Catch ex As Exception
			m_Logger.LogError(String.Format("GetRegionsData: strUserID: {1} | language: {2}{0}{3}", vbNewLine, strUserID, strLanguage, ex.ToString()))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Anstellungsarten auf der Client")>
	Function GetAnstellungData(ByVal strUserID As String,
					  ByVal strLanguage As String,
					  ByVal strBez As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				Throw New Exception("no right")

				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetAnstellungData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get AnstellungData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Bez",
																					   Global.System.Data.SqlDbType.NVarChar,
																					   0,
																					   Global.System.Data.ParameterDirection.Input,
																					   0, 0, "Bez",
																					   Global.System.Data.DataRowVersion.Original,
																					   False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(0).Value = CType(strBez.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Anstellung_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetAnstellungData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Berufe auf der Client")>
	Function GetBerufeData(ByVal strUserID As String,
						   ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBerufeData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get JobData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Berufe_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBerufeData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Fachbereiche eines Berufs auf der Client")>
	Function GetBerufFachbereichData(ByVal strUserID As String,
									 ByVal iID_Parent As Integer,
									 ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBerufFachbereichData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get JobFachbereichData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@iID_Parent",
																					   Global.System.Data.SqlDbType.Int,
																					   0,
																					   Global.System.Data.ParameterDirection.Input,
																					   0, 0, "iID_Parent",
																					   Global.System.Data.DataRowVersion.Original,
																					   False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(iID_Parent, Integer)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "BerufFachbereich_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBerufFachbereichData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Branchen auf der Client")>
	Function GetBranchenData(ByVal strUserID As String,
					  ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBranchenData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get BranchenData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Branchen_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBranchenData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Positionen auf der Client")>
	Function GetPositionData(ByVal strUserID As String,
					  ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetPositionData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get PositionsData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Position_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetPositionData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Alter-Liste auf der Client")>
	Function GetAgeData(ByVal strUserID As String,
					  ByVal strLanguage As String,
					  ByVal strBez As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetAgeData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get AgeData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Bez",
																					   Global.System.Data.SqlDbType.NVarChar,
																					   0,
																					   Global.System.Data.ParameterDirection.Input,
																					   0, 0, "Bez",
																					   Global.System.Data.DataRowVersion.Original,
																					   False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
		adapter.SelectCommand.Parameters(0).Value = CType(strBez.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Alter_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetAgeData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Bildung-Liste auf der Client")>
	Function GetBildungData(ByVal strUserID As String,
					  ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBildungData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get BildungData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Bildung_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBildungData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der Sprach-Liste auf der Client")>
	Function GetLanguageData(ByVal strUserID As String,
					  ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetLanguageData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get LanguageData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "Language_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetLanguageData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function

	<WebMethod(Description:="Datasetversion: Zur Auflistung der SprachNiveau-Liste auf der Client")>
	Function GetLanguageNiveauData(ByVal strUserID As String,
					  ByVal strLanguage As String) As DataSet
		Try
			If Not IsAllowedtoContinue(strUserID) Then
				SaveErrToDb(String.Format("{0}¦{1}",
									"Connection not allowed...",
									String.Empty))
				Return Nothing
			End If

		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetLanguageNiveauData", ex.Message))

		End Try

		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim strSQL As String = "[Get LanguageNiveauData Job.ch]"
		Dim Conn As SqlConnection = New SqlConnection(connString)
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
		adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)

		Try
			' Die Datenbank anders nennen!!!
			adapter.Fill(rFoundedrec, "LanguageNiveau_Online")


		Catch ex As Exception
			SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetLanguageNiveauData", ex.Message))

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rFoundedrec
	End Function


#Region "Sonstige Funktionen..."

	Function IsAllowedtoContinue(ByVal strUserInfo As String) As Boolean
		'    Dim _clsSystem As New ClsMain_Net
		'_UserInfo = strUserInfo
		'Dim aUserData As String() = strUserInfo.Split(CChar("¦"))

		'Dim strResult As String = _clsSystem.GetUserID(aUserData(0), "GAVSonstige")
		'Dim strResult As String = SaveUserToDb(strUserInfo)
		'_UserInfo &= String.Format("¦{0}", strResult)

		Return True ' Temporär Lösung bis wir alle Guids haben.
		'Return Not strResult.ToLower.Contains("fehler:")
	End Function

	'Function SaveUserToDb(ByVal strIDNr As String) As String
	'	Dim strResult As String = "Erfolgreich..."
	'	Dim connString As String = My.Settings.ConnStr_New_spContract
	'	Dim strSQL As String = String.Empty
	'	strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'	strSQL &= "IsWebService, CreatedOn) Values ("
	'	strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @IsWebService, @CreatedOn)"
	'	Dim aUserData As String() = strIDNr.Split(CChar("¦"))

	'	Dim Conn As SqlConnection = New SqlConnection(connString)
	'	Conn.Open()

	'	Try
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@Modulname", "SP_JobsCH_Util.asmx")
	'		param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@UserID", aUserData(0))
	'		param = cmd.Parameters.AddWithValue("@Answer", aUserData(1))
	'		param = cmd.Parameters.AddWithValue("@RequestParam", Me._UserInfo)
	'		param = cmd.Parameters.AddWithValue("@IsWebService", 1)
	'		param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

	'		cmd.ExecuteNonQuery()

	'	Catch ex As Exception
	'		strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)
	'		SaveErrToDb(strResult)

	'	Finally

	'		If Not Conn Is Nothing Then
	'			Conn.Close()
	'			Conn.Dispose()
	'		End If

	'	End Try

	'	Return strResult
	'End Function

	Function SaveErrToDb(ByVal strErrorMessage As String) As String

		m_Logger.LogError(String.Format("{0}", strErrorMessage))

		Return String.Empty
	End Function

#End Region

End Class