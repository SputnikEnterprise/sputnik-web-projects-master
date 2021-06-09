
Imports System.Data.SqlClient
Imports System.IO.File

Public Class ClsMain_Net

  Dim ClsReg As New ClsDivReg

#Region "Interne Funktionen"

  Function GetAppGuidValue() As String
    Return "CFCE4E98-5F9C-4ddc-BEE2-76EDCBA5F55A"
  End Function

  Function GetUserHomePath() As String
    Return ClsReg.AddDirSep(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
  End Function

  Public Function GetConnString() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "Connection String.Net")
  End Function

  Public Function GetRootConnString() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Options\DbSelections", "RootConnStr.Net")
  End Function

  Function GetInitPath() As String
    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "InitPath"))
  End Function

  Function GetSrvRootPath() As String
    Dim strServerInitpath As String = GetInitPath()

    Return strServerInitpath.Substring(0, (Len(strServerInitpath) - 4))
  End Function

  Function GetInitIniFile() As String
    Return GetInitPath() & "Programm.dat"
  End Function

  Function GetUpdatePath() As String
    Return GetSrvRootPath() & "Update\"
  End Function

  Function GetErrorPath() As String
    Return GetInitPath() & "Errors\"
  End Function

  Function GetLocalPath() As String
    Dim strLocalpath As String = ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "ProgUpperPath"))

    Return strLocalpath
  End Function

  Public Function GetMDIniFile() As String
    Return Me.GetMDPath() & "Programm.dat"
  End Function

  Public Function GetMDPath() As String
    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDPath"))
  End Function

  Public Function GetMDMainPath() As String
    Return ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "SelMDMainPath"))
  End Function

  Public Function GetMDTemplatePath() As String
    Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "TemplatePath"))
  End Function

  Function GetSkinPath() As String
    Return ClsReg.AddDirSep(GetMDTemplatePath()) & "Skins\"
  End Function

  Function GetFormDataFile() As String
    Return ClsReg.AddDirSep(GetSkinPath()) & "FormData.XML"
  End Function

  Function GetSQLDataFile() As String
    Return ClsReg.AddDirSep(GetSkinPath()) & "SelectData.XML"
  End Function

  Function GetMDData_XMLFile() As String
    Return ClsReg.AddDirSep(Me.GetMDPath()) & "Programm.XML"
  End Function

  Function GetMSGData_XMLFile() As String
    Return ClsReg.AddDirSep(Me.GetInitPath()) & "MsgInfos.XML"
  End Function

  Public Function GetMDDocPath() As String
    Return Me.GetMDMainPath() & ClsReg.AddDirSep(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "DocPath"))
  End Function

  Function GetUSFiliale() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USFiliale")
  End Function

  Function GetUSLanguage() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USLanguage")
  End Function

  Function GetSQLDateFormat() As String
    Dim strFormat As String = ClsReg.GetINIString(GetInitIniFile, "Customer", "DBServer")
    If strFormat = String.Empty Or strFormat.ToUpper = "dd.MM.yyyy".ToUpper Then strFormat = "dd.MM.yyyy"
    strFormat = strFormat.Replace("mm", "MM")

    Return strFormat
  End Function


  Public Function GetLLLicenceInfo(ByVal iVersion As Integer) As String
    Return CStr(IIf(iVersion = 13, "BsB3EQ", "NwOHEQ"))
  End Function



  Public Function GetSmtpServer() As String
    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "SMTP-Server")
  End Function

  Public Function GetFaxServer() As String
    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "Fax-Server")
  End Function

  Public Function GetDavidServer() As String
    Return ClsReg.GetINIString(GetMDIniFile(), "Mailing", "David-Server")
  End Function

  Function GetMDNr() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\Path", "MDNr").ToString
  End Function

  Function GetUserName() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname") & " " & _
          ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
  End Function

  Function GetUserFName() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USVorname")
  End Function

  Function GetUserLName() As String
    Return ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "USNachname")
  End Function

  Function GetLogedUSNr() As Integer
    Return CInt(ClsReg.GetRegKeyValue(My.Resources.SPSRegKey & "\Sputnik Suite\ProgOptions", "UserNr").ToString)
  End Function

  Function GetUserProfileFile() As String
    Return Me.GetMDMainPath() & "Profiles\UserProfile" & Me.GetLogedUSNr() & ".XML"
  End Function

  Function GetFirstDayOfMonth(ByVal DateTime As Date) As Date
    Return CDate("1." & Month(DateTime) & "." & Year(DateTime))
  End Function

  Function GetLastDayOfMonth(ByVal DateTime As Date) As Date
    Return DateAdd(DateInterval.Day, -1, CDate("01." & CStr(DateTime.Month + 1) & "." & CStr(DateTime.Year.ToString)))
  End Function

  Public Function GetMDDataFromXML(ByVal strFieldName As String) As String
    Dim _ClsReg As New ClsDivReg
    Dim strUserProfileName As String = Me.GetMDData_XMLFile()
    Dim strQuery As String = "//MD_" & Me.GetMDNr & "/Sonstiges/strFieldName" 'AusgNummer"
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

    Return strBez
  End Function

  Public Function GetMessageFromXML(ByVal strMSGID As String) As String
    Dim _ClsReg As New ClsDivReg
    Dim strUserProfileName As String = Me.GetMSGData_XMLFile()
    Dim strQuery As String = "//Messages/MSGID/" & strMSGID
    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)

    Return strBez
  End Function

  Function AllowedExportDoc(ByVal strJobNr As String) As Boolean
    Dim bResult As Boolean
    Dim _ClsReg As New ClsDivReg
    Dim strUserProfileName As String = Me.GetUserProfileFile()
    Dim strQuery As String = "//User_" & Me.GetLogedUSNr & "/Documents/DocName[@ID=" & Chr(34) & strJobNr & Chr(34) & "]/Export"
    'strQuery = "//Control[@Name=" & Chr(34) & "BeraterIn" & Chr(34) & "]/CtlLabel"

    Dim strBez As String = _ClsReg.GetXMLNodeValue(strUserProfileName, strQuery)
    If strBez <> String.Empty Then
      If strBez = CStr(1) Then bResult = True
    End If

    Return bResult
  End Function

  Function GetUserID(ByVal strIDNr As String, ByVal strFieldName As String) As String
    Dim strResult As String = "Fehler: Keine DB-Berechtigung..."
    If strIDNr = String.Empty Then Return strResult

    Dim connString As String = My.Settings.ConnStr_Vak
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

      Dim rGAVrec As SqlDataReader = cmd.ExecuteReader          ' Contracts

      While rGAVrec.Read
        strResult = rGAVrec(strFieldName).ToString
      End While


    Catch ex As Exception
      strResult = String.Format("Fehler_GetUserID: {0}{1}{2}", ex.Message, vbNewLine, strSQL)

    End Try

    Return strResult
  End Function


#End Region

#Region "Setzen von Controls-Eigenschaften..."


#End Region


#Region "Externe Funktionen..."


#End Region

End Class
