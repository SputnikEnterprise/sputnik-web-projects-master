Namespace JobPlatform.X28

	Public Class ProfilmatcherQueryData


		Private m_ValidationErrors As StringBuilder = New StringBuilder()

		Public Property ID As Integer
		Public Property CustomerID As String
		Public Property UserID As String
		Public Property Terms As New List(Of String)
		Public Property Companies As List(Of String)
		Public Property Companysizes As List(Of String)
		Public Property Industries As List(Of String)
		Public Property Recruitmentagencies As Boolean
		Public Property Management As Boolean
		Public Property Temporary As Boolean
		Public Property Regions As List(Of String)
		Public Property Locations As List(Of ProfilMatcherLocationData)
		Public Property LocationDistance As String

		Public Property Clusters As List(Of String)
		Public Property Experiences As List(Of String)
		Public Property Educations As List(Of String)
		Public Property Skills As List(Of String)
		Public Property WorkquotaMinimum As Integer
		Public Property WorkquotaMaximum As Integer
		Public Property DateFrom As DateTime
		Public Property DateTo As DateTime
		Public Property Page As String
		Public Property Size As String

		Public Property CreatedOn As DateTime


		''' <summary>
		''' Gets boolean flag indicating if the data is valid for xml export.
		''' </summary>
		Public ReadOnly Property IsDataValidForXml As Boolean
			Get

				m_ValidationErrors.Clear()

				Dim valid = True

				'valid = valid And Check(OrganisationsID.HasValue, "OrganisationID must have value")
				'valid = valid And Check(InseratID.HasValue, "InseratID must have value")

				'valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Beruf)), "Beruf must have a value")
				'valid = valid And Check(Not String.IsNullOrEmpty(TrimString(Text)), "(Vorspann / Anforderungen / Aufgaben / Tätigkeit) must have a value")


				'' Check Anstellugnsgrad 
				'If Anstellungsgrad.HasValue Then
				'	valid = valid And Check(Anstellungsgrad >= 1, "Anstellungsgrad must be >= 1")
				'	valid = valid And Check(Anstellungsgrad <= 100, "Anstellungsgrad must be <= 100")
				'End If

				'' Check Anstellugnsgrad_Bis 
				'If Anstellungsgrad_Bis.HasValue Then
				'	valid = valid And Check(Anstellungsgrad_Bis >= 1, "Anstellungsgrad_Bis must be >= 1")
				'	valid = valid And Check(Anstellungsgrad_Bis <= 100, "Anstellungsgrad_Bis must be <= 100")
				'End If

				'' Check  Anstellugnsgrad_Bis >= Anstellungsgrad
				'If Anstellungsgrad.HasValue AndAlso Anstellungsgrad_Bis.HasValue Then
				'	valid = valid And Check(Anstellungsgrad_Bis >= Anstellungsgrad, "Anstellugnsgrad_Bis must be >= Anstellungsgrad")
				'End If

				'' Check Sprache
				'If Not String.IsNullOrEmpty(Sprache) Then

				'	Dim lang = Sprache.Trim().ToLower()

				'	Select Case lang
				'		Case "de"
				'		Case "it"
				'		Case "fr"
				'			' ok
				'		Case Else
				'			valid = valid And Check(False, "Sprache must be in [de, it, fr]")
				'	End Select

				'End If

				'' Check RubrikID and Position
				'If Not String.IsNullOrEmpty(TrimString(RubrikID)) Or
				'	Not String.IsNullOrEmpty(TrimString(Position)) Then

				'	Dim tRubridId As String = NothingToEmptyString(TrimString(RubrikID))
				'	Dim tPoitionId As String = NothingToEmptyString(TrimString(Position))

				'	Dim count1 = tRubridId.Count(Function(c) c = ":")
				'	Dim count2 = tPoitionId.Count(Function(c) c = ":")

				'	valid = valid And Check(count1 = count2, "RubrikID and Position do not have same number of values.")

				'End If

				''Check Alter_Von
				'If Alter_Von.HasValue Then

				'	Select Case Alter_Von
				'		Case 0
				'		Case 25
				'		Case 35
				'		Case 45
				'			' ok
				'		Case Else
				'			valid = valid And Check(False, "Alter_Von must be in [0, 25, 35, 45]")
				'	End Select

				'End If

				''Check Alter_Bis
				'If Alter_Bis.HasValue Then

				'	Select Case Alter_Bis
				'		Case 0
				'		Case 24
				'		Case 34
				'		Case 44
				'		Case 65
				'		Case 100
				'			' ok
				'		Case Else
				'			valid = valid And Check(False, "Alter_Bis must be in [0, 24, 34, 44, 65, 100]")
				'	End Select

				'End If

				''Check Alter_Bis >= Alter_Von
				'If Alter_Von.HasValue AndAlso Alter_Bis.HasValue Then
				'	valid = valid And Check(Alter_Bis >= Alter_Von, "Alter_Bis must be >= Alter_Von")
				'End If

				'' Check Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau
				'If Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Kandidat)) Or
				'	Not String.IsNullOrEmpty(TrimString(Sprachkenntniss_Niveau)) Then

				'	Dim tSprachKentnis As String = NothingToEmptyString(TrimString(Sprachkenntniss_Kandidat))
				'	Dim tSprachNiveau As String = NothingToEmptyString(TrimString(Sprachkenntniss_Niveau))

				'	Dim count1 = tSprachKentnis.Count(Function(c) c = ":")
				'	Dim count2 = tSprachNiveau.Count(Function(c) c = ":")

				'	valid = valid And Check(count1 = count2, "Sprachkenntniss_Kandidat and Sprachkenntniss_Niveau do not have same number of values")
				'	valid = valid And Check(count1 <= 4, "To many Sprachkenntniss_Kandidat values (max 3 are allowed)")
				'	valid = valid And Check(count2 <= 4, "To many Sprachkenntniss_Niveau values (max 3 are allowed)")

				'End If

				'' Check Bildungsniveau
				'If Not String.IsNullOrEmpty(TrimString(Bildungsniveau)) Then

				'	Dim tBildungsNiveau As String = NothingToEmptyString(TrimString(Bildungsniveau))

				'	Dim count = tBildungsNiveau.Count(Function(c) c = ":")

				'	valid = valid And Check(count <= 4, "To many Bildungsniveau values (max 3 are allowed)")

				'End If

				'' Check Berufserfahrung and Berufserfahrung_Position
				'If Not String.IsNullOrEmpty(TrimString(Berufserfahrung)) Or
				'	Not String.IsNullOrEmpty(TrimString(Berufserfahrung_Position)) Then

				'	Dim tBerufserfahrung As String = NothingToEmptyString(TrimString(Berufserfahrung))
				'	Dim tBerufserfahrung_Position As String = NothingToEmptyString(TrimString(Berufserfahrung_Position))

				'	Dim count1 = tBerufserfahrung.Count(Function(c) c = ":")
				'	Dim count2 = tBerufserfahrung_Position.Count(Function(c) c = ":")

				'	valid = valid And Check(count1 = count2, "Berufserfahrung and Berufserfahrung_Position do not have same number of values")
				'	valid = valid And Check(count1 <= 3, "To many Berufserfahrung values (max 2 are allowed)")
				'	valid = valid And Check(count2 <= 3, "To many Berufserfahrung_Position values (max 2 are allowed)")

				'End If

				''Check Angebot
				'If Angebot.HasValue Then

				'	Select Case Angebot
				'Select Case Angebot
				'	Case 27 ' Classic Konto 
				'	Case 28 ' Classic Kontingent
				'	Case 29 ' Plus Konto 
				'	Case 30 ' Plus Kontingent
				'	Case 31 ' Premium Konto
				'	Case 32 ' Premium Kontingent 
				'	Case 35 ' Starter Konto 
				'	Case 36 ' Starter Kontingent 
				'	Case 37 ' Standard Konto 
				'	Case 38 ' Standard Kontingent 
				'	Case 39 ' Professional Konto 
				'	Case 40 ' Professional Kontingent 
				'	Case 41 ' Expert Konto 
				'	Case 42 ' Expert Kontingent 

				'	Case 0  ' empty value -> will not be written
				'		' ok
				'	Case Else
				'		valid = valid And Check(False, "Angebot must be in [0, 27, 28, 29, 30, 31, 32, 35, 36, 37, 38, 39, 40, 41]")
				'End Select

				'End If

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


	End Class

	<Serializable()>
	Public Class ProfilMatcherNotificationData
		Public Property ID As Integer
		Public Property CustomerID As String
		Public Property UserID As String
		Public Property QueryName As String
		Public Property QueryContent As String
		Public Property QueryResultContent As String
		Public Property Total As Integer
		Public Property CustomerNumber As Integer?
		Public Property EmployeeNumber As Integer?
		Public Property Notify As Boolean?
		Public Property CreatedFrom As String
		Public Property CreatedOn As DateTime?

	End Class


	<Serializable()>
	Public Class ProfilMatcherLocationData
		Public Property Location As String
		Public Property LocationDistances As Integer

	End Class


	<Serializable()>
	Public Class ProfilmatcherQueryResultData

		Public Property CustomerID As String
		Public Property Status As String
		Public Property Total As Integer
		Public Property Page As Integer
		Public Property Size As Integer
		Public Property ResultContent As String
		Public Property Jobs As List(Of ProfilmatcherQueryJob)

	End Class

	<Serializable()>
	Public Class ProfilmatcherQueryJob
		Public Property ID As Long
		Public Property Title As String
		Public Property Location As String
		Public Property Company As String
		Public Property URL As String
		Public Property DateCreated As DateTime

	End Class


	Public Class ProfilmatcherWebserviceResult

		Public Property HttpState As String
		Public Property APIResponse As String
		Public Property APIResult As String

	End Class


End Namespace