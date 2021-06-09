

<Serializable()>
Public Class UpdateUtilitiesDTO

	Public Property UpdateID As Integer?

	Public Property UpdateFileDate As DateTime?
	Public Property UpdateFileTime As DateTime?
	Public Property UpdateFilename As String

End Class


<Serializable()>
Public Class FTPUpdateFilesDTO

	Public Property UpdateID As Integer?
	Public Property UpdateFilename As String
	Public Property FileDestPath As String
	Public Property FileDestVersion As String
	Public Property UpdateFileDate As DateTime?
	Public Property UpdateFileTime As String
	Public Property UpdateFileSize As Long
	Public Property File_Guid As String

	Public Property FileContent As Byte()


End Class
