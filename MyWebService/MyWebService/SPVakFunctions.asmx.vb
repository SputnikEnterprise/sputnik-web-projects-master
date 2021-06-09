
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPVakFunctions.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPVakFunctions
  Inherits System.Web.Services.WebService

  ''' <summary>
  ''' Diese Funktion dient zur Speicherung der Vankanzen in der Hauptdatenbank (SPContract). 
  ''' Vor der Sicherung löscht er den Datensatz anhand Transfered_Guid und VakNr.
  ''' </summary>
  ''' <param name="strUserID"></param>
  ''' <param name="strVak_Guid"></param>
  ''' <param name="iVakNr"></param>
  ''' <param name="MyDs"></param>
  ''' <returns>String (Die Fehlermeldung steht darin)</returns>
  ''' <remarks></remarks>
  <WebMethod(Description:="Zur Sicherung eines Vakanzen. Dabei wird überprüft ob der Datensatz vorhanden ist, wenn JA, dann wird der Datensatz upgedatet und wenn nicht wird neu angelegt.")> _
Function SaveMyVak(ByVal strUserID As String, _
                   ByVal strVak_Guid As String, _
                   ByVal iVakNr As Integer, _
                   ByVal MyDs As DataSet, _
                   ByVal aCustomerData As String()) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "Vak_Guid")
    If strUserID <> strAllowedToSave Then Return String.Format("{0} {1}", strUserID, strAllowedToSave)
    Dim connString As String = My.Settings.ConnStr_Vak

    Try
      ' Datensatz löschen
      DeleteSelectedVak(strUserID, strVak_Guid, iVakNr)

    Catch ex As Exception
      Return String.Format("Fehler (Daten löschen): {0} {1}", strUserID, ex.Message)

    End Try

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim strSQL As String = "Insert Into KD_Vakanzen ("
      strSQL &= "VakNr, KDNr, KDZHDNr, Customer_ID, Customer_Name, "
      strSQL &= "Customer_Strasse, Customer_Ort, Customer_Telefon, Customer_eMail, "
      strSQL &= "Berater, Filiale, VakKontakt, VakState, Bezeichnung, Slogan, Gruppe, ExistLink, "
      strSQL &= "VakLink, Beginn, JobProzent, Anstellung, Dauer, MAAge, "
      strSQL &= "MASex, MAZivil, MALohn, Jobtime, JobOrt, "
      strSQL &= "MAFSchein, MAAuto, MANationality, KDBeschreibung, KDBietet, "
      strSQL &= "SBeschreibung, Reserve1, Taetigkeit, Anforderung, Reserve2, Reserve3, Ausbildung, "
      strSQL &= "Weiterbildung, SKennt, Transfered_User, User_Guid, Transfered_On, Transfered_Guid, "
      strSQL &= "Vak_Region, Vak_Kanton, EDVKennt, "
      strSQL &= "MSprachen, SSprachen, Qualifikation, SQualifikation, Branchen) " ', "

      'strSQL &= "User_Nachname, User_Vorname, User_Telefon, User_Telefax, User_eMail) "
      strSQL &= "Values ("

      strSQL &= "@VakNr, @KDNr, @KDZHDNr, @Customer_ID, @Customer_Name, "
      strSQL &= "@Customer_Strasse, @Customer_Ort, @Customer_Telefon, @Customer_eMail, "
      strSQL &= "@Berater, @Filiale, @VakKontakt, @VakState, @Bezeichnung, @Slogan, @Gruppe, @ExistLink, "
      strSQL &= "@VakLink, @Beginn, @JobProzent, @Anstellung, @Dauer, @MAAge, "
      strSQL &= "@MASex, @MAZivil, @MALohn, @Jobtime, @JobOrt, "
      strSQL &= "@MAFSchein, @MAAuto, @MANationality, @KDBeschreibung, @KDBietet, "
      strSQL &= "@SBeschreibung, @Reserve1, @Taetigkeit, @Anforderung, @Reserve2, @Reserve3, @Ausbildung, "
      strSQL &= "@Weiterbildung, @SKennt, @Transfered_User, @User_Guid, @Transfered_On, @Transfered_Guid, "
      strSQL &= "@Vak_Region, @Vak_Kanton, @EDVKennt, "
      strSQL &= "@MSprachen, @SSprachen, @Qualifikation, @SQualifikation, @Branchen)" ', "
      'strSQL &= "@User_Nachname, @User_Vorname, @User_Telefon, @User_Telefax, @User_eMail)" ', "


      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      ' Daten vom Client
      Dim dt As DataTable = MyDs.Tables("Vakanzen")

      param = cmd.Parameters.AddWithValue("@VakNr", Val(GetDbValue(CStr(dt.Rows(0)("VakNr").ToString))))
      param = cmd.Parameters.AddWithValue("@KDNr", Val(GetDbValue(CStr(dt.Rows(0)("KDNr").ToString))))
      param = cmd.Parameters.AddWithValue("@KDZHDNr", Val(GetDbValue(CStr(dt.Rows(0)("KDZHDNr").ToString))))
      param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
      param = cmd.Parameters.AddWithValue("@Customer_Name", aCustomerData(1))

      param = cmd.Parameters.AddWithValue("@Customer_Strasse", aCustomerData(2))
      param = cmd.Parameters.AddWithValue("@Customer_Ort", aCustomerData(3))
      param = cmd.Parameters.AddWithValue("@Customer_Telefon", aCustomerData(4))
      param = cmd.Parameters.AddWithValue("@Customer_eMail", aCustomerData(6))

      param = cmd.Parameters.AddWithValue("@Berater", GetDbValue(CStr(dt.Rows(0)("USInitial").ToString)))
      param = cmd.Parameters.AddWithValue("@Filiale", GetDbValue(dt.Rows(0)("Filiale").ToString))
      param = cmd.Parameters.AddWithValue("@VakKontakt", GetDbValue(CStr(dt.Rows(0)("VakKontakt").ToString)))
      param = cmd.Parameters.AddWithValue("@VakState", GetDbValue(CStr(dt.Rows(0)("VakState").ToString)))
      param = cmd.Parameters.AddWithValue("@Bezeichnung", GetDbValue(CStr(dt.Rows(0)("Bezeichnung").ToString)))
      param = cmd.Parameters.AddWithValue("@Slogan", GetDbValue(CStr(dt.Rows(0)("Slogan").ToString)))
      param = cmd.Parameters.AddWithValue("@Gruppe", GetDbValue(CStr(dt.Rows(0)("Gruppe").ToString)))
      param = cmd.Parameters.AddWithValue("@ExistLink", GetDbValue(CStr(dt.Rows(0)("ExistLink").ToString)))

      param = cmd.Parameters.AddWithValue("@VakLink", GetDbValue(CStr(dt.Rows(0)("VakLink").ToString)))
      param = cmd.Parameters.AddWithValue("@Beginn", GetDbValue(CStr(dt.Rows(0)("Beginn").ToString)))
      param = cmd.Parameters.AddWithValue("@JobProzent", GetDbValue(CStr(dt.Rows(0)("JobProzent").ToString))) ' 20
      param = cmd.Parameters.AddWithValue("@Anstellung", GetDbValue(CStr(dt.Rows(0)("Anstellung").ToString)))
      param = cmd.Parameters.AddWithValue("@Dauer", GetDbValue(CStr(dt.Rows(0)("Dauer").ToString)))
      param = cmd.Parameters.AddWithValue("@MAAge", GetDbValue(CStr(dt.Rows(0)("MAAge").ToString)))

      param = cmd.Parameters.AddWithValue("@MASex", GetDbValue(CStr(dt.Rows(0)("MASex").ToString)))
      param = cmd.Parameters.AddWithValue("@MAZivil", GetDbValue(CStr(dt.Rows(0)("MAZivil").ToString)))
      param = cmd.Parameters.AddWithValue("@MALohn", GetDbValue(CStr(dt.Rows(0)("MALohn").ToString)))
      param = cmd.Parameters.AddWithValue("@Jobtime", GetDbValue(CStr(dt.Rows(0)("Jobtime").ToString)))
      param = cmd.Parameters.AddWithValue("@JobOrt", GetDbValue(CStr(dt.Rows(0)("JobOrt").ToString)))

      param = cmd.Parameters.AddWithValue("@MAFSchein", GetDbValue(CStr(dt.Rows(0)("MAFSchein").ToString)))
      param = cmd.Parameters.AddWithValue("@MAAuto", GetDbValue(CStr(dt.Rows(0)("MAAuto").ToString)))
      param = cmd.Parameters.AddWithValue("@MANationality", GetDbValue(CStr(dt.Rows(0)("MANationality").ToString)))
      param = cmd.Parameters.AddWithValue("@KDBeschreibung", GetDbValue(CStr(dt.Rows(0)("KDBeschreibung").ToString)))
      param = cmd.Parameters.AddWithValue("@KDBietet", GetDbValue(CStr(dt.Rows(0)("KDBietet").ToString)))

      param = cmd.Parameters.AddWithValue("@SBeschreibung", GetDbValue(CStr(dt.Rows(0)("SBeschreibung").ToString)))
      param = cmd.Parameters.AddWithValue("@Reserve1", GetDbValue(CStr(dt.Rows(0)("Reserve1").ToString)))
      param = cmd.Parameters.AddWithValue("@Taetigkeit", GetDbValue(CStr(dt.Rows(0)("Taetigkeit").ToString)))
      param = cmd.Parameters.AddWithValue("@Anforderung", GetDbValue(CStr(dt.Rows(0)("Anforderung").ToString)))
      param = cmd.Parameters.AddWithValue("@Reserve2", GetDbValue(CStr(dt.Rows(0)("Reserve2").ToString)))
      param = cmd.Parameters.AddWithValue("@Reserve3", GetDbValue(CStr(dt.Rows(0)("Reserve3").ToString)))
      param = cmd.Parameters.AddWithValue("@Ausbildung", GetDbValue(CStr(dt.Rows(0)("Ausbildung").ToString)))

      param = cmd.Parameters.AddWithValue("@Weiterbildung", GetDbValue(CStr(dt.Rows(0)("Weiterbildung").ToString)))
      param = cmd.Parameters.AddWithValue("@SKennt", GetDbValue(CStr(dt.Rows(0)("SKennt").ToString)))
      param = cmd.Parameters.AddWithValue("@Transfered_User", GetDbValue(CStr(dt.Rows(0)("Transfered_User").ToString)))
      param = cmd.Parameters.AddWithValue("@User_Guid", GetDbValue(CStr(dt.Rows(0)("US_Guid").ToString)))
      param = cmd.Parameters.AddWithValue("@Transfered_On", Now.ToString)
      param = cmd.Parameters.AddWithValue("@Transfered_Guid", GetDbValue(CStr(dt.Rows(0)("Transfered_Guid").ToString)))

      param = cmd.Parameters.AddWithValue("@Vak_Region", GetDbValue(CStr(dt.Rows(0)("Vak_Region").ToString)))
      param = cmd.Parameters.AddWithValue("@Vak_Kanton", GetDbValue(CStr(dt.Rows(0)("Vak_Kanton").ToString)))
      param = cmd.Parameters.AddWithValue("@EDVKennt", GetDbValue(CStr(dt.Rows(0)("EDVKennt").ToString)))

      param = cmd.Parameters.AddWithValue("@MSprachen", GetDbValue(CStr(dt.Rows(0)("MSprachen").ToString)))
      param = cmd.Parameters.AddWithValue("@SSprachen", GetDbValue(CStr(dt.Rows(0)("SSprachen").ToString)))
      param = cmd.Parameters.AddWithValue("@Qualifikation", GetDbValue(CStr(dt.Rows(0)("Qualifikation").ToString)))
      param = cmd.Parameters.AddWithValue("@SQualifikation", GetDbValue(CStr(dt.Rows(0)("SQualifikation").ToString)))
      param = cmd.Parameters.AddWithValue("@Branchen", GetDbValue(CStr(dt.Rows(0)("Branchen").ToString)))

      Try
        cmd.ExecuteNonQuery()           ' vakanzen einfügen...
        strValue = "Ihre Daten wurden erfolgreich importiert..."

        Try
          ' UserData füllen...
          strValue = SaveUserData(strUserID, MyDs, aCustomerData)

        Catch ex As Exception

        End Try


      Catch ex As Exception
        strValue = String.Format("Fehler (WS:SaveMyVak): {0}", ex.Message)

      End Try

    Catch ex As Exception

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strValue
  End Function


  ''' <summary>
  ''' Löscht den Selectierten Datensatz von Vakanzen
  ''' </summary>
  ''' <param name="strUserID">USerID kommt von Client</param>
  ''' <param name="strTransfered_Guid">TransferedGuid kommt auch von Client</param>
  ''' <param name="iVakNr">VakNr kommt von Client</param>
  ''' <remarks></remarks>
  <WebMethod(Description:="Löscht den ausgewählten Kundenvakanz.")> _
  Sub DeleteSelectedVak(ByVal strUserID As String, ByVal strTransfered_Guid As String, ByVal iVakNr As Integer)
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Delete Selected Vak-Rec]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@Transfered_Guid", strTransfered_Guid)
      param = cmd.Parameters.AddWithValue("@VakNr", iVakNr)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader

    Catch ex As Exception

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

  End Sub

  <WebMethod(Description:="Zur Auflistung der Vakanzen eines Kunden")> _
Function ListKDVak_Rec(ByVal strUserID As String, ByVal strBeruf As String, _
                    ByVal strOrt As String, ByVal strKanton As String, _
                    ByVal strRegion As String, _
                    ByVal strFiliale As String, _
                    ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec For Listing]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rVakrec As New DataSet
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
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Region", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "Region", _
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
      adapter.SelectCommand.Parameters(0).Value = CType(strUserID, String)
      adapter.SelectCommand.Parameters(1).Value = CType(strBeruf, String)
      adapter.SelectCommand.Parameters(2).Value = CType(strOrt, String)
      adapter.SelectCommand.Parameters(3).Value = CType(strKanton, String)
      adapter.SelectCommand.Parameters(4).Value = CType(strRegion, String)
      adapter.SelectCommand.Parameters(5).Value = CType(strFiliale, String)
      adapter.SelectCommand.Parameters(6).Value = CType(If(strSortkeys = String.Empty, _
                                                           "KD_Vakanzen.Transfered_On DESC", strSortkeys), String)
      Try

        adapter.Fill(rVakrec, "Vakanzen")


        strRecResult = String.Format(If(rVakrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                        "Keine Daten wurden gefunden."), rVakrec.Tables(0).Rows.Count)

        strRecResult = String.Format("{0} Datensätze wurden gefunden.", rVakrec.Tables(0).Rows.Count)


      Catch ex As Exception
        strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

      End Try

    Catch ex As Exception

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rVakrec
  End Function

  Function SaveUserData(ByVal strUserID As String, _
                   ByVal MyDs As DataSet, _
                   ByVal aCustomerData As String()) As String
    Dim strValue As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim strAllowedToSave As String = _clsSystem.GetUserID(strUserID, "Vak_Guid")
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = String.Empty
    Dim param As System.Data.SqlClient.SqlParameter
    Dim dt As DataTable = MyDs.Tables("Vakanzen")

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim s As New List(Of String)

      strSQL = "[Get UserData From Customer_Users]"

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure

      param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
      param = cmd.Parameters.AddWithValue("@User_ID", GetDbValue(CStr(dt.Rows(0)("US_Guid").ToString)))

      Dim rFoundedrec As SqlDataReader = cmd.ExecuteReader

      rFoundedrec.Read()
      Dim bExistUser As Boolean = rFoundedrec.HasRows
      cmd.Parameters.Clear()
      If bExistUser Then Return "Erfolgreich..."

      If Not bExistUser Then
        strSQL = "Insert Into Customer_Users ("
        strSQL &= "Customer_ID, Customer_Name, "
        strSQL &= "User_Initial, User_Sex, User_Vorname, User_Nachname, User_Filiale, "
        strSQL &= "User_Telefon, User_Telefax, User_eMail, User_Homepage, User_ID, User_Picture, User_Sign, "
        strSQL &= "CreatedOn "
        strSQL &= ") Values ("

        strSQL &= "@Customer_ID, @Customer_Name, "
        strSQL &= "@User_Initial, @User_Sex, @User_Vorname, @User_Nachname, @User_Filiale, "
        strSQL &= "@User_Telefon, @User_Telefax, @User_eMail, @User_Homepage, @User_ID, @User_Picture, @User_Sign, "
        strSQL &= "@CreatedOn) "

        Conn = New SqlConnection(connString)
        Conn.Open()
        cmd = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
        cmd.CommandType = Data.CommandType.Text

        param = cmd.Parameters.AddWithValue("@Customer_ID", aCustomerData(0))
        param = cmd.Parameters.AddWithValue("@Customer_Name", aCustomerData(1))
        param = cmd.Parameters.AddWithValue("@User_Initial", GetDbValue(CStr(dt.Rows(0)("USInitial").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Sex", GetDbValue(CStr(dt.Rows(0)("USAnrede").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Vorname", GetDbValue(CStr(dt.Rows(0)("USVorname").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Nachname", GetDbValue(CStr(dt.Rows(0)("USNachname").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Filiale", GetDbValue(dt.Rows(0)("USFiliale").ToString))

        param = cmd.Parameters.AddWithValue("@User_Telefon", GetDbValue(CStr(dt.Rows(0)("USTelefon").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Telefax", GetDbValue(CStr(dt.Rows(0)("USTelefax").ToString)))
        param = cmd.Parameters.AddWithValue("@User_eMail", GetDbValue(CStr(dt.Rows(0)("USMail").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Homepage", GetDbValue(CStr(dt.Rows(0)("USHomepage").ToString)))

        param = cmd.Parameters.AddWithValue("@User_ID", GetDbValue(CStr(dt.Rows(0)("US_Guid").ToString)))
        param = cmd.Parameters.AddWithValue("@User_Picture", dt.Rows(0)("USBild"))
        param = cmd.Parameters.AddWithValue("@User_Sign", dt.Rows(0)("USSign"))

        param = cmd.Parameters.AddWithValue("@CreatedOn", Now.ToString)


        Try
          cmd.ExecuteNonQuery()           ' Benutzer einfügen...
          strValue = "Ihre Benutzerdaten wurden erfolgreich importiert..."

        Catch ex As Exception
          strValue = String.Format("Fehler (WS:SaveUserData_1): {0}", ex.Message)


        End Try

      End If

    Catch ex As Exception
      strValue = String.Format("Fehler (WS:SaveUserData_0): {0}", ex.Message)

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If
    End Try

    Return strValue
  End Function


  Function GetDbValue(ByVal myVale As String) As String

    If String.IsNullOrEmpty(myVale) Then
      Return String.Empty
    Else
      Return myVale
    End If

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

End Class