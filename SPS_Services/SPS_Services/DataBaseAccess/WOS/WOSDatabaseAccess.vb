
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports wsSPS_Services.SPUtilities
Imports System.Text
Imports wsSPS_Services.Logging

Namespace WOSInfo


	Public Class WOSDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IWOSDatabaseAccess

		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()

		Private m_utility As ClsUtilities
		Private m_customerID As String
		Private Const ASMX_SERVICE_NAME As String = ",WOSInfo"


#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)
			m_utility = New ClsUtilities

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
			m_utility = New ClsUtilities
		End Sub

#End Region

		Function LoadAssignedWOSOwnerMasterData(ByVal customer_ID As String, ByVal WOSEnum As WOSModulData.ModulArt, ByVal withDocData As Boolean) As WOSOwnerData Implements IWOSDatabaseAccess.LoadAssignedWOSOwnerMasterData
			Dim result As WOSOwnerData = Nothing
			If String.IsNullOrWhiteSpace(customer_ID) Then Return Nothing

			Dim fieldName As String = "MA_Guid"
			Select Case WOSEnum
				Case WOSModulData.ModulArt.CustomerDocument
					fieldName = "KD_Guid"

				Case WOSModulData.ModulArt.EmployeeDocument
					fieldName = "MA_Guid"

				Case WOSModulData.ModulArt.VacancyDodument
					fieldName = "Vak_Guid"

				Case Else
					Return Nothing

			End Select

			Dim sql As String

			sql = "SELECT Top 1 ID"
			sql &= ",Customer_ID"
			sql &= ",WOS_Guid"
			sql &= ",Userkey"
			sql &= ",Passwort"
			sql &= ",eMail"
			sql &= ",Customer_Name"
			sql &= ",KD_ZHD"
			sql &= ",ModulName"
			sql &= ",Vak_Guid"
			sql &= ",MA_Guid"
			sql &= ",KD_Guid"
			sql &= ",DP_Guid"
			sql &= ",Verleih_Guid"
			sql &= ",Customer_Ort"
			sql &= ",Customer_Telefon"
			sql &= ",Customer_Telefax"
			sql &= ",Customer_eMail"
			sql &= ",Customer_Strasse"
			sql &= ",Customer_Homepage"
			sql &= ",Customer_cssFile"
			sql &= ",CONVERT(INT, PrintVerleih) PrintVerleih"

			If withDocData Then
				sql &= ",Customer_Logo"
				sql &= ",Customer_AGB"
				sql &= ",Customer_AGBFest"
				sql &= ",Customer_AGBSonst"
				sql &= ",Customer_AGBFest_I"
				sql &= ",Customer_AGBSonst_I"
				sql &= ",Customer_AGBFest_F"
				sql &= ",Customer_AGBSonst_F"
				sql &= ",Customer_AGBFest_E"
				sql &= ",Customer_AGBSonst_E"
				sql &= ",Customer_AGB_I"
				sql &= ",Customer_AGB_F"
				sql &= ",Customer_AGB_E"
				sql &= ",Rahmenvertrag"
				sql &= ",Rahmenvertrag_I"
				sql &= ",Rahmenvertrag_F"
				sql &= ",Rahmenvertrag_E"
			End If

			sql &= ",CONVERT(INT, AutoNotification) AutoNotification"
			sql &= ",Visible_Candidate_Fields"
			sql &= ",Visible_Vacancy_Fields"
			sql &= ",Autonotification_MA"
			sql &= ",Autonotification_KD"
			sql &= ",GAVUnia"
			sql &= ",TplFilename "
			sql &= "FROM dbo.MySetting "

			sql &= "WHERE WOS_Guid = @customer_ID"

			'sql = String.Format(sql, fieldName)

			Dim listOfParams As New List(Of SqlClient.SqlParameter)

			listOfParams.Add(New SqlClient.SqlParameter("customer_ID", customer_ID))


			Dim reader = OpenReader(sql, listOfParams, CommandType.Text)
			result = New WOSOwnerData

			Try
				If reader IsNot Nothing AndAlso reader.Read() Then
					Dim data = New WOSOwnerData

					data.ID = SafeGetInteger(reader, "ID", 0)
					data.Customer_ID = SafeGetString(reader, "Customer_ID")
					data.WOS_Guid = SafeGetString(reader, "WOS_Guid")
					data.Userkey = SafeGetString(reader, "Userkey")
					data.Passwort = SafeGetString(reader, "Passwort")
					data.Customer_Name = SafeGetString(reader, "Customer_Name")
					data.KD_ZHD = SafeGetString(reader, "KD_ZHD")

					data.ModulName = SafeGetString(reader, "ModulName")
					data.Vak_Guid = SafeGetString(reader, "Vak_Guid")
					data.MA_Guid = SafeGetString(reader, "MA_Guid")
					data.KD_Guid = SafeGetString(reader, "KD_Guid")
					data.DP_Guid = SafeGetString(reader, "DP_Guid")
					data.Verleih_Guid = SafeGetString(reader, "Verleih_Guid")

					data.Customer_Strasse = SafeGetString(reader, "Customer_Strasse")
					data.Customer_Ort = SafeGetString(reader, "Customer_Ort")
					data.Customer_Telefon = SafeGetString(reader, "Customer_Telefon")
					data.Customer_Telefax = SafeGetString(reader, "Customer_Telefax")
					data.Customer_eMail = SafeGetString(reader, "Customer_eMail")
					data.Customer_Homepage = SafeGetString(reader, "Customer_Homepage")
					data.PrintVerleih = SafeGetInteger(reader, "PrintVerleih", 0)

					If withDocData Then
						data.Customer_Logo = SafeGetByteArray(reader, "Customer_Logo")
						data.Customer_AGB = SafeGetByteArray(reader, "Customer_AGB")
						data.Customer_cssFile = SafeGetString(reader, "Customer_cssFile")
						data.Customer_AGBFest = SafeGetByteArray(reader, "Customer_AGBFest")
						data.Customer_AGBSonst = SafeGetByteArray(reader, "Customer_AGBSonst")
						data.Customer_AGBFest_I = SafeGetByteArray(reader, "Customer_AGBFest_I")
						data.Customer_AGBSonst_I = SafeGetByteArray(reader, "Customer_AGBSonst_I")
						data.Customer_AGBFest_F = SafeGetByteArray(reader, "Customer_AGBFest_F")
						data.Customer_AGBSonst_F = SafeGetByteArray(reader, "Customer_AGBSonst_F")
						data.Customer_AGBFest_E = SafeGetByteArray(reader, "Customer_AGBFest_E")
						data.Customer_AGBSonst_E = SafeGetByteArray(reader, "Customer_AGBSonst_E")
						data.Customer_AGB_I = SafeGetByteArray(reader, "Customer_AGB_I")
						data.Customer_AGB_F = SafeGetByteArray(reader, "Customer_AGB_F")
						data.Customer_AGB_E = SafeGetByteArray(reader, "Customer_AGB_E")
						data.Rahmenvertrag = SafeGetByteArray(reader, "Rahmenvertrag")
						data.Rahmenvertrag_I = SafeGetByteArray(reader, "Rahmenvertrag_I")
						data.Rahmenvertrag_F = SafeGetByteArray(reader, "Rahmenvertrag_F")
						data.Rahmenvertrag_E = SafeGetByteArray(reader, "Rahmenvertrag_E")
					End If

					data.AutoNotification = SafeGetInteger(reader, "AutoNotification", 0)
					data.Visible_Candidate_Fields = SafeGetString(reader, "Visible_Candidate_Fields")
					data.Visible_Vacancy_Fields = SafeGetString(reader, "Visible_Vacancy_Fields")
					data.Autonotification_MA = SafeGetBoolean(reader, "Autonotification_MA", False)
					data.Autonotification_KD = SafeGetBoolean(reader, "Autonotification_KD", False)
					data.GAVUnia = SafeGetString(reader, "GAVUnia")
					data.TplFilename = SafeGetString(reader, "TplFilename")


					result = data

				End If


			Catch ex As Exception
				Dim msgContent = ex.ToString
				m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadAssignedWOSOwnerMasterData", .MessageContent = msgContent})
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
			Return result
		End Function


	End Class


End Namespace
