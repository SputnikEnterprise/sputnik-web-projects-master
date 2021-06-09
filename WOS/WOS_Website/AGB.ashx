  <%@ WebHandler Language="VB" Class="AGB" %>

Imports System
Imports System.Web
Imports System.Data
Imports WOS_Website


''' <summary>
''' AGB handler
''' </summary>
Public Class AGB : Implements IHttpHandler
	Implements SessionState.IReadOnlySessionState


	' Hinzugefügt durch Sputnik 1.5.11  
	' Application Info
	Dim _ApplicationInfo As ApplicationInfo
	Protected ReadOnly Property ApplicationInfo() As ApplicationInfo
		Get
			Return _ApplicationInfo
		End Get
	End Property

	' Constructor
	Public Sub New()
		_ApplicationInfo = New ApplicationInfo()
	End Sub
	' Ende...

	''' <summary>
	'''Entry point for handler request
	''' </summary>
	Public Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest
		SendAGBToCustomer(context.Response)
	End Sub

	''' <summary>
	''' Sends a the AGB (terms and conditions) to the users browser.
	''' </summary>
	Public Shared Sub SendAGBToCustomer(ByVal Response As HttpResponse)

		' Connection to the database
		Dim conn As New System.Data.SqlClient.SqlConnection
		conn.ConnectionString = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString

		' SQL Command
		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand()
		cmd.CommandType = CommandType.Text
		cmd.CommandText = "SELECT TOP 1 Customer_AGB FROM MySetting Where ID = @CustomerId"

		'hinzugefügt durch Sputnik 1.5.11
		Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)
		Dim strQuery As String = String.Empty
		Dim strMySettingQuery As String

		strMySettingQuery = "MySetting.[ID], "
		strMySettingQuery &= "MySetting.Customer_AGB, "
		strMySettingQuery &= "isnull(MySetting.Customer_AGB_I, MySetting.Customer_AGB) As Customer_AGB_I, "
		strMySettingQuery &= "Isnull(MySetting.Customer_AGB_f, MySetting.Customer_AGB) As Customer_AGB_f, "
		strMySettingQuery &= "ISNull(MySetting.Customer_AGB_E, Customer_AGB) As Customer_AGB_E, "

		strMySettingQuery &= "IsNull(MySetting.Customer_AGBFest, Customer_AGB) As Customer_AGBFest, "
		strMySettingQuery &= "isnull(MySetting.Customer_AGBFest_I, MySetting.Customer_AGB) As Customer_AGBFest_I, "
		strMySettingQuery &= "Isnull(MySetting.Customer_AGBFest_f, MySetting.Customer_AGB) As Customer_AGBFest_f, "
		strMySettingQuery &= "ISNull(MySetting.Customer_AGBFest_E, Customer_AGB) As Customer_AGBFest_E, "

		strMySettingQuery &= "IsNull(MySetting.Customer_AGBSonst, Customer_AGB) As Customer_AGBSonst, "
		strMySettingQuery &= "isnull(MySetting.Customer_AGBSonst_I, MySetting.Customer_AGB) As Customer_AGBSonst_I, "
		strMySettingQuery &= "Isnull(MySetting.Customer_AGBSonst_f, MySetting.Customer_AGB) As Customer_AGBSonst_f, "
		strMySettingQuery &= "ISNull(MySetting.Customer_AGBSonst_E, Customer_AGB) As Customer_AGBSonst_E "


		If appInfo.CurrentGuidName.ToLower = "sp" Then
			strQuery = "SELECT TOP 1 MA.MA_Language As MyLanguage, MA.Customer_ID, ISNull(MA.AGB_Wos, '') As AGBArt, "
			strQuery &= strMySettingQuery

			strQuery &= "FROM Kandidaten MA "
			strQuery &= "Left Join MySetting  On MA.WOS_Guid = MySetting.WOS_Guid "
			strQuery &= "Where MA.MA_Guid = @KD_Guid "

		ElseIf appInfo.CurrentGuidName.ToLower = "kd" Then
			strQuery = "SELECT TOP 1 KD.KD_Language As MyLanguage, KD.Customer_ID, IsNull(KD.KD_AGB_Wos, '') As AGBArt, "
			strQuery &= strMySettingQuery

			strQuery &= "FROM Kunden KD "
			strQuery &= "Left Join MySetting  On KD.WOS_Guid = MySetting.WOS_Guid "
			strQuery &= "Where KD.kd_Guid = @KD_Guid "

		ElseIf appInfo.CurrentGuidName.ToLower = "zhd" Then
			strQuery = "SELECT TOP 1 KD.KD_Language As MyLanguage, KDZ.Customer_ID, IsNull(KDZ.ZHD_AGB_Wos, '') As AGBArt, "
			strQuery &= strMySettingQuery

			strQuery &= "FROM Kunden_ZHD KDZ "
			strQuery &= "Left Join Kunden KD On KDZ.Customer_ID = KD.Customer_ID And KDZ.KD_Guid = KD.KD_Guid "
			strQuery &= "Left Join MySetting  On KDZ.WOS_Guid = MySetting.WOS_Guid "
			strQuery &= "Where KDZ.ZHD_Guid = @KD_Guid "

		End If
		cmd.CommandText = strQuery
		' Ende

		cmd.Connection = conn

		' Take the customer id form the session
		Dim customerId As String = HttpContext.Current.Session("CustomerId")

		' Make sure the customer id is set
		If Not customerId Is Nothing And strQuery <> String.Empty Then

			Dim param As System.Data.SqlClient.SqlParameter
			'      param = cmd.Parameters.AddWithValue("@CustomerId", customerId)
			param = cmd.Parameters.AddWithValue("@KD_Guid", appInfo.CurrentGuidValue)

			Dim dr As System.Data.SqlClient.SqlDataReader = Nothing
			Try
				' Open connection to database and read data
				conn.Open()
				dr = cmd.ExecuteReader()
				dr.Read()

				Dim strLanguage As String = Utility.GetColumnTextStr(dr, "MyLanguage", String.Empty)
				Dim strAGBArt As String = Utility.GetColumnTextStr(dr, "AGBArt", "0")
				Dim strAGBLangname As String = String.Empty

				If strAGBArt.StartsWith("1") Then
					strAGBArt = "Fest" ' Customer_AGBFest, Customer_AGBFest
				ElseIf strAGBArt.StartsWith("2") Then
					strAGBArt = "Sonst" ' Customer_AGBSonst, Customer_AGBSonst
				Else
					strAGBArt = String.Empty
				End If

				' italienisch, französich, englisch
				If strLanguage.ToLower.StartsWith("i") Or _
								strLanguage.ToLower.StartsWith("f") Or _
								strLanguage.ToLower.StartsWith("e") Then
					strAGBLangname = "_" & strLanguage.Substring(0, 1) ' Customer_AGB_I, Customer_AGB_F, Customer_AGB_E
				Else
					strAGBLangname = String.Empty
				End If

				Dim strAGBColumnname As String = String.Format("Customer_AGB{0}{1}", strAGBArt, strAGBLangname)  ' AGB in deutsche
				Dim logoData As Byte() = dr.Item(strAGBColumnname)
				dr.Close()
				Response.ContentType = "application/pdf"
				Response.AddHeader("content-disposition", "filename=" + HttpContext.Current.Server.HtmlEncode("AGB.pdf"))
				Response.AppendHeader("content-length", logoData.Length)
				Response.BinaryWrite(logoData)
				Response.Flush()
				'Response.Write(String.Format("strAGBArt: {0} strAGBLangname: {1} strAGBColumnname:{2} {3} strQuery: {4}", _
				'                                     strAGBArt, strAGBLangname, strAGBColumnname, "<p>", strQuery))

			Finally
				'If (Not (dr Is Nothing Or dr.IsClosed)) Then
				'  dr.Close()
				'End If

				conn.Close()
				conn.Dispose()
			End Try
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