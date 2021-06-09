  <%@ WebHandler Language="VB" Class="Documents" %>

Imports System
Imports System.Web
Imports System.Data
Imports System.Data.SqlClient
Imports WOS_Website

''' <summary>
''' Document handler
''' </summary>
Public Class Documents : Implements IHttpHandler
	Implements SessionState.IReadOnlySessionState

	''' <summary>
	'''Entry point for handler request
	''' </summary>
	Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		Dim dbId As Integer = context.Request.QueryString("id")
		Dim fileName As String = context.Request.QueryString("fn")

		' Retrievs application info object
		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
		If appInfo Is Nothing Then Return

		' Sends the pdf document to the customer
		SendDocumentToCustomer(context.Response, dbId, fileName, appInfo)

		' Log Document View
		If appInfo.CurrentGuidName = "ZHD" Then LogDocumentView(context.Response, dbId, fileName, appInfo)

	End Sub

	''' <summary>
	''' Sends a document to the customers browser.
	''' </summary>
	Public Sub SendDocumentToCustomer(ByVal Response As HttpResponse, ByVal dbId As Integer, ByVal fileName As String, ByVal appInfo As ApplicationInfo)

		Using myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("SpContractConnectionString").ConnectionString)

			Dim myCommand As New SqlCommand(appInfo.DocumentSQLStrings.LoadDocScanSQL, myConnection)
			myCommand.Parameters.AddWithValue("@ID", dbId)
			myCommand.Parameters.AddWithValue("@CurrentGuid", appInfo.CurrentGuidValue)

			myConnection.Open()
			Dim myReader As SqlDataReader = myCommand.ExecuteReader()

			If myReader.Read Then

				Dim data As Byte() = myReader("DocScan")

				Response.ContentType = "application/pdf"
				Response.AddHeader("content-disposition", "filename=" + HttpContext.Current.Server.HtmlEncode(fileName + ".pdf"))
				Response.AppendHeader("content-length", data.Length)
				Response.BinaryWrite(data)
				Response.Flush()

			End If

			myReader.Close()
			myConnection.Close()

		End Using
		Console.Write(appInfo.CurrentGuidValue)

	End Sub


	Public Sub LogDocumentView(ByVal Response As HttpResponse, ByVal dbId As Integer, ByVal fileName As String, ByVal appInfo As ApplicationInfo)

		Using myConnection As New SqlConnection(ConfigurationManager.ConnectionStrings("SpContractConnectionString").ConnectionString)

			Dim myCommand As New SqlCommand(appInfo.DocumentSQLStrings.UpdateDocumentView, myConnection)
			myCommand.Parameters.AddWithValue("@ID", dbId)
			myCommand.Parameters.AddWithValue("@CurrentGuid", appInfo.CurrentGuidValue)

			Try
				myConnection.Open()
				myCommand.ExecuteNonQuery()

			Finally
				If Not myConnection.State = ConnectionState.Closed Then
					myConnection.Close()
				End If
			End Try

		End Using

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