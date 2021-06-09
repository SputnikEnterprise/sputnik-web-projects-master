Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace WOSInfo



	Partial Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess


		Function LoadVacancyData(ByVal customerID As String, ByVal customerWOSID As String, ByVal kdNumber As Integer?, ByVal vakNumber As Integer?) As IEnumerable(Of KDVakanzenDTO) Implements IWOSDatabaseAccess.LoadVacancyData
			Dim listOfSearchResultDTO As List(Of KDVakanzenDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim sql As String

			sql = "[Load Transfered Vacancy Data From WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(customerWOSID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("KDNumber", ReplaceMissing(kdNumber, DBNull.Value)))
			listOfParams.Add(New SqlParameter("VacancyNumber", ReplaceMissing(vakNumber, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)
			listOfSearchResultDTO = New List(Of KDVakanzenDTO)

			Try
				If reader IsNot Nothing Then

					While reader.Read
						Dim data = New KDVakanzenDTO

						data.RecID = SafeGetInteger(reader, "ID", Nothing)
						data.VakNr = SafeGetInteger(reader, "VakNr", Nothing)
						data.KDNr = SafeGetInteger(reader, "KDNr", Nothing)
						data.KDZHDNr = SafeGetInteger(reader, "KDZHDNr", Nothing)
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.Customer_Name = SafeGetString(reader, "Customer_Name")
						data.Customer_Strasse = SafeGetString(reader, "Customer_Strasse")
						data.Customer_Ort = SafeGetString(reader, "Customer_Ort")
						data.Customer_Telefon = SafeGetString(reader, "Customer_Telefon")
						data.Customer_eMail = SafeGetString(reader, "Customer_eMail")
						data.Berater = SafeGetString(reader, "Berater")
						data.Filiale = SafeGetString(reader, "Filiale")
						data.VakKontakt = SafeGetString(reader, "VakKontakt")
						data.VakState = SafeGetString(reader, "VakState")
						data.Bezeichnung = SafeGetString(reader, "Bezeichnung")
						data.TitelForSearch = SafeGetString(reader, "TitelForSearch")
						data.ShortDescription = SafeGetString(reader, "ShortDescription")
						data.Slogan = SafeGetString(reader, "Slogan")
						data.Gruppe = SafeGetString(reader, "Gruppe")
						data.SubGroup = SafeGetString(reader, "SubGroup")
						data.ExistLink = SafeGetBoolean(reader, "ExistLink", Nothing)
						data.VakLink = SafeGetString(reader, "VakLink")
						data.Beginn = SafeGetString(reader, "Beginn")
						data.JobProzent = SafeGetString(reader, "JobProzent")
						data.Anstellung = SafeGetString(reader, "Anstellung")
						data.Dauer = SafeGetString(reader, "Dauer")
						data.MAAge = SafeGetString(reader, "MAAge")
						data.MASex = SafeGetString(reader, "MASex")
						data.MAZivil = SafeGetString(reader, "MAZivil")
						data.MALohn = SafeGetString(reader, "MALohn")
						data.Jobtime = SafeGetString(reader, "Jobtime")
						data.JobOrt = SafeGetString(reader, "JobOrt")
						data.MAFSchein = SafeGetString(reader, "MAFSchein")
						data.MAAuto = SafeGetString(reader, "MAAuto")
						data.MANationality = SafeGetString(reader, "MANationality")
						data.KDBeschreibung = SafeGetString(reader, "KDBeschreibung")
						data.KDBietet = SafeGetString(reader, "KDBietet")
						data.SBeschreibung = SafeGetString(reader, "SBeschreibung")
						data.Reserve1 = SafeGetString(reader, "Reserve1")
						data.Taetigkeit = SafeGetString(reader, "Taetigkeit")
						data.Anforderung = SafeGetString(reader, "Anforderung")
						data.Reserve2 = SafeGetString(reader, "Reserve2")
						data.Reserve3 = SafeGetString(reader, "Reserve3")
						data.Ausbildung = SafeGetString(reader, "Ausbildung")
						data.Weiterbildung = SafeGetString(reader, "Weiterbildung")
						data.SKennt = SafeGetString(reader, "SKennt")
						data.EDVKennt = SafeGetString(reader, "EDVKennt")
						data.html_KDBeschreibung = SafeGetString(reader, "_KDBeschreibung")
						data.html_KDBietet = SafeGetString(reader, "_KDBietet")
						data.html_SBeschreibung = SafeGetString(reader, "_SBeschreibung")
						data.html_Reserve1 = SafeGetString(reader, "_Reserve1")
						data.html_Taetigkeit = SafeGetString(reader, "_Taetigkeit")
						data.html_Anforderung = SafeGetString(reader, "_Anforderung")
						data.html_Reserve2 = SafeGetString(reader, "_Reserve2")
						data.html_Reserve3 = SafeGetString(reader, "_Reserve3")
						data.html_Ausbildung = SafeGetString(reader, "_Ausbildung")
						data.html_Weiterbildung = SafeGetString(reader, "_Weiterbildung")
						data.html_SKennt = SafeGetString(reader, "_SKennt")
						data.html_EDVKennt = SafeGetString(reader, "_EDVKennt")
						data.Branchen = SafeGetString(reader, "Branchen")
						data.CreatedOn = SafeGetDateTime(reader, "CreatedOn", Nothing)
						data.CreatedFrom = SafeGetString(reader, "CreatedFrom")
						data.ChangedOn = SafeGetDateTime(reader, "ChangedOn", Nothing)
						data.ChangedFrom = SafeGetString(reader, "ChangedFrom")
						data.Transfered_User = SafeGetString(reader, "Transfered_User")
						data.Transfered_On = SafeGetDateTime(reader, "Transfered_On", Nothing)
						data.Transfered_Guid = SafeGetString(reader, "Transfered_Guid")
						data.Vak_Region = SafeGetString(reader, "Vak_Region")
						data.Vak_Kanton = SafeGetString(reader, "Vak_Kanton")
						data.MSprachen = SafeGetString(reader, "MSprachen")
						data.SSprachen = SafeGetString(reader, "SSprachen")
						data.Qualifikation = SafeGetString(reader, "Qualifikation")
						data.SQualifikation = SafeGetString(reader, "SQualifikation")
						data.User_Guid = SafeGetString(reader, "User_Guid")
						data.JobPLZ = SafeGetString(reader, "JobPLZ")
						data.Job_Categories = SafeGetString(reader, "Job_Categories")
						data.Job_Disciplines = SafeGetString(reader, "Job_Disciplines")
						data.Job_Position = SafeGetString(reader, "Job_Position")
						data.User_salutation = SafeGetString(reader, "BeraterSex")
						data.User_Firstname = SafeGetString(reader, "BeraterVorname")
						data.User_Lastname = SafeGetString(reader, "BeraterNachname")
						data.User_EMail = SafeGetString(reader, "BeraterEMail")
						data.User_Telefon = SafeGetString(reader, "BeraterTelefon")
						data.User_Picture = SafeGetByteArray(reader, "BeraterPicture")
						data.IsJobsCHOnline = SafeGetBoolean(reader, "JobCHOnline", Nothing)
						data.IsOstJobOnline = SafeGetBoolean(reader, "OstJobOnline", Nothing)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadVacancyData", .MessageContent = msgContent})
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

		Function DeleteVacancyData(ByVal customerID As String, ByVal wosID As String, ByVal vakNumber As Integer) As Boolean Implements IWOSDatabaseAccess.DeleteVacancyData
			Dim success As Boolean = True
			m_customerID = customerID

			Dim sql As String
			sql = "[Delete Assigned Vacancy From WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlParameter("Customer_ID", customerID))
			listOfParams.Add(New SqlParameter("WOSGuid", ReplaceMissing(wosID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("VakNr", ReplaceMissing(vakNumber, DBNull.Value)))


			Try
				success = success AndAlso ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "DeleteVacancyData", .MessageContent = msgContent})

				success = False

			Finally

			End Try

			Return success
		End Function


	End Class


End Namespace
