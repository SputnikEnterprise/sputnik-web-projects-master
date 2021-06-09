<%@ WebHandler Language="VB" Class="FrameworkAgreement" %>

Imports System
Imports System.Web
Imports System.Data
Imports WOS_Website

''' <summary>
''' FrameworkAgreement handler
''' </summary>
Public Class FrameworkAgreement : Implements IHttpHandler
    Implements SessionState.IReadOnlySessionState

    ' Constructor
    Public Sub New()
    End Sub


    ''' <summary>
    '''Entry point for handler request
    ''' </summary>
    Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
        Dim appInfo As WOS_Website.ApplicationInfo = CType(HttpContext.Current.Session(WOS_Website.ApplicationInfo.SESSION_KEY), WOS_Website.ApplicationInfo)
        Dim hasPdf As Boolean = False

        If appInfo.CurrentGuidName.ToLower = "sp" Then
            ' Connection to the database
            Dim conn As New System.Data.SqlClient.SqlConnection
            conn.ConnectionString() = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString()

            ' SQL Command
            Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
            cmd.CommandType = CommandType.Text

            Dim strQuery As String = String.Empty
            strQuery = "SELECT TOP 1 MA.MA_Language As Language, "
            strQuery &= "MA.Customer_ID, "
            strQuery &= "Setting.Rahmenvertrag As Rahmenvertrag_D, "
            strQuery &= "IsNull(Setting.Rahmenvertrag_I, Setting.Rahmenvertrag) As Rahmenvertrag_I, "
            strQuery &= "IsNull(Setting.Rahmenvertrag_F, Setting.Rahmenvertrag) As Rahmenvertrag_F, "
            strQuery &= "IsNull(Setting.Rahmenvertrag_E, Setting.Rahmenvertrag) As Rahmenvertrag_E "
            strQuery &= "FROM Kandidaten MA "
            strQuery &= "Left Join MySetting Setting On MA.WOS_Guid = Setting.WOS_Guid "
            strQuery &= "Where MA.MA_Guid = @Guid"

            cmd.CommandText = strQuery
            cmd.Connection = conn

            Dim param As System.Data.SqlClient.SqlParameter
            param = cmd.Parameters.AddWithValue("@Guid", appInfo.CurrentGuidValue)
            Dim dataReader As System.Data.SqlClient.SqlDataReader = Nothing
            Try
                ' Open connection to database and read data
                conn.Open()
                dataReader = cmd.ExecuteReader()
                dataReader.Read()
                If dataReader.HasRows = True Then
                    Dim strLanguage As String = Utility.GetColumnTextStr(dataReader, "Language", String.Empty)

                    Dim frameAgreement As String
                    Select Case strLanguage.Substring(0, 1).ToLower
                        Case "i"
                            frameAgreement = "Rahmenvertrag_I"
                        Case "f"
                            frameAgreement = "Rahmenvertrag_F"
                        Case "e"
                            frameAgreement = "Rahmenvertrag_E"
                        Case Else
                            frameAgreement = "Rahmenvertrag_D"
                    End Select

                    If Not dataReader.IsDBNull(dataReader.GetOrdinal(frameAgreement)) Then
                        Dim logoData As Byte() = dataReader.Item(frameAgreement)
                        dataReader.Close()
                        context.Response.ContentType = "application/pdf"
                        context.Response.AddHeader("content-disposition", "filename=" + HttpContext.Current.Server.HtmlEncode("Agreement.pdf"))
                        context.Response.AppendHeader("content-length", logoData.Length)
                        context.Response.BinaryWrite(logoData)
                        context.Response.Flush()
                        hasPdf = True
                    End If
                End If
            Finally
                conn.Close()
                conn.Dispose()
            End Try
        End If

        If Not hasPdf Then

            Dim QueryParameter As String = Utility.ERROR_TYPE_PARAMETER _
                                           & "=" & Utility.ERROR_TYPE_DOCUMENT _
                                           & "&" & Utility.ERROR_MESSAGE_PARAMETER _
                                           & "=" & "Agreement.pdf" _
                                           & "&" & appInfo.CurrentGuidName _
                                           & "=" & appInfo.CurrentGuidValue
            context.Response.Redirect("~/NotFound.aspx?" & QueryParameter)
        End If
    End Sub


    ''' <summary>
    ''' This handler is not reusable
    ''' </summary>
    Public ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
        Get
            Return False
        End Get
    End Property

End Class