'------------------------------------
' File: ITableInfoPersister.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Interface that must be implemented by a specific table info persisting implementation.
''' </summary>
Public Interface ITableInfoPersister

    ''' <summary>
    ''' Persists the table info object.
    ''' </summary>
    ''' <param name="tableInfo">The table info object.</param>
    Sub Persist(ByVal tableInfo As TableInfo)

End Interface
