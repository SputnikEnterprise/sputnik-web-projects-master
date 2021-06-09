
Imports System.Web.Services
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports System.IO


Partial Class SPNotification
	Inherits System.Web.Services.WebService

	<WebMethod(Description:="load mailnotification data")>
	Function LoadMailNotificationbyDateData(ByVal customerID As String, ByVal assignedDate As DateTime?) As EMailNotificationDTO()
		Dim result As List(Of EMailNotificationDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of EMailNotificationDTO)
			result = m_WOS.LoadMailNotificationData(customerID, assignedDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMailNotificationbyDateData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load advisor login data")>
	Function LoadAdvisorLoginByDateData(ByVal customerID As String, ByVal assignedDate As DateTime?) As AdvisorLoginData()
		Dim result As List(Of AdvisorLoginData) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of AdvisorLoginData)
			result = m_SysInfo.LoadAdvisorLoginData(customerID, assignedDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvisorLoginByDateData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

	<WebMethod(Description:="load advisor login data monthly")>
	Function LoadAdvisorMonthlyLoginByDateData(ByVal customerID As String, ByVal assignedDate As DateTime?) As AdvisorLoginData()
		Dim result As List(Of AdvisorLoginData) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of AdvisorLoginData)
			result = m_SysInfo.LoadAdvisorLoginMonthlyData(customerID, assignedDate)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAdvisorMonthlyLoginByDateData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function


End Class