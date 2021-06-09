'------------------------------------
' File: JqGridPagedList.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization

''' <summary>
'''  JqGrid paged lis object.
'''  Containts information about a grid and its data.
''' </summary>
Public Class JqGridPagedList

    '--Fields--

    Dim m_Rows As List(Of Dictionary(Of String, String))
    Dim m_TotalRecords As Integer
    Dim m_PageIndex As Integer
    Dim m_PageSize As Integer
    Dim m_UserData As Object

    '--Constructors--

    ''' <summary>
    ''' Default Constructor
    ''' </summary>
    Public Sub New()
    End Sub


    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="rows">The data rows for jqGrid.</param>
    ''' <param name="totalRecords">The total records count.</param>
    ''' <param name="pageIndex">The page index.</param>
    ''' <param name="pageSize">The page size.</param>
    ''' <param name="userData">Additional user data.</param>
    Public Sub New(ByVal rows As List(Of Dictionary(Of String, String)), ByVal totalRecords As Integer, ByVal pageIndex As Integer, ByVal pageSize As Integer, ByVal userData As Object)
        m_Rows = rows
        m_TotalRecords = totalRecords
        m_PageIndex = pageIndex
        m_PageSize = pageSize
        m_UserData = userData
    End Sub

    '--Properties--

    ''' <summary>
    ''' Total pages.
    ''' </summary>
    Public Property total() As Integer
        Get
            If (m_PageSize = 0) Then
                Return 0
            End If

            Return CType(Math.Ceiling(CType(m_TotalRecords, Decimal) / CType(m_PageSize, Decimal)), Decimal)
        End Get
        Set(ByVal value As Integer)
        End Set
    End Property

    ''' <summary>
    ''' Current page.
    ''' </summary>
    Public Property page() As Integer
        Get
            Return m_PageIndex
        End Get

        Set(ByVal value As Integer)
            m_PageIndex = value
        End Set
    End Property

    ''' <summary>
    ''' Number of records.
    ''' </summary>
    Public Property records() As Integer
        Get
            Return m_TotalRecords
        End Get

        Set(ByVal value As Integer)
            m_TotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' The row data.
    ''' </summary>
    Public Property rows() As List(Of Dictionary(Of String, String))
        Get
            Return m_Rows
        End Get

        Set(ByVal value As List(Of Dictionary(Of String, String)))
            m_Rows = value
        End Set
    End Property

    ''' <summary>
    ''' Additional user data.
    ''' </summary>
    Public Property userData() As Object
        Get
            Return m_UserData
        End Get

        Set(ByVal value As Object)
            m_UserData = value
        End Set
    End Property

End Class
