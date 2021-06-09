'---------------------------------------------------------
' Vakanzen.aspx.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services

''' <summary>
''' Shows 'Vakanzen' documents
''' </summary>
''' <remarks></remarks>
Partial Class Vakanzen
    Inherits BasePage

#Region "Properties"
    Private Const VK_MODE_MINE As String = "mine"
    Private Const VK_MODE_ALL As String = "all"
    Private Const REQUEST_TYPE As String = "tp"
    Private Shared VisibleVacancyFields As ArrayList
    Private ValidConfigurableFields As ArrayList
#End Region

#Region "Methods"

    <WebMethod(EnableSession:=True)> _
    Public Shared Function LoadVacancyDetail(ByVal id As Integer, ByVal customerId As String) As StringDictionary


        ' Retrievs application info object
        Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

        ' Creates the connection object
        Dim conn As New System.Data.SqlClient.SqlConnection
        conn.ConnectionString = appInfo.ConnectionString

        ' Creates the command object
        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Connection = conn
        cmd.CommandText = "Get Vakrec By ID"
        cmd.Parameters.AddWithValue("@RecID", id)
        cmd.Parameters.AddWithValue("@UserID", customerId)

        Dim dr As System.Data.SqlClient.SqlDataReader
        Dim detailsDictionary As StringDictionary = New StringDictionary()

        Try
            conn.Open()
            ' Executes the reader
            dr = cmd.ExecuteReader()
            If (dr.Read()) Then
                ' Mandatory fields: id, mail, vacancy number and description
                detailsDictionary.Add("id", Utility.GetColumnTextStr(dr, "ID", String.Empty))
                detailsDictionary.Add("user_email", Utility.GetColumnTextStr(dr, "User_eMail", String.Empty))
                detailsDictionary.Add("bezeichnung", Utility.GetColumnTextStr(dr, "Bezeichnung", String.Empty))
                detailsDictionary.Add("vaknr", Utility.GetColumnInteger(dr, "VakNr", 0))
                For Each field As String In VisibleVacancyFields
                    If (field.Equals("Berater")) Then
                        Dim imageUrl As String = String.Empty
                        ' Show picture if desired
                        If VisibleVacancyFields.Contains("User_Picture_WOS") Then
                            If (Not dr.IsDBNull(dr.GetOrdinal("User_Guid"))) _
                                AndAlso (Not dr.IsDBNull(dr.GetOrdinal("User_picture"))) _
                                AndAlso (CType(dr.Item("User_Picture"), Byte()).Length > 2) Then
                                imageUrl = "<img src='ImageHandler.ashx?type=berater&id=" & Utility.GetColumnTextStr(dr, "User_Guid", String.Empty) & "' width='45%' style='float: right;margin-right: 7%;'>"
                            End If
                        End If

                        ' Show Name
                        detailsDictionary.Add(field.ToLower(), imageUrl & Utility.GetColumnTextStr(dr, "User_Vorname", String.Empty) & " " & Utility.GetColumnTextStr(dr, "User_Nachname", String.Empty))
                    ElseIf field.Equals("User_Picture_WOS") OrElse field.Equals("User_Picture_App") Then
                        ' Do nothing
                        ' This case is handled in the case "Berater" can be shown
                    Else
                        detailsDictionary.Add(field.ToLower(), Utility.GetColumnTextStr(dr, field, String.Empty))
                    End If
                Next field
            End If
            dr.Close()

        Finally
            If Not conn.State = ConnectionState.Closed Then
                conn.Close()
            End If
        End Try
        Return detailsDictionary
    End Function

    ''' <summary>
    ''' Loads the page contents
    ''' </summary>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Check if Owner_Guid is valid (regex) and the customer data can be load
        Dim successFullCustomerDataLoad = Me.IsGuiValid() AndAlso TryToLoadCustomerData()

        If (Session.Contents("AGB_Accepted") Is Nothing) Then
            Dim queryString As String = Request.ServerVariables("QUERY_STRING")
            Response.Redirect(String.Format("~/DefaultPage.aspx?{0}", queryString))
        End If

        If (Not successFullCustomerDataLoad) Then
            Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
        Else

            Dim filterInformation As VacancyFilter = LoadFilterInformation()

            If (filterInformation.IsValid) Then
                If Not LoadVacanciesIntoGrid(filterInformation) Then
                    ShowNoVacanciesMessage()
                Else
                    HideNoVacanciesMessage()
                    ConfigureVacancyDetails(filterInformation.CustomerID)
                End If
            Else
                ShowNoVacanciesMessage()
            End If
        End If
    End Sub

    Private Sub ConfigureVacancyDetails(ByVal CustomerId As String)

        ' Retrievs application info object
        Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

        ' Creates the connection object
        Dim conn As New System.Data.SqlClient.SqlConnection
        conn.ConnectionString = appInfo.ConnectionString

        ' Valid Configurable Columns
        ValidConfigurableFields = New ArrayList(New String("Beginn#Taetigkeit#Anforderung#JobProzent#Anstellung#Vak_Kanton#Vak_Region#JobOrt#Ausbildung#Berater#Filiale").Split("#"))

        ' Creates the command object
        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
        cmd.CommandType = CommandType.Text
        cmd.Connection = conn
		cmd.CommandText = "SELECT Visible_Vacancy_Fields FROM MySetting WHERE WOS_Guid = @ID "
		cmd.Parameters.AddWithValue("@ID", CustomerId)

        Dim dr As System.Data.SqlClient.SqlDataReader
        Try
            conn.Open()

            ' Executes the reader
            dr = cmd.ExecuteReader()
            If (dr.Read()) Then
                Vakanzen.VisibleVacancyFields = New ArrayList()
                Dim visibleColumns As String = Utility.GetColumnTextStr(dr, "Visible_Vacancy_Fields", "-")
                For Each field In visibleColumns.Split("#")
                    If field.Length > 0 Then
                        Vakanzen.VisibleVacancyFields.Add(field)
                    End If
                Next
            End If
            dr.Close()
        Finally
            If Not conn.State = ConnectionState.Closed Then
                conn.Close()
            End If
        End Try

        If Vakanzen.VisibleVacancyFields.Count <> 0 Then
            pnlVacancyDetail.SetWidths(600, 33, 66)
            pnlVacancyDetail.AddEmptyRow()
            pnlVacancyDetail.AddRow("TEXT_JOB_NUMBER", "vaknr")
            pnlVacancyDetail.AddRow("TEXT_JOB_DESCRIPTION", "bezeichnung")
            For Each field As String In Vakanzen.VisibleVacancyFields
                If ValidConfigurableFields.Contains(field) Then
                    AddRow(field)
                End If
            Next field
            pnlVacancyDetail.AddEmptyRow()
            pnlVacancyDetail.AddLinkRow("TEXT_JOB_APPLICATION", "TEXT_JOB_APPLICATION_ACTION", "user_email")
            pnlVacancyDetail.AddLinkRow("TEXT_GENERAL_BUTTON_PRINTER_FRIENDLY_VERSION", "TEXT_GENERAL_BUTTON_PRINT", "printerFriendly")
        End If
    End Sub

    Private Sub AddRow(ByVal fieldName As String)
        Dim field As String = fieldName.ToLower()
        Select Case field
            Case "beginn"
                pnlVacancyDetail.AddRow("TEXT_JOB_START", field)
            Case "taetigkeit"
                pnlVacancyDetail.AddRow("TEXT_JOB_ACTIVITY", field)
            Case "anforderung"
                pnlVacancyDetail.AddRow("TEXT_JOB_DEMAND", field)
                pnlVacancyDetail.AddEmptyRow()
            Case "jobprozent"
                pnlVacancyDetail.AddRow("TEXT_JOB_PROZENT", field)
            Case "anstellung"
                pnlVacancyDetail.AddRow("TEXT_JOB_TYPE", field)
                pnlVacancyDetail.AddEmptyRow()
            Case "vak_kanton"
                pnlVacancyDetail.AddRow("TEXT_ADDRESS_CANTON", field)
            Case "vak_region"
                pnlVacancyDetail.AddRow("TEXT_ADDRESS_REGION", field)
            Case "jobort"
                pnlVacancyDetail.AddRow("TEXT_ADDRESS_LOCATION", field)
                pnlVacancyDetail.AddEmptyRow()
            Case "ausbildung"
                pnlVacancyDetail.AddRow("TEXT_EDUCATION", field)
                pnlVacancyDetail.AddEmptyRow()
                pnlVacancyDetail.AddEmptyRow()
            Case "berater"
                pnlVacancyDetail.AddRow("TEXT_CANDIDATES_ADVISOR", field)
                ' TODO Add Image

            Case "filiale"
                pnlVacancyDetail.AddRow("TEXT_BRANCH_OFFICE", field)
        End Select
    End Sub


    Private Sub ShowNoVacanciesMessage()

        ' If no vacancies could be loaded, then show a message to the user
        Dim master As SiteMaster = CType(Me.Master, SiteMaster)
        master.ShowInfoMessage("TEXT_CANDIDATES_NO_VACANCIES", True)
        pnlNumberOfVacancies.Visible = False
    End Sub


    Private Sub HideNoVacanciesMessage()

        ' If no vacancies could be loaded, then show a message to the user
        Dim master As SiteMaster = CType(Me.Master, SiteMaster)
        master.ShowInfoMessage(False)
        pnlNumberOfVacancies.Visible = True
    End Sub

    Public Function LoadFilterInformation() As VacancyFilter

        Dim vacancyFilter As VacancyFilter = New VacancyFilter()

        ' Creates the connection object
        Dim conn As New System.Data.SqlClient.SqlConnection
        conn.ConnectionString = Me.ApplicationInfo.ConnectionString

        ' Creates the command object
        Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
        cmd.CommandType = CommandType.Text
        cmd.Connection = conn
        cmd.CommandText = "SELECT Customer_ID, MA_Branche FROM Kandidaten WHERE MA_Guid = @CurrentGuidValue"

        ' Add the current GUID value to the SQL query
        Dim param As System.Data.SqlClient.SqlParameter
        param = cmd.Parameters.AddWithValue("@CurrentGuidValue", ApplicationInfo.CurrentGuidValue)
        Dim dr As System.Data.SqlClient.SqlDataReader

        Try
            conn.Open()

            ' Executes the reader
            dr = cmd.ExecuteReader()

            If dr.HasRows Then
                dr.Read()
                vacancyFilter.CustomerID = dr("Customer_ID")
                vacancyFilter.Branch = Utility.GetColumnTextStr(dr, "MA_Branche", String.Empty)
            Else
                vacancyFilter.IsValid = False
            End If

            dr.Close()
        Finally
            If Not conn.State = ConnectionState.Closed Then
                conn.Close()
            End If

            vacancyFilter.IsValid = False
        End Try

        ' Store the search box query 
        vacancyFilter.SearchQuery = txtSearchJob.Text

        vacancyFilter.IsValid = True
        Return vacancyFilter

    End Function

    ''' <summary>
    ''' Fills the the GridView with vacancy data
    ''' </summary>
    Private Function LoadVacanciesIntoGrid(ByVal filterInformation As VacancyFilter) As Boolean

        ' Loads the DataSet with the vacancy information from the KD_Vakanzen table.
        Dim documentsDataSet = LoadVacanciesInformation(filterInformation)

        If (Not documentsDataSet Is Nothing) Then
            ' Bind the DataSet to the GridView
            grid.DataSource = documentsDataSet
            grid.DataBind()

            'Displays the number of found vacancies to the user
            lblNumberOfVacanciesInfoAmount.Text = documentsDataSet.Tables(0).Rows.Count.ToString

            ' Show a message if no vacancies could be found
            If documentsDataSet.Tables(0).Rows.Count = 0 Then
                Return False
            End If
        Else
            Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
        End If

        Return True

    End Function

    ''' <summary>
    ''' Loads vacancy information into a DataSet.
    ''' </summary>
    Private Function LoadVacanciesInformation(ByVal filterInformation As VacancyFilter) As DataSet

        Try
            Dim strQuery As String

            Dim branchFilter As String = CreateBranchFilter(filterInformation.BranchEntries, "Branchen")

            Dim vkQueryParameter As String = Request.QueryString(REQUEST_TYPE)

            If vkQueryParameter = VK_MODE_MINE Then
                ' Add the branch filter
                strQuery = String.Format("SELECT * FROM KD_Vakanzen WHERE (Customer_ID = @CustomerId) {0} AND Bezeichnung LIKE @SearchQuery ORDER BY Transfered_On DESC", branchFilter)

            ElseIf vkQueryParameter = VK_MODE_ALL Then
                strQuery = "SELECT * FROM KD_Vakanzen WHERE (Customer_ID = @CustomerId) AND Bezeichnung LIKE @SearchQuery ORDER BY Transfered_On DESC"
            Else
                Return Nothing
            End If

            ' Creates the connection object
            Dim conn As New System.Data.SqlClient.SqlConnection
            conn.ConnectionString = Me.ApplicationInfo.ConnectionString

            'SQL Command
            Dim objCommand As New SqlCommand(strQuery, conn)
            objCommand.CommandType = CommandType.Text

            objCommand.Parameters.AddWithValue("@CustomerId", filterInformation.CustomerID)
            objCommand.Parameters.AddWithValue("@SearchQuery", "%" + filterInformation.SearchQuery + "%")

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

            conn.Close()

            Return documentsDataSet

        Catch ex As SqlClient.SqlException
            Return Nothing
        End Try

        Return Nothing
    End Function

    Private Function CreateBranchFilter(ByVal branchEntries As String(), ByVal columnName As String) As String

        Dim buffer As StringBuilder = New StringBuilder()
        Dim branch As String = String.Empty
        Dim i As Integer

        If branchEntries.Length = 0 Then
            Return "AND (1 = 0)"
        End If

        For i = 0 To branchEntries.Length - 1

            branch = branchEntries(i)

            If String.IsNullOrEmpty(branch) Then
                Continue For
            End If

            buffer.Append("(")
            buffer.Append(columnName)
            buffer.Append(" LIKE '%")
            buffer.Append(branch)
            buffer.Append("%'")
            buffer.Append(")")

            If (i < branchEntries.Length - 1) Then
                buffer.Append(" OR ")
            End If
        Next

        Dim branchFilter = buffer.ToString()
        If branchFilter.Length > 0 Then
            branchFilter = "AND (" & branchFilter & ")"
        Else
            branchFilter = "AND (1 = 0)"
        End If

        Return branchFilter
    End Function

    Protected Sub GridView_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles grid.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim lnkVacanyDetail As LinkButton = CType(e.Row.FindControl("lnkVacancyDetail"), LinkButton)

            Dim index As Integer = Convert.ToInt32(e.Row.RowIndex)
            Dim dbId As Integer = grid.DataKeys.Item(index).Value

            ' Read the underlying DataSet of the grid
            Dim docDataSet As DataSet = CType(grid.DataSource, DataSet)

            ' Find the correct DataRow
            Dim row As DataRow = docDataSet.Tables(0).Rows().Find(dbId)

            Dim detail = row("Bezeichnung")
            If detail.Length > 40 Then
                detail = detail.Substring(0, 40) & "..."
            End If

            Dim customerId = row("Customer_ID")

            lnkVacanyDetail.Text = Server.HtmlEncode(detail)

            lnkVacanyDetail.OnClientClick = String.Format("javascript: ShowVacancyDetail('{0}', '{1}'); return false;", dbId, customerId)

        End If

    End Sub

    ''' <summary>
    ''' Needed for paging support
    ''' </summary>
    Protected Sub GridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles grid.PageIndexChanging
        grid.PageIndex = e.NewPageIndex
        grid.DataBind()
    End Sub

    ' Returns welcome text for candidate pages
    Public Overrides Function GetWelcomeText(ByVal dr As SqlDataReader) As String
        Return String.Format("{0},", Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty))
    End Function

    Public Overrides Function GetUserName(ByVal dr As SqlDataReader) As String
        Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "MA_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "MA_Vorname", String.Empty))
    End Function

    Public Overrides Function GetUserEmail(ByVal dr As SqlDataReader) As String
        Return Utility.GetColumnTextStr(dr, "MA_EMail", String.Empty)
    End Function


#End Region

End Class
