Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports wsSPS_Services.IBANUtil.SwissIBANEncoder
Imports wsSPS_Services.IBANUtil
Imports wsSPS_Services.IBANUtil.IBANDecoder
Imports System.Data.SqlClient
Imports wsSPS_Services.Logging
Imports wsSPS_Services.SPUtilities


' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPIBANUtil.asmx/")>
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)>
<ToolboxItem(False)>
Public Class SPIBANUtil
	Inherits System.Web.Services.WebService


#Region "private consts"

	Private Const ASMX_SERVICE_NAME As String = "SPIBANUtil"

#End Region

	''' <summary>
	''' The logger.
	''' </summary>
	Protected m_Logger As ILogger = New Logger()

	Dim _UserInfo As String = String.Empty
	Private m_utility As ClsUtilities
	Private m_customerID As String


	Public Sub New()

		m_utility = New ClsUtilities
		m_customerID = String.Empty

	End Sub


	''' <summary>
	''' Encodes a swizerland (or liechtenstein) clearing number and account number to an IBAN.
	''' </summary>
	''' <param name="bcPC_Or_SwiftBicNumber">The clearing number or swift bic number</param>
	''' <param name="accountNumber">The accountn umber.</param>
	''' <returns>The convert result.</returns>
	<WebMethod()>
	Public Function EncodeSwissIBAN(ByVal bcPC_Or_SwiftBicNumber As String, ByVal accountNumber As String) As IBANConvertResult

		Dim ibanEncoder As New SwissIBANEncoder
		Dim encodeResult As IBANConvertResult = Nothing

		Try
			encodeResult = ibanEncoder.ConvertToIBAN("", bcPC_Or_SwiftBicNumber, accountNumber)
		Catch ex As Exception
			encodeResult = New IBANConvertResult With {.Success = False}
			Dim errorMessage As String = String.Format("EncodeSwissIBAN: ClearingNummer={0}, AccountNumber={1}", bcPC_Or_SwiftBicNumber, accountNumber)
			Dim msgContent = String.Format("{1}{0}{2}", vbNewLine, errorMessage, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "EncodeSwissIBAN", .MessageContent = msgContent})

		End Try

		Return encodeResult

	End Function

	''' <summary>
	''' Gets the IBANEncode DLL version info.
	''' </summary>
	''' <returns>IBAN encode version info.</returns>
	<WebMethod()>
	Public Function IBANEncodeDLLVersionInfo() As IBANVersionResult

		Dim ibanEncoder As New SwissIBANEncoder
		Dim ibanVersion As IBANVersionResult = Nothing

		Try
			ibanVersion = ibanEncoder.IBANVersion
		Catch ex As Exception
			Dim msgContent = String.Format("IBANEncodeDLLVersionInfo: {0}", ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "IBANEncodeDLLVersionInfo", .MessageContent = msgContent})
		End Try

		Return ibanVersion

	End Function

	''' <summary>
	''' Decodes an IBAN.
	''' </summary>
	''' <param name="iban">The iban.</param>
	''' <returns>The IBAN decode result.</returns>
	<WebMethod()>
	Public Function DecodeIBAN(ByVal iban As String) As IBANDecodeResult

		Dim ibanDecoder As New IBANDecoder()
		Dim decodeResult As IBANDecodeResult = Nothing

		Try
			decodeResult = ibanDecoder.DecodeIBAN(iban)
		Catch ex As Exception
			decodeResult = New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.Failure}
			Dim errorMessage As String = String.Format("DecodeIBAN: IBAN={0}", iban)
			Dim msgContent = String.Format("{1}{0}{2}", vbNewLine, errorMessage, ex.ToString)
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DecodeIBAN", .MessageContent = msgContent})

		End Try

		Return decodeResult

	End Function


End Class