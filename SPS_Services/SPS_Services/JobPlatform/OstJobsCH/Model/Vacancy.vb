Namespace JobPlatform.OstJobsCH

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

		Public Property CustomerGuid As String
		Public Property UserGuid As String
		Public Property VakNr As Integer
		Public Property JobVersion As Char
		Public Property Company As String
		Public Property Title As String
		Public Property WorkplaceCountry As String
		Public Property WorkplaceZip As String
		Public Property WorkplaceCity As String
		Public Property CompanyDescription As String
		Public Property WirBieten As String
		Public Property Anforderungen As String
		Public Property Aufgabe As String
		Public Property Contact As String
		Public Property DescriptionUrl As String
		Public Property ApplicationUrl As String
		Public Property PublicationOstjobCh As Boolean
		Public Property PublicationWestjobAt As Boolean
		Public Property PublicationNicejobDe As Boolean
		Public Property PublicationZentraljobCh As Boolean
		Public Property PublicationMinisite As Boolean
		Public Property Apprenticeship As Boolean
		Public Property Template As String
		Public Property Keywords As String
		Public Property CreatedOn As DateTime
		Public Property IsOnline As Boolean

		Public ReadOnly Property AnforderungenCombinedWithAufgabe
			Get
				Return String.Format("{0}<br/>{1}", Aufgabe, Anforderungen)
			End Get
		End Property

		''' <summary>
		''' Gets boolean flag indicating if the data is valid for xml export.
		''' </summary>
		Public ReadOnly Property IsDataValidForXml As Boolean
			Get
				m_ValidationErrors.Clear()

				Dim valid = True

				valid = valid And Check(VakNr <> 0, "VakNr (id) must have value")
				valid = valid And Check(Not String.IsNullOrWhiteSpace(Title), "Title must have value")
				valid = valid And Check(Not String.IsNullOrWhiteSpace(WorkplaceCity), "WorkplaceCity must have value")

				Dim hasPublication As Boolean = PublicationOstjobCh Or
			  PublicationWestjobAt Or
			  PublicationNicejobDe Or
			  PublicationZentraljobCh Or
			  PublicationMinisite

				valid = valid And Check(hasPublication, "At least one publication must have the value 1")

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

				Dim vacancy As New XElement("vacancy")

				'vacancy.Add(New XElement("id", "REF-" + (Environment.TickCount + VakNr).ToString())) ' VakNr.ToString()))
				vacancy.Add(New XElement("id", "REF-" + (VakNr).ToString()))
				vacancy.Add(New XElement("company", LimitStringLength(TrimString(Company), 200)))
				vacancy.Add(New XElement("title", LimitStringLength(TrimString(Title), 200)))
				vacancy.Add(New XElement("workplace",
							New XElement("country", TrimString(WorkplaceCountry)),
							New XElement("zip", TrimString(WorkplaceZip)),
							New XElement("city", TrimString(WorkplaceCity))
			))
				vacancy.Add(New XElement("company_description", WrapStringInCData(TrimString(CompanyDescription))))
				vacancy.Add(New XElement("description", WrapStringInCData(TrimString(AnforderungenCombinedWithAufgabe))))
				vacancy.Add(New XElement("requirements", WrapStringInCData(TrimString(WirBieten))))

				'vacancy.Add(New XElement("description", WrapStringInCData(TrimString(WirBieten))))
				'vacancy.Add(New XElement("requirements", WrapStringInCData(TrimString(AnforderungenCombinedWithAufgabe))))

				vacancy.Add(New XElement("contact", TrimString(Contact)))

				Dim links = New XElement("links")
				If Not String.IsNullOrWhiteSpace(DescriptionUrl) Then
					links.Add(New XElement("description_url", TrimString(DescriptionUrl)))
				End If
				If Not String.IsNullOrWhiteSpace(ApplicationUrl) Then
					links.Add(New XElement("application_url", TrimString(ApplicationUrl)))
				End If
				vacancy.Add(links)

				vacancy.Add(New XElement("publication",
							 New XElement("ostjob_ch", BooleanToString(PublicationOstjobCh)),
							 New XElement("westjob_at", BooleanToString(PublicationWestjobAt)),
							 New XElement("nicejob_de", BooleanToString(PublicationNicejobDe)),
							 New XElement("zentraljob_ch", BooleanToString(PublicationZentraljobCh)),
							 New XElement("minisite", BooleanToString(PublicationMinisite))
			))

				If Apprenticeship Then
					vacancy.Add(New XElement("apprenticeship", BooleanToString(Apprenticeship)))
				End If

				If Not String.IsNullOrWhiteSpace(Template) Then
					vacancy.Add(New XElement("template", BooleanToString(Template)))
				End If
				If Not String.IsNullOrWhiteSpace(Keywords) Then
					vacancy.Add(New XElement("keywords", TrimString(Keywords)))
				End If

				Return vacancy
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

		Private Function BooleanToString(ByVal boolValue As Boolean?) As String
			If boolValue Is Nothing Then
				Return "0"
			End If
			Return IIf(boolValue, "1", "0")
		End Function

#End Region


	End Class


End Namespace

