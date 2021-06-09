Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.DataTransferObjects.TaxInfoService
Imports wsSPS_Services.SPUtilities


Namespace SystemInfo


	Partial Class PublicDataDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IPublicDatabaseAccess


		Function LoadTaxInfoData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As IEnumerable(Of TaxDataItemDTO) Implements IPublicDatabaseAccess.LoadTaxInfoData
			Dim listOfSearchResultDTO As List(Of TaxDataItemDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = String.Format("SELECT Kanton, Gruppe, Kinder, Kirchensteuer FROM Tar{0}{1} ", year, canton)
			'If year >= 2021 Then sql &= "Where RecordArt = 6 "
			sql &= "GROUP BY Kanton, Gruppe, Kinder, Kirchensteuer "
			sql &= "ORDER BY Kanton, Gruppe, Kinder, Kirchensteuer"

			Dim reader = OpenReader(sql, Nothing, CommandType.Text)
			listOfSearchResultDTO = New List(Of TaxDataItemDTO)

			Try
				If reader IsNot Nothing Then

					'If year >= 2021 Then
					'	Dim data = New TaxDataItemDTO

					'	data.Kanton = canton
					'	data.Gruppe = "0"
					'	data.Kinder = 0
					'	data.Kirchensteuer = ""

					'	listOfSearchResultDTO.Add(data)
					'End If


					While reader.Read
							Dim data = New TaxDataItemDTO

						data.Kanton = SafeGetString(reader, "Kanton")
						data.Gruppe = SafeGetString(reader, "Gruppe")
						data.Kinder = SafeGetShort(reader, "Kinder", Nothing)
						data.Kirchensteuer = SafeGetString(reader, "Kirchensteuer")
						If String.IsNullOrEmpty(data.Gruppe) Then data.Gruppe = "0"


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxInfoData", .MessageContent = msgContent})
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

		Function LoadTaxCodeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TaxCodeDataDTO) Implements IPublicDatabaseAccess.LoadTaxCodeData
			Dim listOfSearchResultDTO As List(Of TaxCodeDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String

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

			Sql = String.Format("[Load Tax Code Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(Sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of TaxCodeDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New TaxCodeDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxCodeData", .MessageContent = msgContent})
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

		Function LoadTaxChurchCodeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TaxChurchCodeDataDTO) Implements IPublicDatabaseAccess.LoadTaxChurchCodeData
			Dim listOfSearchResultDTO As List(Of TaxChurchCodeDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String

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

			sql = String.Format("[Load Church Tax Code Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of TaxChurchCodeDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New TaxChurchCodeDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxChurchCodeData", .MessageContent = msgContent})
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

		Function LoadTaxNumberOfChildrenData(ByVal customerID As String, ByVal userID As String, ByVal language As String, ByVal canton As String, ByVal code As String, ByVal church As String) As IEnumerable(Of Integer) Implements IPublicDatabaseAccess.LoadTaxNumberOfChildrenData
			Dim listOfSearchResultDTO As List(Of Integer) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String

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

			sql = String.Format("[Load Number Of Children For Given Tax Code Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("code", ReplaceMissing(code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("church", ReplaceMissing(church, "N")))
			listOfParams.Add(New SqlClient.SqlParameter("canton", ReplaceMissing(canton, "AG")))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of Integer)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data As Integer = 0

						data = SafeGetInteger(reader, "Kinder", 0)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTaxNumberOfChildrenData", .MessageContent = msgContent})
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


		Function LoadQstData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal einkommen As Double, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As IEnumerable(Of QstDataDTO) Implements IPublicDatabaseAccess.LoadQstData
			Dim listOfSearchResultDTO As List(Of QstDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = String.Format("[Get QSTTarife For {0}{1}]", year, canton)

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("MANr", 0))
			listOfParams.Add(New SqlParameter("Einkommen", ReplaceMissing(einkommen, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kirchensteuer", ReplaceMissing(kirchsteuer, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Gruppe", ReplaceMissing(qstGroup, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Geschlecht", ReplaceMissing(geschlecht, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kinder", ReplaceMissing(childern, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of QstDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New QstDataDTO

						data.Mindest_Abzug = SafeGetDecimal(reader, "Mindest_Abzug", 0)
						data.Steuer_Proz = SafeGetDecimal(reader, "Steuer_Proz", 0)
						data.Einkommen = SafeGetDecimal(reader, "Einkommen", 0)
						data.Schritt = SafeGetDecimal(reader, "Schritt", 0)
						data.Steuer_Fr = SafeGetDecimal(reader, "Steuer_Fr", 0)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadQstData", .MessageContent = msgContent})
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

		Function LoadAllowedQstData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer, ByVal childern As Integer, ByVal qstGroup As String, ByVal kirchsteuer As String, ByVal geschlecht As String) As QstDataAllowedDTO Implements IPublicDatabaseAccess.LoadAllowedQstData
			Dim listOfSearchResultDTO As QstDataAllowedDTO = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Allowed QSTCode]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Year", ReplaceMissing(year, Now.Year)))
			listOfParams.Add(New SqlParameter("Kanton", ReplaceMissing(canton, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kirchensteuer", ReplaceMissing(kirchsteuer, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Gruppe", ReplaceMissing(qstGroup, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Geschlecht", ReplaceMissing(geschlecht, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kinder", ReplaceMissing(childern, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New QstDataAllowedDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New QstDataAllowedDTO

						data.IsQstDataAllowed = reader.HasRows

						listOfSearchResultDTO = data

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAllowedQstData", .MessageContent = msgContent})
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

		Function LoadCommunityData(ByVal customerID As String, ByVal userID As String, ByVal canton As String, ByVal language As String) As IEnumerable(Of CommunityDataDTO) Implements IPublicDatabaseAccess.LoadCommunityData
			Dim listOfSearchResultDTO As List(Of CommunityDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load Assigned Canton Community Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("canton", ReplaceMissing(canton, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of CommunityDataDTO)


			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New CommunityDataDTO

						' don't need any more!
						data.ID = 0
						data.HistoricNumber = 0
						data.BezirkNumber = 0 ' SafeGetInteger(reader, "BezirkNumber", Nothing)
						data.BezirkName = String.Empty ' SafeGetString(reader, "BezirkName")

						data.Canton = SafeGetString(reader, "Canton")
						data.BFSNumber = SafeGetInteger(reader, "BFSNumber", Nothing)
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCommunityData", .MessageContent = msgContent})
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

		Function LoadEmploymentTypeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of EmploymentTypeDataDTO) Implements IPublicDatabaseAccess.LoadEmploymentTypeData
			Dim listOfSearchResultDTO As List(Of EmploymentTypeDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load EmploymentType Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of EmploymentTypeDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New EmploymentTypeDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadEmploymentTypeData", .MessageContent = msgContent})
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

		Function LoadOtherEmploymentTypeData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of EmploymentTypeDataDTO) Implements IPublicDatabaseAccess.LoadOtherEmploymentTypeData
			Dim listOfSearchResultDTO As List(Of EmploymentTypeDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load Other EmploymentType Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of EmploymentTypeDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New EmploymentTypeDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadOtherEmploymentTypeData", .MessageContent = msgContent})
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

		Function LoadTypeOfStayData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of TypeOfStayDataDTO) Implements IPublicDatabaseAccess.LoadTypeOfStayData
			Dim listOfSearchResultDTO As List(Of TypeOfStayDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load Type Of Stay Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of TypeOfStayDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New TypeOfStayDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTypeOfStayData", .MessageContent = msgContent})
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

		Function LoadPermissionData(ByVal customerID As String, ByVal userID As String, ByVal language As String) As IEnumerable(Of PermissionDataDTO) Implements IPublicDatabaseAccess.LoadPermissionData
			Dim listOfSearchResultDTO As List(Of PermissionDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load Permission Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of PermissionDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PermissionDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Code = SafeGetString(reader, "Code")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPermissionData", .MessageContent = msgContent})
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

		Function LoadForeignCategoryData(ByVal customerID As String, ByVal userID As String, ByVal code As String, ByVal language As String) As IEnumerable(Of PermissionDataDTO) Implements IPublicDatabaseAccess.LoadForeignCategoryData
			Dim listOfSearchResultDTO As List(Of PermissionDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1

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

			Dim sql As String
			sql = String.Format("[Load Foreign Category Data]")

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customerID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("userID", ReplaceMissing(userID, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("code", ReplaceMissing(code, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of PermissionDataDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New PermissionDataDTO

						data.ID = SafeGetInteger(reader, "ID", Nothing)
						data.RecNr = SafeGetInteger(reader, "RecNr", Nothing)
						data.Rec_Value = SafeGetString(reader, "Rec_Value")
						data.Code = SafeGetString(reader, "Code")
						data.Translated_Value = SafeGetString(reader, "Translated_Value")


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadForeignCategoryData", .MessageContent = msgContent})
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



		Function LoadChildEducationData(ByVal customerID As String, ByVal canton As String, ByVal year As Integer) As ChildEducationDataDTO Implements IPublicDatabaseAccess.LoadChildEducationData
			Dim listOfSearchResultDTO As ChildEducationDataDTO = Nothing
			Dim excludeCheckInteger As Integer = 1

			Dim sql As String
			sql = "[Get ChildEducation Data]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Year", ReplaceMissing(year, DBNull.Value)))
			listOfParams.Add(New SqlParameter("Kanton", ReplaceMissing(canton, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New ChildEducationDataDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New ChildEducationDataDTO

						data.Ado_Zulage = SafeGetDecimal(reader, "Ado_Zulage", 0)
						data.Au1_Day = SafeGetDecimal(reader, "Au1_Day", 0)
						data.Au1_Month = SafeGetDecimal(reader, "Au1_Month", 0)
						data.Au1_Std = SafeGetDecimal(reader, "Au1_Std", 0)
						data.Au2_Day = SafeGetDecimal(reader, "Au2_Day", 0)
						data.Au2_Std = SafeGetDecimal(reader, "Au2_Std", 0)
						data.Bemerkung_1 = SafeGetString(reader, "Bemerkung_1")
						data.Bemerkung_2 = SafeGetString(reader, "Bemerkung_2")
						data.Bemerkung_3 = SafeGetString(reader, "Bemerkung_3")
						data.Bemerkung_4 = SafeGetString(reader, "Bemerkung_4")
						data.ChangeAuIn = SafeGetString(reader, "ChangeAuIn")
						data.ChangeAuIn_2 = SafeGetString(reader, "ChangeAuIn_2")
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangeKiIn = SafeGetString(reader, "ChangeKiIn")
						data.ChangeKiIn_2 = SafeGetString(reader, "ChangeKiIn_2")
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.FAK_Kanton = SafeGetString(reader, "FAK_Kanton")
						data.Fak_Name = SafeGetString(reader, "Fak_Name")
						data.Fak_ZHD = SafeGetString(reader, "Fak_ZHD")
						data.Fak_PLZOrt = SafeGetString(reader, "Fak_PLZOrt")
						data.Fak_Postfach = SafeGetString(reader, "Fak_Postfach")
						data.Fak_Proz = SafeGetDecimal(reader, "Fak_Proz", 0)
						data.Fak_Strasse = SafeGetString(reader, "Fak_Strasse")
						data.Geb_Zulage = SafeGetDecimal(reader, "Geb_Zulage", 0)
						data.ID = SafeGetInteger(reader, "ID", 0)
						data.Ki1_Day = SafeGetDecimal(reader, "Ki1_Day", 0)
						data.Ki1_FakMax = SafeGetDecimal(reader, "Ki1_FakMax", 0)
						data.Ki1_Month = SafeGetDecimal(reader, "Ki1_Month", 0)
						data.Ki1_Std = SafeGetDecimal(reader, "Ki1_Std", 0)
						data.Ki2_Day = SafeGetDecimal(reader, "Ki2_Day", 0)
						data.Ki2_FakMax = SafeGetDecimal(reader, "Ki2_FakMax", 0)
						data.Ki2_Month = SafeGetDecimal(reader, "Ki2_Month", 0)
						data.Ki2_Std = SafeGetDecimal(reader, "Ki2_Std", 0)
						data.YMinLohn = SafeGetDecimal(reader, "YMinLohn", 0)


						listOfSearchResultDTO = data

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadChildEducationData", .MessageContent = msgContent})
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
