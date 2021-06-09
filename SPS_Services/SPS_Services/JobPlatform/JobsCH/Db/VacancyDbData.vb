Namespace JobPlatform.JobsCH

  ''' <summary>
  ''' JobCh vacancy db data.
  ''' </summary>
  Public Class VacancyDbData

    ''' <summary>
    ''' is but organisation sub id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OrganisationsID As Integer?
		Public Property ID As Integer?
		Public Property InseratID As Integer?

		Public Property Vorspann As String
    Public Property Beruf As String
    Public Property Text_Taetigkeit As String
    Public Property Text_Anforderung As String
    Public Property Text_WirBieten As String
    Public Property PLZ As String
    Public Property Ort As String

    Public Property Kontakt As String
    Public Property Email As String
    Public Property URL

    ' Additional fields

    Public Property Titel As String
    Public Property Anriss As String
    Public Property Firma As String

    Public Property Anstellungsgrad_Von_Bis As String
    Public Property Anstellungsart As String

    Public Property RubrikID As String
    Public Property Position As String

    Public Property Branche As String
    Public Property Sprache As String
    Public Property Region As String

    Public Property Alter_Von_Bis As String

    Public Property Sprachkenntniss_Kandidat As String
    Public Property Sprachkenntniss_Niveau As String
    Public Property Bildungsniveau As String
    Public Property Berufserfahrung As String
    Public Property Berufserfahrung_Position As String

    Public Property Angebot As Integer?

		Public Property Direkt_Url As String
		Public Property Direkt_Url_Post_Args As String

    Public Property Xing_Poster_URL As String
    Public Property Xing_Company_Is_Poc As Boolean?
    Public Property Xing_Company_Profile_URL As String

    Public Property Layout As Integer?
    Public Property Logo As Integer?
    Public Property Bewerben_URL As String

  End Class

End Namespace