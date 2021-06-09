'---------------------------------------------------------
' ZHDNav.ascx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

''' <summary>
''' Navmenu for 'ZHD' query parameter
''' </summary>
Partial Class ZHDNav
    Inherits NavigationBase

    Public Overrides Sub InitNavigationMenu()
        ' Retrievs application info object
        Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
        Dim zhdQueryParameter As String = Request(appInfo.ZHDQueryParameterName)

        hlnkVerleihvertraege.NavigateUrl = String.Format("~/Kunden/Verleihvertraege.aspx?{0}={1}", appInfo.ZHDQueryParameterName, zhdQueryParameter)
        hlnkOffeneVerleihvertraege.NavigateUrl = String.Format("~/Kunden/OffeneVerleihvertraege.aspx?{0}={1}", appInfo.ZHDQueryParameterName, zhdQueryParameter)

		hlnkProposal.NavigateUrl = String.Format("~/Kunden/AllPropose.aspx?{0}={1}", appInfo.ZHDQueryParameterName, zhdQueryParameter)
		hlnkNewProposal.NavigateUrl = String.Format("~/Kunden/NewPropose.aspx?{0}={1}", appInfo.ZHDQueryParameterName, zhdQueryParameter)

    'hlnkJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=all", appInfo.ZHDQueryParameterName, zhdQueryParameter)
    '      If (Me.Page.AppRelativeVirtualPath = "~/Kunden/JobKandidaten.aspx") Then
    '          'Job Candidates all
    '          hlnkAlleJobKandidaten.Visible = True
    '          hlnkAlleJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=all", appInfo.ZHDQueryParameterName, zhdQueryParameter)
    '          'Job Candidates fits to me
    '          hlnkMeineJobKandidaten.Visible = True
    '          hlnkMeineJobKandidaten.NavigateUrl = String.Format("~/Kunden/JobKandidaten.aspx?{0}={1}&tp=mine", appInfo.ZHDQueryParameterName, zhdQueryParameter)
    '      End If

    hlnkRapporte.NavigateUrl = String.Format("~/Kunden/Rapporte.aspx?{0}={1}", appInfo.ZHDQueryParameterName, zhdQueryParameter)

	End Sub

End Class
