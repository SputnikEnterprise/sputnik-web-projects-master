'------------------------------------
' File: Utility.vb
' Date: 19.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.IO.Compression
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary
Imports System.Runtime.Serialization.Json
Imports System.Security.Cryptography
Imports System.Text
Imports System.Web.Script.Serialization
Imports System.Text.RegularExpressions

''' <summary>
''' Utility class.
''' </summary>
Public Class Utility

    ''' <summary>
    ''' Compresses a string with gzip algorithm.
    ''' </summary>
    ''' <param name="text">The text to compress</param>
    Public Shared Function Compress(ByVal text As String) As String
        Dim buffer() As Byte = Encoding.UTF8.GetBytes(text)
        Dim memoryStream = New MemoryStream()
        Using gZipStream = New GZipStream(memoryStream, CompressionMode.Compress, True)
            gZipStream.Write(buffer, 0, buffer.Length)
        End Using

        memoryStream.Position = 0

        Dim compressedData = New Byte(memoryStream.Length - 1) {}
        memoryStream.Read(compressedData, 0, compressedData.Length)

        Dim gZipBuffer = New Byte(compressedData.Length + 4 - 1) {}
        System.Buffer.BlockCopy(compressedData, 0, gZipBuffer, 4, compressedData.Length)
        System.Buffer.BlockCopy(BitConverter.GetBytes(buffer.Length), 0, gZipBuffer, 0, 4)
        Return Convert.ToBase64String(gZipBuffer)
    End Function


    ''' <summary>
    ''' Decompresses a gzip compressed string.
    ''' </summary>
    ''' <param name="compressedText">The compressed text.</param>
    ''' <returns>Decompressed text.</returns>
    Public Shared Function DecompressString(ByVal compressedText As String) As String
        Dim gZipBuffer() As Byte = Convert.FromBase64String(compressedText)
        Using memoryStream = New MemoryStream()
            Dim dataLength As Integer = BitConverter.ToInt32(gZipBuffer, 0)
            memoryStream.Write(gZipBuffer, 4, gZipBuffer.Length - 4)

            Dim buffer = New Byte(dataLength - 1) {}

            memoryStream.Position = 0
            Using gZipStream = New GZipStream(memoryStream, CompressionMode.Decompress)
                gZipStream.Read(buffer, 0, buffer.Length)
            End Using

            Return Encoding.UTF8.GetString(buffer)
        End Using
    End Function

    ''' <summary>
    ''' Serrializes as list of strings as a base64 encoded string.
    ''' </summary>
    ''' <param name="listToSerialize">The list of string to serialize.</param>
    ''' <returns>Base64 encoded string.</returns>
    Public Shared Function SerializeListAsBase64String(ByVal listToSerialize As List(Of String)) As String
        Dim binaryFormatter = New BinaryFormatter()
        Dim stream = New MemoryStream()
        binaryFormatter.Serialize(stream, listToSerialize)
        stream.Position = 0
        Dim byteArray = stream.ToArray()
        Dim retString = Convert.ToBase64String(byteArray)
        Return retString
    End Function

    ''' <summary>
    ''' Deserializes a base64 encoded string to a list of strings.
    ''' </summary>
    ''' <param name="serializedList">The base64 encoded list of string.</param>
    ''' <returns>List of strings.</returns>
    Public Shared Function DeSerializeGuidList(ByVal serializedList As String) As List(Of String)
        Dim byteArray = Convert.FromBase64String(serializedList)
        Dim stream = New MemoryStream(byteArray)

        Dim ret As List(Of String)
        Dim binaryFormatter = New BinaryFormatter()
        stream.Seek(0, SeekOrigin.Begin)
        ret = DirectCast(binaryFormatter.Deserialize(stream), List(Of String))
        Return ret
    End Function

    ''' <summary>
    ''' Serializes an object to a json formatted string.
    ''' </summary>
    ''' <param name="obj">The object to be serialized.</param>
    ''' <returns>Json formatted object string.</returns>
    Public Shared Function SerializeToJson(ByVal obj As Object) As String
        Dim jsonSerializer As New JavaScriptSerializer
        Dim json = jsonSerializer.Serialize(obj)
        Return json
    End Function

    ''' <summary>
    '''  Serializes an object to a json formatted string with the use of a data contract json serializer.
    '''  This way the the names of the generated json class members can be controlled exactly.
    ''' </summary>
    ''' <param name="obj">The object to be serialized.</param>
    ''' <param name="type">The type of object to be serialized.</param>
    ''' <returns>Json formatted object string.</returns>
    Public Shared Function SerializeToJson(ByVal obj As Object, ByVal type As Type) As String
        Dim jsonSerializer As New DataContractJsonSerializer(type)
        Dim ms As New MemoryStream
        jsonSerializer.WriteObject(ms, obj)
        Dim json = Encoding.UTF8.GetString(ms.ToArray())
        Return json
    End Function

    ''' <summary>
    ''' Checks if a browser agent string belongs to a mobile browser.
    ''' See http://detectmobilebrowsers.com/
    ''' </summary>
    ''' <param name="browserAgent">The browser agent string.</param>
    ''' <returns>Boolean truht value.</returns>
    Public Shared Function IsMobileAgent(ByVal browserAgent As String) As Boolean
        Dim b As New Regex("android.+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase)
        Dim v As New Regex("1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|e\-|e\/|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(di|rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|xda(\-|2|g)|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase)
        Return b.IsMatch(browserAgent) Or v.IsMatch(Left(browserAgent, 4))

    End Function

    ''' <summary>
    ''' Concatenates a list of strings.
    ''' </summary>
    ''' <param name="listOfStrings">The list of string.</param>
    ''' <param name="delimeter">The delimeter symbol.</param>
    ''' <returns>Concatenated strings.</returns>
    Public Shared Function ConcatenateList(ByVal listOfStrings As List(Of String), ByVal delimeter As String) As String

        Dim stringBuilder As New StringBuilder

        For Each s In listOfStrings
            stringBuilder.Append(s)
            stringBuilder.Append(delimeter)
        Next

        Dim chainedList As String = stringBuilder.ToString()

        ' Remove last ','
        Return chainedList.Substring(0, chainedList.Length - 1)
    End Function
End Class
