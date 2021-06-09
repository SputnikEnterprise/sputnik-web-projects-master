'------------------------------------
' File: UtilityTests.vb
' Date: 20.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports CockpitPublisher.Common

<TestClass()>
Public Class UtilityTests

    <TestMethod()>
    Public Sub Compress_SimpleTextIsCompressed_DecompressedTextIsSameAsOrginalText()

        ' ----------Arrange----------

        Dim originalText As String = "This is a simple text."

        ' ----------Act----------

        Dim compressedText = Utility.Compress(originalText)
        Dim decompressedText = Utility.DecompressString(compressedText)

        ' ----------Assert----------

        Assert.IsTrue(decompressedText = originalText)

    End Sub

    <TestMethod()>
    Public Sub SerializeListAsBase64String_GuidWith1ElementIsSerialized_SerializeAndDeserializeWorkTogether()

        ' ----------Arrange----------

        Dim guid = "C942EF9B-A455-49BE-B7FB-5507FCD2F1C0"
        Dim guidList As List(Of String) = New List(Of String)()
        guidList.Add(guid)

        ' ----------Act----------

        Dim serializedList = Utility.SerializeListAsBase64String(guidList)
        Dim deserializedList = Utility.DeSerializeGuidList(serializedList)

        ' ----------Assert----------

        Assert.IsTrue(guidList(0).Equals(deserializedList(0)))

    End Sub


End Class
