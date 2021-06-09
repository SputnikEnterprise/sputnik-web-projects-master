Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace WOSInfo



	Partial Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess


		Function AddInternalVacancyData(ByVal customerID As String, ByVal wosID As String, ByVal userGuid As String, ByVal CustomerData As CustomerUserData, ByVal vacancyData As DataTable) As Boolean Implements IWOSDatabaseAccess.AddInternalVacancyData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim vacancyRowData = vacancyData.Rows(0)

			Try
				' Extract parameter data from data row.
				Dim vakNr As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "VakNr"))
				Dim kdNr As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "KDNr"))
				Dim kdZhdNr As Integer? = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "KDZHDNr"))

				Dim Berater As String = GetValueFromdataRow(vacancyRowData, "berater")
				Dim filiale As String = GetValueFromdataRow(vacancyRowData, "Filiale")

				Dim vakKontakt As String = GetValueFromdataRow(vacancyRowData, "vakkontakt")
				Dim Kontakt As String = GetValueFromdataRow(vacancyRowData, "Userkontakt")
				Dim vakState As String = GetValueFromdataRow(vacancyRowData, "vakState")
				Dim bezeichnung As String = GetValueFromdataRow(vacancyRowData, "Bezeichnung")

				Dim ShortDescription As String = String.Empty
				Dim TitelForSearch As String = String.Empty
				If vacancyRowData.Table.Columns.Contains("ShortDescription") Then ShortDescription = GetValueFromdataRow(vacancyRowData, "ShortDescription")
				If vacancyRowData.Table.Columns.Contains("TitelForSearch") Then TitelForSearch = GetValueFromdataRow(vacancyRowData, "TitelForSearch")

				Dim JobChannelPriority As Boolean = False
				If vacancyRowData.Table.Columns.Contains("JobChannelPriority") Then
					JobChannelPriority = GetValueFromdataRow(vacancyRowData, "JobChannelPriority")
				End If

				Dim SBNNumber As Integer = 0
				Dim vakSubGroup As String = String.Empty
				If vacancyRowData.Table.Columns.Contains("SBNNumber") Then SBNNumber = ParseNullableInt(GetValueFromdataRow(vacancyRowData, "SBNNumber"))
				If vacancyRowData.Table.Columns.Contains("SubGroup") Then vakSubGroup = GetValueFromdataRow(vacancyRowData, "SubGroup")


				Dim vakslogan As String = GetValueFromdataRow(vacancyRowData, "Slogan")
				Dim vakgruppe As String = GetValueFromdataRow(vacancyRowData, "Gruppe")
				Dim url As String = GetValueFromdataRow(vacancyRowData, "VakLink")
				Dim vakBeginn As String = GetValueFromdataRow(vacancyRowData, "Beginn")
				Dim vakJobprozent As String = GetValueFromdataRow(vacancyRowData, "Jobprozent")

				Dim JobProzTuple = SplitTwoValueString(vakJobprozent, "#")

				Dim proz_von As String = JobProzTuple.Item1
				Dim proz_bis As String = JobProzTuple.Item2
				If proz_von = proz_bis Then
					vakJobprozent = proz_von
				Else
					vakJobprozent = String.Format("{0} - {1}", proz_von, proz_bis)

				End If

				Dim vakAnstellung As String = GetValueFromdataRow(vacancyRowData, "Anstellung")
				Dim vakdauer As String = GetValueFromdataRow(vacancyRowData, "Dauer")
				Dim vakMAAge As String = GetValueFromdataRow(vacancyRowData, "MAAge")
				Dim vakMASex As String = GetValueFromdataRow(vacancyRowData, "MASex")
				Dim vakMAZivil As String = GetValueFromdataRow(vacancyRowData, "MAZivil")
				Dim vakMALohn As String = GetValueFromdataRow(vacancyRowData, "MALohn")
				Dim vakJobTime As String = GetValueFromdataRow(vacancyRowData, "JobTime")
				Dim ort As String = GetValueFromdataRow(vacancyRowData, "JobOrt")
				Dim plz As String = GetValueFromdataRow(vacancyRowData, "JobPLZ")

				Dim Ausbildung As String = GetValueFromdataRow(vacancyRowData, "Ausbildung")
				Dim _Ausbildung As String = GetValueFromdataRow(vacancyRowData, "_Ausbildung")
				Dim Reserve1 As String = GetValueFromdataRow(vacancyRowData, "Reserve1")
				Dim _Reserve1 As String = GetValueFromdataRow(vacancyRowData, "_Reserve1")
				Dim Reserve2 As String = GetValueFromdataRow(vacancyRowData, "Reserve2")
				Dim _Reserve2 As String = GetValueFromdataRow(vacancyRowData, "_Reserve2")
				Dim Reserve3 As String = GetValueFromdataRow(vacancyRowData, "Reserve3")
				Dim _Reserve3 As String = GetValueFromdataRow(vacancyRowData, "_Reserve3")
				Dim Weiterbildung As String = GetValueFromdataRow(vacancyRowData, "v_zusatz_Weiterbildung")
				Dim _Weiterbildung As String = GetValueFromdataRow(vacancyRowData, "_Weiterbildung")
				Dim SBeschreibung As String = GetValueFromdataRow(vacancyRowData, "SBeschreibung")
				Dim _SBeschreibung As String = GetValueFromdataRow(vacancyRowData, "_SBeschreibung")
				Dim SKennt As String = GetValueFromdataRow(vacancyRowData, "SKennt")
				Dim _SKennt As String = GetValueFromdataRow(vacancyRowData, "_SKennt")
				Dim EDVKennt As String = GetValueFromdataRow(vacancyRowData, "EDVKennt")
				Dim _EDVKennt As String = GetValueFromdataRow(vacancyRowData, "_EDVKennt")


				Dim Jobs_Vorspann As String = GetValueFromdataRow(vacancyRowData, "Jobs_Vorspann")
				Dim _Jobs_Vorspann As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Vorspann")

				Dim Jobs_Aufgabe As String = GetValueFromdataRow(vacancyRowData, "Jobs_Aufgabe")
				Dim _Jobs_Aufgabe As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Aufgabe")

				Dim Jobs_Anforderung As String = GetValueFromdataRow(vacancyRowData, "Jobs_Anforderung")
				Dim _Jobs_Anforderung As String = GetValueFromdataRow(vacancyRowData, "_Jobs_Anforderung")

				Dim Jobs_WirBieten As String = GetValueFromdataRow(vacancyRowData, "Jobs_WirBieten")
				Dim _Jobs_WirBieten As String = GetValueFromdataRow(vacancyRowData, "_Jobs_WirBieten")


				Dim Branche As String = GetValueFromdataRow(vacancyRowData, "Branchen")
				Dim Sprache As String = GetValueFromdataRow(vacancyRowData, "MSprachen")
				Dim Region As String = GetValueFromdataRow(vacancyRowData, "Region")

				Dim vak_Kanton As String = GetValueFromdataRow(vacancyRowData, "Vak_Kanton")
				Dim Qualifikation As String = GetValueFromdataRow(vacancyRowData, "Qualifikation")

				Dim setOnline As Boolean = GetValueFromdataRow(vacancyRowData, "IsOnline")

				Dim jobCHBerufData As String = GetValueFromdataRow(vacancyRowData, "JobCHBerufData")


				' Save vacancy in databse.

				Dim sql As String
				sql = "[Create New Vacancy For Internal Jobplattform]"

				Dim listOfParams As New List(Of SqlParameter)


				listOfParams.Add(New SqlClient.SqlParameter("@Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("WOSGuid", ReplaceMissing(wosID, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Guid", ReplaceMissing(userGuid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Name", ReplaceMissing(CustomerData.Customer_Name, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_strasse", ReplaceMissing(CustomerData.Customer_Street, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_plz", ReplaceMissing(CustomerData.Customer_Postcode, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_ort", ReplaceMissing(CustomerData.Customer_City, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_land", ReplaceMissing(CustomerData.Customer_Country, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_telefon", ReplaceMissing(CustomerData.Customer_Telephone, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_telefax", ReplaceMissing(CustomerData.Customer_Telefax, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_eMail", ReplaceMissing(CustomerData.Customer_eMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Customer_Homepage", ReplaceMissing(CustomerData.Customer_Homepage, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("User_Vorname", ReplaceMissing(CustomerData.User_Firstname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Nachname", ReplaceMissing(CustomerData.User_Lastname, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("User_Telefon", ReplaceMissing(CustomerData.User_Telephone, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Telefax", ReplaceMissing(CustomerData.User_Telefax, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_eMail", ReplaceMissing(CustomerData.User_eMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Homepage", ReplaceMissing(CustomerData.User_Homepage, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Anrede", ReplaceMissing(CustomerData.User_Salution, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(CustomerData.User_Initial, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(CustomerData.User_Filiale, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Loged_UserName", ReplaceMissing(CustomerData.Loged_UserName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Loged_UserGuid", ReplaceMissing(CustomerData.Loged_UserGuid, DBNull.Value)))


				listOfParams.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vakNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(kdNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KDZHDNr", ReplaceMissing(kdZhdNr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Berater", ReplaceMissing(Berater, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Filiale", ReplaceMissing(filiale, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vakKontakt", ReplaceMissing(vakKontakt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Userkontakt", ReplaceMissing(Kontakt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vakState", ReplaceMissing(vakState, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("bezeichnung", ReplaceMissing(bezeichnung, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("ShortDescription", ReplaceMissing(ShortDescription, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("TitelForSearch", ReplaceMissing(TitelForSearch, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Slogan", ReplaceMissing(vakslogan, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Gruppe", ReplaceMissing(vakgruppe, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("VakLink", ReplaceMissing(url, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Beginn", ReplaceMissing(vakBeginn, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Jobprozent", ReplaceMissing(vakJobprozent, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Anstellung", ReplaceMissing(vakAnstellung, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Dauer", ReplaceMissing(vakdauer, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MAAge", ReplaceMissing(vakMAAge, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MASex", ReplaceMissing(vakMASex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MAZivil", ReplaceMissing(vakMAZivil, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MALohn", ReplaceMissing(vakMALohn, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobTime", ReplaceMissing(vakJobTime, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobOrt", ReplaceMissing(ort, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobPLZ", ReplaceMissing(plz, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Ausbildung", ReplaceMissing(Ausbildung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Ausbildung", ReplaceMissing(_Ausbildung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Reserve1", ReplaceMissing(Reserve1, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Reserve1", ReplaceMissing(_Reserve1, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Reserve2", ReplaceMissing(Reserve2, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Reserve2", ReplaceMissing(_Reserve2, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Reserve3", ReplaceMissing(Reserve3, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Reserve3", ReplaceMissing(_Reserve3, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Weiterbildung", ReplaceMissing(Weiterbildung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Weiterbildung", ReplaceMissing(_Weiterbildung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("SBeschreibung", ReplaceMissing(SBeschreibung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_SBeschreibung", ReplaceMissing(_SBeschreibung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("SKennt", ReplaceMissing(SKennt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_SKennt", ReplaceMissing(_SKennt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("EDVKennt", ReplaceMissing(EDVKennt, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_EDVKennt", ReplaceMissing(_EDVKennt, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Jobs_Vorspann", ReplaceMissing(Jobs_Vorspann, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Jobs_Vorspann", ReplaceMissing(_Jobs_Vorspann, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Jobs_Aufgabe", ReplaceMissing(Jobs_Aufgabe, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Jobs_Aufgabe", ReplaceMissing(_Jobs_Aufgabe, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Jobs_Anforderung", ReplaceMissing(Jobs_Anforderung, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Jobs_Anforderung", ReplaceMissing(_Jobs_Anforderung, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Jobs_WirBieten", ReplaceMissing(Jobs_WirBieten, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("_Jobs_WirBieten", ReplaceMissing(_Jobs_WirBieten, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Branchen", ReplaceMissing(Branche, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("MSprache", ReplaceMissing(Sprache, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Region", ReplaceMissing(Region, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Vak_Kanton", ReplaceMissing(vak_Kanton, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Qualifikation", ReplaceMissing(Qualifikation, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(GetValueFromdataRow(vacancyRowData, "USBild"), DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(GetValueFromdataRow(vacancyRowData, "USSign"), DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("SetOnline", ReplaceMissing(setOnline, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("SBNNumber", ReplaceMissing(SBNNumber, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("vakSubGroup", ReplaceMissing(vakSubGroup, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("JobChannelPriority", ReplaceMissing(JobChannelPriority, 0)))


				success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)
				m_Logger.LogInfo(String.Format("Wos_Guid: {0} vacancy {1} is now {2}", wosID, vakNr, If(setOnline, "added", "deleted")))


				'Save Field jobCHBerufData: Beruf#Erfahrung#Position|Beruf#Erfahung#Position...
				If success AndAlso setOnline AndAlso Not String.IsNullOrWhiteSpace(jobCHBerufData) Then

					Dim jobCHBerufRecords = jobCHBerufData.Split({"|"c})
					For Each record In jobCHBerufRecords
						If String.IsNullOrWhiteSpace(record) Then
							Continue For
						End If

						' Split Beruf#Erfahrung#Position
						Dim berufErfahrungPosition As String() = record.Split({"#"c})
						Dim beruf As String = String.Empty
						Dim erfahrung As String = String.Empty
						Dim position As String = String.Empty

						If berufErfahrungPosition.Length > 0 Then
							beruf = berufErfahrungPosition(0)
						End If
						If berufErfahrungPosition.Length > 1 Then
							erfahrung = berufErfahrungPosition(1)
						End If
						If berufErfahrungPosition.Length > 2 Then
							position = berufErfahrungPosition(2)
						End If

						' INSERT
						sql = "INSERT INTO dbo.tblVacancyJobExperience "
						sql &= " (Customer_Guid, WOS_Guid, VakNr, Berufgruppe, BerufErfahrung, BerufPosition, CreatedOn, CreatedFrom) "
						sql &= " VALUES "
						sql &= " (@Customer_ID, @WOSGuid, @VakNr, @Berufgruppe, @BerufErfahrung, @BerufPosition, @CreatedOn, @CreatedFrom)"

						' Parameters
						Dim sqlParameter As New List(Of SqlClient.SqlParameter)

						sqlParameter.Add(New SqlClient.SqlParameter("@Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
						sqlParameter.Add(New SqlClient.SqlParameter("WOSGuid", ReplaceMissing(wosID, DBNull.Value)))

						sqlParameter.Add(New SqlClient.SqlParameter("VakNr", ReplaceMissing(vakNr, DBNull.Value)))
						sqlParameter.Add(New SqlClient.SqlParameter("Berufgruppe", ReplaceMissing(beruf, DBNull.Value)))
						sqlParameter.Add(New SqlClient.SqlParameter("BerufErfahrung", ReplaceMissing(erfahrung, DBNull.Value)))
						sqlParameter.Add(New SqlClient.SqlParameter("BerufPosition", ReplaceMissing(position, DBNull.Value)))
						sqlParameter.Add(New SqlClient.SqlParameter("CreatedOn", Now))
						sqlParameter.Add(New SqlClient.SqlParameter("CreatedFrom", DBNull.Value))

						Try
							success = success AndAlso ExecuteNonQuery(sql, sqlParameter, CommandType.Text, False)

						Catch ex As Exception
							Dim msgContent = String.Format("{0}", ex.ToString)
							m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "INSERT INTO tblVacancyJobExperience", .MessageContent = msgContent})

						End Try
					Next ' In jobCHBerufRecords

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddInternalVacancyData", .MessageContent = msgContent})

				success = False

			Finally

			End Try

			Return success
		End Function



#Region "helpers"

		''' <summary>
		''' Gets a value from a data row.
		''' </summary>
		''' <param name="dataRow">The data row.</param>
		''' <param name="column">The column.</param>
		''' <returns>The value or nothing.</returns>
		Private Function GetValueFromdataRow(ByVal dataRow As DataRow, ByVal column As String) As Object

			If Not dataRow.IsNull(column) Then
				Dim value As Object = dataRow(column)
				Return value
			End If

			Return Nothing
		End Function

		''' <summary>
		''' Splits a two value string.
		''' </summary>
		''' <param name="str">The string.</param>
		''' <param name="delimeter">The delimeter.</param>
		''' <returns>Tuple with values.</returns>
		Private Function SplitTwoValueString(ByVal str As String, ByVal delimeter As String) As Tuple(Of Integer?, Integer?)

			Dim value1 As Integer? = Nothing
			Dim value2 As Integer? = Nothing

			If Not String.IsNullOrEmpty(str) Then

				Dim tokens As String() = str.Trim().Split(delimeter)

				If tokens.Count = 2 Then
					value1 = Integer.Parse(tokens(0))
					value2 = Integer.Parse(tokens(1))
				End If

			End If

			Return New Tuple(Of Integer?, Integer?)(value1, value2)

		End Function

		''' <summary>
		''' Parsets a nullable integer.
		''' </summary>
		''' <param name="str">The string.</param>
		''' <returns>Nullable integer value or nothing.</returns>
		Private Function ParseNullableInt(ByVal str As String) As Integer?

			If String.IsNullOrEmpty(str) Then
				Return Nothing
			End If

			Return Integer.Parse(str)

		End Function

#End Region


	End Class


End Namespace
