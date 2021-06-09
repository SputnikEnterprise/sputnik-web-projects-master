
Imports System.Data.SqlClient
Imports System.IO
Imports SPWebService.DataTransferObject.Notify
Imports System.Security.Cryptography
Imports System.Globalization

Namespace SPUtilities


	Public Class ErrorMessageData

		Public Property CustomerID As String
		Public Property SourceModul As String
		Public Property MessageArt As Integer
		Public Property MessageHeader As String
		Public Property MessageContent As String
		Public Property CreatedOn As DateTime
		Public Property CreatedFrom As String

	End Class


	Public Class ClsUtilities

		Private Const MODUL_NAME As String = "ClsUtilities"

		'Private Property _ClsSetting As New ClsSetting
		Private Property _UserInfo As String


#Region "Constructor"

		'Public Sub New()

		'End Sub

		Public Sub New()

			'Me._ClsSetting = _setting

		End Sub

#End Region


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
				param = cmd.Parameters.AddWithValue("@Modulname", "SPModulUtil.asmx")
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

			Finally

				If Not Conn Is Nothing Then
					Conn.Close()
					Conn.Dispose()
				End If

			End Try

			Return strResult
		End Function

		Function AddErrorToDb(ByVal msgData As ErrorMessageData) As Boolean
			Dim success As Boolean = True
			Dim connString As String = My.Settings.ConnStr_SystemInfo
			Dim strSQL As String = String.Empty
			strSQL = "Insert Into tbl_ErrorMessage (Customer_ID, SourceModul, MessageHeader, MessageContent, CreatedOn, CreatedFrom"
			strSQL &= ") Values ("
			strSQL &= "@CustomerID, @SourceModul, @MessageHeader, @MessageContent, GetDate(), @CreatedFrom)"

			Dim Conn As SqlConnection = New SqlConnection(connString)
			Conn.Open()

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
				cmd.CommandType = Data.CommandType.Text
				Dim param As System.Data.SqlClient.SqlParameter

				param = cmd.Parameters.AddWithValue("@CustomerID", ReplaceMissing(msgData.CustomerID, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@SourceModul", ReplaceMissing(msgData.SourceModul, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@MessageHeader", ReplaceMissing(msgData.MessageHeader, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@MessageContent", ReplaceMissing(msgData.MessageContent, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@CreatedFrom", ReplaceMissing(msgData.CreatedFrom, DBNull.Value))

				cmd.ExecuteNonQuery()

			Catch ex As Exception
				success = False
			Finally

				If Not Conn Is Nothing Then
					Conn.Close()
					Conn.Dispose()
				End If

			End Try

			Return success
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
				param = cmd.Parameters.AddWithValue("@Modulname", ReplaceMissing(strErrorMessage, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
				param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
				param = cmd.Parameters.AddWithValue("@Answer", ReplaceMissing(strErrorMessage, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@RequestParam", ReplaceMissing(Me._UserInfo, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

				cmd.ExecuteNonQuery()

			Catch ex As Exception
				'     strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)
			Finally

				If Not Conn Is Nothing Then
					Conn.Close()
					Conn.Dispose()
				End If

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


		''' <summary>
		''' Returns a string or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetString(columnIndex)
				Return CStr(reader(columnIndex))
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
		Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetBoolean(columnIndex)
				Return CBool(reader(columnIndex))
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
		Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetInt32(columnIndex)
				Return CInt(reader(columnIndex))
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns an short integer or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Function SafeGetShort(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Short?) As Short?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetInt16(columnIndex)
				Return CShort(reader(columnIndex))
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns an byte or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Protected Shared Function SafeGetByte(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Byte?) As Byte?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetByte(columnIndex)
				Return CByte(reader(columnIndex))
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns an decimal or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Function SafeGetDecimal(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Decimal?) As Decimal?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetDecimal(columnIndex)
				Return CDec(reader(columnIndex))
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
		Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				'Return reader.GetDateTime(columnIndex)
				Return CDate(reader(columnIndex))
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
		Function SafeGetByteArray(ByVal reader As SqlDataReader, ByVal columnName As String) As Byte()

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader(columnIndex)
			Else
				Return Nothing
			End If
		End Function


		''' <summary>
		''' Replaces a missing object with another object.
		''' </summary>
		''' <param name="obj">The object.</param>
		''' <param name="replacementObject">The replacement object.</param>
		''' <returns>The object or the replacement object it the object is nothing.</returns>
		Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
			If (obj Is Nothing) Then
				Return replacementObject
			Else
				Return obj
			End If
		End Function

		''' <summary>
		''' Writes bytes to a file.
		''' </summary>
		''' <param name="filePath">The file path.</param>
		''' <param name="bytes">The file bytes.</param>
		''' <returns>Boolean value indicating success.</returns>
		Function WriteFileBytes(ByVal filePath As String, ByVal bytes As Byte()) As Boolean
			Dim success = True

			Try
				File.WriteAllBytes(filePath, bytes)

			Catch ex As Exception
				success = False
			End Try

			Return success

		End Function


		Function GetEMailDataForNotifications(ByVal customerID As String) As EMailNotificationData

			Dim connString As String = My.Settings.ConnStr_Scanning
			Dim listOfSearchResultDTO As New EMailNotificationData
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try

				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get Notifications EMail Data]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CustomerID", customerID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}", _
																					cmd.Parameters(i).ParameterName, _
																					cmd.Parameters(i).DbType, _
																					cmd.Parameters(i).Size, _
																					cmd.Parameters(i).Value, _
																					ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New EMailNotificationData
				reader = cmd.ExecuteReader

				' Read all data.
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					listOfSearchResultDTO.ID = utility.SafeGetInteger(reader, "ID", 0)
					listOfSearchResultDTO.Customer_ID = utility.SafeGetString(reader, "Customer_ID")
					listOfSearchResultDTO.BCCAddresses = utility.SafeGetString(reader, "bccAddresses")
					listOfSearchResultDTO.MailPassword = utility.SafeGetString(reader, "MailPassword")
					listOfSearchResultDTO.MailSender = utility.SafeGetString(reader, "MailSender")
					listOfSearchResultDTO.MailUserName = utility.SafeGetString(reader, "MailUserName")
					listOfSearchResultDTO.Recipients = utility.SafeGetString(reader, "Recipients")
					listOfSearchResultDTO.Report_Recipients = utility.SafeGetString(reader, "Report_Recipients")
					listOfSearchResultDTO.SmtpPort = utility.SafeGetInteger(reader, "SmtpPort", 0)
					listOfSearchResultDTO.SmtpServer = utility.SafeGetString(reader, "SmtpServer")
					listOfSearchResultDTO.ActivateSSL = utility.SafeGetBoolean(reader, "ActivateSSL", False)
					listOfSearchResultDTO.TemplateFolder = utility.SafeGetString(reader, "TemplateFolder")

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = MODUL_NAME, .MessageHeader = "GetEMailDataForNotifications", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			' Return search data as an array.
			Return listOfSearchResultDTO
		End Function





	End Class


End Namespace
