

'Imports System.Net
'Imports System.Net.Http
'Imports System.Net.Http.Headers
'Imports System.Text
'Imports System.Threading
'Imports System.Threading.Tasks

'Namespace JobPlatform


'	Partial Class VacancyUtilities


'		Public Async Function webserviceResponse(ByVal sb As StringBuilder, ByVal baseUri As Uri, ByVal Method As String, ByVal User As String, ByVal Password As String) As Task(Of HttpResponseMessage)

'			Dim client As HttpClient = New HttpClient()
'			client.BaseAddress = baseUri

'			If String.IsNullOrEmpty(User) Then
'				Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("None")
'				client.DefaultRequestHeaders.Authorization = authHeader

'			Else

'				Dim authHeader As AuthenticationHeaderValue = New AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(String.Format("{0}:{1}", User, Password))))
'				client.DefaultRequestHeaders.Authorization = authHeader

'			End If

'			Dim timeout As TimeSpan = TimeSpan.FromMinutes(5)
'			client.Timeout = timeout

'			Dim content As New StringContent(sb.ToString, System.Text.Encoding.UTF8, "application/json")

'			Dim resp As HttpResponseMessage = Nothing
'			Dim cancellationToken As CancellationToken

'			If Method = "Post" Then
'				resp = Await client.PostAsync(baseUri, content, cancellationToken)

'			ElseIf Method = "Put" Then
'				resp = Await client.PutAsync(baseUri, content, cancellationToken)

'			ElseIf Method = "Delete" Then
'				resp = Await client.DeleteAsync(baseUri, cancellationToken)

'			ElseIf Method = "Get" Then
'				resp = Await client.GetAsync(baseUri, cancellationToken)

'			End If
'			If resp.StatusCode = HttpStatusCode.BadRequest Then Return Nothing


'			Return resp
'		End Function


'	End Class


'End Namespace
