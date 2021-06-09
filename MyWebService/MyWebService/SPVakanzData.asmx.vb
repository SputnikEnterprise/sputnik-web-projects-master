
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET-AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/SPVakanzData.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPVakanzData
  Inherits System.Web.Services.WebService

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Berufe (Titel)")> _
Function GetKDVak_Titel(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Titel]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Bezeichnung
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Bezeichnung").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Regionen (Region)")> _
Function GetKDVak_Region(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Region]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Regionen
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Vak_Region").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Filiale (Filiale)")> _
Function GetKDVak_Filiale(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Filiale]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Filiale
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Filiale").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Ort (Ort)")> _
Function GetKDVak_Ort(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Ort]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Ort
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("JobOrt").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Kanton (Vak_Kanton)")> _
Function GetKDVak_Kanton(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Kanton]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Vak_Kanton
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Vak_Kanton").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Gruppe (Gruppe)")> _
Function GetKDVak_Gruppe(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Gruppe]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Gruppe
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Gruppe").ToString)

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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Branchen (Branchen)")> _
Function GetKDVak_Branchen(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vak-Branchen]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Gruppe
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("Branchen").ToString)

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


#Region "JobCategories, JobDisciplines and positions"

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Berufsgruppen (Berufgruppe)")> _
  Function Vacancy_Job_Category(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[List Vacancy Job Categories]"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try

      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Berufgruppen
      Dim i As Integer

      While rVakrec.Read
        s.Add(rVakrec("[Job Categories]").ToString)

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

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Fachrichtigungen (Fachrichtung)")>
	Function Vacancy_Job_Disciplines(ByVal strUserID As String, ByVal JobCategories As String) As List(Of String)
		'Function Vacancy_Job_Category(ByVal strUserID As String, ByVal JobCategories As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_Vak
		Dim strSQL As String = "[List Vacancy Job Disciplines]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@JobCategories", JobCategories)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Fachrichtungen
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("[Job Disciplines]").ToString)

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

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Position (Berufposition)")>
	Function Vacancy_Job_Position(ByVal strUserID As String, ByVal JobCategories As String, ByVal JobDisciplines As String) As List(Of String)
		'Function Vacancy_Job_Category(ByVal strUserID As String, ByVal JobCategories As String, ByVal JobDisciplines As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net
		If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

		Dim connString As String = My.Settings.ConnStr_Vak
		Dim strSQL As String = "[List Vacancy Job Positions]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@JobCategories", JobCategories)
			param = cmd.Parameters.AddWithValue("@Jobdisciplines", JobDisciplines)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader          ' Position
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("[Job Position]").ToString)

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


#End Region






	<WebMethod(Description:="Zur Auflistung der Vakanzen (Alle Datensätze)")> _
Function GetKDVak_Rec(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strRegion As String, _
                      ByVal strFiliale As String, _
                      ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rVakrec As New DataSet

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

      adapter.Fill(rVakrec, "Vakanzen")


      strRecResult = String.Format(If(rVakrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rVakrec.Tables(0).Rows.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", rVakrec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strRegion, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rVakrec
  End Function

  <WebMethod(Description:="Zur Auflistung der Vakanzen mit Gruppen (Alle Datensätze)")> _
Function GetKDVak_With_Group_Rec(ByVal strUserID As String, ByVal strBeruf As String, _
                    ByVal strOrt As String, ByVal strKanton As String, _
                    ByVal strRegion As String, _
                    ByVal strFiliale As String, _
                    ByVal strGruppe As String, _
                    ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec with Gruppe]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rVakrec As New DataSet

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
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Gruppe", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "Gruppe", _
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
      adapter.SelectCommand.Parameters(6).Value = CType(strGruppe, String)
      adapter.SelectCommand.Parameters(7).Value = CType(If(strSortkeys = String.Empty, _
                                                           "KD_Vakanzen.Transfered_On DESC", strSortkeys), String)

      adapter.Fill(rVakrec, "Vakanzen")


      strRecResult = String.Format(If(rVakrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rVakrec.Tables(0).Rows.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", rVakrec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strRegion, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rVakrec
  End Function

  <WebMethod(Description:="Zur Auflistung der Vakanzen mit Gruppen und Branchen (Alle Datensätze)")> _
Function GetKDVak_With_Group_Branchen_Rec(ByVal strUserID As String, ByVal strBeruf As String, _
                  ByVal strOrt As String, ByVal strKanton As String, _
                  ByVal strRegion As String, _
                  ByVal strFiliale As String, _
                  ByVal strGruppe As String, _
                  ByVal strBranchen As String, _
                  ByVal strSortkeys As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec with Gruppe And Branchen]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rVakrec As New DataSet

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
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Gruppe", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "Gruppe", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@Branchen", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "Branchen", _
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
      adapter.SelectCommand.Parameters(6).Value = CType(strGruppe, String)
      adapter.SelectCommand.Parameters(7).Value = CType(strBranchen, String)
      adapter.SelectCommand.Parameters(8).Value = CType(If(strSortkeys = String.Empty, _
                                                           "KD_Vakanzen.Transfered_On DESC", strSortkeys), String)

      adapter.Fill(rVakrec, "Vakanzen")


      strRecResult = String.Format(If(rVakrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rVakrec.Tables(0).Rows.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", rVakrec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strRegion, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rVakrec
  End Function


  <WebMethod(Description:="Zur Auflistung eines Datensatzes anhand eines ID in Form von Dataset")> _
Function GetSelectedVakRec_By_ID(ByVal strUserID As String, ByVal strRecID As String) As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec By ID]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rVakrec As New DataSet

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
      adapter.SelectCommand.Parameters.Add(New Global.System.Data.SqlClient.SqlParameter("@RecID", _
                                                                                         Global.System.Data.SqlDbType.NVarChar, _
                                                                                         0, _
                                                                                         Global.System.Data.ParameterDirection.Input, _
                                                                                         0, 0, "RecID", _
                                                                                         Global.System.Data.DataRowVersion.Original, _
                                                                                         False, Nothing, "", "", ""))
      adapter.SelectCommand.Parameters(0).Value = CType(strUserID, String)
      adapter.SelectCommand.Parameters(1).Value = CType(strRecID, String)

      adapter.Fill(rVakrec, "Vakanzen")


      strRecResult = String.Format(If(rVakrec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rVakrec.Tables(0).Rows.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", rVakrec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally

      WriteConnectionHistory(strUserID, strRecID, "GetSelectedVakRec_By_ID", _
                                   String.Empty, String.Empty, String.Empty, String.Empty, strRecResult)
      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return rVakrec
  End Function


  <WebMethod(Description:="Zur Auflistung der Vakanzen-Datensätze als Array (Alle gesuchten Datensätze)")> _
  Function GetKDVak_Rec_In_Array(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strRegion As String, _
                      ByVal strFiliale As String, _
                      ByVal strSortkeys As String) As List(Of String)
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim liVakrec As New List(Of String)

    ' ---------------------------------------------------------------------------------------
    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@Beruf", strBeruf)
      param = cmd.Parameters.AddWithValue("@Ort", strOrt)
      param = cmd.Parameters.AddWithValue("@Kanton", strKanton)
      param = cmd.Parameters.AddWithValue("@Region", strRegion)
      param = cmd.Parameters.AddWithValue("@Filiale", strFiliale)
      param = cmd.Parameters.AddWithValue("@SortString", If(strSortkeys = String.Empty, "KD_Vakanzen.Transfered_On DESC", strSortkeys))

      Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' GAV-Gruppe0

      While rGAVrec.Read
        For i As Integer = 0 To rGAVrec.FieldCount - 1
          liVakrec.Add(String.Format("{0}#{1}", rGAVrec.GetName(i).ToString.Replace(" ", "_"), rGAVrec(i).ToString))
        Next i
      End While


      strRecResult = String.Format(If(liVakrec.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), liVakrec.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", liVakrec.Count / 61)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strRegion, strFiliale, strSortkeys, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return liVakrec
  End Function

  <WebMethod(Description:="Zur Auflistung eines Datensatzes anhand eines ID in Form von Array")> _
Function GetSelectedVakRec_By_ID_In_Array(ByVal strUserID As String, ByVal strRecID As String) As List(Of String)
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strSQL As String = "[Get Vakrec By ID]"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim liVakrec As New List(Of String)

    ' ---------------------------------------------------------------------------------------
    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.StoredProcedure
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserID", strUserID)
      param = cmd.Parameters.AddWithValue("@RecID", strRecID)

      Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' Vakanzen...

      While rGAVrec.Read
        For i As Integer = 0 To rGAVrec.FieldCount - 1
          liVakrec.Add(String.Format("{0}#{1}", rGAVrec.GetName(i).ToString.Replace(" ", "_"), rGAVrec(i).ToString))
        Next i
      End While


      strRecResult = String.Format(If(liVakrec.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), liVakrec.Count)

      strRecResult = String.Format("{0} Datensätze wurden gefunden.", liVakrec.Count / 61)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      WriteConnectionHistory(strUserID, strRecID, "GetSelectedVakRec_By_ID_In_Array", _
                             String.Empty, String.Empty, String.Empty, String.Empty, strRecResult)

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return liVakrec
  End Function

  Sub WriteConnectionHistory(ByVal strUserID As String, ByVal strBeruf As String, _
                      ByVal strOrt As String, ByVal strKanton As String, _
                      ByVal strRegion As String, ByVal strFiliale As String, _
                      ByVal strSortkeys As String, ByVal strRecResult As String)
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = My.Settings.ConnStr_Vak
    Dim strQuery As String = "" ' "Declare @iCount int "
    'strQuery &= "Set @iCount = IsNull((Select COUNT(*) + 1 From Tab_ModulUsage Where ModulParameter Like '%{0}%'), 1) "
    'strQuery &= "Delete Tab_ModulUsage Where ModulParameter Like '%{0}%' "
    strQuery &= "Insert Into Tab_ModulUsage (ModulName, UseCount, UseDate, MachineID, ModulParameter, IsWebService) Values ("
    strQuery &= "@ModulName, @UseCount, "
    strQuery &= "@UseDate, @MachineID, @ModulParameter, @IsWebService)"
    'strQuery = String.Format(strQuery, strUserID)


    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(strQuery, Conn)
      cmd.CommandType = CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter

      param = cmd.Parameters.AddWithValue("@ModulName", "SPVakanzData.asmx")
      param = cmd.Parameters.AddWithValue("@UseCount", 1)
      param = cmd.Parameters.AddWithValue("@UseDate", Now.ToString)
      param = cmd.Parameters.AddWithValue("@MachineID", String.Empty)
      param = cmd.Parameters.AddWithValue("@ModulParameter", strUserID & vbNewLine & _
                                          strBeruf & vbNewLine & _
                                          strOrt & vbNewLine & _
                                          strKanton & vbNewLine & _
                                          strRegion & vbNewLine & _
                                          strFiliale & vbNewLine & _
                                          strSortkeys & vbNewLine & vbNewLine & _
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