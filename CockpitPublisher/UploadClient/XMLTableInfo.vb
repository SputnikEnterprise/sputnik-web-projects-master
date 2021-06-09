'------------------------------------
' File: XMLTableInfo.vb
' Date: 02.11.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Table information. 
''' </summary>
Public Class XMLTableInfo

    ' --Fields--

    Private m_TableSchema As String
    Private m_TableData As String
    Private m_MDGuids As New List(Of String)

    ' --Methods--

    ''' <summary>
    ''' Adds a mdguid.
    ''' </summary>
    Public Sub AddMDGuid(ByVal mdGuid As String)
        m_MDGuids.Add(mdGuid)
    End Sub

    ' --Properties--

    ''' <summary>
    ''' The table schema.
    ''' </summary>
    Public Property TableSchema() As String
        Get
            Return m_TableSchema
        End Get

        Set(ByVal value As String)
            m_TableSchema = value
        End Set
    End Property

    ''' <summary>
    ''' The table data.
    ''' </summary>
    Public Property TableData() As String
        Get
            Return m_TableData
        End Get

        Set(ByVal value As String)
            m_TableData = value
        End Set
    End Property

    ''' <summary>
    ''' The Mandant guids
    ''' </summary>
    Public ReadOnly Property MDGuids() As String()
        Get
            Return m_MDGuids.ToArray()
        End Get
    End Property

End Class
