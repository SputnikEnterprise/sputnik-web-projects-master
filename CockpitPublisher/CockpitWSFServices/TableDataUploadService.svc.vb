'------------------------------------
' File: TableDataUploadService.svc.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Reflection
Imports log4net
Imports log4net.Config

''' <summary>
''' Service used to upload table data of Sputnik Cockpit databases.
''' </summary>
Public Class TableDataUploadService
    Implements ITableDataUploadService

    '--Fields--

    ''' <summary>
    ''' Log4Net logger
    ''' </summary>
    Private logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

    '--Methods--

    ''' <summary>
    ''' Constructor
    ''' </summary>
    Public Sub New()
        ' Configure log4net
        XmlConfigurator.Configure()
    End Sub

    ''' <summary>
    ''' Processes table data sent by upload client.
    ''' </summary>
    ''' <param name="tableInfo">The table data.</param>
    ''' <returns>True on scuccess, otherwhise false.</returns>
    Function ProcessTableData(ByVal tableInfo As TableInfo) As Boolean Implements ITableDataUploadService.ProcessTableData
        Try
            Dim sqlServerTableInfoPersister As New SqlServerTableInfoPersister
            sqlServerTableInfoPersister.ConnectionString = My.Settings.SputnikCockpit_ConnectionString

            ' Persist the table information to the sql server database.
            sqlServerTableInfoPersister.Persist(tableInfo)
        Catch e As Exception
            LogError("Table data sent by customer could not be persisted on the server.", e.ToString(), tableInfo.CustomerName)
            Return False
        End Try

        Return True
    End Function

    ''' <summary>
    ''' Used by upload clients to report about errors.
    ''' </summary>
    ''' <param name="shortDescription">A short error description.</param>
    ''' <param name="exception">The exception trace.</param>
    ''' <param name="customerName">The name of the customer.</param>
    Sub LogError(ByVal shortDescription As String, ByVal exception As String, ByVal customerName As String) Implements ITableDataUploadService.LogError
        logger.Error(String.Format("Customer={0}, ErrorMessage={1}. Exception={2}", customerName, shortDescription, exception))
    End Sub

End Class
