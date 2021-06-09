'------------------------------------
' File: JqGridTableData.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Encapsulates all required table data that is needed by the client side javascript code to
''' create a table whit the jqGrid plugin.
''' </summary>
Public Class JqGridTableData

    ' -- Fields --

    Dim m_ColumnNamesJsonData As String
    Dim m_PagedListJsonData As String
    Dim m_ColumnModlesJsonData As String

    ' -- Methdos --

    ''' <summary>
    ''' Column names as json string.
    ''' </summary>
    ''' <value></value>
    Public Property ColumnNamesJson() As String
        Get
            Return m_ColumnNamesJsonData
        End Get
        Set(ByVal value As String)
            m_ColumnNamesJsonData = value
        End Set

    End Property

    ''' <summary>
    ''' jqGrid column models as json string.
    ''' </summary>
    ''' <value></value>
    Public Property ColumnModelsJson() As String
        Get
            Return m_ColumnModlesJsonData
        End Get
        Set(ByVal value As String)
            m_ColumnModlesJsonData = value
        End Set
    End Property

    ''' <summary>
    ''' jqGrid paged list data as json string.
    ''' </summary>
    ''' <value></value>
    Public Property PagedListJson() As String
        Get
            Return m_PagedListJsonData
        End Get
        Set(ByVal value As String)
            m_PagedListJsonData = value
        End Set
    End Property

End Class
