'------------------------------------
' File: SqlServerTableInfoPersister.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Data.SqlClient
Imports CockpitPublisher.Common

''' <summary>
''' Persists a table info object in a sql server database.
''' </summary>
Public Class SqlServerTableInfoPersister
    Implements ITableInfoPersister

    '--Constants--

    ''' <summary>
    ''' SQL template used to delete table data. 
    ''' </summary>
    ''' 
    Private Const DB_DELETE_TABLE_DATA_SCRIPT_TEMPLATE As String = "DELETE FROM {0} WHERE MDGuid IN ({1})"

    '--Properties--

    ''' <summary>
    ''' The connection string to connect to the database.
    ''' </summary>
    Public Property ConnectionString As String

    '--Methods--

    ''' <summary>
    ''' Persists the table info object.
    ''' </summary>
    ''' <param name="tableInfo">The table info object.</param>
    Public Sub Persist(ByVal tableInfo As TableInfo) Implements ITableInfoPersister.Persist

        Dim xmlSchema As String
        Dim xmlData As String

        ' Decompress the xml schema
        xmlSchema = Utility.DecompressString(tableInfo.CompressedTableSchema)

        ' Decompress the xml data
        xmlData = Utility.DecompressString(tableInfo.CompressedTableData)

        ' Fill the table with the xml schemadata sent by the client
        FillTable(tableInfo.MDGuids, tableInfo.TableName, xmlSchema, xmlData)

    End Sub

    ''' <summary>
    ''' Fills the table data into a sql server table.
    ''' </summary>
    ''' <param name="mdGuids">The mdGuids.</param>
    ''' <param name="tableName">The table name.</param>
    ''' <param name="xmlSchema">The table schema in xml format.</param>
    ''' <param name="xmlData">The table data in xml format.</param>
    Private Sub FillTable(ByVal mdGuids() As String, ByVal tableName As String, ByVal xmlSchema As String, ByVal xmlData As String)

        Dim ds As New DataSet
        Dim xmlReader As StringReader

        ' Read the xml schema into the dataset.
        xmlReader = New StringReader(xmlSchema)
        ds.ReadXmlSchema(xmlReader)

        ' Read the xml data into a dataset.
        xmlReader = New StringReader(xmlData)
        ds.ReadXml(xmlReader)

        Dim dbCon As SqlConnection = New SqlConnection()
        dbCon.ConnectionString = ConnectionString

        ' A SqlBulkCopy object is used to copy the xml data efficiently to the destination table.
        Dim bulkCopy As New SqlBulkCopy(dbCon)
        bulkCopy.DestinationTableName = "dbo.[" + tableName + "]"

        Dim commaSeparatedGuids As String = ConvertMDGuidsToTSQLString(mdGuids)
        Dim deleteTableDataScript = String.Format(DB_DELETE_TABLE_DATA_SCRIPT_TEMPLATE, bulkCopy.DestinationTableName, commaSeparatedGuids)

        Try
            dbCon.Open()

            Dim command As New SqlCommand(deleteTableDataScript, dbCon)

            ' 1. Execute delete table data script (delete existing data by guid)
            command.ExecuteNonQuery()

            ' 2. Write the table data
            bulkCopy.WriteToServer(ds.Tables(0))

            bulkCopy.Close()

            dbCon.Close()
        Finally
            bulkCopy.Close()
            If (dbCon IsNot Nothing) Then
                dbCon.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Converts a list of md guids to a comma separated TSQL string.
    ''' </summary>
    ''' <param name="mdGuids">The list of the md guids.</param>
    ''' <returns>Comma separated string</returns>
    Private Function ConvertMDGuidsToTSQLString(ByVal mdGuids() As String) As String
        Dim stringBuilder As New StringBuilder

        If (mdGuids.Length = 0) Then
            Return "''"
        End If

        For Each mdGuid In mdGuids
            stringBuilder.Append(String.Format("'{0}',", mdGuid))
        Next

        Dim commaSeparatedString As String = stringBuilder.ToString()

        ' Remove last ','
        Return commaSeparatedString.Substring(0, commaSeparatedString.Length - 1)

    End Function

End Class
