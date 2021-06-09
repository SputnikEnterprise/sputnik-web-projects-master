Namespace JobPlatform.JobsCH

  ''' <summary>
  ''' Represents a job vacancy.
  ''' </summary>
  Public Class Vacancy

#Region "Private Fields"

    ''' <summary>
    ''' The validation erros.
    ''' </summary>
    Private m_ValidationErrors As StringBuilder = New StringBuilder()

#End Region

#Region "Public Properties"

    Public Property OrganisationsID As Integer?
    Public Property InseratID As Integer?

    Public Property Vorspann As String
    Public Property Beruf As String
    Public Property Text As String
    Public Property PLZ As String
    Public Property Ort As String

    Public Property Kontakt As String
    Public Property Email As String
    Public Property Url

    Public Property Titel As String
    Public Property Anriss As String
    Public Property Firma As String

    Public Property Anstellungsgrad As Integer?
    Public Property Anstellungsgrad_Bis As Integer?
    Public Property Anstellungart As String


    Public Property RubrikID As String
    Public Property Position As String

    Public Property Branche As String
    Public Property Sprache As String
    Public Property Region As String

    Public Property Alter_Von As Integer?
    Public Property Alter_Bis As Integer?
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

    ''' <summary>
    ''' Gets boolean flag indicating if the data is valid for xml export.
    ''' </summary>
    Public ReadOnly Property IsDataValidForXml As Boolean
      Get

        m_ValidationErrors.Clear()

        Dim valid = True

        valid = valid And Check(OrganisationsID.HasValue, "OrganisationID must have value")
        valid = valid And Check(InseratID.HasValue, "InseratID must have value")

        valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Beruf)), "Beruf must have a value")
        valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Text)), "(Vorspann / Anforderungen / Aufgaben / Tätigkeit) must have a value")


        ' Check Anstellugnsgrad 
        If Anstellungsgrad.HasValue Then
          valid = valid And Check(Anstellungsgrad >= 1, "Anstellungsgrad must be >= 1")
          valid = valid And Check(Anstellungsgrad <= 100, "Anstellungsgrad must be <= 100")
        End If

        ' Check Anstellugnsgrad_Bis 
        If Anstellungsgrad_Bis.HasValue Then
          valid = valid And Check(Anstellungsgrad_Bis >= 1, "Anstellungsgrad_Bis must be >= 1")
          valid = valid And Check(Anstellungsgrad_Bis <= 100, "Anstellungsgrad_Bis must be <= 100")
        End If

        ' Check  Anstellugnsgrad_Bis >= Anstellungsgrad
        If Anstellungsgrad.HasValue AndAlso Anstellungsgrad_Bis.HasValue Then
          valid = valid And Check(Anstellungsgrad_Bis >= Anstellungsgrad, "Anstellugnsgrad_Bis must be >= Anstellungsgrad")
        End If

        ' Check Sprache
        If Not String.IsNullOrEmpty(Sprache) Then

          Dim lang = Sprache.Trim().ToLower()

          Select Case lang
            Case "de"
            Case "it"
            Case "fr"
              ' ok
            Case Else
              valid = valid And Check(False, "Sprache must be in [de, it, fr]")
          End Select

        End If

        ' Check RubrikID and Position
        If Not String.IsNullOrEmpty(TrimString(RubrikID)) Or
          Not String.IsNullOrEmpty(TrimString(Position)) Then

          Dim tRubridId As String = NothingToEmptyString(TrimString(RubrikID))
          Dim tPoitionId As String = NothingToEmptyString(TrimString(Position))

          Dim count1 = tRubridId.Count(Function(c) c = ":")
          Dim count2 = tPoitionId.Count(Function(c) c = ":")

          valid = valid And Check(count1 = count2, "RubrikID and Position do not have same number of values.")

        End If

        'Check Alter_Von
        If Alter_Von.HasValue Then

          Select Case Alter_Von
            Case 0
            Case 25
            Case 35
            Case 45
              ' ok
            Case Else
              valid = valid And Check(False, "Alter_Von must be in [0, 25, 35, 45]")
          End Select

        End If

        'Check Alter_Bis
        If Alter_Bis.HasValue Then

          Select Case Alter_Bis
            Case 0
            Case 24
            Case 34
            Case 44
            Case 65
            Case 100
              ' ok
            Case Else
              valid = valid And Check(False, "Alter_Bis must be in [0, 24, 34, 44, 65, 100]")
          End Select

        End If

        'Check Alter_Bis >= Alter_Von
        If Alter_Von.HasValue AndAlso Alter_Bis.HasValue Then
          valid = valid And Check(Alter_Bis >= Alter_Von, "Alter_Bis must be >= Alter_Von")
        End If

        ' Check Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau
        If Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Kandidat)) Or
          Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Niveau)) Then

          Dim tSprachKentnis As String = NothingToEmptyString(TrimString(Sprachkenntniss_Kandidat))
          Dim tSprachNiveau As String = NothingToEmptyString(TrimString(Sprachkenntniss_Niveau))

          Dim count1 = tSprachKentnis.Count(Function(c) c = ":")
          Dim count2 = tSprachNiveau.Count(Function(c) c = ":")

          valid = valid And Check(count1 = count2, "Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau do not have same number of values")
          valid = valid And Check(count1 <= 4, "To many Sprachkenntniss_Kandidat values (max 3 are allowed)")
          valid = valid And Check(count2 <= 4, "To many Sprachkenntniss_Niveau values (max 3 are allowed)")

        End If

        ' Check Bildungsniveau
        If Not String.IsNullOrEmpty(TrimString(Bildungsniveau)) Then

          Dim tBildungsNiveau As String = NothingToEmptyString(TrimString(Bildungsniveau))

          Dim count = tBildungsNiveau.Count(Function(c) c = ":")

          valid = valid And Check(count <= 4, "To many Bildungsniveau values (max 3 are allowed)")

        End If

        ' Check Berufserfahrung and Berufserfahrung_Position
        If Not String.IsNullOrEmpty(TrimString(Berufserfahrung)) Or
          Not String.IsNullOrEmpty(TrimString(Berufserfahrung_Position)) Then

          Dim tBerufserfahrung As String = NothingToEmptyString(TrimString(Berufserfahrung))
          Dim tBerufserfahrung_Position As String = NothingToEmptyString(TrimString(Berufserfahrung_Position))

          Dim count1 = tBerufserfahrung.Count(Function(c) c = ":")
          Dim count2 = tBerufserfahrung_Position.Count(Function(c) c = ":")

          valid = valid And Check(count1 = count2, "Berufserfahrung and Berufserfahrung_Position do not have same number of values")
          valid = valid And Check(count1 <= 3, "To many Berufserfahrung values (max 2 are allowed)")
          valid = valid And Check(count2 <= 3, "To many Berufserfahrung_Position values (max 2 are allowed)")

        End If

        'Check Angebot
        If Angebot.HasValue Then

          Select Case Angebot
            Case 27 ' Classic Konto 
            Case 28 ' Classic Kontingent
            Case 29 ' Plus Konto 
            Case 30 ' Plus Kontingent
            Case 31 ' Premium Konto
            Case 32 ' Premium Kontingent 
						Case 35 ' Starter Konto 
						Case 36 ' Starter Kontingent 
						Case 37 ' Standard Konto 
						Case 38 ' Standard Kontingent 
						Case 39 ' Professional Konto 
						Case 40 ' Professional Kontingent 
						Case 41 ' Expert Konto 
						Case 42 ' Expert Kontingent 

						Case 0  ' empty value -> will not be written
							' ok
						Case Else
							valid = valid And Check(False, "Angebot must be in [0, 27, 28, 29, 30, 31, 32, 35, 36, 37, 38, 39, 40, 41, 42]")
					End Select

        End If

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
    ''' Creates an XElement from the data.
    ''' </summary>
    ''' <returns>An XElement object.</returns>
    Public Function ToXElement() As XElement

      If IsDataValidForXml() Then

        Dim vacancyElement As New XElement("INSERAT",
                                           New XElement("ORGANISATIONID", TrimString(NullableIntToString(OrganisationsID))),
                                           New XElement("INSERATID", LimitStringLength(TrimString(NullableIntToString(InseratID)), 64)),
                                           New XElement("VORSPANN", WrapStringInCData(TrimString(Vorspann))),
                                           New XElement("BERUF", WrapStringInCData(LimitStringLength(TrimString(Beruf), 255))),
                                           New XElement("TEXT", WrapStringInCData(Trim(Text))),
                                           New XElement("PLZ", LimitStringLength(TrimString(PLZ), 10)),
                                           New XElement("ORT", LimitStringLength(TrimString(Ort), 50)),
                                           New XElement("KONTAKT", WrapStringInCData(TrimString(Kontakt))),
                                           New XElement("EMAIL", LimitStringLength(TrimString(Email), 50)),
                                           New XElement("URL", LimitStringLength(TrimString(Url), 100)))

        ' Optinal fields

        If Not String.IsNullOrEmpty(TrimString(Titel)) Then
          vacancyElement.Add(New XElement("TITEL", WrapStringInCData(LimitStringLength(TrimString(Titel), 100))))
        End If
        If Not String.IsNullOrEmpty(TrimString(Anriss)) Then
          vacancyElement.Add(New XElement("ANRISS", WrapStringInCData(LimitStringLength(TrimString(Anriss), 150))))
        End If

        If Not String.IsNullOrEmpty(TrimString(Firma)) Then
          vacancyElement.Add(New XElement("FIRMA", LimitStringLength(TrimString(Firma), 60)))
        End If

        If Anstellungsgrad.HasValue AndAlso Anstellungsgrad_Bis.HasValue Then
          vacancyElement.Add(New XElement("ANSTELLUNGSGRAD", NullableIntToString(Anstellungsgrad)))
          vacancyElement.Add(New XElement("ANSTELLUNGSGRAD_BIS", NullableIntToString(Anstellungsgrad_Bis)))
        ElseIf Anstellungsgrad.HasValue Then
          vacancyElement.Add(New XElement("ANSTELLUNGSGRAD", NullableIntToString(Anstellungsgrad)))
          vacancyElement.Add(New XElement("ANSTELLUNGSGRAD_BIS", NullableIntToString(Anstellungsgrad))) ' same value as Anstellungsgrad
        End If

        If Not String.IsNullOrEmpty(TrimString(Anstellungart)) Then
          vacancyElement.Add(New XElement("ANSTELLUNGSART", LimitStringLength(TrimString(Anstellungart), 50)))
        End If

        If Not String.IsNullOrEmpty(TrimString(RubrikID)) AndAlso Not String.IsNullOrEmpty(TrimString(Position)) Then
          vacancyElement.Add(New XElement("RUBRIKID", TrimString(RubrikID)))
          vacancyElement.Add(New XElement("POSITION", TrimString(Position)))
        End If

        If Not String.IsNullOrEmpty(TrimString(Branche)) Then
          vacancyElement.Add(New XElement("BRANCHE", TrimString(Branche)))
        End If

        If Not String.IsNullOrEmpty(TrimString(Sprache)) Then
          vacancyElement.Add(New XElement("SPRACHE", TrimString(NothingToEmptyString(Sprache).ToLower())))
        End If

        If Not String.IsNullOrEmpty(TrimString(Region)) Then
          vacancyElement.Add(New XElement("REGION", TrimString(Region)))
        End If

        If Alter_Von.HasValue AndAlso Alter_Bis.HasValue Then
          vacancyElement.Add(New XElement("ALTER_VON", NullableIntToString(Alter_Von)))
          vacancyElement.Add(New XElement("ALTER_BIS", NullableIntToString(Alter_Bis)))
        End If

        If Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Kandidat)) AndAlso Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Niveau)) Then
          vacancyElement.Add(New XElement("SPRACHKENNTNIS_KANDIDAT", TrimString(Sprachkenntniss_Kandidat)))
          vacancyElement.Add(New XElement("SPRACHKENNTNIS_NIVEAU", TrimString(Sprachkenntniss_Niveau)))
        Else
          vacancyElement.Add(New XElement("SPRACHKENNTNIS_KANDIDAT", ":0:"))
          vacancyElement.Add(New XElement("SPRACHKENNTNIS_NIVEAU", ":0:"))
        End If

        If Not String.IsNullOrEmpty(TrimString(Bildungsniveau)) Then
          vacancyElement.Add(New XElement("BILDUNGSNIVEAU", TrimString(Bildungsniveau)))
        End If

        If Not String.IsNullOrEmpty(TrimString(Berufserfahrung)) AndAlso Not String.IsNullOrEmpty(TrimString(Berufserfahrung_Position)) Then
          vacancyElement.Add(New XElement("BERUFSERFAHRUNG", TrimString(Berufserfahrung)))
          vacancyElement.Add(New XElement("BERUFSERFAHRUNG_POSITION", TrimString(Berufserfahrung_Position)))
        Else
          vacancyElement.Add(New XElement("BERUFSERFAHRUNG", WrapStringInCData(":0:")))
          vacancyElement.Add(New XElement("BERUFSERFAHRUNG_POSITION", ":0:"))
        End If

        If Angebot.HasValue AndAlso Angebot > 0 Then
          vacancyElement.Add(New XElement("ANGEBOT", NullableIntToString(Angebot)))
        End If

				If Not String.IsNullOrEmpty(TrimString(Direkt_Url)) Then
					vacancyElement.Add(New XElement("DIREKT_URL", LimitStringLength(TrimString(Direkt_Url), 255)))
				End If
				If Not String.IsNullOrEmpty(TrimString(Direkt_Url_Post_Args)) Then
					vacancyElement.Add(New XElement("Direkt_Url_Post_Args", LimitStringLength(TrimString(Direkt_Url_Post_Args), 255)))
				End If

        If Not String.IsNullOrEmpty(TrimString(Xing_Poster_URL)) Then
          vacancyElement.Add(New XElement("XING_POSTER_URL", LimitStringLength(TrimString(Xing_Poster_URL), 255)))
        End If

        If Xing_Company_Is_Poc.HasValue Then
          vacancyElement.Add(New XElement("XING_COMPANY_IS_POC", If(Xing_Company_Is_Poc, "1", "0")))
        End If

        If Not String.IsNullOrEmpty(TrimString(Xing_Company_Profile_URL)) Then
          vacancyElement.Add(New XElement("XING_COMPANY_PROFILE_URL", LimitStringLength(TrimString(Xing_Company_Profile_URL), 255)))
        End If

        If Layout.HasValue Then
          vacancyElement.Add(New XElement("LAYOUT", NullableIntToString(Layout)))
        End If

        If Logo.HasValue Then
          vacancyElement.Add(New XElement("LOGO", NullableIntToString(Logo)))
        End If

        If Not String.IsNullOrEmpty(TrimString(Bewerben_URL)) Then
          vacancyElement.Add(New XElement("BEWERBEN_URL", LimitStringLength(TrimString(Bewerben_URL), 255)))
        End If

        Return vacancyElement

      End If

      Return Nothing

    End Function

#End Region


#Region "Private Methods"

    ''' <summary>
    ''' Limits the string length.
    ''' </summary>
    ''' <param name="str">The string.</param>
    ''' <param name="maxLength">The max length.</param>
    ''' <returns>String or cut string if its to long.</returns>
    Private Function LimitStringLength(ByVal str As String, ByVal maxLength As Integer) As String

      If str Is Nothing Then
        Return Nothing
      End If

      If str.Length <= maxLength Then
        Return str
      End If

      Return str.Substring(0, maxLength)

    End Function

    ''' <summary>
    ''' Trims a string.
    ''' </summary>
    ''' <param name="str">The string.</param>
    ''' <returns>The trimmed string.</returns>
    Private Function TrimString(ByVal str As String) As String

      If str Is Nothing Then
        Return Nothing
      End If

      Return str.Trim()

    End Function

    ''' <summary>
    ''' Converts nothing string to empty string.
    ''' </summary>
    ''' <param name="str">The string.</param>
    ''' <returns>Empty string if string is nothing else the passed string.</returns>
    Private Function NothingToEmptyString(ByVal str) As String

      If String.IsNullOrEmpty(str) Then
        Return String.Empty
      End If

      Return str

    End Function

    ''' <summary>
    ''' Wraps a string in a CData tag. 
    ''' </summary>
    ''' <param name="str">The string.</param>
    ''' <returns>The wrapped string.</returns>
    Private Function WrapStringInCData(ByVal str As String) As XCData

      If str Is Nothing Then
        Return New XCData(String.Empty)
      Else
        Return New XCData(str)
      End If

    End Function

    ''' <summary>
    ''' Converts a nullable integer to a string.
    ''' </summary>
    ''' <param name="int">The integer value.</param>
    ''' <returns>Converted integer string or string.empty if nullable integer is nothing.</returns>
    Protected Shared Function NullableIntToString(ByVal int As Integer?)

      If int.HasValue Then
        Return int.ToString()
      End If

      Return String.Empty

    End Function

    ''' <summary>
    ''' Helper method to catch validation errors.
    ''' </summary>
    ''' <param name="isValid">Boolan flag indicating if validation is valid.</param>
    ''' <param name="stringIfNotValid">String if not valid.</param>
    ''' <returns>isValid value.</returns>
    Private Function Check(ByVal isValid As Boolean, ByVal stringIfNotValid As String) As Boolean

      If Not isValid Then
        m_ValidationErrors.Append(stringIfNotValid)
        m_ValidationErrors.Append(";")
      End If

      Return isValid
    End Function

#End Region


  End Class


End Namespace

