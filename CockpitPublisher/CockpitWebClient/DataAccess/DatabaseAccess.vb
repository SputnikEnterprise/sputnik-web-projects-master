'------------------------------------
' File: DatabaseAccess.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Data.SqlClient
Imports log4net
Imports System.Reflection

''' <summary>
''' Databasse access class.
''' </summary>
Public Class DatabaseAccess

    ''' <summary>
    ''' Reads table data with the use of a stored procedure.
    ''' </summary>
    ''' <param name="storedProcedure">The name of the stored procedure.</param>
    ''' <param name="parameters">Dictionary of parameter / value pairs.</param>
    ''' <returns>Data table object.</returns>
    Public Function GetTableDataWidthStoredProcedure(ByVal storedProcedure As String, ByVal parameters As Dictionary(Of String, String)) As DataTable

        Dim dbCon As SqlConnection = New SqlConnection()
        Dim myCommand As SqlCommand
        Dim listOfTables As New List(Of String)
        Dim dt As New DataTable

        dbCon.ConnectionString = My.Settings.Sputnik_CockpitServer_ConnectionString

        Try
            myCommand = New SqlCommand()
            myCommand.CommandText = storedProcedure
            myCommand.CommandType = CommandType.StoredProcedure
            myCommand.Connection = dbCon

            For Each key As String In parameters.Keys
                myCommand.Parameters.AddWithValue(key, parameters(key))
            Next

            dbCon.Open()

            Dim dataAdapter As New SqlDataAdapter(myCommand)

            ' Fill the dataset with data from the database
            dataAdapter.Fill(dt)

        Catch e As Exception
            Dim logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

            Dim messageBuffer As New StringBuilder

            messageBuffer.Append(String.Format("Error while reading data from database with stored procedure {0}.", storedProcedure))
            messageBuffer.AppendLine()
            messageBuffer.Append("Parameters:")
            messageBuffer.AppendLine()

            For Each key As String In parameters.Keys
                messageBuffer.Append(String.Format("Parameter-Key: {0}, Paramerter-Value: {1}", key, parameters(key)))
            Next

            logger.Error(messageBuffer.ToString(), e)

            Return Nothing
        Finally
            ' Makes sure that the db connection is closed.
            If (dbCon IsNot Nothing) Then
                dbCon.Close()
            End If
        End Try

        Return dt

    End Function

End Class
