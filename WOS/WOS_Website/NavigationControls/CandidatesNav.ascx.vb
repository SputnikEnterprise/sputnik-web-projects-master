'---------------------------------------------------------
' CandidatesNav.ascx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

''' <summary>
''' Navmenu for 'sp' query parameter
''' </summary>
Partial Class CandidatesNav
    Inherits NavigationBase

    Public Overrides Sub InitNavigationMenu()

        ' Retrievs application info object
        Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
        Dim candidateQueryParameter As String = Request(appInfo.CandidateQueryParameterName)

        'Arbeitsbescheinigung
        hlnkArbeitsbescheinigung.Visible = True
        hlnkArbeitsbescheinigung.NavigateUrl = String.Format("~/Kandidaten/Arbeitsbescheinigung.aspx?{0}={1}", appInfo.CandidateQueryParameterName, candidateQueryParameter)

		'Offene Stellen
		hlnkVakanzen.Visible = False
		hlnkVakanzen.NavigateUrl = String.Format("~/Kandidaten/Vakanzen.aspx?{0}={1}&tp=all", appInfo.CandidateQueryParameterName, candidateQueryParameter)

        If (Me.Page.AppRelativeVirtualPath = "~/Kandidaten/Vakanzen.aspx") Then
			'Offene Stellen Alle
			hlnkAlleVakanzen.Visible = False
			hlnkAlleVakanzen.NavigateUrl = String.Format("~/Kandidaten/Vakanzen.aspx?{0}={1}&tp=all", appInfo.CandidateQueryParameterName, candidateQueryParameter)
			'Offene Stellen Zu mir passend
			hlnkMeineVakanzen.Visible = False
			hlnkMeineVakanzen.NavigateUrl = String.Format("~/Kandidaten/Vakanzen.aspx?{0}={1}&tp=mine", appInfo.CandidateQueryParameterName, candidateQueryParameter)
        End If

        'Zwischenverdienstformulare
        hlnkZwischenverdienstformulare.Visible = True
        hlnkZwischenverdienstformulare.NavigateUrl = String.Format("~/Kandidaten/Zwischenverdienstformulare.aspx?{0}={1}", appInfo.CandidateQueryParameterName, candidateQueryParameter)

        'Lohnabrechnungen
        hlnkLohnabrechnungen.Visible = True
        hlnkLohnabrechnungen.NavigateUrl = String.Format("~/Kandidaten/Lohnabrechnungen.aspx?{0}={1}", appInfo.CandidateQueryParameterName, candidateQueryParameter)

        'Lohnausweis
        hlnkLohnausweis.Visible = True
        hlnkLohnausweis.NavigateUrl = String.Format("~/Kandidaten/Lohnausweis.aspx?{0}={1}", appInfo.CandidateQueryParameterName, candidateQueryParameter)

        'Einsatzvertraege
        hlnkEinsatzvertraege.Visible = True
        hlnkEinsatzvertraege.NavigateUrl = String.Format("~/Kandidaten/Einsatzvertraege.aspx?{0}={1}", appInfo.CandidateQueryParameterName, candidateQueryParameter)

        'Rapporte
        hlnkMARapporte.Visible = True
        hlnkMARapporte.NavigateUrl = String.Format("~/Kandidaten/MARapporte.aspx?{0}={1}&tp=all", appInfo.CandidateQueryParameterName, candidateQueryParameter)

    End Sub

End Class
