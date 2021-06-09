Imports System.Data.SqlClient

Namespace JobPlatform.OstJobsCH

  ' OstJobCh vacancy database access.
  Public Class VacancyDbAccess
    Inherits JobPlatform.JobPlatformDbAccessBase

#Region "Public Methods"

    ''' <summary>
    '''  Loads OstJobCH database vacancy data.
    ''' </summary>
    ''' <param name="customerGuid">The customer guid.</param>
    ''' <returns> List of OstJobCh database data.</returns>
    Public Function LoadOstJobCHVacancyDbData(ByVal customerGuid As String) As IEnumerable(Of Vacancy)

      Dim connString As String = My.Settings.ConnStr_JobCh
      Dim conn As SqlConnection = New SqlConnection(connString)
      Dim reader As SqlClient.SqlDataReader = Nothing

      Dim listOfVacancyData As List(Of Vacancy) = Nothing
      Try

        ' Create command.
        Dim cmd As System.Data.SqlClient.SqlCommand =
          New System.Data.SqlClient.SqlCommand("SELECT * FROM tblOstJobCHPlattform WHERE Customer_Guid = @Customer_Guid ORDER BY CreatedOn Desc", conn)

        cmd.CommandType = CommandType.Text
        cmd.Parameters.AddWithValue("@Customer_Guid", customerGuid)

        ' Open connection to database.
        conn.Open()

        ' Execute the data reader.
        reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

        listOfVacancyData = New List(Of Vacancy)

        ' Read all data.
        While (reader.Read())
					Dim vacancyData As New Vacancy With {
						.CustomerGuid = SafeGetString(reader, "Customer_Guid"),
						.UserGuid = SafeGetString(reader, "User_Guid"),
						.VakNr = SafeGetInteger(reader, "VakNr", Nothing),
						.JobVersion = SafeGetString(reader, "JobVersion", String.Empty),
						.Company = SafeGetString(reader, "Company", String.Empty),
						.Title = SafeGetString(reader, "Title", String.Empty),
						.WorkplaceCountry = SafeGetString(reader, "Workplace_Country", String.Empty),
						.WorkplaceZip = SafeGetString(reader, "Workplace_Zip", String.Empty),
						.WorkplaceCity = SafeGetString(reader, "Workplace_City", String.Empty),
						.CompanyDescription = SafeGetString(reader, "Company_Description", String.Empty),
						.WirBieten = SafeGetString(reader, "WirBieten", String.Empty),
						.Anforderungen = SafeGetString(reader, "Anforderungen", String.Empty),
						.Aufgabe = SafeGetString(reader, "Aufgabe", String.Empty),
						.Contact = SafeGetString(reader, "Contact", String.Empty),
						.DescriptionUrl = SafeGetString(reader, "Description_url", String.Empty),
						.ApplicationUrl = SafeGetString(reader, "Application_url", String.Empty),
						.PublicationOstjobCh = SafeGetBoolean(reader, "Publication_ostjob_ch", False),
						.PublicationWestjobAt = SafeGetBoolean(reader, "Publication_westjob_at", False),
						.PublicationNicejobDe = SafeGetBoolean(reader, "Publication_nicejob_de", False),
						.PublicationZentraljobCh = SafeGetBoolean(reader, "Publication_zentraljob_ch", False),
						.PublicationMinisite = SafeGetBoolean(reader, "Publication_minisite", False),
						.Apprenticeship = SafeGetBoolean(reader, "Apprenticeship", False),
						.Template = SafeGetString(reader, "Template"),
						.Keywords = SafeGetString(reader, "keywords"),
						.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Now),
						.IsOnline = SafeGetBoolean(reader, "IsOnline", False)
					}

          listOfVacancyData.Add(vacancyData)

        End While
      Catch ex As Exception

      Finally
        If Not reader Is Nothing Then

          Try
            reader.Close()
          Catch
            ' Do nothing
          End Try

        End If
      End Try

      ' Return search data as an array.
      Return listOfVacancyData.ToArray()

    End Function

#End Region

  End Class

End Namespace