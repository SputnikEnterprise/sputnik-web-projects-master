
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports SPWebService.SPUtilities
Imports System.Globalization



' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET-AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPGAVData.asmx")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class ClsGetGAVData
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "ClsGetGAVData"


	<WebMethod(Description:="Zur Auflistung der GAV-Berufe eines Kantons")>
	Function GetGruppe0ByKanton(ByVal strUserID As String,
															ByVal strGAVKanton As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVGruppe0]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Gruppe0
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("Gruppe0").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-Berufe in allen Kantonen")>
	Function GetGruppe0inAllCanton(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVGruppe0 In Kanton]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Gruppe0
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("Gruppe0").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-Gruppe1")>
	Function GetGruppe1ByKanton(ByVal strUserID As String,
															ByVal strGAVKanton As String,
															ByVal strGAVGruppe0 As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVGruppe1]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
		param = cmd.Parameters.AddWithValue("@Gruppe0", strGAVGruppe0)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Gruppe1
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("Gruppe1").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-Gruppe2")>
	Function GetGruppe2ByKanton(ByVal strUserID As String,
														ByVal strGAVKanton As String,
														ByVal strGAVGruppe0 As String,
														ByVal strGAVGruppe1 As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVGruppe2]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
		param = cmd.Parameters.AddWithValue("@Gruppe0", strGAVGruppe0)
		param = cmd.Parameters.AddWithValue("@Gruppe1", strGAVGruppe1)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAVDatenbank
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("Gruppe2").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-Gruppe3")>
	Function GetGruppe3ByKanton(ByVal strUserID As String,
														ByVal strGAVKanton As String,
														ByVal strGAVGruppe0 As String,
														ByVal strGAVGruppe1 As String,
														ByVal strGAVGruppe2 As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVGruppe3]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
		param = cmd.Parameters.AddWithValue("@Gruppe0", strGAVGruppe0)
		param = cmd.Parameters.AddWithValue("@Gruppe1", strGAVGruppe1)
		param = cmd.Parameters.AddWithValue("@Gruppe2", strGAVGruppe2)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAVDatenbank
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("Gruppe3").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-Adressen")>
	Function GetGAVAdressen(ByVal strUserID As String,
													ByVal strGAVKanton As String,
													ByVal strGAVBeruf As String,
													ByVal strGAVOrgan As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVAdressen]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
		param = cmd.Parameters.AddWithValue("@BerufBez", strGAVBeruf)
		param = cmd.Parameters.AddWithValue("@Organ", strGAVOrgan)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Adress Datenbank
			Dim i As Integer

			While rGAVrec.Read
				s.Add(GetDbValue(CStr(rGAVrec("BerufBez").ToString)))          ' 0

				s.Add(GetDbValue(CStr(rGAVrec("GAV_Name").ToString)))          ' 1
				s.Add(GetDbValue(CStr(rGAVrec("GAV_ZHD").ToString)))          ' 2
				s.Add(GetDbValue(CStr(rGAVrec("GAV_Postfach").ToString)))          ' 3
				s.Add(GetDbValue(CStr(rGAVrec("GAV_Strasse").ToString)))          ' 4
				s.Add(GetDbValue(CStr(rGAVrec("GAV_PLZ").ToString)))          ' 5
				s.Add(GetDbValue(CStr(rGAVrec("GAV_Ort").ToString)))          ' 6
				s.Add(GetDbValue(CStr(rGAVrec("GAV_AdressNr").ToString)))          ' 7

				s.Add(GetDbValue(CStr(rGAVrec("GAV_Bank").ToString)))          ' 8
				s.Add(GetDbValue(CStr(rGAVrec("GAV_BankPLZOrt").ToString)))          ' 9
				s.Add(GetDbValue(CStr(rGAVrec("GAV_Bankkonto").ToString)))          ' 10
				s.Add(GetDbValue(CStr(rGAVrec("GAV_IBAN").ToString)))          ' 11
				s.Add(GetDbValue(CStr(rGAVrec("Kanton").ToString)))          ' 12
				s.Add(GetDbValue(CStr(rGAVrec("Organ").ToString)))          ' 13

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

	<WebMethod(Description:="Zur Auflistung der GAV-Text")>
	Function GetGAVText(ByVal strUserID As String,
												ByVal strGAVKanton As String,
												ByVal strGAVGruppe0 As String,
												ByVal strGAVGruppe1 As String,
												ByVal strGAVGruppe2 As String,
												ByVal strGAVGruppe3 As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return s

		Dim connString As String = My.Settings.ConnStr_GAV
		Dim strSQL As String = "[Get GAVText]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		cmd.CommandType = Data.CommandType.StoredProcedure
		Dim param As System.Data.SqlClient.SqlParameter
		param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
		param = cmd.Parameters.AddWithValue("@Gruppe0", strGAVGruppe0)
		param = cmd.Parameters.AddWithValue("@Gruppe1", strGAVGruppe1)
		param = cmd.Parameters.AddWithValue("@Gruppe2", strGAVGruppe2)
		param = cmd.Parameters.AddWithValue("@Gruppe3", strGAVGruppe3)

		Try
			Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAVDatenbank
			Dim i As Integer

			While rGAVrec.Read
				s.Add(rGAVrec("GAVText").ToString)

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

	<WebMethod(Description:="Zur Auflistung der GAV-ID")>
	Function GetGAVRecByID(ByVal strUserID As String,
												ByVal strGAVKanton As String,
												ByVal strGAVGruppe0 As String,
												ByVal strGAVGruppe1 As String,
												ByVal strGAVGruppe2 As String,
												ByVal strGAVGruppe3 As String,
												ByVal strGAVText As String) As List(Of String)
		Dim s As New List(Of String)
		Dim customerID As String = strUserID.Split("¦")(0)

		Dim connString As String = My.Settings.ConnStr_GAV


		' -------------------------------------------------------------------------
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try

			Dim strOldLangInfo As String = System.Globalization.CultureInfo.CurrentUICulture.Name

			Dim culInfo As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("de-CH")
			Threading.Thread.CurrentThread.CurrentUICulture = New CultureInfo("de-CH")
			Threading.Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("de-CH")

			Dim strNewLangInfo As String = System.Globalization.CultureInfo.CurrentUICulture.Name
			culInfo.NumberFormat.CurrencyDecimalSeparator = "."
			culInfo.NumberFormat.CurrencyGroupSeparator = ""
			strNewLangInfo = String.Format("{0} | NumberFormat: {1}", strNewLangInfo, culInfo.NumberFormat.CurrencyDecimalSeparator)

			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get GAVRec]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Kanton", strGAVKanton))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", strGAVGruppe0))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe1", strGAVGruppe1))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe2", strGAVGruppe2))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe3", strGAVGruppe3))
			listOfParams.Add(New SqlClient.SqlParameter("GAVText", strGAVText))

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			' Open connection to database.
			conn.Open()

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																					cmd.Parameters(i).ParameterName,
																					cmd.Parameters(i).DbType,
																					cmd.Parameters(i).Size,
																					cmd.Parameters(i).Value,
																					ControlChars.NewLine))
			Next

			reader = cmd.ExecuteReader

			' Read all data.
			While (reader.Read())

				' 0 - 5
				s.Add(utility.SafeGetString(reader, "GavKanton"))
				s.Add(utility.SafeGetString(reader, "Gruppe0"))
				s.Add(utility.SafeGetString(reader, "Gruppe1"))
				s.Add(utility.SafeGetString(reader, "Gruppe2"))
				s.Add(utility.SafeGetString(reader, "Gruppe3"))
				s.Add(utility.SafeGetString(reader, "GavText"))
				' 6 - 7
				s.Add(utility.SafeGetInteger(reader, "CalcFerien", 0))
				s.Add(utility.SafeGetInteger(reader, "Calc13Lohn", 0))
				' 8 - 17
				s.Add(utility.SafeGetDecimal(reader, "Minlohn", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FeiertagLohn", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Feierbtr", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FerienLohn", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Ferienbtr", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Lohn13", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Lohn13btr", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "StdLohn", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Monatslohn", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "Mittagszulagen", 0).ToString.Replace(",", "."))
				' 18 - 23
				s.Add(utility.SafeGetDecimal(reader, "FAG", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FAN", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAG", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAN", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAG", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAN", 0).ToString.Replace(",", "."))
				' 24 - 29
				s.Add(utility.SafeGetDecimal(reader, "FAG_S", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FAN_S", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAG_S", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAN_S", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAG_S", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAN_S", 0).ToString.Replace(",", "."))
				' 30 - 35
				s.Add(utility.SafeGetDecimal(reader, "FAG_M", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FAN_M", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAG_M", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAN_M", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAG_M", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAN_M", 0).ToString.Replace(",", "."))
				' 36 - 41
				s.Add(utility.SafeGetDecimal(reader, "FAG_J", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "FAN_J", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAG_J", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "WAN_J", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAG_J", 0).ToString.Replace(",", "."))
				s.Add(utility.SafeGetDecimal(reader, "VAN_J", 0).ToString.Replace(",", "."))


				' 42 -47
				If utility.SafeGetDateTime(reader, "GueltigAb", Nothing) Is Nothing Then s.Add(CStr(-9)) Else s.Add(utility.SafeGetDateTime(reader, "GueltigAb", Nothing))
				If utility.SafeGetDateTime(reader, "GueltigBis", Nothing) Is Nothing Then s.Add(CStr(-9)) Else s.Add(utility.SafeGetDateTime(reader, "GueltigBis", Nothing))
				s.Add(utility.SafeGetString(reader, "ZusatzFeier"))
				s.Add(utility.SafeGetString(reader, "Zusatz13Lohn"))

				'      If IsDBNull(rGAVrec("[FAR-Pflicht]")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("[FAR-Pflicht]").ToString)
				s.Add(utility.SafeGetString(reader, "Ferientext"))
				s.Add(utility.SafeGetString(reader, "Lohn13text"))

				' 48 - 50
				s.Add(utility.SafeGetDecimal(reader, "StdWeek", 0))
				s.Add(utility.SafeGetDecimal(reader, "StdMonth", 0))
				s.Add(utility.SafeGetDecimal(reader, "StdYear", 0))

				' 51 - 52
				s.Add(utility.SafeGetString(reader, "F_Alter"))
				s.Add(utility.SafeGetString(reader, "L_Alter"))

				' 53 - 64
				s.Add(utility.SafeGetString(reader, "Zusatz1"))
				s.Add(utility.SafeGetString(reader, "Zusatz2"))
				s.Add(utility.SafeGetString(reader, "Zusatz3"))
				s.Add(utility.SafeGetString(reader, "Zusatz4"))
				s.Add(utility.SafeGetString(reader, "Zusatz5"))
				s.Add(utility.SafeGetString(reader, "Zusatz6"))
				s.Add(utility.SafeGetString(reader, "Zusatz7"))
				s.Add(utility.SafeGetString(reader, "Zusatz8"))
				s.Add(utility.SafeGetString(reader, "Zusatz9"))
				s.Add(utility.SafeGetString(reader, "Zusatz10"))
				s.Add(utility.SafeGetString(reader, "Zusatz11"))
				s.Add(utility.SafeGetString(reader, "Zusatz12"))

				' 65 - 66
				s.Add(utility.SafeGetInteger(reader, "ID", 0))
				s.Add(utility.SafeGetInteger(reader, "GAVNr", 0))


			End While

			AddModulNotifying(customerID, "GetGAVRecByID", String.Format("{0} - {1}", strOldLangInfo, strNewLangInfo))

			'Try






			'	strSQL = "[Update ModulUsage]"

			'	conn = New SqlConnection(connString)
			'	conn.Open()

			'	cmd = New System.Data.SqlClient.SqlCommand(strSQL, conn)
			'	cmd.CommandType = Data.CommandType.StoredProcedure
			'	param = cmd.Parameters.AddWithValue("@ModulName", "Tarifrechner")
			'	param = cmd.Parameters.AddWithValue("@MachineID", GetMachineIPAddress)
			'	param = cmd.Parameters.AddWithValue("@IsWebService", 1)

			'	cmd.ExecuteNonQuery()     ' Aufzählen...


			'Catch ex As Exception

			'End Try

		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetGAVRecByID", .MessageContent = msgContent})

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

		Return s
	End Function

	Private Sub AddModulNotifying(ByVal customerID As String, ByVal modulName As String, ByVal culName As String)
		Dim connString As String = My.Settings.ConnStr_GAV
		Dim conn As SqlConnection = Nothing
		Dim strMessage As New StringBuilder()
		Dim utility As New ClsUtilities
		Dim reader As SqlDataReader = Nothing

		Try
			' Create command.
			conn = New SqlConnection(connString)

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Update ModulUsage]", conn)
			cmd.CommandType = CommandType.StoredProcedure

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ModulName", modulName))
			listOfParams.Add(New SqlClient.SqlParameter("MachineID", culName))
			listOfParams.Add(New SqlClient.SqlParameter("IsWebService", 1))

			' Open connection to database.
			conn.Open()

			' Execute the data reader.
			cmd.Parameters.AddRange(listOfParams.ToArray())

			For i As Integer = 0 To cmd.Parameters.Count - 1
				strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																				cmd.Parameters(i).ParameterName,
																				cmd.Parameters(i).DbType,
																				cmd.Parameters(i).Size,
																				cmd.Parameters(i).Value,
																				ControlChars.NewLine))
			Next

			cmd.ExecuteNonQuery()


		Catch ex As Exception
			Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
			utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddModulNotifying", .MessageContent = msgContent})

		Finally

			If Not conn Is Nothing Then
				conn.Close()
				conn.Dispose()
			End If

		End Try

	End Sub





	' -------------------------------------------------------------------------




	'Dim Conn As SqlConnection = New SqlConnection(connString)
	'Conn.Open()

	'Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'  cmd.CommandType = Data.CommandType.StoredProcedure
	'  Dim param As System.Data.SqlClient.SqlParameter
	'  param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
	'  param = cmd.Parameters.AddWithValue("@Gruppe0", strGAVGruppe0)
	'  param = cmd.Parameters.AddWithValue("@Gruppe1", strGAVGruppe1)
	'  param = cmd.Parameters.AddWithValue("@Gruppe2", strGAVGruppe2)
	'  param = cmd.Parameters.AddWithValue("@Gruppe3", strGAVGruppe3)
	'  param = cmd.Parameters.AddWithValue("@GAVText", strGAVText)

	'Try
	'    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAVDatenbank
	'    Dim i As Integer

	'    While rGAVrec.Read
	'      ' 0 - 5
	'      s.Add(rGAVrec("GavKanton").ToString)
	'      s.Add(rGAVrec("Gruppe0").ToString)
	'      s.Add(rGAVrec("Gruppe1").ToString)
	'      s.Add(rGAVrec("Gruppe2").ToString)
	'      s.Add(rGAVrec("Gruppe3").ToString)
	'      s.Add(rGAVrec("GavText").ToString)
	'      ' 6 - 7
	'      s.Add(rGAVrec("CalcFerien").ToString)
	'      s.Add(rGAVrec("Calc13Lohn").ToString)
	'      ' 8 - 17
	'      s.Add(rGAVrec("Minlohn").ToString)
	'      s.Add(rGAVrec("FeiertagLohn").ToString)
	'      s.Add(rGAVrec("Feierbtr").ToString)
	'      s.Add(rGAVrec("FerienLohn").ToString)
	'      s.Add(rGAVrec("Ferienbtr").ToString)
	'      s.Add(rGAVrec("Lohn13").ToString)
	'      s.Add(rGAVrec("Lohn13btr").ToString)
	'      s.Add(rGAVrec("StdLohn").ToString)
	'      s.Add(rGAVrec("Monatslohn").ToString)
	'      s.Add(rGAVrec("Mittagszulagen").ToString)
	'      ' 18 - 23
	'      s.Add(rGAVrec("FAG").ToString)
	'      s.Add(rGAVrec("FAN").ToString)
	'      s.Add(rGAVrec("WAG").ToString)
	'      s.Add(rGAVrec("WAN").ToString)
	'      s.Add(rGAVrec("VAG").ToString)
	'      s.Add(rGAVrec("VAN").ToString)
	'      ' 24 - 29
	'      s.Add(rGAVrec("FAG_S").ToString)
	'      s.Add(rGAVrec("FAN_S").ToString)
	'      s.Add(rGAVrec("WAG_S").ToString)
	'      s.Add(rGAVrec("WAN_S").ToString)
	'      s.Add(rGAVrec("VAG_S").ToString)
	'      s.Add(rGAVrec("VAN_S").ToString)
	'      ' 30 - 35
	'      s.Add(rGAVrec("FAG_M").ToString)
	'      s.Add(rGAVrec("FAN_M").ToString)
	'      s.Add(rGAVrec("WAG_M").ToString)
	'      s.Add(rGAVrec("WAN_M").ToString)
	'      s.Add(rGAVrec("VAG_M").ToString)
	'      s.Add(rGAVrec("VAN_M").ToString)
	'      ' 36 - 41
	'      s.Add(rGAVrec("FAG_J").ToString)
	'      s.Add(rGAVrec("FAN_J").ToString)
	'      s.Add(rGAVrec("WAG_J").ToString)
	'      s.Add(rGAVrec("WAN_J").ToString)
	'      s.Add(rGAVrec("VAG_J").ToString)
	'      s.Add(rGAVrec("VAN_J").ToString)
	'      ' 42 -47
	'      If IsDBNull(rGAVrec("GueltigAb")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("GueltigAb").ToString)
	'      If IsDBNull(rGAVrec("GueltigBis")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("GueltigBis").ToString)
	'      If IsDBNull(rGAVrec("ZusatzFeier")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("ZusatzFeier").ToString)
	'      If IsDBNull(rGAVrec("Zusatz13Lohn")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz13Lohn").ToString)
	'      '      If IsDBNull(rGAVrec("[FAR-Pflicht]")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("[FAR-Pflicht]").ToString)
	'      If IsDBNull(rGAVrec("Ferientext")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Ferientext").ToString)
	'      If IsDBNull(rGAVrec("Lohn13text")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Lohn13text").ToString)

	'      ' 48 - 50
	'      If IsDBNull(rGAVrec("StdWeek")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("StdWeek").ToString)
	'      If IsDBNull(rGAVrec("StdMonth")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("StdMonth").ToString)
	'      If IsDBNull(rGAVrec("StdYear")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("StdYear").ToString)

	'      ' 51 - 52
	'      If IsDBNull(rGAVrec("F_Alter")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("F_Alter").ToString)
	'      If IsDBNull(rGAVrec("L_Alter")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("L_Alter").ToString)

	'      ' 53 - 64
	'      If IsDBNull(rGAVrec("Zusatz1")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz1").ToString)
	'      If IsDBNull(rGAVrec("Zusatz2")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz2").ToString)
	'      If IsDBNull(rGAVrec("Zusatz3")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz3").ToString)
	'      If IsDBNull(rGAVrec("Zusatz4")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz4").ToString)
	'      If IsDBNull(rGAVrec("Zusatz5")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz5").ToString)
	'      If IsDBNull(rGAVrec("Zusatz6")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz6").ToString)
	'      If IsDBNull(rGAVrec("Zusatz7")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz7").ToString)
	'      If IsDBNull(rGAVrec("Zusatz8")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz8").ToString)
	'      If IsDBNull(rGAVrec("Zusatz9")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz9").ToString)
	'      If IsDBNull(rGAVrec("Zusatz10")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz10").ToString)
	'      If IsDBNull(rGAVrec("Zusatz11")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz11").ToString)
	'      If IsDBNull(rGAVrec("Zusatz12")) Then s.Add(CStr(-9)) Else s.Add(rGAVrec("Zusatz12").ToString)

	'      ' 65 - 66
	'      s.Add(rGAVrec("ID").ToString)
	'      s.Add(rGAVrec("GAVNr").ToString)

	'      i += 1
	'    End While




#Region "Hilf-Funktionen..."

	Function GetDbValue(ByVal myVale As String) As String

		If String.IsNullOrEmpty(myVale) Then
			Return String.Empty
		Else
			Return myVale
		End If

	End Function

	Public Function GetMachineIPAddress() As String

		Dim strHostName As String = ""
		Dim strIPAddress As String = ""
		Dim host As System.Net.IPHostEntry

		strHostName = System.Net.Dns.GetHostName()
		strIPAddress = System.Net.Dns.GetHostEntry(strHostName).HostName.ToString()

		host = System.Net.Dns.GetHostEntry(strHostName)
		Dim ip As System.Net.IPAddress
		For Each ip In host.AddressList
			Return ip.ToString()
		Next

		Return ""

	End Function

#End Region

End Class