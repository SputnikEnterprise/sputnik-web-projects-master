'------------------------------------
' File: ColumnConfiguration.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Contains column configuration data which was read from xml file.
''' </summary>
Public Class ColumnConfiguration

    '--Fields--

    Private m_ID As String
    Private m_Title As String
    Private m_DbColumnName As String
    Private m_Type As String
    Private m_ColumnWidth As Integer
    Private m_SortPosition As Integer
    Private m_Align As String
    Private m_Hidden As Boolean
    Private m_JavaScriptColumnFormatterFunctionName As String

    '--Properties--

    ''' <summary>
    ''' The id of the column (must be unique in the contex of a table configuration).
    ''' </summary>
    Public Property ID() As String
        Get
            Return m_ID
        End Get

        Set(ByVal value As String)
            m_ID = value
        End Set
    End Property

    ''' <summary>
    ''' The columns title text.
    ''' </summary>
    Public Property Title() As String
        Get
            Return m_Title
        End Get

        Set(ByVal value As String)
            m_Title = value
        End Set
    End Property

    ''' <summary>
    ''' The columns initial width.
    ''' </summary>
    Public Property ColumnWidth() As Integer
        Get
            Return m_ColumnWidth
        End Get

        Set(ByVal value As Integer)
            m_ColumnWidth = value
        End Set
    End Property

    ''' <summary>
    ''' The table column alignment.
    ''' </summary>
    Public Property Align() As String
        Get
            Return m_Align
        End Get

        Set(ByVal value As String)
            m_Align = value
        End Set
    End Property

    ''' <summary>
    ''' Boolean truth value if column is hidden.
    ''' </summary>
    Public Property Hidden() As Boolean
        Get
            Return m_Hidden
        End Get

        Set(ByVal value As Boolean)
            m_Hidden = value
        End Set
    End Property

    ''' <summary>
    ''' The table column alignment.
    ''' </summary>
    Public Property JavaScriptColumnFormatterFunctionName() As String
        Get
            Return m_JavaScriptColumnFormatterFunctionName
        End Get

        Set(ByVal value As String)
            m_JavaScriptColumnFormatterFunctionName = value
        End Set
    End Property

End Class
