'------------------------------------
' File: DefaultLoggerTests.vb
' Date: 20.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports System.Linq
Imports UploadClient.CockpitWCFServices

<TestClass()>
Public Class DefaultLoggerTests

    <TestMethod()>
    Public Sub LogErrorRemote_ErrorRemoteIsCalled_LogErrorOfWebservicesIsCalledWithCorrectParameters()

        ' ----------Arrange----------

        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        uploadServiceMock.Setup(Sub(o As ITableDataUploadService) o.LogError(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)))

        ' Configure table uploader with mock objects
        Dim logger As ILogger = New DefaultLogger(uploadServiceMock.Object)

        ' ----------Act----------

        logger.LogErrorRemote("A critial error happend", "exceptionText")

        ' ----------Assert----------

        uploadServiceMock.Verify(Sub(o As ITableDataUploadService) o.LogError("A critial error happend", "exceptionText", It.IsAny(Of String)), Times.Exactly(1))

    End Sub

End Class
