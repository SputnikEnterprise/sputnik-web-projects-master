Imports System.Runtime.InteropServices
Imports System.Text

Namespace IBANUtil

  Module IBANToolDllDeclarations

		'Documentation: https://www.six-group.com/interbank-clearing/dam/downloads/de/standardization/iban/tool/installation-guide.pdf
		', CallingConvention:=CallingConvention.Cdecl)>

		<DllImport("IBANKernel64.dll", CallingConvention:=CallingConvention.Cdecl)>
		Public Function IT_IBANConvert(
				ByVal sKonto As String,
				ByVal sBCPC As String,
				ByVal lpIBAN As StringBuilder,
				ByVal nIBANLen As Integer,
				ByVal lpBC As StringBuilder,
				ByVal nBCLen As Integer,
				ByVal lpPC As StringBuilder,
				ByVal nPCLen As Integer,
				ByVal lpRES As StringBuilder,
				ByVal nRESLen As Integer) As Integer
		End Function

		<DllImport("IBANKernel.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Public Function IT_IBANVersion( _
        ByRef nMajor As Integer, _
        ByRef nMinor As Integer, _
        ByVal lpValidThru As StringBuilder, _
        ByVal nValidThruLen As Integer) As Integer
    End Function

  End Module

End Namespace
