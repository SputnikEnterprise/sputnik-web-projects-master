
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.DocumentScan.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace DocumentScan


	Partial Class ScanDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IScanDatabaseAccess

		Function LoadNotificationEMailData(ByVal customerID As String) As EMailNotificationData Implements IScanDatabaseAccess.LoadNotificationEMailData
			Dim listOfSearchResultDTO As EMailNotificationData = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Notifications EMail Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New EMailNotificationData

			Try
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					listOfSearchResultDTO.ID = SafeGetInteger(reader, "ID", 0)
					listOfSearchResultDTO.Customer_ID = SafeGetString(reader, "Customer_ID")
					listOfSearchResultDTO.Customer_Name = SafeGetString(reader, "Customer_Name")
					listOfSearchResultDTO.BCCAddresses = SafeGetString(reader, "bccAddresses")
					listOfSearchResultDTO.MailPassword = SafeGetString(reader, "MailPassword")
					listOfSearchResultDTO.MailSender = SafeGetString(reader, "MailSender")
					listOfSearchResultDTO.MailUserName = SafeGetString(reader, "MailUserName")
					listOfSearchResultDTO.Recipients = SafeGetString(reader, "Recipients")
					listOfSearchResultDTO.Report_Recipients = SafeGetString(reader, "Report_Recipients")
					listOfSearchResultDTO.SmtpPort = SafeGetInteger(reader, "SmtpPort", 0)
					listOfSearchResultDTO.SmtpServer = SafeGetString(reader, "SmtpServer")
					listOfSearchResultDTO.ActivateSSL = SafeGetBoolean(reader, "ActivateSSL", False)
					listOfSearchResultDTO.TemplateFolder = SafeGetString(reader, "TemplateFolder")


				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadNotificationEMailData", .MessageContent = msgContent})
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
			Return listOfSearchResultDTO
		End Function


	End Class

End Namespace
