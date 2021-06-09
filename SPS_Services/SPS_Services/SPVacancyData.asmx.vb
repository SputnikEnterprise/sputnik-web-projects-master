Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.SPUtilities

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET-AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPVacancyData.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPVakanzData
  Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPVacancyData"

#Region "WebMethods"

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Berufe (Titel)")> _
  Function ListVacancyTitle(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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
  Function ListVacancyRegion(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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
  Function ListVacancyBranch(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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
  Function ListVacancyCity(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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
  Function ListVacancyCanton(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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
  Function ListVacancyGroup(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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

	<WebMethod(Description:="Zur Auflistung der Vakanzen-SubGruppe (SubGroup)")>
	Function ListVacancySubGroup(ByVal strUserID As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net

		Dim connString As String = My.Settings.ConnStr_Vak
		Dim strSQL As String = "[List Vak-SubGruppe]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("SubGroup").ToString)

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

	<WebMethod(Description:="Zur Auflistung der Vakanzen-SubGruppe anhand einer Grouppe (SubGroup)")>
	Function ListVacancySubGroupAssignedGroup(ByVal strUserID As String, ByVal gruppe As String) As List(Of String)
		Dim s As New List(Of String)
		Dim _clsSystem As New ClsMain_Net

		Dim connString As String = My.Settings.ConnStr_Vak
		Dim strSQL As String = "[List Vacancy SubGruppe For Assigned Group]"

		Dim Conn As SqlConnection = New SqlConnection(connString)
		Conn.Open()

		Try

			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
			cmd.CommandType = Data.CommandType.StoredProcedure
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@UserID", strUserID)
			param = cmd.Parameters.AddWithValue("@Group", gruppe)
			Dim rVakrec As SqlDataReader = cmd.ExecuteReader
			Dim i As Integer

			While rVakrec.Read
				s.Add(rVakrec("SubGroup").ToString)

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
  Function ListVacancySector(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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


#Region "WebMethods JobCategories, JobDisciplines and positions"

	<WebMethod(Description:="Zur Auflistung der Vakanzen-Berufsgruppen (Berufgruppe)")> _
  Function ListVacancyJobCategory(ByVal strUserID As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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

      While rVakrec.Read
        s.Add(rVakrec("Berufgruppe").ToString)
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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Fachrichtigungen (Fachrichtung)")> _
  Function ListVacancyJobDisciplines(ByVal strUserID As String, ByVal JobCategories As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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

      While rVakrec.Read
        s.Add(rVakrec("BerufErfahrung").ToString)
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

  <WebMethod(Description:="Zur Auflistung der Vakanzen-Beruf-Position (Berufposition)")> _
  Function ListVacancyJobPositions(ByVal strUserID As String, ByVal JobCategories As String, ByVal JobDisciplines As String) As List(Of String)
    Dim s As New List(Of String)
    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "Vak_Guid") Then Return s

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

      While rVakrec.Read
        s.Add(rVakrec("BerufPosition").ToString)
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



#End Region






#Region "Private Methods"

	''' <summary>
	''' Replaces a missing object with another object.
	''' </summary>
	''' <param name="obj">The object.</param>
	''' <param name="replacementObject">The replacement object.</param>
	''' <returns>The object or the replacement object it the object is nothing.</returns>
	Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
    If (obj Is Nothing) Then
      Return replacementObject
    Else
      Return obj
    End If
  End Function

  ''' <summary>
  ''' Returns a string or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetString(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns a boolean or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetBoolean(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an integer or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetInt32(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an datetime or the default value if its nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <param name="defaultValue">The default value.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Protected Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader.GetDateTime(columnIndex)
    Else
      Return defaultValue
    End If
  End Function

  ''' <summary>
  ''' Returns an byte array or nothing.
  ''' </summary>
  ''' <param name="reader">The reader.</param>
  ''' <param name="columnName">The column name.</param>
  ''' <returns>Value or default value if the value is nothing</returns>
  Protected Shared Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

    Dim columnIndex As Integer = reader.GetOrdinal(columnName)

    If (Not reader.IsDBNull(columnIndex)) Then
      Return reader(columnIndex)
    Else
      Return Nothing
    End If
  End Function

#End Region


End Class

