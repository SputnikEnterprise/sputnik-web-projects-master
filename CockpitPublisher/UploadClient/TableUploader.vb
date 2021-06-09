'------------------------------------
' File: TableUploader.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Text
Imports UploadClient.CockpitWCFServices
Imports System.Text.RegularExpressions
Imports CockpitPublisher.Common

''' <summary>
''' Uploads local databse table 
''' </summary>
Public Class TableUploader

    '--Constants--

    ' Used to check that only tables which end with '__T' are transmitted to web service.
    Private Const VALID_TABLE_NAME_PATTERN As String = "__T$"

    '--Fields--

    Private m_DatabseAccess As IDatabaseAccess
    Private m_Logger As ILogger
    Private m_UploadService As ITableDataUploadService

    '--Methods--

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="uploadService">The upload service implementation</param>
    ''' <param name="databaseAccess">The database access implementation</param>
    ''' <param name="logger">The logger implementation</param>
    Public Sub New(ByVal uploadService As ITableDataUploadService, ByVal databaseAccess As IDatabaseAccess, ByVal logger As ILogger)
        m_UploadService = uploadService
        m_DatabseAccess = databaseAccess
        m_Logger = logger
    End Sub

    ''' <summary>
    ''' Uploads local sputnik cockpit database tables to the server.
    ''' </summary>
    Public Sub UploadTables()

        Dim i As Integer
        Dim tableName As String
        Dim tableXMLInfo As XMLTableInfo
        Dim filteredListOfTables As New List(Of String)
        Dim listOfFailedTableUploads As New List(Of String)

        m_Logger.LogMessageLocal(String.Format("{0}{0}{0}{0}", System.Environment.NewLine))

        m_Logger.LogMessageLocal(String.Format("Upload Tables started (local time {0})", DateTime.Now.ToString))

        Try
            ' Step 1: Reads all tables of local sputnik cockpit database.
            Dim listOfTables = m_DatabseAccess.ReadTableNames()

            If (listOfTables Is Nothing) Then
                ' Table names could not be read -> end upload table proccess
                m_Logger.LogErrorRemote("Upload proccess stopped because table names could not be read.", String.Empty)
                Return
            End If

            ' Step 2: Filter out tables that should not be copied.
            filteredListOfTables = FilterInvalidTables(listOfTables)

            m_Logger.LogMessageLocal(System.Environment.NewLine)

            ' Step 3: Process the filtered tables
            For i = 0 To filteredListOfTables.Count - 1

                tableName = filteredListOfTables(i)

                m_Logger.LogMessageLocal(String.Format("Table: {0}", tableName))

                ' Step 3.1: Read the table info (mdguids, schema and data)
                tableXMLInfo = m_DatabseAccess.ReadTableInfo(tableName)

                If tableXMLInfo Is Nothing Then
                    ' Table date could not be read -> continue with next table
                    m_Logger.LogErrorRemote(String.Format("Upload proccess for table {0} skipped because mdguids, table schema or data could not be read.", tableName), String.Empty)
                    Continue For
                End If

                ' Step 3.2: Fill a table info object with the table data.
                Dim tableInfo As New TableInfo()
                tableInfo.TableName = tableName
                tableInfo.CustomerName = My.Settings.CustomerName
                tableInfo.MDGuids = tableXMLInfo.MDGuids
                tableInfo.CompressedTableSchema = Utility.Compress(tableXMLInfo.TableSchema)
                tableInfo.CompressedTableData = Utility.Compress(tableXMLInfo.TableData)

                ' Step 3.3 Upload the table data
                If Not UploadTableInfo(tableInfo) Then
                    listOfFailedTableUploads.Add(tableName)
                End If

                m_Logger.LogMessageLocal(System.Environment.NewLine)
            Next
        Catch e As Exception
            m_Logger.LogErrorRemote("Unexpected error happend. [FAILED]", e.ToString)
        End Try

        ' Log upload summary.
        If listOfFailedTableUploads.Count = 0 Then
            m_Logger.LogMessageLocal("Upload process finished succesfully.")
        Else
            m_Logger.LogMessageLocal("Upload process finished with errors.")
            m_Logger.LogMessageLocal("The following tables could not be uploaded:")

            ' Write the names of the tables that failed
            For Each failedTableName As String In listOfFailedTableUploads
                m_Logger.LogMessageLocal(failedTableName)
            Next
            m_Logger.LogMessageLocal(System.Environment.NewLine)
        End If
        m_Logger.LogMessageLocal(String.Format("Successfully uploaded {0} of {1} tables.", filteredListOfTables.Count - listOfFailedTableUploads.Count, filteredListOfTables.Count))

    End Sub

    ''' <summary>
    ''' Uploads the table information to the server
    ''' </summary>
    ''' <param name="tableInfo">The table info object.</param>
    Private Function UploadTableInfo(ByVal tableInfo As TableInfo) As Boolean

        m_Logger.LogMessageLocal("Begin uploading table data.")

        Dim success As Boolean = True
        Try
            ' Uploads the table information (it is excpected that the table is existing on the remote server).
            success = m_UploadService.ProcessTableData(tableInfo)

        Catch e As Exception
            m_Logger.LogErrorRemote("Error while calling upload service. [FAILED]", e.ToString())

            Return False
        End Try

        If success Then
            m_Logger.LogMessageLocal("Finished uploading table info. [OK]")
        Else
            m_Logger.LogMessageLocal("Upload failed. [FAILED]")
        End If

        Return success

    End Function

    ''' <summary>
    ''' Filters invalid table names.
    ''' </summary>
    ''' <param name="allTableNames">The list of all table names</param>
    ''' <returns>Filtered list of template table names.</returns>
    Private Function FilterInvalidTables(ByVal allTableNames As List(Of String)) As List(Of String)

        Dim filteredTables As New List(Of String)

        For Each tableName As String In allTableNames
            ' Table name must contain a GUID a the end
            If Regex.Match(tableName, VALID_TABLE_NAME_PATTERN).Success Then
                filteredTables.Add(tableName)
            End If
        Next

        Return filteredTables
    End Function

End Class
