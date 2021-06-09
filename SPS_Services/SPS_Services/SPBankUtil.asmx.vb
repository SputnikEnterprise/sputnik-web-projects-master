
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
Imports wsSPS_Services.IBANUtil.SwissIBANEncoder
Imports wsSPS_Services.IBANUtil
Imports wsSPS_Services.IBANUtil.IBANDecoder
Imports wsSPS_Services.Logging

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPBankUtil.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPBankUtil
	Inherits System.Web.Services.WebService

	Private Const ASMX_SERVICE_NAME As String = "SPBankUtil"

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Private m_customerID As String
	Private m_utility As ClsUtilities
	Private m_PublicData As PublicDataDatabaseAccess



	Public Sub New()

		m_utility = New ClsUtilities
		m_PublicData = New PublicDataDatabaseAccess(My.Settings.ConnStr_spPublicData, Language.German)

	End Sub

	<WebMethod()>
	Public Function HelloWorld() As String
		Return "Hello World"
	End Function


	<WebMethod(Description:="load bank data for employee")>
	Function LoadBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankPostcode As String, ByVal bankLocation As String, ByVal swift As String) As BankSearchResultDTO()
		Dim result As List(Of BankSearchResultDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of BankSearchResultDTO)
			result = m_PublicData.LoadBankData(m_customerID, clearingNumber, bankName, bankPostcode, bankLocation, swift)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadBankData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load alk data for employee")>
	Function LoadAssignedBankData(ByVal customerID As String, ByVal clearingNumber As String, ByVal bankName As String, ByVal bankLocation As String) As BankSearchResultDTO
		Dim result As BankSearchResultDTO = Nothing
		m_customerID = customerID

		Try
			result = New BankSearchResultDTO
			result = m_PublicData.LoadAssignedBankData(m_customerID, clearingNumber, bankName, bankLocation)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedBankData", .MessageContent = msgContent})
		Finally
		End Try


		' Return search data as an array.
		Return (result)
	End Function


#Region "iban data"

	''' <summary>
	''' Encodes a swizerland (or liechtenstein) clearing number and account number to an IBAN.
	''' </summary>
	<WebMethod()>
	Function EncodeSwissIBAN(ByVal customerID As String, ByVal bcPC_Or_SwiftBicNumber As String, ByVal accountNumber As String) As IBANConvertResult

		Dim ibanEncoder As New SwissIBANEncoder
		Dim encodeResult As IBANConvertResult = Nothing
		m_customerID = customerID

		Try
			encodeResult = ibanEncoder.ConvertToIBAN(customerID, bcPC_Or_SwiftBicNumber, accountNumber)
		Catch ex As Exception
			encodeResult = New IBANConvertResult With {.Success = False}
			Dim msgContent As String = String.Format("Fehler: EncodeSwissIBAN: ClearingNummer={0}, AccountNumber={1}, Exception={2}", bcPC_Or_SwiftBicNumber, accountNumber, ex.ToString())
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "EncodeSwissIBAN", .MessageContent = msgContent})
		End Try

		Return encodeResult

	End Function

	''' <summary>
	''' Gets the IBANEncode DLL version info.
	''' </summary>
	<WebMethod()>
	Function IBANEncodeDLLVersionInfo(ByVal customerID As String) As IBANVersionResult

		Dim ibanEncoder As New SwissIBANEncoder
		Dim ibanVersion As IBANVersionResult = Nothing
		m_customerID = customerID

		Try
			ibanVersion = ibanEncoder.IBANVersion
		Catch ex As Exception
			Dim msgContent As String = String.Format("Fehler: IBANEncodeDLLVersionInfo: {0}", ex.Message)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "IBANEncodeDLLVersionInfo", .MessageContent = msgContent})
		End Try

		Return ibanVersion

	End Function

	''' <summary>
	''' Decodes an IBAN.
	''' </summary>
	<WebMethod()>
	Function DecodeIBAN(ByVal customerID As String, ByVal iban As String) As IBANDecodeResult

		Dim ibanDecoder As New IBANDecoder()
		Dim decodeResult As IBANDecodeResult = Nothing
		m_customerID = customerID

		Try
			decodeResult = ibanDecoder.DecodeIBAN(iban)
		Catch ex As Exception
			decodeResult = New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.Failure}
			Dim msgContent As String = String.Format("Fehler: DecodeIBAN: IBAN={0}, Exception={1}", iban, ex.ToString())
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DecodeIBAN", .MessageContent = msgContent})
		End Try

		Return decodeResult

	End Function


#End Region


	''' <summary>
	''' Gets the bank search data by clearing number.
	''' </summary>
	''' <param name="clearingNumber">The clearing number.</param>
	''' <returns>Array containing search results.</returns>
	<WebMethod(Description:="Zur Auflistung der Bankdaten auf der Client")>
	Function GetBankDataByClearingNumber(ByVal clearingNumber As String, ByVal location As String) As BankSearchResultDTO


		Dim connString As String = My.Settings.ConnStr_spPublicData
		Dim conn As SqlConnection = New SqlConnection(connString)
		Dim reader As SqlClient.SqlDataReader = Nothing

		Dim listOfBankSearchResultDTO As BankSearchResultDTO = Nothing
		Try

			' Create command.
			Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Get Search Bank Data By ClearingNumber]", conn)
			cmd.CommandType = CommandType.StoredProcedure
			cmd.Parameters.AddWithValue("@ClearingNumber", If(clearingNumber Is Nothing, String.Empty, clearingNumber))

			' Open connection to database.
			conn.Open()

			' Execute the data reader.
			reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

			' Read all data.
			If (reader.Read()) Then

				listOfBankSearchResultDTO = New BankSearchResultDTO With {
					.ClearingNumber = reader.GetString(reader.GetOrdinal("ClearingNr")),
					.BankName = reader.GetString(reader.GetOrdinal("BankName")),
					.BanName = reader.GetString(reader.GetOrdinal("BankName")),
					.Postcode = reader.GetString(reader.GetOrdinal("BankPLZ")),
					.Location = reader.GetString(reader.GetOrdinal("BankOrt")),
					.Swift = reader.GetString(reader.GetOrdinal("Swift")),
					.Telephone = reader.GetString(reader.GetOrdinal("Telefon")),
					.Telefax = reader.GetString(reader.GetOrdinal("Telefax")),
					.PostAccount = reader.GetString(reader.GetOrdinal("Postkonto"))
				}

			End If

		Catch ex As Exception
			Dim msgContent = String.Format("clearingNumber: {1} | location: {2}{0}{3}", vbNewLine, clearingNumber, location, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetBankDataByClearingNumber", .MessageContent = msgContent})

		Finally
			If Not reader Is Nothing Then

				Try
					reader.Close()
				Catch
					' Do nothing
				End Try

			End If
		End Try

		' Return search data
		Return listOfBankSearchResultDTO
	End Function


	'''' <summary>
	'''' Saves error to database.
	'''' </summary>
	'''' <param name="strErrorMessage">The error message.</param>
	'Sub SaveErrToDb(ByVal strErrorMessage As String)
	'	Dim connString As String = My.Settings.ConnStr_New_spContract
	'	Dim strSQL As String = String.Empty
	'	strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'	strSQL &= "CreatedOn) Values ("
	'	strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, GetDate())"

	'	Dim Conn As SqlConnection = New SqlConnection(connString)
	'	Conn.Open()

	'	Try
	'		Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'		cmd.CommandType = Data.CommandType.Text
	'		Dim param As System.Data.SqlClient.SqlParameter
	'		param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPBankUtil.asmx")
	'		param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
	'		param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
	'		param = cmd.Parameters.AddWithValue("@RequestParam", String.Empty)

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

End Class