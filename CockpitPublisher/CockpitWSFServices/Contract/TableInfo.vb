'------------------------------------
' File: TableInfo.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data transfer object used by the upload clients to transfer table information to the service.
''' </summary>
<DataContract()>
Public Class TableInfo

    ''' <summary>
    ''' The table name.
    ''' </summary>
    <DataMember()>
    Public Property TableName As String

    ''' <summary>
    ''' The compressed table data.
    ''' </summary>
    <DataMember()>
    Public Property CompressedTableData As String

    ''' <summary>
    ''' The compressed table schema.
    ''' </summary>
    <DataMember()>
    Public Property CompressedTableSchema As String

    ''' <summary>
    ''' The name of the customer.
    ''' </summary>
    <DataMember()>
    Public Property CustomerName As String

    ''' <summary>
    ''' The array of Mandant guids.
    ''' </summary>
    <DataMember()>
    Public Property MDGuids As String()

End Class
