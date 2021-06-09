'------------------------------------
' File: MyCockpit.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports CockpitPublisher.Common

''' <summary>
''' My cokpit web client page.
''' </summary>
Public Class MyCockpit
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' The page load mehtod.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The args.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim userAgent As String = Request.ServerVariables("HTTP_USER_AGENT")

        ' Display different tab layout if its a mobile browser.
        If Utility.IsMobileAgent(userAgent) Then
            LoadAccordionLayout()

            RegisterMobileCssChanges()

        Else
            LoadTabLayout()
        End If

    End Sub

    ''' <summary>
    ''' Loads the tab layout for desktop browsers.
    ''' </summary>
    Private Sub LoadTabLayout()
        layoutPalceholder.Controls.Add(LoadControl("~/Controls/Layouts/TabLayout.ascx"))
    End Sub

    ''' <summary>
    ''' Loads the accordion layout for mobile browsers.
    ''' </summary>
    Private Sub LoadAccordionLayout()
        layoutPalceholder.Controls.Add(LoadControl("~/Controls/Layouts/AccordionLayout.ascx"))
    End Sub

    ''' <summary>
    ''' Registers css file with changes for mobile browsers.
    ''' </summary>
    Private Sub RegisterMobileCssChanges()
        Dim cssLink As String = String.Format("<link href=""{0}"" rel=""stylesheet"" type=""text/css"" />", ResolveClientUrl("~/Styles/MobileChanges.css"))
        Dim literal As New LiteralControl(cssLink)
        additionalCSSPlaceholder.Controls.Add(literal)
    End Sub


End Class