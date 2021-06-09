
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPKDFunctions.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPKDFunctions
  Inherits System.Web.Services.WebService

  <WebMethod(Description:="Zur Sicherung eines Kundendokuments aus der lokalen Datenbank.")> _
   Function SaveMyKDDocToDbFromLocalDB(ByVal MyDs As DataSet, ByVal strUserID As String) As String
    Dim strValue As String = "Erfolgreich..."
    Dim _clsSystem As New ClsMain_Net
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "KD_Guid")
    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)
    Dim connString As String = My.Settings.ConnStr_KD
    'Return connString

    Dim iKDNr As Integer
    Dim iZHDNr As Integer
    Dim iESNr As Integer
    Dim iESLohnNr As Integer
    Dim iRPNr As Integer
    Dim iRENr As Integer
    Dim strKD_Guid As String = String.Empty
    Dim strZHD_Guid As String = String.Empty
    Dim strDoc_Guid As String = String.Empty
    Dim strDoc_Info As String = String.Empty
    Dim strDoc_Art As String = String.Empty

    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)

    Try
      Dim dt As DataTable = MyDs.Tables("KD_Doc_Online")

      For i As Integer = 0 To dt.Rows.Count - 1
        iKDNr = CInt(dt.Rows(i)("KDNr"))
        iZHDNr = CInt(dt.Rows(i)("ZHDNr"))
        iESNr = CInt(dt.Rows(i)("ESNr"))
        iESLohnNr = CInt(dt.Rows(i)("ESLohnNr"))
        iRPNr = CInt(dt.Rows(i)("RPNr"))
        iRENr = CInt(dt.Rows(i)("RENr"))

        strKD_Guid = CStr(dt.Rows(i)("KD_Guid"))
        strZHD_Guid = CStr(dt.Rows(i)("ZHD_Guid"))
        strDoc_Guid = CStr(dt.Rows(i)("Doc_Guid"))
        strDoc_Info = CStr(dt.Rows(i)("Doc_Info"))
        strDoc_Art = CStr(dt.Rows(i)("Doc_Art"))

        If iESNr + iESLohnNr + iRPNr + iRENr = 0 Then
          ' ist sonstiges Dokument wie Lohnausweis, ZV, Arbeitgeberbescheinigung
          strValue = DeleteSelectedKDDocWithArt(strUserID, strKD_Guid, iKDNr, strDoc_Art, strDoc_Info)

        Else
          strValue = DeleteSelectedKDDocWithGuid(strUserID, strKD_Guid, iKDNr, strDoc_Guid)

        End If
        If strValue.ToLower.Contains("Fehler".ToLower) Then
          Return String.Format("Fehler beim Löschen von Dokumenten: {0} {1}", _
                If(iESNr + iESLohnNr + iRPNr + iRENr = 0, "DeleteSelectedkdDocWithArt", _
                   "DeleteSelectedkdDocWithGuid"), _
                   strValue)

        End If


        Dim Conn As SqlConnection = New SqlConnection(connString)
        Conn.Open()
        Try

          Dim strSQL As String = "Insert Into Kunden_Doc_Online ("
          strSQL &= "KDNr, ZHDNr, ESNr, ESLohnNr, RPNr, RENr, "
          strSQL &= "LogedUser_ID, Customer_ID, "
          strSQL &= "KD_Name, KD_Filiale, KD_Kanton, KD_Ort, KD_eMail, "
          strSQL &= "KD_Beruf, KD_Branche, KD_Berater, "

          strSQL &= "ZHD_Vorname, ZHD_Nachname, ZHDSex, Zhd_BriefAnrede, ZHD_eMail, "
          strSQL &= "ZHD_Beruf, ZHD_Branche, ZHD_GebDat, "

          strSQL &= "KD_Guid, ZHD_Guid, ZHD_Berater, "
          strSQL &= "Doc_Guid, Doc_Art, Doc_Info, "

          strSQL &= "Transfered_User, Transfered_On,"
          strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail, "
          strSQL &= "DocFilename, DocScan"
          strSQL &= ") Values ("

          strSQL &= "@KDNr, @ZHDNr, @ESNr, @ESLohnNr, @RPNr, @RENr, "
          strSQL &= "@LogedUser_ID, @Customer_ID, "
          strSQL &= "@KD_Name, @KD_Filiale, @KD_Kanton, @KD_Ort, @KD_eMail, "
          strSQL &= "@KD_Beruf, @KD_Branche, @KD_Berater, "

          strSQL &= "@ZHD_Vorname, @ZHD_Nachname, @ZHDSex, @Zhd_BriefAnrede, @ZHD_eMail, "
          strSQL &= "@ZHD_Beruf, @ZHD_Branche, @ZHD_GebDat, "
          strSQL &= "@KD_Guid, @ZHD_Guid, @ZHD_Berater, "

          strSQL &= "@Doc_Guid, @Doc_Art, @Doc_Info, "

          strSQL &= "@Transfered_User, @Transfered_On, "
          strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail, "
          strSQL &= "@DocFilename, @DocScan"

          strSQL &= ") "

          strSQL &= "Delete Customer_Users Where Customer_Id = @Customer_ID And User_Id = @LogedUser_ID "

          strSQL &= "Insert Into Customer_Users ("
          strSQL &= "User_ID, Customer_ID, Customer_Name, "
          strSQL &= "User_Initial, User_Sex, User_Vorname, User_Nachname, "
          strSQL &= "User_Telefon, User_Telefax, User_eMail, User_Homepage, User_Filiale, "
          strSQL &= "User_Picture, User_Sign, CreatedOn "
          strSQL &= ") Values ("

          strSQL &= "@LogedUser_ID, @Customer_ID, @Customer_Name, "
          strSQL &= "@User_Initial, @User_Sex, @User_Vorname, @User_Nachname, "
          strSQL &= "@User_Telefon, @User_Telefax, @User_eMail, @User_Homepage, @User_Filiale, "
          strSQL &= "@User_Picture, @User_Sign, @CreatedOn) "

          Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
          cmd.CommandType = Data.CommandType.Text
          Dim param As System.Data.SqlClient.SqlParameter

          param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
          param = cmd.Parameters.AddWithValue("@ZHDNr", iZHDNr)
          param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
          param = cmd.Parameters.AddWithValue("@ESLohnNr", iESLohnNr)
          param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
          param = cmd.Parameters.AddWithValue("@RENr", iRENr)

          param = cmd.Parameters.AddWithValue("@Customer_ID", dt.Rows(i)("Customer_ID"))
          param = cmd.Parameters.AddWithValue("@LogedUser_ID", dt.Rows(i)("LogedUser_ID"))

          param = cmd.Parameters.AddWithValue("@KD_Name", GetDbValue(dt.Rows(i)("KD_Name").ToString))
          param = cmd.Parameters.AddWithValue("@KD_Filiale", GetDbValue(dt.Rows(i)("KD_Filiale").ToString))
          param = cmd.Parameters.AddWithValue("@KD_Kanton", GetDbValue(CStr(dt.Rows(i)("KD_Kanton").ToString)))
          param = cmd.Parameters.AddWithValue("@KD_Ort", GetDbValue(CStr(dt.Rows(i)("KD_Ort").ToString)))
          param = cmd.Parameters.AddWithValue("@KD_eMail", GetDbValue(dt.Rows(i)("KD_eMail").ToString))

          param = cmd.Parameters.AddWithValue("@KD_Beruf", GetDbValue(dt.Rows(i)("KD_Beruf").ToString))
          param = cmd.Parameters.AddWithValue("@KD_Branche", GetDbValue(dt.Rows(i)("KD_Branche").ToString))
          param = cmd.Parameters.AddWithValue("@KD_Berater", GetDbValue(dt.Rows(i)("KD_Berater").ToString))

          param = cmd.Parameters.AddWithValue("@ZHD_Vorname", GetDbValue(dt.Rows(i)("ZHD_Vorname").ToString))
          param = cmd.Parameters.AddWithValue("@ZHD_Nachname", GetDbValue(dt.Rows(i)("ZHD_Nachname").ToString))

          param = cmd.Parameters.AddWithValue("@ZHDSex", GetDbValue(CStr(dt.Rows(i)("ZHDSex").ToString)))
          param = cmd.Parameters.AddWithValue("@Zhd_BriefAnrede", GetDbValue(CStr(dt.Rows(i)("Zhd_BriefAnrede").ToString)))
          param = cmd.Parameters.AddWithValue("@ZHD_eMail", GetDbValue(CStr(dt.Rows(i)("ZHD_eMail").ToString)))

          param = cmd.Parameters.AddWithValue("@ZHD_Beruf", GetDbValue(dt.Rows(i)("ZHD_Beruf").ToString))
          param = cmd.Parameters.AddWithValue("@ZHD_Branche", GetDbValue(dt.Rows(i)("ZHD_Branche").ToString))
          param = cmd.Parameters.AddWithValue("@ZHD_GebDat", GetDbValue(dt.Rows(i)("ZHD_GebDat").ToString))

          param = cmd.Parameters.AddWithValue("@KD_Guid", GetDbValue(CStr(dt.Rows(i)("KD_Guid").ToString)))
          param = cmd.Parameters.AddWithValue("@ZHD_Guid", GetDbValue(CStr(dt.Rows(i)("ZHD_Guid").ToString)))
          param = cmd.Parameters.AddWithValue("@ZHD_Berater", GetDbValue(CStr(dt.Rows(i)("ZHD_Berater").ToString)))

          param = cmd.Parameters.AddWithValue("@Doc_Guid", strDoc_Guid)
          param = cmd.Parameters.AddWithValue("@Doc_Art", dt.Rows(i)("Doc_Art"))
          param = cmd.Parameters.AddWithValue("@Doc_Info", dt.Rows(i)("Doc_Info"))

          param = cmd.Parameters.AddWithValue("@Transfered_User", _clsSystem.GetUserName)
          param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)

          param = cmd.Parameters.AddWithValue("@User_Nachname", dt.Rows(i)("User_Nachname"))
          param = cmd.Parameters.AddWithValue("@User_Vorname", dt.Rows(i)("User_Vorname"))
					param = cmd.Parameters.AddWithValue("@User_Telefon", dt.Rows(i)("_MDTelefon"))
					param = cmd.Parameters.AddWithValue("@User_Telefax", dt.Rows(i)("_MDTelefax"))
					param = cmd.Parameters.AddWithValue("@User_eMail", dt.Rows(i)("_MDeMail"))


					param = cmd.Parameters.AddWithValue("@DocFilename", dt.Rows(i)("DocFilename"))
          param = cmd.Parameters.AddWithValue("@DocScan", dt.Rows(i)("DocScan"))


          ' User-Daten anlegen...
          param = cmd.Parameters.AddWithValue("@Customer_Name", dt.Rows(i)("_MDName"))
          param = cmd.Parameters.AddWithValue("@User_Initial", dt.Rows(i)("_USInitial"))
          param = cmd.Parameters.AddWithValue("@User_Sex", dt.Rows(i)("_USAnrede"))
          param = cmd.Parameters.AddWithValue("@User_Homepage", dt.Rows(i)("_MDHomepage"))
          param = cmd.Parameters.AddWithValue("@User_Filiale", GetDbValue(dt.Rows(i)("USFiliale").ToString))
          param = cmd.Parameters.AddWithValue("@User_Picture", dt.Rows(i)("USBild"))
          param = cmd.Parameters.AddWithValue("@User_Sign", dt.Rows(i)("USSign"))
          param = cmd.Parameters.AddWithValue("@CreatedOn", Now.ToString)

          cmd.ExecuteNonQuery()           ' Datensatz einfügen...
          cmd.Parameters.Clear()


          strValue = RegisterToKundenDb_0(strKD_Guid, strZHD_Guid, MyDs, "KD_Doc_Online", strUserID, _
                                        iKDNr, _
                                        GetDbValue(dt.Rows(i)("KD_Name").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Berater").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_AGB_WOS").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Kanton").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Ort").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Beruf").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Branche").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_EMail").ToString), _
                                        GetDbValue(dt.Rows(i)("KD_Language").ToString))


          If strValue.ToLower.Contains("erfolgreich".ToLower) Then
            If iZHDNr > 0 Then strValue = RegisterToZHDDb_0(strKD_Guid, strZHD_Guid, MyDs, "KD_Doc_Online", strUserID, _
                                                            iKDNr, _
                                                            iZHDNr, _
                                                            GetDbValue(CStr(dt.Rows(i)("ZHD_Guid").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_Vorname").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_Nachname").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_Beruf").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_Branche").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("ZHDSex").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_GebDat").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_BriefAnrede").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("Zhd_EMail").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("ZHD_Berater").ToString)), _
                                                            GetDbValue(CStr(dt.Rows(i)("ZHD_AGB_WOS").ToString)))
          End If

          If strValue.ToLower.Contains("erfolgreich".ToLower) Then
            strValue = "Ihre Daten wurden erfolgreich importiert..."

          Else
            Return strValue

          End If



        Catch ex As Exception
          strValue = String.Format("Fehler ({0}): {1}", "SaveMyKDDocToDbFromLocalDB_1", ex.Message)

          If Not Conn Is Nothing Then
            Conn.Close()
            Conn.Dispose()
          End If
          Return strValue

        End Try
      Next


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "SaveMyKDDocToDbFromLocalDB_0", ex.Message)

    End Try

    Return strValue
  End Function

  Function RegisterToKundenDb_0(ByVal strKD_Guid As String, _
                              ByVal strZHD_Guid As String, _
                              ByVal MyDs As DataSet, _
                              ByVal strTableName As String, _
                              ByVal strUserID As String, _
                              ByVal iKDNr As Integer, _
                              ByVal KD_Name As String, ByVal KD_Berater As String, ByVal KD_AGB_WOS As String, _
                              ByVal KD_Kanton As String, ByVal KD_Ort As String, _
                              ByVal KD_Beruf As String, ByVal KD_Branche As String, ByVal KD_EMail As String, _
                              ByVal KD_Language As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    ' Datensatz löschen
    DeleteSelectedKundenRec(strUserID, iKDNr) ' strKD_Guid) ', strZHD_Guid)

    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "Insert Into Kunden ("
    strSQL &= "Customer_ID, KDNr, KD_Guid, KD_Name, "
    strSQL &= "KD_Berater, KD_AGB_WOS, KD_Kanton, KD_Ort, KD_Beruf, KD_Branche, KD_EMail, KD_Language, "
    strSQL &= "Transfered_On "
    strSQL &= ") Values ("

    strSQL &= "@Customer_ID, @KDNr, @KD_Guid, @KD_Name, "
    strSQL &= "@KD_Berater, @KD_AGB_WOS, @KD_Kanton, @KD_Ort, @KD_Beruf, @KD_Branche, @KD_EMail, @KD_Language, "
    strSQL &= "@Transfered_On "

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables(strTableName)

      param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)

      param = cmd.Parameters.AddWithValue("@KD_Name", KD_Name)
      param = cmd.Parameters.AddWithValue("@KD_Berater", KD_Berater)
      param = cmd.Parameters.AddWithValue("@KD_AGB_WOS", KD_AGB_WOS)

      param = cmd.Parameters.AddWithValue("@KD_Kanton", KD_Kanton)
      param = cmd.Parameters.AddWithValue("@KD_Ort", KD_Ort)
      param = cmd.Parameters.AddWithValue("@KD_Beruf", KD_Beruf)
      param = cmd.Parameters.AddWithValue("@KD_Branche", KD_Branche)
      param = cmd.Parameters.AddWithValue("@KD_EMail", KD_EMail)
      param = cmd.Parameters.AddWithValue("@KD_Language", KD_Language)

      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)


      cmd.ExecuteNonQuery()           ' Kunden einfügen...
      strValue = "RegisterToKundenDb_0: Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "RegisterToKundenDb_0", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  Function RegisterToZHDDb_0(ByVal strKD_Guid As String, _
                           ByVal strZHD_Guid As String, _
                           ByVal MyDs As DataSet, _
                           ByVal strTableName As String, _
                           ByVal strUserID As String, _
                           ByVal iKDNr As Integer, _
                           ByVal iZHDNr As Integer, _
                           ByVal ZHD_Guid As String, _
                           ByVal Zhd_Vorname As String, _
                           ByVal Zhd_Nachname As String, _
                           ByVal Zhd_Beruf As String, _
                           ByVal Zhd_Branche As String, _
                           ByVal ZHDSex As String, _
                           ByVal Zhd_GebDat As String, _
                           ByVal Zhd_BriefAnrede As String, _
                           ByVal Zhd_EMail As String, _
                           ByVal ZHD_Berater As String, _
                           ByVal ZHD_AGB_WOS As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    ' Datensatz löschen, darf vernachlässigt werden schon oben beim (DeleteSelectedKundenRec) gelöscht wird
    'DeleteSelectedZHDRec(strUserID, strKD_Guid, strZHD_Guid)
    DeleteSelectedZHDRec(strUserID, iKDNr, iZHDNr)

    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "Insert Into Kunden_ZHD ("
    strSQL &= "Customer_ID, KDNr, KD_Guid, "
    strSQL &= "ZHDNr, ZHD_Guid, Zhd_Vorname, Zhd_Nachname, "
    strSQL &= "Zhd_Beruf, Zhd_Branche, ZHDSex, Zhd_GebDat, Zhd_BriefAnrede, ZHD_EMail, "
    strSQL &= "ZHD_Berater, ZHD_AGB_WOS, "
    strSQL &= "Transfered_On "
    strSQL &= ") Values ("

    strSQL &= "@Customer_ID, @KDNr, @KD_Guid, "
    strSQL &= "@ZHDNr, @ZHD_Guid, @Zhd_Vorname, @Zhd_Nachname, "
    strSQL &= "@Zhd_Beruf, @Zhd_Branche, @ZHDSex, @Zhd_GebDat, @Zhd_BriefAnrede, @ZHD_EMail, "
    strSQL &= "@ZHD_Berater, @ZHD_AGB_WOS, "
    strSQL &= "@Transfered_On "

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables(strTableName)

      param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)

      param = cmd.Parameters.AddWithValue("@ZHDNr", iZHDNr)
      param = cmd.Parameters.AddWithValue("@ZHD_Guid", ZHD_Guid)
      param = cmd.Parameters.AddWithValue("@Zhd_Vorname", Zhd_Vorname)
      param = cmd.Parameters.AddWithValue("@Zhd_Nachname", Zhd_Nachname)
      param = cmd.Parameters.AddWithValue("@Zhd_Beruf", Zhd_Beruf)
      param = cmd.Parameters.AddWithValue("@Zhd_Branche", Zhd_Branche)
      param = cmd.Parameters.AddWithValue("@ZHDSex", ZHDSex)
      param = cmd.Parameters.AddWithValue("@Zhd_GebDat", Zhd_GebDat)
      param = cmd.Parameters.AddWithValue("@Zhd_BriefAnrede", Zhd_BriefAnrede)
      param = cmd.Parameters.AddWithValue("@Zhd_EMail", Zhd_EMail)
      param = cmd.Parameters.AddWithValue("@ZHD_Berater", ZHD_Berater)
      param = cmd.Parameters.AddWithValue("@ZHD_AGB_WOS", ZHD_AGB_WOS)

      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)


      cmd.ExecuteNonQuery()           ' ZHD einfügen...
      cmd.Parameters.Clear()

      strValue = "RegisterToZHDDb_0: Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "RegisterToZHDDb_0", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function



  <WebMethod(Description:="Speichern von Kunden-Daten (Kunden).")> _
  Function RegisterToKundenDb(ByVal strKD_Guid As String, _
                              ByVal strZHD_Guid As String, _
                              ByVal MyDs As DataSet, _
                              ByVal strTableName As String, _
                              ByVal strUserID As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    ' Datensatz löschen
    DeleteSelectedKundenRec(strUserID, strKD_Guid) ', strZHD_Guid)

    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "Insert Into Kunden ("
    strSQL &= "Customer_ID, KDNr, KD_Guid, KD_Name, "
    strSQL &= "KD_Berater, KD_AGB_WOS, KD_Kanton, KD_Ort, KD_Beruf, KD_Branche, KD_EMail, KD_Language, "
    strSQL &= "Transfered_On "
    strSQL &= ") Values ("

    strSQL &= "@Customer_ID, @KDNr, @KD_Guid, @KD_Name, "
    strSQL &= "@KD_Berater, @KD_AGB_WOS, @KD_Kanton, @KD_Ort, @KD_Beruf, @KD_Branche, @KD_EMail, @KD_Language, "
    strSQL &= "@Transfered_On "

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables(strTableName)

      param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", GetDbValue(CStr(dt.Rows(0)("KDNr").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)

      param = cmd.Parameters.AddWithValue("@KD_Name", GetDbValue(CStr(dt.Rows(0)("KD_Name").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_Berater", GetDbValue(CStr(dt.Rows(0)("KD_Berater").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_AGB_WOS", GetDbValue(CStr(dt.Rows(0)("KD_AGB_WOS").ToString)))

      param = cmd.Parameters.AddWithValue("@KD_Kanton", GetDbValue(dt.Rows(0)("KD_Kanton").ToString))
      param = cmd.Parameters.AddWithValue("@KD_Ort", GetDbValue(dt.Rows(0)("KD_Ort").ToString))
      param = cmd.Parameters.AddWithValue("@KD_Beruf", GetDbValue(CStr(dt.Rows(0)("KD_Beruf").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_Branche", GetDbValue(dt.Rows(0)("KD_Branche").ToString))
      param = cmd.Parameters.AddWithValue("@KD_EMail", GetDbValue(CStr(dt.Rows(0)("KD_EMail").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_Language", GetDbValue(dt.Rows(0)("KD_Language").ToString))

      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)


      cmd.ExecuteNonQuery()           ' Kunden einfügen...
      strValue = "RegisterToKundenDb: Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "RegisterToKundenDb", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Speichern von ZHD-Daten (Kunden_ZHD).")> _
  Function RegisterToZHDDb(ByVal strKD_Guid As String, _
                           ByVal strZHD_Guid As String, _
                           ByVal MyDs As DataSet, _
                           ByVal strTableName As String, _
                           ByVal strUserID As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    ' Datensatz löschen, darf vernachlässigt werden schon oben beim (DeleteSelectedKundenRec) gelöscht wird
    DeleteSelectedZHDRec(strUserID, strKD_Guid, strZHD_Guid)

    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "Insert Into Kunden_ZHD ("
    strSQL &= "Customer_ID, KDNr, KD_Guid, "
    strSQL &= "ZHDNr, ZHD_Guid, Zhd_Vorname, Zhd_Nachname, "
    strSQL &= "Zhd_Beruf, Zhd_Branche, ZHDSex, Zhd_GebDat, Zhd_BriefAnrede, ZHD_EMail, "
    strSQL &= "ZHD_Berater, ZHD_AGB_WOS, "
    strSQL &= "Transfered_On "
    strSQL &= ") Values ("

    strSQL &= "@Customer_ID, @KDNr, @KD_Guid, "
    strSQL &= "@ZHDNr, @ZHD_Guid, @Zhd_Vorname, @Zhd_Nachname, "
    strSQL &= "@Zhd_Beruf, @Zhd_Branche, @ZHDSex, @Zhd_GebDat, @Zhd_BriefAnrede, @ZHD_EMail, "
    strSQL &= "@ZHD_Berater, @ZHD_AGB_WOS, "
    strSQL &= "@Transfered_On "

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables(strTableName)

      param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", GetDbValue(CStr(dt.Rows(0)("KDNr").ToString)))
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)

      param = cmd.Parameters.AddWithValue("@ZHDNr", GetDbValue(CStr(dt.Rows(0)("ZHDNr").ToString)))
      param = cmd.Parameters.AddWithValue("@ZHD_Guid", GetDbValue(CStr(dt.Rows(0)("ZHD_Guid").ToString)))
      param = cmd.Parameters.AddWithValue("@Zhd_Vorname", GetDbValue(dt.Rows(0)("Zhd_Vorname").ToString))
      param = cmd.Parameters.AddWithValue("@Zhd_Nachname", GetDbValue(dt.Rows(0)("Zhd_Nachname").ToString))
      param = cmd.Parameters.AddWithValue("@Zhd_Beruf", GetDbValue(CStr(dt.Rows(0)("Zhd_Beruf").ToString)))
      param = cmd.Parameters.AddWithValue("@Zhd_Branche", GetDbValue(CStr(dt.Rows(0)("Zhd_Branche").ToString)))
      param = cmd.Parameters.AddWithValue("@ZHDSex", GetDbValue(CStr(dt.Rows(0)("ZHDSex").ToString)))
      param = cmd.Parameters.AddWithValue("@Zhd_GebDat", GetDbValue(CStr(dt.Rows(0)("Zhd_GebDat").ToString)))
      param = cmd.Parameters.AddWithValue("@Zhd_BriefAnrede", GetDbValue(CStr(dt.Rows(0)("Zhd_BriefAnrede").ToString)))
      param = cmd.Parameters.AddWithValue("@Zhd_EMail", GetDbValue(CStr(dt.Rows(0)("Zhd_EMail").ToString)))
      param = cmd.Parameters.AddWithValue("@ZHD_Berater", GetDbValue(CStr(dt.Rows(0)("ZHD_Berater").ToString)))
      param = cmd.Parameters.AddWithValue("@ZHD_AGB_WOS", GetDbValue(CStr(dt.Rows(0)("ZHD_AGB_WOS").ToString)))

      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)


      cmd.ExecuteNonQuery()           ' ZHD einfügen...
      cmd.Parameters.Clear()

      strValue = "RegisterToZHDDb: Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "RegisterToZHDDb", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function


#Region "Funktionen zum Löschen der Datensätze vom Kundendoc..."

  <WebMethod(Description:="Löscht das ausgewählte Kunden-Document.")> _
Function DeleteSelectedKDDocWithGuid(ByVal strUserID As String, _
                              ByVal strKD_Guid As String, _
                              ByVal iKDNr As Integer, _
                              ByVal strDoc_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKDDocWithGuid"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDDoc With DocGuid]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@Doc_Guid", strDoc_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedKDDocWithGuid: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Kunden-Document.")> _
  Function DeleteSelectedKDDocWithArt(ByVal strUserID As String, _
                                ByVal strKD_Guid As String, _
                                ByVal iKDNr As Integer, _
                                ByVal strDoc_Art As String, _
                                ByVal strDoc_Info As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKDDocWithArt"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDDoc With DocInfo]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@Doc_Art", strDoc_Art)
      param = cmd.Parameters.AddWithValue("@Doc_Info", strDoc_Info)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_DeleteSelectedKDDocWithArt: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-Einsatz).")> _
  Function DeleteSelectedKDESDoc(ByVal strUserID As String, _
                                ByVal strKD_Guid As String, _
                                ByVal iKDNr As Integer, _
                                ByVal iESNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKDESDoc"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDESDoc]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_DeleteSelectedKDESDoc: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-RP).")> _
  Function DeleteSelectedKDRPDoc(ByVal strUserID As String, _
                                ByVal strKD_Guid As String, _
                                ByVal iKDNr As Integer, _
                                ByVal iRPNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKDRPDoc"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDRPDoc]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_DeleteSelectedKDRPDoc: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-RE).")> _
  Function DeleteSelectedKDREDoc(ByVal strUserID As String, _
                                ByVal strKD_Guid As String, _
                                ByVal iKDNr As Integer, _
                                ByVal iRENr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKDREDoc"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDREDoc]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@RENr", iRENr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_DeleteSelectedKDREDoc: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (KD-Alle).")> _
Function DeleteSelectedAllKDDoc(ByVal strUserID As String, _
                              ByVal strKD_Guid As String, _
                              ByVal iKDNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedAllKDDoc"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected AllKDDoc]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_DeleteSelectedAllKDDoc: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  ''' <summary>
  ''' löscht den ausgewählten Kundendatensatz NUR aus der Kunden
  ''' </summary>
  ''' <param name="strUserID"></param>
  ''' <param name="strKD_Guid"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  Function DeleteSelectedKundenRec(ByVal strUserID As String, _
                                   ByVal strKD_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KD-Rec From Kunden]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      '      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

      cmd.ExecuteNonQuery()           ' Datensatz löschen...

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  Function DeleteSelectedKundenRec(ByVal strUserID As String, _
                                 ByVal iKDNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KD-Rec From Kunden With KDNr]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      '      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

      cmd.ExecuteNonQuery()           ' Datensatz löschen...

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  Function DeleteSelectedZHDRec(ByVal strUserID As String, _
                            ByVal iKDNr As Integer, _
                            ByVal iZHDNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedZHDRec"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDZHD-Rec From Kunden ZHD With ZHDNr]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KDNr", iKDNr)
      param = cmd.Parameters.AddWithValue("@ZHDNr", iZHDNr)

      cmd.ExecuteNonQuery()           ' Datensatz löschen...

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedZHDRec", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  ''' <summary>
  ''' Löscht den Kundendatensatz (Kunden) inklusive ZHD (Kunden_ZHD)
  ''' </summary>
  ''' <param name="strUserID"></param>
  ''' <param name="strKD_Guid"></param>
  ''' <param name="strZHD_Guid"></param>
  ''' <remarks></remarks>
  <WebMethod(Description:="Löscht den ausgewählten Kunden (Kunden).")> _
Function DeleteSelectedKundenRec(ByVal strUserID As String, _
                            ByVal strKD_Guid As String, _
                            ByVal strZHD_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKundenRec"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KD-Rec From ALL KD_Db]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

      cmd.ExecuteNonQuery()           ' Datensatz löschen...

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKundenRec", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht den ausgewählten Kunden_ZHD (Kunden_ZHD).")> _
Function DeleteSelectedZHDRec(ByVal strUserID As String, _
                            ByVal strKD_Guid As String, _
                            ByVal strZHD_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedZHDRec"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Delete Selected KDZHD-Rec From Kunden ZHD]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@KD_Guid", strKD_Guid)
      param = cmd.Parameters.AddWithValue("@ZHD_Guid", strZHD_Guid)

      cmd.ExecuteNonQuery()           ' Datensatz löschen...

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedZHDRec", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Ändert das ALTE KD_Guid durch NEUEN KD_Guid in allen Tabellen.")> _
Function ChangeKDGuidWithNewGuid(ByVal strUserID As String, _
                            ByVal strOldKD_Guid As String, _
                            ByVal strNewKD_Guid As String) As String
    Dim strValue As String = "Erfolgreich: ChangeKDGuidWithNewGuid"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Change Selected KDGuid With NewGuid]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@OldKD_Guid", strOldKD_Guid)
      param = cmd.Parameters.AddWithValue("@NewKD_Guid", strNewKD_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz ändern...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_ChangeKDGuidWithNewGuid: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Ändert das ALTE KDZHD_Guid durch NEUEN KDZHD_Guid in allen Tabellen.")> _
Function ChangeKDZHDGuidWithNewGuid(ByVal strUserID As String, _
                                    ByVal strKD_Guid As String, _
                                    ByVal strOldKDZHD_Guid As String, _
                                    ByVal strNewKDZHD_Guid As String) As String
    Dim strValue As String = "Erfolgreich: ChangeKDZHDGuidWithNewGuid"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Change Selected KDZHDGuid With NewGuid]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@OldKD_Guid", strOldKDZHD_Guid)
      param = cmd.Parameters.AddWithValue("@NewKD_Guid", strNewKDZHD_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz ändern...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_ChangeKDZHDGuidWithNewGuid: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

#End Region

#Region "Funktionen zur Auflistung der Datensätze..."

  <WebMethod(Description:="Zur Auflistung der Kundendokumente auf der Client (Gesamte Datensätze)")> _
Function GetKDDoc_RecToShowOnClient(ByVal strUserID As String, ByVal strKDNr As String, _
                  ByVal strDocArt As String, ByVal strTransferedOn As String, _
                  ByVal strOperator As String, ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Get KDDocrec2ShowOnClient]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rKDrec As New DataSet
    ' ---------------------------------------------------------------------------------------

    Try
      adapter.SelectCommand.CommandText = strSQL
      adapter.SelectCommand.CommandType = Global.System.Data.CommandType.StoredProcedure
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@UserID", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "UserID", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@KDNr", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "KDNr", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@DocArt", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "DocArt", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@TransferedOn", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "TransferedOn", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Operator", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "Operator", _
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
      adapter.SelectCommand.Parameters(1).Value = CType(strKDNr.Trim, String)
      adapter.SelectCommand.Parameters(2).Value = CType(strDocArt.Trim, String)
      adapter.SelectCommand.Parameters(3).Value = CType(strTransferedOn.Trim, String)
      adapter.SelectCommand.Parameters(4).Value = CType(If(strOperator.Trim = String.Empty, _
                                                           ">=", strOperator.Trim), String)
      adapter.SelectCommand.Parameters(5).Value = CType(If(strSortkeys.Trim = String.Empty, _
                                                           "KDDoc.Transfered_On DESC", strSortkeys.Trim), String)
      ' Die Datenbank anders nennen!!!
      ' Return rKDrec
      adapter.Fill(rKDrec, "Kunden_Doc_Online")

      strRecResult = String.Format(If(rKDrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rKDrec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)
      WriteConnectionHistory(strUserID, strKDNr, strDocArt, strTransferedOn, "", strSortkeys, strRecResult)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rKDrec
  End Function

#End Region


#Region "Hilf-Funktionen..."

  Function GetDbValue(ByVal myVale As String) As String

    If String.IsNullOrEmpty(myVale) Then
      Return String.Empty
    Else
      Return myVale
    End If

  End Function

  Function checkfolderexists(ByVal sFolder As String) As Boolean

    'split the directory out so that you can do a check for each folder
    Dim sFolderSplit() As String = sFolder.Split(CChar("\"))

    Dim strFolder As String = ""          'current folder to check              
    Try
      'loop through each folder
      For x As Int16 = 0 To CShort(sFolderSplit.GetUpperBound(0) - 1)
        'set the current folder
        strFolder += sFolderSplit(x) & "\"

        'do check
        If Directory.Exists(strFolder) = False Then
          'try to create the none existing folder
          Directory.CreateDirectory(strFolder)
        End If
      Next

      'looped through all folders successfully
      Return True
    Catch ex As Exception
      'error occured
      Return False
    End Try

  End Function

  Function GetFileToByte(ByVal filePath As String) As Byte()
    Dim stream As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)
    Dim reader As BinaryReader = New BinaryReader(stream)

    Dim photo() As Byte = Nothing
    Try

      photo = reader.ReadBytes(CInt(stream.Length))
      reader.Close()
      stream.Close()

    Catch ex As Exception

    End Try

    Return photo
  End Function

  Public Function ConvertDataReaderToDataSet(ByVal reader As SqlDataReader) As DataSet
    Dim dataSet As DataSet = New DataSet
    Dim schemaTable As DataTable = reader.GetSchemaTable()
    Dim dataTable As DataTable = New DataTable
    Dim intCounter As Integer
    For intCounter = 0 To schemaTable.Rows.Count - 1
      Dim dataRow As DataRow = schemaTable.Rows(intCounter)
      Dim columnName As String = CType(dataRow("ColumnName"), String)
      Dim column As DataColumn = New DataColumn(columnName, _
           CType(dataRow("DataType"), Type))
      dataTable.Columns.Add(column)
    Next
    dataSet.Tables.Add(dataTable)
    While reader.Read()
      Dim dataRow As DataRow = dataTable.NewRow()
      For intCounter = 0 To reader.FieldCount - 1
        dataRow(intCounter) = reader.GetValue(intCounter)
      Next
      dataTable.Rows.Add(dataRow)
    End While

    Return dataSet
  End Function

  Sub CountDocWithUserID(ByVal strUserID As String)
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strQuery As String = "[Refresh DocCount With UserID]"


    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@UseCount", strUserID)


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

#End Region

End Class