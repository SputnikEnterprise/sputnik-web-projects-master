
Imports System.Data.SqlClient
Imports System.IO.File

Public Class ClsMain_Net


#Region "Interne Funktionen"

	Function GetAppGuidValue() As String
    Return "5DBFA1BC-6FC2-4A49-A599-AB5327DEED55"
  End Function

  Function GetUserID(ByVal strIDNr As String, ByVal strFieldName As String) As String
    Dim strResult As String = String.Format("{0}¦{1}¦{2}", strIDNr, _
                                            "Fehler: Keine Berechtigung", String.Empty)
    If strIDNr = String.Empty Then Return strResult

		Dim connString As String = My.Settings.ConnStr_New_spContract
		If strFieldName = String.Empty Then strFieldName = "UserKey"
    Dim strSQL As String = String.Format("Select Top 1 {0} From MySetting ", strFieldName)
    strSQL &= String.Format("Where {0} = @UserKey", strFieldName)

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@UserKey", strIDNr.Trim)

      Dim rGAVrec As SqlDataReader = cmd.ExecuteReader

      While rGAVrec.Read
        strResult = String.Format("{0}¦{1}¦{2}", rGAVrec(strFieldName).ToString, _
                                  "Connection allowed...", String.Empty)
      End While


    Catch ex As Exception
      strResult = String.Format("{0}¦Fehler: GetUserID: {1}¦{2}", strIDNr, ex.Message, strSQL)

    End Try

    Return strResult
  End Function


#End Region


#Region "Externe Funktionen..."


#End Region

End Class
