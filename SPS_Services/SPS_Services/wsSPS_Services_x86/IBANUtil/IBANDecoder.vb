Imports System.Text

Namespace IBANUtil

  ''' <summary>
  ''' IBAN Decoder.
  ''' 
  ''' IBAN validity check is taken from http://www.tsql.de/csharp/csharp_iban_validieren_iban_testen_iban_code and converted to VB.Net.
  ''' </summary>
  ''' <remarks>Converted to VB.Net with http://converter.telerik.com</remarks>
  Public Class IBANDecoder

#Region "Private Fields"

    Private m_SupportedCountryCodes As String()
    Private m_IBAN_Patterns As Dictionary(Of String, IBANLengthInfo)

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      ' The information for the supported country codes and iban section length is taken from IBAN-Prüfzifferrechner JavaScript Code (laenderoptions.js) 
      ' http://www.six-interbank-clearing.com/de/home/standardization/iban/pruefzifferrechner.html

      m_SupportedCountryCodes = New String() {
          "AL", "AD", "AT", "BA", "BE", "BG", "CH", "CY", "CZ", "DE",
          "DK", "EE", "ES", "FI", "FO", "FR", "GL", "GB", "GI", "GR",
          "HR", "HU", "IE", "IL", "IS", "IT", "LI", "LT", "LU", "LV",
          "MC", "ME", "MK", "MT", "MU", "NL", "NO", "PL", "PT", "RO",
          "RS", "SA", "SE", "SI", "SK", "SM", "TN", "TR"
      }

      m_IBAN_Patterns = New Dictionary(Of String, IBANLengthInfo)

      m_IBAN_Patterns.Add("AL", New IBANLengthInfo("AL", 8, 16))
      m_IBAN_Patterns.Add("AD", New IBANLengthInfo("AD", 8, 12))
      m_IBAN_Patterns.Add("AT", New IBANLengthInfo("AT", 5, 11))
      m_IBAN_Patterns.Add("BA", New IBANLengthInfo("BA", 6, 10))
      m_IBAN_Patterns.Add("BE", New IBANLengthInfo("BE", 3, 9))
      m_IBAN_Patterns.Add("BG", New IBANLengthInfo("BG", 8, 10))
      m_IBAN_Patterns.Add("CH", New IBANLengthInfo("CH", 5, 12))
      m_IBAN_Patterns.Add("CY", New IBANLengthInfo("CY", 8, 16))
      m_IBAN_Patterns.Add("CZ", New IBANLengthInfo("CZ", 4, 16))
      m_IBAN_Patterns.Add("DE", New IBANLengthInfo("DE", 8, 10))
      m_IBAN_Patterns.Add("DK", New IBANLengthInfo("DK", 4, 10))
      m_IBAN_Patterns.Add("EE", New IBANLengthInfo("EE", 4, 12))
      m_IBAN_Patterns.Add("ES", New IBANLengthInfo("ES", 8, 12))
      m_IBAN_Patterns.Add("FI", New IBANLengthInfo("FI", 6, 8))
      m_IBAN_Patterns.Add("FO", New IBANLengthInfo("FO", 4, 10))
      m_IBAN_Patterns.Add("FR", New IBANLengthInfo("FR", 10, 13))
      m_IBAN_Patterns.Add("GL", New IBANLengthInfo("GL", 4, 10))
      m_IBAN_Patterns.Add("GB", New IBANLengthInfo("GB", 10, 8))
      m_IBAN_Patterns.Add("GI", New IBANLengthInfo("GI", 4, 15))
      m_IBAN_Patterns.Add("GR", New IBANLengthInfo("GR", 7, 16))
      m_IBAN_Patterns.Add("HR", New IBANLengthInfo("HR", 7, 10))
      m_IBAN_Patterns.Add("HU", New IBANLengthInfo("HU", 7, 17))
      m_IBAN_Patterns.Add("IE", New IBANLengthInfo("IE", 10, 8))
      m_IBAN_Patterns.Add("IL", New IBANLengthInfo("IL", 6, 13))
      m_IBAN_Patterns.Add("IS", New IBANLengthInfo("IS", 4, 18))
      m_IBAN_Patterns.Add("IT", New IBANLengthInfo("IT", 11, 12))
      m_IBAN_Patterns.Add("LI", New IBANLengthInfo("LI", 5, 12))
      m_IBAN_Patterns.Add("LT", New IBANLengthInfo("LT", 5, 11))
      m_IBAN_Patterns.Add("LU", New IBANLengthInfo("LU", 3, 13))
      m_IBAN_Patterns.Add("LV", New IBANLengthInfo("LV", 4, 13))
      m_IBAN_Patterns.Add("MC", New IBANLengthInfo("MC", 10, 13))
      m_IBAN_Patterns.Add("ME", New IBANLengthInfo("ME", 3, 15))
      m_IBAN_Patterns.Add("MK", New IBANLengthInfo("MK", 3, 12))
      m_IBAN_Patterns.Add("MT", New IBANLengthInfo("MT", 9, 18))
      m_IBAN_Patterns.Add("MU", New IBANLengthInfo("MU", 8, 18))
      m_IBAN_Patterns.Add("NL", New IBANLengthInfo("NL", 4, 10))
      m_IBAN_Patterns.Add("NO", New IBANLengthInfo("NO", 4, 7))
      m_IBAN_Patterns.Add("PL", New IBANLengthInfo("PL", 8, 16))
      m_IBAN_Patterns.Add("PT", New IBANLengthInfo("PT", 8, 13))
      m_IBAN_Patterns.Add("RO", New IBANLengthInfo("RO", 4, 16))
      m_IBAN_Patterns.Add("RS", New IBANLengthInfo("RS", 3, 15))
      m_IBAN_Patterns.Add("SA", New IBANLengthInfo("SA", 2, 18))
      m_IBAN_Patterns.Add("SE", New IBANLengthInfo("SE", 3, 17))
      m_IBAN_Patterns.Add("SI", New IBANLengthInfo("SI", 5, 10))
      m_IBAN_Patterns.Add("SK", New IBANLengthInfo("SK", 4, 16))
      m_IBAN_Patterns.Add("SM", New IBANLengthInfo("SM", 11, 12))
      m_IBAN_Patterns.Add("TN", New IBANLengthInfo("TN", 5, 15))
      m_IBAN_Patterns.Add("TR", New IBANLengthInfo("TR", 5, 17))
    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Decodes an IBAN.
    ''' </summary>
    ''' <param name="iban">The IBAN number.</param>
    ''' <returns>The IBAn decode result.</returns>
    Public Function DecodeIBAN(ByVal iban As String) As IBANDecodeResult

      If iban Is Nothing Then
        iban = String.Empty
      End If

      iban = iban.Replace(" ", "").Trim()

      If iban.Length < 5 Then
        Return New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.InvalidIBAN}
      End If

      If Not IsCountryCodeSupported(iban.Substring(0, 2)) Then
        Return New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.UnkownIBANCountryCode}
      End If

      If Not IsIBAN(iban) Then
        Return New IBANDecodeResult With {.ResultCode = IBANDecodeResultCode.InvalidIBAN}
      End If

      Dim result = DecodeIBANInternal(iban)

      Return result

    End Function

    ''' <summary>
    ''' Checks if its a valid iban.
    ''' </summary>
    ''' <param name="iban">The IBAN.</param>
    ''' <returns>Boolean flag indicating if its a valid IBAN.</returns>
    Public Function IsIBAN(ByVal iban As String) As Boolean

      If iban Is Nothing Then
        iban = String.Empty
      End If

      'Leerzeichen entfernen
      Dim mysIBAN As String = iban.Replace(" ", "")
      'Eine IBAN hat maximal 34 Stellen
      If mysIBAN.Length > 34 OrElse mysIBAN.Length < 5 Then
        Return False
      Else
        'IBAN ist zu lang oder viel zu lang
        Dim LaenderCode As String = mysIBAN.Substring(0, 2).ToUpper()
        Dim Pruefsumme As String = mysIBAN.Substring(2, 2).ToUpper()
        Dim BLZ_Konto As String = mysIBAN.Substring(4).ToUpper()

        If Not IsNumeric(Pruefsumme) Then
          Return False
        End If
        'Prüfsumme ist nicht numerisch
        If Not IsCountryCodeSupported(LaenderCode) Then
          Return False
        End If
        'Ländercode ist ungültig
        'Pruefsumme validieren
        Dim Umstellung As String = (BLZ_Konto & LaenderCode) + "00"
        Dim Modulus As String = IBANCleaner(Umstellung)
        If 98 - Modulo(Modulus, 97) <> Integer.Parse(Pruefsumme) Then
          Return False
          'Prüfsumme ist fehlerhaft 
        End If
      End If
      Return True
    End Function

#End Region

#Region "Private Function"

    ''' <summary>
    ''' Checks if country code is supported.
    ''' </summary>
    ''' <param name="code">The country code.</param>
    ''' <returns>Boolean flag indicating if the country code is supported.</returns>
    Private Function IsCountryCodeSupported(code As String) As Boolean
      If code.Length <> 2 Then
        Return False
      Else
        code = code.ToUpper()

        If Array.IndexOf(m_SupportedCountryCodes, code) = -1 Then
          Return False
        Else
          Return True
        End If
      End If
    End Function

    ''' <summary>
    ''' Replaces IBAN letters through digits.
    ''' </summary>
    Private Function IBANCleaner(sIBAN As String) As String
      For x As Integer = 65 To 90
        Dim replacewith As Integer = x - 64 + 9
        Dim replace As String = CChar(ChrW(x)).ToString()
        sIBAN = sIBAN.Replace(replace, replacewith.ToString())
      Next
      Return sIBAN
    End Function

    ''' <summary>
    ''' Special modulo implementation because of big numbers.
    ''' </summary>
    Private Function Modulo(sModulus As String, iTeiler As Integer) As Integer
      Dim iStart As Integer, iEnde As Integer, iErgebniss As Integer, iRestTmp As Integer, iBuffer As Integer
      Dim iRest As String = "", sErg As String = ""

      iStart = 0
      iEnde = 0

      While iEnde <= sModulus.Length - 1
        iBuffer = Integer.Parse(iRest & sModulus.Substring(iStart, iEnde - iStart + 1))

        If iBuffer >= iTeiler Then
          iErgebniss = iBuffer \ iTeiler
          iRestTmp = iBuffer - iErgebniss * iTeiler
          iRest = iRestTmp.ToString()

          sErg = sErg & iErgebniss.ToString()

          iStart = iEnde + 1
          iEnde = iStart
        Else
          If sErg <> "" Then
            sErg = sErg & Convert.ToString("0")
          End If

          iEnde = iEnde + 1
        End If
      End While

      If iStart <= sModulus.Length Then
        iRest = iRest & sModulus.Substring(iStart)
      End If

      Return Integer.Parse(iRest)
    End Function

    ''' <summary>
    ''' Checks if a string is numeric.
    ''' </summary>
    Private Function IsNumeric(value As String) As Boolean
      Try
        Integer.Parse(value)
        Return (True)
      Catch
        Return (False)
      End Try
    End Function

    ''' <summary>
    ''' Decodes an IBAN.
    ''' </summary>
    ''' <param name="iban">The IBAN.</param>
    ''' <returns>The decoded IBAN</returns>
    Private Function DecodeIBANInternal(ByVal iban As String) As IBANDecodeResult

      Dim result As New IBANDecodeResult

      Dim countryCode As String = iban.Substring(0, 2).ToUpper()

      If Not m_IBAN_Patterns.ContainsKey(countryCode) Then
        result.ResultCode = IBANDecodeResultCode.UnkownIBANCountryCode
        Return result
      End If

      Dim ibanPattern = m_IBAN_Patterns(countryCode)

      result.Landcode = countryCode
      result.BankID = iban.Substring(4, ibanPattern.LengthBankID)
      result.Kontonummer = iban.Substring(4 + ibanPattern.LengthBankID, ibanPattern.LengthAccountNumber)

      result.ResultCode = IBANDecodeResultCode.Success

      Return result

    End Function

#End Region

#Region "Helper Classes"

    ''' <summary>
    ''' IBAN Length info
    ''' </summary>
    Class IBANLengthInfo

      Public Sub New(ByVal countryCode As String, ByVal lengthBankID As Integer, ByVal lengthAccountNumber As Integer)
        Me.CountryCode = countryCode
        Me.LengthBankID = lengthBankID
        Me.LengthAccountNumber = lengthAccountNumber
      End Sub

      Public Property CountryCode As String
      Public Property LengthBankID As Integer
      Public Property LengthAccountNumber As Integer

    End Class

#End Region

#Region "Result Types"

    Public Enum IBANDecodeResultCode
      Success
      InvalidIBAN
      UnkownIBANCountryCode
      Failure
    End Enum

    ''' <summary>
    ''' IBAN Decode result.
    ''' </summary>
    Public Class IBANDecodeResult
      Public Property Landcode As String
      Public Property BankID As String
      Public Property Kontonummer As String
      Public Property ResultCode As IBANDecodeResultCode
    End Class

#End Region

  End Class

End Namespace
