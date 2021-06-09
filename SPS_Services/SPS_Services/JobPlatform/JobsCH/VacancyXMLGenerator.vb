Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_Services.JobPlatform

Namespace JobPlatform.JobsCH

  ''' <summary>
  ''' Xml gnererator for JobCh.
  ''' </summary>
  Public Class JobChVacanciesXMLGenerator
    Inherits VacancyXMLGeneratorBase

#Region "Private Fields"

    Private m_CustomerGuid As String
    Private m_organisationSubID As String

    Private m_DbAcceas As VacancyDbAccess

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="customerGuid">The customer guid.</param>
    ''' <param name="organisationSubID">The organisation sub id.</param>
    Public Sub New(ByVal customerGuid As String, ByVal organisationSubID As Integer)

      m_CustomerGuid = customerGuid
      m_organisationSubID = organisationSubID
      m_DbAcceas = New VacancyDbAccess()

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GenerateVacanciesXml() As XDocument

      Dim jobCHDBData As IEnumerable(Of VacancyDbData) = m_DbAcceas.LoadJobCHVacancyDbData(m_CustomerGuid, m_organisationSubID)

      Dim xDoc As XDocument = MapDbVacanciesToXML(jobCHDBData)

      Return xDoc

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Maps db vacancy data to jobCh compatible xml.
    ''' </summary>
    ''' <param name="dbVacancyData">The db vacancy data.</param>
    ''' <returns>The xml doc.</returns>
    Private Function MapDbVacanciesToXML(ByVal dbVacancyData As IEnumerable(Of VacancyDbData)) As XDocument

      Dim vacancies = New Vacancies

      ' Map each db data to jobCh data structure.
      For Each dbVacancy In dbVacancyData

        Dim jobAd = MapDbVacancyToVacancyModel(dbVacancy)

        vacancies.AddVacancy(jobAd)

      Next

      Dim xmlDoc = vacancies.ToXDoc()

      If xmlDoc Is Nothing Then
        Throw New Exception(String.Format("Convert to xml failed (CustomerGuid={0}). Errors={1}", m_CustomerGuid, vacancies.ValidationErrors))
      End If

      Return xmlDoc

    End Function

    ''' <summary>
    ''' Maps db vacancy data to jobCh vacancy model data.
    ''' </summary>
    ''' <param name="dbVacancy">The database vacancy data.</param>
    ''' <returns>JobCh vacany model object.</returns>
    Private Function MapDbVacancyToVacancyModel(ByVal dbVacancy As VacancyDbData) As Vacancy

      Dim jobVacancyModel As New Vacancy

			jobVacancyModel.OrganisationsID = dbVacancy.OrganisationsID

			jobVacancyModel.InseratID = dbVacancy.ID  ' dbVacancy.InseratID
			jobVacancyModel.Vorspann = dbVacancy.Vorspann
			jobVacancyModel.Beruf = dbVacancy.Beruf

      Dim textBuffer As New StringBuilder

      If Not dbVacancy.Text_Taetigkeit Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", dbVacancy.Text_Taetigkeit))
      End If

      If Not dbVacancy.Text_Anforderung Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", dbVacancy.Text_Anforderung))
      End If

      If Not dbVacancy.Text_WirBieten Is Nothing Then
        textBuffer.Append(String.Format("{0}</br>", dbVacancy.Text_WirBieten))
      End If

      jobVacancyModel.Text = textBuffer.ToString()

      jobVacancyModel.PLZ = dbVacancy.PLZ
      jobVacancyModel.Ort = dbVacancy.Ort
      jobVacancyModel.Kontakt = dbVacancy.Kontakt
      jobVacancyModel.Email = dbVacancy.Email
      jobVacancyModel.Url = dbVacancy.URL

      ' Additional fields
      jobVacancyModel.Titel = dbVacancy.Titel
      jobVacancyModel.Anriss = dbVacancy.Anriss
      jobVacancyModel.Firma = dbVacancy.Firma

      Dim anstellungsgradTuple = SplitTwoValueString(dbVacancy.Anstellungsgrad_Von_Bis, "#")

      jobVacancyModel.Anstellungsgrad = anstellungsgradTuple.Item1
      jobVacancyModel.Anstellungsgrad_Bis = anstellungsgradTuple.Item2
      jobVacancyModel.Anstellungart = ConvertDbIdFormat1ToJobChIdFormat(dbVacancy.Anstellungsart)

      jobVacancyModel.RubrikID = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.RubrikID)
      jobVacancyModel.Position = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Position)

      jobVacancyModel.Branche = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Branche)
      jobVacancyModel.Sprache = dbVacancy.Sprache
      jobVacancyModel.Region = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Region)

      Dim alterTuple = SplitTwoValueString(dbVacancy.Alter_Von_Bis, "#")

      jobVacancyModel.Alter_Von = alterTuple.Item1
      jobVacancyModel.Alter_Bis = alterTuple.Item2
      jobVacancyModel.Sprachkenntniss_Kandidat = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Sprachkenntniss_Kandidat, 3) ' Max 3 ids
      jobVacancyModel.Sprachkenntniss_Niveau = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Sprachkenntniss_Niveau, 3) ' Max 3 ids
      jobVacancyModel.Bildungsniveau = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Bildungsniveau, 3) ' Max 3 ids
      jobVacancyModel.Berufserfahrung = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Berufserfahrung, 2) ' Max 2 ids
      jobVacancyModel.Berufserfahrung_Position = ConvertDbIdFormat2ToJobChIdFormat(dbVacancy.Berufserfahrung_Position, 2) ' Max 2 ids

      jobVacancyModel.Angebot = dbVacancy.Angebot

			jobVacancyModel.Direkt_Url = dbVacancy.Direkt_Url
			jobVacancyModel.Direkt_Url_Post_Args = dbVacancy.Direkt_Url_Post_Args

      jobVacancyModel.Xing_Poster_URL = dbVacancy.Xing_Poster_URL
      jobVacancyModel.Xing_Company_Is_Poc = dbVacancy.Xing_Company_Is_Poc
      jobVacancyModel.Xing_Company_Profile_URL = dbVacancy.Xing_Company_Profile_URL

      jobVacancyModel.Layout = dbVacancy.Layout
      jobVacancyModel.Logo = dbVacancy.Logo
      jobVacancyModel.Bewerben_URL = dbVacancy.Bewerben_URL

      Return (jobVacancyModel)

    End Function

    ''' <summary>
    ''' Convets the db id format1 to the jobsCH id format.
    ''' </summary>
    ''' <param name="dbIdString">Format is id#id#id ...</param>
    ''' <returns>JobCh id string :id:id:id: ...</returns>
    Private Function ConvertDbIdFormat1ToJobChIdFormat(ByVal dbIdString As String) As String

      If String.IsNullOrEmpty(dbIdString) Then
        Return dbIdString
      End If

      Dim tokens As String() = dbIdString.Split("#")

      Dim stringBuilder As New StringBuilder

      For Each token In tokens

        stringBuilder.Append(":")
        stringBuilder.Append(token)
      Next

      stringBuilder.Append(":")

      Return stringBuilder.ToString()
    End Function

    ''' <summary>
    ''' Convets the db id format2 to the jobsCH id format.
    ''' </summary>
    ''' <param name="dbIdString">Format is text|id#text|id#text|id ...</param>
    ''' <returns>JobCh id string :id:id:id: ...</returns>
    Private Function ConvertDbIdFormat2ToJobChIdFormat(ByVal dbIdString As String, Optional ByVal skiptAfterEntries As Integer? = Nothing) As String

      ' Check for empty string.
      If String.IsNullOrEmpty(dbIdString) Then
        Return dbIdString
      End If

      Dim ids As New List(Of String)

      ' Split by '#' symbol.
      Dim textAndIdTokens As String() = dbIdString.Split("#")

      For Each textAndIdToken In textAndIdTokens

        ' Split by pipe
        Dim token As String() = textAndIdToken.Split("|")

        If token.Count = 2 Then

          ids.Add(token(1))
        Else
          Throw New Exception(String.Format("Invalid format {0}.", dbIdString))
        End If

      Next

      If ids.Count = 0 Then
        Return String.Empty
      Else

        ' Convert ids to job ch format
        Dim stringBuilder As New StringBuilder

        Dim count As Integer = Math.Min(ids.Count, If(skiptAfterEntries.HasValue, skiptAfterEntries.Value, Integer.MaxValue))

        For i As Integer = 0 To count - 1

          Dim id As String = ids(i)

          stringBuilder.Append(":")
          stringBuilder.Append(id)
        Next

        stringBuilder.Append(":")

        Return stringBuilder.ToString()

      End If

    End Function

    ''' <summary>
    ''' Splits a two value string.
    ''' </summary>
    ''' <param name="str">The string.</param>
    ''' <param name="delimeter">The delimeter.</param>
    ''' <returns>Tuple with values.</returns>
    Private Function SplitTwoValueString(ByVal str As String, ByVal delimeter As String) As Tuple(Of Integer?, Integer?)

      Dim value1 As Integer? = Nothing
      Dim value2 As Integer? = Nothing

      If Not String.IsNullOrEmpty(str) Then

        Dim tokens As String() = str.Trim().Split(delimeter)

        If tokens.Count = 2 Then
          value1 = Integer.Parse(tokens(0))
          value2 = Integer.Parse(tokens(1))
        End If

      End If

      Return New Tuple(Of Integer?, Integer?)(value1, value2)

    End Function

#End Region

  End Class

End Namespace
