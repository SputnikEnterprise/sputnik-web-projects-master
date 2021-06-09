
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects


Namespace PVLInfo


	Partial Class PVLDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPVLDatabaseAccess

		Function LoadArchivePVLDatabases(ByVal customerID As String) As IEnumerable(Of GAVPVLArchiveDatabaseDTO) Implements IPVLDatabaseAccess.LoadArchivePVLDatabases
			Dim listOfSearchResultDTO As List(Of GAVPVLArchiveDatabaseDTO) = Nothing
			m_customerID = customerID

			Dim sql As String
				sql = "SELECT [ID], [DbName], [DbConnstring]"
			sql &= "FROM Dbo.[tbl_PVLArchiveDatabases] "
			sql &= "Order By ID"

			Dim reader = OpenReader(sql, Nothing, CommandType.Text)

			listOfSearchResultDTO = New List(Of GAVPVLArchiveDatabaseDTO)

				Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVPVLArchiveDatabaseDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.DbName = SafeGetString(reader, "DbName")
						data.DbConnstring = SafeGetString(reader, "DbConnstring")

						listOfSearchResultDTO.Add(data)

					End While
				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadArchivePVLDatabases", .MessageContent = msgContent})

				Return Nothing
			End Try


			Return listOfSearchResultDTO

		End Function

		Function LoadCurrentMetaInfo(ByVal customerID As String, ByVal canton As String, ByVal contractNumber As Integer) As GAVNameResultDTO Implements IPVLDatabaseAccess.LoadCurrentMetaInfo
			Dim listOfSearchResultDTO As GAVNameResultDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Load Parifond Hour Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("gavnumber", ReplaceMissing(contractNumber, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(canton, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New GAVNameResultDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVNameResultDTO

						data.id_meta = SafeGetInteger(reader, "id_meta", 0)
						data.gav_number = SafeGetInteger(reader, "gav_number", 0)
						data.stdweek = SafeGetDecimal(reader, "stdweek", 0)
						data.stdmonth = SafeGetDecimal(reader, "stdmonth", 0)
						data.stdyear = SafeGetDecimal(reader, "stdyear", 0)
						data.fag = SafeGetDecimal(reader, "fag", 0)
						data.fan = SafeGetDecimal(reader, "fan", 0)
						data.resor_fan = SafeGetDecimal(reader, "Resor_fan", 0)
						data.resor_fag = SafeGetDecimal(reader, "Resor_fag", 0)
						data.van = SafeGetDecimal(reader, "van", 0)
						data.vag = SafeGetDecimal(reader, "vag", 0)
						data.wan = SafeGetDecimal(reader, "wan", 0)
						data.wag = SafeGetDecimal(reader, "wag", 0)

						data.old_fag = SafeGetDecimal(reader, "_FAG", 0)
						data.old_fan = SafeGetDecimal(reader, "_FAN", 0)
						data.old_wag = SafeGetDecimal(reader, "_WAG", 0)
						data.old_wan = SafeGetDecimal(reader, "_WAN", 0)
						data.old_WAG_s = SafeGetDecimal(reader, "_WAG_s", 0)
						data.old_WAN_s = SafeGetDecimal(reader, "_WAN_s", 0)
						data.old_WAG_J = SafeGetDecimal(reader, "_WAG_J", 0)
						data.old_WAN_J = SafeGetDecimal(reader, "_WAN_J", 0)
						data.old_vag = SafeGetDecimal(reader, "_VAG", 0)
						data.old_van = SafeGetDecimal(reader, "_VAN", 0)
						data.old_VAG_s = SafeGetDecimal(reader, "_VAG_s", 0)
						data.old_VAN_s = SafeGetDecimal(reader, "_VAN_s", 0)
						data.old_VAG_J = SafeGetDecimal(reader, "_VAG_J", 0)
						data.old_VAN_J = SafeGetDecimal(reader, "_VAN_J", 0)

						data.GAVKanton = SafeGetString(reader, "GAVKanton")


						listOfSearchResultDTO = data

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadCurrentMetaInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCurrentMetaInfo", .MessageContent = msgContent})
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


		Function LoadGAVVersionChangingData(ByVal customerID As String, ByVal gavNumber As Integer) As GAVVersionDataDTO Implements IPVLDatabaseAccess.LoadGAVVersionChangingData
			Dim listOfSearchResultDTO As GAVVersionDataDTO = Nothing
			m_customerID = customerID

			Try

				Dim sql As String
				sql = "SELECT TOP 1 [ID], [GAVNumber], [GAVDate], [GAVInfo], [schema_version] "
				sql &= "FROM [spPVLPublicData].[Dbo].GAV_Versions "
				sql &= "WHERE GAVNumber = @gavNumber "


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("gavNumber", ReplaceMissing(gavNumber, 0)))

				Dim reader = OpenReader(sql, listOfParams, CommandType.Text)

				Dim versionDataDTO = New GAVVersionDataDTO()
				If (Not reader Is Nothing AndAlso reader.Read()) Then
					versionDataDTO.ID = SafeGetInteger(reader, "ID", 0)
					versionDataDTO.GAVNumber = SafeGetInteger(reader, "GAVNumber", 0)
					versionDataDTO.GAVDate = SafeGetDateTime(reader, "GAVDate", Nothing)
					versionDataDTO.GAVInfo = SafeGetString(reader, "GAVInfo")
					versionDataDTO.schema_version = SafeGetString(reader, "schema_version")

				Else
					' No version data could be found. In this case also return an object with GAVNumber only.
					'versionDataDTO = New GAVVersionDataDTO() With {.GAVNumber = gavNumber}
					versionDataDTO.GAVNumber = gavNumber
				End If
				listOfSearchResultDTO = versionDataDTO

				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.PVLDatabaseAccess",
																.NotifyComments = String.Format("LoadGAVVersionChangingData: {0}", gavNumber), .NotifyArt = SPUtilities.NotifyArtEnum.PVLVERSIONCHECK, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadGAVVersionChangingData", .MessageContent = msgContent})

				Return Nothing
			End Try


			Return listOfSearchResultDTO

		End Function

		Function LoadPVLAddressData(ByVal customerID As String, ByVal gavnumber As Integer?, ByVal canton As String, ByVal gruppe0 As String, ByVal organ As String) As GAVAddressDataDTO Implements IPVLDatabaseAccess.LoadPVLAddressData
			Dim listOfSearchResultDTO As GAVAddressDataDTO = Nothing
			m_customerID = customerID

			Try

				Dim sql As String
				sql = "[Get PVL Address Data]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("gavNumber", ReplaceMissing(gavnumber, 0)))
				listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(canton, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("BerufBez", ReplaceMissing(gruppe0, String.Empty)))
				listOfParams.Add(New SqlClient.SqlParameter("Organ", ReplaceMissing(organ, String.Empty)))

				Dim reader = OpenReader(sql, listOfParams, CommandType.Text)

				Dim versionDataDTO = New GAVAddressDataDTO()
				If (Not reader Is Nothing AndAlso reader.Read()) Then

					versionDataDTO.ID = SafeGetInteger(reader, "ID", 0)
					versionDataDTO.GAVNumber = SafeGetInteger(reader, "GAVNumber", 0)
					versionDataDTO.BerufBez = SafeGetString(reader, "BerufBez")
					versionDataDTO.GAV_Name = SafeGetString(reader, "GAV_Name")
					versionDataDTO.GAV_ZHD = SafeGetString(reader, "GAV_ZHD")
					versionDataDTO.GAV_Postfach = SafeGetString(reader, "GAV_Postfach")
					versionDataDTO.GAV_Strasse = SafeGetString(reader, "GAV_Strasse")
					versionDataDTO.GAV_PLZ = SafeGetString(reader, "GAV_PLZ")
					versionDataDTO.GAV_Ort = SafeGetString(reader, "GAV_Ort")
					versionDataDTO.GAV_AdressNr = SafeGetString(reader, "GAV_AdressNr")
					versionDataDTO.GAV_Bank = SafeGetString(reader, "GAV_Bank")
					versionDataDTO.GAV_BankPLZOrt = SafeGetString(reader, "GAV_BankPLZOrt")
					versionDataDTO.GAV_Bankkonto = SafeGetString(reader, "GAV_Bankkonto")
					versionDataDTO.GAV_IBAN = SafeGetString(reader, "GAV_IBAN")
					versionDataDTO.Kanton = SafeGetString(reader, "Kanton")
					versionDataDTO.Organ = SafeGetString(reader, "Organ")


					listOfSearchResultDTO = versionDataDTO

				End If

				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.PVLDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLAddressData: gavnumber: {0} > canton: {1} > gruppe0: {2} > organ: {3}", gavnumber, canton, gruppe0, organ),
																.NotifyArt = SPUtilities.NotifyArtEnum.PVLADDRESS, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLAddressData", .MessageContent = msgContent})

				Return Nothing
			End Try


			Return listOfSearchResultDTO

		End Function

		Function LoadAssignedAdvisorPublicationInfoData(ByVal customerID As String, ByVal userID As String) As IEnumerable(Of PVLPublicationViewDataDTO) Implements IPVLDatabaseAccess.LoadAssignedAdvisorPublicationInfoData
			Dim listOfSearchResultDTO As List(Of PVLPublicationViewDataDTO) = Nothing
			m_customerID = customerID

			Try

				Dim sql As String
				sql = "[List PVL Assigned Advisor Publication Data]"


				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))


				Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

				listOfSearchResultDTO = New List(Of PVLPublicationViewDataDTO)
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PVLPublicationViewDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.User_ID = SafeGetString(reader, "User_ID")
						data.ContractNumber = SafeGetString(reader, "ContractNumber")
						data.VersionNumber = SafeGetInteger(reader, "VersionNumber", 0)
						data.PublicationDate = SafeGetDateTime(reader, "PublicationDate", Nothing)
						data.Title = SafeGetString(reader, "Title")
						data.Content = SafeGetString(reader, "Content")
						data.Viewed = SafeGetBoolean(reader, "Viewed", False)
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")


						listOfSearchResultDTO.Add(data)

					End While
				End If

				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.LoadAssignedAdvisorPublicationInfoData",
																.NotifyComments = String.Format("LoadAssignedAdvisorPublicationInfoData: customerID: {0} > userID: {1} | recordcount: {2}", customerID, userID, listOfSearchResultDTO.Count),
																.NotifyArt = SPUtilities.NotifyArtEnum.PVLADDRESS, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString

				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedAdvisorPublicationInfoData", .MessageContent = msgContent})

				Return Nothing
			End Try


			Return listOfSearchResultDTO

		End Function

		Function UpdateAssignedAdvisorPublicationViewedData(ByVal customerID As String, ByVal userID As String, ByVal recID As Integer, ByVal ContractNumber As String, ByVal VersionNumber As Integer,
															ByVal PublicationDate As DateTime?, ByVal Title As String, ByVal checked As Boolean?,
															ByVal userData As String) As Boolean Implements IPVLDatabaseAccess.UpdateAssignedAdvisorPublicationViewedData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql As String
			sql = "[Update Assigned Advisor Publication View Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))

			listOfParams.Add(New SqlClient.SqlParameter("ContractNumber", ReplaceMissing(ContractNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("VersionNumber", ReplaceMissing(VersionNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("PublicationDate", ReplaceMissing(PublicationDate, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("Title", ReplaceMissing(Title, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("checked", ReplaceMissing(checked, False)))

			listOfParams.Add(New SqlClient.SqlParameter("UserData", ReplaceMissing(userData, DBNull.Value)))


			success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Return success
		End Function


	End Class


End Namespace
