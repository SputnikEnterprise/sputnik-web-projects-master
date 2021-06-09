'------------------------------------
' File: IDatabaseAcceess.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Infterface for local database access implementation.
''' </summary>
''' <remarks></remarks>
Public Interface IDatabaseAccess

    ''' <summary>
    ''' Reads table names from database.
    ''' </summary>
    ''' <returns>List of table names.</returns>
    Function ReadTableNames() As List(Of String)

    ''' <summary>
    ''' Reads the mdguids, schema and data of a table.
    ''' </summary>
    ''' <param name="tableName">The table name.</param>
    ''' <returns>Table information (mdguids, data and schema)</returns>
    Function ReadTableInfo(ByVal tableName As String) As XMLTableInfo

End Interface
