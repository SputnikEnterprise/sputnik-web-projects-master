'------------------------------------
' File: TableUploaderTests.vb
' Date: 20.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Text
Imports Moq
Imports System.Linq
Imports CockpitPublisher.Common
Imports UploadClient.CockpitWCFServices

<TestClass()>
Public Class TableUploaderTests

    <TestMethod()>
    Public Sub UploadTables_DatabaseHas4TablesToUpload_UploadServiceIsCalled4Times()

        ' ----------Arrange----------

        Dim tableNames As New List(Of String)

        tableNames.Add("tblMyOpenRP__T")
        tableNames.Add("tbl_RecCount__T")
        tableNames.Add("tblMyKDKredit__T")
        tableNames.Add("tblMyKontakte__T")

        Dim xmlTableInfo As New XMLTableInfo()
        xmlTableInfo.TableSchema = "<xml></xml>"
        xmlTableInfo.TableData = "<xml></xml>"

        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        uploadServiceMock.Setup(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo))).Returns(True)

        ' Mock for database access
        Dim dbAccessMock As New Mock(Of IDatabaseAccess)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableNames).Returns(tableNames)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableInfo(It.IsAny(Of String))).Returns(xmlTableInfo)

        ' Mock for logger
        Dim dbLoggerMock As New Mock(Of ILogger)


        ' Configure table uploader with mock objects
        Dim tableUploader As New TableUploader(uploadServiceMock.Object, dbAccessMock.Object, dbLoggerMock.Object)

        ' ----------Act----------

        tableUploader.UploadTables()

        ' ----------Assert----------

        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo)), Times.Exactly(4))
    End Sub

    <TestMethod()>
    Public Sub UploadTables_DatabaseHas4TablesToUploadButAlsoSomeOthers_UploadServiceIsCalled4TimesWithCorrectTableNames()

        ' ----------Arrange----------

        Dim tableNames As New List(Of String)

        tableNames.Add("tblMyOpenRP__5B42C92A-9F23-450F-B95E-66DBB4904E4E")
        tableNames.Add("tblMyOpenRP__T")
        tableNames.Add("tbl_RecCount__2051EF1C-B946-42DA-9201-859DA2D4C104")
        tableNames.Add("tbl_RecCount__T")
        tableNames.Add("tblMyKDKredit__349EC3E8-E2A5-4F0E-95D2-3FEA5DF47761")
        tableNames.Add("tblMyKDKredit__T")
        tableNames.Add("tblMyKontakte__3B57FE82-F59F-4987-ACB6-B2EE4261BD7D")
        tableNames.Add("tblMyKontakte__T")
        tableNames.Add("tblMySetting")
        tableNames.Add("testTable__T_3B57FE82-F59F-4987-ACB6-B2EE4261BD7D")
        tableNames.Add("__T_x")

        Dim validTableNames As New List(Of String)
        validTableNames.Add("tblMyOpenRP__T")
        validTableNames.Add("tbl_RecCount__T")
        validTableNames.Add("tblMyKDKredit__T")
        validTableNames.Add("tblMyKontakte__T")

        Dim xmlTableInfo As New XMLTableInfo()
        xmlTableInfo.TableSchema = "<xml></xml>"
        xmlTableInfo.TableData = "<xml></xml>"

        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        uploadServiceMock.Setup(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo))).Returns(True)

        ' Mock for database access
        Dim dbAccessMock As New Mock(Of IDatabaseAccess)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableNames).Returns(tableNames)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableInfo(It.IsAny(Of String))).Returns(xmlTableInfo)

        ' Mock for logger
        Dim dbLoggerMock As New Mock(Of ILogger)

        ' Configure table uploader with mock objects
        Dim tableUploader As New TableUploader(uploadServiceMock.Object, dbAccessMock.Object, dbLoggerMock.Object)

        ' ----------Act----------

        tableUploader.UploadTables()

        ' ----------Assert----------

        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo)), Times.Exactly(4))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) validTableNames.Contains(o2.TableName))), Times.Exactly(4))
    End Sub

    <TestMethod()>
    Public Sub UploadTables_DatabaseHasOneTablesToUpload_UploadServiceIsCalledWithCorrectParameters()

        ' ----------Arrange----------

        Dim tableNames As New List(Of String)

        tableNames.Add("tblMyOpenRP__T")

        Dim xmlData As String = "<xml><value1>1</value1></xml>"

        Dim xmlTableInfo As New XMLTableInfo()
        xmlTableInfo.TableSchema = "<xml></xml>"
        xmlTableInfo.TableData = xmlData
        xmlTableInfo.AddMDGuid("C942EF9B-A455-49BE-B7FB-5507FCD2F1C0")
        xmlTableInfo.AddMDGuid("AB42EF9B-A455-49BE-B7FB-5507FCD2F1C0")

        Dim validMdGuids As New List(Of String)
        validMdGuids.Add("C942EF9B-A455-49BE-B7FB-5507FCD2F1C0")
        validMdGuids.Add("AB42EF9B-A455-49BE-B7FB-5507FCD2F1C0")

        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        uploadServiceMock.Setup(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo))).Returns(True)

        ' Mock for database access
        Dim dbAccessMock As New Mock(Of IDatabaseAccess)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableNames).Returns(tableNames)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableInfo(It.IsAny(Of String))).Returns(xmlTableInfo)

        ' Mock for logger
        Dim dbLoggerMock As New Mock(Of ILogger)

        ' Configure table uploader with mock objects
        Dim tableUploader As New TableUploader(uploadServiceMock.Object, dbAccessMock.Object, dbLoggerMock.Object)

        ' ----------Act----------

        tableUploader.UploadTables()

        ' ----------Assert----------

        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) o2.TableName = "tblMyOpenRP__T")), Times.Once)
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) o2.CustomerName IsNot Nothing)))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) validMdGuids.Count = 2)))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) validMdGuids.Contains(o2.MDGuids(0)))))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) validMdGuids.Contains(o2.MDGuids(1)))))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) o2.CompressedTableSchema = Utility.Compress(xmlTableInfo.TableSchema))))
        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.Is(Of TableInfo)(Function(o2 As TableInfo) o2.CompressedTableData = Utility.Compress(xmlTableInfo.TableData))))
    End Sub

    <TestMethod()>
    Public Sub UploadTables_ReadTableNamesFails_ErrorIsReportedWithWebservice()

        ' ----------Arrange----------

        Dim xmlData As String = "<xml></xml>"

        Dim xmlTableInfo As New XMLTableInfo()
        xmlTableInfo.TableSchema = "<xml></xml>"
        xmlTableInfo.TableData = "<xml></xml>"

        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        'Web service call fails
        uploadServiceMock.Setup(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo))).Returns(True)

        ' Mock for database access
        Dim dbAccessMock As New Mock(Of IDatabaseAccess)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableNames()).Returns(CType(Nothing, List(Of String)))
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableInfo(It.IsAny(Of String))).Returns(xmlTableInfo)

        Dim logger As ILogger = New DefaultLogger(uploadServiceMock.Object)

        ' Configure table uploader with mock objects
        Dim tableUploader As New TableUploader(uploadServiceMock.Object, dbAccessMock.Object, logger)

        ' ----------Act----------

        tableUploader.UploadTables()

        ' ----------Assert----------

        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo)), Times.Never)
        uploadServiceMock.Verify(Sub(o As ITableDataUploadService) o.LogError(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Once)

    End Sub

    <TestMethod()>
    Public Sub UploadTables_ReadTableDataFails_ErrorIsReportedWithWebservice()

        ' ----------Arrange----------

        Dim tableNames As New List(Of String)

        tableNames.Add("tblMyOpenRP__T")


        ' Mock for upload service
        Dim uploadServiceMock As New Mock(Of ITableDataUploadService)
        'Web service call fails
        uploadServiceMock.Setup(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo))).Returns(True)

        ' Mock for database access
        Dim dbAccessMock As New Mock(Of IDatabaseAccess)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableNames()).Returns(tableNames)
        dbAccessMock.Setup(Function(o As IDatabaseAccess) o.ReadTableInfo(It.IsAny(Of String))).Returns(CType(Nothing, XMLTableInfo))

        Dim logger As ILogger = New DefaultLogger(uploadServiceMock.Object)

        ' Configure table uploader with mock objects
        Dim tableUploader As New TableUploader(uploadServiceMock.Object, dbAccessMock.Object, logger)

        ' ----------Act----------

        tableUploader.UploadTables()

        ' ----------Assert----------

        uploadServiceMock.Verify(Function(o As ITableDataUploadService) o.ProcessTableData(It.IsAny(Of TableInfo)), Times.Never)
        uploadServiceMock.Verify(Sub(o As ITableDataUploadService) o.LogError(It.IsAny(Of String), It.IsAny(Of String), It.IsAny(Of String)), Times.Once)

    End Sub

End Class
