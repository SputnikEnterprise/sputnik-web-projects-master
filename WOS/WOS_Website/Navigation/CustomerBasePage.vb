'---------------------------------------------------------
' CustomerBasePage.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient

' Base page of canidate pages
Public MustInherit Class CustomerBasePage
    Inherits DocumentBasePage


    ' Returns welcome text for customer pages
    Public Overrides Function GetWelcomeText(ByVal dr As SqlDataReader) As String

        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

        If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return String.Empty
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHDSex", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty))
        Else
            Return String.Empty
        End If

    End Function

    Public Overrides Function GetUserName(ByVal dr As SqlDataReader) As String

        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

        If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "KD_Name", String.Empty)
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Vorname", String.Empty))
        Else
            Return String.Empty
        End If

    End Function

    Public Overrides Function GetUserEmail(ByVal dr As SqlDataReader) As String
        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
        If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "KD_eMail", String.Empty)
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "ZHD_eMail", String.Empty)
        Else
            Return String.Empty
        End If
    End Function

End Class
