
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPEmployeeData.asmx")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPEmployeeData
	Inherits System.Web.Services.WebService

	<WebMethod(Description:="Zur Auflistung der Kandidaten-Berufe (Qualifikation)")>
	Function ListEmployeeProfessionTitles(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "MA_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[List MA-Berufe]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			Dim rMArec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rMArec.Read
				s.Add(rMArec("MA_Beruf").ToString)

				i += 1
			End While

		Catch ex As Exception

		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Kandidaten-Filiale (Filiale)")>
	Function ListEmployeeBrunchoffices(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "MA_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[List MA-Filiale]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			Dim rMArec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rMArec.Read
				s.Add(rMArec("MA_Filiale").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Kandidaten-Ort (Ort)")>
	Function ListEmployeeLocations(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "MA_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[List MA-Ort]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			Dim rMArec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rMArec.Read
				s.Add(rMArec("MA_Ort").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try
		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Kandidaten-Kanton (Kanton)")>
	Function ListEmployeeCantons(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "MA_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[List MA-Kanton]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			Dim rMArec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rMArec.Read
				s.Add(rMArec("MA_Kanton").ToString)

				i += 1
			End While

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return s
	End Function

	<WebMethod(Description:="Zur Auflistung der Kandidaten (Gesamte Datensätze)")>
	Function ListEmployees(ByVal strUserID As String, ByVal profession As String,
											ByVal location As String, ByVal canton As String,
											ByVal brunchoffice As String,
											ByVal sortKey As String) As DataSet
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[Get MArec]"
		Dim strRecResult As String = String.Empty

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim adapter As New SqlDataAdapter()
		adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
		adapter.SelectCommand.Connection = Conn

		Dim rMArec As New DataSet

		' ---------------------------------------------------------------------------------------
		adapter.SelectCommand.CommandText = strSQL
		adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@UserID",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "UserID",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Beruf",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Beruf",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Ort",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Ort",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Kanton",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Kanton",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Filiale",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "Filiale",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@SortString",
																																											 Global.System.Data.SqlDbType.NVarChar,
																																											 0,
																																											 Global.System.Data.ParameterDirection.Input,
																																											 0, 0, "SortString",
																																											 Global.System.Data.DataRowVersion.Original,
																																											 False, Nothing, "", "", ""))
		adapter.SelectCommand.Parameters(0).Value = CType(strUserID.Trim, String)
		adapter.SelectCommand.Parameters(1).Value = CType(profession.Trim, String)
		adapter.SelectCommand.Parameters(2).Value = CType(location.Trim, String)
		adapter.SelectCommand.Parameters(3).Value = CType(canton.Trim, String)
		adapter.SelectCommand.Parameters(4).Value = CType(brunchoffice.Trim, String)
		adapter.SelectCommand.Parameters(5).Value = CType(If(sortKey.Trim = String.Empty,
																												 "MA.Transfered_On DESC", sortKey.Trim), String)
		Try
			' Die Datenbank anders nennen!!!
			' Return rMArec
			adapter.Fill(rMArec, "Kandidaten_Online")

			strRecResult = String.Format(If(rMArec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.",
																			"Keine Daten wurden gefunden."), rMArec.Tables(0).Rows.Count)


		Catch ex As Exception
			strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

		Finally
			WriteConnectionHistory(strUserID, profession, location, canton, brunchoffice, sortKey, strRecResult)

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return rMArec
	End Function

	<WebMethod(Description:="Zur Auflistung der Kandidaten-Datensätze als Array (Alle gesuchten Datensätze)")>
	Function ListEmployee_Array(ByVal strUserID As String, ByVal profession As String,
											ByVal location As String, ByVal canton As String,
											ByVal brunchoffice As String,
											ByVal sortKey As String) As List(Of String)
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_MA
		Dim strSQL As String = "[Get MArec]"
		Dim strRecResult As String = String.Empty

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim liMArec As New List(Of String)

		' ---------------------------------------------------------------------------------------
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter

		param = cmd.Parameters.AddWithValue("@UserID", strUserID.Trim)
		param = cmd.Parameters.AddWithValue("@Beruf", profession.Trim)
		param = cmd.Parameters.AddWithValue("@Ort", location.Trim)
		param = cmd.Parameters.AddWithValue("@Kanton", canton.Trim)
		param = cmd.Parameters.AddWithValue("@Filiale", brunchoffice.Trim)
		param = cmd.Parameters.AddWithValue("@SortString", If(sortKey.Trim = String.Empty,
																													"MA.Transfered_On DESC", sortKey.Trim))

		Dim rMArec As SqlDataReader = cmd.ExecuteReader

		While rMArec.Read
			For i As Integer = 0 To rMArec.FieldCount - 1
				liMArec.Add(String.Format("{0}={1}", rMArec.GetName(i).ToString, rMArec(i).ToString))
			Next i
		End While

		Try
			strRecResult = String.Format(If(liMArec.Count > 0, "{0} Datensätze wurden gefunden.",
																			"Keine Daten wurden gefunden."), liMArec.Count)


		Catch ex As Exception
			strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

		Finally
			WriteConnectionHistory(strUserID, profession, location, canton, brunchoffice, sortKey, strRecResult)

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

		Return liMArec
	End Function

	Sub WriteConnectionHistory(ByVal strUserID As String, ByVal strBeruf As String,
											ByVal strOrt As String, ByVal strKanton As String,
											ByVal strFiliale As String,
											ByVal strSortkeys As String, ByVal strRecResult As String)
		Dim _clsSystem As New ClsMain_Net
		Dim connString As String = My.Settings.ConnStr_MA
		Dim strQuery As String = "Insert Into Tab_ModulUsage (ModulName, UseCount, UseDate, MachineID, "
		strQuery &= "ModulParameter, IsWebService) Values ("
		strQuery &= "@ModulName, @UseCount, @UseDate, @MachineID, @ModulParameter, @IsWebService)"


		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try
			Dim cmd As System.Data.SqlClient.SqlCommand
			cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
			cmd.CommandType = CommandType.Text
			Dim param As System.Data.SqlClient.SqlParameter

			param = cmd.Parameters.AddWithValue("@ModulName", "SPMAData.asmx")
			param = cmd.Parameters.AddWithValue("@UseCount", 1)
			param = cmd.Parameters.AddWithValue("@UseDate", Now.ToString)
			param = cmd.Parameters.AddWithValue("@MachineID", String.Empty)
			param = cmd.Parameters.AddWithValue("@ModulParameter", strUserID & vbNewLine &
																					"Beruf: " & strBeruf & vbNewLine &
																					"Ort: " & strOrt & vbNewLine &
																					"Kanton: " & strKanton & vbNewLine &
																					"Filiale: " & strFiliale & vbNewLine &
																					"Sortkey: " & strSortkeys & vbNewLine & vbNewLine &
																					strRecResult)
			param = cmd.Parameters.AddWithValue("@IsWebService", True)


			Try
				cmd.ExecuteNonQuery()


			Catch ex As Exception


			End Try

		Catch ex As Exception
		Finally

			If Not Conn Is Nothing Then
				Conn.Close()
				Conn.Dispose()
			End If

		End Try

	End Sub

End Class