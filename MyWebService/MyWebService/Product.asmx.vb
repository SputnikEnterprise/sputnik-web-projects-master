
Imports System.IO
Imports System.Data.SqlClient

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="MyWebSerice", Description:="Product")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class Product
  Inherits System.Web.Services.WebService

  <WebMethod()> _
  Public Function HelloWorld() As String
    Return "Hello World"
  End Function

  <WebMethod(Description:="Returns all Products")> _
Function GetProduct() As DataSet
    Dim _clsSystem As New ClsMain_Net
    Dim connString As String = "Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;Initial Catalog=AdventureWorks;Data Source=192.168.19.115"
    Dim strSQL As String = "SELECT ProductId, Name FROM Production.Product"
    Dim strRecResult As String = String.Empty

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Dim adapter As New SqlDataAdapter()
    adapter.SelectCommand = New Global.System.Data.SqlClient.SqlCommand
    adapter.SelectCommand.Connection = Conn

    Dim rMArec As New DataSet

    ' ---------------------------------------------------------------------------------------
    adapter.SelectCommand.CommandText = strSQL
    adapter.SelectCommand.CommandType = Global.System.Data.CommandType.Text

    Try

      adapter.Fill(rMArec, "Product")

      strRecResult = String.Format(If(rMArec.Tables(0).Rows.Count > 0, "{0} Datensätze wurden gefunden.", _
                                      "Keine Daten wurden gefunden."), rMArec.Tables(0).Rows.Count)


    Catch ex As Exception
      strRecResult = String.Format("Ein Fehler ist aufgetreten.{0}", vbNewLine & ex.Message)

    Finally
      'WriteConnectionHistory(strUserID, strBeruf, strOrt, strKanton, strFiliale, strSortkeys, strRecResult)

    End Try

    Return rMArec
  End Function

End Class