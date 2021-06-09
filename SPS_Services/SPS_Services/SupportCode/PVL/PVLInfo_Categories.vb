
Imports wsSPS_Services.DataTransferObject.PVLInfo.DataObjects


Namespace PVLInfo


	Partial Class CurrentpvlDatabaseAccess
		Inherits DatabaseAccessBase
		Implements ICurrentPVLDatabaseAccess

		Function LoadCurrentPVLInfo(ByVal customerID As String, ByVal canton As String, ByVal postcode As String, ByVal language As String) As IEnumerable(Of GAVNameResultDTO) Implements ICurrentPVLDatabaseAccess.LoadCurrentPVLInfo
			Dim listOfSearchResultDTO As List(Of GAVNameResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

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
			sql = "[Get GAVGruppe0]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("CustomerID", ReplaceMissing(customerID, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("Kanton", ReplaceMissing(canton, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("PLZ", ReplaceMissing(postcode, String.Empty)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVNameResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVNameResultDTO

						data.id_meta = SafeGetInteger(reader, "id_meta", 0)
						data.gav_number = SafeGetInteger(reader, "gav_number", 0)
						data.ave = SafeGetBoolean(reader, "ave", False)
						data.name_de = SafeGetString(reader, "name_de")
						data.name_fr = SafeGetString(reader, "name_fr")
						data.name_it = SafeGetString(reader, "name_it")
						data.publication_date = SafeGetDateTime(reader, "publication_date", Nothing)
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
						data.PVL_Edition = SafeGetInteger(reader, "PVL_Edition", 0)
						data.Created = SafeGetDateTime(reader, "Created", Nothing)

						data.ave_validity_start = SafeGetDateTime(reader, "ave_validity_start", Nothing)
						data.ave_validity_end = SafeGetDateTime(reader, "ave_validity_end", Nothing)
						data.unia_validity_start = SafeGetDateTime(reader, "unia_validity_start", Nothing)
						data.unia_validity_end = SafeGetDateTime(reader, "unia_validity_end", Nothing)
						data.validity_start_date = SafeGetDateTime(reader, "validity_start_date", Nothing)
						data.State = SafeGetInteger(reader, "State", 0)
						data.Version = SafeGetString(reader, "Version")
						data.GAVKanton = SafeGetString(reader, "GAVKanton")
						data.ID_Calculator = SafeGetInteger(reader, "ID_Calculator", 0)

						data.currdbname = SafeGetString(reader, "currdbname")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadCurrentPVLInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadCurrentPVLInfo", .MessageContent = msgContent})
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

		Function LoadPVLCriteriasInfo(ByVal customerID As String, ByVal metaID As Integer, ByVal language As String) As IEnumerable(Of GAVCriteriasResultDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLCriteriasInfo
			Dim listOfSearchResultDTO As List(Of GAVCriteriasResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

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
			sql = "[Get GAVCriterionValues]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("IDMeta", ReplaceMissing(metaID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVCriteriasResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVCriteriasResultDTO

						data.ID_Criterion = SafeGetInteger(reader, "ID_Criterion", 0)
						data.ID_Contract = SafeGetInteger(reader, "ID_Contract", 0)
						data.Element_ID = SafeGetInteger(reader, "Element_ID", 0)

						data.name_de = SafeGetString(reader, "name_de")
						data.name_fr = SafeGetString(reader, "name_fr")
						data.name_it = SafeGetString(reader, "name_it")


						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLCriteriasInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLCriteriasInfo", .MessageContent = msgContent})
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

		Function LoadPVLCriteriaValueInfo(ByVal customerID As String, ByVal criteriaID As Integer, ByVal language As String) As IEnumerable(Of GAVCriteriaValueResultDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLCriteriaValueInfo
			Dim listOfSearchResultDTO As List(Of GAVCriteriaValueResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

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
			sql = "[Get GAVCriterionValuesByIDCriterion]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("IDCriterion", ReplaceMissing(criteriaID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVCriteriaValueResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVCriteriaValueResultDTO

						data.ID_Criterion = SafeGetInteger(reader, "ID_Criterion", 0)
						data.ID_CriterionValue = SafeGetInteger(reader, "ID_CriterionValue", 0)

						data.txtText = SafeGetString(reader, "txtText")
						data.txtTable = SafeGetString(reader, "txtTable")

						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLCriteriaValueInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLCriteriaValueInfo", .MessageContent = msgContent})
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

		Function LoadPVLTaxonomyInfo(ByVal customerID As String, ByVal metaID As Integer) As IEnumerable(Of GAVTaxonomyDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLTaxonomyInfo
			Dim listOfSearchResultDTO As List(Of GAVTaxonomyDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get GAVTaxonomyValues]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("IDMeta", ReplaceMissing(metaID, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVTaxonomyDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVTaxonomyDTO

						data.ID_Taxonomy_Entry = SafeGetInteger(reader, "ID_Taxonomy_Entry", 0)
						data.ID_Taxonomy = SafeGetInteger(reader, "ID_Taxonomy", 0)
						data.Value = SafeGetString(reader, "Value")

						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLTaxonomyInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLTaxonomyInfo", .MessageContent = msgContent})
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

		Function LoadPVLCalculationData(ByVal customerID As String, ByVal categoryValues As String) As GAVCalculationDTO Implements ICurrentPVLDatabaseAccess.LoadPVLCalculationData
			Dim listOfSearchResultDTO As GAVCalculationDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get GAVCalculationValue]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("IDCategoryValue", ReplaceMissing(categoryValues, String.Empty)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New GAVCalculationDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVCalculationDTO

						data.ID_Calculation = SafeGetInteger(reader, "ID_Calculation", 0)
						data.ID_Calculator = SafeGetInteger(reader, "ID_Calculator", 0)
						data.basic_hourly_wage = SafeGetString(reader, "basic_hourly_wage")
						data.gross_hourly_wage = SafeGetString(reader, "gross_hourly_wage")
						data.number_of_holidays = SafeGetString(reader, "number_of_holidays")

						data.holiday_compensation = SafeGetString(reader, "holiday_compensation")
						data.compensation_13th_month_salary = SafeGetString(reader, "compensation_13th_month_salary")
						data.monthly_wage = SafeGetString(reader, "monthly_wage")
						data.number_of_vacation_days = SafeGetString(reader, "number_of_vacation_days")
						data.vacation_pay = SafeGetString(reader, "vacation_pay")
						data.percentage_vacation_pay = SafeGetString(reader, "percentage_vacation_pay")
						data.percentage_holiday_compensation = SafeGetString(reader, "percentage_holiday_compensation")
						data.percentage_13th_month_salary = SafeGetString(reader, "percentage_13th_month_salary")
						data.calculation_vacation_pay = SafeGetString(reader, "calculation_vacation_pay")

						data.calculation_holiday_compensation = SafeGetString(reader, "calculation_holiday_compensation")
						data.calculation_13th_month_salary = SafeGetString(reader, "calculation_13th_month_salary")
						data.has_13th_month_salary_compensation = SafeGetBoolean(reader, "has_13th_month_salary_compensation", False)

						data.sortPosition = SafeGetInteger(reader, "sortPosition", 0)
						data.percentage_far_an = SafeGetString(reader, "percentage_far_an")
						data.percentage_far_ag = SafeGetString(reader, "percentage_far_ag")
						data.calculation_far = SafeGetString(reader, "calculation_far")
						data.far_bvg_relevant = SafeGetBoolean(reader, "far_bvg_relevant", False)
						data.ID_AlternativeText = SafeGetInteger(reader, "ID_AlternativeText", 0)

						listOfSearchResultDTO = data

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLCalculationData"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLCalculationData", .MessageContent = msgContent})
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

		Function LoadPVLCategoryValueWithBaseValueInfo(ByVal customerID As String, ByVal categoryID As Integer, ByVal baseCategoryValueID As Integer?, ByVal language As String) As IEnumerable(Of GAVCategoryValueDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLCategoryValueWithBaseValueInfo
			Dim listOfSearchResultDTO As List(Of GAVCategoryValueDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

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
			If baseCategoryValueID.HasValue Then
				sql = "[Get GAVCategoryValues With BaseNr]"
			Else
				sql = "[Get GAVCategoryValues]"
			End If

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID_Category", ReplaceMissing(categoryID, 0)))
			If baseCategoryValueID.HasValue Then listOfParams.Add(New SqlClient.SqlParameter("ID_BaseCategoryValue", ReplaceMissing(baseCategoryValueID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVCategoryValueDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVCategoryValueDTO

						data.ID_CategoryValue = SafeGetInteger(reader, "ID_CategoryValue", 0)
						data.ID_Category = SafeGetInteger(reader, "ID_Category", 0)
						data.ID_BaseCategory = baseCategoryValueID
						data.Text_De = SafeGetString(reader, "Text_De")

						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLCategoryValueWithBaseValueInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLCategoryValueWithBaseValueInfo", .MessageContent = msgContent})
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

		Function LoadPVLCategoryInfo(ByVal customerID As String, ByVal metaID As Integer, ByVal language As String) As IEnumerable(Of GAVCategoryDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLCategoryInfo
			Dim listOfSearchResultDTO As List(Of GAVCategoryDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

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
			sql = "[Get GAVCategoryNames]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("ID_Meta", ReplaceMissing(metaID, 0)))
			listOfParams.Add(New SqlClient.SqlParameter("language", ReplaceMissing(selLanguage, "DE")))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVCategoryDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVCategoryDTO

						data.ID_Category = SafeGetInteger(reader, "ID_Category", 0)
						data.ID_Calculator = SafeGetInteger(reader, "ID_Calculator", 0)
						data.ID_BaseCategory = SafeGetInteger(reader, "ID_BaseCategory", 0)

						data.name_de = SafeGetString(reader, "name_de")
						data.name_fr = SafeGetString(reader, "name_fr")
						data.name_it = SafeGetString(reader, "name_it")

						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLCategoryInfo"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLCategoryInfo", .MessageContent = msgContent})
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

		Function LoadPVLWarningData(ByVal customerID As String, ByVal gavNumber As Integer) As GAVNotificationDTO Implements ICurrentPVLDatabaseAccess.LoadPVLWarningData
			Dim listOfSearchResultDTO As GAVNotificationDTO = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get Sputnik Info about PVL GAVNumber]"

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("GAVNumber", ReplaceMissing(gavNumber, 0)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New GAVNotificationDTO

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVNotificationDTO

						data.gav_number = SafeGetInteger(reader, "gav_number", 0)
						data.Id = SafeGetInteger(reader, "Id", 0)
						data.ID_CategoryValue = SafeGetInteger(reader, "ID_CategoryValue", 0)

						data.Info = SafeGetString(reader, "Info")

						listOfSearchResultDTO = data

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLWarningData"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLWarningData", .MessageContent = msgContent})
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

		Function LoadPVLAnhang1Data(ByVal customerID As String) As IEnumerable(Of GAVNameResultDTO) Implements ICurrentPVLDatabaseAccess.LoadPVLAnhang1Data
			Dim listOfSearchResultDTO As List(Of GAVNameResultDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String
			sql = "[Get PVL Anhang1 Berufe]"

			Dim reader = OpenReader(sql, Nothing, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of GAVNameResultDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New GAVNameResultDTO

						data.gav_number = SafeGetInteger(reader, "GAV_Number", 0)

						listOfSearchResultDTO.Add(data)

					End While

				End If
				m_utility.AddNotifyData(New SPUtilities.NotifyMessageData With {.CustomerID = m_customerID, .NotifyHeader = "PVLInfo.CurrentpvlDatabaseAccess",
																.NotifyComments = String.Format("LoadPVLAnhang1Data"), .NotifyArt = SPUtilities.NotifyArtEnum.PVLCATEGORIES, .CreatedFrom = "System"})


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadPVLAnhang1Data", .MessageContent = msgContent})
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
