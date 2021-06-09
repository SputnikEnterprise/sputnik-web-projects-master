
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects


Namespace PVLInfo


	Partial Class PVLDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPVLDatabaseAccess


		Function LoadFLGAVGruppe0Info(ByVal customerID As String, ByVal canton As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe0ResultDTO) Implements IPVLDatabaseAccess.LoadFLGAVGruppe0Info
			Dim listOfSearchResultDTO As List(Of FLGAVGruppe0ResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Gruppe0 Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of FLGAVGruppe0ResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVGruppe0ResultDTO

						data.GAVNumber = SafeGetInteger(reader, "GAVNr", 0)
						data.Gruppe0Label = SafeGetString(reader, "Gruppe0")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVGruppe0Info"), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVGruppe0Info", .MessageContent = msgContent})
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

		Function LoadFLGAVGruppe1Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe1ResultDTO) Implements IPVLDatabaseAccess.LoadFLGAVGruppe1Info
			Dim listOfSearchResultDTO As List(Of FLGAVGruppe1ResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Gruppe1 Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", ReplaceMissing(gruppe0, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of FLGAVGruppe1ResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVGruppe1ResultDTO

						data.Gruppe1Label = SafeGetString(reader, "Gruppe1")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVGruppe1Info: {0}", gruppe0), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVGruppe1Info", .MessageContent = msgContent})
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

		Function LoadFLGAVGruppe2Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe2ResultDTO) Implements IPVLDatabaseAccess.LoadFLGAVGruppe2Info
			Dim listOfSearchResultDTO As List(Of FLGAVGruppe2ResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Gruppe2 Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", ReplaceMissing(gruppe0, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe1", ReplaceMissing(gruppe1, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of FLGAVGruppe2ResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVGruppe2ResultDTO

						data.Gruppe2Label = SafeGetString(reader, "Gruppe2")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVGruppe2Info"), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVGruppe2Info", .MessageContent = msgContent})
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

		Function LoadFLGAVGruppe3Info(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal language As String) As IEnumerable(Of FLGAVGruppe3ResultDTO) Implements IPVLDatabaseAccess.LoadFLGAVGruppe3Info
			Dim listOfSearchResultDTO As List(Of FLGAVGruppe3ResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Gruppe3 Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", ReplaceMissing(gruppe0, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe1", ReplaceMissing(gruppe1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe2", ReplaceMissing(gruppe2, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of FLGAVGruppe3ResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVGruppe3ResultDTO

						data.Gruppe3Label = SafeGetString(reader, "Gruppe3")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVGruppe3Info"), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVGruppe3Info", .MessageContent = msgContent})
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

		Function LoadFLGAVTextInfo(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal language As String) As IEnumerable(Of FLGAVTextResultDTO) Implements IPVLDatabaseAccess.LoadFLGAVTextInfo
			Dim listOfSearchResultDTO As List(Of FLGAVTextResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Text Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", ReplaceMissing(gruppe0, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe1", ReplaceMissing(gruppe1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe2", ReplaceMissing(gruppe2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe3", ReplaceMissing(gruppe3, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of FLGAVTextResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVTextResultDTO

						data.GAVLabel = SafeGetString(reader, "GAVText")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVTextInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVTextInfo", .MessageContent = msgContent})
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

		Function LoadFLGAVSalaryInfo(ByVal customerID As String, ByVal canton As String, ByVal gruppe0 As String, ByVal gruppe1 As String, ByVal gruppe2 As String, ByVal gruppe3 As String, ByVal gavtext As String, ByVal language As String) As FLGAVSalaryResultDTO Implements IPVLDatabaseAccess.LoadFLGAVSalaryInfo
			Dim listOfSearchResultDTO As FLGAVSalaryResultDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim selLanguage As String = GetMyLanguage(language)

			Dim sql As String
			sql = "[Get FL GAV Salary Data]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Gruppe0", ReplaceMissing(gruppe0, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe1", ReplaceMissing(gruppe1, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe2", ReplaceMissing(gruppe2, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Gruppe3", ReplaceMissing(gruppe3, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("GAVText", ReplaceMissing(gavtext, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New FLGAVSalaryResultDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New FLGAVSalaryResultDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.GAVNr = SafeGetInteger(reader, "GAVNr", 0)

						data.GavKanton = SafeGetString(reader, "GavKanton")
						data.Gruppe0 = SafeGetString(reader, "Gruppe0")
						data.Gruppe1 = SafeGetString(reader, "Gruppe1")
						data.Gruppe2 = SafeGetString(reader, "Gruppe2")
						data.Gruppe3 = SafeGetString(reader, "Gruppe3")
						data.GavText = SafeGetString(reader, "GavText")

						data.CalcFerien = SafeGetInteger(reader, "CalcFerien", 0)
						data.Calc13Lohn = SafeGetInteger(reader, "Calc13Lohn", 0)

						data.Minlohn = SafeGetDecimal(reader, "Minlohn", 0)
						data.FeiertagLohn = SafeGetDecimal(reader, "FeiertagLohn", 0)
						data.Feierbtr = SafeGetDecimal(reader, "Feierbtr", 0)
						data.FerienLohn = SafeGetDecimal(reader, "FerienLohn", 0)
						data.Ferienbtr = SafeGetDecimal(reader, "Ferienbtr", 0)
						data.Lohn13 = SafeGetDecimal(reader, "Lohn13", 0)
						data.Lohn13btr = SafeGetDecimal(reader, "Lohn13btr", 0)
						data.StdLohn = SafeGetDecimal(reader, "StdLohn", 0)
						data.Monatslohn = SafeGetDecimal(reader, "Monatslohn", 0)
						data.Mittagszulagen = SafeGetDecimal(reader, "Mittagszulagen", 0)

						data.FAG = SafeGetDecimal(reader, "FAG", 0)
						data.FAN = SafeGetDecimal(reader, "FAN", 0)
						data.WAG = SafeGetDecimal(reader, "WAG", 0)
						data.WAN = SafeGetDecimal(reader, "WAN", 0)
						data.VAG = SafeGetDecimal(reader, "VAG", 0)
						data.VAN = SafeGetDecimal(reader, "VAN", 0)

						data.FAG_S = SafeGetDecimal(reader, "FAG_S", 0)
						data.FAN_S = SafeGetDecimal(reader, "FAN_S", 0)
						data.WAG_S = SafeGetDecimal(reader, "WAG_S", 0)
						data.WAN_S = SafeGetDecimal(reader, "WAN_S", 0)
						data.VAG_S = SafeGetDecimal(reader, "VAG_S", 0)
						data.VAN_S = SafeGetDecimal(reader, "VAN_S", 0)
						data.FAG_M = SafeGetDecimal(reader, "FAG_M", 0)
						data.FAN_M = SafeGetDecimal(reader, "FAN_M", 0)
						data.WAG_M = SafeGetDecimal(reader, "WAG_M", 0)
						data.WAN_M = SafeGetDecimal(reader, "WAN_M", 0)
						data.VAG_M = SafeGetDecimal(reader, "VAG_M", 0)
						data.VAN_M = SafeGetDecimal(reader, "VAN_M", 0)
						data.FAG_J = SafeGetDecimal(reader, "FAG_J", 0)
						data.FAN_J = SafeGetDecimal(reader, "FAN_J", 0)
						data.WAG_J = SafeGetDecimal(reader, "WAG_J", 0)
						data.WAN_J = SafeGetDecimal(reader, "WAN_J", 0)
						data.VAG_J = SafeGetDecimal(reader, "VAG_J", 0)
						data.VAN_J = SafeGetDecimal(reader, "VAN_J", 0)

						data.GueltigAb = SafeGetDateTime(reader, "GueltigAb", Nothing)
						data.GueltigBis = SafeGetDateTime(reader, "GueltigBis", Nothing)
						data.ZusatzFeier = SafeGetString(reader, "ZusatzFeier")
						data.Zusatz13Lohn = SafeGetString(reader, "Zusatz13Lohn")

						data.Ferientext = SafeGetString(reader, "Ferientext")
						data.Lohn13text = SafeGetString(reader, "Lohn13text")

						data.StdWeek = SafeGetInteger(reader, "StdWeek", 0)
						data.StdMonth = SafeGetInteger(reader, "StdMonth", 0)
						data.StdYear = SafeGetInteger(reader, "StdYear", 0)

						data.F_Alter = SafeGetString(reader, "F_Alter")
						data.L_Alter = SafeGetString(reader, "L_Alter")

						data.Zusatz1 = SafeGetString(reader, "Zusatz1")
						data.Zusatz2 = SafeGetString(reader, "Zusatz2")
						data.Zusatz3 = SafeGetString(reader, "Zusatz3")
						data.Zusatz4 = SafeGetString(reader, "Zusatz4")
						data.Zusatz5 = SafeGetString(reader, "Zusatz5")
						data.Zusatz6 = SafeGetString(reader, "Zusatz6")
						data.Zusatz7 = SafeGetString(reader, "Zusatz7")
						data.Zusatz8 = SafeGetString(reader, "Zusatz8")
						data.Zusatz9 = SafeGetString(reader, "Zusatz9")
						data.Zusatz10 = SafeGetString(reader, "Zusatz10")
						data.Zusatz11 = SafeGetString(reader, "Zusatz11")
						data.Zusatz12 = SafeGetString(reader, "Zusatz12")


						listOfSearchResultDTO = data

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.IPVLDatabaseAccess",
																.NotifyComments = String.Format("LoadFLGAVSalaryInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.GAVFLDATA, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				msgContent &= String.Format(" | customerID: {0} | canton: {1} | gruppe0: {2} | gruppe1: {3} | gruppe2: {4} | gruppe3: {5} | gavtext: {6} | language: {7}", customerID, canton, gruppe0, gruppe1, gruppe2, gruppe3, gavtext, language)
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadFLGAVSalaryInfo", .MessageContent = msgContent})
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


		Private Function GetMyLanguage(ByVal language As String) As String
			Dim result As String

			Dim myLanguage As String = ReplaceMissing(language, "DE")
			Dim selLanguage As String = myLanguage
			Select Case myLanguage.ToLower().TrimEnd()
				Case "deutsch", "de", "d"
					selLanguage = "DE"
				Case "italienisch", "it", "i"
					selLanguage = "IT"
				Case "französisch", "fr", "f"
					selLanguage = "FR"
				Case "englisch", "en", "e"
					selLanguage = "EN"

				Case Else
					selLanguage = "DE"
			End Select
			result = selLanguage


			Return result

		End Function


	End Class

End Namespace
