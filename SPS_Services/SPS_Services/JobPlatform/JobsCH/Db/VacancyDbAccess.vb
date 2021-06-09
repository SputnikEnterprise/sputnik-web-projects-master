Imports System.Data.SqlClient

Namespace JobPlatform.JobsCH

  ' JobCh vacancy database access.
  Public Class VacancyDbAccess
    Inherits JobPlatform.JobPlatformDbAccessBase

#Region "Public Methods"

    ''' <summary>
    '''  Loads JobCH database vacancy data.
    ''' </summary>
    ''' <param name="customerGuid">The customer guid.</param>
    ''' <returns> List of JobCh database data.</returns>
    Public Function LoadJobCHVacancyDbData(ByVal customerGuid As String, ByVal organisationSubID As String) As IEnumerable(Of VacancyDbData)

      Dim connString As String = My.Settings.ConnStr_JobCh
      Dim conn As SqlConnection = New SqlConnection(connString)
      Dim reader As SqlClient.SqlDataReader = Nothing

      Dim listOfVacancyData As List(Of VacancyDbData) = Nothing
      Try

        ' Create command.
				Dim cmd As System.Data.SqlClient.SqlCommand =
					New System.Data.SqlClient.SqlCommand("SELECT * FROM tblJobCHPlattform WHERE OrganisationID = @OrganisationSubID ORDER BY StartDate Desc", conn)

				' not good for work-shop
				'New System.Data.SqlClient.SqlCommand("SELECT * FROM tblJobCHPlattform WHERE Customer_Guid = @Customer_Guid And OrganisationID = @OrganisationSubID ORDER BY StartDate Desc", conn)

        cmd.CommandType = CommandType.Text
				'cmd.Parameters.AddWithValue("@Customer_Guid", customerGuid)
        cmd.Parameters.AddWithValue("@OrganisationSubID", organisationSubID)

        ' Open connection to database.
        conn.Open()

        ' Execute the data reader.
        reader = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection)

        listOfVacancyData = New List(Of VacancyDbData)

        ' Read all data.
        While (reader.Read())
					' TODO: direkt_url could be defined new! can save empty as error 
					Dim dbVacancyData As New VacancyDbData With {
						.ID = SafeGetInteger(reader, "ID", Nothing),
						.OrganisationsID = SafeGetInteger(reader, "OrganisationID", Nothing),
						.InseratID = SafeGetInteger(reader, "InseratID", Nothing),
						.Vorspann = SafeGetString(reader, "Vorspann"),
						.Beruf = SafeGetString(reader, "Beruf"),
						.Text_Taetigkeit = SafeGetString(reader, "Text_Taetigkeit"),
						.Text_Anforderung = SafeGetString(reader, "Text_Anforderung"),
						.Text_WirBieten = SafeGetString(reader, "Text_WirBieten"),
						.PLZ = SafeGetString(reader, "PLZ"),
						.Ort = SafeGetString(reader, "Ort"),
						.Kontakt = SafeGetString(reader, "Kontakt"),
						.Email = SafeGetString(reader, "Email"),
						.URL = SafeGetString(reader, "URL"),
						.Titel = SafeGetString(reader, "Titel"),
						.Anriss = SafeGetString(reader, "Anriss"),
						.Firma = SafeGetString(reader, "Firma"),
						.Anstellungsgrad_Von_Bis = SafeGetString(reader, "Anstellungsgrad_Von_Bis"),
						.Anstellungsart = SafeGetString(reader, "Anstellungsart"),
						.RubrikID = SafeGetString(reader, "RubrikID"),
						.Position = SafeGetString(reader, "Position"),
						.Branche = SafeGetString(reader, "Branche"),
						.Sprache = SafeGetString(reader, "Sprache"),
						.Region = SafeGetString(reader, "Region"),
						.Alter_Von_Bis = SafeGetString(reader, "Alter_Von_Bis"),
						.Sprachkenntniss_Kandidat = SafeGetString(reader, "Sprachkenntniss_Kandidat"),
						.Sprachkenntniss_Niveau = SafeGetString(reader, "Sprachkenntniss_Niveau"),
						.Bildungsniveau = SafeGetString(reader, "Bildungsniveau"),
						.Berufserfahrung = SafeGetString(reader, "Berufserfahrung"),
						.Berufserfahrung_Position = SafeGetString(reader, "Berufserfahrung_Position"),
						.Angebot = SafeGetInteger(reader, "Angebot", Nothing),
						.Direkt_Url = SafeGetString(reader, "direkt_url", Nothing),
						.Direkt_Url_Post_Args = SafeGetString(reader, "direkt_url_post_args", Nothing),
			.Xing_Poster_URL = SafeGetString(reader, "Xing_Poster_URL", Nothing),
						.Xing_Company_Is_Poc = SafeGetBoolean(reader, "Xing_Company_Is_Poc", Nothing),
						.Xing_Company_Profile_URL = SafeGetString(reader, "Xing_Company_Profile_URL"),
						.Layout = SafeGetInteger(reader, "Lyout", Nothing),
						.Logo = SafeGetInteger(reader, "Logo", Nothing),
						.Bewerben_URL = SafeGetString(reader, "Bewerben_URL", Nothing)
					}

					listOfVacancyData.Add(dbVacancyData)

        End While

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