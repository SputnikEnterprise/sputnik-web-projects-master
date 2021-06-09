
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPMAData.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPMAData
  Inherits System.Web.Services.WebService

  <WebMethod(Description:="Zur Auflistung der Kandidaten-Berufe (Qualifikation)")> _
Function GetMABeruf_Titel(ByVal strUserID As String) As List(Of String)
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

  <WebMethod(Description:="Zur Auflistung der Kandidaten-Filiale (Filiale)")> _
Function GetMA_Filiale(ByVal strUserID As String) As List(Of String)
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

  <WebMethod(Description:="Zur Auflistung der Kandidaten-Ort (Ort)")> _
Function GetMA_Ort(ByVal strUserID As String) As List(Of String)
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

  <WebMethod(Description:="Zur Auflistung der Kandidaten-Kanton (Kanton)")> _
Function GetMA_Kanton(ByVal strUserID As String) As List(Of String)
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

  <WebMethod(Description:="Zur Auflistung der Kandidaten (Gesamte Datensätze)")> _
Function GetMA_Rec(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strFiliale As String, _
                      ByVal strSortkeys As String) As DataSet
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
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@UserID", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "UserID", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Beruf", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Beruf", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Ort", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Ort", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Kanton", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Kanton", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Filiale", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Filiale", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@SortString", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "SortString", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters(0).Value = CType(strUserID.Trim, String)
    adapter.SelectCommand.Parameters(1).Value = CType(strBeruf.Trim, String)
    adapter.SelectCommand.Parameters(2).Value = CType(strOrt.Trim, String)
    adapter.SelectCommand.Parameters(3).Value = CType(strKanton.Trim, String)
    adapter.SelectCommand.Parameters(4).Value = CType(strFiliale.Trim, String)
    adapter.SelectCommand.Parameters(5).Value = CType(If(strSortkeys.Trim = String.Empty, _
                                                         "MA.Transfered_On DESC", strSortkeys.Trim), String)
    Try
      ' Die Datenbank anders nennen!!!
      ' Return rMArec
      adapter.Fill(rMArec, "Kandidaten_Online")

      strRecResult = String.Format(If(rMArec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rMArec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rMArec
  End Function

  <WebMethod(Description:="Zur Auflistung der Kandidaten-Datensätze als Array (Alle gesuchten Datensätze)")> _
  Function GetMA_Rec_In_Array(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strFiliale As String, _
                      ByVal strSortkeys As String) As List(Of String)
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
    param = cmd.Parameters.AddWithValue("@Beruf", strBeruf.Trim)
    param = cmd.Parameters.AddWithValue("@Ort", strOrt.Trim)
    param = cmd.Parameters.AddWithValue("@Kanton", strKanton.Trim)
    param = cmd.Parameters.AddWithValue("@Filiale", strFiliale.Trim)
    param = cmd.Parameters.AddWithValue("@SortString", If(strSortkeys.Trim = String.Empty, _
                                                          "MA.Transfered_On DESC", strSortkeys.Trim))

    Dim rMArec As SqlDataReader = cmd.ExecuteReader

    While rMArec.Read
      For i As Integer = 0 To rMArec.FieldCount - 1
        liMArec.Add(String.Format("{0}={1}", rMArec.GetName(i).ToString, rMArec(i).ToString))
      Next i
    End While

    Try
      strRecResult = String.Format(If(liMArec.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), liMArec.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return liMArec
  End Function

  Sub WriteConnectionHistory(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strFiliale As String, _
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
      param = cmd.Parameters.AddWithValue("@ModulParameter", strUserID & vbNewLine & _
                                          "Beruf: " & strBeruf & vbNewLine & _
                                          "Ort: " & strOrt & vbNewLine & _
                                          "Kanton: " & strKanton & vbNewLine & _
                                          "Filiale: " & strFiliale & vbNewLine & _
                                          "Sortkey: " & strSortkeys & vbNewLine & vbNewLine & _
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