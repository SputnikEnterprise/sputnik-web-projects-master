
Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_AvamServices.DataTransferObject.Notify
Imports System.Security.Cryptography


Namespace SPUtilities

	Partial Class ClsUtilities


#Region "Crypting and decrypting"

		Private key() As Byte = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24}
		Private iv() As Byte = {65, 110, 68, 26, 69, 178, 200, 219}

		Private lbtVector() As Byte = {240, 135, 45, 29, 101, 76, 27, 52}
		Private lscryptoKey As String = "%3/NJA98ASJAG7S634JAHZH&\GHK21&74575m823w6467KS#J578iH4HS61648@567Qq635766A836=58)73425(JKJASDKJ?238ASuD852sKJKJASDJK5ADSAKJ34WKJASJKAW4598lKJSDnFKJs"

		Private Declare Function CryptAcquireContext Lib "advapi32.dll" Alias "CryptAcquireContextA" (ByRef phProv As Integer, ByVal pszContainer As String, ByVal pszProvider As String, ByVal dwProvType As Integer, ByVal dwFlags As Integer) As Integer

		Private Declare Function CryptCreateHash Lib "advapi32.dll" (ByVal hProv As Integer, ByVal Algid As Integer, ByVal hKey As Integer, ByVal dwFlags As Integer, ByRef phHash As Integer) As Integer
		Private Declare Function CryptHashData Lib "advapi32.dll" (ByVal hHash As Integer, ByVal pbData As String, ByVal dwDataLen As Integer, ByVal dwFlags As Integer) As Integer
		Private Declare Function CryptDeriveKey Lib "advapi32.dll" (ByVal hProv As Integer, ByVal Algid As Integer, ByVal hBaseData As Integer, ByVal dwFlags As Integer, ByRef phKey As Integer) As Integer
		Private Declare Function CryptDestroyHash Lib "advapi32.dll" (ByVal hHash As Integer) As Integer
		Private Declare Function CryptEncrypt Lib "advapi32.dll" (ByVal hKey As Integer, ByVal hHash As Integer, ByVal Final As Integer, ByVal dwFlags As Integer, ByVal pbData As String, ByRef pdwDataLen As Integer, ByVal dwBufLen As Integer) As Integer
		Private Declare Function CryptDestroyKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer
		Private Declare Function CryptReleaseContext Lib "advapi32.dll" (ByVal hProv As Integer, ByVal dwFlags As Integer) As Integer
		Private Declare Function CryptDecrypt Lib "advapi32.dll" (ByVal hKey As Integer, ByVal hHash As Integer, ByVal Final As Integer, ByVal dwFlags As Integer, ByVal pbData As String, ByRef pdwDataLen As Integer) As Integer
		Private Const SERVICE_PROVIDER As String = "Microsoft Base Cryptographic Provider v1.0"
		Private Const PROV_RSA_FULL As Integer = 1
		Private Const PP_NAME As Integer = 4
		Private Const PP_CONTAINER As Integer = 6
		Private Const CRYPT_NEWKEYSET As Integer = 8
		Private Const ALG_CLASS_DATA_ENCRYPT As Integer = 24576
		Private Const ALG_CLASS_HASH As Integer = 32768
		Private Const ALG_TYPE_ANY As Integer = 0
		Private Const ALG_TYPE_STREAM As Integer = 2048
		Private Const ALG_SID_RC4 As Integer = 1
		Private Const ALG_SID_MD5 As Integer = 3
		Private Const CALG_MD5 As Integer = ((ALG_CLASS_HASH Or ALG_TYPE_ANY) Or ALG_SID_MD5)
		Private Const CALG_RC4 As Integer = ((ALG_CLASS_DATA_ENCRYPT Or ALG_TYPE_STREAM) Or ALG_SID_RC4)
		Private Const ENCRYPT_ALGORITHM As Integer = CALG_RC4
		Private Const ENCRYPT_NUMBERKEY As String = "16006833"
		Private lngCryptProvider As Integer
		Private avarSeedValues As Object
		Private lngSeedLevel As Integer
		Private lngDecryptPointer As Integer
		Private astrEncryptionKey(131) As String
		Private Const lngALPKeyLength As Integer = 8
		Public strKeyContainer As String

		Function DecryptWithALP(ByRef strData As String) As String
			Dim strALPKey As String = ""
			Dim strALPKeyMask As String = ""
			Dim lngIterator As Integer = 0
			Dim blnOscillator As Boolean
			Dim strOutput As String = ""
			Dim lngHex As Integer = 0

			If Len(strData) = 0 Then Return ""
			strALPKeyMask = Right(New String("0", lngALPKeyLength) & DoubleToBinary(CInt("&H" & Left(strData, 2))), lngALPKeyLength)
			strData = Right(strData, Len(strData) - 2)
			For lngIterator = lngALPKeyLength To 1 Step -1
				If Mid(strALPKeyMask, lngIterator, 1) = "1" Then
					strALPKey = Left(strData, 1) & strALPKey
					strData = Right(strData, Len(strData) - 1)
				Else
					strALPKey = Right(strData, 1) & strALPKey
					strData = Left(strData, Len(strData) - 1)
				End If
			Next lngIterator

			lngIterator = 0
			Do Until Len(strData) = 0
				blnOscillator = Not blnOscillator
				lngIterator = lngIterator + 1
				If lngIterator > lngALPKeyLength Then lngIterator = 1

				lngHex = IIf(blnOscillator, CInt(CDbl("&H" & Left(strData, 2)) - Asc(Mid(strALPKey, lngIterator, 1))), CInt(CDbl("&H" & Left(strData, 2)) + Asc(Mid(strALPKey, lngIterator, 1))))
				If lngHex > 255 Then
					lngHex = lngHex - 255
				ElseIf lngHex < 0 Then
					lngHex = lngHex + 255
				End If

				strOutput = strOutput & Chr(lngHex)
				strData = Right(strData, Len(strData) - 2)
			Loop
			DecryptWithALP = strOutput

		End Function

		Function DecryptWithClipper(ByVal strData As String, ByVal strCryptKey As String) As String
			Dim strDecryptionChunk As String = ""
			Dim strDecryptedText As String = ""

			On Error Resume Next

			InitCrypt(strCryptKey)
			Do Until Len(strData) < 16
				strDecryptionChunk = ""
				strDecryptionChunk = Left(strData, 16)
				strData = Right(strData, Len(strData) - 16)
				If Len(strDecryptionChunk) > 0 Then
					strDecryptedText = strDecryptedText & PerformClipperDecryption(strDecryptionChunk)
				End If
			Loop
			DecryptWithClipper = Trim(strDecryptedText)

		End Function

		Function EncryptWithClipper(ByVal strData As String, ByVal strCryptKey As String) As String
			Dim strEncryptionChunk As String = ""
			Dim strEncryptedText As String = ""

			If Len(strData) > 0 Then
				InitCrypt(strCryptKey)
				Do Until Len(strData) = 0
					strEncryptionChunk = ""
					If Len(strData) > 6 Then
						strEncryptionChunk = Left(strData, 6)
						strData = Right(strData, Len(strData) - 6)
					Else
						strEncryptionChunk = Left(strData & Space(6), 6)
						strData = ""
					End If
					If Len(strEncryptionChunk) > 0 Then _
					strEncryptedText = strEncryptedText & PerformClipperEncryption(strEncryptionChunk)

				Loop
			End If
			EncryptWithClipper = strEncryptedText

		End Function

		'Private Function DecryptNumber(ByVal strData As String) As Integer
		'  Dim lngIterator As Integer

		'  For lngIterator = 1 To 8
		'    DecryptNumber = (10 * DecryptNumber) + (Asc(Mid(strData, lngIterator, 1)) - Asc(Mid(ENCRYPT_NUMBERKEY, lngIterator, 1)))
		'  Next lngIterator

		'End Function

		Function EncryptDecrypt(ByVal strData As String, ByVal strCryptKey As String, ByVal Encrypt As Boolean) As String
			Dim lngDataLength As Integer = 0
			Dim strTempData As String = ""
			Dim lngHaslngCryptKey As Integer = 0
			Dim lngCryptKey As Integer = 0

			If lngCryptProvider = 0 Then
				Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Not connected to CSP")

				Return ""
			End If
			If CryptCreateHash(lngCryptProvider, CALG_MD5, 0, 0, lngHaslngCryptKey) = 0 Then
				Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Error during CryptCreateHash.")
			End If
			If CryptHashData(lngHaslngCryptKey, strCryptKey, Len(strCryptKey), 0) = 0 Then
				Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Error during CryptHashData.")
			End If
			If CryptDeriveKey(lngCryptProvider, ENCRYPT_ALGORITHM, lngHaslngCryptKey, 0, lngCryptKey) = 0 Then
				Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Error during CryptDeriveKey!")
			End If

			strTempData = strData
			lngDataLength = Len(strData)
			If Encrypt Then
				If CryptEncrypt(lngCryptKey, 0, 1, 0, strTempData, lngDataLength, lngDataLength) = 0 Then
					Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Error during CryptEncrypt.")
				End If

			Else
				If CryptDecrypt(lngCryptKey, 0, 1, 0, strTempData, lngDataLength) = 0 Then

					Err.Raise(vbObjectError + 999, "EncryptDecrypt", "Error during CryptDecrypt.")
				End If
			End If
			EncryptDecrypt = Mid(strTempData, 1, lngDataLength)
			If lngCryptKey <> 0 Then CryptDestroyKey(lngCryptKey)
			If lngHaslngCryptKey <> 0 Then CryptDestroyHash(lngHaslngCryptKey)

		End Function

		Private Function EncryptionCSPConnect() As Boolean

			If Len(strKeyContainer) = 0 Then
				strKeyContainer = "FastTrack"
			End If
			If CryptAcquireContext(lngCryptProvider, strKeyContainer, SERVICE_PROVIDER, PROV_RSA_FULL, CRYPT_NEWKEYSET) = 0 Then
				If CryptAcquireContext(lngCryptProvider, strKeyContainer, SERVICE_PROVIDER, PROV_RSA_FULL, 0) = 0 Then
					Err.Raise(vbObjectError + 999, "EncryptionCSPConnect", "Error during CryptAcquireContext for a new key container." & vbCrLf & "A container with this name probably already exists.")
					EncryptionCSPConnect = False

					Exit Function
				End If
			End If
			EncryptionCSPConnect = True

		End Function

		Private Function EncryptNumber(ByVal lngData As Integer) As String
			Dim lngIterator As Integer = 0
			Dim strData As String = ""
			Dim strEncryptedtext As String = ""

			strData = Format(lngData, "00000000")
			For lngIterator = 1 To 8
				strEncryptedtext = strEncryptedtext & Chr(Asc(Mid(ENCRYPT_NUMBERKEY, lngIterator, 1)) + _
																									Val(Mid(strData, lngIterator, 1)))
			Next lngIterator
			Return strEncryptedtext

		End Function

		Private Sub EncryptionCSPDisconnect()

			If lngCryptProvider <> 0 Then CryptReleaseContext(lngCryptProvider, 0)

		End Sub

		Private Sub InitCrypt(ByRef strEncryptionKey As String)

			'UPGRADE_WARNING: Array hat ein neues Verhalten. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="9B7D5ADD-D8FE-4819-A36C-6DEDAF088CC7"'
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			avarSeedValues = New Object() {"A3", "D7", "09", "83", "F8", "48", "F6", "F4", "B3", "21", "15", "78", "99", "B1", "AF", "F9", "E7", "2D", "4D", "8A", "CE", "4C", "CA", "2E", "52", "95", "D9", "1E", "4E", "38", "44", "28", "0A", "DF", "02", "A0", "17", "F1", "60", "68", "12", "B7", "7A", "C3", "E9", "FA", "3D", "53", "96", "84", "6B", "BA", "F2", "63", "9A", "19", "7C", "AE", "E5", "F5", "F7", "16", "6A", "A2", "39", "B6", "7B", "0F", "C1", "93", "81", "1B", "EE", "B4", "1A", "EA", "D0", "91", "2F", "B8", "55", "B9", "DA", "85", "3F", "41", "BF", "E0", "5A", "58", "80", "5F", "66", "0B", "D8", "90", "35", "D5", "C0", "A7", "33", "06", "65", "69", "45", "00", "94", "56", "6D", "98", "9B", "76", "97", "FC", "B2", "C2", "B0", "FE", "DB", "20", "E1", "EB", "D6", "E4", "DD", "47", "4A", "1D", "42", "ED", "9E", "6E", "49", "3C", "CD", "43", "27", "D2", "07", "D4", "DE", "C7", "67", "18", "89", "CB", "30", "1F", "8D", "C6", "8F", "AA", "C8", "74", "DC", "C9", "5D", "5C", "31", "A4", "70", "88", "61", "2C", "9F", "0D", "2B", "87", "50", "82", "54", "64", "26", "7D", "03", "40", "34", "4B", "1C", "73", "D1", "C4", "FD", "3B", "CC", "FB", "7F", "AB", "E6", "3E", "5B", "A5", "AD", "04", "23", "9C", "14", "51", "22", "F0", "29", "79", "71", "7E", "FF", "8C", "0E", "E2", "0C", "EF", "BC", "72", "75", "6F", "37", "A1", "EC", "D3", "8E", "62", "8B", "86", "10", "E8", "08", "77", "11", "BE", "92", "4F", "24", "C5", "32", "36", "9D", "CF", "F3", "A6", "BB", "AC", "5E", "6C", "A9", "13", "57", "25", "B5", "E3", "BD", "A8", "3A", "01", "05", "59", "2A", "%ç"}
			SetKey(strEncryptionKey)

		End Sub

		Private Function PerformClipperDecryption(ByVal strData As String) As String
			'UPGRADE_WARNING: Die untere Begrenzung des Arrays bytChunk wurde von 1,0 in 0,0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
			Dim bytChunk(4, 32) As String
			Dim bytCounter(32) As Byte
			Dim lngIterator As Integer
			Dim strDecryptedData As String

			On Error Resume Next

			bytChunk(1, 32) = Mid(strData, 1, 4)
			bytChunk(2, 32) = Mid(strData, 5, 4)
			bytChunk(3, 32) = Mid(strData, 9, 4)
			bytChunk(4, 32) = Mid(strData, 13, 4)
			lngSeedLevel = 32
			lngDecryptPointer = 31

			For lngIterator = 0 To 32
				bytCounter(lngIterator) = lngIterator + 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel - 1) = PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey)
				bytChunk(2, lngSeedLevel - 1) = PerformXOR(PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey), PerformXOR(bytChunk(3, lngSeedLevel), Hex(bytCounter(lngSeedLevel - 1))))
				bytChunk(3, lngSeedLevel - 1) = bytChunk(4, lngSeedLevel)
				bytChunk(4, lngSeedLevel - 1) = bytChunk(1, lngSeedLevel)
				lngDecryptPointer = lngDecryptPointer - 1
				lngSeedLevel = lngSeedLevel - 1
			Next lngIterator

			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel - 1) = PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey)
				bytChunk(2, lngSeedLevel - 1) = bytChunk(3, lngSeedLevel)
				bytChunk(3, lngSeedLevel - 1) = bytChunk(4, lngSeedLevel)
				bytChunk(4, lngSeedLevel - 1) = PerformXOR(PerformXOR(bytChunk(1, lngSeedLevel), bytChunk(2, lngSeedLevel)), Hex(bytCounter(lngSeedLevel - 1)))
				lngDecryptPointer = lngDecryptPointer - 1
				lngSeedLevel = lngSeedLevel - 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel - 1) = PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey)
				bytChunk(2, lngSeedLevel - 1) = PerformXOR(PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey), PerformXOR(bytChunk(3, lngSeedLevel), Hex(bytCounter(lngSeedLevel - 1))))
				bytChunk(3, lngSeedLevel - 1) = bytChunk(4, lngSeedLevel)
				bytChunk(4, lngSeedLevel - 1) = bytChunk(1, lngSeedLevel)
				lngDecryptPointer = lngDecryptPointer - 1
				lngSeedLevel = lngSeedLevel - 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel - 1) = PerformClipperDecryptionChunk(bytChunk(2, lngSeedLevel), astrEncryptionKey)
				bytChunk(2, lngSeedLevel - 1) = bytChunk(3, lngSeedLevel)
				bytChunk(3, lngSeedLevel - 1) = bytChunk(4, lngSeedLevel)
				bytChunk(4, lngSeedLevel - 1) = PerformXOR(PerformXOR(bytChunk(1, lngSeedLevel), bytChunk(2, lngSeedLevel)), Hex(bytCounter(lngSeedLevel - 1)))
				lngDecryptPointer = lngDecryptPointer - 1
				lngSeedLevel = lngSeedLevel - 1
			Next lngIterator

			strDecryptedData = HexToString(bytChunk(1, 0) & bytChunk(2, 0) & bytChunk(3, 0) & bytChunk(4, 0))
			If InStr(strDecryptedData, Chr(0)) > 0 Then
				strDecryptedData = Left(strDecryptedData, InStr(strDecryptedData, Chr(0)) - 1)
			End If
			PerformClipperDecryption = strDecryptedData

		End Function

		Private Function PerformClipperDecryptionChunk(ByVal strData As String, ByRef strEncryptionKey() As String) As String
			'UPGRADE_WARNING: Die untere Begrenzung des Arrays astrDecryptionLevel wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
			Dim astrDecryptionLevel(6) As String
			Dim strDecryptedString As String

			astrDecryptionLevel(5) = Mid(strData, 1, 2)
			astrDecryptionLevel(6) = Mid(strData, 3, 2)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strDecryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrDecryptionLevel(5), strEncryptionKey((4 * lngDecryptPointer) + 3)))))
			astrDecryptionLevel(4) = PerformXOR(strDecryptedString, astrDecryptionLevel(6))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strDecryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrDecryptionLevel(4), strEncryptionKey((4 * lngDecryptPointer) + 2)))))
			astrDecryptionLevel(3) = PerformXOR(strDecryptedString, astrDecryptionLevel(5))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strDecryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrDecryptionLevel(3), strEncryptionKey((4 * lngDecryptPointer) + 1)))))
			astrDecryptionLevel(2) = PerformXOR(strDecryptedString, astrDecryptionLevel(4))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strDecryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrDecryptionLevel(2), strEncryptionKey(4 * lngDecryptPointer)))))
			astrDecryptionLevel(1) = PerformXOR(strDecryptedString, astrDecryptionLevel(3))
			strDecryptedString = astrDecryptionLevel(1) & astrDecryptionLevel(2)
			PerformClipperDecryptionChunk = strDecryptedString

		End Function

		Private Function PerformClipperEncryption(ByVal strData As String) As String
			'UPGRADE_WARNING: Die untere Begrenzung des Arrays bytChunk wurde von 1,0 in 0,0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
			Dim bytChunk(4, 32) As String
			Dim lngCounter As Integer
			Dim lngIterator As Integer
			On Error Resume Next
			strData = StringToHex(strData)
			bytChunk(1, 0) = Mid(strData, 1, 4)
			bytChunk(2, 0) = Mid(strData, 5, 4)
			bytChunk(3, 0) = Mid(strData, 9, 4)
			bytChunk(4, 0) = Mid(strData, 13, 4)
			lngSeedLevel = 0
			lngCounter = 1
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel + 1) = PerformXOR(PerformXOR(PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey), bytChunk(4, lngSeedLevel)), Hex(lngCounter))
				bytChunk(2, lngSeedLevel + 1) = PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey)
				bytChunk(3, lngSeedLevel + 1) = bytChunk(2, lngSeedLevel)
				bytChunk(4, lngSeedLevel + 1) = bytChunk(3, lngSeedLevel)
				lngCounter = lngCounter + 1
				lngSeedLevel = lngSeedLevel + 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel + 1) = bytChunk(4, lngSeedLevel)
				bytChunk(2, lngSeedLevel + 1) = PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey)
				bytChunk(3, lngSeedLevel + 1) = PerformXOR(PerformXOR(bytChunk(1, lngSeedLevel), bytChunk(2, lngSeedLevel)), Hex(lngCounter))
				bytChunk(4, lngSeedLevel + 1) = bytChunk(3, lngSeedLevel)
				lngCounter = lngCounter + 1
				lngSeedLevel = lngSeedLevel + 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel + 1) = PerformXOR(PerformXOR(PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey), bytChunk(4, lngSeedLevel)), Hex(lngCounter))
				bytChunk(2, lngSeedLevel + 1) = PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey)
				bytChunk(3, lngSeedLevel + 1) = bytChunk(2, lngSeedLevel)
				bytChunk(4, lngSeedLevel + 1) = bytChunk(3, lngSeedLevel)
				lngCounter = lngCounter + 1
				lngSeedLevel = lngSeedLevel + 1
			Next lngIterator
			For lngIterator = 1 To 8
				bytChunk(1, lngSeedLevel + 1) = bytChunk(4, lngSeedLevel)
				bytChunk(2, lngSeedLevel + 1) = PerformClipperEncryptionChunk(bytChunk(1, lngSeedLevel), astrEncryptionKey)
				bytChunk(3, lngSeedLevel + 1) = PerformXOR(PerformXOR(bytChunk(1, lngSeedLevel), bytChunk(2, lngSeedLevel)), Hex(lngCounter))
				bytChunk(4, lngSeedLevel + 1) = bytChunk(3, lngSeedLevel)
				lngCounter = lngCounter + 1
				lngSeedLevel = lngSeedLevel + 1
			Next lngIterator
			PerformClipperEncryption = bytChunk(1, 32) & bytChunk(2, 32) & bytChunk(3, 32) & bytChunk(4, 32)
		End Function

		Private Function PerformClipperEncryptionChunk(ByVal strData As String, ByRef strEncryptionKey() As String) As String
			'UPGRADE_WARNING: Die untere Begrenzung des Arrays astrEncryptionLevel wurde von 1 in 0 geändert. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="0F1C9BE1-AF9D-476E-83B1-17D43BECFF20"'
			Dim astrEncryptionLevel(6) As String
			Dim strEncryptedString As String
			astrEncryptionLevel(1) = Mid(strData, 1, 2)
			astrEncryptionLevel(2) = Mid(strData, 3, 2)
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strEncryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrEncryptionLevel(2), strEncryptionKey(4 * lngSeedLevel)))))
			astrEncryptionLevel(3) = PerformXOR(strEncryptedString, astrEncryptionLevel(1))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strEncryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrEncryptionLevel(3), strEncryptionKey((4 * lngSeedLevel) + 1)))))
			astrEncryptionLevel(4) = PerformXOR(strEncryptedString, astrEncryptionLevel(2))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strEncryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrEncryptionLevel(4), strEncryptionKey((4 * lngSeedLevel) + 2)))))
			astrEncryptionLevel(5) = PerformXOR(strEncryptedString, astrEncryptionLevel(3))
			'UPGRADE_WARNING: Die Standardeigenschaft des Objekts avarSeedValues() konnte nicht aufgelöst werden. Klicken Sie hier für weitere Informationen: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
			strEncryptedString = avarSeedValues(CByte(PerformTranslation(PerformXOR(astrEncryptionLevel(5), strEncryptionKey((4 * lngSeedLevel) + 3)))))
			astrEncryptionLevel(6) = PerformXOR(strEncryptedString, astrEncryptionLevel(4))
			strEncryptedString = astrEncryptionLevel(5) & astrEncryptionLevel(6)
			PerformClipperEncryptionChunk = strEncryptedString
		End Function

		Private Function PerformTranslation(ByVal strData As String) As Double
			Dim strTranslationString As String
			Dim strTranslationChunk As String
			Dim lngTranslationIterator As Integer
			Dim lngHexConversion As Integer
			Dim lngHexConversionIterator As Integer
			Dim dblTranslation As Double
			Dim lngTranslationMarker As Integer
			Dim lngTranslationModifier As Integer
			Dim lngTranslationLayerModifier As Integer
			strTranslationString = strData
			strTranslationString = Right(strTranslationString, 8)
			strTranslationChunk = New String("0", 8 - Len(strTranslationString)) & strTranslationString
			strTranslationString = ""
			For lngTranslationIterator = 1 To 8
				lngHexConversion = Val("&H" & Mid(strTranslationChunk, lngTranslationIterator, 1))
				For lngHexConversionIterator = 3 To 0 Step -1
					If lngHexConversion And 2 ^ lngHexConversionIterator Then
						strTranslationString = strTranslationString & "1"
					Else
						strTranslationString = strTranslationString & "0"
					End If
				Next lngHexConversionIterator
			Next lngTranslationIterator
			dblTranslation = 0
			For lngTranslationIterator = Len(strTranslationString) To 1 Step -1
				If Mid(strTranslationString, lngTranslationIterator, 1) = "1" Then
					lngTranslationLayerModifier = 1
					lngTranslationMarker = (Len(strTranslationString) - lngTranslationIterator)
					lngTranslationModifier = 2
					Do While lngTranslationMarker > 0
						Do While (lngTranslationMarker / 2) = (lngTranslationMarker \ 2)
							lngTranslationModifier = (lngTranslationModifier * lngTranslationModifier) Mod 255
							lngTranslationMarker = lngTranslationMarker / 2
						Loop
						lngTranslationLayerModifier = (lngTranslationModifier * lngTranslationLayerModifier) Mod 255
						lngTranslationMarker = lngTranslationMarker - 1
					Loop
					dblTranslation = dblTranslation + lngTranslationLayerModifier
				End If
			Next lngTranslationIterator
			PerformTranslation = dblTranslation
		End Function

		Private Function PerformXOR(ByVal strData As String, ByVal strMask As String) As String
			Dim strXOR As String = ""
			Dim lngXORIterator As Integer = 0
			Dim lngXORMarker As Integer = 0

			lngXORMarker = Len(strData) - Len(strMask)
			If lngXORMarker < 0 Then
				strXOR = Left(strMask, System.Math.Abs(lngXORMarker))
				strMask = Mid(strMask, System.Math.Abs(lngXORMarker) + 1)
			ElseIf lngXORMarker > 0 Then
				strXOR = Left(strData, System.Math.Abs(lngXORMarker))
				strData = Mid(strData, lngXORMarker + 1)
			End If
			For lngXORIterator = 1 To Len(strData)
				strXOR = strXOR & Hex(Val("&H" & Mid(strData, lngXORIterator, 1)) Xor _
															Val("&H" & Mid(strMask, lngXORIterator, 1)))
			Next lngXORIterator

			Return Right(strXOR, 8)

		End Function

		Private Sub SetKey(ByVal strEncryptionKey As String)
			Dim intEncryptionKeyIterator As Short
			For intEncryptionKeyIterator = 0 To 131 Step 10
				If intEncryptionKeyIterator = 130 Then
					astrEncryptionKey(intEncryptionKeyIterator + 0) = Mid(strEncryptionKey, 1, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 1) = Mid(strEncryptionKey, 3, 2)
				Else
					astrEncryptionKey(intEncryptionKeyIterator + 0) = Mid(strEncryptionKey, 1, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 1) = Mid(strEncryptionKey, 3, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 2) = Mid(strEncryptionKey, 5, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 3) = Mid(strEncryptionKey, 7, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 4) = Mid(strEncryptionKey, 9, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 5) = Mid(strEncryptionKey, 11, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 6) = Mid(strEncryptionKey, 13, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 7) = Mid(strEncryptionKey, 15, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 8) = Mid(strEncryptionKey, 17, 2)
					astrEncryptionKey(intEncryptionKeyIterator + 9) = Mid(strEncryptionKey, 19, 2)
				End If
			Next
		End Sub

		Private Function BinaryToDouble(ByVal strData As String) As Double
			Dim dblOutput As Double
			Dim lngIterator As Integer
			Do Until Len(strData) = 0
				dblOutput = dblOutput + IIf(Right(strData, 1) = "1", (2 ^ lngIterator), 0)
				strData = Left(strData, Len(strData) - 1)
				lngIterator = lngIterator + 1
			Loop
			BinaryToDouble = dblOutput
		End Function

		Private Function DoubleToBinary(ByVal dblData As Double) As String
			Dim strOutput As String = ""
			Dim lngIterator As Integer = 0

			Do Until (2 ^ lngIterator) > dblData
				strOutput = CStr(IIf(((2 ^ lngIterator) And dblData) > 0, "1", "0") + strOutput)
				lngIterator = lngIterator + 1
			Loop
			Return strOutput

		End Function

		Private Function HexToString(ByVal strData As String) As String
			Dim strOutput As String = ""
			Do Until Len(strData) < 2
				strOutput = strOutput & Chr(CInt("&H" & Left(strData, 2)))
				strData = Right(strData, Len(strData) - 2)
			Loop
			Return strOutput
		End Function

		Private Function StringToHex(ByVal strData As String) As String
			Dim strOutput As String = ""

			Do Until Len(strData) = 0
				strOutput = strOutput & Right(New String("0", 2) & Hex(Asc(Left(strData, 1))), 2)
				strData = Right(strData, Len(strData) - 1)
			Loop
			Return strOutput

		End Function

		Private Function ByteToString(ByRef bytData() As Byte, ByVal lngDataLength As Integer) As String
			Dim lngIterator As Integer = 0
			Dim strOutput As String = ""

			For lngIterator = LBound(bytData) To (LBound(bytData) + lngDataLength)
				strOutput = strOutput & Chr(bytData(lngIterator))
			Next lngIterator
			Return strOutput

		End Function


		'Function Encrypt(ByVal plainText As String) As Byte()
		'	' Declare a UTF8Encoding object so we may use the GetByte
		'	' method to transform the plainText into a Byte array.
		'	Dim utf8encoder As UTF8Encoding = New UTF8Encoding()
		'	Dim inputInBytes() As Byte = utf8encoder.GetBytes(plainText)
		'	' Create a new TripleDES service provider 
		'	Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
		'	' The ICryptTransform interface uses the TripleDES
		'	' crypt provider along with encryption key and init vector
		'	' information 
		'	Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateEncryptor(Me.key, Me.iv)
		'	' All cryptographic functions need a stream to output the
		'	' encrypted information.  Here we declare a memory stream
		'	' for this purpose.
		'	Dim encryptedStream As MemoryStream = New MemoryStream()
		'	Dim cryptStream As CryptoStream = New CryptoStream(encryptedStream, cryptoTransform, CryptoStreamMode.Write)

		'	' Write the encrypted information to the stream.  Flush the information
		'	' when done to ensure everything is out of the buffer.
		'	cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
		'	cryptStream.FlushFinalBlock()
		'	encryptedStream.Position = 0

		'	' Read the stream back into a Byte array and return it to the calling
		'	' method.
		'	Dim result(CInt(encryptedStream.Length) - 1) As Byte
		'	encryptedStream.Read(result, 0, CInt(encryptedStream.Length))
		'	cryptStream.Close()

		'	Return result
		'End Function

		'Function Decrypt(ByVal inputInBytes() As Byte) As String
		'	' UTFEncoding is used to transform the decrypted Byte Array
		'	' information back into a string.
		'	Dim utf8encoder As UTF8Encoding = New UTF8Encoding()
		'	Dim tdesProvider As TripleDESCryptoServiceProvider = New TripleDESCryptoServiceProvider()
		'	' As before we must provide the encryption/decryption key along with
		'	' the init vector.
		'	Dim cryptoTransform As ICryptoTransform = tdesProvider.CreateDecryptor(Me.key, Me.iv)
		'	' Provide a memory stream to decrypt information into
		'	Dim decryptedStream As MemoryStream = New MemoryStream()
		'	Dim cryptStream As CryptoStream = New CryptoStream(decryptedStream, cryptoTransform, CryptoStreamMode.Write)

		'	cryptStream.Write(inputInBytes, 0, inputInBytes.Length)
		'	cryptStream.FlushFinalBlock()
		'	decryptedStream.Position = 0

		'	' Read the memory stream and convert it back into a string
		'	Dim result(CInt(decryptedStream.Length) - 1) As Byte
		'	decryptedStream.Read(result, 0, CInt(decryptedStream.Length))
		'	cryptStream.Close()
		'	Dim myutf As UTF8Encoding = New UTF8Encoding()

		'	Return myutf.GetString(result)
		'End Function

		'Function psDecrypt(ByVal sQueryString As String) As String

		'	Dim buffer() As Byte
		'	Dim loCryptoClass As New TripleDESCryptoServiceProvider
		'	Dim loCryptoProvider As New MD5CryptoServiceProvider

		'	Try

		'		buffer = Convert.FromBase64String(sQueryString)
		'		loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey))
		'		loCryptoClass.IV = lbtVector
		'		Return Encoding.ASCII.GetString(loCryptoClass.CreateDecryptor().TransformFinalBlock(buffer, 0, buffer.Length()))
		'	Catch ex As Exception
		'		Throw ex
		'	Finally
		'		loCryptoClass.Clear()
		'		loCryptoProvider.Clear()
		'		loCryptoClass = Nothing
		'		loCryptoProvider = Nothing
		'	End Try

		'End Function

		'Function psEncrypt(ByVal sInputVal As String) As String

		'	Dim loCryptoClass As New TripleDESCryptoServiceProvider
		'	Dim loCryptoProvider As New MD5CryptoServiceProvider
		'	Dim lbtBuffer() As Byte

		'	Try
		'		lbtBuffer = System.Text.Encoding.ASCII.GetBytes(sInputVal)
		'		loCryptoClass.Key = loCryptoProvider.ComputeHash(ASCIIEncoding.ASCII.GetBytes(lscryptoKey))
		'		loCryptoClass.IV = lbtVector
		'		sInputVal = Convert.ToBase64String(loCryptoClass.CreateEncryptor().TransformFinalBlock(lbtBuffer, 0, lbtBuffer.Length()))
		'		psEncrypt = sInputVal
		'	Catch ex As CryptographicException
		'		Throw ex
		'	Catch ex As FormatException
		'		Throw ex
		'	Catch ex As Exception
		'		Throw ex
		'	Finally
		'		loCryptoClass.Clear()
		'		loCryptoProvider.Clear()
		'		loCryptoClass = Nothing
		'		loCryptoProvider = Nothing
		'	End Try

		'End Function


#End Region


	End Class

End Namespace

