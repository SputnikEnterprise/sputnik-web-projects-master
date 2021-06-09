'---------------------------------------------------------
' KDNav.ascx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

''' <summary>
''' Navmenu for 'KD' query parameter
''' </summary>
Partial Class KDNav
	Inherits NavigationBase

	Public Overrides Sub InitNavigationMenu()
		' Retrievs application info object
		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
		Dim customerQueryParameter As String = Request(appInfo.KDQueryParameterName)

		If AllowedToShowContractLink() Then
			hlnkVerleihvertraege.NavigateUrl = String.Format("~/Kunden/Verleihvertraege.aspx?{0}={1}", appInfo.KDQueryParameterName, customerQueryParameter)
			hlnkOffeneVerleihvertraege.NavigateUrl = String.Format("~/Kunden/OffeneVerleihvertraege.aspx?{0}={1}", appInfo.KDQueryParameterName, customerQueryParameter)

      'hlnkProposal.NavigateUrl = String.Format("~/Kunden/ALLPropose.aspx?{0}={1}", appInfo.ZHDQueryParameterName, customerQueryParameter)
      'hlnkNewProposal.NavigateUrl = String.Format("~/Kunden/NewPropose.aspx?{0}={1}", appInfo.ZHDQueryParameterName, customerQueryParameter)

    Else
			hlnkVerleihvertraege.Visible = False
			hlnkOffeneVerleihvertraege.Visible = False

      'hlnkProposal.Visible = False
      'hlnkNewProposal.Visible = False
    End If

    'hlnkJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=all", appInfo.KDQueryParameterName, customerQueryParameter)
    'If (Me.Page.AppRelativeVirtualPath = "~/Kunden/JobKandidaten.aspx") Then
    '	'Job Candidates all
    '	hlnkAlleJobKandidaten.Visible = True
    '	hlnkAlleJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=all", appInfo.KDQueryParameterName, customerQueryParameter)
    '	'Job Candidates fits to me
    '	hlnkMeineJobKandidaten.Visible = True
    '	hlnkMeineJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=mine", appInfo.KDQueryParameterName, customerQueryParameter)
    'End If

    hlnkRapporte.NavigateUrl = String.Format("~/Kunden/Rapporte.aspx?{0}={1}", appInfo.KDQueryParameterName, customerQueryParameter)
		hlnkRechnungen.NavigateUrl = String.Format("~/Kunden/Rechnungen.aspx?{0}={1}", appInfo.KDQueryParameterName, customerQueryParameter)
	End Sub

	''' <summary>
	''' Shows the FrameworkAgreement
	''' </summary>
	Private Function AllowedToShowContractLink()
		Dim result As Boolean = True

		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
		Dim customerQueryParameter As String = Request(appInfo.KDQueryParameterName)

		If appInfo.CurrentGuidName.ToLower = "kd" Then
			' SQL Command
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
			cmd.CommandType = CommandType.Text
			cmd.CommandText = "SELECT TOP 1 IsNull(EnableContractLink, 1) EnableContractLink FROM Kunden Where KD_Guid = @KD_Guid"
			Dim param As System.Data.SqlClient.SqlParameter
			param = cmd.Parameters.AddWithValue("@KD_Guid", customerQueryParameter)
			Dim conn As New System.Data.SqlClient.SqlConnection
			Dim dataReader As System.Data.SqlClient.SqlDataReader = Nothing
			Try
				conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString()
				' Open connection to database and read data
				conn.Open()
				cmd.Connection = conn
				dataReader = cmd.ExecuteReader()
				dataReader.Read()
				If dataReader.HasRows Then
					result = Utility.SafeGetBoolean(dataReader, "EnableContractLink", True)
				End If
				dataReader.Close()
			Finally
				conn.Close()
				conn.Dispose()
			End Try
		End If

		Return result

	End Function

End Class
