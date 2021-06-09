'------------------------------------
' File: Utility.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web
Imports System.Globalization
Imports JobFinderApp.Contracts

Namespace Utility

    ''' <summary>
    ''' Utility class.
    ''' </summary>
    ''' <remarks>Important: To use this class, the two properties Context and Logger must be injected.</remarks>
    ''' <see cref="Utility.Context" />
    ''' <see cref="Utility.Logger" />
    Public Class Utility

        ''' <summary>
        ''' Property for HttpContext that can be used to inject Mock for Unit Testing.
        ''' </summary>
        Public Shared ReadOnly Property Context As HttpContextBase
            Get
                Return Utility.ContextGetter()
            End Get
        End Property

        Public Shared Sub SetContext(contextGetter As Func(Of HttpContextBase))
            Utility.ContextGetter = contextGetter
        End Sub

        ' By default: return the real Http-Context
        Private Shared ContextGetter As Func(Of HttpContextBase) = Function() New HttpContextWrapper(HttpContext.Current)

        ''' <summary>
        ''' Property for logging.
        ''' </summary>
        Public Shared Property Logger As ILoggingService

        ''' <summary>
        ''' Serializes as list of strings as a url encoded string.
        ''' </summary>
        ''' <param name="obj">The object to serialize.</param>
        ''' <returns>The serialized object.</returns>
        Public Shared Function SerializeParameterObject(ByVal obj As Object, Optional ByVal enrcrypt As Boolean = False) As String
            Try
                Dim binaryFormatter = New BinaryFormatter()
                Dim stream = New MemoryStream()
                binaryFormatter.Serialize(stream, obj)
                stream.Position = 0
                Dim byteArray = stream.ToArray()

                Dim base64String = Convert.ToBase64String(byteArray)
                If enrcrypt Then
                    base64String = Utility.EncryptString(base64String)
                End If
                Return base64String
            Catch ex As Exception
                Utility.Logger.Log("The serialisation did not work. Exception message: " & ex.Message, LogLevel.Debug_Level)
                Return String.Empty
            End Try

        End Function

        ''' <summary>
        ''' Deserializes a URL encoded string to an object.
        ''' </summary>
        ''' <param name="serializedObject">The URL encoded object of string.</param>
        ''' <returns>List of strings.</returns>
        Public Shared Function DeSerializeParameterObject(ByVal serializedObject As String, Optional ByVal dercrypt As Boolean = False) As Object
            Try
                Dim base64String = serializedObject
                If dercrypt Then
                    base64String = Utility.DecryptString(base64String)
                End If
                Dim byteArray = Convert.FromBase64String(base64String)
                Dim stream = New MemoryStream(byteArray)

                Dim ret As Object
                Dim binaryFormatter = New BinaryFormatter()
                stream.Seek(0, SeekOrigin.Begin)
                ret = DirectCast(binaryFormatter.Deserialize(stream), Object)
                Return ret
            Catch ex As Exception
                Utility.Logger.Log("The deserialization did not work. Exception message: " & ex.Message, LogLevel.Debug_Level)
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Encrypts a string.
        ''' </summary>
        ''' <param name="strtext">The string to encrypt.</param>
        ''' <param name="key">The key for encryption.</param>
        ''' <returns>Encrypted text as Base64 String.</returns>
        Public Shared Function EncryptString(ByVal strtext As String, Optional ByVal key As String = Constants.DEFAULT_KRYPTOGRAPHIC_SECRET_KEY) As String
            Try
                Dim byKey() As Byte = {}
                Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(key, 8))
                Dim cryptoProvider As DESCryptoServiceProvider = New DESCryptoServiceProvider()
                Dim memoryStream As MemoryStream = New MemoryStream()
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, cryptoProvider.CreateEncryptor(byKey, IV), CryptoStreamMode.Write)
                Dim writer As StreamWriter = New StreamWriter(cryptoStream)
                writer.Write(strtext)
                writer.Flush()
                cryptoStream.FlushFinalBlock()
                writer.Flush()
                Return Convert.ToBase64String(memoryStream.GetBuffer(), 0, CType(memoryStream.Length, Integer))
            Catch ex As Exception
                Utility.Logger.Log("The encryption did not work. Exception message: " & ex.Message, LogLevel.Debug_Level)
                Return String.Empty
            End Try
        End Function

        ''' <summary>
        ''' Decrypts a string.
        ''' </summary>
        ''' <param name="strtext">The base 64 string to decrypt.</param>
        ''' <param name="key">The key for decryption.</param>
        ''' <returns>Decrypted text or the empty string, if it could not decrypted.</returns>
        Public Shared Function DecryptString(ByVal strtext As String, Optional ByVal key As String = Constants.DEFAULT_KRYPTOGRAPHIC_SECRET_KEY) As String
            Try
                Dim byKey() As Byte = {}
                Dim IV() As Byte = {&H12, &H34, &H56, &H78, &H90, &HAB, &HCD, &HEF}
                byKey = System.Text.Encoding.UTF8.GetBytes(Left(key, 8))
                Dim cryptoProvider As DESCryptoServiceProvider = New DESCryptoServiceProvider()
                Dim memoryStream As MemoryStream = New MemoryStream(Convert.FromBase64String(strtext))
                Dim cryptoStream As CryptoStream = New CryptoStream(memoryStream, cryptoProvider.CreateDecryptor(byKey, IV), CryptoStreamMode.Read)
                Dim reader As StreamReader = New StreamReader(cryptoStream)
                Return reader.ReadToEnd()
            Catch ex As Exception
                Utility.Logger.Log("The decryption did not work. Exception message: " & ex.Message, LogLevel.Debug_Level)
                Return String.Empty
            End Try

        End Function

        ''' <summary>
        ''' Reads a value from a dictionary value.
        ''' Tries to read the value by two different keys.
        ''' </summary>
        ''' <param name="dictionary">The dictionary.</param>
        ''' <param name="keyForFirstTry">The key for the frist try.</param>
        ''' <param name="keyForSecondTry">The key for the second try.</param>
        ''' <param name="fallbackReturnValue">Fallback value that is returned if both keys did not lead to values.</param>
        ''' <returns>The value or the fallback value.</returns>
        Public Shared Function ReadDictionaryValue(ByVal dictionary As IDictionary, ByVal keyForFirstTry As String, ByVal keyForSecondTry As String, ByVal fallbackReturnValue As String) As String
            If dictionary.Contains(keyForFirstTry) And Not String.IsNullOrEmpty(dictionary(keyForFirstTry)) Then
                Return dictionary(keyForFirstTry)
            ElseIf dictionary.Contains(keyForSecondTry) And Not String.IsNullOrEmpty(dictionary(keyForSecondTry)) Then
                Return dictionary(keyForSecondTry)
            Else
                Return fallbackReturnValue
            End If

        End Function

        ''' <summary>
        ''' Retreives the base url of a web application.
        ''' </summary>
        ''' <returns>The base url</returns>
        Public Shared Function GetBaseUrl() As String
            Dim baseUrl As String = Utility.Context.Request.Url.Scheme & "://" & _
                                    Utility.Context.Request.Url.Authority &
                                    Utility.Context.Request.ApplicationPath.TrimEnd("/")
            Return baseUrl
        End Function

        ''' <summary>
        ''' Retreives the base url of a web application.
        ''' </summary>
        ''' <returns>The base url</returns>
        Public Shared Function GetApplicationPath() As String
            Return Utility.Context.Request.ApplicationPath
        End Function

        ''' <summary>
        ''' Throws an Exception with timestamp and message.
        ''' </summary>
        Public Shared Sub ThrowTimestampedException(ByVal message As String)
            Dim timestamp As String = Date.Now.ToString("F", CultureInfo.CreateSpecificCulture("de-ch"))
            Throw New Exception("Timestamp: " & timestamp & ", Message: " & message)
        End Sub
    End Class

End Namespace