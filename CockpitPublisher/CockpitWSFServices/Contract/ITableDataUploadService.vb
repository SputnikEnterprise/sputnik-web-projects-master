'------------------------------------
' File: ITableDataUploadService.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Service contract for table upload service.
''' </summary>
<ServiceContract()>
Public Interface ITableDataUploadService

    ''' <summary>
    ''' Receives and processes table data sent by upload client.
    ''' </summary>
    ''' <param name="tableInfo">The table data.</param>
    <OperationContract()>
    Function ProcessTableData(ByVal tableInfo As TableInfo) As Boolean

    ''' <summary>
    ''' Used by upload clients to report about errors.
    ''' </summary>
    ''' <param name="shortDescription">A short error description.</param>
    ''' <param name="exception">The exception trace.</param>
    ''' <param name="customerName">The name of the customer.</param>
    <OperationContract()>
    Sub LogError(ByVal shortDescription As String, ByVal exception As String, ByVal customerName As String)

End Interface

