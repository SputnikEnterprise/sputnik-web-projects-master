Imports System.Data.SqlClient
Imports System.IO
Imports wsSPS_Services.JobPlatform

Namespace JobPlatform.OstJobsCH

  ''' <summary>
  ''' Xml gnererator for OstJobCh.
  ''' </summary>
  Public Class OstJobChVacanciesXMLGenerator
    Inherits VacancyXMLGeneratorBase

#Region "Private Fields"

    Private m_CustomerGuid As String

    Private m_DbAcceas As VacancyDbAccess

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="customerGuid">The customer guid.</param>
    Public Sub New(ByVal customerGuid As String)

      m_CustomerGuid = customerGuid
      m_DbAcceas = New VacancyDbAccess()

    End Sub

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Generate Vacancies to Xml
    ''' </summary>
    Public Function GenerateVacanciesXml() As XDocument

      Dim ostjobCHDBData As IEnumerable(Of Vacancy) = m_DbAcceas.LoadOstJobCHVacancyDbData(m_CustomerGuid)

      Dim xDoc As XDocument = MapDbVacanciesToXML(ostjobCHDBData)

      Return xDoc

    End Function

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Maps db vacancy data to jobCh compatible xml.
    ''' </summary>
    ''' <param name="vacancyData">The db vacancy data.</param>
    ''' <returns>The xml doc.</returns>
    Private Function MapDbVacanciesToXML(ByVal vacancyData As IEnumerable(Of Vacancy)) As XDocument

      Dim vacancies = New Vacancies

      ' Map each db data to jobCh data structure.
      For Each vacancy In vacancyData
        vacancies.AddVacancy(vacancy)
      Next

      Dim xmlDoc = vacancies.ToXDoc()

      If xmlDoc Is Nothing Then
        Throw New Exception(String.Format("Convert to xml failed (CustomerGuid={0}). Errors={1}", m_CustomerGuid, vacancies.ValidationErrors))
      End If

      Return xmlDoc

    End Function

#End Region

  End Class

End Namespace
