Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports wsSPS_Services_x86.IBANUtil.SwissIBANEncoder
Imports wsSPS_Services_x86.IBANUtil
Imports wsSPS_Services_x86.IBANUtil.IBANDecoder
Imports System.Data.SqlClient

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services_x86/SPIBANUtil.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPIBANUtil
  Inherits System.Web.Services.WebService

  Dim _UserInfo As String = String.Empty

  ''' <summary>
  ''' Encodes a swizerland (or liechtenstein) clearing number and account number to an IBAN.
  ''' </summary>
  ''' <param name="bcPC_Or_SwiftBicNumber">The clearing number or swift bic number</param>
  ''' <param name="accountNumber">The accountn umber.</param>
  ''' <returns>The convert result.</returns>
  <WebMethod()> _
  Public Function EncodeSwissIBAN(ByVal bcPC_Or_SwiftBicNumber As String, ByVal accountNumber As String) As IBANConvertResult

    Dim ibanEncoder As New SwissIBANEncoder
    Dim encodeResult As IBANConvertResult = Nothing

    Try
      encodeResult = ibanEncoder.ConvertToIBAN(bcPC_Or_SwiftBicNumber, accountNumber)
    Catch ex As Exception
      encodeResult = New IBANConvertResult With {.Success = False}
      Dim errorMessage As String = String.Format("Fehler: EncodeSwissIBAN: ClearingNummer={0}, AccountNumber={1}, Exception={2}", bcPC_Or_SwiftBicNumber, accountNumber, ex.ToString())
      SaveErrToDb(errorMessage)
    End Try

    Return encodeResult

  End Function

  ''' <summary>
  ''' Gets the IBANEncode DLL version info.
  ''' </summary>
  ''' <returns>IBAN encode version info.</returns>
  <WebMethod()> _
  Public Function IBANEncodeDLLVersionInfo() As IBANVersionResult

    Dim ibanEncoder As New SwissIBANEncoder
    Dim ibanVersion As IBANVersionResult = Nothing

    Try
      ibanVersion = ibanEncoder.IBANVersion
    Catch ex As Exception
      Dim errorMessage As String = String.Format("Fehler: IBANEncodeDLLVersionInfo: {0}", ex.Message)
      SaveErrToDb(errorMessage)
    End Try

    Return ibanVersion

  End Function

  ''' <summary>
  ''' Decodes an IBAN.
  ''' </summary>
  ''' <param name="iban">The iban.</param>
  ''' <returns>The IBAN decode result.</returns>
  <WebMethod()> _
  Public Function DecodeIBAN(ByVal iban As String) As IBANDecodeResult

    Dim ibanDecoder As New IBANDecoder()
    Dim decodeResult As IBANDecodeResult = Nothing

    Try
      decodeResult = ibanDecoder.DecodeIBAN(iban)
    Catch ex As Exception
      decodeResult = New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.Failure}
      Dim errorMessage As String = String.Format("Fehler: DecodeIBAN: IBAN={0}, Exception={1}", iban, ex.ToString())
      SaveErrToDb(errorMessage)
    End Try

    Return decodeResult

  End Function

  ''' <summary>
  ''' Saves an error into the database.
  ''' </summary>
  Private Sub SaveErrToDb(ByVal strErrorMessage As String)
    Dim connString As String = My.Settings.SettingDb_Connection
    Dim strSQL As String = String.Empty
    strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
    strSQL &= "CreatedOn) Values ("
    strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

    Dim Conn As SqlConnection = New SqlConnection(connString)
    Conn.Open()

    Try
      Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
      cmd.CommandType = Data.CommandType.Text
      Dim param As System.Data.SqlClient.SqlParameter
      param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPIBANUtil.asmx")
      param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
      param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
      param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
      param = cmd.Parameters.AddWithValue("@RequestParam", String.Empty)
      param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

      cmd.ExecuteNonQuery()

    Catch ex As Exception
      ' Do nothing

    Finally

      If Not Conn Is Nothing Then
        Conn.Close()
        Conn.Dispose()
      End If

    End Try
  End Sub

End Class