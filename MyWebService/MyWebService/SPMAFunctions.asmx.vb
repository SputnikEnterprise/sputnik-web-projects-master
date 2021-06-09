
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports SPWebService.SPUtilities

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPMAFunctions.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPMAFunctions
  Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPMAFunctions"

#Region "Speichervariable..."

	Dim _BaseDirectory As String = My.Settings.FileSaveToLocation
  Private ReadOnly Property BaseDirectory() As String
    Get
      Return _BaseDirectory
    End Get
  End Property

  Dim _objFile As Byte()
  Private Property objFile() As Byte()
    Get
      Return _objFile
    End Get
    Set(ByVal value As Byte())
      _objFile = value
    End Set
  End Property

  Dim _objFileStream As FileStream
  Private Property objFileStream() As FileStream
    Get
      Return _objFileStream
    End Get
    Set(ByVal value As FileStream)
      _objFileStream = value
    End Set
  End Property

  Dim _fileName As String
  Private Property fileName() As String
    Get
      Return _fileName
    End Get
    Set(ByVal value As String)
      _fileName = value
    End Set
  End Property

#End Region


#Region "Funktionen für Normale Kandidaten zu speichern..."

  ''' <summary>
  ''' Diese Funktion dient zur Speicherung der Kandidaten in der Hauptdatenbank (SPContract). 
  ''' Vor der Sicherung löscht er den Datensatz anhand Transfered_Guid und MANr.
  ''' </summary>
  ''' <param name="strMA_Guid"></param>
  ''' <param name="iMANr"></param>
  ''' <param name="MyDs"></param>
  ''' <returns>String (Die Fehlermeldung steht darin)</returns>
  ''' <remarks></remarks>
  <WebMethod(Description:="Zur Sicherung eines Kandidaten. Dabei wird überprüft ob der Datensatz vorhanden ist, wenn JA, dann wird der Datensatz upgedatet und wenn nicht wird neu angelegt.")> _
Function SaveMyMA(ByVal strMA_Guid As String, _
                   ByVal iMANr As Integer, _
                   ByVal MyDs As DataSet, _
                   ByVal aCustomerData As String()) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim strUserID As String = aCustomerData(0)
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "MA_Guid")
    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)

    ' Datensatz löschen
    strValue = DeleteSelectedMAOnline(strUserID, strMA_Guid, iMANr)

    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "Insert Into Kandidaten_Online ("
    strSQL &= "MANr, Customer_ID, Customer_Name, "
    strSQL &= "Customer_Strasse, Customer_Ort, Customer_Telefon, Customer_eMail, "
    strSQL &= "Berater, MA_Filiale, MA_Kanton, MA_Ort, MA_Vorname, MA_Nachname, "
    strSQL &= "MA_Kontakt, MA_State1, MA_State2, MA_Beruf, "
    strSQL &= "JobProzent, MAGebDat, MASex, MAZivil, "
    strSQL &= "MAFSchein, MAAuto, MANationality, Bewillig, "
    strSQL &= "BriefAnrede, MA_Res1, MA_Res2, MA_Res3, MA_Res4, "
    strSQL &= "MA_MSprache, MA_SSprache, MA_Eigenschaft, "

    strSQL &= "Transfered_User, Transfered_On, Transfered_Guid, "
    strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail"
    strSQL &= ") Values ("

    strSQL &= "@MANr, @Customer_ID, @Customer_Name, "
    strSQL &= "@Customer_Strasse, @Customer_Ort, @Customer_Telefon, @Customer_eMail, "
    strSQL &= "@Berater, @MA_Filiale, @MA_Kanton, @MA_Ort, @MA_Vorname, @MA_Nachname, "
    strSQL &= "@MAKontakt, @MAState1, @MAState2, @MABeruf, "
    strSQL &= "@JobProzent, @MAGebDat, @MASex, @MAZivil, "
    strSQL &= "@MAFSchein, @MAAuto, @MANationality, @Bewillig, "
    strSQL &= "@BriefAnrede, @MA_Res1, @MA_Res2, @MA_Res3, @MA_Res4, "
    strSQL &= "@MA_MSprache, @MA_SSprache, @MA_Eigenschaft, "

    strSQL &= "@Transfered_User, @Transfered_On, @Transfered_Guid, "
    strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail"

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.Text
    Dim param As System.Data.SqlClient.SqlParameter
    ' Daten vom Client
    Dim dt As DataTable = MyDs.Tables("Kandidaten_Online")

    param = cmd.Parameters.AddWithValue("@MANr", iMANr) ' Val(GetDbValue(CStr(dt.Rows(0)("MANr").ToString))))
    param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
    param = cmd.Parameters.AddWithValue("@Customer_Name", aCustomerData(1))

    param = cmd.Parameters.AddWithValue("@Customer_Strasse", aCustomerData(2))
    param = cmd.Parameters.AddWithValue("@Customer_Ort", aCustomerData(3))
    param = cmd.Parameters.AddWithValue("@Customer_Telefon", aCustomerData(4))
    param = cmd.Parameters.AddWithValue("@Customer_eMail", aCustomerData(6))

    param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(0)("Berater").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Filiale", GetDbValue(dt.Rows(0)("MA_Filiale").ToString))

    param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(0)("MA_Kanton").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(0)("MA_Ort").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(0)("MA_Vorname").ToString))
    param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(0)("MA_Nachname").ToString))

    param = cmd.Parameters.AddWithValue("@MAKontakt", GetDbValue(dt.Rows(0)("MA_Kontakt").ToString))
    param = cmd.Parameters.AddWithValue("@MAState1", GetDbValue(dt.Rows(0)("MA_State1").ToString))
    param = cmd.Parameters.AddWithValue("@MAState2", GetDbValue(dt.Rows(0)("MA_State2").ToString))
    param = cmd.Parameters.AddWithValue("@MABeruf", GetDbValue(dt.Rows(0)("MA_Beruf").ToString))
    param = cmd.Parameters.AddWithValue("@JobProzent", GetDbValue(CStr(dt.Rows(0)("JobProzent").ToString)))
    param = cmd.Parameters.AddWithValue("@MAGebDat", GetDbValue(CStr(dt.Rows(0)("MA_GebDat").ToString)))
    param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(0)("MASex").ToString)))
    param = cmd.Parameters.AddWithValue("@MAZivil", GetDbValue(CStr(dt.Rows(0)("Zivilstand").ToString)))

    param = cmd.Parameters.AddWithValue("@MAFSchein", GetDbValue(CStr(dt.Rows(0)("MAFSchein").ToString)))
    param = cmd.Parameters.AddWithValue("@MAAuto", GetDbValue(CStr(dt.Rows(0)("MAAuto").ToString)))
    param = cmd.Parameters.AddWithValue("@MANationality", GetDbValue(CStr(dt.Rows(0)("MA_Nationality").ToString)))
    param = cmd.Parameters.AddWithValue("@Bewillig", GetDbValue(CStr(dt.Rows(0)("Bewillig").ToString)))
    param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(0)("BriefAnrede").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Res1", GetDbValue(CStr(dt.Rows(0)("MA_Res1").ToString)))

    param = cmd.Parameters.AddWithValue("@MA_Res2", GetDbValue(CStr(dt.Rows(0)("MA_Res2").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Res3", GetDbValue(CStr(dt.Rows(0)("MA_Res3").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Res4", GetDbValue(CStr(dt.Rows(0)("MA_Res4").ToString)))

    param = cmd.Parameters.AddWithValue("@MA_MSprache", GetDbValue(CStr(dt.Rows(0)("MA_MSprache").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_SSprache", GetDbValue(CStr(dt.Rows(0)("MA_SSprache").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Eigenschaft", GetDbValue(CStr(dt.Rows(0)("MA_Eigenschaft").ToString)))

    param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(0)("Transfered_User").ToString)))
    param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)
    param = cmd.Parameters.AddWithValue("@Transfered_Guid", GetDbValue(CStr(dt.Rows(0)("MA_Guid").ToString)))

    param = cmd.Parameters.AddWithValue("@User_Nachname", aCustomerData(7))
    param = cmd.Parameters.AddWithValue("@User_Vorname", aCustomerData(8))
    param = cmd.Parameters.AddWithValue("@User_Telefon", aCustomerData(9))
    param = cmd.Parameters.AddWithValue("@User_Telefax", aCustomerData(10))
    param = cmd.Parameters.AddWithValue("@User_eMail", aCustomerData(11))
    '    Return strValue

    Try
      cmd.ExecuteNonQuery()           ' Kandidat einfügen...
      strValue = "SaveMyMA: Ihre Daten wurden erfolgreich importiert..."

      Try
        strValue = RegisterToKandidatDb(strMA_Guid, MyDs, "Kandidaten_Online", aCustomerData(0))

      Catch ex As Exception
        strValue = String.Format("Fehler ({0}): {1}", "RegisterToKandidatDb_0", ex.Message)
      End Try


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "SaveMyMA", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Zur Speichernung der Daten eines Kandidaten.")> _
  Function RegisterToKandidatDb(ByVal strMA_Guid As String, _
                           ByVal MyDs As DataSet, _
                           ByVal strTableName As String, _
                           ByVal strUserID As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    ' Datensatz löschen
    strValue = DeleteSelectedKandidatRec(strUserID, strMA_Guid)

    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "Insert Into Kandidaten ("
    strSQL &= "Customer_ID, MANr, MA_Guid, "
    strSQL &= "Berater, MA_Vorname, MA_Nachname, MA_Kanton, MA_Ort, "
    strSQL &= "MA_Beruf, MA_Branche, MASex, MA_EMail, MA_GebDat, MA_Language, "
    strSQL &= "MA_Nationality, BriefAnrede, AGB_WOS, "

    strSQL &= "Transfered_User, Transfered_On "
    'strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail"
    strSQL &= ") Values ("

    strSQL &= "@Customer_ID, @MANr, @MA_Guid, "
    strSQL &= "@Berater, @MA_Vorname, @MA_Nachname, @MA_Kanton, @MA_Ort, "
    strSQL &= "@MA_Beruf, @MA_Branche, @MASex, @MA_EMail, @MA_GebDat, @MA_Language, "
    strSQL &= "@MA_Nationality, @BriefAnrede, @AGB_WOS, "

    strSQL &= "@Transfered_User, @Transfered_On "
    'strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail"

    strSQL &= ")"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
    cmd.CommandType = Data.CommandType.Text
    Dim param As System.Data.SqlClient.SqlParameter
    ' Daten vom Client
    Dim dt As DataTable = MyDs.Tables(strTableName)

    param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
    param = cmd.Parameters.AddWithValue("@MANr", GetDbValue(CStr(dt.Rows(0)("MANr").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)

    param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(0)("Berater").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(0)("MA_Vorname").ToString))
    param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(0)("MA_Nachname").ToString))
    param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(0)("MA_Kanton").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(0)("MA_Ort").ToString)))

    param = cmd.Parameters.AddWithValue("@MA_Beruf", GetDbValue(dt.Rows(0)("MA_Beruf").ToString))
    param = cmd.Parameters.AddWithValue("@MA_Branche", GetDbValue(dt.Rows(0)("MA_Branche").ToString))
    param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(0)("MASex").ToString)))

    param = cmd.Parameters.AddWithValue("@MA_EMail", GetDbValue(dt.Rows(0)("MA_EMail").ToString))
    param = cmd.Parameters.AddWithValue("@MA_GebDat", GetDbValue(CStr(dt.Rows(0)("MA_GebDat").ToString)))
    param = cmd.Parameters.AddWithValue("@MA_Language", GetDbValue(dt.Rows(0)("MA_Language").ToString))

    param = cmd.Parameters.AddWithValue("@MA_Nationality", GetDbValue(CStr(dt.Rows(0)("MA_Nationality").ToString)))
    param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(0)("BriefAnrede").ToString)))
    param = cmd.Parameters.AddWithValue("@AGB_WOS", GetDbValue(CStr(dt.Rows(0)("AGB_WOS").ToString)))

    param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(0)("Transfered_User").ToString)))
    param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)


    Try
      cmd.ExecuteNonQuery()           ' Kandidat einfügen...
      strValue = "RegisterToKandidatDb: Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "RegisterToKandidatDb", ex.Message)

    End Try

    Return strValue
  End Function

  ''' <summary>
  ''' Löscht den Selectierten Datensatz von Kandidaten
  ''' </summary>
  ''' <param name="strUserID">USerID kommt von Client</param>
  ''' <param name="strMA_Guid">GuidNr kommt auch von Client</param>
  ''' <param name="iMANr">MANr kommt von Client</param>
  ''' <remarks></remarks>
  <WebMethod(Description:="Löscht den ausgewählten Online Kandidaten (Kandidaten_Online).")> _
  Function DeleteSelectedMAOnline(ByVal strUserID As String, ByVal strMA_Guid As String, ByVal iMANr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedMAOnline"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MA-Rec]"

    Try
      Dim Conn As SqlConnection = New SqlConnection(connString)
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@Transfered_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      Dim rMArec As SqlDataReader = cmd.ExecuteReader

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedMAOnline", ex.Message)

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht den ausgewählten Kandidaten (Kandidaten).")> _
  Function DeleteSelectedKandidatRec(ByVal strUserID As String, ByVal strMA_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedKandidatRec"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MA-Rec From Kandidaten]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@Transfered_Guid", strMA_Guid)

      Dim rMArec As SqlDataReader = cmd.ExecuteReader

    Catch ex As Exception
      strValue = String.Format("Fehler ({0}): {1}", "DeleteSelectedKandidatRec", ex.Message)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Ändert das ALTE MA_Guid durch NEUEN MA_Guid in allen Tabellen.")> _
Function ChangeMAGuidWithNewGuid(ByVal strUserID As String, _
                                 ByVal strOldMA_Guid As String, _
                                 ByVal strNewMA_Guid As String) As String
    Dim strValue As String = "Erfolgreich: ChangeMAGuidWithNewGuid"
    Dim connString As String = My.Settings.ConnStr_KD
    Dim strSQL As String = "[Change Selected MAGuid With NewGuid]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()
    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@OldMA_Guid", strOldMA_Guid)
      param = cmd.Parameters.AddWithValue("@NewMA_Guid", strNewMA_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz ändern...


    Catch ex As Exception
      strValue = String.Format("Fehler_Main_ChangeMAGuidWithNewGuid: {0}", ex.ToString)

    Finally
      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

#End Region


#Region "Funktionen für Dokumente der Kandidaten..."

  ''' <summary>
  ''' Diese Funktion dient zur Speicherung der Kandidaten-Dokumete in der Datenbank (Kandidaten_Doc_Online. 
  ''' Vor der Sicherung löscht er den Datensatz anhand Transfered_Guid und MANr.
  ''' </summary>
  ''' <param name="strMA_Guid"> Gespeicherte Guid von Kandidaten</param>
  ''' <param name="iMANr">Kandidatennummer</param>
  ''' <param name="MyDs">Kompletten Datensatz mit Kandidatendaten</param>
  ''' <param name="aCustomerData">Ein Array mit allen nötigen Customer wie auch User-Daten und FileName</param>
  ''' <param name="objFile">Die Datei in Binär-Format</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  <WebMethod(Description:="Zur Sicherung eines Kandidatendokuments.")> _
 Function SaveMyDocToDb(ByVal strMA_Guid As String, _
                        ByVal strDoc_Guid As String, _
                        ByVal iMANr As Integer, _
                        ByVal iESNr As Integer, _
                        ByVal iESLohnNr As Integer, _
                        ByVal iLONr As Integer, _
                        ByVal iRPNr As Integer, _
                        ByVal iRPLNr As Integer, _
                        ByVal iRPDocNr As Integer, _
                        ByVal MyDs As DataSet, _
                        ByVal aCustomerData As String(), _
                        ByVal objFile As Byte()) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim strUserID As String = aCustomerData(0)
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "MA_Guid")
    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)
		Dim connString As String = My.Settings.ConnStr_MA

		Try
      If iESNr + iESLohnNr + iLONr + iRPDocNr = 0 Then
        ' ist sonstiges Dokument wie Lohnausweis, ZV, Arbeitgeberbescheinigung
        strValue = DeleteSelectedMADocWithArt(aCustomerData(0), strMA_Guid, iMANr, aCustomerData(14), aCustomerData(15))

      Else
        strValue = DeleteSelectedMADocWithGuid(aCustomerData(0), strMA_Guid, iMANr, strDoc_Guid)

      End If
      If strValue.ToLower.Contains("Fehler".ToLower) Then
				Return String.Format("Löschen von Dokumenten: {0} {1}",
							If(iESNr + iESLohnNr + iLONr = 0, "DeleteSelectedMADocWithArt",
								 "DeleteSelectedMADocWithGuid"),
								 strValue)

			End If


    Catch ex As Exception
			Return String.Format("Löschen von Dokumenten: {0} {1}",
										If(iESNr + iESLohnNr + iLONr = 0, "DeleteSelectedMADocWithArt",
											 "DeleteSelectedMADocWithGuid"),
											 ex.Message)
		End Try

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()
    Try

      Dim strSQL As String = "Insert Into Kandidaten_Doc_Online ("
      strSQL &= "MANr, ESNr, ESLohnNr, LONr, RPNr, RPLNr, RPDocNr, Customer_ID, Customer_Name, "
      strSQL &= "Customer_Strasse, Customer_Ort, Customer_Telefon, Customer_eMail, "
      strSQL &= "Berater, MA_Filiale, MA_Kanton, MA_Ort, MA_Vorname, MA_Nachname, "
      strSQL &= "MASex, BriefAnrede, "

      strSQL &= "Transfered_User, Transfered_On, Owner_Guid, Doc_Guid, "
      strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail, "
      strSQL &= "Doc_Art, Doc_Info, LogedUser_ID, DocFilename, DocScan"
      strSQL &= ") Values ("

      strSQL &= "@MANr, @ESNr, @ESLohnNr, @LONr, @RPNr, @RPLNr, @RPDocNr, @Customer_ID, @Customer_Name, "
      strSQL &= "@Customer_Strasse, @Customer_Ort, @Customer_Telefon, @Customer_eMail, "
      strSQL &= "@Berater, @MA_Filiale, @MA_Kanton, @MA_Ort, @MA_Vorname, @MA_Nachname, "
      strSQL &= "@MASex, @BriefAnrede, "
      strSQL &= "@Transfered_User, @Transfered_On, @Owner_Guid, @Doc_Guid, "
      strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail, "
      strSQL &= "@Doc_Art, @Doc_Info, @LogedUser_ID, @DocFilename, @DocScan"

      strSQL &= ") "



      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter

      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables("Kandidaten_Doc_Online")

      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
      param = cmd.Parameters.AddWithValue("@ESLohnNr", iESLohnNr)
      param = cmd.Parameters.AddWithValue("@LONr", iLONr)

      param = cmd.Parameters.AddWithValue("@RPNr", iRPNr)
      param = cmd.Parameters.AddWithValue("@RPLNr", iRPLNr)
      param = cmd.Parameters.AddWithValue("@RPDocNr", iRPDocNr)

      param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
      param = cmd.Parameters.AddWithValue("@Customer_Name", aCustomerData(1))

      param = cmd.Parameters.AddWithValue("@Customer_Strasse", aCustomerData(2))
      param = cmd.Parameters.AddWithValue("@Customer_Ort", aCustomerData(3))
      param = cmd.Parameters.AddWithValue("@Customer_Telefon", aCustomerData(4))
      param = cmd.Parameters.AddWithValue("@Customer_eMail", aCustomerData(6))

      param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(0)("Berater").ToString)))
      param = cmd.Parameters.AddWithValue("@MA_Filiale", GetDbValue(dt.Rows(0)("MA_Filiale").ToString))

      param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(0)("MA_Kanton").ToString)))
      param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(0)("MA_Ort").ToString)))
      param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(0)("Vorname").ToString))
      param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(0)("Nachname").ToString))

      param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(0)("Geschlecht").ToString)))
      param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(0)("BriefAnrede").ToString)))

      param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(0)("Transfered_User").ToString)))
      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)
      param = cmd.Parameters.AddWithValue("@Owner_Guid", GetDbValue(CStr(dt.Rows(0)("MA_Guid").ToString)))
      param = cmd.Parameters.AddWithValue("@Doc_Guid", strDoc_Guid)

      param = cmd.Parameters.AddWithValue("@User_Nachname", aCustomerData(7))
      param = cmd.Parameters.AddWithValue("@User_Vorname", aCustomerData(8))
      param = cmd.Parameters.AddWithValue("@User_Telefon", aCustomerData(9))
      param = cmd.Parameters.AddWithValue("@User_Telefax", aCustomerData(10))
      param = cmd.Parameters.AddWithValue("@User_eMail", aCustomerData(11))

      param = cmd.Parameters.AddWithValue("@Doc_Art", aCustomerData(14))
      param = cmd.Parameters.AddWithValue("@Doc_Info", aCustomerData(15))
      param = cmd.Parameters.AddWithValue("@LogedUser_ID", aCustomerData(16))

      param = cmd.Parameters.AddWithValue("@DocFilename", aCustomerData(13))
      param = cmd.Parameters.AddWithValue("@DocScan", objFile)

      cmd.ExecuteNonQuery()           ' Datensatz einfügen...
      cmd.Parameters.Clear()

      strValue = "Ihre Daten wurden erfolgreich importiert..."
      strValue = RegisterToKandidatDb(strMA_Guid, MyDs, "Kandidaten_Doc_Online", aCustomerData(0))


    Catch ex As Exception
      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

      Return String.Format("Fehler_Main: {0}", ex.ToString)

    End Try

    Return strValue
  End Function

  ''' <summary>
  ''' Diese Funktion dient zur Speicherung der Kandidaten-Dokumete in der Datenbank (Kandidaten_Doc_Online. 
  ''' Die Datensätze werden in Bündel vom Client kommen. Das sollte eine verbesserte Leistung bewirken!
  ''' Vor der Sicherung löscht er den Datensatz anhand Transfered_Guid und MANr.
  ''' </summary>
  ''' <param name="MyDs">Kompletten Datensatz mit Kandidatendaten</param>
  ''' <param name="strUserID">Ein Array mit allen nötigen Customer wie auch User-Daten und FileName</param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  <WebMethod(Description:="Zur Sicherung eines Kandidatendokuments aus der lokalen Datenbank.")> _
 Function SaveMyDocToDbFromLocalDB(ByVal MyDs As DataSet, ByVal strUserID As String) As String
    Dim strValue As String = "Initializing..."
    Dim _clsSystem As New ClsMain_Net
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "MA_Guid")
    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)
    Dim connString As String = My.Settings.ConnStr_MA
    'Return connString

    Dim iMANr As Integer
    Dim iESNr As Integer
    Dim iESLohnNr As Integer
    Dim iLONr As Integer
    Dim iRPNr As Integer
    Dim iRPLNr As Integer
    Dim iRPDocNr As Integer

    Dim strMA_Guid As String = String.Empty
    Dim strDoc_Guid As String = String.Empty
    Dim strMADoc_Info As String = String.Empty
    Dim strMADoc_Art As String = String.Empty

    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)

    Try
      Dim dt As DataTable = MyDs.Tables("Kandidaten_Doc_Online")

      For i As Integer = 0 To dt.Rows.Count - 1
        iMANr = CInt(dt.Rows(i)("MANr"))
        iESNr = CInt(dt.Rows(i)("ESNr"))
        iESLohnNr = CInt(dt.Rows(i)("ESLohnNr"))
        iLONr = CInt(dt.Rows(i)("LONr"))

        iRPNr = CInt(dt.Rows(i)("RPNr"))
        iRPLNr = CInt(dt.Rows(i)("RPLNr"))
        iRPDocNr = CInt(dt.Rows(i)("RPDocNr"))

        strMA_Guid = CStr(dt.Rows(i)("Owner_Guid"))
        strDoc_Guid = CStr(dt.Rows(i)("Doc_Guid"))
        strMADoc_Info = CStr(dt.Rows(i)("Doc_Info"))
        strMADoc_Art = CStr(dt.Rows(i)("Doc_Art"))

        If iESNr + iESLohnNr + iLONr + iRPDocNr = 0 Then
          ' ist sonstiges Dokument wie Lohnausweis, ZV, Arbeitgeberbescheinigung
          strValue = DeleteSelectedMADocWithArt(strUserID, strMA_Guid, iMANr, strMADoc_Art, strMADoc_Info)

        Else
          strValue = DeleteSelectedMADocWithGuid(strUserID, strMA_Guid, iMANr, strDoc_Guid)

        End If
        If strValue.ToLower.Contains("Fehler".ToLower) Then
          Return String.Format("Fehler beim Löschen von Dokumenten: {0} {1}", _
                If(iESNr + iESLohnNr + iLONr = 0, "DeleteSelectedMADocWithArt", _
                   "DeleteSelectedMADocWithGuid"), _
                   strValue)

        End If

        Dim Conn As SqlConnection = New SqlConnection(connString)
        Conn.Open()
        Try

          Dim strSQL As String = "Insert Into Kandidaten_Doc_Online ("
          strSQL &= "MANr, ESNr, ESLohnNr, LONr, RPNr, RPLNr, RPDocNr, Customer_ID, Customer_Name, "
          strSQL &= "Customer_Strasse, Customer_Ort, Customer_Telefon, Customer_eMail, "
          strSQL &= "Berater, MA_Filiale, MA_Kanton, MA_Ort, MA_Vorname, MA_Nachname, "
          strSQL &= "MASex, BriefAnrede, "

          strSQL &= "Transfered_User, Transfered_On, Owner_Guid, Doc_Guid, "
          strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail, "
          strSQL &= "Doc_Art, Doc_Info, LogedUser_ID, DocFilename, DocScan"
          strSQL &= ") Values ("

          strSQL &= "@MANr, @ESNr, @ESLohnNr, @LONr, @RPNr, @RPLNr, @RPDocNr, @Customer_ID, @Customer_Name, "
          strSQL &= "@Customer_Strasse, @Customer_Ort, @Customer_Telefon, @Customer_eMail, "
          strSQL &= "@Berater, @MA_Filiale, @MA_Kanton, @MA_Ort, @MA_Vorname, @MA_Nachname, "
          strSQL &= "@MASex, @BriefAnrede, "
          strSQL &= "@Transfered_User, @Transfered_On, @Owner_Guid, @Doc_Guid, "
          strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail, "
          strSQL &= "@Doc_Art, @Doc_Info, @LogedUser_ID, @DocFilename, @DocScan"

          strSQL &= ")"

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

          param = cmd.Parameters.AddWithValue("@MANr", GetDbValue(dt.Rows(i)("MANr").ToString))
          param = cmd.Parameters.AddWithValue("@ESNr", GetDbValue(dt.Rows(i)("ESNr").ToString))
          param = cmd.Parameters.AddWithValue("@ESLohnNr", GetDbValue(dt.Rows(i)("ESLohnNr").ToString))
          param = cmd.Parameters.AddWithValue("@LONr", GetDbValue(dt.Rows(i)("LONr").ToString))

          param = cmd.Parameters.AddWithValue("@RPNr", GetDbValue(dt.Rows(i)("RPNr").ToString))
          param = cmd.Parameters.AddWithValue("@RPLNr", GetDbValue(dt.Rows(i)("RPLNr").ToString))
          param = cmd.Parameters.AddWithValue("@RPDocNr", GetDbValue(dt.Rows(i)("RPDocNr").ToString))

          param = cmd.Parameters.AddWithValue("@Customer_ID", GetDbValue(dt.Rows(i)("Customer_ID").ToString))
          param = cmd.Parameters.AddWithValue("@Customer_Name", GetDbValue(dt.Rows(i)("_MDName").ToString))

          param = cmd.Parameters.AddWithValue("@Customer_Strasse", GetDbValue(dt.Rows(i)("_MDStrasse").ToString))
          param = cmd.Parameters.AddWithValue("@Customer_Ort", GetDbValue(dt.Rows(i)("_MDOrt").ToString))
          param = cmd.Parameters.AddWithValue("@Customer_Telefon", GetDbValue(dt.Rows(i)("_MDTelefon").ToString))
          param = cmd.Parameters.AddWithValue("@Customer_eMail", GetDbValue(dt.Rows(i)("_MDeMail").ToString))

          param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(i)("Berater").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Filiale", GetDbValue(dt.Rows(i)("MA_Filiale").ToString))

          param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(i)("MA_Kanton").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(i)("MA_Ort").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(i)("MA_Vorname").ToString))
          param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(i)("MA_Nachname").ToString))

          param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(i)("MASex").ToString)))
          param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(i)("BriefAnrede").ToString)))

          param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(i)("Transfered_User").ToString)))
          param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)
          param = cmd.Parameters.AddWithValue("@Owner_Guid", GetDbValue(CStr(dt.Rows(i)("Owner_Guid").ToString)))
          param = cmd.Parameters.AddWithValue("@Doc_Guid", GetDbValue(dt.Rows(i)("Doc_Guid").ToString))

          param = cmd.Parameters.AddWithValue("@User_Nachname", GetDbValue(dt.Rows(i)("User_Nachname").ToString))
          param = cmd.Parameters.AddWithValue("@User_Vorname", GetDbValue(dt.Rows(i)("User_Vorname").ToString))
					param = cmd.Parameters.AddWithValue("@User_Telefon", dt.Rows(i)("_MDTelefon"))
					param = cmd.Parameters.AddWithValue("@User_Telefax", dt.Rows(i)("_MDTelefax"))
					param = cmd.Parameters.AddWithValue("@User_eMail", dt.Rows(i)("_MDeMail"))

					param = cmd.Parameters.AddWithValue("@Doc_Art", GetDbValue(dt.Rows(i)("Doc_Art").ToString))
          param = cmd.Parameters.AddWithValue("@Doc_Info", GetDbValue(dt.Rows(i)("Doc_Info").ToString))
          param = cmd.Parameters.AddWithValue("@LogedUser_ID", GetDbValue(dt.Rows(i)("LogedUser_ID").ToString))

          param = cmd.Parameters.AddWithValue("@DocFilename", GetDbValue(dt.Rows(i)("DocFilename").ToString))
          param = cmd.Parameters.AddWithValue("@DocScan", dt.Rows(i)("DocScan"))

          ' User-Daten anlegen...
          param = cmd.Parameters.AddWithValue("@User_Initial", dt.Rows(i)("_USInitial"))
          param = cmd.Parameters.AddWithValue("@User_Sex", dt.Rows(i)("_USAnrede"))
          param = cmd.Parameters.AddWithValue("@User_Homepage", dt.Rows(i)("_MDHomepage"))
          param = cmd.Parameters.AddWithValue("@User_Filiale", GetDbValue(dt.Rows(i)("USFiliale").ToString))
          param = cmd.Parameters.AddWithValue("@User_Picture", dt.Rows(i)("USBild"))
          param = cmd.Parameters.AddWithValue("@User_Sign", dt.Rows(i)("USSign"))
          param = cmd.Parameters.AddWithValue("@CreatedOn", Now.ToString)

          cmd.ExecuteNonQuery()           ' Datensatz einfügen...
          cmd.Parameters.Clear()
          strValue = "Ihre Daten wurden erfolgreich importiert..."


        Catch ex As Exception

          If Not Conn Is Nothing Then
            Conn.Close()
            Conn.Dispose()
          End If

          Return String.Format("Fehler_1: {0}", ex.ToString)
        End Try

      Next i
      strValue = RegisterToKandidatDb_0(MyDs, "Kandidaten_Doc_Online", strUserID)


    Catch ex As Exception
      Return String.Format("Fehler_0: {0}", ex.Message)

    End Try

    Return strValue
  End Function

  Function RegisterToKandidatDb_0(ByVal MyDs As DataSet, _
                         ByVal strTableName As String, _
                         ByVal strUserID As String) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net

    Try

      Dim iMANr As Integer
      Dim iESNr As Integer
      Dim iESLohnNr As Integer
      Dim iLONr As Integer
      Dim iRPNr As Integer
      Dim iRPLNr As Integer
      Dim iRPDocNr As Integer

      Dim strMA_Guid As String = String.Empty
      Dim strDoc_Guid As String = String.Empty
      Dim strMADoc_Info As String = String.Empty
      Dim strMADoc_Art As String = String.Empty
      Dim connString As String = My.Settings.ConnStr_MA

      Dim dt As DataTable = MyDs.Tables("Kandidaten_Doc_Online")

      For i As Integer = 0 To dt.Rows.Count - 1

        iMANr = CInt(dt.Rows(i)("MANr"))
        iESNr = CInt(dt.Rows(i)("ESNr"))
        iESLohnNr = CInt(dt.Rows(i)("ESLohnNr"))
        iLONr = CInt(dt.Rows(i)("LONr"))

        iRPNr = CInt(dt.Rows(i)("RPNr"))
        iRPLNr = CInt(dt.Rows(i)("RPLNr"))
        iRPDocNr = CInt(dt.Rows(i)("RPDocNr"))

        strMA_Guid = CStr(dt.Rows(i)("Owner_Guid"))
        strDoc_Guid = CStr(dt.Rows(i)("Doc_Guid"))
        strMADoc_Info = CStr(dt.Rows(i)("Doc_Info"))
        strMADoc_Art = CStr(dt.Rows(i)("Doc_Art"))

        strValue = DeleteSelectedKandidatRec(strUserID, strMA_Guid)
        If strValue.ToLower.Contains("Fehler".ToLower) Then
          Return String.Format("Fehler beim Löschen von Dokumenten: {0} {1}", _
                If(iESNr + iESLohnNr + iLONr = 0, "DeleteSelectedMADocWithArt", _
                   "DeleteSelectedMADocWithGuid"), _
                   strValue)

        End If

        Try

          Dim strSQL As String = "Insert Into Kandidaten ("
          strSQL &= "Customer_ID, MANr, MA_Guid, "
          strSQL &= "Berater, MA_Vorname, MA_Nachname, MA_Kanton, MA_Ort, "
          strSQL &= "MA_Beruf, MA_Branche, MASex, MA_EMail, MA_GebDat, MA_Language, "
          strSQL &= "MA_Nationality, BriefAnrede, AGB_WOS, "

          strSQL &= "Transfered_User, Transfered_On "
          strSQL &= ") Values ("

          strSQL &= "@Customer_ID, @MANr, @MA_Guid, "
          strSQL &= "@Berater, @MA_Vorname, @MA_Nachname, @MA_Kanton, @MA_Ort, "
          strSQL &= "@MA_Beruf, @MA_Branche, @MASex, @MA_EMail, @MA_GebDat, @MA_Language, "
          strSQL &= "@MA_Nationality, @BriefAnrede, @AGB_WOS, "

          strSQL &= "@Transfered_User, @Transfered_On "

          strSQL &= ")"

          Dim Conn As SqlConnection = New SqlConnection(connString)
          Conn.Open()

          Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
          cmd.CommandType = Data.CommandType.Text
          Dim param As System.Data.SqlClient.SqlParameter


          param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
          param = cmd.Parameters.AddWithValue("@MANr", GetDbValue(CStr(dt.Rows(i)("MANr").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
          param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(i)("Berater").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Vorname", GetDbValue(dt.Rows(i)("MA_Vorname").ToString))
          param = cmd.Parameters.AddWithValue("@MA_Nachname", GetDbValue(dt.Rows(i)("MA_Nachname").ToString))
          param = cmd.Parameters.AddWithValue("@MA_Kanton", GetDbValue(CStr(dt.Rows(i)("MA_Kanton").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Ort", GetDbValue(CStr(dt.Rows(i)("MA_Ort").ToString)))

          param = cmd.Parameters.AddWithValue("@MA_Beruf", GetDbValue(dt.Rows(i)("MA_Beruf").ToString))
          param = cmd.Parameters.AddWithValue("@MA_Branche", GetDbValue(dt.Rows(i)("MA_Branche").ToString))
          param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(i)("MASex").ToString)))

          param = cmd.Parameters.AddWithValue("@MA_EMail", GetDbValue(dt.Rows(i)("MA_EMail").ToString))
          param = cmd.Parameters.AddWithValue("@MA_GebDat", GetDbValue(CStr(dt.Rows(i)("MA_GebDat").ToString)))
          param = cmd.Parameters.AddWithValue("@MA_Language", GetDbValue(dt.Rows(i)("MA_Language").ToString))

          param = cmd.Parameters.AddWithValue("@MA_Nationality", GetDbValue(CStr(dt.Rows(i)("MA_Nationality").ToString)))
          param = cmd.Parameters.AddWithValue("@BriefAnrede", GetDbValue(CStr(dt.Rows(i)("BriefAnrede").ToString)))
          param = cmd.Parameters.AddWithValue("@AGB_WOS", GetDbValue(CStr(dt.Rows(i)("AGB_WOS").ToString)))

          param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(i)("Transfered_User").ToString)))
          param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)

          cmd.ExecuteNonQuery()           ' Datensatz einfügen...
          cmd.Parameters.Clear()
          strValue = "Ihre Daten wurden erfolgreich importiert..."


        Catch ex As Exception
          Return String.Format("Fehler_0: {0}", ex.Message)

        End Try

      Next


    Catch ex As Exception
      Return String.Format("Fehler_0: {0}", ex.Message)

    End Try



    Return strValue
  End Function






  ''' <summary>
  ''' Löscht den Selectierten Datensatz von Kandidaten-Document
  ''' </summary>
  ''' <param name="strUserID">Customer-Guid kommt von Client</param>
  ''' <param name="strMA_Guid">Kandidaten-Guid kommt auch von Client</param>
  ''' <param name="iMANr">MANr kommt von Client</param>
  ''' <param name="strMADoc_Guid">Doc-Guid kommt von Client</param>
  ''' <remarks></remarks>
  <WebMethod(Description:="Löscht das ausgewählte Kandidaten-Document.")> _
  Function DeleteSelectedMADocWithGuid(ByVal strUserID As String, _
                                ByVal strMA_Guid As String, _
                                ByVal iMANr As Integer, _
                                ByVal strMADoc_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedMADocWithGuid"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MADoc With DocGuid]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@Doc_Guid", strMADoc_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedMADocWithGuid: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  ''' <summary>
  ''' Löscht den Selectierten Datensatz von Kandidaten-Document
  ''' </summary>
  ''' <param name="strUserID">Customer-Guid kommt von Client</param>
  ''' <param name="strMA_Guid">Kandidaten-Guid kommt auch von Client</param>
  ''' <param name="iMANr">MANr kommt von Client</param>
  ''' <param name="strMADoc_Info">Doc-Art kommt von Client</param>
  ''' <remarks></remarks>
  <WebMethod(Description:="Löscht das ausgewählte Kandidaten-Document.")> _
  Function DeleteSelectedMADocWithArt(ByVal strUserID As String, _
                                ByVal strMA_Guid As String, _
                                ByVal iMANr As Integer, _
                                ByVal strMADoc_Art As String, _
                                ByVal strMADoc_Info As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedMADocWithArt"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MADoc With DocInfo]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@Doc_Art", strMADoc_Art)
      param = cmd.Parameters.AddWithValue("@Doc_Info", strMADoc_Info)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedMADocWithArt: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (Einsatz).")> _
  Function DeleteSelectedMAESDoc(ByVal strUserID As String, _
                                ByVal strMA_Guid As String, _
                                ByVal iMANr As Integer, _
                                ByVal iESNr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedMAESDoc"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MAESDoc]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@ESNr", iESNr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedMAESDoc: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (Lohn).")> _
  Function DeleteSelectedMALODoc(ByVal strUserID As String, _
                                ByVal strMA_Guid As String, _
                                ByVal iMANr As Integer, _
                                ByVal iLONr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedMALODoc"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected MALODoc]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      param = cmd.Parameters.AddWithValue("@LONr", iLONr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedMALODoc: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Modul-Document (Einsatz).")> _
Function DeleteSelectedAllMADoc(ByVal strUserID As String, _
                              ByVal strMA_Guid As String, _
                              ByVal iMANr As Integer) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedAllMADoc"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected AllMADoc]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@MA_Guid", strMA_Guid)
      param = cmd.Parameters.AddWithValue("@MANr", iMANr)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedAllMADoc: {0}", ex.ToString)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function

  <WebMethod(Description:="Löscht das ausgewählte Document anhand GUID (Einsatz).")> _
Function DeleteSelectedDocWithGuid(ByVal strUserID As String, _
                                   ByVal strDoc_Guid As String) As String
    Dim strValue As String = "Erfolgreich: DeleteSelectedDocWithGuid"
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Delete Selected Doc With Guid]"
    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@Doc_Guid", strDoc_Guid)
      cmd.ExecuteNonQuery()           ' Datensatz löschen...


    Catch ex As Exception
      Return String.Format("Fehler_Main_DeleteSelectedDocWithGuid: {0}", ex.ToString)
    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function


  <WebMethod(Description:="Zur Auflistung der Kandidaten (Gesamte Datensätze)")> _
Function GetMADoc_RecToShowOnClient(ByVal strUserID As String, ByVal strMANr As String, _
                    ByVal strDocArt As String, ByVal strTransferedOn As String, _
                    ByVal strOperator As String, ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strSQL As String = "[Get MADocrec2ShowOnClient]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rMArec As New DataSet

    Try

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
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@MANr", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "MANr", _
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
      adapter.SelectCommand.Parameters(1).Value = CType(strMANr.Trim, String)
      adapter.SelectCommand.Parameters(2).Value = CType(strDocArt.Trim, String)
      adapter.SelectCommand.Parameters(3).Value = CType(strTransferedOn.Trim, String)
      adapter.SelectCommand.Parameters(4).Value = CType(If(strOperator.Trim = String.Empty, _
                                                           ">=", strOperator.Trim), String)
      adapter.SelectCommand.Parameters(5).Value = CType(If(strSortkeys.Trim = String.Empty, _
                                                           "MADoc.Transfered_On DESC", strSortkeys.Trim), String)
      Try
        ' Die Datenbank anders nennen!!!
        ' Return rMArec
        adapter.Fill(rMArec, "Kandidaten_Doc_Online")

        strRecResult = String.Format(If(rMArec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                        "Keine Daten wurden gefunden."), rMArec.Tables(0).Rows.Count)


      Catch ex As Exception
        strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)
        WriteConnectionHistory(strUserID, strMANr, strDocArt, strTransferedOn, "", strSortkeys, strRecResult)

      End Try

    Catch ex As Exception

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rMArec
  End Function

  <WebMethod(Description:="Zur Übergabe eines Dokuments dem Client (in Byte())")> _
  Function SendMyDocToClient(ByVal iMANr As Integer, _
                             ByVal iKDNr As Integer, _
                             ByVal strOwner_Guid As String, _
                             ByVal strDoc_Guid As String, _
                             ByVal strUserID As String) As Byte()
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim bMyDoc As Byte() = Nothing
    Dim strSelectedFile As String = String.Empty
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, If(iMANr > 0, "MA_Guid", "KD_Guid"))
    If strUserID <> strAllowedToSave Then Return bMyDoc '"Not Allowed: strSelectedFile"
    Dim connString As String = My.Settings.ConnStr_MA

    Try

      Dim strSQL As String = "Select Top 1 DocScan, DocFileName From {0} Where "
      strSQL &= "{1}Nr = @MANr And Customer_ID = @Customer_ID "
      strSQL &= "And {2}_Guid = @Owner_Guid And Doc_Guid = @Doc_Guid"
      If iMANr > 0 Then
        strSQL = String.Format(strSQL, "Kandidaten_Doc_Online", "MA", "Owner")

      Else
        strSQL = String.Format(strSQL, "Kunden_Doc_Online", "KD", "KD")

      End If

      Dim Conn As SqlConnection = New SqlConnection(connString)
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@MANr", If(iMANr > 0, iMANr, iKDNr))
      param = cmd.Parameters.AddWithValue("@Customer_ID", strUserID)
      param = cmd.Parameters.AddWithValue("@Owner_Guid", strOwner_Guid)
      param = cmd.Parameters.AddWithValue("@Doc_Guid", strDoc_Guid)

      Dim rFoundedDoc As SqlDataReader = cmd.ExecuteReader
      Try
        While rFoundedDoc.Read()
          bMyDoc = CType(rFoundedDoc("DocScan"), Byte())

        End While


      Catch ex As Exception
        strSelectedFile = String.Format("Fehler_0: {0}", ex.Message)

      Finally

        If Not Conn Is Nothing Then
          Conn.Close()
          Conn.Dispose()
        End If

      End Try
      strValue = "Ihre Daten wurden erfolgreich importiert..."


    Catch ex As Exception
      strSelectedFile = String.Format("Fehler_1: {0}", ex.Message)

    End Try

    Return bMyDoc ' strSelectedFile
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

  Sub WriteConnectionHistory(ByVal strUserID As String, ByVal strBeruf As String, _
                    ByVal strOrt As String, ByVal strKanton As String, _
                    ByVal strFiliale As String, _
                    ByVal strSortkeys As String, ByVal strRecResult As String)
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_MA
    Dim strQuery As String = "Insert Into Tab_ModulUsage (ModulName, UseCount, UseDate, MachineID, "
    strQuery &= "ModulParameter, IsWebService) Values ("
    strQuery &= "@ModulName, @UseCount, @UseDate, @MachineID, @ModulParameter, @IsWebService)"

    Try

      Dim Conn As SqlConnection = New SqlConnection(connString)
      Conn.Open()

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

      Finally

        If Not Conn Is Nothing Then
          Conn.Close()
          Conn.Dispose()
        End If

      End Try

    Catch ex As Exception

    End Try

  End Sub

#End Region


End Class