'---------------------------------------------------------
' SiteMaster.aspx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports System.Data

''' <summary>
''' The master page
''' </summary>
Partial Public Class SiteMaster
    Inherits System.Web.UI.MasterPage

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        FrameworkAgreementLink()
    UpdateLinkEnableState()
    'Added Martin 16.01.2019
    Page.Header.DataBind()
  End Sub

    ''' <summary>
    ''' Updates the navigation link state
    ''' </summary>
    Public Sub UpdateLinkEnableState()

    Dim queryString As String = Request.ServerVariables("QUERY_STRING")
    Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
    'Dim appInfo = New ApplicationInfo With {.CurrentGuidValue = "2f7f1458-afc8-4826-8184-08a0b1e557c1", .CurrentGuidName = "ZHD"}
    Dim navPanel As NavigationBase
        If Not (Session.Contents("AGB_Accepted") Is Nothing Or queryString Is Nothing) Then

            If (appInfo.CurrentGuidName = appInfo.CandidateQueryParameterName) Then
                navPanel = LoadControl("~/NavigationControls/CandidatesNav.ascx")
            ElseIf (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
                navPanel = LoadControl("~/NavigationControls/KDNav.ascx")
            ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
                navPanel = LoadControl("~/NavigationControls/ZHDNav.ascx")
            Else
                Return
            End If

            navPanel.ID = "navigation"
            navPanel.InitNavigationMenu()
            navigationPanelContainer.Controls.Add(navPanel)
        End If

    End Sub

    ''' <summary>
    ''' Shows the FrameworkAgreement
    ''' </summary>
    Public Sub FrameworkAgreementLink()
        frameworkAgreement.Visible = False
    Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
    'Dim appInfo = New ApplicationInfo With {.CurrentGuidValue = "2f7f1458-afc8-4826-8184-08a0b1e557c1", .CurrentGuidName = "ZHD"}
    If appInfo.CurrentGuidName.ToLower = "sp" Then
			' SQL Command
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
			cmd.CommandType = CommandType.Text
			cmd.CommandText = "SELECT TOP 1 AGB_Wos FROM Kandidaten Where MA_Guid = @KD_Guid"
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@KD_Guid", appInfo.CurrentGuidValue)
			Dim conn As New System.Data.SqlClient.SqlConnection
			Dim dataReader As System.Data.SqlClient.SqlDataReader = Nothing
			Try
				conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString()
				' Open connection to database and read data
				conn.Open()
				cmd.Connection = conn
				dataReader = cmd.ExecuteReader()
				dataReader.Read()
				If dataReader.HasRows = True Then
					Dim strAGBArt As String = Utility.GetColumnTextStr(dataReader, "AGB_Wos", "")
					If (String.IsNullOrEmpty(strAGBArt) Or strAGBArt = "0") Then
						frameworkAgreement.Visible = True
					End If
				End If
				dataReader.Close()
			Finally
				conn.Close()
				conn.Dispose()
			End Try
		End If
	End Sub

	''' <summary>
	''' Shows or hides message for "no documetns found"
	''' </summary>
	Public Sub ShowInfoMessage(ByVal show As Boolean)
        lblInfo.Visible = show
    End Sub

    ''' <summary>
    ''' Shows or hides info message
    ''' </summary>
    Public Sub ShowInfoMessage(ByVal textKey As String, ByVal show As Boolean)
        lblInfo.Visible = show
        lblInfo.TextKey = textKey
    End Sub

    ''' <summary>
    ''' Hides logo and customer info
    ''' </summary>
    Public Sub HideLogoAndCustomerInfo()
        customerLogo.Visible = False
        customerName.Visible = False
    End Sub


End Class
