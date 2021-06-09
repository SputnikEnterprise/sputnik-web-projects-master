'------------------------------------
' File: UtilityTest.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

Imports Moq
Imports System.Text
Imports System.Web

Namespace Utility

    <TestClass()>
    Public Class UtilityTest

        <ClassInitialize()>
        Public Shared Sub Init(ByVal testContext As TestContext)
            Dim logger = New Mock(Of ILoggingService)()
            JobFinderApp.Domain.Utility.Utility.Logger = logger.Object
        End Sub

        <TestMethod()>
        Public Sub SerializeParameterObject_SerializeWithoutEncryption_ObjectIsSerializedToBase64String()
            ' Arrange
            Dim objectToSerialize = UtilityTest.GetRandomList()

            ' Act
            Dim serializedObject As String = JobFinderApp.Domain.Utility.Utility.SerializeParameterObject(objectToSerialize)

            ' Assert
            Dim isBase64 As Boolean = True
            Try
                Dim byteArray() As Byte = Convert.FromBase64String(serializedObject)
            Catch ex As FormatException
                ' Exception occurs if serializedObjectString does not contain base64-characters.
                isBase64 = False
            End Try
            Assert.IsTrue(isBase64, "Serialized object is not a base64 string.")

        End Sub

        <TestMethod()>
        Public Sub SerializeParameterObject_SerializeWithEncryption_ObjectIsSerializedToBase64String()
            ' Arrange
            Dim objectToSerialize = UtilityTest.GetRandomList()

            ' Act
            Dim serializedObject As String = JobFinderApp.Domain.Utility.Utility.SerializeParameterObject(objectToSerialize, True)

            ' Assert
            Dim isBase64 As Boolean = True
            Try
                Dim byteArray() As Byte = Convert.FromBase64String(serializedObject)
            Catch ex As FormatException
                ' Exception occurs if serializedObjectString does not contain base64-characters.
                isBase64 = False
            End Try
            Assert.IsTrue(isBase64, "Serialized object is not a base64 string.")
        End Sub

        <TestMethod()>
        Public Sub DeSerializeParameterObject_DeSerializeWithoutEncryption_ObjectIsSerializedToBase64String()
            ' Arrange
            Dim objectToSerialize = UtilityTest.GetRandomList()

            ' Generate serialized string
            Dim serializedObject As String = JobFinderApp.Domain.Utility.Utility.SerializeParameterObject(objectToSerialize)

            ' Act
            Dim deSerializedObject = JobFinderApp.Domain.Utility.Utility.DeSerializeParameterObject(serializedObject)

            ' Assert

            ' Check the tye is right
            Assert.IsTrue(TypeOf deSerializedObject Is List(Of String), "The type of the deserialized list is not the same as the original list.")

            ' Check the content is right
            Assert.IsTrue(UtilityTest.CompareCollections(objectToSerialize, deSerializedObject), "The content of the deserialized list is not the same as the original list.")

        End Sub

        <TestMethod()>
        Public Sub DeSerializeParameterObject_DeSerializeWithEncryption_ObjectIsSerializedToBase64String()
            ' Arrange
            Dim objectToSerialize = UtilityTest.GetRandomList()

            ' Generate serialized string
            Dim serializedObject As String = JobFinderApp.Domain.Utility.Utility.SerializeParameterObject(objectToSerialize, True)

            ' Act
            Dim deSerializedObject = JobFinderApp.Domain.Utility.Utility.DeSerializeParameterObject(serializedObject, True)

            ' Assert
            ' Check the tye is right
            Assert.IsTrue(TypeOf deSerializedObject Is List(Of String), "The type of the deserialized list is not the same as the original list.")

            ' Check the content is right
            Assert.IsTrue(UtilityTest.CompareCollections(objectToSerialize, deSerializedObject), "The content of the deserialized list is not the same as the original list.")
        End Sub

        <TestMethod()>
        Public Sub EncryptString_UseCustomKey_StringIsEncrypted()
            ' Arrange
            Dim stringToEncrypt As String = UtilityTest.GetRandomString(30)

            ' Act
            Dim encryptedString As String = JobFinderApp.Domain.Utility.Utility.EncryptString(stringToEncrypt, "MyOwnKey")

            ' Assert
            Assert.IsFalse(stringToEncrypt.Equals(encryptedString), "The enrcypted string equals to the original string.")
        End Sub

        <TestMethod()>
        Public Sub EncryptString_UseDefaultKey_StringIsEncrypted()
            ' Arrange
            Dim stringToEncrypt As String = UtilityTest.GetRandomString(30)

            ' Act
            Dim encryptedString As String = JobFinderApp.Domain.Utility.Utility.EncryptString(stringToEncrypt)

            ' Assert
            Assert.IsFalse(stringToEncrypt.Equals(encryptedString), "The enrcypted string equals to the original string.")
        End Sub

        <TestMethod()>
        Public Sub DecryptString_UseCustomKey_StringIsDecrypted()
            ' Arrange
            Dim stringToEncrypt As String = UtilityTest.GetRandomString(30)
            Dim encryptedString As String = JobFinderApp.Domain.Utility.Utility.EncryptString(stringToEncrypt, "MyOwnKey")

            ' Act
            Dim decryptedString As String = JobFinderApp.Domain.Utility.Utility.DecryptString(encryptedString, "MyOwnKey")

            ' Assert
            Assert.IsTrue(stringToEncrypt.Equals(decryptedString), "The decrypted string is not equal to the original string.")
        End Sub

        <TestMethod()>
        Public Sub DecryptString_UseDefaultKey_StringIsDecrypted()
            ' Arrange
            Dim stringToEncrypt As String = UtilityTest.GetRandomString(30)
            Dim encryptedString As String = JobFinderApp.Domain.Utility.Utility.EncryptString(stringToEncrypt)

            ' Act
            Dim decryptedString As String = JobFinderApp.Domain.Utility.Utility.DecryptString(encryptedString)

            ' Assert
            Assert.IsTrue(stringToEncrypt.Equals(decryptedString), "The decrypted string is not equal to the original string.")
        End Sub

        <TestMethod()>
        Public Sub DecryptString_TryToHackTheKey_StringIsNotDecrypted()
            ' Arrange
            Dim stringToEncrypt As String = UtilityTest.GetRandomString(30)
            Dim encryptedString As String = JobFinderApp.Domain.Utility.Utility.EncryptString(stringToEncrypt, "MyOwnKey")

            ' Act
            Dim decryptedString As String = JobFinderApp.Domain.Utility.Utility.DecryptString(encryptedString, "HackerKey")

            ' Assert
            Assert.IsFalse(stringToEncrypt.Equals(decryptedString), "The hacker decrypted string is equal to the original string.")
        End Sub

        <TestMethod()>
        Public Sub ReadDictionaryValue_FirstKeyIsContained_ReturnsFirstValue()
            ' Arrange
            Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
            dictionary.Add("key1", "value1")
            dictionary.Add("key2", "value2")
            dictionary.Add("key3", "value3")

            ' Act
            Dim value As String = JobFinderApp.Domain.Utility.Utility.ReadDictionaryValue(dictionary, "key1", "key2", "fallbackValue")

            ' Assert
            Assert.IsTrue(value.Equals("value1"), "The value for the first key should have been returned.")
        End Sub

        <TestMethod()>
        Public Sub ReadDictionaryValue_FirstKeyIsNotContained_ReturnsSecondValue()
            ' Arrange
            Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
            dictionary.Add("key0", "value0")
            dictionary.Add("key2", "value2")
            dictionary.Add("key3", "value3")

            ' Act
            Dim value As String = JobFinderApp.Domain.Utility.Utility.ReadDictionaryValue(dictionary, "key1", "key2", "fallbackValue")

            ' Assert
            Assert.IsTrue(value.Equals("value2"), "The value for the second key should have been returned.")
        End Sub

        <TestMethod()>
        Public Sub ReadDictionaryValue_FirstAndSecondKeyAreNotContained_ReturnsFallbackValue()
            ' Arrange
            Dim dictionary As Dictionary(Of String, String) = New Dictionary(Of String, String)()
            dictionary.Add("key3", "value3")
            dictionary.Add("key4", "value4")
            dictionary.Add("key5", "value5")

            ' Act
            Dim value As String = JobFinderApp.Domain.Utility.Utility.ReadDictionaryValue(dictionary, "key1", "key2", "fallbackValue")

            ' Assert
            Assert.IsTrue(value.Equals("fallbackValue"), "The fallback value should have been returned.")
        End Sub

        <TestMethod()>
        Public Sub ThrowTimestampedException_CallThisMethod_ThrowsAnException()
            ' Arrange
            Dim exceptionThrowed As Boolean = False

            ' Act
            Try
                JobFinderApp.Domain.Utility.Utility.ThrowTimestampedException("MyMessage")
            Catch ex As Exception
                exceptionThrowed = True
            End Try

            ' Assert
            Assert.IsTrue(exceptionThrowed, "Expected exception was not thrown.")
        End Sub

        <TestMethod()>
        Public Sub GetBaseUrl_ContextIsValid_GetRightBaseUrl()
            ' Arrange
            ' Inject HttpContext Stub
            Dim mockContext = New Mock(Of HttpContextBase)
            Dim mockRequest = New Mock(Of HttpRequestBase)

            Dim url As Uri = New Uri("http://WebServerAddress:1234/applicationName/furtherParameter?querystringParameterName=value")
            Dim applicationPath As String = "/goodJobs"

            mockContext.Setup(Function(context As HttpContextBase) context.Request).Returns(mockRequest.Object)
            mockRequest.Setup(Function(request As HttpRequestBase) request.Url).Returns(url)
            mockRequest.Setup(Function(request As HttpRequestBase) request.ApplicationPath).Returns(applicationPath)

            JobFinderApp.Domain.Utility.Utility.SetContext(Function() mockContext.Object)

            ' Act
            Dim baseUrl = JobFinderApp.Domain.Utility.Utility.GetBaseUrl()

            ' Assert
            Assert.IsTrue(baseUrl.Equals("http://webserveraddress:1234/goodJobs"), "The baseurl is not right.")
        End Sub

#Region "Private Methods"

        ''' <summary>
        ''' Creates a random filled list.
        ''' </summary>
        Private Shared Function GetRandomList() As List(Of String)
            Dim result = New List(Of String)()

            ' Generate 100 random strings and insert them in the list object that will be serialized.
            For Each number In Enumerable.Range(0, 100)
                Dim valueToInsert As String = UtilityTest.GetRandomString(20)
                result.Add(valueToInsert)
            Next

            Return result
        End Function

        ''' <summary>
        ''' Returns a random String with a desired lenght.
        ''' </summary>
        ''' <param name="length"></param>
        ''' <returns>A random String</returns>
        Private Shared Function GetRandomString(ByVal length As Integer) As String
            Dim chars() As Char = "ABCDEFGHIJKLMNOPQRSTUVWXYZäöüàéè?!,.-+""*ç%&/()=0123456789"

            Dim result As String = New String(Enumerable.Repeat(chars, length) _
                                              .Select(Function(s) s(random.Next(s.Length))) _
                                              .ToArray())
            Return result
        End Function

        ''' <summary>
        ''' Compares two collections and determines whether they are identical.
        ''' </summary>
        ''' <param name="coll1"></param>
        ''' <param name="coll2"></param>
        ''' <returns>True if the two collections are identical, false otherwise.</returns>
        Private Shared Function CompareCollections(ByVal coll1 As ICollection(Of String), coll2 As ICollection(Of String)) As Boolean
            If coll1.Count <> coll2.Count Then
                Return False
            End If

            For Each element In coll1
                If Not coll2.Contains(element) Then
                    Return False
                End If
            Next

            Return True
        End Function

#End Region

#Region "Private Fields"

        Private Shared random As Random = New Random()

#End Region

    End Class
End Namespace
