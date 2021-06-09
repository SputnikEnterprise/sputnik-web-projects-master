Imports System.Data.SqlClient
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities


Namespace WOSInfo



	Partial Class WOSDatabaseAccess
		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess


		Function AddWOSCustomerDocumentData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal customerWosData As CustomerWOSData) As Boolean Implements IWOSDatabaseAccess.AddWOSCustomerDocumentData
			Dim success As Boolean = True
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Try

				Dim sql As String
				sql = "[Create New Customer Document For WOS]"


				Dim mANr As Integer? = customerWosData.EmployeeNumber
				Dim kdNr As Integer? = customerWosData.CustomerNumber
				Dim ZHDNr As Integer? = customerWosData.CresponsibleNumber
				Dim ESNr As Integer? = customerWosData.EmploymentNumber
				Dim ESLohnNr As Integer? = customerWosData.EmploymentLineNumber
				Dim RPNr As Integer? = customerWosData.ReportNumber
				Dim RENr As Integer? = customerWosData.InvoiceNumber
				Dim proposeNr As Integer? = customerWosData.ProposeNumber

				Dim MDName As String = customerWosData.customername
				Dim LogedUser_Guid As String = customerWosData.LogedUserID

				Dim KD_Name As String = customerWosData.KD_Name
				Dim ZHD_Vorname As String = customerWosData.ZHD_Vorname
				Dim ZHD_Nachname As String = customerWosData.ZHD_Nachname
				Dim KD_Filiale As String = customerWosData.KD_Filiale

				Dim KD_Postfach As String = customerWosData.KD_Postfach
				Dim KD_Strasse As String = customerWosData.KD_Strasse
				Dim KD_PLZ As String = customerWosData.KD_PLZ
				Dim KD_Kanton As String = String.Empty
				Dim KD_Ort As String = customerWosData.KD_Ort
				Dim KD_AGB_Wos As String = customerWosData.KD_AGB_Wos
				Dim ZHDSex As String = customerWosData.ZHDSex
				Dim ZHD_Briefanrede As String = customerWosData.Zhd_BriefAnrede
				Dim DoNotShowContractInWOS As Boolean? = customerWosData.DoNotShowContractInWOS

				Dim KD_EMail As String = customerWosData.KD_Email
				Dim myArray = KD_EMail.Split("#"c)
				KD_EMail = String.Join(",", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim KD_Guid As String = customerWosData.KDTransferedGuid
				Dim ZHD_Guid As String = customerWosData.ZHDTransferedGuid
				Dim Doc_Guid As String = customerWosData.AssignedDocumentGuid
				Dim Doc_Art As String = customerWosData.AssignedDocumentArtName
				Dim Doc_Info As String = customerWosData.AssignedDocumentInfo
				Dim Result As String = String.Empty
				Dim KD_Berater As String = customerWosData.KD_Berater
				Dim ZHD_Berater As String = customerWosData.Zhd_Berater

				Dim KD_Beruf As String = customerWosData.KD_Beruf
				myArray = KD_Beruf.Split("#"c)
				KD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim KD_Branche As String = customerWosData.KD_Branche
				myArray = KD_Branche.Split("#"c)
				KD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim ZHD_Beruf As String = customerWosData.Zhd_Beruf
				myArray = ZHD_Beruf.Split("#"c)
				ZHD_Beruf = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim ZHD_Branche As String = customerWosData.Zhd_Branche
				myArray = ZHD_Branche.Split("#"c)
				ZHD_Branche = String.Join("#", myArray.Where(Function(s) Not String.IsNullOrEmpty(s)))

				Dim ZHD_AGB_WOS As String = customerWosData.ZHD_AGB_Wos
				Dim ZHD_GebDat As DateTime? = customerWosData.ZHD_GebDat
				Dim TransferedUser As String = String.Empty

				Dim US_Nachname As String = customerWosData.UserName
				Dim US_Vorname As String = customerWosData.UserVorname
				Dim US_Telefon As String = customerWosData.UserTelefon
				Dim US_Telefax As String = customerWosData.UserTelefax
				Dim US_eMail As String = customerWosData.UserMail

				Dim MD_Telefon As String = customerWosData.MDTelefon
				Dim MD_DTelefon As String = customerWosData.MD_DTelefon
				Dim MD_Telefax As String = customerWosData.MDTelefax
				Dim MD_eMail As String = customerWosData.MDMail
				If String.IsNullOrWhiteSpace(MD_Telefon) Then MD_Telefon = US_Telefon
				If String.IsNullOrWhiteSpace(MD_DTelefon) Then MD_DTelefon = US_Telefon
				If String.IsNullOrWhiteSpace(MD_Telefax) Then MD_Telefax = US_Telefax
				If String.IsNullOrWhiteSpace(MD_eMail) Then MD_eMail = US_eMail

				Dim KD_Language As String = customerWosData.KD_Language
				Dim ZHD_EMail As String = customerWosData.ZHD_EMail
				Dim DocFilename As String = customerWosData.ScanDocName
				Dim DocScan As Byte() = customerWosData.ScanDoc

				Dim User_Initial As String = customerWosData.UserInitial
				Dim User_Sex As String = customerWosData.UserSex
				Dim User_Filiale As String = customerWosData.UserFiliale
				Dim UserSign As Byte() = customerWosData.UserSign
				Dim UserPicture As Byte() = customerWosData.UserPicture


				Dim listOfParams As New List(Of SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))

				listOfParams.Add(New SqlClient.SqlParameter("MANr", ReplaceMissing(mANr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KDNr", ReplaceMissing(kdNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHDNr", ReplaceMissing(ZHDNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ESNr", ReplaceMissing(ESNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ESLohnNr", ReplaceMissing(ESLohnNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("RPNr", ReplaceMissing(RPNr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("RENr", ReplaceMissing(RENr, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ProposeNr", ReplaceMissing(proposeNr, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("LogedUser_Guid", ReplaceMissing(LogedUser_Guid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("KD_Name", ReplaceMissing(KD_Name, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Vorname", ReplaceMissing(ZHD_Vorname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Nachname", ReplaceMissing(ZHD_Nachname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Filiale", ReplaceMissing(KD_Filiale, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Postfach", ReplaceMissing(KD_Postfach, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Strasse", ReplaceMissing(KD_Strasse, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("KD_PLZ", ReplaceMissing(KD_PLZ, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Kanton", ReplaceMissing(KD_Kanton, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("KD_Ort", ReplaceMissing(KD_Ort, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_AGB_Wos", ReplaceMissing(KD_AGB_Wos, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHDSex", ReplaceMissing(ZHDSex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Briefanrede", ReplaceMissing(ZHD_Briefanrede, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("DoNotShowContractInWOS", ReplaceMissing(DoNotShowContractInWOS, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_EMail", ReplaceMissing(KD_EMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Guid", ReplaceMissing(KD_Guid, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Guid", ReplaceMissing(ZHD_Guid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Doc_Guid", ReplaceMissing(Doc_Guid, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Doc_Art", ReplaceMissing(Doc_Art, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Doc_Info", ReplaceMissing(Doc_Info, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("Result", ReplaceMissing(Result, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Berater", ReplaceMissing(KD_Berater, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Berater", ReplaceMissing(ZHD_Berater, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Beruf", ReplaceMissing(KD_Beruf, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("KD_Branche", ReplaceMissing(KD_Branche, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Beruf", ReplaceMissing(ZHD_Beruf, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_Branche", ReplaceMissing(ZHD_Branche, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("ZHD_AGB_WOS", ReplaceMissing(ZHD_AGB_WOS, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("TransferedUser", ReplaceMissing(TransferedUser, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Nachname", ReplaceMissing(US_Nachname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Vorname", ReplaceMissing(US_Vorname, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Telefon", ReplaceMissing(MD_Telefon, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_Telefax", ReplaceMissing(MD_Telefax, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("US_eMail", ReplaceMissing(MD_eMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("KD_Language", ReplaceMissing(KD_Language, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("ZHD_EMail", ReplaceMissing(ZHD_EMail, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("DocFilename", ReplaceMissing(DocFilename, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("DocScan", ReplaceMissing(DocScan, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("Customer_Name", ReplaceMissing(MDName, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Initial", ReplaceMissing(User_Initial, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Sex", ReplaceMissing(User_Sex, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Filiale", ReplaceMissing(User_Filiale, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Picture", ReplaceMissing(UserPicture, DBNull.Value)))
				listOfParams.Add(New SqlClient.SqlParameter("User_Sign", ReplaceMissing(UserSign, DBNull.Value)))

				listOfParams.Add(New SqlClient.SqlParameter("SignTransferedDocument", ReplaceMissing(customerWosData.SignTransferedDocument.GetValueOrDefault(False), False)))

				' New ID of ZG
				Dim newIdParameter = New SqlClient.SqlParameter("@NewId", SqlDbType.Int)
				newIdParameter.Direction = ParameterDirection.Output
				listOfParams.Add(newIdParameter)

				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)

				If success AndAlso
					Not newIdParameter.Value Is Nothing Then
					customerWosData.ID = CType(newIdParameter.Value, Integer)
				Else
					success = False
				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "AddWOSCustomerDocumentData", .MessageContent = msgContent})

				Return False
			End Try


			Return success
		End Function

		Function LoadAssignedCustomerWOSData(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal modulNumber As Integer) As IEnumerable(Of CustomerWOSDataDTO) Implements IWOSDatabaseAccess.LoadAssignedCustomerWOSData
			Dim listOfSearchResultDTO As List(Of CustomerWOSDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID


			Dim sql As String
			sql = "[Load Assigned Customer Data From WOS]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerNumber", ReplaceMissing(modulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("modulGuid", ReplaceMissing(modulGuid, DBNull.Value)))

			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then
					listOfSearchResultDTO = New List(Of CustomerWOSDataDTO)

					While reader.Read
						Dim data = New CustomerWOSDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.WOS_Guid = SafeGetString(reader, "WOS_Guid")
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.ProposeNr = SafeGetInteger(reader, "ProposeNr", 0)
						data.FK_StateID = SafeGetInteger(reader, "FK_StateID", 0)

						data.DocGuid = SafeGetString(reader, "Doc_Guid")
						data.DocumentArt = SafeGetString(reader, "Doc_Art")
						data.DocumentInfo = SafeGetString(reader, "Doc_Info")
						data.GetResult = SafeGetInteger(reader, "GetResult", Nothing)
						data.Get_On = SafeGetDateTime(reader, "Get_On", Nothing)
						data.Viewed_On = SafeGetDateTime(reader, "Viewed_On", Nothing)
						data.ViewedResult = SafeGetInteger(reader, "ViewedResult", Nothing)
						data.TransferedOn = SafeGetDateTime(reader, "Transfered_On", Nothing)

						data.CustomerFeedback = SafeGetString(reader, "Customer_Feedback")
						data.CustomerFeedback_On = SafeGetDateTime(reader, "Customer_Feedback_On", Nothing)
						data.NotifyAdvisor = SafeGetBoolean(reader, "NotifyAdvisor", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCustomerWOSData", .MessageContent = msgContent})
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

		Function LoadAssignedCustomerWOSDataByDocArt(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal modulNumber As Integer, ByVal modulDocArt As Integer) As IEnumerable(Of CustomerWOSDataDTO) Implements IWOSDatabaseAccess.LoadAssignedCustomerWOSDataByDocArt
			Dim listOfSearchResultDTO As List(Of CustomerWOSDataDTO) = Nothing
			Dim excludeCheckInteger As Integer = 1
			m_customerID = customerID

			Dim docArtName As String = String.Empty
			Select Case modulDocArt
				Case WOSDocumentArt.Rechnung
					docArtName = "Rechnung"
				Case WOSDocumentArt.Verleihvertrag
					docArtName = "Verleihvertrag"
				Case WOSDocumentArt.Vorschlag
					docArtName = "Vorschlag"
				Case WOSDocumentArt.Rapport
					docArtName = "Rapport"

				Case Else

			End Select

			Dim sql As String
			sql = "[Load Assigned Customer Data From WOS By DocArt]"

			Dim listOfParams As New List(Of SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
			listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
			listOfParams.Add(New SqlClient.SqlParameter("CustomerNumber", ReplaceMissing(modulNumber, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("modulGuid", ReplaceMissing(modulGuid, DBNull.Value)))
			listOfParams.Add(New SqlClient.SqlParameter("modulArtName", ReplaceMissing(docArtName, DBNull.Value)))


			Dim reader = OpenReader(sql, listOfParams, CommandType.StoredProcedure)

			Try
				If reader IsNot Nothing Then
					listOfSearchResultDTO = New List(Of CustomerWOSDataDTO)

					While reader.Read
						Dim data = New CustomerWOSDataDTO

						data.ID = SafeGetInteger(reader, "ID", 0)
						data.WOS_Guid = SafeGetString(reader, "WOS_Guid")
						data.Customer_ID = SafeGetString(reader, "Customer_ID")
						data.ProposeNr = SafeGetInteger(reader, "ProposeNr", 0)
						data.FK_StateID = SafeGetInteger(reader, "FK_StateID", 0)

						data.DocGuid = SafeGetString(reader, "Doc_Guid")
						data.DocumentArt = SafeGetString(reader, "Doc_Art")
						data.DocumentInfo = SafeGetString(reader, "Doc_Info")
						data.GetResult = SafeGetInteger(reader, "GetResult", Nothing)
						data.Get_On = SafeGetDateTime(reader, "Get_On", Nothing)
						data.Viewed_On = SafeGetDateTime(reader, "Viewed_On", Nothing)
						data.ViewedResult = SafeGetInteger(reader, "ViewedResult", Nothing)
						data.TransferedOn = SafeGetDateTime(reader, "Transfered_On", Nothing)
						data.CustomerFeedback = SafeGetString(reader, "Customer_Feedback")
						data.CustomerFeedback_On = SafeGetDateTime(reader, "Customer_Feedback_On", Nothing)
						data.NotifyAdvisor = SafeGetBoolean(reader, "NotifyAdvisor", False)


						listOfSearchResultDTO.Add(data)

					End While

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedCustomerWOSDataByDocArt", .MessageContent = msgContent})
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

		Function UpdateAssignedDocNotificationAsDone(ByVal customerID As String, ByVal customerWosGuid As String, ByVal modulGuid As String, ByVal recID As Integer) As Boolean Implements IWOSDatabaseAccess.UpdateAssignedDocNotificationAsDone
			Dim success As Boolean = True
			m_customerID = customerID

			Try
				Dim sql As String
				sql = "[Set Propose Notification To Done]"

				Dim listOfParams As New List(Of SqlClient.SqlParameter)

				listOfParams.Add(New SqlClient.SqlParameter("Customer_ID", ReplaceMissing(customerID, DBNull.Value)))
				listOfParams.Add(New SqlParameter("WOSGuid", customerWosGuid))
				listOfParams.Add(New SqlClient.SqlParameter("recID", ReplaceMissing(recID, DBNull.Value)))


				success = ExecuteNonQuery(sql, listOfParams, CommandType.StoredProcedure, False)



			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "UpdateAssignedDocNotificationAsDone", .MessageContent = msgContent})
			Finally
			End Try

			Return success
		End Function


	End Class


End Namespace
