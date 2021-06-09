

Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.CVLizer.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace CVLizer


	Partial Class CVLizerDatabaseAccess

#Region "Statistic"

		Private Function LoadCVLStatisticPhaseData(ByVal profileID As Integer) As List(Of CVCodeSummaryDataDTO)
			Dim connString As String = My.Settings.ConnStr_Applicant
			Dim listOfSearchResultDTO As List(Of CVCodeSummaryDataDTO) = Nothing
			Dim conn As SqlConnection = Nothing
			Dim strMessage As New StringBuilder()
			Dim utility As New ClsUtilities
			Dim reader As SqlDataReader = Nothing

			Try
				' Create command.
				conn = New SqlConnection(connString)

				Dim cmd As System.Data.SqlClient.SqlCommand = New System.Data.SqlClient.SqlCommand("[Load Assigned CVL Statistic Data For Notification]", conn)
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

				listOfSearchResultDTO = New List(Of CVCodeSummaryDataDTO)
				reader = cmd.ExecuteReader

				' Read all data.
				While (reader.Read())

					Dim dto As New CVCodeSummaryDataDTO With {
											.ID = utility.SafeGetInteger(reader, "ID", 0),
											.Code = utility.SafeGetString(reader, "Code"),
											.Name = utility.SafeGetString(reader, "Name"),
											.Weight = utility.SafeGetDecimal(reader, "Weight", Nothing),
											.Duration = utility.SafeGetInteger(reader, "Duration", Nothing),
											.Domain = utility.SafeGetString(reader, "Domain")
									}


					listOfSearchResultDTO.Add(dto)

				End While

			Catch ex As Exception
				Dim msgContent = ex.ToString & vbNewLine & strMessage.ToString
				utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCVLStatisticPhaseData", .MessageContent = msgContent})
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


	End Class

End Namespace