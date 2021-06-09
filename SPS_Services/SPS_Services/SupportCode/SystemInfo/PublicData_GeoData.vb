
Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo


	Partial Class PublicDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPublicDatabaseAccess


		Function LoadGeoCoordinationData(ByVal customerID As String, ByVal countryCode As String, ByVal firstPostcode As String, ByVal secondPostcode As String) As IEnumerable(Of LocationGoordinateDataDTO) Implements IPublicDatabaseAccess.LoadGeoCoordinationData
			Dim listOfSearchResultDTO As List(Of LocationGoordinateDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Load Geo Coordination Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("firstPostcode", ReplaceMissing(firstPostcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("secondPostcode", ReplaceMissing(secondPostcode, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("countryCode", ReplaceMissing(countryCode, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of LocationGoordinateDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New LocationGoordinateDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.CountryCode = SafeGetString(reader, "CountryCode")
						data.Postcode = SafeGetString(reader, "Postcode")
						data.PlaceName = SafeGetString(reader, "PlaceName")
						data.AdminName1 = SafeGetString(reader, "AdminName1")
						data.AdminCode1 = SafeGetString(reader, "AdminCode1")
						data.AdminName2 = SafeGetString(reader, "AdminName2")
						data.AdminCode2 = SafeGetString(reader, "AdminCode2")
						data.AdminName3 = SafeGetString(reader, "AdminName3")
						data.AdminCode3 = SafeGetString(reader, "AdminCode3")
						data.Longitude = SafeGetDouble(reader, "Longitude", 0)
						data.Latitude = SafeGetDouble(reader, "Latitude", 0)
						data.Accuracy = SafeGetInteger(reader, "Accuracy", 0)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGeoCoordinationData", .MessageContent = msgContent})
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
