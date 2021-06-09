<%@ WebHandler Language="VB" Class="ImageHandler" %>

Imports System
Imports System.Web
Imports System.Data

Public Class ImageHandler : Implements IHttpHandler
    
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim type As String = context.Request.Params("type")
        Dim id As String = context.Request.Params("id")
        If Not type Is Nothing AndAlso type.Equals("berater") AndAlso Not id Is Nothing Then
            ' The connection to the database
            Dim conn As New System.Data.SqlClient.SqlConnection
            conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString

            ' The SQL Command
            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
            cmd.CommandType = CommandType.Text
            cmd.CommandText = "SELECT User_Picture FROM Customer_Users Where User_ID = @UserId"
            cmd.Parameters.AddWithValue("@UserId", id)
            cmd.Connection = conn
            
            Dim dr As System.Data.SqlClient.SqlDataReader
            Try
                conn.Open()

                ' Executes the reader
                dr = cmd.ExecuteReader()
                If (dr.Read()) Then
                    Dim image As Byte() = dr.Item("User_Picture")
                    context.Response.ContentType = "image/jpeg"
                    context.Response.OutputStream.Write(image, 0, image.Length)
                End If
                dr.Close()
            Finally
                If Not conn.State = ConnectionState.Closed Then
                    conn.Close()
                End If
            End Try
        End If

    End Sub
 
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            ' To be thread safe, use always a new instance for each request.
            Return False
        End Get
    End Property

End Class