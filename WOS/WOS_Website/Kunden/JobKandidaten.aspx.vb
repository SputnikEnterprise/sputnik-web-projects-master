Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.Services
Imports System.Collections.Generic

Partial Class JobKandidaten
	Inherits BasePage

#Region "Properties"
	Private Const JOB_MODE_MINE As String = "mine"
	Private Const JOB_MODE_ALL As String = "all"
	Private Const REQUEST_TYPE As String = "tp"
	Private Shared VisibleCandidateFields As ArrayList
	Private ValidConfigurableFields As ArrayList
#End Region

	<WebMethod(EnableSession:=True)>
	Public Shared Function LoadJobCandidateDetail(ByVal CandidateID As Integer) As StringDictionary

		'	' Retrievs application info object
		'	Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		'	' Creates the connection object
		'	Dim conn As New System.Data.SqlClient.SqlConnection
		'	conn.ConnectionString = appInfo.ConnectionString

		'	' Creates the command object
		'	Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		'	cmd.CommandType = CommandType.Text
		'	cmd.Connection = conn
		'	Dim sql As String
		'	'sql = "SELECT k.ID, k.MANr, k.Berater, k.MA_Vorname, k.MA_Nachname, k.MA_Kanton, k.MA_Ort, k.MA_Beruf, k.MA_Branche, k.MASex, k.MA_GebDat, k.MA_Language, k.MA_Nationality,"
		'	'sql &= "o.JobProzent, o.MAZivil, o.MAFSchein, o.MAAuto, o.Bewillig, o.User_eMail "
		'	'sql &= "FROM Kandidaten_Online AS o, Kandidaten AS k WHERE o.ID = @ID AND o.Customer_ID = k.Customer_ID AND o.Transfered_Guid = k.MA_Guid "

		'	sql = "SELECT o.ID,"
		'	sql &= "o.WOS_Guid,"
		'	sql &= "o.Customer_ID,"
		'	sql &= "o.MANr,"
		'	sql &= "o.Berater,"
		'	sql &= "o.MA_Vorname,"
		'	sql &= "o.MA_Nachname,"
		'	sql &= "o.MA_Kanton,"
		'	sql &= "o.MA_Ort,"
		'	sql &= "o.MA_Beruf,"
		'	sql &= "o.branches,"
		'	sql &= "o.MASex,"
		'	sql &= "o.MAGebDat,"
		'	sql &= "o.MainLanguage,"
		'	sql &= "o.MA_MSprache,"
		'	sql &= "o.MANationality,"
		'	sql &= "o.JobProzent,"
		'	sql &= "o.MAZivil,"
		'	sql &= "o.MAFSchein,"
		'	sql &= "o.MAAuto,"
		'	sql &= "o.Bewillig,"
		'	sql &= "		   ("
		'	sql &= "   Case "
		'	sql &= "When ISNULL(O.Advisor_ID, '') = '' THEN O.User_eMail ELSE (SELECT TOP 1 U.User_eMail FROM dbo.Customer_Users U WHERE U.WOS_Guid = O.WOS_Guid AND U.User_ID = O.Advisor_ID) "
		'	sql &= "End ) User_eMail "
		'	sql &= "FROM Kandidaten_Online As O "
		'	sql &= "WHERE o.ID = @ID;"

		'	'("Berater#MA_Vorname#MA_Nachname#MA_Kanton#MA_Ort#branches#MASex#MAGebDat#MainLanguage#MANationality#JobProzent#MAZivil#MAFSchein#MAAuto#Bewillig")

		'	'sql = "Select O.ID, O.MANr, O.Berater, O.MA_Vorname, O.MA_Nachname, O.MA_Kanton, k.MA_Ort, k.MA_Beruf, k.MA_Branche, k.MASex, k.MA_GebDat, k.MA_Language, k.MA_Nationality,"
		'	'sql &= "o.JobProzent, o.MAZivil, o.MAFSchein, o.MAAuto, o.Bewillig, o.User_eMail "
		'	'sql &= "FROM Kandidaten_Online As o, Kandidaten As k WHERE o.ID = @ID And o.Customer_ID = k.Customer_ID And o.Transfered_Guid = k.MA_Guid "

		'	cmd.CommandText = sql
		'	cmd.Parameters.AddWithValue("@ID", CandidateID)

		'	Dim dr As System.Data.SqlClient.SqlDataReader
		'	Dim detailsDictionary As StringDictionary = New StringDictionary()
		'	Try
		'		conn.Open()

		'		' Executes the reader
		'		dr = cmd.ExecuteReader()
		'		If (dr.Read()) Then
		'			' Mandatory fields: id, mail and profession
		'			detailsDictionary.Add("id", Utility.GetColumnTextStr(dr, "ID", "0"))
		'			detailsDictionary.Add("manr", Utility.GetColumnTextStr(dr, "MANr", "-"))
		'			detailsDictionary.Add("user_email", Utility.GetColumnTextStr(dr, "User_eMail", String.Empty))

		'			detailsDictionary.Add("ma_beruf", Utility.GetColumnTextStr(dr, "MA_Beruf", String.Empty).Replace("#", ", "))
		'			For Each field As String In VisibleCandidateFields
		'				If field.Equals("MAGebDat") Then
		'					detailsDictionary.Add(field.ToLower(), CType(Utility.GetColumnTextStr(dr, field, String.Empty), DateTime).Year.ToString())
		'				ElseIf field.Equals("branches") Then
		'					detailsDictionary.Add(field.ToLower(), Utility.GetColumnTextStr(dr, field, String.Empty).Replace("#", ", "))
		'				Else
		'					detailsDictionary.Add(field.ToLower(), Utility.GetColumnTextStr(dr, field, String.Empty))
		'				End If
		'			Next field

		'		End If
		'		dr.Close()
		'	Finally
		'		If Not conn.State = ConnectionState.Closed Then
		'			conn.Close()
		'		End If
		'	End Try

		'	Return detailsDictionary
	End Function

	Private Sub ConfigureCandidateDetails(ByVal CustomerId As String)

		' Retrievs application info object
		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		' Creates the connection object
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = appInfo.ConnectionString

		' Valid Configurable Columns
		ValidConfigurableFields = New ArrayList(New String("Berater#MA_Vorname#MA_Nachname#MA_Kanton#MA_Ort#branches#MASex#MAGebDat#MainLanguage#MANationality#JobProzent#MAZivil#MAFSchein#MAAuto#Bewillig").Split("#"))

		' Creates the command object
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.Connection = conn
		cmd.CommandText = "Select Visible_Candidate_Fields FROM MySetting WHERE KD_Guid = @ID "
		cmd.Parameters.AddWithValue("@ID", CustomerId)

		Dim dr As System.Data.SqlClient.SqlDataReader
		Try
			conn.Open()

			' Executes the reader
			dr = cmd.ExecuteReader()
			If (dr.Read()) Then
				JobKandidaten.VisibleCandidateFields = New ArrayList()
				Dim visibleColumns As String = Utility.GetColumnTextStr(dr, "Visible_Candidate_Fields", "-")
				For Each field In visibleColumns.Split("#")
					If field.Length > 0 Then
						JobKandidaten.VisibleCandidateFields.Add(field)
					End If
				Next
			End If
			dr.Close()
		Finally
			If Not conn.State = ConnectionState.Closed Then
				conn.Close()
			End If
		End Try

		If JobKandidaten.VisibleCandidateFields.Count <> 0 Then
			pnlJobCandidateDetail.SetWidths(600, 33, 66)
			pnlJobCandidateDetail.AddEmptyRow()
			pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_PROFESSION", "ma_beruf_0")
			For Each field As String In VisibleCandidateFields
				If ValidConfigurableFields.Contains(field) Then
					AddRow(field)
				End If
			Next field
			pnlJobCandidateDetail.AddEmptyRow()
			pnlJobCandidateDetail.AddLinkRow("TEXT_CANDIDATE_EMAIL", "TEXT_CANDIDATE_REQUEST", "user_email")
			pnlJobCandidateDetail.AddLinkRow("TEXT_GENERAL_BUTTON_PRINTER_FRIENDLY_VERSION", "TEXT_GENERAL_BUTTON_PRINT", "printerFriendly")
		End If
	End Sub

	Private Sub AddRow(ByVal fieldName As String)
		Dim field As String = fieldName.ToLower()
		Select Case field
			Case "berater"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_CONSULTANT", field)
			Case "ma_vorname"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_FIRST_NAME", field)
			Case "ma_nachname"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_LAST_NAME", field)
			Case "ma_kanton"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_CANTON", field)
			Case "ma_ort"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_LOCATION", field)
			Case "ma_branche"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_BRANCH", field)
			Case "masex"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_SEX", field)
			Case "MAGebDat".ToLower
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_YEAR", field)
			Case "MainLanguage".ToLower
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_LANGUAGE", field)
			Case "MANationality".ToLower
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_NATIONALITY", field)
			Case "jobprozent"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_PERCENT", field)
			Case "mazivil"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_CIVIL_STATUS", field)
			Case "mafschein"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_DRIVER_LICENCE", field)
			Case "maauto"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_CAR", field)
			Case "bewillig"
				pnlJobCandidateDetail.AddRow("TEXT_CANDIDATE_PERMITS", field)
		End Select

	End Sub

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

			Dim filterInformation As JobCandidateFilter = LoadFilterInformation()

			If (filterInformation.IsValid) Then
				If Not LoadJobCandidatesIntoGrid(filterInformation) Then
					ShowNoJobCandidatesMessage()
				Else
					HideNoJobCandidatesMessage()
					ConfigureCandidateDetails(filterInformation.CustomerID)
				End If
			Else
				ShowNoJobCandidatesMessage()
			End If
		End If
	End Sub

	Private Sub ShowNoJobCandidatesMessage()
		' If no vacancies could be loaded, then show a message to the user
		Dim master As SiteMaster = CType(Me.Master, SiteMaster)
		master.ShowInfoMessage("TEXT_CUSTOMER_NO_CANDIDATES", True)
		pnlNumberOfJobCandidates.Visible = False
	End Sub


	Private Sub HideNoJobCandidatesMessage()

		' If no vacancies could be loaded, then show a message to the user
		Dim master As SiteMaster = CType(Me.Master, SiteMaster)
		master.ShowInfoMessage(False)
		pnlNumberOfJobCandidates.Visible = True
	End Sub

	Public Function LoadFilterInformation() As JobCandidateFilter

		Dim jobCandidateFilter As JobCandidateFilter = New JobCandidateFilter()

		' Creates the connection object
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = Me.ApplicationInfo.ConnectionString

		' Creates the command object
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.Connection = conn

		Dim branchColumn As String = String.Empty

		If ApplicationInfo.CurrentGuidName = ApplicationInfo.KDQueryParameterName Then
			cmd.CommandText = "Select Customer_ID, KD_Branche FROM Kunden WHERE KD_Guid = @CurrentGuidValue"
			branchColumn = "KD_Branche"
		ElseIf ApplicationInfo.CurrentGuidName = ApplicationInfo.ZHDQueryParameterName Then
			cmd.CommandText = "Select Customer_ID, Zhd_Branche FROM Kunden_ZHD WHERE ZHD_Guid = @CurrentGuidValue"
			branchColumn = "Zhd_Branche"
		Else
			' Must be an error
			jobCandidateFilter.IsValid = False
			Return jobCandidateFilter
		End If

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
				jobCandidateFilter.CustomerID = dr("Customer_ID")
				jobCandidateFilter.Branch = Utility.GetColumnTextStr(dr, branchColumn, String.Empty)
			Else
				jobCandidateFilter.IsValid = False
			End If

			dr.Close()
		Finally
			If Not conn.State = ConnectionState.Closed Then
				conn.Close()
			End If

			jobCandidateFilter.IsValid = False
		End Try

		' Store the search box query 
		jobCandidateFilter.SearchQuery = txtSearchJob.Text

		jobCandidateFilter.IsValid = True
		Return jobCandidateFilter

	End Function

	''' <summary>
	''' Fills the the GridView with job candidate data
	''' </summary>
	Private Function LoadJobCandidatesIntoGrid(ByVal filterInformation As JobCandidateFilter) As Boolean

		' Loads the DataSet with the job candidate information from the Kandidaten table.
		Dim documentsDataSet = LoadJobCandidateInformation(filterInformation)

		If (Not documentsDataSet Is Nothing) Then
			' Bind the DataSet to the GridView
			grid.DataSource = documentsDataSet
			grid.DataBind()

			'Displays the number of found candidates to the user
			lblNumberOfJobCandidatesInfoAmount.Text = documentsDataSet.Tables(0).Rows.Count.ToString

			' Show a message if no candidates could be found
			If documentsDataSet.Tables(0).Rows.Count = 0 Then
				Return False
			End If
		Else
			Response.Redirect("~/NotFound.aspx?" & ApplicationInfo.CurrentGuidName & "=" & ApplicationInfo.CurrentGuidValue)
		End If

		Return True

	End Function

	''' <summary>
	''' Loads job candidate information into a DataSet.
	''' </summary>
	Private Function LoadJobCandidateInformation(ByVal filterInformation As JobCandidateFilter) As DataSet

		Try
			Dim sql As String

			Dim branchFilter As String = CreateBranchFilter(filterInformation.BranchEntries, "O.Branches")

			Dim vkQueryParameter As String = Request.QueryString(REQUEST_TYPE)

			If vkQueryParameter = JOB_MODE_MINE Then
				' Add the branch filter
				sql = "Select * FROM Kandidaten_Online As O "
				sql &= "WHERE (O.WOS_Guid = @CustomerId) "
				sql &= "And k.Customer_ID = o.Customer_ID "
				sql &= "And o.Transfered_Guid = k.MA_Guid "
				sql &= String.Format("{0} ", branchFilter)
				sql &= "And o.MA_Beruf Like @SearchQuery "
				sql &= "ORDER BY o.Transfered_On DESC"

				sql = "SELECT o.ID,"
				sql &= "o.WOS_Guid,"
				sql &= "o.Customer_ID,"
				sql &= "o.MANr,"
				sql &= "o.Berater,"
				sql &= "o.MA_Vorname,"
				sql &= "o.MA_Nachname,"
				sql &= "o.MA_Kanton,"
				sql &= "o.MA_Ort,"
				sql &= "o.MA_Beruf,"
				sql &= "o.branches,"
				sql &= "o.MASex,"
				sql &= "o.MAGebDat,"
				sql &= "o.MainLanguage,"
				sql &= "o.MA_MSprache,"
				sql &= "o.MANationality,"
				sql &= "o.JobProzent,"
				sql &= "o.MAZivil,"
				sql &= "o.MAFSchein,"
				sql &= "o.MAAuto,"
				sql &= "o.Bewillig,"
				sql &= "		   ("
				sql &= "   Case "
				sql &= "When ISNULL(O.Advisor_ID, '') = '' THEN O.User_eMail ELSE (SELECT TOP 1 U.User_eMail FROM dbo.Customer_Users U WHERE U.WOS_Guid = O.WOS_Guid AND U.User_ID = O.Advisor_ID) "
				sql &= "End ) User_eMail "
				sql &= "FROM Kandidaten_Online As O "
				sql &= "WHERE o.WOS_Guid = @CustomerId;"


				'strQuery = String.Format("Select * FROM Kandidaten_Online As o, Kandidaten As k " &
				'																	 "WHERE (k.Customer_ID = @CustomerId) " &
				'																	 "And k.Customer_ID = o.Customer_ID " &
				'																	 "And o.Transfered_Guid = k.MA_Guid " &
				'																	 "{0} " &
				'																	 "And o.MA_Beruf Like @SearchQuery " &
				'																	 "ORDER BY o.Transfered_On DESC",
				'																	 branchFilter)
			ElseIf vkQueryParameter = JOB_MODE_ALL Then
				'strQuery = "Select * FROM Kandidaten_Online WHERE (Wos_Guid = @CustomerId) And MA_Beruf Like @SearchQuery ORDER BY Transfered_On DESC"

				sql = "SELECT o.ID,"
				sql &= "o.WOS_Guid,"
				sql &= "o.Customer_ID,"
				sql &= "o.MANr,"
				sql &= "o.Berater,"
				sql &= "o.MA_Vorname,"
				sql &= "o.MA_Nachname,"
				sql &= "o.MA_Kanton,"
				sql &= "o.MA_Ort,"
				sql &= "o.MA_Beruf,"
				sql &= "o.branches,"
				sql &= "o.MASex,"
				sql &= "o.MAGebDat,"
				sql &= "o.MainLanguage,"
				sql &= "o.MA_MSprache,"
				sql &= "o.MANationality,"
				sql &= "o.JobProzent,"
				sql &= "o.MAZivil,"
				sql &= "o.MAFSchein,"
				sql &= "o.MAAuto,"
				sql &= "o.Bewillig,"
				sql &= "		   ("
				sql &= "   Case "
				sql &= "When ISNULL(O.Advisor_ID, '') = '' THEN O.User_eMail ELSE (SELECT TOP 1 U.User_eMail FROM dbo.Customer_Users U WHERE U.WOS_Guid = O.WOS_Guid AND U.User_ID = O.Advisor_ID) "
				sql &= "End ) User_eMail "
				sql &= "FROM Kandidaten_Online As O "
				sql &= "WHERE o.WOS_Guid = @CustomerId;"

			Else
				Return Nothing
			End If
			'(strQuery)


			' Creates the connection object
			Dim conn As New System.Data.SqlClient.SqlConnection
			conn.ConnectionString = Me.ApplicationInfo.ConnectionString

			'SQL Command
			Dim objCommand As New SqlCommand(sql, conn)
			objCommand.CommandType = CommandType.Text

			objCommand.Parameters.AddWithValue("@CustomerId", filterInformation.CustomerID)
			If Not vkQueryParameter = JOB_MODE_ALL Then
				'objCommand.Parameters.AddWithValue("@SearchQuery", "%" + filterInformation.SearchQuery + "%")
			End If


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
			Return "And (1 = 0)"
		End If

		For i = 0 To branchEntries.Length - 1

			branch = branchEntries(i)

			If String.IsNullOrEmpty(branch) Then
				Continue For
			End If

			buffer.Append("(")
			buffer.Append(columnName)
			buffer.Append(" Like '%")
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

			'--------- MA_Beruf -----------
			Dim lnkJobCandidateDetail As LinkButton = CType(e.Row.FindControl("lnkJobCandidateDetail"), LinkButton)

			Dim index As Integer = Convert.ToInt32(e.Row.RowIndex)
			'Dim dbId As Integer = grid.DataKeys.Item(index).Value
			Dim dbId As Integer = 56044

			' Read the underlying DataSet of the grid
			Dim docDataSet As DataSet = CType(grid.DataSource, DataSet)

			' Find the correct DataRow
			Dim row As DataRow = docDataSet.Tables(0).Rows().Find(dbId)

			Dim profession As String = row("MA_Beruf") ' String.Format("{0} {2}", row("MA_Vorname"), row("MA_Nachname")) 
			profession = profession.Replace("#", ", ")
			Dim toolTip As String = profession

			If profession.Length > 60 Then
				profession = profession.Substring(0, 60) & "..."
			End If
			lnkJobCandidateDetail.Text = Server.HtmlEncode(profession)
			lnkJobCandidateDetail.ToolTip = toolTip
			lnkJobCandidateDetail.OnClientClick = String.Format("javascript: ShowJobCandidatesDetail({0}); return false;", dbId)

			'--------- Year -----------
			Dim lblAgeGroup As Label = CType(e.Row.FindControl("lblAgeGroup"), Label)

			Dim dateOfBirth As DateTime = CType(row("MAGebDat"), DateTime)
			lblAgeGroup.Text = dateOfBirth.Year.ToString()

		End If

	End Sub


	' Returns welcome text for customer pages
	Public Overrides Function GetWelcomeText(ByVal dr As SqlDataReader) As String
		Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
			Return String.Empty
		ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
			Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHDSex", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty))
		Else
			Return String.Empty
		End If

	End Function

	Public Overrides Function GetUserName(ByVal dr As SqlDataReader) As String
		Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

		If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
			Return Utility.GetColumnTextStr(dr, "KD_Name", String.Empty)
		ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
			Return String.Format("{0} {1}", Utility.GetColumnTextStr(dr, "ZHD_Nachname", String.Empty), Utility.GetColumnTextStr(dr, "ZHD_Vorname", String.Empty))
		Else
			Return String.Empty
		End If
	End Function

	Public Overrides Function GetUserEmail(ByVal dr As SqlDataReader) As String
		Dim appInfo As ApplicationInfo = CType(Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
		If (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
			Return Utility.GetColumnTextStr(dr, "KD_eMail", String.Empty)
		ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
			Return Utility.GetColumnTextStr(dr, "ZHD_eMail", String.Empty)
		Else
			Return String.Empty
		End If
	End Function

End Class
