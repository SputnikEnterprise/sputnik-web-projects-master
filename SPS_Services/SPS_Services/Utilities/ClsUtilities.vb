
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Net.Mail
Imports wsSPS_Services.CommonXmlUtility
Imports wsSPS_Services.Logging


Namespace SPUtilities


	<Serializable()>
	Public Class NotifyMessageData

		Public Property ID As Integer?
		Public Property CustomerID As String
		Public Property NotifyHeader As String
		Public Property NotifyComments As String
		Public Property NotifyArt As NotifyArtEnum
		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property CheckedOn As DateTime?
		Public Property CheckedFrom As String

	End Class

	<Serializable()>
	Public Enum NotifyArtEnum
		COMMON
		DOCUMENTSCANNING
		APPLICATION

		EMPLOYEEWOS
		CUSTOMERWOS
		WOSMAIL

		STATIONUPDATE
		FTPUPDATE

		PVLCATEGORIES
		PVLVERSIONCHECK

		GAVFLDATA
		PVLADDRESS

		SYSTEMUPDATE
		SCANFILEINFO
		SCANERROR

		INTERNALINFO        ' 15

	End Enum

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

#Region "private consts"

		Private Const RUNTIME_COMMON_CONFIG_FOLDER As String = "Config"
		Private Const PROGRAM_SETTING_FILE As String = "NotifyerSettings.xml"
		Private Const PROGRAM_XML_SETTING_PATH As String = "Settings/Path"
		Private Const PROGRAM_XML_SETTING_DBCONNECTIONS As String = "Settings/DBConnection"
		Private Const PROGRAM_XML_SETTING_EMAIL As String = "Settings/EMailSetting"
		Private Const PROGRAM_XML_SETTING_FTP As String = "Settings/FTPSetting"

#End Region

		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()

		Private m_CommonConfigFolder As String

		Private m_ProgSettingsXml As SettingsXml
		Private m_SettingFileName As String
		Private m_SettingFile As ProgramSettings

		Private Property _ClsSetting As New ClsSetting
		Private Property _UserInfo As String


#Region "Constructor"

		Public Sub New()

			m_CommonConfigFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin", RUNTIME_COMMON_CONFIG_FOLDER)
			m_SettingFile = ReadSettingFile()

		End Sub

		Public Sub New(ByVal _setting As ClsSetting)

			m_CommonConfigFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Bin", RUNTIME_COMMON_CONFIG_FOLDER)
			Me._ClsSetting = _setting

			m_SettingFile = ReadSettingFile()

		End Sub

#End Region


		Function AddNotifyData(ByVal msgData As NotifyMessageData) As Boolean
			Dim success As Boolean = True
			Dim connString As String = My.Settings.Connstr_spSystemInfo_2016
			Dim sql As String = String.Empty
			sql = "[Add Notify Data]"

			Dim Conn As SqlConnection = New SqlConnection(connString)
			Conn.Open()

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
				cmd.CommandType = Data.CommandType.StoredProcedure
				Dim param As System.Data.SqlClient.SqlParameter

				param = cmd.Parameters.AddWithValue("@CustomerID", ReplaceMissing(msgData.CustomerID, String.Empty))
				param = cmd.Parameters.AddWithValue("@NotifyHeader", ReplaceMissing(msgData.NotifyHeader, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@NotifyComments", ReplaceMissing(msgData.NotifyComments, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@NotifyArt", ReplaceMissing(msgData.NotifyArt, DBNull.Value))
				param = cmd.Parameters.AddWithValue("@CreatedFrom", ReplaceMissing(msgData.CreatedFrom, DBNull.Value))

				cmd.ExecuteNonQuery()

			Catch ex As Exception
				Dim msg As String = String.Format("connString: {0} >>> sql: {1} | CustomerID: {2} | SourceModul: {3}  | MessageHeader: {4} | MessageContent: {5}  | MessageArt: {6}  | MessageArt: {7}",
																				connString, sql, msgData.CustomerID, msgData.NotifyHeader, msgData.NotifyHeader, msgData.NotifyComments, msgData.NotifyArt,
																					ex.ToString)
				AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = msgData.CustomerID, .SourceModul = msgData.NotifyHeader, .MessageHeader = msgData.NotifyComments, .MessageContent = msg})
				success = False

				m_Logger.LogError(msg)
			Finally

				If Not Conn Is Nothing Then
					Conn.Close()
					Conn.Dispose()
				End If

			End Try

			Return success
		End Function

		Function AddErrorToDb(ByVal msgData As ErrorMessageData) As Boolean
			Dim success As Boolean = True
			Dim connString As String = My.Settings.Connstr_spSystemInfo_2016
			Dim sql As String = String.Empty
			sql = "Insert Into [spSystemInfo].Dbo.[tbl_ErrorMessage] (Customer_ID, SourceModul, MessageHeader, MessageContent, CreatedOn, CreatedFrom"
			sql &= ") Values ("
			sql &= "@CustomerID, @SourceModul, @MessageHeader, @MessageContent, GetDate(), @CreatedFrom)"

			Dim Conn As SqlConnection = New SqlConnection(connString)
			Conn.Open()

			Try
				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(sql, Conn)
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
				m_Logger.LogError(String.Format("connString: {0} >>> sql: {1} | CustomerID: {2} | SourceModul: {3}  | MessageHeader: {4} | MessageContent: {5}  | MessageArt: {6}",
																				connString, sql, msgData.CustomerID, msgData.SourceModul, msgData.MessageHeader, msgData.MessageContent, msgData.MessageArt))
			Finally

				If Not Conn Is Nothing Then
					Conn.Close()
					Conn.Dispose()
				End If

			End Try

			Return success
		End Function

		Function SendNotificationMails(ByVal strFrom As String, ByVal strTo As String, ByVal strSubject As String, ByVal strBody As String, ByVal aAttachmentFile As List(Of String)) As Boolean
			Dim strMethodeName As String = New System.Diagnostics.StackTrace().GetFrame(0).GetMethod().Name
			Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
			Dim mailmsg As New System.Net.Mail.MailMessage
			Dim result As Boolean = True
			Dim strToAdresses = strTo.Split(New Char() {";"c, ","c, "#"c}).ToList()
			Dim enableSSL As Boolean = m_SettingFile.SmtpUseTLS
			Dim smtpServer As String = m_SettingFile.SmtpServer
			Dim smtpPort As Integer = m_SettingFile.SmtpPort

			Try
				Dim strEx_UserName As String
				Dim strEx_UserPW As String
				strEx_UserName = m_SettingFile.SmtpUser
				strEx_UserPW = m_SettingFile.SmtpPassword

#If DEBUG Then
				smtpServer = "email-smtp.eu-west-1.amazonaws.com"
				strEx_UserName = "EDDJxUJIlNzguWoNafFYCxI0YCrXxYrp/nHzpjXSC7Q="
			strEx_UserPW = "xy9jBgC+EEA8fFRjTcLgRdmHyNYMSVXRX1OihfIttbRZMFVjpDjJ1bP2MbB0hVP8"
				smtpPort = 587
				enableSSL = True

				strFrom = "info@sputnik-it.com"
			strToAdresses = New List(Of String) From {"f.asghari@sputnik-it.com"}
#End If

				With mailmsg
					.IsBodyHtml = True
					.To.Clear()
					.From = New MailAddress(strFrom)
					'.To.Add(New MailAddress(strToAdresses(0).Trim))
					For Each toItem In strToAdresses
						If Not String.IsNullOrWhiteSpace(toItem) Then .To.Add(New MailAddress(toItem.Trim))
					Next
					.ReplyToList.Clear()
					.ReplyToList.Add(.From)

					.Subject = strSubject.Trim()
					.Body = strBody.Trim()
					.Priority = Net.Mail.MailPriority.High

					If Not aAttachmentFile Is Nothing AndAlso aAttachmentFile.Count > 0 Then
						For Each itm In aAttachmentFile
							If File.Exists(itm) Then
								.Attachments.Add(New System.Net.Mail.Attachment(itm))
							End If
						Next
					End If

				End With


				Try
					If Not String.IsNullOrWhiteSpace(strEx_UserName) Then
						Dim mailClient As New System.Net.Mail.SmtpClient(smtpServer, smtpPort)
						mailClient.Credentials = New NetworkCredential(strEx_UserName, strEx_UserPW)
						mailClient.DeliveryMethod = Net.Mail.SmtpDeliveryMethod.Network
						mailClient.EnableSsl = enableSSL

						mailClient.Send(mailmsg)

					Else
						obj.Host = smtpServer
						obj.Send(mailmsg)

					End If

					result = True
					'm_Logger.LogDebug(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", smtpServer, smtpPort, enableSSL, strTo, strFrom, result))


				Catch ex As Exception
					Dim msgContent = ex.ToString
					AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = "", .SourceModul = "Sendig mails", .MessageHeader = "SendNotificationMails", .MessageContent = msgContent})

					m_Logger.LogError(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", smtpServer, smtpPort, enableSSL, strTo, strFrom, ex.ToString()))
					result = False

				Finally
					obj = Nothing
					mailmsg.Attachments.Dispose()
					mailmsg.Dispose()

				End Try
			Catch ex As Exception
				Dim msgContent = ex.ToString
				AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = "", .SourceModul = "Initialization", .MessageHeader = "SendNotificationMails", .MessageContent = msgContent})

				m_Logger.LogError(String.Format("SMTPServer: {0} >>> SMTP-Port: {1} >>> SSL: {2} | From: {3} >>  AN: {4}  | Message: {5}", smtpServer, smtpPort, enableSSL, strTo, strFrom, ex.ToString()))
				result = False

			End Try

			Return result
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

		Function SafeGetDouble(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Double?) As Double?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetDouble(columnIndex)
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

		Function LoadFileBytes(ByVal filePath) As Byte()

			Dim bytes As Byte() = Nothing

			Try

				Using fs As FileStream = New FileStream(filePath, FileMode.Open, FileAccess.Read)

					Using br As BinaryReader = New BinaryReader(fs)

						bytes = br.ReadBytes(Convert.ToInt32(fs.Length))

					End Using
				End Using

			Catch ex As Exception
				bytes = Nothing
			End Try

			Return bytes

		End Function


		Private Function ParseToDouble(ByVal stringvalue As String, ByVal value As Double?) As Double
			Dim result As Double
			If (Not Double.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function

		Private Function ParseToBoolean(ByVal stringvalue As String, ByVal value As Boolean?) As Boolean
			Dim result As Boolean
			If (Not Boolean.TryParse(stringvalue, result)) Then
				Return value
			End If
			Return result
		End Function


#Region "Helpers"

		Private Function ReadSettingFile() As ProgramSettings
			Dim result As New ProgramSettings
			Dim pathSetting As String
			Dim eMailSetting As String
			Dim ftpSetting As String
			Dim dbConnSetting As String

			m_SettingFileName = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
			'm_Logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, m_SettingFileName))
			If Not File.Exists(m_SettingFileName) Then
				MsgBox(String.Format("Die Datei konnte nicht geladen werden!{0}{1}", vbNewLine, m_SettingFileName))

				Return Nothing
			End If
			m_ProgSettingsXml = New SettingsXml(m_SettingFileName)
			pathSetting = PROGRAM_XML_SETTING_PATH
			eMailSetting = PROGRAM_XML_SETTING_EMAIL
			ftpSetting = PROGRAM_XML_SETTING_FTP
			dbConnSetting = PROGRAM_XML_SETTING_DBCONNECTIONS

			Dim fileServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/spsenterprisefolder", pathSetting))
			Dim cvscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvscanfolder", pathSetting))
			Dim reportscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportscanfolder", pathSetting))
			Dim scanparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/scanparserstartprogram", pathSetting))
			Dim emailparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/emailparserstartprogram", pathSetting))
			Dim parseemailattachment = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/parseemailattachment", pathSetting)), True)
			Dim cvlfoldertowatch = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertowatch", pathSetting))
			Dim cvlfoldertoarchive = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertoarchive", pathSetting))
			Dim cvlxmlfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlxmlfolder", pathSetting))
			Dim temporaryfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/temporaryfolder", pathSetting))

			Dim notificationintervalperiode = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting))), 0)
			Dim notificationintervalperiodeforreport = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting))), 0)
			Dim cvlparseasdemo As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlparseasdemo", pathSetting)), True)
			Dim asksendtocvlizer As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/asksendtocvlizer", pathSetting)), True)

			'm_Logger.LogDebug(String.Format("Path is readed: {0}", m_SettingFileName))

			Dim smtpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpserver", eMailSetting))
			Dim SmtpPort = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpport", eMailSetting))
			Dim SmtpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpuser", eMailSetting))
			Dim SmtpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpassword", eMailSetting))
			Dim SmtpUseTLS = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpusetls", eMailSetting))

			Dim stagingEMailFrom = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/stagingemailfrom", eMailSetting))
			Dim stagingEMailTo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/stagingemailto", eMailSetting))
			Dim notifyEMailfrom = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notifyemailfrom", eMailSetting))

			Dim reportmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportmailbox", eMailSetting))
			Dim reportEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailuser", eMailSetting))
			Dim reportEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailpassword", eMailSetting))
			Dim cvmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvmailbox", eMailSetting))
			Dim cvEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailuser", eMailSetting))
			Dim cvEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailpassword", eMailSetting))
			'm_Logger.LogDebug(String.Format("eMail is readed: {0}", m_SettingFileName))

			Dim ftpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpserver", ftpSetting))
			Dim ftpFolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpfolder", ftpSetting))
			Dim ftpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpuser", ftpSetting))
			Dim ftpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftppassword", ftpSetting))
			'm_Logger.LogDebug(String.Format("FTP is readed: {0}", m_SettingFileName))


			Dim connstring_application = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_application", dbConnSetting))
			Dim connstring_cvlizer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_cvlizer", dbConnSetting))
			Dim connstring_systeminfo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_systeminfo", dbConnSetting))
			Dim connstring_scanjobs = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_scanjobs", dbConnSetting))
			Dim connstring_email = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_email", dbConnSetting))
			Dim connstring_wos = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_wos", dbConnSetting))
			'm_Logger.LogDebug(String.Format("Connections is readed: {0}", m_SettingFileName))


			'm_Logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | notificationintervalperiodeforreport: {4}",
			'																fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))
			'm_Logger.LogDebug(String.Format("smtpServer: {0} >>> {1} | SmtpUser: {2} >>> {3} | stagingEMailFrom: {4} | stagingEMailTo: {5} | notifyEMailfrom: {6}",
			'															smtpServer, SmtpPort, SmtpUser, SmtpPassword, stagingEMailFrom, stagingEMailTo, notifyEMailfrom))

			Try
				result.FileServerPath = fileServer
				result.CVScanFolder = cvscanfolder
				result.ReportScanFolder = reportscanfolder
				result.TemporaryFolder = temporaryfolder

				result.ScanParserStartProgram = If(Not File.Exists(scanparserstartprogram), String.Empty, scanparserstartprogram)
				result.EMailParserStartProgram = If(Not File.Exists(emailparserstartprogram), String.Empty, emailparserstartprogram)
				result.Notificationintervalperiode = notificationintervalperiode
				result.Notificationintervalperiodeforreport = notificationintervalperiodeforreport
				result.CVLFolderTOWatch = cvlfoldertowatch
				result.CVLFolderTOArchive = cvlfoldertoarchive
				result.CVLXMLFolder = cvlxmlfolder

				result.SmtpServer = smtpServer
				result.SmtpPort = SmtpPort
				result.SmtpUser = SmtpUser
				result.SmtpPassword = SmtpPassword
				result.stagingEMailFrom = stagingEMailFrom
				result.stagingEMailTo = stagingEMailTo
				result.NotifyEMailFrom = notifyEMailfrom
				result.SmtpUseTLS = SmtpUseTLS

				result.ReportMailbox = reportmailbox
				result.ReportEmailUser = reportEmailUser
				result.ReportEmailPassword = reportEmailPassword
				result.CVMailbox = cvmailbox
				result.CVEmailUser = cvEmailUser
				result.CVEmailPassword = cvEmailPassword
				result.FTPServer = ftpServer
				result.FTPFolder = ftpFolder
				result.FTPUser = ftpUser
				result.FTPPassword = ftpPassword

				result.ConnstringApplication = connstring_application
				result.ConnstringCVLizer = connstring_cvlizer
				result.ConnstringSysteminfo = connstring_systeminfo
				result.ConnstringScanjobs = connstring_scanjobs
				result.ConnstringEMail = connstring_email
				result.ConnstringWOS = connstring_wos
				result.CVLParseAsDemo = cvlparseasdemo
				result.ParseEMailAttachment = parseemailattachment
				result.AskSendToCVLizer = asksendtocvlizer

			Catch ex As Exception
				m_Logger.LogError(String.Format("{0} >>> {1}", m_SettingFileName, ex.ToString))

			End Try

			'm_Logger.LogDebug(String.Format("file is readed: {0}", m_SettingFileName))

			Dim success = CreateTemporaryWorkingFolders(result)
			If Not success Then Return Nothing


			Return result

		End Function

		''' <summary>
		''' Creates the temporary working folders.
		''' </summary>
		Private Function CreateTemporaryWorkingFolders(ByVal settingFile As ProgramSettings) As Boolean

			Try

				If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOWatch) AndAlso Not Directory.Exists(settingFile.CVLFolderTOWatch) Then Directory.CreateDirectory(settingFile.CVLFolderTOWatch)
				If Not String.IsNullOrWhiteSpace(settingFile.CVLFolderTOArchive) AndAlso Not Directory.Exists(settingFile.CVLFolderTOArchive) Then Directory.CreateDirectory(settingFile.CVLFolderTOArchive)
				If Not String.IsNullOrWhiteSpace(settingFile.CVLXMLFolder) AndAlso Not Directory.Exists(settingFile.CVLXMLFolder) Then Directory.CreateDirectory(settingFile.CVLXMLFolder)
				If Not String.IsNullOrWhiteSpace(settingFile.TemporaryFolder) AndAlso Not Directory.Exists(settingFile.TemporaryFolder) Then Directory.CreateDirectory(settingFile.TemporaryFolder)


			Catch ex As Exception
				m_Logger.LogError(ex.ToString)
				Return False
			End Try

			Return True
		End Function

		'Private Function ReadSettingFile_OLD() As ProgramSettings
		'	Dim result As New ProgramSettings
		'	Dim pathSetting As String
		'	Dim eMailSetting As String
		'	Dim ftpSetting As String
		'	Dim dbConnSetting As String

		'	m_SettingFileName = Path.Combine(m_CommonConfigFolder, PROGRAM_SETTING_FILE)
		'	m_Logger.LogDebug(String.Format("m_CommonConfigFolder: {0} | settingFile: {1}", m_CommonConfigFolder, m_SettingFileName))

		'	Try
		'		' C:\Program Files\IIS Express\Config\NotifyerSettings.xml
		'		m_ProgSettingsXml = New SettingsXml(m_SettingFileName)
		'		pathSetting = PROGRAM_XML_SETTING_PATH
		'		eMailSetting = PROGRAM_XML_SETTING_EMAIL
		'		ftpSetting = PROGRAM_XML_SETTING_FTP
		'		dbConnSetting = PROGRAM_XML_SETTING_DBCONNECTIONS

		'		Dim fileServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/spsenterprisefolder", pathSetting))
		'		Dim cvscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvscanfolder", pathSetting))
		'		Dim reportscanfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportscanfolder", pathSetting))
		'		Dim scanparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/scanparserstartprogram", pathSetting))
		'		Dim emailparserstartprogram = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/emailparserstartprogram", pathSetting))
		'		Dim parseemailattachment = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/parseemailattachment", pathSetting)), True)
		'		Dim cvlfoldertowatch = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertowatch", pathSetting))
		'		Dim cvlfoldertoarchive = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlfoldertoarchive", pathSetting))
		'		Dim cvlxmlfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlxmlfolder", pathSetting))
		'		Dim temporaryfolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/temporaryfolder", pathSetting))

		'		Dim notificationintervalperiode = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiode", pathSetting))), 0)
		'		Dim notificationintervalperiodeforreport = ParseToDouble(Val(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/notificationintervalperiodeforreport", pathSetting))), 0)
		'		Dim cvlparseasdemo As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvlparseasdemo", pathSetting)), True)
		'		Dim asksendtocvlizer As Boolean = ParseToBoolean(m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/asksendtocvlizer", pathSetting)), True)

		'		m_Logger.LogDebug(String.Format("Path is readed: {0}", m_SettingFileName))

		'		Dim smtpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/smtpserver", eMailSetting))
		'		Dim reportmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportmailbox", eMailSetting))
		'		Dim reportEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailuser", eMailSetting))
		'		Dim reportEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/reportemailpassword", eMailSetting))
		'		Dim cvmailbox = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvmailbox", eMailSetting))
		'		Dim cvEmailUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailuser", eMailSetting))
		'		Dim cvEmailPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/cvemailpassword", eMailSetting))
		'		m_Logger.LogDebug(String.Format("eMail is readed: {0}", m_SettingFileName))

		'		Dim ftpServer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpserver", ftpSetting))
		'		Dim ftpFolder = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpfolder", ftpSetting))
		'		Dim ftpUser = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftpuser", ftpSetting))
		'		Dim ftpPassword = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/ftppassword", ftpSetting))
		'		m_Logger.LogDebug(String.Format("FTP is readed: {0}", m_SettingFileName))


		'		Dim connstring_application = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_application", dbConnSetting))
		'		Dim connstring_cvlizer = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_cvlizer", dbConnSetting))
		'		Dim connstring_systeminfo = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_systeminfo", dbConnSetting))
		'		Dim connstring_scanjobs = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_scanjobs", dbConnSetting))
		'		Dim connstring_email = m_ProgSettingsXml.GetSettingByKey(String.Format("{0}/connstring_email", dbConnSetting))
		'		m_Logger.LogDebug(String.Format("Connections is readed: {0}", m_SettingFileName))


		'		m_Logger.LogDebug(String.Format("fileServer: {0} | cvscanfolder: {1} | reportscanfolder: {2} | notificationintervalperiode: {3} | | notificationintervalperiodeforreport: {4}",
		'																		fileServer, cvscanfolder, reportscanfolder, notificationintervalperiode, notificationintervalperiodeforreport))

		'		result.FileServerPath = fileServer
		'		result.CVScanFolder = cvscanfolder
		'		result.ReportScanFolder = reportscanfolder
		'		result.TemporaryFolder = temporaryfolder

		'		result.ScanParserStartProgram = If(Not File.Exists(scanparserstartprogram), String.Empty, scanparserstartprogram)
		'		result.EMailParserStartProgram = If(Not File.Exists(emailparserstartprogram), String.Empty, emailparserstartprogram)
		'		result.Notificationintervalperiode = notificationintervalperiode
		'		result.Notificationintervalperiodeforreport = notificationintervalperiodeforreport
		'		result.CVLFolderTOWatch = cvlfoldertowatch
		'		result.CVLFolderTOArchive = cvlfoldertoarchive
		'		result.CVLXMLFolder = cvlxmlfolder

		'		result.SmtpServer = smtpServer
		'		result.ReportMailbox = reportmailbox
		'		result.ReportEmailUser = reportEmailUser
		'		result.ReportEmailPassword = reportEmailPassword
		'		result.CVMailbox = cvmailbox
		'		result.CVEmailUser = cvEmailUser
		'		result.CVEmailPassword = cvEmailPassword
		'		result.FTPServer = ftpServer
		'		result.FTPFolder = ftpFolder
		'		result.FTPUser = ftpUser
		'		result.FTPPassword = ftpPassword

		'		result.ConnstringApplication = connstring_application
		'		result.ConnstringCVLizer = connstring_cvlizer
		'		result.ConnstringSysteminfo = connstring_systeminfo
		'		result.ConnstringScanjobs = connstring_scanjobs
		'		result.ConnstringEMail = connstring_email
		'		result.CVLParseAsDemo = cvlparseasdemo
		'		result.ParseEMailAttachment = parseemailattachment
		'		result.AskSendToCVLizer = asksendtocvlizer

		'	Catch ex As Exception
		'		m_Logger.LogError(String.Format("{0}", ex.ToString))

		'		Return Nothing
		'	End Try

		'	m_Logger.LogDebug(String.Format("file is readed: {0}", m_SettingFileName))


		'	Return result

		'End Function


#End Region


	End Class


End Namespace
