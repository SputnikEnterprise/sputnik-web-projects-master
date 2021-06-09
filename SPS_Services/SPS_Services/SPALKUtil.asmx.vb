
Imports System.Web.Services
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.Notify
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.CVLizer
Imports wsSPS_Services.Notification
Imports wsSPS_Services.DataTransferObject.Notification.DataObjects
Imports wsSPS_Services.SystemInfo
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects



' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPALKUtil.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPALKUtil
	Inherits System.Web.Services.WebService


	Private Const ASMX_SERVICE_NAME As String = "SPALKUtil"

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_PublicData As PublicDataDatabaseAccess



	Public Sub New()

		m_utility = New ClsUtilities
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub


	<WebMethod(Description:="load alk data for employee")>
	Function GetALKData() As ALKResultDTO()
		Dim result As List(Of ALKResultDTO) = Nothing
		m_customerID = String.Empty

		Try
			result = New List(Of ALKResultDTO)
			result = m_PublicData.LoadALKData(m_customerID)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetALKData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load alk data for employee")>
	Function GetALKDataByALKNumber(ByVal ALKNumber As Integer?) As ALKResultDTO
		Dim result As ALKResultDTO = Nothing
		m_customerID = String.Empty

		Try
			result = New ALKResultDTO
			result = m_PublicData.LoadAssignedALKData(m_customerID, ALKNumber)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetALKDataByALKNumber", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)
	End Function


	'''' <summary>
	'''' Gets the ALK data.
	'''' </summary>
	'''' <returns>Array containing search results.</returns>
	'<WebMethod(Description:="Zur Auflistung der ALK-Daten auf der Client")> _
	'Function GetALKData() As ALKResultDTO()

	'	Dim connString As String = My.Settings.ConnStr_ServiceUtil
	'	Dim conn As SqlConnection = New SqlConnection(connString)
	'	Dim reader As SqlClient.SqlDataReader = Nothing

	'	Dim listOfALKSearchResultDTO As List(Of ALKResultDTO) = Nothing
	'	Try

	'		' Create command.
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get ALK Data]", conn)
	'		cmd.CommandType = CommandType.StoredProcedure

	'		' Open connection to database.
	'		conn.Open()

	'		' Execute the data reader.
	'		reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

	'		listOfALKSearchResultDTO = New List(Of ALKResultDTO)

	'		' Read all data.
	'		While (reader.Read())

	'			Dim dto As New ALKResultDTO With {
	'				.ALKNumber = SafeGetInteger(reader, "KassenNr", 0),
	'				.ALKName = SafeGetString(reader, "KassenName"),
	'				.POBox = SafeGetString(reader, "Postfach"),
	'				.Street = SafeGetString(reader, "Strasse"),
	'				.Postcode = SafeGetInteger(reader, "PLZ", 0),
	'				.Location = SafeGetString(reader, "Ort"),
	'				.Telephone = SafeGetString(reader, "Telefon"),
	'				.Telefax = SafeGetString(reader, "TeleFax"),
	'				.EMail = SafeGetString(reader, "EMail")
	'			}

	'			listOfALKSearchResultDTO.Add(dto)

	'		End While

	'	Catch ex As Exception
	'		SaveErrToDb(String.Format("Fehler {1}:{0}{2}", vbNewLine, "GetALKData", ex.Message))
	'	Finally
	'		If Not reader Is Nothing Then

	'			Try
	'				reader.Close()
	'			Catch
	'				' Do nothing
	'			End Try

	'		End If
	'	End Try

	'	' Return search data as an array.
	'	Return listOfALKSearchResultDTO.ToArray()
	'End Function

	'''' <summary>
	'''' Gets the ALK search data by ALK number.
	'''' </summary>
	'''' <param name="ALKNumber">The clearing number.</param>
	'''' <returns>Array containing search results.</returns>
	'<WebMethod(Description:="Zur Auflistung der ALK-Daten auf der Client")> _
	'Function GetALKDataByALKNumber(ByVal ALKNumber As Integer?) As ALKResultDTO

	'	Dim connString As String = My.Settings.ConnStr_ServiceUtil
	'	Dim conn As SqlConnection = New SqlConnection(connString)
	'	Dim reader As SqlClient.SqlDataReader = Nothing

	'	Dim listOfALKSearchResultDTO As ALKResultDTO = Nothing
	'	Try

	'		' Create command.
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get Search ALK Data By ALKNumber]", conn)
	'		cmd.CommandType = CommandType.StoredProcedure
	'		cmd.Parameters.AddWithValue("@ALKNumber", If(Not ALKNumber.HasValue, 0, ALKNumber))

	'		' Open connection to database.
	'		conn.Open()

	'		' Execute the data reader.
	'		reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

	'		' Read all data.
	'		If (reader.Read()) Then

	'			listOfALKSearchResultDTO = New ALKResultDTO With {
	'				.ALKNumber = SafeGetInteger(reader, "KassenNr", 0),
	'				.ALKName = SafeGetString(reader, "KassenName"),
	'				.POBox = SafeGetString(reader, "Postfach"),
	'				.Street = SafeGetString(reader, "Strasse"),
	'				.Postcode = SafeGetInteger(reader, "PLZ", 0),
	'				.Location = SafeGetString(reader, "Ort"),
	'				.Telephone = SafeGetString(reader, "Telefon"),
	'				.Telefax = SafeGetString(reader, "TeleFax"),
	'				.EMail = SafeGetString(reader, "EMail")
	'			}

	'		End If

	'	Catch ex As Exception
	'		SaveErrToDb(String.Format("Fehler {1}:{0}{2}", vbNewLine, "GetALKDataByALKNumber", ex.Message))
	'	Finally
	'		If Not reader Is Nothing Then

	'			Try
	'				reader.Close()
	'			Catch
	'				' Do nothing
	'			End Try

	'		End If
	'	End Try

	'	' Return search data
	'	Return listOfALKSearchResultDTO
	'End Function


	'''' <summary>
	'''' Saves error to database.
	'''' </summary>
	'''' <param name="strErrorMessage">The error message.</param>
	'Sub SaveErrToDb(ByVal strErrorMessage As String)
	'	Dim connString As String = My.Settings.ConnStr_New_spContract
	'	Dim strSQL As String = String.Empty
	'	strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'	strSQL &= "CreatedOn) Values ("
	'	strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

	'	Dim Conn As SqlConnection = New SqlConnection(connString)
	'	Conn.Open()

	'	Try
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPALKUtil.asmx")
	'		param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
	'		param = cmd.Parameters.AddWithValue("@RequestParam", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

	'		cmd.ExecuteNonQuery()

	'	Catch ex As Exception
	'		' Do nothing
	'	Finally

	'		If Not Conn Is Nothing Then
	'			Conn.Close()
	'			Conn.Dispose()
	'		End If
	'	End Try

	'End Sub


	'Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

	'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

	'	If (Not reader.IsDBNull(columnIndex)) Then
	'		'Return reader.GetString(columnIndex)
	'		Return CStr(reader(columnIndex))
	'	Else
	'		Return defaultValue
	'	End If
	'End Function

	'''' <summary>
	'''' Returns a boolean or the default value if its nothing.
	'''' </summary>
	'''' <param name="reader">The reader.</param>
	'''' <param name="columnName">The column name.</param>
	'''' <param name="defaultValue">The default value.</param>
	'''' <returns>Value or default value if the value is nothing</returns>
	'Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

	'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

	'	If (Not reader.IsDBNull(columnIndex)) Then
	'		'Return reader.GetBoolean(columnIndex)
	'		Return CBool(reader(columnIndex))
	'	Else
	'		Return defaultValue
	'	End If
	'End Function

	'''' <summary>
	'''' Returns an integer or the default value if its nothing.
	'''' </summary>
	'''' <param name="reader">The reader.</param>
	'''' <param name="columnName">The column name.</param>
	'''' <param name="defaultValue">The default value.</param>
	'''' <returns>Value or default value if the value is nothing</returns>
	'Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

	'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

	'	If (Not reader.IsDBNull(columnIndex)) Then
	'		'Return reader.GetInt32(columnIndex)
	'		Return CInt(reader(columnIndex))
	'	Else
	'		Return defaultValue
	'	End If
	'End Function

End Class