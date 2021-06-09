'---------------------------------------------------------
' CandidatesBasePage.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

' Base page of candidate pages
Public Class CandidatesBasePage
    Inherits DocumentBasePage

    ' Returns welcome text for candidate pages
    Public Overrides Function GetWelcomeText(ByVal dr As SqlDataReader) As String
    Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty), Utility.GetColumnTextStr(dr, "MA_Nachname", String.Empty))
  End Function

    Public Overrides Function GetUserName(ByVal dr As SqlDataReader) As String
        Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "MA_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty))
    End Function

    Public Overrides Function GetUserEmail(ByVal dr As SqlDataReader) As String
        Return Utility.GetColumnTextStr(dr, "MA_EMail", String.Empty)
    End Function

End Class
