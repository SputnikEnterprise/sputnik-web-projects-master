

Imports System.Data.SqlClient
Imports wsSPS_Services.DatabaseAccessBase
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer

	Partial Class CVLizerDatabaseAccess
		Inherits DatabaseAccessBase

#Region "Document"

		Private Function LoadCVLDocumentPhaseData(ByVal profileID As Integer) As List(Of DocumentDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of DocumentDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Document Data For Notification]", conn)
				cmd.CommandType = CommandType.StoredProcedure

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", profileID))

				' Execute the data reader.
				cmd.Parameters.AddRange(listOfParams.ToArray())

				' Open connection to database.
				conn.Open()

				For i As Integer = 0 To cmd.Parameters.Count - 1
					strMessage.Append(String.Format("{0} ({1} {2}): {3}{4}",
																																										cmd.Parameters(i).ParameterName,
																																										cmd.Parameters(i).DbType,
																																										cmd.Parameters(i).Size,
																																										cmd.Parameters(i).Value,
																																										ControlChars.NewLine))
				Next

				listOfSearchResultDTO = New List(Of DocumentDataDTO)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New DocumentDataDTO With {
												.ID = utility.SafeGetInteger(reader, "ID", 0),
												.DocClass = utility.SafeGetString(reader, "DocClass"),
												.Pages = utility.SafeGetInteger(reader, "Pages", 0),
												.Plaintext = utility.SafeGetString(reader, "PlainText"),
												.FileType = utility.SafeGetString(reader, "FileType"),
												.DocBinary = utility.SafeGetByteArray(reader, "DocBinary"),
												.DocID = utility.SafeGetInteger(reader, "DocID", Nothing),
												.DocSize = utility.SafeGetInteger(reader, "DocSize", Nothing),
												.DocLanguage = utility.SafeGetString(reader, "DocLanguage")
										}


					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLDocumentPhaseData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try

			Return listOfSearchResultDTO
		End Function


#End Region


		Function LoadApplicantDocumentsFromCVLData(ByVal customerID As String, ByVal cvlPrifleID As Integer, ByVal showForApplicant As Boolean) As IEnumerable(Of DocumentViewDataDTO) Implements ICVLizerDatabaseAccess.LoadApplicantDocumentsFromCVLData
			Dim result As List(Of DocumentViewDataDTO) = Nothing
			m_customerID = customerID

			Dim sql As String
			If showForApplicant Then
				sql = "[Load Assigned Applicant Documents From CVL Data]"
			Else
				sql = "[Load ALL Assigned CVL Documents]"
			End If


			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of DocumentViewDataDTO)

					While reader.Read
						Dim data = New DocumentViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.DocClass = SafeGetString(reader, "DocClass")
						data.Pages = SafeGetInteger(reader, "Pages", Nothing)
						data.Plaintext = SafeGetString(reader, "Plaintext")
						data.FileType = SafeGetString(reader, "FileType")
						data.DocID = SafeGetInteger(reader, "DocID", Nothing)
						data.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						data.DocLanguage = SafeGetString(reader, "DocLanguage")
						data.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						data.DocXML = SafeGetString(reader, "DocXML")
						data.DocBinary = SafeGetByteArray(reader, "DocBinary")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadApplicantDocumentsFromCVLData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadCVLApplicantPictureData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As DocumentViewDataDTO Implements ICVLizerDatabaseAccess.LoadCVLApplicantPictureData
			Dim result As DocumentViewDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Applicant Picture From CVL Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New DocumentViewDataDTO

				If reader IsNot Nothing AndAlso reader.Read() Then

					result.ID = SafeGetInteger(reader, "ID", Nothing)
					result.DocClass = SafeGetString(reader, "DocClass")
					result.Pages = SafeGetInteger(reader, "Pages", Nothing)
					result.Plaintext = SafeGetString(reader, "Plaintext")
					result.FileType = SafeGetString(reader, "FileType")
					result.DocID = SafeGetInteger(reader, "DocID", Nothing)
					result.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
					result.DocLanguage = SafeGetString(reader, "DocLanguage")
					result.FileHashvalue = SafeGetString(reader, "FileHashvalue")
					result.DocXML = SafeGetString(reader, "DocXML")
					result.DocBinary = SafeGetByteArray(reader, "DocBinary")

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLApplicantPictureData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadAssignedApplicationEMailData(ByVal customerID As String, ByVal applicationNumber As Integer, ByVal withAttachments As Boolean?) As EMailDataDTO Implements ICVLizerDatabaseAccess.LoadAssignedApplicationEMailData
			Dim result As EMailDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load EMail Data For Assigned Application]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("applicationID", ReplaceMissing(applicationNumber, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New EMailDataDTO

				If reader IsNot Nothing AndAlso reader.Read() Then

					result.ID = SafeGetInteger(reader, "ID", Nothing)
					result.Customer_ID = SafeGetString(reader, "Customer_ID")
					result.EMailSubject = SafeGetString(reader, "EMailSubject")
					result.EMailUidl = SafeGetInteger(reader, "EMailUidl", 0)
					result.EMailFrom = SafeGetString(reader, "EMailFrom")
					result.EMailTo = SafeGetString(reader, "EMailTo")
					result.HasHtmlBody = SafeGetBoolean(reader, "HasHtmlBody", False)
					result.EMailPlainTextBody = SafeGetString(reader, "EMailPlainTextBody")
					result.EMailBody = SafeGetString(reader, "EMailBody")
					result.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
					result.CreatedFrom = SafeGetString(reader, "CreatedFrom")
					result.EMailMime = SafeGetString(reader, "EMailMime")
					result.ApplicationID = SafeGetInteger(reader, "ApplicationId", Nothing)
					result.EMailContent = SafeGetByteArray(reader, "Content")

					If withAttachments.GetValueOrDefault(False) Then
						result.EMailAttachment = LoadAssignedEMailAttachmentData(result.ID)
					End If

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedApplicationEMailData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Private Function LoadAssignedEMailAttachmentData(ByVal emailID As Integer) As IEnumerable(Of EMailAttachment)
			Dim result As List(Of EMailAttachment) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql = "[Load EMail Attatchment Data For Assigned EMail]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("eMailID", ReplaceMissing(emailID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of EMailAttachment)

					While reader.Read
						Dim data = New EMailAttachment

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.FK_REID = SafeGetInteger(reader, "FK_REID", Nothing)
						data.DocumentCategoryNumber = SafeGetInteger(reader, "DocumentCategoryNumber", Nothing)
						data.AttachmentName = SafeGetString(reader, "AttachmentFileName")
						data.AttachmentSize = SafeGetByteArray(reader, "ScanContent")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedEMailAttachmentData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function

		Function LoadAssignedDocumentData(ByVal customerID As String, ByVal id As Integer) As DocumentViewDataDTO Implements ICVLizerDatabaseAccess.LoadAssignedDocumentData
			Dim result As DocumentViewDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load Assigned Document Data]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("@ID", m_utility.ReplaceMissing(id, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				result = New DocumentViewDataDTO

				If reader IsNot Nothing AndAlso reader.Read() Then

					result.ID = m_utility.SafeGetInteger(reader, "ID", Nothing)
					result.DocClass = m_utility.SafeGetString(reader, "DocClass")
					result.Pages = m_utility.SafeGetInteger(reader, "Pages", Nothing)
					result.Plaintext = m_utility.SafeGetString(reader, "Plaintext")
					result.FileType = m_utility.SafeGetString(reader, "FileType")
					result.DocBinary = m_utility.SafeGetByteArray(reader, "DocBinary")
					result.DocID = m_utility.SafeGetInteger(reader, "DocID", Nothing)
					result.DocSize = m_utility.SafeGetInteger(reader, "DocSize", Nothing)
					result.DocLanguage = m_utility.SafeGetString(reader, "DocLanguage")
					result.FileHashvalue = m_utility.SafeGetString(reader, "FileHashvalue")
					result.DocXML = m_utility.SafeGetString(reader, "DocXML")

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedDocumentData", .MessageContent = msgContent})
				result = Nothing
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function





		Function LoadCVLDocumentData(ByVal customerID As String, ByVal cvlPrifleID As Integer) As IEnumerable(Of DocumentViewDataDTO) Implements ICVLizerDatabaseAccess.LoadCVLDocumentData
			Dim result As List(Of DocumentViewDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql = "[Load ALL Assigned CVL Documents]"

			Dim listOfParams = New List(Of SqlClient.SqlParameter)
			listOfParams.Add(New SqlClient.SqlParameter("CVLProfileID", ReplaceMissing(cvlPrifleID, DBNull.Value)))

			Dim reader As SqlClient.SqlDataReader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try

				If reader IsNot Nothing Then

					result = New List(Of DocumentViewDataDTO)

					While reader.Read
						Dim data = New DocumentViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.DocClass = SafeGetString(reader, "DocClass")
						data.Pages = SafeGetInteger(reader, "Pages", Nothing)
						data.Plaintext = SafeGetString(reader, "Plaintext")
						data.FileType = SafeGetString(reader, "FileType")
						data.DocID = SafeGetInteger(reader, "DocID", Nothing)
						data.DocSize = SafeGetInteger(reader, "DocSize", Nothing)
						data.DocLanguage = SafeGetString(reader, "DocLanguage")
						data.FileHashvalue = SafeGetString(reader, "FileHashvalue")
						data.DocXML = SafeGetString(reader, "DocXML")


						result.Add(data)

					End While

				End If

			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLDocumentData", .MessageContent = msgContent})
			Finally
				If Not reader Is Nothing Then
					Try
						reader.Close()
					Catch
						' Do nothing
					End Try

				End If
			End Try


			Return result
		End Function



	End Class

End Namespace
