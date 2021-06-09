'------------------------------------
' File: AccordionLayout.vb
' Date: 26.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Accordion layout for mobile browsers.
''' </summary>
Public Class AccordionLayout
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' The page load event.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The args.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadMobileBrowserControls()
    End Sub

    ''' <summary>
    ''' Loads the content controls for a mobile browser.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadMobileBrowserControls()
        generalPalceholder.Controls.Add(LoadControl("~/Controls/PageContent/MobileClient/GeneralMobile.ascx"))
        assignmentOfPersonnelPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/MobileClient/AssignmentOfPersonnelMobile.ascx"))
        salesFiguresPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/MobileClient/SalesFiguresMobile.ascx"))
        candidatesDataPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/MobileClient/CandidatesDataMobile.ascx"))
        customerDataPlaceholder.Controls.Add(LoadControl("~/Controls/PageContent/MobileClient/CustomerDataMobile.ascx"))
    End Sub

End Class