
Imports Microsoft.Win32

Imports System.IO
Imports System.Xml
Imports System.Xml.Linq

Imports System.Xml.XmlTextWriter
Imports System.Xml.XmlTextReader
Imports System.Xml.XPath

Public Class ClsDivReg

  Private Declare Ansi Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" ( _
     ByVal lpApplicationName As String, _
     ByVal lpKeyName As String, _
     ByVal lpDefault As String, _
     ByVal lpReturnedString As String, _
     ByVal nSize As Integer, _
     ByVal lpFileName As String) _
 As Integer

  Private Declare Ansi Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" ( _
      ByVal lpApplicationName As String, _
      ByVal lpKeyName As String, _
      ByVal lpString As String, _
      ByVal lpFileName As String) _
  As Integer

  Private Declare Ansi Function DeletePrivateProfileSection Lib "kernel32" Alias "WritePrivateProfileStringA" ( _
      ByVal Section As String, _
      ByVal NoKey As Integer, _
      ByVal NoSetting As Integer, _
      ByVal FileName As String) _
  As Integer

  Function GetXMLNodeValue(ByVal strFileName As String, _
                      ByVal strQuery As String) As String
    Dim strValue As String = String.Empty
    Dim xmlDoc As New XmlDocument()
    Dim xpNav As XPathNavigator
    Dim xni As XPathNodeIterator

    Try
      If File.Exists(strFileName) Then
        xmlDoc.Load(strFileName)
        xpNav = xmlDoc.CreateNavigator()

        xni = xpNav.Select(strQuery)
        Do While xni.MoveNext()
          strValue = xni.Current.Value
          Trace.WriteLine("GetXMLNodeValue: True")
        Loop
      End If

    Catch ex As Exception

    End Try

    Return strValue
  End Function

  Public Sub EraseINISection(ByVal INIFile As String, _
                             ByVal Section As String)
    DeletePrivateProfileSection(Section, 0, 0, INIFile)
  End Sub

  Public Sub SetINIString(ByVal INIFile As String, _
                          ByVal Section As String, _
                          ByVal Key As String, _
                          ByVal Value As String)
    WritePrivateProfileString(Section, Key, Value, INIFile)
  End Sub

  Public Function GetINIString(ByVal INIFile As String, _
                              ByVal Section As String, _
                              ByVal Key As String, _
                              Optional ByVal DefaultValue As String = "", _
                              Optional ByVal BufferSize As Integer = 1024) As String
    Dim sTemp As String = Space(BufferSize)
    Dim Length As Integer = GetPrivateProfileString(Section, Key, DefaultValue, sTemp, BufferSize, INIFile)
    Return Left(sTemp, Length)

  End Function

  Public Sub DeleteKey(ByVal INIFile As String, _
                       ByVal Section As String, _
                       ByVal Key As String)
    WritePrivateProfileString(Section, Key, Nothing, INIFile)
  End Sub

  'GetRegKeyValue(My.Resources.SPSRegKey & "\sputnik Suite\path", "MDPath")
  Public Function GetRegKeyValue(ByVal strRegKey As String, _
                                   ByVal strRegValue As String) As String
    Dim MeinKey As RegistryKey
    Dim KeyValue As String

    MeinKey = Registry.CurrentUser.OpenSubKey(strRegKey)
    If IsNothing(MeinKey) Then
      KeyValue = ""
    Else
      KeyValue = MeinKey.GetValue(strRegValue, "").ToString
      MeinKey.Close()
    End If

    Return KeyValue

  End Function

  'SetRegKeyValue(My.Resources.SPSRegKey & "\sputnik Suite\path", "MDPath")
  Public Function SetRegKeyValue(ByVal strRegKey As String, ByVal strNewKey As String, _
                                   ByVal strRegValue As String) As Boolean
    Dim MeinKey As RegistryKey

    MeinKey = Registry.CurrentUser.CreateSubKey(strRegKey)
    MeinKey.SetValue(strNewKey, strRegValue)

    MeinKey.Close()

    Return True

  End Function

  'GetAllKeyName("SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall", "DisplayName")
  Public Function GetAllKeyName(ByVal strRegKey As String, _
                                 ByVal strRegValue As String) As String
    Dim RegKey As Microsoft.Win32.RegistryKey
    Dim SubRegKeyName As String
    Dim subkeys() As String
    Dim software As String = ""
    Dim strKeyName As String = ""

    Try
      'Registry
      RegKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(strRegKey)
      subkeys = RegKey.GetSubKeyNames

      ' Key auslesen
      For i As Integer = 0 To RegKey.SubKeyCount - 1
        SubRegKeyName = subkeys(i).ToString
        software = ""
        Try
          software = RegKey.OpenSubKey(SubRegKeyName).GetValue(strRegValue).ToString
        Catch ex As Exception

        End Try
        If software <> "" Then
          strKeyName &= "#" & software
          '                    System.IO.File.AppendAllText("C:\software.txt", vbLf & software)
        End If
      Next
      strKeyName &= "#" & software & "#"

    Catch ex As Exception
      'MessageBox.Show("Fehler")
    End Try
    Return strKeyName

  End Function

  Function AddDirSep(ByVal strPathName As String) As String

    Const gstrSEP_URLDIR As String = "/"                      ' Separator for dividing directories in URL addresses.
    Const gstrSEP_DIR As String = "\"                         ' Directory separator character

    If Right(Trim(strPathName), Len(gstrSEP_URLDIR)) <> gstrSEP_URLDIR And _
              Right(Trim(strPathName), Len(gstrSEP_DIR)) <> gstrSEP_DIR Then
      strPathName = RTrim$(strPathName) & gstrSEP_DIR
    End If
    AddDirSep = strPathName

  End Function

End Class
