
Namespace JobPlatform.JobsCH

  ''' <summary>
  ''' Represents a bunch of jobCh job vacancies.
  ''' </summary>
  Public Class Vacancies

#Region "Private Fields"

    Private m_Vacancies As List(Of Vacancy)

    ''' <summary>
    ''' The validation erros.
    ''' </summary>
    Private m_ValidationErrors As StringBuilder = New StringBuilder()

#End Region

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    Public Sub New()

      m_Vacancies = New List(Of Vacancy)


    End Sub
#End Region

#Region "Public Propeties"


    ''' <summary>
    ''' Gets boolean flag indicating if the data is valid for xml export.
    ''' </summary>
    Public ReadOnly Property IsDataValidForXml As Boolean
      Get

        m_ValidationErrors.Clear()

        Dim valid = True

        For Each vacancy As Vacancy In m_Vacancies
          valid = valid And CheckVacancy(vacancy)
        Next

        Return valid

      End Get

    End Property

    ''' <summary>
    ''' Gets the validation errors.
    ''' </summary>
    ''' <returns>The validation errors.</returns>
    Public ReadOnly Property ValidationErrors As String
      Get
        Return m_ValidationErrors.ToString()
      End Get
    End Property

#End Region

#Region "Public Methods"

    ''' <summary>
    ''' Adds a vacancy.
    ''' </summary>
    ''' <param name="vacancy">The vacancy.</param>
    Public Sub AddVacancy(ByVal vacancy As Vacancy)
      m_Vacancies.Add(vacancy)
    End Sub


    ''' <summary>
    ''' Creates an XDoc from the data.
    ''' </summary>
    ''' <returns>An XDoc object.</returns>
    Public Function ToXDoc() As XDocument

      If IsDataValidForXml Then

        Dim xDoc As New XDocument(
               New XDeclaration("1.0", "ISO-8859-1", True),
               New XElement("JOBS",
                            New XElement("INSERATE",
                                         (From vac In m_Vacancies
                                          Select vac.ToXElement()))))

        Return xDoc

      End If

      Return Nothing

    End Function


#End Region

#Region "Pivate Functions"

    ''' <summary>
    ''' Helper method to check a vacancy.
    ''' </summary>
    ''' <param name="vacancy">The vacancy.</param>
    ''' <returns>Boolean flag indicating if vacancy is valid.</returns>
    Private Function CheckVacancy(ByVal vacancy As Vacancy) As Boolean

      If Not vacancy.IsDataValidForXml Then
        m_ValidationErrors.Append(String.Format("OrganisationId={0},InseratId={1}. Errors={2}",
                                              If(vacancy.OrganisationsID.HasValue.ToString(), vacancy.OrganisationsID, "?"),
                                              If(vacancy.InseratID.HasValue.ToString(), vacancy.InseratID, "?"), vacancy.ValidationErrors))

        Return False
      End If

      Return True
    End Function

#End Region

  End Class

End Namespace
