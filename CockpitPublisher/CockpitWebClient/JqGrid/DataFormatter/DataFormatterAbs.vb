'------------------------------------
' File: DataFromatterAbs.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports log4net
Imports System.Reflection

''' <summary>
''' Abstrat base class for data formatters.
''' </summary>
Public MustInherit Class DataFormatterAbs

    ''' <summary>
    ''' Formats the data of contained in a data table object.
    ''' </summary>
    ''' <param name="tableData">The data table object.</param>
    ''' <returns>jqGrid paged list object</returns>
    Public Function FormatTableData(ByVal tableData As DataTable) As JqGridPagedList

        ' Stores the fromatted row data
        ' Each row is represented as a dictionary of column name/value pairs.
        ' This way the data can be easily serialized in json format.
        Dim formattedRows As New List(Of Dictionary(Of String, String))

        ' Process all rows of the data table object.
        For Each row As DataRow In tableData.Rows

            Dim columData As Dictionary(Of String, String)

            Try
                ' The conrecte formatter implementation does the formatting 
                columData = FormatColumnData(row)
                formattedRows.Add(columData)
            Catch ex As Exception
                Dim logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

                logger.Error("Error while formatting column data on server.", ex)

            End Try
        Next

        ' Create a jqGrid PageList data object (this will be searilized in json format)
        Return New JqGridPagedList(formattedRows, formattedRows.Count, 0, tableData.Rows.Count, Nothing)

    End Function

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' A concrete formatter implementation must override this method.
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected MustOverride Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)

End Class
