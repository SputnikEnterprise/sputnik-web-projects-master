'------------------------------------
' File: JqGridColumnModel.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Runtime.Serialization

''' <summary>
''' JqGrid column model.
''' </summary>
<DataContract()>
Public Class JqGridColumnModel

    '--Fields--

    Dim m_Name As String
    Dim m_Width As Integer
    Dim m_Sortable As Boolean
    Dim m_Align As String
    Dim m_Formatter As String
    Dim m_Hidden As Boolean

    '--Methods--

    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="columnName">The column name</param>
    ''' <param name="columnWidth">The column width</param>
    ''' <param name="isColumnSortable">Flag if column is sortable</param>
    Public Sub New(ByVal columnName As String, ByVal columnWidth As Integer, ByVal isColumnSortable As Boolean)
        m_Name = columnName
        m_Width = columnWidth
        m_Sortable = isColumnSortable
    End Sub

    ''' <summary>
    ''' The Column name.
    ''' </summary>
    <DataMember(Name:="name")>
    Public Property Name() As String
        Get
            Return m_Name
        End Get

        Set(ByVal value As String)
            m_Name = value
        End Set
    End Property

    ''' <summary>
    ''' The width of the column.
    ''' </summary>
    <DataMember(Name:="width")>
    Public Property Width() As Integer
        Get
            Return m_Width
        End Get
        Set(ByVal value As Integer)
            m_Width = value
        End Set
    End Property

    ''' <summary>
    ''' Flag if column is sortable.
    ''' </summary>
    <DataMember(Name:="sortable")>
    Public Property Sortable() As Boolean
        Get
            Return m_Sortable
        End Get
        Set(ByVal value As Boolean)
            m_Sortable = value
        End Set
    End Property

    ''' <summary>
    ''' Alignment (left, center, right) of the column.
    ''' </summary>
    <DataMember(Name:="align")>
    Public Property Align() As String
        Get
            Return m_Align
        End Get

        Set(ByVal value As String)
            m_Align = value
        End Set
    End Property

    ''' <summary>
    ''' Column formatter function.
    ''' </summary>
    <DataMember(Name:="formatter")>
    Public Property Formatter() As String
        Get
            Return m_Formatter
        End Get

        Set(ByVal value As String)
            m_Formatter = value
        End Set
    End Property

    ''' <summary>
    ''' Flag if column is hidden.
    ''' </summary>
    <DataMember(Name:="hidden")>
    Public Property Hidden() As Boolean
        Get
            Return m_Hidden
        End Get
        Set(ByVal value As Boolean)
            m_Hidden = value
        End Set
    End Property

End Class
