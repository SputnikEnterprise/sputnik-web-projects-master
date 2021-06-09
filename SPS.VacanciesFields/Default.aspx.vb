Imports System.IO

Public Class _Default
  Inherits System.Web.UI.Page

#Region "Private Fields"

  ''' <summary>
  ''' The language.
  ''' </summary>
  Private m_Language As String = "de"

#End Region

#Region "Private Properties"

  ''' <summary>
  ''' Gets ors sets the language.
  ''' </summary>
  Private Property Language As String
    Get
      Return m_Language
    End Get
    Set(value As String)

      If String.IsNullOrEmpty(value) Then
        m_Language = "de"
      End If

      Select Case value.Trim().ToLower
        Case "i"
          m_Language = "it"
        Case "d"
          m_Language = "de"
        Case "f"
          m_Language = "fr"
        Case "de", "fr", "it"
          m_Language = value
        Case Else
          m_Language = "de"
      End Select
    End Set
  End Property

  ''' <summary>
  ''' Gets ors sets the font size in pixel.
  ''' </summary>
  Private Property FontSizeInPixel As Integer = 14

  ''' <summary>
  ''' Gets ors sets height pixel.
  ''' </summary>
  Private Property HeightInPixel As Integer?

#End Region

#Region "Protected Methods"

  ''' <summary>
  ''' Handles page load event.
  ''' </summary>
  Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    ' Check for language parameter.
    If Not Request.QueryString("lang") Is Nothing Then
      Language = Request.QueryString("lang")
    End If

    ' Check for fontsize parameter.
    If Not Request.QueryString("fontSize") Is Nothing Then
      Integer.TryParse(Request.QueryString("fontSize"), FontSizeInPixel)
    End If

    ' Check for height parameter.
    If Not Request.QueryString("height") Is Nothing Then
      Integer.TryParse(Request.QueryString("height"), HeightInPixel)
    End If

    ' Load the tinyMce init script.
    LoadTinyMceInitScript()

    ' Load tinyMce placeholder.
    LoadTinyMcePlaceholder()

  End Sub

#End Region

#Region "Private Fields"

  ''' <summary>
  ''' Loads the tinyMce init script.
  ''' </summary>
  Private Sub LoadTinyMceInitScript()

    Dim tinyMCEInitTemplate As String =
     File.ReadAllText(Server.MapPath("tinyMCEInitTemplate.txt"))

    Dim cssVersion = ConfigurationManager.AppSettings("tinyMCECssVersion")

    ' Increase the version in the Web.config file if changes where made to mycontent.css
    tinyMCEInitPlaceholder.Text = tinyMCEInitTemplate.Replace("__lang__", Language) _
                                                     .Replace("__fontSize__", FontSizeInPixel) _
                                                     .Replace("__version__", cssVersion)

  End Sub

  ''' <summary>
  ''' Loads the tinyMce placeholder.
  ''' </summary>
  Private Sub LoadTinyMcePlaceholder()

    Dim template = "<textarea name=""tinyMceEditor"" cols=""1"" rows=""1"" style=""width:100%; height: __height__""></textarea>"

    If HeightInPixel.HasValue Then
      template = template.Replace("__height__", HeightInPixel + "px")
    Else
      template = template.Replace("__height__", "100%")
    End If

    tinyMceEditorPlaceholder.Text = template

  End Sub

#End Region

End Class