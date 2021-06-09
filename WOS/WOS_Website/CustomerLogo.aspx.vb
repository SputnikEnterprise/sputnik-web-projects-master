'---------------------------------------------------------
' CustomerLogo.aspx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports System.Drawing
Imports System.IO
Imports System.Data

''' <summary>
''' Sends the customer logo to client browser.
''' </summary>
Partial Class CustomerLogo
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadLogo()
    End Sub

    ''' <summary>
    ''' Sends the customer logo to client browser.
    ''' </summary>
    Public Sub LoadLogo()

        ' The connection to the database
        Dim conn As New System.Data.SqlClient.SqlConnection
        conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString

        ' The SQL Command
        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
        cmd.CommandType = CommandType.Text
        cmd.CommandText = "SELECT TOP 1 Customer_Logo FROM MySetting Where ID = @CustomerId"
        cmd.Connection = conn

        ' The customerId is stored in the session
        Dim customerId As String = Session("CustomerId")

        ' Make sure that a customerId is set
        If Not customerId Is Nothing Then

            Dim param As System.Data.SqlClient.SqlParameter
            param = cmd.Parameters.AddWithValue("@CustomerId", customerId)

            Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
            Try
                ' Open connection to databse an read logo data
                conn.Open()
                dr = cmd.ExecuteReader()
                dr.Read()

                Dim logoData As Byte() = dr.Item("Customer_Logo")

                ' Send the logo to the client
                Utility.SendFileToCustomer(Response, "image/png", logoData, False, String.Empty)

            Finally
                dr.Close()
                conn.Close()
                conn.Dispose()
            End Try

        End If

    End Sub

End Class
