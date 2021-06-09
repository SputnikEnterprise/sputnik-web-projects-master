'---------------------------------------------------------
' DefaultPage.aspx.vb
'
' © by mf Sputnik Informatik GmbH
'---------------------------------------------------------

Imports System.Web.UI
Imports System.Data.SqlClient
Imports System.Net.Mail
Imports System.IO
Imports System.Data

''' <summary>
''' Shows terms and conditions (AGB).
''' </summary>
Partial Class DefaultPage

    Inherits BasePage

#Region "Const"
#End Region

#Region "Properties"
#End Region

#Region "Methods"

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    Session.Remove("AGB_Accepted")
    tblTermsAndConditionsAfterConfirmation.visible = False


    ' Load customer data
    Dim successFullCustomerDataLoad = Me.IsGuiValid() AndAlso TryToLoadCustomerData()

        If (Not successFullCustomerDataLoad) Then
            Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
        End If

    End Sub

  ''' <summary>
  ''' Stores user feedback for terms and conditions (AGB) in the session.
  ''' If user accepts, the navigation links are enabled.
  ''' </summary>
  Protected Sub chkAGBAccept_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAGBAccept.Click

    Session.Add("AGB_Accepted", True)

    tblTermsAndConditionsAfterConfirmation.Visible = True
    tblTermsAndConditionsToConfirm.Visible = False


    Dim master As SiteMaster = CType(Me.Master, SiteMaster)
    master.UpdateLinkEnableState()
  End Sub

  ''' <summary>
  ''' Returns welcome text for default page
  ''' </summary>
  Public Overrides Function GetWelcomeText(ByVal dr As SqlDataReader) As String

        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

        If (appInfo.CurrentGuidName = appInfo.CandidateQueryParameterName) Then
      Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty), Utility.GetColumnTextStr(dr, "MA_Nachname", String.Empty))
    ElseIf (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return String.Empty
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHDSex", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty))
        Else
            Return String.Empty
        End If

    End Function

    Public Overrides Function GetUserName(ByVal dr As SqlDataReader) As String

        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
        If (appInfo.CurrentGuidName = appInfo.CandidateQueryParameterName) Then
            Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "MA_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty))
        ElseIf (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "KD_Name", String.Empty)
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Vorname", String.Empty))
        Else
            Return String.Empty
        End If
    End Function

    Public Overrides Function GetUserEmail(ByVal dr As SqlDataReader) As String
        Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
        If (appInfo.CurrentGuidName = appInfo.CandidateQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "MA_EMail", String.Empty)
        ElseIf (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "KD_eMail", String.Empty)
        ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            Return Utility.GetColumnTextStr(dr, "ZHD_eMail", String.Empty)
        Else
            Return String.Empty
        End If
    End Function

#End Region

End Class
