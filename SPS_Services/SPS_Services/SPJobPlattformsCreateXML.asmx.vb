Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_Services.JobPlatform.JobsCH

' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
' <System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://asmx.sputnik-it.com/wsSPS_services/SPJobPlattformsCreateXML.asmx/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class SPJobPlattformsCreateXML
  Inherits System.Web.Services.WebService


#Region "Methodes for jobs.ch"

  ''' <summary>
  ''' Erstellt eine neue XML-Datei in entsprechende Verzeichnis von Jobs.ch
  ''' </summary>
  ''' <param name="customerGuid"></param>
  ''' <param name="organisationID"></param>
  ''' <param name="organisationSubID"></param>
  ''' <returns></returns>
  ''' <remarks></remarks>
  <WebMethod(Description:="Erstellt die XML-Datei Für Jobs.ch.")> _
  Function CreateNewXMLForJobsCH(ByVal customerGuid As String,
                                 ByVal organisationID As Integer?,
                                 ByVal organisationSubID As Integer?) As Boolean

		If (organisationSubID.HasValue) Then
			Return False
		End If

		If Not customerGuid.Length < 20 Then
      Return False
    End If

    Dim conn As SqlConnection = Nothing
    Try
			Dim jobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_JobCh")
			Dim organsiationRootPath = System.IO.Path.Combine(jobChRoot, organisationSubID.ToString)
			Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

      If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)

      Dim vacanciesGenerator As New JobPlatform.JobsCH.JobChVacanciesXMLGenerator(customerGuid, organisationSubID)
      Dim xDoc = vacanciesGenerator.GenerateVacanciesXml()

      Try
        SaveXmlDocumentForJobsCH(xDoc, organisationSubID)

      Catch ex As Exception
        If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)
        Throw New Exception(String.Format("(CustomerGuid={0}, OrganisationId={1}, OrganisationsubId={2}). SaveXmlDocument={3}",
                                          customerGuid, organisationID, organisationSubID, ex.ToString))

      End Try

    Catch ex As Exception
      Throw New Exception(String.Format("(CustomerGuid={0}, OrganisationId={1}). jobChRoot={2}", customerGuid, organisationID, ex.ToString))

    End Try

    Return True
  End Function

  ''' <summary>
  ''' Saves the jobCh xml document on the file system.
  ''' </summary>
  ''' <param name="xDoc">The xml document.</param>
  Protected Sub SaveXmlDocumentForJobsCH(ByVal xDoc As XDocument, ByVal organisationId As Integer)

    Dim jobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_JobCh")
    Dim organsiationRootPath = System.IO.Path.Combine(jobChRoot, organisationId.ToString)
    Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

    If Not Directory.Exists(organsiationRootPath) Then
      Directory.CreateDirectory(organsiationRootPath)
    End If

    Dim xmlCode As String = String.Empty

    ' Use a special string writer here in order to get the iso-8859-1 encodinginto the the xml.
    Using sw As StringWriterWithEncoding = New StringWriterWithEncoding(Encoding.GetEncoding(xDoc.Declaration.Encoding))
      xDoc.Save(sw)
      xmlCode = sw.ToString()
    End Using

    Using sw As New System.IO.StreamWriter(System.IO.File.Open(xmlFileName, FileMode.Create), Encoding.GetEncoding("ISO-8859-1"))

      Try
        sw.WriteLine(xmlCode)
      Catch ex As Exception
        ' Maybe the file is currently in use -> wait a little bit.
        Threading.Thread.Sleep(500)
        sw.WriteLine(xmlCode)
      End Try

    End Using

  End Sub


#End Region


#Region "Methods for Ostjob.ch"

  <WebMethod(Description:="Erstellt die XML-Datei Für Jobs.ch.")> _
  Function CreateNewXMLForOstJobCH(ByVal customerGuid As String) As Boolean

    If Not customerGuid.Length < 20 Then
      Return False
    End If

    Dim conn As SqlConnection = Nothing
    Try
			Dim ostJobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_OstJobCh")
			Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, customerGuid)
			Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

      If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)

      Dim vacanciesGenerator As New JobPlatform.OstJobsCH.OstJobChVacanciesXMLGenerator(customerGuid)
      Dim xDoc = vacanciesGenerator.GenerateVacanciesXml()

      Try
        SaveXmlDocumentForOstJobCH(xDoc, customerGuid)

      Catch ex As Exception
        If IO.File.Exists(xmlFileName) Then IO.File.Delete(xmlFileName)
        Throw New Exception(String.Format("(CustomerGuid={0}). SaveXmlDocument={1}", customerGuid, ex.ToString))

      End Try

    Catch ex As Exception
      Throw New Exception(String.Format("(CustomerGuid={0},). OstJobChRoot={1}", customerGuid, ex.ToString))

    End Try

    Return True
  End Function

  ''' <summary>
  ''' Saves the ostjob.ch xml document on the file system.
  ''' </summary>
  ''' <param name="xDoc">The xml document.</param>
  Protected Sub SaveXmlDocumentForOstJobCH(ByVal xDoc As XDocument, ByVal customerGuid As String)

		Dim ostJobChRoot = System.Configuration.ConfigurationManager.AppSettings("XmlRoot_OstJobCh")
		Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, customerGuid)
		Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "vacancies.xml")

    If Not Directory.Exists(organsiationRootPath) Then
      Directory.CreateDirectory(organsiationRootPath)
    End If

    Dim xmlCode As String = String.Empty

    Using sw As StringWriter = New StringWriter()
      xDoc.Save(sw)
      xmlCode = sw.ToString()
    End Using


    Try
      xDoc.Save(xmlFileName)
    Catch ex As Exception
      ' Maybe the file is currently in use -> wait a little bit.
      Threading.Thread.Sleep(500)
      xDoc.Save(xmlFileName)
    End Try

  End Sub


#End Region


#Region "Helpers"


  ''' <summary>
  ''' Gets a value from a data row.
  ''' </summary>
  ''' <param name="dataRow">The data row.</param>
  ''' <param name="column">The column.</param>
  ''' <returns>The value or nothing.</returns>
  Private Function GetValueFromdataRow(ByVal dataRow As DataRow, ByVal column As String) As Object

    If Not dataRow.IsNull(column) Then
      Dim value As Object = dataRow(column)
      Return value
    End If

    Return Nothing
  End Function

  ''' <summary>
  ''' Parsets a nullable integer.
  ''' </summary>
  ''' <param name="str">The string.</param>
  ''' <returns>Nullable integer value or nothing.</returns>
  Private Function ParseNullableInt(ByVal str As String) As Integer?

    If String.IsNullOrEmpty(str) Then
      Return Nothing
    End If

    Return Integer.Parse(str)

  End Function

  ''' <summary>
  ''' Replaces a missing object with another object.
  ''' </summary>
  ''' <param name="obj">The object.</param>
  ''' <param name="replacementObject">The replacement object.</param>
  ''' <returns>The object or the replacement object it the object is nothing.</returns>
  Protected Shared Function ReplaceMissing(ByVal obj As Object, ByVal replacementObject As Object) As Object
    If (obj Is Nothing) Then
      Return replacementObject
    Else
      Return obj
    End If
  End Function

	'''' <summary>
	'''' Saves an error to the database.
	'''' </summary>
	'''' <param name="strErrorMessage">The error message.</param>
	'Function SaveErrToDb(ByVal strErrorMessage As String) As String
	'  Dim strResult As String = "Erfolgreich..."
	'Dim connString As String = My.Settings.ConnStr_New_spContract
	'Dim strSQL As String = String.Empty
	'  strSQL = "Insert Into SP_ModulUsage (ModulName, ModulVersion, UserID, Answer, RequestParam, "
	'  strSQL &= "CreatedOn) Values ("
	'  strSQL &= "@ModulName, @ModulVersion, @UserID, @Answer, @RequestParam, @CreatedOn)"

	'  Dim Conn As SqlConnection = New SqlConnection(connString)
	'  Conn.Open()

	'  Try
	'    Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand(strSQL, Conn)
	'    cmd.CommandType = Data.CommandType.Text
	'    Dim param As System.Data.SqlClient.SqlParameter
	'    param = cmd.Parameters.AddWithValue("@Modulname", "Err: SPJobPlattformsCreateXML.asmx")
	'    param = cmd.Parameters.AddWithValue("@ModulVersion", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@UserID", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@Answer", strErrorMessage)
	'    param = cmd.Parameters.AddWithValue("@RequestParam", String.Empty)
	'    param = cmd.Parameters.AddWithValue("@CreatedOn", Now)

	'    cmd.ExecuteNonQuery()

	'  Catch ex As Exception
	'    '     strResult = String.Format("Fehler: SaveUserToDb: {0}{1}{2}", ex.Message, vbNewLine, strSQL)

	'  Finally

	'    If Not Conn Is Nothing Then
	'      Conn.Close()
	'      Conn.Dispose()
	'    End If

	'  End Try

	'  Return strResult
	'End Function


#End Region


End Class

