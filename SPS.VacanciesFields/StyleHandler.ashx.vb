Imports System.Web
Imports System.Web.Services
Imports System.IO

Public Class StyleHandler
  Implements System.Web.IHttpHandler

#Region "Properties"

  ''' <summary>
  ''' Gets or sets the font size in pixel.
  ''' </summary>
  Private Property FontSizeInPixel As Integer = 14

  ''' <summary>
  ''' Gets the IsReusable value.
  ''' </summary>
  ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
    Get
      Return False
    End Get
  End Property

#End Region

#Region "Methods"

  ''' <summary>
  ''' Handles the ProcessRequest event.
  ''' </summary>
  Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

    context.Response.ContentType = "text/plain"


    If Not context.Request.QueryString("fontSize") Is Nothing Then

      Integer.TryParse(context.Request.QueryString("fontSize"), FontSizeInPixel)

    End If

    context.Response.WriteFile(context.Server.MapPath("mycontent.css"))

    WriteStyleCSS(context)
  End Sub


  ''' <summary>
  ''' Writes the style css.
  ''' </summary>
  Private Sub WriteStyleCSS(ByVal context As HttpContext)

    Dim cssTemplate As String = File.ReadAllText(context.Server.MapPath("mycontent.css"))

    ' Replace the fontSize placeholder
    cssTemplate = cssTemplate.Replace("__fontSize__", FontSizeInPixel)

    context.Response.Write(cssTemplate)
    context.Response.Flush()
  End Sub

#End Region

End Class