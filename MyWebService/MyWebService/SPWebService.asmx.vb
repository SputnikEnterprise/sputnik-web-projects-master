
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET-AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
'"http://tempuri.org/")> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/spwebservice/spwebService.asmx")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Service1
  Inherits System.Web.Services.WebService

#Region "Properties..."

  'Dim _ConnectionString As String = ConfigurationSettings.AppSettings("connectionString").ToString
  'Private ReadOnly Property ConnectionString() As String
  '  Get
  '    Return _ConnectionString
  '  End Get
  'End Property

  Dim _BaseDirectory As String = My.Settings.FileSaveToLocation
  Private ReadOnly Property BaseDirectory() As String
    Get
      Return _BaseDirectory
    End Get
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

  Dim _customer_Email As String
  Private Property customer_Email() As String
    Get
      Return _customer_Email
    End Get
    Set(ByVal value As String)
      _customer_Email = value
    End Set
  End Property

  Dim _spUserName As String
  Private Property spUserName() As String
    Get
      Return _spUserName
    End Get
    Set(ByVal value As String)
      _spUserName = value
    End Set
  End Property

  Dim _spUserEmail As String
  Private Property spUserEmail() As String
    Get
      Return _spUserEmail
    End Get
    Set(ByVal value As String)
      _spUserEmail = value
    End Set
  End Property

  Dim _KDZName As String
  Private Property KDZName() As String
    Get
      Return _KDZName
    End Get
    Set(ByVal value As String)
      _KDZName = value
    End Set
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

  Dim _SPID As String
  Private Property SPID() As String
    Get
      Return _SPID
    End Get
    Set(ByVal value As String)
      _SPID = value
    End Set
  End Property

  Dim _ReturnStatus As Boolean
  Private Property ReturnStatus() As Boolean
    Get
      Return _ReturnStatus
    End Get
    Set(ByVal value As Boolean)
      _ReturnStatus = value
    End Set
  End Property

  Dim _ReturnMessage As String
  Private Property ReturnMessage() As String
    Get
      Return _ReturnMessage
    End Get
    Set(ByVal value As String)
      _ReturnMessage = value
    End Set
  End Property

  Dim _GUI As String
  Private Property GUI() As String
    Get
      Return _GUI
    End Get
    Set(ByVal value As String)
      _GUI = value
    End Set
  End Property

#End Region

  ' (Description:="File Upload Webservice")
  <WebMethod(Description:="File Upload Webservice")> _
  Function UploadFile(ByVal strUserID As String, _
                        ByVal strClientID As String, _
                        ByVal filename As String, _
                        ByVal objFile_1 As Byte(), _
                        ByRef strReturnMessage As String) As Boolean
    Dim strBaseDirectory As String = My.Settings.FileSaveToLocation
    Me._ReturnStatus = False
    Me._fileName = filename
    Me._objFile = objFile_1

    Dim _clsSystem As New ClsMain_Net
    'If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return False

    'making sure the base directory string ends with a \ so that
    'the file is uploaded to the correct spot
    If strBaseDirectory.EndsWith("\") = False Then strBaseDirectory &= "\"

    Try

      'checks to make sure the base directory exists
      'if not it attempts to create it
      If Not Directory.Exists(strBaseDirectory) Then
        Throw New Exception("Base Folder does not exist and could not create.")
      End If

      'creates the file by opening and setting the fileaccess to write
      Me._objFileStream = File.Open(strBaseDirectory & Me._fileName, FileMode.Create, FileAccess.Write)

      'byte count
      Dim lngLen As Long = Me._objFile.Length

      'Write the file
      Me._objFileStream.Write(Me._objFile, 0, CType(lngLen, Integer))

      'clears the buffer and writes any buffered data
      Me._objFileStream.Flush()

      'close the file so it can be access by other files
      Me._objFileStream.Close()

      'return a successful upload status
      Me._ReturnStatus = True

      Try
        Dim _clsFunc As New DivFunctions
        Dim strFileContent As String = String.Empty
        Dim CheckFile As New FileInfo(strBaseDirectory & Me._fileName)
        Dim r As StreamReader = CheckFile.OpenText()
        Dim bSendMail As Boolean = My.Settings.bSaveAllLOGS

        strFileContent = r.ReadToEnd()
        r.Close()
        If Not bSendMail Then
          If strFileContent.ToUpper.Contains("Fehler:".ToUpper) And My.Settings.bSaveJustErrors Then
            bSendMail = True
          End If
        End If

        If bSendMail Then
          If strFileContent.Length > 0 Then
            ' Temporär ausschalten...
            '_clsFunc.InsertLog2Db(strClientID, strBaseDirectory & Me._fileName)
            Me.SendMailToKD(strUserID, True, My.Settings.errMailFrom, _
                            My.Settings.errMailTo, _
                            My.Settings.errMailSubject, _
                            strClientID & vbCrLf & My.Settings.errMailBody, _
                            2, strBaseDirectory & Me._fileName)
          End If
        End If
        If My.Settings.bDeleteUploadFile Then File.Delete(strBaseDirectory & Me._fileName)

      Catch ex_0 As Exception
        Me._ReturnMessage = "Exception Occurred. InsertLog2Db: " & ex_0.Message.ToString

      End Try

    Catch exc As System.UnauthorizedAccessException
      'Account does not have enough access to create the file, do to either login or system permissions
      Me._ReturnMessage = "Unauthorized Access Exception Occurred. Error: " & exc.Message.ToString

    Catch exc As SqlException
      Me._ReturnMessage = "SQL Exception Occurred. Error: " & exc.Message.ToString

    Catch exc As Exception
      'catchall error handler
      Me._ReturnMessage = "Exception Occurred. Error: " & exc.Message.ToString

    Finally

      'cleanup just in case
      If Not objFileStream Is Nothing Then
        objFileStream.Close()
      End If
      strReturnMessage = Me.ReturnMessage

    End Try

    Return Me._ReturnStatus
  End Function

  <WebMethod()> Function UploadFile_1(ByVal filename As String, ByVal f As Byte()) As Boolean

    ' the byte array argument containst the content of the file
    ' the string argument contains the name and extension of the 
    ' file pass in through the byte array
    Try

      ' instance a memory stream and pass the byte array
      ' to its constructor
      Dim ms As New MemoryStream(f)

      ' instance a filestream pointing to the
      ' storatge folder, use the original file name
      ' to name the resulting file
      Dim fs As New FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/TransientStorage/") & filename, FileMode.Create)

      ms.WriteTo(fs)

      ' clean up
      ms.Close()
      fs.Close()
      fs.Dispose()

      ' return OK if we made it to here
      Return True

    Catch ex As Exception
      Return False

    End Try


  End Function

  <WebMethod(Description:="Sendet eMail an Sputnik über Webservice")> _
  Function SendMailToKD(ByVal strUserID As String, _
                        ByVal bIsHtml As Boolean, _
                        ByVal strFrom As String, _
                        ByVal strTo As String, _
                        ByVal strSubject As String, _
                        ByVal strBody As String, _
                        ByVal iPriority As Integer, _
                        ByVal strAttachmentFile As String) As Boolean
    Dim obj As System.Net.Mail.SmtpClient = New System.Net.Mail.SmtpClient
    Dim mailmsg As New System.Net.Mail.MailMessage
    Dim bResult As Boolean = False

    Dim _clsSystem As New ClsMain_Net
    If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return False

    Dim strSmtp As String = My.Settings.MySmtpServer
    Try
      With mailmsg
        .IsBodyHtml = bIsHtml
        .To.Clear()
        .From = New System.Net.Mail.MailAddress(strFrom)
        .To.Add(New System.Net.Mail.MailAddress(strTo))

        .Subject = strSubject.Trim()
        .Body = strBody.Trim()
        Select Case iPriority
          Case 0
            .Priority = Net.Mail.MailPriority.Low
          Case 1
            .Priority = Net.Mail.MailPriority.Normal
          Case 2
            .Priority = Net.Mail.MailPriority.High
        End Select
        If strAttachmentFile <> String.Empty Then _
                      .Attachments.Add(New System.Net.Mail.Attachment(strAttachmentFile))

      End With
      Try
        obj.Host = strSmtp
        obj.Send(mailmsg)
        bResult = True


      Catch ex As Exception
        MsgBox(ex.Message)
        bResult = False

      Finally
        obj = Nothing
        mailmsg.Attachments.Dispose()
        mailmsg.Dispose()

      End Try
    Catch ex As Exception
      Dim _clsFunc As New DivFunctions
      _clsFunc.InsertLog2Db("SendMailToKD" & vbCrLf & ex.Message, "")

    End Try

    Return bResult
  End Function

  <WebMethod(Description:="Findet die Versionsnummer für Update")> _
Public Function GetUpdateVersion(ByVal strUserID As String, _
                        ByVal strIDString As String) As String
    Dim strResult As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim _clsReg As New ClsDivReg
    Dim strConnString As String = My.Settings.connectionString
    Dim strRootConnString As String = My.Settings.RootconnectionString

    If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return String.Empty

    If strConnString = String.Empty Then strConnString = _clsSystem.GetConnString()
    If strRootConnString = String.Empty Then strRootConnString = _clsSystem.GetRootConnString()

    Dim Conn As SqlConnection = New SqlConnection(strConnString)

    Dim sSql As String = "Select UpdateVersion From tbl_UpdateInfo"


    Try
      Dim Time_1 As Double = System.Environment.TickCount
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

      Dim rUpdaterec As SqlDataReader = cmd.ExecuteReader
      Trace.Listeners.Clear()

      If rUpdaterec.HasRows() Then
        rUpdaterec.Read()
        strResult = CStr(IIf(IsDBNull(rUpdaterec("Updateversion")), "", rUpdaterec("Updateversion").ToString))
      End If

      Dim _clsFunc As New DivFunctions
      _clsFunc.InsertLog2Db(strIDString, "")
      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für GetUpdateVersion: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "GetUpdateVersion")

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strResult
  End Function

  <WebMethod(Description:="Updatedatum und Uhrzeit")> _
  Public Function GetUpdateDateTime(ByVal strUserID As String, _
                        ByVal strIDString As String) As String
    Dim strResult As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim _clsReg As New ClsDivReg
    Dim strConnString As String = My.Settings.connectionString
    Dim strRootConnString As String = My.Settings.RootconnectionString

    If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return String.Empty

    If strConnString = String.Empty Then strConnString = _clsSystem.GetConnString()
    If strRootConnString = String.Empty Then strRootConnString = _clsSystem.GetRootConnString()

    Dim Conn As SqlConnection = New SqlConnection(strConnString)

    Dim sSql As String = "Select UpdateDate From tbl_UpdateInfo"


    Try
      Dim Time_1 As Double = System.Environment.TickCount
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

      Dim rUpdaterec As SqlDataReader = cmd.ExecuteReader
      Trace.Listeners.Clear()

      If rUpdaterec.HasRows() Then
        rUpdaterec.Read()
        strResult = CStr(IIf(IsDBNull(rUpdaterec("UpdateDate")), "", rUpdaterec("UpdateDate").ToString))
      End If

      Dim _clsFunc As New DivFunctions
      _clsFunc.InsertLog2Db(strIDString, "")
      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für GetUpdateDateTime: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "GetUpdateDateTime")

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strResult
  End Function

  <WebMethod(Description:="Findet die Einstellungsdatei fürs Update")> _
  Public Function GetUpdateSettingFile(ByVal strUserID As String, _
                        ByVal strIDString As String) As String
    Dim strResult As String = String.Empty
    Dim _clsSystem As New ClsMain_Net
    Dim _clsReg As New ClsDivReg
    Dim strConnString As String = My.Settings.connectionString
    Dim strRootConnString As String = My.Settings.RootconnectionString

    If strUserID <> _clsSystem.GetUserID(strUserID, "") Then Return String.Empty

    If strConnString = String.Empty Then strConnString = _clsSystem.GetConnString()
    If strRootConnString = String.Empty Then strRootConnString = _clsSystem.GetRootConnString()

    Dim Conn As SqlConnection = New SqlConnection(strConnString)

    Dim sSql As String = "Select UpdateSettingFile From tbl_UpdateInfo"


    Try
      Dim Time_1 As Double = System.Environment.TickCount
      Conn.Open()

      Dim cmd As System.Data.SqlClient.SqlCommand
      cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)

      Dim rUpdaterec As SqlDataReader = cmd.ExecuteReader
      Trace.Listeners.Clear()

      If rUpdaterec.HasRows() Then
        rUpdaterec.Read()
        strResult = CStr(IIf(IsDBNull(rUpdaterec("UpdateSettingFile")), "", rUpdaterec("UpdateSettingFile").ToString))
      End If

      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für GetUpdateDateTime: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "GetUpdateDateTime")

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

    Return strResult
  End Function


#Region "Sonstige Funktionen..."

  <WebMethod()> _
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

  <WebMethod()> _
  Public Function HelloWorld() As String
    Return "Hello World"
  End Function

  <WebMethod()> _
  Public Function Add(ByVal Num1 As Integer, ByVal Num2 As Integer) As Integer
    Return Num1 + Num2
  End Function
  <WebMethod()> _
Public Function Substract(ByVal Num1 As Integer, ByVal Num2 As Integer) As Integer
    Return Num1 - Num2
  End Function
  <WebMethod()> _
  Public Function Divide(ByVal Num1 As Integer, ByVal Num2 As Integer) As Integer
    Return CInt(Num1 / Num2)
  End Function

#End Region

End Class

Public Class DivFunctions

  ''' <summary>
  ''' Diese Funktion schreibt ein Logrecord in die Datenbank wenn der Client eine Abfrage gesstartet hat...
  ''' </summary>
  ''' <param name="strClientID"></param>
  ''' <remarks></remarks>
  Sub InsertLog2Db(ByVal strClientID As String, ByVal strFullFilename As String)
    Dim _clsSystem As New ClsMain_Net
    Dim _clsReg As New ClsDivReg
    Dim strConnString As String = My.Settings.connectionString
    Dim strRootConnString As String = My.Settings.RootconnectionString

    If strConnString = String.Empty Then strConnString = _clsSystem.GetConnString()
    If strRootConnString = String.Empty Then strRootConnString = _clsSystem.GetRootConnString()

    Dim Conn As SqlConnection = New SqlConnection(strConnString)

    Dim sSql As String = "Insert Into tbl_UserLog (ClientID, TimeDate"

    Dim Time_1 As Double = System.Environment.TickCount
    Conn.Open()
    Dim cmd As System.Data.SqlClient.SqlCommand

    If strFullFilename <> String.Empty Then
      sSql += ", UpdateFileContent) Values (@ClientID, @CreatedOn, @BinaryFile)"

      Dim fi As New System.IO.FileInfo(strFullFilename)
      Dim fs As System.IO.FileStream = fi.OpenRead
      Dim CheckFile As New FileInfo(strFullFilename)

      Dim lBytes As Integer = CInt(fs.Length)
      Dim myImage(lBytes) As Byte

      fs.Read(myImage, 0, lBytes)
      fs.Close()
      fs.Dispose()

      cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@ClientID", strClientID)
      param = cmd.Parameters.AddWithValue("@CreatedOn", Now)
      param = cmd.Parameters.AddWithValue("@BinaryFile", myImage)

    Else
      sSql += ") Values (@ClientID, @CreatedOn)"
      cmd = New System.Data.SqlClient.SqlCommand(sSql, Conn)
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@ClientID", strClientID)
      param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

    End If

    Try

      cmd.ExecuteNonQuery()     ' Datensatz hinzufügen...
      Dim Time_2 As Double = System.Environment.TickCount
      Console.WriteLine("Zeit für InsertLog2Db: (" & ((Time_2 - Time_1) / 1000).ToString() + " s)")


    Catch e As Exception
      MsgBox(e.Message, MsgBoxStyle.Critical, "InsertLog2Db")

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try

  End Sub

  Function SetSyntax(ByVal str1 As String) As String

    str1 = Replace(str1, vbCrLf, "<br />")
    str1 = Replace(str1, "ä", "&auml;")
    str1 = Replace(str1, "ö", "&ouml;")
    str1 = Replace(str1, "ü", "&uuml;")
    str1 = Replace(str1, "Ä", "&Auml;")
    str1 = Replace(str1, "Ö", "&Ouml;")
    str1 = Replace(str1, "Ü", "&Uuml;")
    str1 = Replace(str1, "ß", "&szlig;")
    str1 = Replace(str1, "§", "&sect;")

    str1 = Replace(str1, "€", "&euro;")
    str1 = Replace(str1, vbTab, "&#9;")

    'str1 = Replace(str1, Chr(228), "&auml;")      ' ä
    str1 = Replace(str1, Chr(252), "&uuml;")      ' ü
    str1 = Replace(str1, Chr(129), "&uuml;")      ' ü

    SetSyntax = str1

  End Function


End Class