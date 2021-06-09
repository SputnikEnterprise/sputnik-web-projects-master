Imports System.Runtime.InteropServices
Imports System.Text

Namespace IBANUtil

  Module IBANToolDllDeclarations

    'Documentation: http://www.six-interbank-clearing.com/dam/downloads/de/standardization/iban/tool/ibantool_fi_swh.pdf

    <DllImport("IBANKernel.dll", CallingConvention:=CallingConvention.Cdecl)> _
    Public Function IT_IBANConvert( _
        ByVal sKonto As String, _
        ByVal sBCPC As String, _
        ByVal lpIBAN As StringBuilder, _
        ByVal nIBANLen As Integer, _
        ByVal lpBC As StringBuilder, _
        ByVal nBCLen As Integer, _
        ByVal lpPC As StringBuilder, _
        ByVal nPCLen As Integer, _
        ByVal lpRES As StringBuilder, _
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
