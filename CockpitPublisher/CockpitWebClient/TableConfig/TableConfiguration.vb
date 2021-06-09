'------------------------------------
' File: TableConfiguration.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Contains table configuration data which was read from xml file.
''' </summary>
Public Class TableConfiguration

    '--Fields--

    Private m_ID As String
    Private m_ColumnConfigurations As New List(Of ColumnConfiguration)
    Private m_StoredProcedure As String
    Private m_DataFromatterName As String
    Private m_Title As String

    ''' <summary>
    ''' The table configurations unique id.
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
    ''' The stored procedure that should be called to retrieve the table data.
    ''' </summary>
    Public Property StoredProcedure() As String
        Get
            Return m_StoredProcedure
        End Get

        Set(ByVal value As String)
            m_StoredProcedure = value
        End Set
    End Property

    ''' <summary>
    ''' The name of the data formatter class.
    ''' </summary>
    Public Property DataFormatternName() As String
        Get
            Return m_DataFromatterName
        End Get

        Set(ByVal value As String)
            m_DataFromatterName = value
        End Set
    End Property

    ''' <summary>
    ''' Title text used for table header.
    ''' </summary>
    Public Property Title() As String
        Get
            Return m_Title
        End Get

        Set(ByVal value As String)
            m_Title = value
        End Set
    End Property

    '--Methods--

    ''' <summary>
    ''' Adds a column configuration.
    ''' </summary>
    Public Sub AddColumnConfigurations(ByVal columnConfiguration As ColumnConfiguration)
        m_ColumnConfigurations.Add(columnConfiguration)
    End Sub

    ''' <summary>
    ''' Gets the table column names.
    ''' </summary>
    ''' <returns>Column names as string array.</returns>
    Public Function TableColumnNames() As List(Of String)

        Dim columnNames As New List(Of String)

        ' Read the column names from the table configuration
        For Each columnConfig As ColumnConfiguration In m_ColumnConfigurations
            columnNames.Add(columnConfig.Title)
        Next

        Return columnNames
    End Function

    Public Function ConvertColumnModelsToJqGridColumnModels() As List(Of JqGridColumnModel)
        Dim jqGridColumnModels As New List(Of JqGridColumnModel)

        ' Convert all colum to jqgrid column data objects.
        For Each columnConfig As ColumnConfiguration In m_ColumnConfigurations
            Dim columnModel As New JqGridColumnModel()
            columnModel.Name = columnConfig.ID
            columnModel.Width = columnConfig.ColumnWidth
            columnModel.Sortable = False
            columnModel.Align = columnConfig.Align
            columnModel.Formatter = columnConfig.JavaScriptColumnFormatterFunctionName
            columnModel.Hidden = columnConfig.Hidden
            jqGridColumnModels.Add(columnModel)
        Next

        Return jqGridColumnModels
    End Function


End Class
