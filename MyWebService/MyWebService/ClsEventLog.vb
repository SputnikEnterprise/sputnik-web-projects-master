
Imports System.Reflection.Assembly
Imports System.Diagnostics
Imports System.IO
Imports System.Text

<CLSCompliant(True)> _
Public Class ClsEventLog

  Dim _ClsSystem As New ClsMain_Net
  '  Dim _ClsSettingPath As New ClsProgSettingPath

  Public Sub New()

    'default constructor

  End Sub

  Public Sub WriteToLogFile(ByVal msg As String, _
                             ByVal stkTrace As String, _
                             ByVal strMyTitle As String, _
                             ByVal bWriteTime As Boolean, _
                             ByVal strFullFilename As String)
    Dim strPathForLOGs As String = String.Empty
    Dim objAssInfo As New ClsAssInfo()
    Dim strMyFilename As String = My.Settings.strProgLOGFile ' _ClsSystem.GetProgLOGFile


    If strFullFilename = "" Then
      strPathForLOGs = My.Settings.strProgLOGFile
    Else
      Dim fi As FileInfo = New FileInfo(strFullFilename)
      strPathForLOGs = fi.DirectoryName()
      strMyFilename = Split(fi.ToString, "\").GetValue(Split(fi.ToString, "\").Length - 1).ToString
    End If

    Try
      'check and make the directory if necessary; this is set to look in the application
      'folder, you may wish to place the error log in another location depending upon the
      'the user's role and write access to different areas of the file system
      If Not System.IO.Directory.Exists(strPathForLOGs) Then
        System.IO.Directory.CreateDirectory(strPathForLOGs)
      End If

      'check the file
      Dim fs As FileStream = New FileStream(strMyFilename, FileMode.OpenOrCreate, FileAccess.ReadWrite)
      Dim s As StreamWriter = New StreamWriter(fs)
      s.Close()
      fs.Close()

      'log it
      Dim fs1 As FileStream = New FileStream(strMyFilename, FileMode.Append, FileAccess.Write)
      Dim s1 As StreamWriter = New StreamWriter(fs1)
      If strMyTitle <> String.Empty Then s1.Write("Title: " & strMyTitle & vbCrLf)
      s1.Write("Message: " & msg & vbCrLf)
      If stkTrace <> String.Empty Then s1.Write("StackTrace: " & stkTrace & vbCrLf) Else s1.Write(objAssInfo.Product & ":" & vbCrLf)
      If bWriteTime Then s1.Write("Date/Time: " & DateTime.Now.ToString() & vbCrLf)
      s1.Write("==============================================================================" & vbCrLf)
      s1.Close()
      fs1.Close()

    Catch ex As Exception
      Me.WriteToEventLog("Fehler ist aufgetreten... " & ex.Message)

    End Try

  End Sub

  Public Sub WriteErrorLogFile(ByVal msg As String, Optional ByVal strFullFilename As String = "")
    If strFullFilename = String.Empty Then strFullFilename = My.Settings.strErrorLOGFile ' _ClsSystem.GetErrorLOGFile

    Try
      Dim fs1 As FileStream = New FileStream(strFullFilename, FileMode.Append, FileAccess.Write)
      Dim s1 As StreamWriter = New StreamWriter(fs1)
      s1.Write(DateTime.Now.ToString() & vbTab & msg & vbNewLine)
      s1.Close()
      fs1.Close()

    Catch ex As Exception
      Me.WriteToEventLog("Fehler ist aufgetreten... " & ex.StackTrace)

    End Try

  End Sub

  Public Sub WriteTempLogFile(ByVal msg As String, Optional ByVal strFullFilename As String = "")
    If strFullFilename = String.Empty Then strFullFilename = My.Settings.strErrorLOGFile '  _ClsSystem.GetProzessLOGFile

    Try
      Dim fs1 As FileStream = New FileStream(strFullFilename, FileMode.Append, FileAccess.Write)
      Dim s1 As StreamWriter = New StreamWriter(fs1)
      s1.Write(DateTime.Now.ToString() & vbTab & msg & vbNewLine)
      s1.Close()
      fs1.Close()

    Catch ex As Exception
      Me.WriteToEventLog("Fehler ist aufgetreten... " & ex.StackTrace)

    End Try

  End Sub

  Sub WriteMainLog(ByVal strFuncName As String)
    Dim objAssInfo As New ClsAssInfo()
    Dim Filename As String = My.Settings.strErrorLOGFile '  _ClsSystem.GetMainProgLOGFile
    Dim strContent As String = String.Format("({0}){2}{1}{2}{3}\{2}{4}{2}{2}{5}{2}({6})", _
                                             _ClsSystem.GetUserName, Now, vbTab, _
                                             My.Application.Info.DirectoryPath, objAssInfo.Product, _
                                             strFuncName, _
                                             My.Application.Info.Version.Major & "." & _
                                             My.Application.Info.Version.MajorRevision & "." & _
                                             My.Application.Info.Version.Minor & "." & _
                                             My.Application.Info.Version.MinorRevision)

    Try
      Dim OldContent() As Byte = Nothing

      If File.Exists(Filename) Then
        Dim br As BinaryReader = New BinaryReader(File.OpenRead(Filename))
        OldContent = br.ReadBytes(CInt(br.BaseStream.Length))
        br.Close()
      End If
      Dim bw As BinaryWriter = New BinaryWriter(File.OpenWrite(Filename))
      Dim NewContent() As Byte = Encoding.Default.GetBytes(strContent + Environment.NewLine)
      bw.Write(NewContent)
      If Not OldContent Is Nothing Then bw.Write(OldContent)
      bw.Close()


    Catch ex As Exception
      MsgBox(ex.Message.ToString(), MsgBoxStyle.Critical, "Info")
    End Try

  End Sub

  Public Function WriteToEventLog(ByVal entry As String, _
                  Optional ByVal appName As String = "Sputnik", _
                  Optional ByVal eventType As  _
                  EventLogEntryType = EventLogEntryType.Information, _
                  Optional ByVal logName As String = "SP_Update") As Boolean

    Dim objEventLog As New EventLog

    Try

      'Try
      '  EventLog.DeleteEventSource(logName)
      '  EventLog.Delete(appName)

      'Catch ex As Exception

      'End Try

      'Register the Application as an Event Source
      If Not EventLog.SourceExists(appName) Then
        EventLog.CreateEventSource(logName, appName)
      End If

      'log the entry
      objEventLog.Log = appName
      objEventLog.Source = logName
      'objEventLog.Clear()

      objEventLog.WriteEntry(entry)

      Return True

    Catch Ex As Exception

      Return False

    End Try

  End Function

End Class
