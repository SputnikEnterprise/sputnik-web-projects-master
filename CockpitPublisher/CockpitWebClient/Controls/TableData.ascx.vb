'------------------------------------
' File: TableData.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Public Class TableData
    Inherits System.Web.UI.UserControl

    '--Fields--'

    Private m_GridWidth As Integer
    Private m_Float As String
    Private m_TableCaptionText As String
    Private m_TableConfigurtionID As String
    Private m_GridRowInsertedJavaScriptHandler As String
    Private m_IsMobileBrowser As Boolean

    ' --Methods--

    ''' <summary>
    ''' The page load method.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The args.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Float) Then
            wrapperDiv.Style("float") = Float
        End If

        If Not String.IsNullOrEmpty(GridWidth) Then
            wrapperDiv.Style("width") = String.Format("{0}px", GridWidth)
        End If

        If Not String.IsNullOrEmpty(TableConfigurationID) Then
            ' Load the table caption from the table configuration.
            Dim tableConfigurationManager = CType(Application(ApplicationCacheKeys.TableConfigurationManager), TableConfigurationManager)

            If (Not tableConfigurationManager Is Nothing) Then
                Dim tableConfiguration = tableConfigurationManager.GetTableConfigurationById(TableConfigurationID)

                If (Not tableConfiguration Is Nothing) Then
                    tableCaption.Text = tableConfiguration.Title
                End If
            End If

        End If

    End Sub

    ''' <summary>
    ''' Grid width value.
    ''' </summary>
    Public Property GridWidth() As Integer
        Get
            Return m_GridWidth
        End Get

        Set(ByVal value As Integer)
            m_GridWidth = value
        End Set
    End Property

    ''' <summary>
    '''The id of the table configuration.
    ''' </summary>
    Public Property TableConfigurationID() As String
        Get
            Return m_TableConfigurtionID
        End Get

        Set(ByVal value As String)
            m_TableConfigurtionID = value
        End Set
    End Property

    ''' <summary>
    ''' The float value (left or right)
    ''' </summary>
    Public Property Float() As String
        Get
            Return m_Float
        End Get

        Set(ByVal value As String)
            m_Float = value
        End Set
    End Property


    ''' <summary>
    ''' Javscript handler for jqGrid row inserted event.
    ''' This handler can be used to colorize a row for example
    ''' </summary>
    Public Property GridRowInsertedJavaScriptHandler() As String
        Get
            Return m_GridRowInsertedJavaScriptHandler
        End Get

        Set(ByVal value As String)
            m_GridRowInsertedJavaScriptHandler = value
        End Set
    End Property


    ''' <summary>
    ''' Boolean truth value if its a mobile browser.
    ''' </summary>
    Public Property IsMobileBrowser() As String
        Get
            Return m_IsMobileBrowser.ToString().ToLower()
        End Get

        Set(ByVal value As String)
            m_IsMobileBrowser = value
        End Set
    End Property

End Class