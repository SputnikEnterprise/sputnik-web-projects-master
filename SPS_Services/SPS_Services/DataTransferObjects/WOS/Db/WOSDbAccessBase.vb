
Imports System.Data.SqlClient


Namespace WOSUtilities


	Public Class WOSDbAccess
		'Inherits JobPlatform.JobPlatformDbAccessBase


#Region "Public Methods"


		''' <summary>
		'''  Loads JobCH database vacancy data.
		''' </summary>
		''' <param name="customer_ID">The customer guid.</param>
		''' <returns>true if founded or false.</returns>
		Public Function IsUserAllowedForUsingService(ByVal customer_ID As String, ByVal WOSEnum As WOSModulData.ModulArt) As Boolean
			Dim result As Boolean = True
			If String.IsNullOrWhiteSpace(customer_ID) Then Return False

			Dim connString As String = My.Settings.ConnStr_New_spContract
			Dim fieldName As String = "MA_Guid"
			Dim sql As String

			Select Case True
				Case WOSEnum = WOSModulData.ModulArt.CustomerDocument

					fieldName = "KD_Guid"

				Case WOSEnum = WOSModulData.ModulArt.CustomerDocument
					fieldName = "MA_Guid"

				Case WOSEnum = WOSModulData.ModulArt.CustomerDocument
					fieldName = "Vak_Guid"

			End Select

			sql = "Select Top 1 {0} From MySetting "
			sql &= "WHERE {0} = @customer_ID"

			sql = String.Format(sql, fieldName)

			Dim Conn As SqlConnection = New SqlConnection(connString)
			Conn.Open()

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = Data.CommandType.Text
				Dim param As System.Data.SqlClient.SqlParameter
				param = cmd.Parameters.AddWithValue("customer_ID", customer_ID)

				Dim reader As SqlDataReader = cmd.ExecuteReader

				If (Not reader Is Nothing) Then
					result = reader.HasRows
				End If


			Catch ex As Exception
				'SaveErrToDb(String.Format("Fehler: GetUserID: {0}", customer_ID), ex.ToString)

			End Try


			Return result

		End Function

		'''' <summary>
		'''' Saves an error to the database.
		'''' </summary>
		'''' <param name="strErrorMessage">The error message.</param>
		'Public Sub SaveErrToDb(ByVal strErrorMessage As String, ByVal AdditioinalInfo As String)
		'	Dim connString As String = My.Settings.ConnStr_New_spContract
		'	Dim strSQL As String = String.Empty
		'	strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
		'	strSQL &= "CreatedOn) Values ("
		'	strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, GetDate())"

		'	Dim Conn As SqlConnection = New SqlConnection(connString)
		'	Conn.Open()

		'	Try
		'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
		'		cmd.CommandType = Data.CommandType.Text
		'		Dim param As System.Data.SqlClient.SqlParameter
		'		param = cmd.Parameters.AddWithValue("@Modulname", "WOS")
		'		param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
		'		param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
		'		param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
		'		param = cmd.Parameters.AddWithValue("@RequestParam", AdditioinalInfo)

		'		cmd.ExecuteNonQuery()

		'	Catch ex As Exception

		'	Finally

		'		If Not Conn Is Nothing Then
		'			Conn.Close()
		'			Conn.Dispose()
		'		End If

		'	End Try

		'End Sub

#End Region

	End Class

End Namespace
