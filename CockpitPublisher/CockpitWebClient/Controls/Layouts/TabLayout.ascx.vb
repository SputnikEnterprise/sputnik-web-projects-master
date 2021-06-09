'------------------------------------
' File: TabLayout.vb
' Date: 26.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Tab layout for desktop browsers.
''' </summary>
Public Class TabLayout
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' The page load event.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The args.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadDesktopBrowserTabs()
    End Sub

    ''' <summary>
    ''' Loads the tabs for a desktop browser.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadDesktopBrowserTabs()
        generalTabPalceholder.Controls.Add(LoadControl("~/Controls/PageContent/DesktopClient/General.ascx"))
        assignmentOfPersonnelTabPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/DesktopClient/AssignmentOfPersonnel.ascx"))
        salesFiguresTabPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/DesktopClient/SalesFigures.ascx"))
        candidatesDataTabPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/DesktopClient/CandidatesData.ascx"))
        customerDataTabPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/DesktopClient/CustomerData.ascx"))
    End Sub

End Class