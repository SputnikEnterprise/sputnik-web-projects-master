Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spgav_services/SPServiceUtil.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPServiceUtil
  Inherits System.Web.Services.WebService

  Dim _UserInfo As String = String.Empty

  <WebMethod(Description:="Die Benutzerdaten werden in Datenbank registrieren")> _
Function SaveUserData2WSDb(ByVal strUserData As String) As String
    Dim s As String = "Erfolg"
    Dim strMyString As String() = strUserData.Split("¦")

    Dim connString As String = My.Settings.Connstr_InfoService
    Dim strSQL As String = "[Insert LoginData into WS Database]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@UserData", strUserData)

    Try
      cmd.ExecuteNonQuery()

    Catch ex As Exception
      s = String.Format("Fehler (SaveUserData2WSDb): {0}", ex.Message)
      Return s

    End Try

    Return s
  End Function


  <WebMethod(Description:="Datasetversion: Zur Auflistung der Quellensteuerdaten auf der Client")> _
Function GetQstData(ByVal strUserID As String, _
                          ByVal strKanton As String, ByVal iYear As Integer, _
                          ByVal cEinkommen As Double, ByVal iKinder As Integer, _
                          ByVal strGruppe As String, ByVal strGeschlecht As String) As String
    Dim s As String = String.Empty
    Try
      If Not IsAllowedtoContinue(strUserID) Then
        SaveErrToDb(String.Format("{0}¦{1}", _
                            "Connection not allowed...", _
                            String.Empty))
        Return Nothing
      End If

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetQstData", ex.Message))

    End Try

    Dim connString As String = My.Settings.ConnStr_ServiceUtil
    Dim strSQL As String = String.Format("[Get QSTTarife For {0}{1}]", iYear, strKanton)
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@MANr", 0)
    param = cmd.Parameters.AddWithValue("@Einkommen", cEinkommen)
    param = cmd.Parameters.AddWithValue("@Gruppe", strGruppe)
    param = cmd.Parameters.AddWithValue("@Geschlecht", strGeschlecht)
    param = cmd.Parameters.AddWithValue("@Kinder", iKinder)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader

    Try
      While rGAVrec.Read
        s = String.Format("{0}¦{1}¦{2}|", _
                          rGAVrec("Mindest_Abzug").ToString, _
                          rGAVrec("Steuer_Proz").ToString, _
                          rGAVrec("Einkommen").ToString, _
                          rGAVrec("Schritt").ToString)

      End While

    Catch ex As Exception
      s = String.Format("GetPVLBerufWarning: {0}", ex.Message)
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Datasetversion: Zur Auflistung der Ländertabelle auf der Client")> _
Function GetCountrylist(ByVal strUserID As String, _
                            ByVal strCountryCode As String) As DataSet
    Try
      If Not IsAllowedtoContinue(strUserID) Then
        SaveErrToDb(String.Format("{0}¦{1}", _
                            "Connection not allowed...", _
                            String.Empty))
        Return Nothing
      End If

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetCountrylist", ex.Message))

    End Try

    Dim connString As String = My.Settings.ConnStr_ServiceUtil
    Dim strSQL As String = "[Get Countries]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn
    Dim rGAVrec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Code", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Code", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))

    adapter.SelectCommand.Parameters(0).Value = CType(strCountryCode.Trim, String)
    Try
      ' Die Datenbank anders nennen!!!
      adapter.Fill(rGAVrec, "Countries_Online")


    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetCountrylist", ex.Message))

    Finally

    End Try

    Return rGAVrec
  End Function

  <WebMethod(Description:="Datasetversion: Zur Auflistung der Bankdaten auf der Client")> _
  Function GetBankData(ByVal strUserID As String, _
                            ByVal strBCNr As String, ByVal iYear As Integer, _
                            ByVal cEinkommen As Double, ByVal iKinder As Integer, _
                            ByVal strGruppe As String, ByVal strGeschlecht As String) As DataSet
    Try
      If Not IsAllowedtoContinue(strUserID) Then
        SaveErrToDb(String.Format("{0}¦{1}", _
                            "Connection not allowed...", _
                            String.Empty))
        Return Nothing
      End If

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBankData", ex.Message))

    End Try

    Dim connString As String = My.Settings.ConnStr_ServiceUtil
    Dim strSQL As String = "[Get BankData]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn
    Dim rGAVrec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@BCNr", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "BCNr", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Bankname", _
                                                                                   Global.System.Data.SqlDbType.NVarChar, _
                                                                                   0, _
                                                                                   Global.System.Data.ParameterDirection.Input, _
                                                                                   0, 0, "Bankname", _
                                                                                   Global.System.Data.DataRowVersion.Original, _
                                                                                   False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@BankPLZ", _
                                                                                   Global.System.Data.SqlDbType.NVarChar, _
                                                                                   0, _
                                                                                   Global.System.Data.ParameterDirection.Input, _
                                                                                   0, 0, "BankPLZ", _
                                                                                   Global.System.Data.DataRowVersion.Original, _
                                                                                   False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Bankort", _
                                                                                   Global.System.Data.SqlDbType.NVarChar, _
                                                                                   0, _
                                                                                   Global.System.Data.ParameterDirection.Input, _
                                                                                   0, 0, "Bankort", _
                                                                                   Global.System.Data.DataRowVersion.Original, _
                                                                                   False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Swift", _
                                                                                   Global.System.Data.SqlDbType.NVarChar, _
                                                                                   0, _
                                                                                   Global.System.Data.ParameterDirection.Input, _
                                                                                   0, 0, "Swift", _
                                                                                   Global.System.Data.DataRowVersion.Original, _
                                                                                   False, Nothing, "", "", ""))

    adapter.SelectCommand.Parameters(0).Value = CType(strBCNr.Trim, String)
    Try
      ' Die Datenbank anders nennen!!!
      adapter.Fill(rGAVrec, "Banken_Online")


    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetBankData", ex.Message))

    Finally

    End Try

    Return rGAVrec
  End Function

  <WebMethod(Description:="Datasetversion: Zur Auflistung der Berufdaten auf der Client")> _
Function GetJobData(ByVal strUserID As String, _
                    ByVal strLanguage As String, _
                    ByVal strGeschlecht As String) As DataSet
    Try
      If Not IsAllowedtoContinue(strUserID) Then
        SaveErrToDb(String.Format("{0}¦{1}", _
                            "Connection not allowed...", _
                            String.Empty))
        Return Nothing
      End If

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetJobData", ex.Message))

    End Try

    Dim connString As String = My.Settings.ConnStr_ServiceUtil
    Dim strSQL As String = "[Get JobData]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn
    Dim rGAVrec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Language", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Geschlecht", _
                                                                                   Global.System.Data.SqlDbType.NVarChar, _
                                                                                   0, _
                                                                                   Global.System.Data.ParameterDirection.Input, _
                                                                                   0, 0, "Geschlecht", _
                                                                                   Global.System.Data.DataRowVersion.Original, _
                                                                                   False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters(0).Value = CType(strLanguage.Trim, String)
    adapter.SelectCommand.Parameters(0).Value = CType(strGeschlecht.Trim, String)

    Try
      ' Die Datenbank anders nennen!!!
      adapter.Fill(rGAVrec, "Job_Online")


    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetJobData", ex.Message))

    Finally

    End Try

    Return rGAVrec
  End Function

#Region "Sonstige Funktionen..."

  Function IsAllowedtoContinue(ByVal strUserInfo As String) As Boolean
    Dim _clsSystem As New ClsMain_Net
    _UserInfo = strUserInfo
    Dim aUserData As String() = strUserInfo.Split(CChar("¦"))

    Dim strResult As String = _clsSystem.GetUserID(aUserData(0), "GAVSonstige")
    strResult = SaveUserToDb(strResult)
    _UserInfo &= String.Format("¦{0}", strResult)

    Return True ' Temporär Lösung bis wir alle Guids haben.
    'Return Not strResult.ToLower.Contains("fehler:")
  End Function

  Function SaveUserToDb(ByVal strIDNr As String) As String
    Dim strResult As String = "Erfolgreich..."
    Dim connString As String = My.Settings.SettingDb_Connection
    Dim strSQL As String = String.Empty
    strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
    strSQL &= "IsWebService, CreatedOn) Values ("
    strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @IsWebService, @CreatedOn)"
    Dim aUserData As String() = strIDNr.Split(CChar("¦"))

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@Modulname", "SPServiceUtil.asmx")
      param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
      param = cmd.Parameters.AddWithValue("@UserID", aUserData(0))
      param = cmd.Parameters.AddWithValue("@Answer", aUserData(1))
      param = cmd.Parameters.AddWithValue("@RequestParam", Me._UserInfo)
      param = cmd.Parameters.AddWithValue("@IsWebService", 1)
      param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)
      SaveErrToDb(strResult)

    End Try

    Return strResult
  End Function

  Function SaveErrToDb(ByVal strErrorMessage As String) As String
    Dim strResult As String = "Erfolgreich..."
    Dim connString As String = My.Settings.SettingDb_Connection
    Dim strSQL As String = String.Empty
    strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
    strSQL &= "CreatedOn) Values ("
    strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPServiceUtil.asmx")
      param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
      param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
      param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
      param = cmd.Parameters.AddWithValue("@RequestParam", Me._UserInfo)
      param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      '     strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)

    End Try

    Return strResult
  End Function

#End Region

End Class