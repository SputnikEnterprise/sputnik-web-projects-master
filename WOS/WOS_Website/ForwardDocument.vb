Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Web.Script.Services
Imports System.Data.SqlClient
Imports System.Data

Namespace WebServices
  ' Um das Aufrufen dieses Webdiensts aus einem Skript mit ASP.NET AJAX zuzulassen, heben Sie die Auskommentierung der folgenden Zeile auf.
  ' <System.Web.Script.Services.ScriptService()> _
  <WebService(Namespace:="http://tempuri.org/")> _
  <WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
  <ScriptService()> _
  <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
  Public Class ForwardDocument
    Inherits System.Web.Services.WebService

    ' Called by java script in order to forward documents
    <WebMethod(EnableSession:=True)> _
    Public Function FowardDocument(ByVal documentEmailReceiver As String, ByVal dbId As Integer, ByVal guid As String, _
                                   ByVal docArt As String) As Boolean

      ' Retrievs application info object
      Dim appInfo As ApplicationInfo = CType(HttpContext.Current.Session(ApplicationInfo.SESSION_KEY), ApplicationInfo)

      Dim documentDataSet As DataSet = ReadDocumentData(appInfo, dbId, guid, docArt)

      If (documentDataSet.Tables.Count > 0 AndAlso documentDataSet.Tables(0).Rows.Count = 1) Then

        Try

          Dim dataRow As DataRow = documentDataSet.Tables(0).Rows(0)

          Dim emailTempleText As String = String.Empty

          If (appInfo.CurrentGuidName = appInfo.CandidateQueryParameterName) Then
            emailTempleText = Utility.GetEmailTemplate("Candidate_ForwardDocument.txt")
            emailTempleText = FillDocumentForwardTemplateForCandidate(emailTempleText, dataRow)
          ElseIf (appInfo.CurrentGuidName = appInfo.KDQueryParameterName) Then
            emailTempleText = Utility.GetEmailTemplate("KD_ForwardDocument.txt")
            emailTempleText = FillDocumentForwardTemplateForKD(emailTempleText, dataRow)
          ElseIf (appInfo.CurrentGuidName = appInfo.ZHDQueryParameterName) Then
            emailTempleText = Utility.GetEmailTemplate("ZHD_ForwardDocument.txt")
            emailTempleText = FillDocumentForwardTemplateForZHD(emailTempleText, dataRow)
          End If

          Dim docScanBytes As Byte() = dataRow("DocScan")
          'Dim docInfo As String = dataRow("Doc_Info")     ' geändert durch Sputnik 30.4.11
          Dim docInfo As String = dataRow("DocFileName")

          ' Send mail to (KD or ZHD) and the advisor
          Dim strAttachmentFilename As String = String.Format("{0}", docInfo)
          Utility.SendMailWithAttachment(appInfo.EmailSenderAddress, documentEmailReceiver, _
                                         "Dokumentweiterleitung", emailTempleText, True, _
                                         strAttachmentFilename, docScanBytes, "application/pdf")

        Catch ex As Exception
          Return False
        End Try

      End If

      Return True
    End Function

    ' Reads the document data
    Private Function ReadDocumentData(ByVal appInfo As ApplicationInfo, ByVal dbId As Integer, ByVal guid As String, ByVal docArt As String) As DataSet

      Dim documentRetrievalSQL As String = appInfo.DocumentSQLStrings.LoadDocumentForEmailForwardSQL

      ' Create connetion
      Dim conStr As String = appInfo.ConnectionString
      Dim objCon As New SqlConnection(conStr)

      Dim documentsDataSet As New DataSet

      Try

        'SQL Command
        Dim objCommand As New SqlCommand(documentRetrievalSQL, objCon)
        objCommand.CommandType = CommandType.Text

        ' Parameters
        Dim idParam As New SqlParameter()
        Dim currentGuidParam As New SqlParameter()
        Dim docArtParam As New SqlParameter()

        idParam = objCommand.Parameters.Add(New SqlParameter("@ID", SqlDbType.Int, 50))
        currentGuidParam = objCommand.Parameters.Add(New SqlParameter("@CurrentGuid", SqlDbType.VarChar, 255))
        docArtParam = objCommand.Parameters.Add(New SqlParameter("@Doc_Art", SqlDbType.VarChar, 255))

        idParam.Direction = ParameterDirection.Input
        idParam.Value = dbId
        currentGuidParam.Value = guid
        docArtParam.Value = docArt

        'DataAdapter
        Dim objAdapter As SqlDataAdapter
        objAdapter = New SqlDataAdapter()
        objAdapter.SelectCommand = objCommand

        'Fill DataSet
        objAdapter.Fill(documentsDataSet)

      Finally
        objCon.Close()
      End Try
      objCon.Close()

      Return documentsDataSet

    End Function

    ' Fill candidate forward template
    Private Function FillDocumentForwardTemplateForCandidate(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

      ' geändert durch 30.4.11
      Try
        Dim maFirstname = Utility.GetColumnTextStr(dataRow, "MA_Vorname", String.Empty)
        Dim maSurname = Utility.GetColumnTextStr(dataRow, "MA_Nachname", String.Empty)
        Dim userFirstname = Utility.GetColumnTextStr(dataRow, "User_Vorname", String.Empty)
        Dim userSurname = Utility.GetColumnTextStr(dataRow, "User_Nachname", String.Empty)
        Dim userTelephone = Utility.GetColumnTextStr(dataRow, "User_Telefon", String.Empty)
        Dim userFax = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
				Dim userHomepage = Utility.GetColumnTextStr(dataRow, "User_Homepage", String.Empty)

        emailTemplateText = emailTemplateText.Replace("{MAVorname}", maFirstname)
        emailTemplateText = emailTemplateText.Replace("{MANachname}", maSurname)
        emailTemplateText = emailTemplateText.Replace("{UserVorname}", userFirstname)
        emailTemplateText = emailTemplateText.Replace("{UserNachname}", userSurname)
        emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
        emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
				emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", userHomepage)
				emailTemplateText = emailTemplateText.Replace("{UserHomepage}", userHomepage)

      Catch ex As Exception
        emailTemplateText = String.Format("Fehler (FillDocumentForwardTemplateForCandidate): {0}", ex.Message)

      End Try

      Return emailTemplateText

    End Function

    ' Fill kd forward template
    Private Function FillDocumentForwardTemplateForKD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

      ' geändert durch 30.4.11
      Try
        Dim kdName As String = Utility.GetColumnTextStr(dataRow, "KD_Name", String.Empty)
        Dim adviser As String = Utility.GetColumnTextStr(dataRow, "KD_Berater", String.Empty)
        Dim customerName As String = Session("CustomerName")
        Dim customerStreet As String = Session("CustomerStreet")
        Dim customerPlace As String = Session("CustomerPlace")
        Dim userTelephone As String = dataRow("User_Telefon")
        Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
        Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
        Dim customerHompage As String = Session("CustomerHomepage")
				Dim userHomepage = Utility.GetColumnTextStr(dataRow, "User_Homepage", String.Empty)


        emailTemplateText = emailTemplateText.Replace("{KDName}", kdName)
        emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
        emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
        emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
        emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
        emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
        emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
        emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
        emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)
				emailTemplateText = emailTemplateText.Replace("{UserHomepage}", userHomepage)

      Catch ex As Exception
        emailTemplateText = String.Format("Fehler (FillDocumentForwardTemplateForKD): {0}", ex.Message)

      End Try

      Return emailTemplateText

    End Function

    ' Fill ZHD forward template
    Private Function FillDocumentForwardTemplateForZHD(ByVal emailTemplateText As String, ByVal dataRow As DataRow) As String

      ' geändert durch 30.4.11
      Try
        Dim salutation As String = Utility.GetColumnTextStr(dataRow, "ZHD_BriefAnrede", String.Empty)
        Dim zhdName As String = Utility.GetColumnTextStr(dataRow, "ZHD_Nachname", String.Empty)
        Dim zhdFirstName As String = Utility.GetColumnTextStr(dataRow, "ZHD_Vorname", String.Empty)
        Dim kdName As String = Utility.GetColumnTextStr(dataRow, "KD_Name", String.Empty)
        Dim adviser As String = Utility.GetColumnTextStr(dataRow, "ZHD_Berater", String.Empty)
        Dim customerName As String = Session("CustomerName")
        Dim customerStreet As String = Session("CustomerStreet")
        Dim customerPlace As String = Session("CustomerPlace")
        Dim userTelephone As String = Utility.GetColumnTextStr(dataRow, "User_Telefon", String.Empty)
        Dim userFax As String = Utility.GetColumnTextStr(dataRow, "User_Telefax", String.Empty)
        Dim userEmail As String = Utility.GetColumnTextStr(dataRow, "User_eMail", String.Empty)
        Dim customerHompage As String = Session("CustomerHomepage")
				Dim userHomepage = Utility.GetColumnTextStr(dataRow, "User_Homepage", String.Empty)

        emailTemplateText = emailTemplateText.Replace("{Anrede}", salutation)
        emailTemplateText = emailTemplateText.Replace("{ZHDNachname}", zhdName)
        emailTemplateText = emailTemplateText.Replace("{ZHDVorname}", zhdFirstName)
        emailTemplateText = emailTemplateText.Replace("{KDName}", kdName)
        emailTemplateText = emailTemplateText.Replace("{Berater}", adviser)
        emailTemplateText = emailTemplateText.Replace("{KundenName}", customerName)
        emailTemplateText = emailTemplateText.Replace("{KundeStrasse}", customerStreet)
        emailTemplateText = emailTemplateText.Replace("{KundeOrt}", customerPlace)
        emailTemplateText = emailTemplateText.Replace("{UserTelefon}", userTelephone)
        emailTemplateText = emailTemplateText.Replace("{UserTelefax}", userFax)
        emailTemplateText = emailTemplateText.Replace("{UserEmail}", userEmail)
        emailTemplateText = emailTemplateText.Replace("{KundeHomepage}", customerHompage)
				emailTemplateText = emailTemplateText.Replace("{UserHomepage}", userHomepage)

      Catch ex As Exception
        emailTemplateText = String.Format("Fehler (FillDocumentForwardTemplateForKD): {0}", ex.Message)

      End Try

      Return emailTemplateText

    End Function

  End Class
End Namespace

