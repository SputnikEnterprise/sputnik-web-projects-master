'------------------------------------
' File: DatabaseAccess.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports System.IO
Imports System.Text

''' <summary>
''' Local database access class.
''' </summary>
Public Class DatabaseAccess
    Implements IDatabaseAccess

    '--Fields--

    Private m_Logger As ILogger

    '--Methods--

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New(ByVal logger As ILogger)
        m_Logger = logger
    End Sub

    ''' <summary>
    ''' Reads all table names of sputnik cockpit database.
    ''' </summary>
    ''' <returns>List of table names.</returns>
    Public Function ReadTableNames() As List(Of String) Implements IDatabaseAccess.ReadTableNames

        m_Logger.LogMessageLocal("Reading table names.")

        Dim dbCon As SqlConnection = New SqlConnection()
        Dim myCommand As SqlCommand
        Dim dr As SqlDataReader

        Dim listOfTables As New List(Of String)
        Dim tableName As String

        dbCon.ConnectionString = My.Settings.SputnikCockpit_ConnectionString

        Try
            dbCon.Open()
            myCommand = New SqlCommand("SELECT * FROM sys.Tables", dbCon)
            dr = myCommand.ExecuteReader

            ' Reads all table names
            Do
                While dr.Read()
                    tableName = dr("name")
                    listOfTables.Add(tableName)
                End While
            Loop While dr.NextResult()

        Catch e As Exception
            m_Logger.LogErrorRemote("Error reading table names. [FAILED]", e.ToString())
            Return Nothing
        Finally
            ' Makes sure that the db connection is closed.
            If (dbCon IsNot Nothing) Then
                dbCon.Close()
            End If
        End Try

        m_Logger.LogMessageLocal("Reading table names finished. [OK]")

        Return listOfTables
    End Function

    ''' <summary>
    ''' Reads the data of a table (mdguids, schema, data).
    ''' </summary>
    ''' <param name="tableName">The table name.</param>
    ''' <returns>Table data.</returns>
    Public Function ReadTableInfo(ByVal tableName As String) As XMLTableInfo Implements IDatabaseAccess.ReadTableInfo

        m_Logger.LogMessageLocal("Begin reading table information (mdguids, schema, data).")

        Dim command As New SqlCommand
        Dim dbCon As SqlConnection = New SqlConnection()
        Dim ds As New DataSet

        command.CommandType = CommandType.Text

        dbCon.ConnectionString = My.Settings.SputnikCockpit_ConnectionString

        Dim xmlSchema As String = String.Empty
        Dim xmlData As String = String.Empty

        Dim xmlTableInfo As New XMLTableInfo

        Try
            command.Connection = dbCon
            dbCon.Open()

            ' 1. ---------Read Mandant guids----------

            command.CommandText = String.Format("SELECT DISTINCT MDGuid FROM [{0}] WHERE MDGuid IS NOT NULL", tableName)

            Dim dr As SqlDataReader
            dr = command.ExecuteReader

            ' Reads all md guids
            Do
                While dr.Read()
                    xmlTableInfo.AddMDGuid(dr("MDGuid"))
                End While
            Loop While dr.NextResult()
            dr.Close()


            ' 2. ---------Read table schema and data----------
            command.CommandText = String.Format("SELECT * FROM [{0}]", tableName)
            Dim dataAdapter As New SqlDataAdapter(command)

            ' Fill the dataset with data from the database
            dataAdapter.Fill(ds, "[" + tableName + "]")


            ' Transform the schema to an xml format.
            Dim schemaStringWriter = New StringWriter()
            ds.WriteXmlSchema(schemaStringWriter)
            xmlSchema = schemaStringWriter.ToString()

            ' Tranform the data to an xml format.
            Dim dataStringWriter = New StringWriter()
            ds.WriteXml(dataStringWriter)
            xmlData = dataStringWriter.ToString()


            xmlTableInfo.TableSchema = xmlSchema
            xmlTableInfo.TableData = xmlData

        Catch e As Exception
            m_Logger.LogErrorRemote("Error reading table information (mdguids, schema, data). [FAILED]}", e.ToString())
            Return Nothing
        Finally
            ' Makes sure that the db connection is closed.
            If (dbCon IsNot Nothing) Then
                dbCon.Close()
            End If
        End Try

        m_Logger.LogMessageLocal("Finished reading table information (mdguids, schema, data). [OK]")

        Return xmlTableInfo
    End Function

End Class
