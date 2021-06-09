'---------------------------------------------------------
' DocumentBasePage.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

' Base page of all document pages
Public MustInherit Class DocumentBasePage
    Inherits BasePage

    Dim CandidateQueryParameterName As Object

    ' SQL query from document retrieval
    Dim _DocumentsQuerySQL As String
    Protected Property DocumentsQuerySQL() As String
        Get
            Return _DocumentsQuerySQL
        End Get

        Set(ByVal value As String)
            _DocumentsQuerySQL = value
        End Set
    End Property

    Dim forwardDocumentService As ScriptReference = New ScriptReference("~/ForwardDocument.asmx/js")

    ' Loads the documents into the grid
    Public Function LoadDocuments(ByVal grid As GridView, ByVal lblNumberOfDocsInfoAmount As Label, ByVal pnlNumberOfDocs As Panel, ByVal docArt As String) As Boolean
        Return LoadDocuments(grid, lblNumberOfDocsInfoAmount, pnlNumberOfDocs, docArt, String.Empty)
    End Function

    ' Loads the documents into the grid
    Public Function LoadDocuments(ByVal grid As GridView, ByVal lblNumberOfDocsInfoAmount As Label, ByVal pnlNumberOfDocs As Panel, ByVal docArt As String, ByVal additionalFilter As String) As Boolean

        Dim sm As ScriptManager = ScriptManager.GetCurrent(Page)
        sm.Scripts.Add(forwardDocumentService)


        ' Check if Owner_Guid is valid (regex) and the customer data can be load
        Dim successFullCustomerDataLoad = Me.IsGuiValid() AndAlso TryToLoadCustomerData()

        If (Session.Contents("AGB_Accepted") Is Nothing) Then
            Dim queryString As String = Request.ServerVariables("QUERY_STRING")
            Response.Redirect(String.Format("~/DefaultPage.aspx?{0}", queryString))
        End If

        If (Not successFullCustomerDataLoad) Then
            Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
        Else
            Return LoadDocumentsIntoGrid(grid, lblNumberOfDocsInfoAmount, pnlNumberOfDocs, docArt, additionalFilter)
        End If

        Return False

    End Function

    ' Creates the js code for document forward vai email
    Protected Function CreateForwardDocumentJavaScript(ByVal dbId As Integer, ByVal docArt As String, ByVal lnkClientId As String) As String

        Dim js As String = "javascript: $.alerts.okButton = 'Senden';  $.alerts.cancelButton = 'Abbrechen';"
        js = js & "jPrompt('Bitte Email Adresse eingeben', '', 'Dokument weiterleiten', "
        js = js & String.Format("function(email) {{ if(email) {{$('#{0}').addClass('targetDocument'); WOS_Website.WebServices.ForwardDocument.FowardDocument(email, {1}, '{2}', '{3}', OnForwardDocumentComplete); }}}}); return false; ", lnkClientId, dbId, ApplicationInfo.CurrentGuidValue, docArt)
        Return js
    End Function


    ''' <summary>
    ''' Fills the the GridView with document data
    ''' </summary>
    Private Function LoadDocumentsIntoGrid(ByVal grid As GridView, ByVal lblNumberOfDocsInfoAmount As Label, ByVal pnlNumberOfDocs As Panel, ByVal docArt As String, ByVal additionalFilter As String) As Boolean

        ' Loads the DataSet with the document information from the Kandidaten_Doc_Online table.
        Dim documentsDataSet = LoadDocumentInformation(docArt, ApplicationInfo.CurrentGuidValue, additionalFilter)

        If (Not documentsDataSet Is Nothing) Then
            ' Bind the DataSet to the GridView
            grid.DataSource = documentsDataSet
            grid.DataBind()

            'Displays the number of found documents to the user
            lblNumberOfDocsInfoAmount.Text = documentsDataSet.Tables(0).Rows.Count.ToString

            ' Show a message if no documents could be found
            If documentsDataSet.Tables(0).Rows.Count = 0 Then
                Return False
            End If
        Else
            Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
        End If

        Return True

    End Function

    ''' <summary>
    ''' Loads document information into a DataSet.
    ''' </summary>
    Private Function LoadDocumentInformation(ByVal docArt As String, ByVal ownerGuid As String, ByVal additionalFilter As String) As DataSet

        Try
            Dim strQuery As String

            strQuery = ApplicationInfo.DocumentSQLStrings.LoadDocumentInformationSQL + additionalFilter + " ORDER BY Orig_Transfered_On DESC"

            Dim conStr As String = ApplicationInfo.ConnectionString

            Dim objCon As New SqlConnection(conStr)

            'SQL Command
            Dim objCommand As New SqlCommand(strQuery, objCon)
            objCommand.CommandType = CommandType.Text

            ' Parameters
            Dim docArtParam As New SqlParameter()
            Dim ownerGuidParam As New SqlParameter()

            docArtParam = objCommand.Parameters.Add(New SqlParameter("@Doc_Art", SqlDbType.VarChar, 50))
            ownerGuidParam = objCommand.Parameters.Add(New SqlParameter("@CurrentGuid", SqlDbType.VarChar, 255))

            docArtParam.Direction = ParameterDirection.Input
            docArtParam.Value = docArt
            ownerGuidParam.Value = ownerGuid

            'DataAdapter
            Dim objAdapter As SqlDataAdapter
            objAdapter = New SqlDataAdapter()
            objAdapter.SelectCommand = objCommand

            'Fill DataSet
            Dim documentsDataSet As New DataSet
            objAdapter.Fill(documentsDataSet)

            Dim keys(1) As DataColumn

            keys(0) = documentsDataSet.Tables(0).Columns("ID")
            documentsDataSet.Tables(0).PrimaryKey = keys

            objCon.Close()

            Return documentsDataSet

        Catch ex As SqlClient.SqlException
            Return Nothing
        End Try

        Return Nothing
    End Function

End Class
