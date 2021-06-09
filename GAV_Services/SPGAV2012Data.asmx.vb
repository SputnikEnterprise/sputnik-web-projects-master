
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spgav_services/SPGAV2012Data.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPGAV2012Data
  Inherits System.Web.Services.WebService

  ''' <summary>
  ''' _UserInfo = String.Format("{0}¦{1}", _ClsPropSetting.GetMDGuid, strStationID)
  ''' strStationID = String.Format("{0}: {1}\{2} | {3}", Environment.MachineName, Environment.UserDomainName, _
  ''' Environment.UserName, _ClsPropSetting.GetSelectedMDData(1))
  ''' </summary>
  ''' <remarks></remarks>
  Dim _UserInfo As String = String.Empty

  Function IsAllowedtoContinue(ByVal strUserInfo As String) As Boolean
    Dim _clsSystem As New ClsMain_Net
    _UserInfo = strUserInfo
    Dim aUserData As String() = strUserInfo.Split(CChar("¦"))

    Dim strResult As String = _clsSystem.GetUserID(aUserData(0), "GAVUnia")
    strResult = SaveUserToDb(strResult)
    _UserInfo &= String.Format("¦{0}", strResult)

    Return True ' Temporär Lösung bis wir alle Guids haben.
    'Return Not strResult.ToLower.Contains("fehler:")
  End Function

  <WebMethod(Description:="Zur Auflistung der GAV-Berufe eines Kantons")> _
Function GetGruppe0ByKanton(ByVal strUserID As String, _
                            ByVal strGAVKanton As String, _
                            ByVal strPLZ As String, _
                            ByVal strLanguage As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    Try
      If Not IsAllowedtoContinue(strUserID) Then
        s.Add(String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}¦{6}¦{7}¦{8}¦{9}¦{10}¦{11}¦{12}¦{13}¦" & _
                            "{14}¦{15}¦{16}¦{17}¦{18}¦{19}¦{20}¦{21}¦{22}¦{23}¦{24}¦{25}", _
                            "Connection not allowed...", _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty))
        Return s
      End If

    Catch ex As Exception

    End Try

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVGruppe0]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@Kanton", strGAVKanton)
    param = cmd.Parameters.AddWithValue("@PLZ", strPLZ)
    param = cmd.Parameters.AddWithValue("@Language", strLanguage)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Gruppe0
    Dim i As Integer

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}¦{6}¦{7}¦{8}¦{9}¦{10}¦{11}¦{12}¦{13}¦" & _
                            "{14}¦{15}¦{16}¦{17}¦{18}¦{19}¦{20}¦{21}¦{22}¦{23}¦{24}¦{25}", _
                            rGAVrec("ID_Meta").ToString, _
                            rGAVrec("GAV_Number").ToString, _
                            rGAVrec("Version").ToString, _
                            rGAVrec("ave").ToString, _
                            rGAVrec("unia_validity_start").ToString, _
                            rGAVrec("unia_validity_end").ToString, _
                            rGAVrec("ave_validity_start").ToString, _
                            rGAVrec("ave_validity_end").ToString, _
                            rGAVrec("publication_date").ToString, _
                            rGAVrec("validity_start_date").ToString, _
                            rGAVrec("State").ToString, _
                            rGAVrec("ID_Calculator").ToString, _
                            rGAVrec("LohnZone").ToString, _
 _
                            rGAVrec("StdWeek").ToString, _
                            rGAVrec("StdMonth").ToString, _
                            rGAVrec("StdYear").ToString, _
 _
                            rGAVrec("FAG").ToString, _
                            rGAVrec("FAN").ToString, _
                            rGAVrec("WAG").ToString, _
                            rGAVrec("WAN").ToString, _
                            rGAVrec("VAG").ToString, _
                            rGAVrec("VAN").ToString, _
 _
                            rGAVrec("Name_de").ToString, _
                            rGAVrec("GAVKanton").ToString, _
                            rGAVrec("Resor_FAG").ToString, _
                            rGAVrec("Resor_FAN").ToString))

        i += 1
      End While

    Catch ex As Exception
      s.Add(String.Format("GetGruppe0ByKanton: {0}¦{1}¦{2}¦{3}¦{4}¦{5}¦{6}¦{7}¦{8}¦{9}¦{10}¦{11}¦{12}¦{13}¦" & _
                          "{14}¦{15}¦{16}¦{17}¦{18}¦{19}¦{20}¦{21}¦{22}¦{23}¦{24}¦{25}", _
                        ex.Message, _
                        String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                        String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                        String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                        String.Empty, String.Empty, String.Empty, String.Empty, _
                        String.Empty, String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Datasetversion: Zur Auflistung der Kundendokumente auf der Client (Gesamte Datensätze)")> _
Function GetGruppe0ByKanton_DS(ByVal strUserID As String, _
                            ByVal strGAVKanton As String, _
                            ByVal strPLZ As String, _
                            ByVal strLanguage As String) As DataSet
    Try
      If Not IsAllowedtoContinue(strUserID) Then
        SaveErrToDb(String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}¦{6}¦{7}¦{8}¦{9}¦{10}¦{11}¦{12}¦{13}¦" & _
                            "{14}¦{15}¦{16}¦{17}¦{18}¦{19}¦{20}¦{21}¦{22}¦{23}¦{24}¦{25}", _
                            "Connection not allowed...", _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty, String.Empty, _
                            String.Empty, String.Empty, String.Empty))
        Return Nothing
      End If

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetPVLBerufe_DS_0", ex.Message))

    End Try

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVGruppe0]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn
    Dim rGAVrec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Kanton", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Kanton", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@PLZ", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "PLZ", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@language", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "language", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    adapter.SelectCommand.Parameters(0).Value = CType(strGAVKanton.Trim, String)
    adapter.SelectCommand.Parameters(1).Value = CType(strPLZ.Trim, String)
    adapter.SelectCommand.Parameters(2).Value = CType(strLanguage.Trim, String)
    Try
      ' Die Datenbank anders nennen!!!
      adapter.Fill(rGAVrec, "PVL_Online")


    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetPVLBerufe_DS", ex.Message))

    Finally

    End Try

    Return rGAVrec
  End Function

  <WebMethod(Description:="Zur Auflistung der GAV-Berufe im Anhang1")> _
Function GetPVLAnhang1Berufe(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net

    Try
      If Not IsAllowedtoContinue(strUserID) Then
        s.Add(String.Format("{0}", "Connection not allowed..."))
        Return s
      End If

    Catch ex As Exception

    End Try

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get PVL Anhang1 Berufe]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader
    Dim i As Integer

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}", rGAVrec("GAV_Number").ToString))

        i += 1
      End While

    Catch ex As Exception
      s.Add(String.Format("GetPVLAnhang1Berufe: {0}", ex.Message))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Gibt die kritischen Infos über eine GAV-Number zurück. Das pflegen wir selbst ein!!!")> _
Function GetPVLBerufWarning(ByVal iGAVNr As Integer) As String
    Dim s As String = String.Empty

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get Sputnik Info about PVL GAVNumber]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@GAVNumber", iGAVNr)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader

    Try
      While rGAVrec.Read
        s = String.Format("{0}", rGAVrec("info").ToString)

      End While

    Catch ex As Exception
      s = String.Format("GetPVLBerufWarning: {0}", ex.Message)
      Return s

    End Try

    Return s
  End Function


  <WebMethod(Description:="Datasetversion: Zur Auflistung der Name aller Kategorien einer MetaNr")> _
Function GetGAVCategoryNames_DS(ByVal strUserID As String, _
                          ByVal ID_Meta As Integer, _
                          ByVal strLanguage As String) As DataSet
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCategoryNames]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn
    Dim rGAVrec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@ID_Meta", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "ID_Meta", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))
    adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Language", _
                                                                                       Global.System.Data.SqlDbType.NVarChar, _
                                                                                       0, _
                                                                                       Global.System.Data.ParameterDirection.Input, _
                                                                                       0, 0, "Language", _
                                                                                       Global.System.Data.DataRowVersion.Original, _
                                                                                       False, Nothing, "", "", ""))

    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    adapter.SelectCommand.Parameters(0).Value = CType(CStr(ID_Meta).Trim, String)
    adapter.SelectCommand.Parameters(1).Value = CType(CStr(strLanguage).Trim, String)
    Try
      ' Die Datenbank anders nennen!!!
      adapter.Fill(rGAVrec, "PVLCategory_Online")


    Catch ex As Exception
      SaveErrToDb(String.Format("GetGAVCategoryNames: {0}¦{1}¦{2}¦{3}", _
                          ex.Message, _
                          String.Empty, String.Empty, String.Empty))

    Finally

    End Try

    Return rGAVrec
  End Function

  <WebMethod(Description:="Zur Auflistung der Name aller Kategorien einer MetaNr")> _
Function GetGAVCategoryNames(ByVal strUserID As String, _
                          ByVal ID_Meta As Integer, _
                          ByVal strLanguage As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCategoryNames]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@ID_Meta", ID_Meta)
    param = cmd.Parameters.AddWithValue("@Language", strLanguage)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Category
    Dim i As Integer

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}¦{3}", rGAVrec("ID_Category").ToString, _
                            rGAVrec("ID_Calculator").ToString, _
                            rGAVrec("ID_BaseCategory").ToString, _
                            rGAVrec("Name_De").ToString))

        i += 1
      End While

    Catch ex As Exception
      s.Add(String.Format("GetGAVCategoryNames: {0}¦{1}¦{2}¦{3}", _
                          ex.Message, _
                          String.Empty, String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der Werte aller Kategorievalues einer CategoryNr")> _
Function GetGAVCategoryValues(ByVal strUserID As String, _
                              ByVal ID_Category As Integer) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCategoryValues]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@ID_Category", ID_Category)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Category
    Dim i As Integer

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}", rGAVrec("ID_CategoryValue").ToString, _
                            rGAVrec("ID_Category").ToString, _
                            rGAVrec("Text_De").ToString))

        i += 1
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVCategoryValues", ex.Message))
      s.Add(String.Format("GetGAVCategoryValues: {0}¦{1}¦{2}", ex.Message, _
                          String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der Werte aller Kategorievalues einer BaseCategoryValue")> _
Function GetGAVCategoryValuesWithBaseValue(ByVal strUserID As String, _
                                           ByVal ID_Category As Integer, _
                                           ByVal ID_BaseCategoryValue As Integer, _
                                           ByVal strLanguage As String) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCategoryValues With BaseNr]"
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@ID_Category", ID_Category)
    param = cmd.Parameters.AddWithValue("@ID_BaseCategoryValue", ID_BaseCategoryValue)
    param = cmd.Parameters.AddWithValue("@Language", strLanguage)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Category
    Dim i As Integer

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}", rGAVrec("ID_CategoryValue").ToString, _
                            rGAVrec("ID_Category").ToString, _
                            rGAVrec("Text_De").ToString))

        i += 1
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVCategoryValuesWithBaseValue", ex.Message))
      s.Add(String.Format("GetGAVCategoryValuesWithBaseValue: {0}¦{1}¦{2}", ex.Message, _
                          String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der Wert von Calculation-Db anhand Kategorievalues")> _
Function GetGAVCalculationValue(ByVal strUserID As String, _
                                ByVal strID_CategoryValues As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'Try
    '  If Not IsAllowedtoContinue(strUserID) Or strID_CategoryValues = String.Empty Then
    '    s.Add(String.Format("GetGAVCalculationValue: {0}¦{1}", _
    '                        "Connection not allowed...", String.Empty))
    '    Return s
    '  End If

    'Catch ex As Exception

    'End Try

    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCalculationValue]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@IDCategoryValue", strID_CategoryValues)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Category

    Try
      While rGAVrec.Read
        For i As Integer = 0 To rGAVrec.FieldCount - 1
          s.Add(String.Format("{0}¦{1}", rGAVrec.GetName(i), rGAVrec(i).ToString))
        Next
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVCalculationValue", ex.Message))
      s.Add(String.Format("GetGAVCalculationValue: {0}¦{1}", _
                          ex.Message, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der GAV-Details von Criterion-Db anhand ID_Meta")> _
Function GetGAVCriterionValue(ByVal strUserID As String, _
                                ByVal ID_Meta As Integer, _
                                ByVal strLanguage As String) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCriterionValues]"
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@IDMeta", ID_Meta)
    param = cmd.Parameters.AddWithValue("@Language", strLanguage)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Criterion

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}¦{3}¦{4}¦{5}", rGAVrec("ID_Criterion").ToString, _
                              rGAVrec("ID_Contract").ToString, _
                              rGAVrec("Element_ID").ToString, _
                              rGAVrec("Name_De").ToString, _
                              rGAVrec("name_fr").ToString, _
                              rGAVrec("name_it").ToString))
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVCriterionValue", ex.Message))
      s.Add(String.Format("GetGAVCriterionValue: {0}¦{1}¦{2}¦{3}¦{4}¦{5}", _
                          ex.Message, _
                          String.Empty, String.Empty, String.Empty, String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der GAV-Details von CriterionValue-Db anhand ID_Criterion")> _
Function GetGAVCriterionValueByIDCriterion(ByVal strUserID As String, _
                                           ByVal ID_Criterion As Integer, _
                                           ByVal strLanguage As String) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVCriterionValuesByIDCriterion]"
    If String.IsNullOrEmpty(strLanguage.Trim) Then strLanguage = "DE"
    If strLanguage = "I" Then strLanguage = "IT"
    If strLanguage = "F" Then strLanguage = "FR"
    If strLanguage = "D" Then strLanguage = "DE"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@IDCriterion", ID_Criterion)
    param = cmd.Parameters.AddWithValue("@Language", strLanguage)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Criterion

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}¦{3}", rGAVrec("ID_Criterion").ToString, _
                              rGAVrec("ID_CriterionValue").ToString, _
                              rGAVrec("txtText").ToString, _
                              rGAVrec("txtTable").ToString))
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVCriterionValueByIDCriterion", ex.Message))
      s.Add(String.Format("GetGAVCriterionValueByIDCriterion: {0}¦{1}¦{2}¦{3}", _
                          ex.Message, _
                          String.Empty, String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Zur Auflistung der GAV-Kantone von TaxonomyEntry-Db anhand ID_Meta")> _
Function GetGAVTaxonomyEntryValue(ByVal strUserID As String, _
                                ByVal ID_Meta As Integer) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.ConnStr_GAV
    Dim strSQL As String = "[Get GAVTaxonomyValues]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@IDMeta", ID_Meta)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-TaxonomyEntry

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}", rGAVrec("ID_Taxonomy_Entry").ToString, _
                              rGAVrec("ID_Taxonomy").ToString, _
                              rGAVrec("Value").ToString))
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVTaxonomyEntryValue", ex.Message))
      s.Add(String.Format("GetGAVTaxonomyEntryValue: {0}¦{1}¦{2}", _
                          ex.Message, _
                          String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

  <WebMethod(Description:="Kontrolle der GAV-Version anhand GAVNumber und GAVDatum")> _
Function GetGAVVersionValue(ByVal strUserID As String, _
                            ByVal iGAVNumber As Integer) As List(Of String)
    Dim s As New List(Of String)
    Dim connString As String = My.Settings.Connstr_InfoService
    Dim strSQL As String = "[Get Selected GAVVersion]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.StoredProcedure
    Dim param As System.Data.SqlClient.SqlParameter
    param = cmd.Parameters.AddWithValue("@GAVNumber", iGAVNumber)

    Dim rGAVrec As SqlDataReader = cmd.ExecuteReader

    Try
      While rGAVrec.Read
        s.Add(String.Format("{0}¦{1}¦{2}", rGAVrec("GAVNumber").ToString, _
                              rGAVrec("GAVDate").ToString, _
                              rGAVrec("GAVInfo").ToString))
      End While

    Catch ex As Exception
      SaveErrToDb(String.Format("Fehler: {0}{1}", vbNewLine, "GetGAVVersionValue", ex.Message))
      s.Add(String.Format("GetGAVVersionValue: {0}¦{1}¦{2}", _
                          ex.Message, _
                          String.Empty, String.Empty))
      Return s

    End Try

    Return s
  End Function

#Region "Hilf-Funktionen..."


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
      param = cmd.Parameters.AddWithValue("@Modulname", "SPGAV2012Data.asmx")
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
      param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPGAV2012Data.asmx")
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
