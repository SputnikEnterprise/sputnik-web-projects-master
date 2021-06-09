Imports System.Text
Imports wsSPS_Services.SPUtilities

Namespace IBANUtil

	''' <summary>
	''' Wrapper class for Six IBAN Tool functions.
	''' </summary>
	''' <remarks>http://www.six-interbank-clearing.com/dam/downloads/de/standardization/iban/tool/ibantool_fi_swh.pdf</remarks>
	Public Class SwissIBANEncoder

		Private Const ASMX_SERVICE_NAME As String = "SwissIBANEncoder"

		Private m_utility As ClsUtilities


#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		Public Sub New()
			m_utility = New ClsUtilities
		End Sub

#End Region


#Region "Properties"

		''' <summary>
		''' Gets the IBAN Version and valid until date.
		''' </summary>
		''' <returns>IBAN versions and valid until dates.</returns>
		''' <remarks>The function uses the IBANKernel.dll.</remarks>
		Public ReadOnly Property IBANVersion As IBANVersionResult

			Get
				Dim sbValidThru As StringBuilder = New StringBuilder(100)
				Dim nMajor As Integer = 0
				Dim nMinor As Integer = 0
				Dim nResult As Integer = IT_IBANVersion(nMajor,
				nMinor,
				sbValidThru,
				sbValidThru.Capacity)

				Dim returnValue As New IBANVersionResult With {
					.MajorVersion = nMajor,
					.MinorVersion = nMinor,
					.ValidUntil = sbValidThru.ToString()
				}

				Return returnValue

			End Get
		End Property

#End Region

#Region "Methods"

		''' <summary>
		''' Converts an BC/PC-Nr. /SWIFT-BIC and account number to an IBAN number.
		''' </summary>
		''' <returns>IBAN versions and valid until dates.</returns>
		''' <remarks>The function uses the IBANKernel.dll.</remarks>
		Public Function ConvertToIBAN(ByVal customerID As String, ByVal bcPC_Or_SwiftBicNumber As String, ByVal accountNumber As String) As IBANConvertResult

			Dim sbIBAN As StringBuilder = New StringBuilder(255)
			Dim sbBC As StringBuilder = New StringBuilder(32)
			Dim sbPC As StringBuilder = New StringBuilder(32)
			Dim sbRES As StringBuilder = New StringBuilder(32)
			Dim nResult As Integer
			Try

				nResult = IT_IBANConvert(accountNumber, bcPC_Or_SwiftBicNumber, sbIBAN, sbIBAN.Capacity, sbBC, sbBC.Capacity, sbPC, sbPC.Capacity, sbRES, sbRES.Capacity)

				Dim returnValue As New IBANConvertResult With
					{
					.IBAN = sbIBAN.ToString(),
					.PC = sbPC.ToString(),
					.ResultCode = nResult,
					.Success = (nResult >= 1 And nResult <= 9)
					}

				Return returnValue

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "ConvertToIBAN", .MessageContent = msgContent})

				Return Nothing
			End Try


		End Function

#End Region

#Region "Result Types"

		''' <summary>
		''' IBAN Version result.
		''' </summary>
		Public Class IBANVersionResult
			Public Property MajorVersion As Integer
			Public Property MinorVersion As Integer
			Public Property ValidUntil As String
		End Class

		''' <summary>
		''' IBAN convert result.
		''' </summary>
		Public Class IBANConvertResult
			Public Property IBAN As String
			Public Property PC As String
			Public Property ResultCode As Integer
			Public Property Success As Boolean
		End Class

#End Region

	End Class

End Namespace
