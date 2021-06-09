
Public Class WOSModulData

	Public Enum ModulArt
		EmployeeDocument
		CustomerDocument
		VacancyDodument
	End Enum

	Public Property WOSModulEnum As ModulArt

End Class

Public Enum WOSDocumentArt

	Arbeitgeberbescheinigung
	Zwischenverdienstformular
	Einsatzvertrag
	Rapport
	Lohnabrechnung
	Lohnausweis

	Verleihvertrag
	Rechnung
	Vorschlag

End Enum


Public Class WOSOwnerData
	Public Property ID As Integer?
	Public Property Customer_ID As String
	Public Property WOS_Guid As String
	Public Property Userkey As String
	Public Property Passwort As String
	Public Property eMail As String
	Public Property Customer_Name As String
	Public Property KD_ZHD As String
	Public Property ModulName As String
	Public Property Vak_Guid As String
	Public Property MA_Guid As String
	Public Property KD_Guid As String
	Public Property DP_Guid As String
	Public Property Verleih_Guid As String
	Public Property Customer_Logo As Byte()
	Public Property Customer_Ort As String
	Public Property Customer_Telefon As String
	Public Property Customer_Telefax As String
	Public Property Customer_eMail As String
	Public Property Customer_AGB As Byte()
	Public Property Customer_Strasse As String
	Public Property Customer_Homepage As String
	Public Property Customer_cssFile As String
	Public Property PrintVerleih As Integer?
	Public Property Customer_AGBFest As Byte()
	Public Property Customer_AGBSonst As Byte()
	Public Property Customer_AGBFest_I As Byte()
	Public Property Customer_AGBSonst_I As Byte()
	Public Property Customer_AGBFest_F As Byte()
	Public Property Customer_AGBSonst_F As Byte()
	Public Property Customer_AGBFest_E As Byte()
	Public Property Customer_AGBSonst_E As Byte()
	Public Property Customer_AGB_I As Byte()
	Public Property Customer_AGB_F As Byte()
	Public Property Customer_AGB_E As Byte()
	Public Property Rahmenvertrag As Byte()
	Public Property Rahmenvertrag_I As Byte()
	Public Property Rahmenvertrag_F As Byte()
	Public Property Rahmenvertrag_E As Byte()
	Public Property AutoNotification As Integer?
	Public Property Visible_Candidate_Fields As String
	Public Property Visible_Vacancy_Fields As String
	Public Property Autonotification_MA As Boolean?
	Public Property Autonotification_KD As Boolean?
	Public Property GAVUnia As String
	Public Property TplFilename As String

End Class


<Serializable()>
Public Class CustomerWOSData

	Public Property CustomerWOSID As String

	Public Property ID As Integer?
	Public Property EmployeeNumber As Integer?
	Public Property CustomerNumber As Integer?
	Public Property CresponsibleNumber As Integer?
	Public Property EmploymentNumber As Integer?
	Public Property EmploymentLineNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property InvoiceNumber As Integer?
	Public Property ProposeNumber As Integer?

	Public Property KDTransferedGuid As String
	Public Property ZHDTransferedGuid As String
	Public Property ESDoc_Guid As String
	Public Property RPDoc_Guid As String
	Public Property REDoc_Guid As String
	Public Property CustomerMail As String
	Public Property customername As String
	Public Property CustomerStrasse As String
	Public Property CustomerOrt As String
	Public Property CustomerPLZ As String
	Public Property CustomerTelefon As String
	Public Property CustomerTelefax As String
	Public Property CustomerHomepage As String
	Public Property UserAnrede As String
	Public Property UserVorname As String
	Public Property UserName As String
	Public Property UserTelefon As String
	Public Property UserTelefax As String
	Public Property UserMail As String
	Public Property UserInitial As String
	Public Property UserSex As String
	Public Property UserFiliale As String
	Public Property UserSign As Byte()
	Public Property UserPicture As Byte()
	Public Property LogedUserID As String

	Public Property MDTelefon As String
	Public Property MD_DTelefon As String
	Public Property MDTelefax As String
	Public Property MDMail As String

	Public Property KD_Name As String
	Public Property KD_Postfach As String
	Public Property KD_Strasse As String
	Public Property KD_PLZ As String
	Public Property KD_Ort As String
	Public Property KD_Land As String
	Public Property KD_Filiale As String
	Public Property KD_Berater As String
	Public Property KD_Email As String
	Public Property KD_AGB_Wos As String
	Public Property KD_Beruf As String
	Public Property KD_Branche As String
	Public Property KD_Language As String
	Public Property DoNotShowContractInWOS As Boolean?

	Public Property ZHD_Vorname As String
	Public Property ZHD_Nachname As String
	Public Property ZHD_EMail As String
	Public Property ZHDSex As String
	Public Property Zhd_BriefAnrede As String
	Public Property Zhd_Berater As String
	Public Property Zhd_Beruf As String
	Public Property Zhd_Branche As String
	Public Property ZHD_AGB_Wos As String
	Public Property ZHD_GebDat As DateTime?

	Public Property AssignedDocumentGuid As String
	Public Property AssignedDocumentArtName As String
	Public Property AssignedDocumentInfo As String
	Public Property ScanDoc As Byte()
	Public Property ScanDocName As String
	Public Property SignTransferedDocument As Boolean?

End Class


<Serializable()>
Public Class EmployeeWOSData

	Public Property EmployeeWOSID As String
	Public Property ID As Integer?

	Public Property EmployeeNumber As Integer?
	Public Property EmploymentNumber As Integer?
	Public Property EmploymentLineNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property ReportLineNumber As Integer?
	Public Property ReportDocNumber As Integer?
	Public Property PayrollNumber As Integer?

	Public Property MATransferedGuid As String
	Public Property ESDoc_Guid As String
	Public Property RPDoc_Guid As String
	Public Property PayrollDoc_Guid As String
	Public Property UserAnrede As String
	Public Property UserVorname As String
	Public Property UserName As String
	Public Property UserTelefon As String
	Public Property UserTelefax As String
	Public Property UserMail As String
	Public Property UserInitial As String
	Public Property UserSex As String
	Public Property UserFiliale As String
	Public Property UserSign As Byte()
	Public Property UserPicture As Byte()
	Public Property LogedUserID As String

	Public Property MDTelefon As String
	Public Property MD_DTelefon As String
	Public Property MDTelefax As String
	Public Property MDMail As String

	Public Property MA_Nachname As String
	Public Property MA_Vorname As String
	Public Property MA_Postfach As String
	Public Property MA_Strasse As String
	Public Property MA_PLZ As String
	Public Property MA_Ort As String
	Public Property MA_Land As String
	Public Property MA_Filiale As String
	Public Property MA_Berater As String
	Public Property MA_Email As String
	Public Property MA_AGB_Wos As String
	Public Property MA_Beruf As String
	Public Property MA_Branche As String
	Public Property MA_Language As String
	Public Property MA_GebDat As DateTime?
	Public Property MA_Gender As String
	Public Property MA_BriefAnrede As String
	Public Property MA_Zivil As String
	Public Property MA_Nationality As String

	Public Property MA_FSchein As String
	Public Property MA_Auto As String
	Public Property MA_Kontakt As String
	Public Property MA_State1 As String
	Public Property MA_State2 As String
	Public Property MA_Eigenschaft As String
	Public Property MA_SSprache As String
	Public Property MA_MSprache As String

	Public Property AHV_Nr As String
	Public Property MA_Canton As String

	Public Property AssignedDocumentGuid As String
	Public Property AssignedDocumentArtName As String
	Public Property AssignedDocumentInfo As String
	Public Property ScanDoc As Byte()
	Public Property ScanDocName As String
	Public Property SignTransferedDocument As Boolean?


End Class


Public Class WOSUserData

	Public Property USNr As Integer?
	Public Property MDName As String
	Public Property MDName2 As String
	Public Property MDName3 As String
	Public Property MDPostfach As String
	Public Property MDStrasse As String
	Public Property MDPLZ As String
	Public Property MDOrt As String
	Public Property MDLand As String
	Public Property MD_Kanton As String
	Public Property MD_Telefon As String
	Public Property MD_Telefax As String
	Public Property MDeMail As String
	Public Property MDHomepage As String
	Public Property USAnrede As String
	Public Property USVorname As String
	Public Property USNachname As String
	Public Property USTelefon As String
	Public Property USTelefax As String
	Public Property USTitel_1 As String
	Public Property USTitel_2 As String
	Public Property USLanguage As String
	Public Property USFiliale As String
	Public Property KST As String
	Public Property USGuid As String
	Public Property USeMail As String
	Public Property USNatel As String
	Public Property UserMDGuid As String
	Public Property UserPicture As Byte()
	Public Property UserSign As Byte()


End Class


<Serializable()>
Public Class CustomerWOSDataDTO

	Public Property ID As Integer
	Public Property WOS_Guid As String
	Public Property Customer_ID As String
	Public Property ZHDGuid As String
	Public Property EmploymentNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property InvoiceNumber As Integer?
	Public Property CRepesponsibleNumber As Integer?
	Public Property ProposeNr As Integer?

	Public Property CustomerGuid As String
	Public Property CustomerNumber As Integer?
	Public Property CustomerName As String
	Public Property ZLastName As String
	Public Property ZFirstName As String
	Public Property DocumentArt As String
	Public Property DocumentInfo As String

	Public Property TransferedOn As DateTime?
	Public Property TransferedUser As String
	Public Property CheckedOn As DateTime?
	Public Property GetResult As Integer?
	Public Property GetOn As DateTime?
	Public Property LastNotification As DateTime?

	Public Property DocGuid As String
	Public Property ScanContent As Byte()

	Public Property FK_StateID As Integer?
	Public Property Get_On As DateTime?
	Public Property Viewed_On As DateTime?
	Public Property ViewedResult As Integer?
	Public Property CustomerFeedback_On As DateTime?
	Public Property CustomerFeedback As String
	Public Property NotifyAdvisor As Boolean

	Public ReadOnly Property CustomerWOSLink() As String
		Get
			Return String.Format("http://edoc.sputnik-it.com/sponlinedoc/DefaultPage.aspx?kd={0}", ZHDGuid)
		End Get
	End Property

	Public ReadOnly Property ResponsiblePersonFullName() As String
		Get
			Return String.Format("{0}, {1}", ZLastName, ZFirstName)
		End Get
	End Property

	Public ReadOnly Property ResponsiblePersonWOSLink() As String
		Get
			Return String.Format("http://edoc.sputnik-it.com/sponlinedoc/DefaultPage.aspx?ZHD={0}", ZHDGuid)
		End Get
	End Property

End Class


<Serializable()>
Public Class EmployeeWOSDataDTO

	Public Property ID As Integer
	Public Property Customer_ID As String
	Public Property EmployeeGuid As String
	Public Property EmploymentNumber As Integer?
	Public Property ReportNumber As Integer?
	Public Property PayrollNumber As Integer?

	Public Property EmployeeNumber As Integer?
	Public Property EmployeeLastName As String
	Public Property EmployeeFirstName As String
	Public Property DocumentArt As String
	Public Property DocumentInfo As String

	Public Property TransferedOn As DateTime?
	Public Property TransferedUser As String
	Public Property LastNotification As DateTime?

	Public Property DocGuid As String
	Public Property ScanContent As Byte()


	Public ReadOnly Property EmployeeWOSLink() As String
		Get
			Return String.Format("http://edoc.sputnik-it.com/sponlinedoc/DefaultPage.aspx?sp={0}", EmployeeGuid)
		End Get
	End Property

	Public ReadOnly Property EmployeeFullName() As String
		Get
			Return String.Format("{0}, {1}", EmployeeLastName, EmployeeFirstName)
		End Get
	End Property

End Class


