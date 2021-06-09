Imports System.Net
Imports System.Net.Http.Headers

Namespace JobPlatform.X28

	''' <summary>
	''' Represents a bunch of x28 profilmatcher.
	''' </summary>
	Public Class Profilmatcher

#Region "public properties"

		Public Property PMQueryData As ProfilmatcherQueryData


#End Region


#Region "Public Methods"

		Public Function GenerateVacanciesXml(ByVal profilematchdata As ProfilmatcherQueryData) As XDocument

			If profilematchdata Is Nothing Then Return Nothing

			Dim xDoc As XDocument = MapDbVacanciesToXML(profilematchdata)

			Return xDoc

		End Function

		Private Function MapDbVacanciesToXML(ByVal vacancyData As ProfilmatcherQueryData) As XDocument

			'Dim vacancies = New Profilmatcher

			' Map each db data to jobCh data structure.
			PMQueryData = vacancyData

			Dim xmlDoc = ToXDoc()

			If xmlDoc Is Nothing Then
				'Throw New Exception(String.Format("Convert to xml failed (CustomerGuid={0}). Errors={1}", m_CustomerGuid, vacancies.ValidationErrors))
			End If

			Return xmlDoc

		End Function

		Public Function ToXDoc() As XDocument

			Dim xDoc As New XDocument(
							 New XDeclaration("1.0", "utf-8", "true"))

			xDoc.Add(ToXElement())

			Return xDoc


			Return Nothing

		End Function


		Public Function ToXElement() As XElement

			If PMQueryData Is Nothing Then Return Nothing
			Dim request As New XElement("request")

			Dim result As New XElement("query")

			If Not PMQueryData.Terms Is Nothing AndAlso PMQueryData.Terms.Count > 0 Then
				Dim links = New XElement("terms")

				For Each itm In PMQueryData.Terms
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Companies Is Nothing AndAlso PMQueryData.Companies.Count > 0 Then
				Dim links = New XElement("companies")

				For Each itm In PMQueryData.Companies
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Companysizes Is Nothing AndAlso PMQueryData.Companysizes.Count > 0 Then
				Dim links = New XElement("companysizes")

				For Each itm In PMQueryData.Companysizes
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Industries Is Nothing AndAlso PMQueryData.Industries.Count > 0 Then
				Dim links = New XElement("industries")

				For Each itm In PMQueryData.Industries
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			result.Add(New XElement("recruitmentagencies", BooleanToString(PMQueryData.Recruitmentagencies)))
			result.Add(New XElement("management", BooleanToString(PMQueryData.Management)))
			result.Add(New XElement("temporary", BooleanToString(PMQueryData.Recruitmentagencies)))

			If Not PMQueryData.Regions Is Nothing AndAlso PMQueryData.Regions.Count > 0 Then
				Dim links = New XElement("regions")

				For Each itm In PMQueryData.Regions
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Locations Is Nothing AndAlso PMQueryData.Locations.Count > 0 Then
				Dim links = New XElement("locations")

				For Each itm In PMQueryData.Locations
					links.Add(New XElement("value", itm.Location))
					If itm.LocationDistances > 0 Then
						Dim dis = New XElement("distance")
						dis.Add(New XElement("value", itm.LocationDistances))

						links.Add(dis)
					End If
				Next

				result.Add(links)
			End If

			If Not PMQueryData.Clusters Is Nothing AndAlso PMQueryData.Clusters.Count > 0 Then
				Dim links = New XElement("clusters")

				For Each itm In PMQueryData.Clusters
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Experiences Is Nothing AndAlso PMQueryData.Experiences.Count > 0 Then
				Dim links = New XElement("experiences")

				For Each itm In PMQueryData.Experiences
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Educations Is Nothing AndAlso PMQueryData.Educations.Count > 0 Then
				Dim links = New XElement("educations")

				For Each itm In PMQueryData.Educations
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If Not PMQueryData.Skills Is Nothing AndAlso PMQueryData.Skills.Count > 0 Then
				Dim links = New XElement("skills")

				For Each itm In PMQueryData.Skills
					links.Add(New XElement("value", itm))
				Next
				result.Add(links)
			End If

			If PMQueryData.WorkquotaMinimum > 0 Then
				Dim links = New XElement("workquota")
				links.Add(New XElement("minimum", PMQueryData.WorkquotaMinimum))

				If PMQueryData.WorkquotaMaximum > 0 Then
					links.Add(New XElement("maximum", PMQueryData.WorkquotaMaximum))
				End If

				result.Add(links)
			End If


			request.Add(result)

			request.Add(New XElement("date",
												New XElement("from", PMQueryData.DateFrom.ToString("yyyy-MM-dd")),
												New XElement("to", PMQueryData.DateFrom.ToString("yyyy-MM-dd"))))
			request.Add(New XElement("page", PMQueryData.Page.ToString()))
			request.Add(New XElement("size", PMQueryData.Size.ToString()))


			Return request

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
				'm_ValidationErrors.Append(stringIfNotValid)
				'm_ValidationErrors.Append(";")
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


	Public Class ParsX28ProfilMatcherXMLData

		Private m_XMLfilename As String


		Public Sub New(ByVal xmlFilename As String)
			m_XMLfilename = xmlFilename
		End Sub

		Public Function LoadProfilMatcherResultData() As ProfilmatcherQueryResultData
			Dim result As ProfilmatcherQueryResultData

			Dim xelement As XElement = XElement.Load(m_XMLfilename)

			Dim m_NS As XNamespace = xelement.Name.Namespace
			result = CVPersonalInformation(xelement, m_NS)


			Return result

		End Function

		Private Function CVPersonalInformation(ByVal xEL As XElement, ByVal m_NS As XNamespace) As ProfilmatcherQueryResultData

			Dim respnseData = New ProfilmatcherQueryResultData

			Try

				respnseData.Status = GetSafeStringFromXElement(xEL.Element(m_NS + "status"))
				respnseData.Total = GetSafeIntegerFromXElement(xEL.Element(m_NS + "total"))
				respnseData.Page = GetSafeIntegerFromXElement(xEL.Element(m_NS + "page"))
				respnseData.Size = GetSafeIntegerFromXElement(xEL.Element(m_NS + "size"))


				Dim jobsData = From address In xEL.Elements(m_NS + "jobs")
											 Select address
				Dim jobs = From address In xEL.Elements(m_NS + "jobs").Elements(m_NS + "job")
									 Select address

				Dim resultJobs = New List(Of ProfilmatcherQueryJob)
				For Each itm As XElement In jobs
					Dim workData = New ProfilmatcherQueryJob

					workData.ID = GetSafeLongFromXElement(itm.Element(m_NS + "id"))
					workData.Title = GetSafeStringFromXElement(itm.Element(m_NS + "title"))
					workData.Location = GetSafeStringFromXElement(itm.Element(m_NS + "location"))
					workData.Company = GetSafeStringFromXElement(itm.Element(m_NS + "company"))
					workData.URL = GetSafeStringFromXElement(itm.Element(m_NS + "url"))
					workData.DateCreated = GetSafeDateFromXElement(itm.Element(m_NS + "date"))


					resultJobs.Add(workData)

				Next
				respnseData.Jobs = resultJobs


			Catch ex As Exception
				respnseData = Nothing

				Return respnseData
			End Try

			Return respnseData

		End Function


#Region "helpers"

		Private Function GetSafeStringFromXElement(ByVal xelment As XElement) As String

			If xelment Is Nothing Then
				Return String.Empty
			Else

				Return xelment.Value
			End If

		End Function

		Private Function GetSafeDateFromXElement(ByVal xelment As XElement) As Date?

			If xelment Is Nothing Then
				Return Nothing
			Else

				Return CDate(xelment.Value)
			End If

		End Function

		Private Function GetSafeBooleanFromXElement(ByVal xelment As XElement) As Boolean?

			If xelment Is Nothing Then
				Return Nothing
			Else

				Return CBool(xelment.Value)
			End If

		End Function

		Private Function GetSafeIntegerFromXElement(ByVal xelment As XElement) As Integer?

			If xelment Is Nothing Then
				Return Nothing
			Else

				Return CInt(xelment.Value)
			End If

		End Function

		Private Function GetSafeLongFromXElement(ByVal xelment As XElement) As Long?

			If xelment Is Nothing Then
				Return Nothing
			Else

				Return CLng(xelment.Value)
			End If

		End Function

		Private Function GetSafeByteFromXElement(ByVal xelment As XElement) As Byte()

			If xelment Is Nothing Then
				Return Nothing
			Else

				'Dim utf8 As Encoding = Encoding.UTF8()
				Dim bytes As Byte() = Convert.FromBase64String(xelment.Value) ' utf8.GetBytes(xelment.Value)


				Return (bytes)
			End If

		End Function

#End Region


	End Class



	Public Class ProfilmatcherUtility


		Private Const X28_RM_WEB_REQUEST_URL As String = "https://api.x28.ch/search/api/jobs/xml"

		Private Const X28_RM_API_USERNAME As String = "sputnikit"
		Private Const X28_RM_API_USERPASSWORD As String = "XhDJxv!hc*qW5hNBj9x4"

		Private httpStatus As String
		Private m_APIResponse As String
		Private m_APIResult As String


#Region "Public properties"

		Public Property PMQueryStringToSend As String

#End Region


#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		Public Sub New()

		End Sub


#End Region

		Public ReadOnly Property GetPMAPIResponseData() As String
			Get
				Return m_APIResponse
			End Get
		End Property

		Public ReadOnly Property GetPMAPIResultData() As String
			Get
				Return m_APIResult
			End Get
		End Property


		Public Function postProfilmatcherSearchAPI() As Boolean
			Dim baseUri As Uri = New Uri(X28_RM_WEB_REQUEST_URL)

			'Dim response As HttpWebResponse
			m_APIResponse = String.Empty
			m_APIResult = String.Empty

			'response = clsWebserviceProcess(PMQueryStringToSend, baseUri, "Post", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD)

			'Dim myStreamReader As New StreamReader(response.Content.ReadAsStreamAsync().Result)

			'Dim xmlToDeserialize As String = myStreamReader.ReadToEnd()

			'm_APIResponse = response.ToString
			'm_APIResult = xmlToDeserialize

			'' saving result into a file
			'Dim ostJobChRoot = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
			'Dim organsiationRootPath = System.IO.Path.Combine(ostJobChRoot, "00000")
			'Dim xmlFileName = System.IO.Path.Combine(organsiationRootPath, "pmQueryResult.xml")
			'Using sw As StreamWriter = New StreamWriter(xmlFileName, False, Encoding.UTF8)
			'	sw.Write(xmlToDeserialize)
			'	sw.Close()
			'End Using


			Return True

		End Function

		Private Function clsWebserviceProcess(ByVal sb As String) As HttpWebResponse
			Dim baseUri As Uri = New Uri(X28_RM_WEB_REQUEST_URL)

			'Dim response As HttpWebResponse
			m_APIResponse = String.Empty
			m_APIResult = String.Empty

			'response = clsWebserviceProcess(PMQueryStringToSend, baseUri, "Post", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD)

			Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue(
				"Basic",
				Convert.ToBase64String(
						System.Text.ASCIIEncoding.ASCII.GetBytes(
								String.Format("{0}:{1}", X28_RM_API_USERNAME, X28_RM_API_USERPASSWORD))))

			Dim xmlBytes() As Byte = System.Text.Encoding.UTF8.GetBytes(sb)

			Dim req As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create(baseUri), System.Net.HttpWebRequest)
			req.Method = "POST"
			req.ContentType = "application/xml"
			req.Headers.Add("Authorization", authHeader.ToString)

			req.ContentLength = xmlBytes.Length
			Dim post As System.IO.Stream = req.GetRequestStream
			post.Write(xmlBytes, 0, xmlBytes.Length)

			Dim result As String = Nothing
			Dim resp As System.Net.HttpWebResponse = CType(req.GetResponse, System.Net.HttpWebResponse)
			Dim reader As System.IO.StreamReader = New System.IO.StreamReader(resp.GetResponseStream)
			result = reader.ReadToEnd
			reader.Close()

			httpStatus = CStr(resp.StatusCode)

			m_APIResponse = httpStatus
			m_APIResult = result

			If (resp.StatusCode <> HttpStatusCode.OK AndAlso resp.StatusCode <> HttpStatusCode.NoContent AndAlso resp.StatusCode <> HttpStatusCode.Created) Then
				Return resp
			End If


			Return resp

		End Function


	End Class


End Namespace

