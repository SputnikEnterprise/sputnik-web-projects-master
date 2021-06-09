
<Serializable()>
Public Class KDVakanzenDTO
  Public Property RecID As Integer?
  Public Property VakNr As Integer?
  Public Property KDNr As Integer?
  Public Property KDZHDNr As Integer?
  Public Property Customer_ID As String
  Public Property Customer_Name As String
  Public Property Customer_Strasse As String
  Public Property Customer_Ort As String
  Public Property Customer_Telefon As String
  Public Property Customer_eMail As String
  Public Property Berater As String
  Public Property Filiale As String
  Public Property VakKontakt As String
  Public Property VakState As String
  Public Property Bezeichnung As String

  Public Property TitelForSearch As String
  Public Property ShortDescription As String

  Public Property Slogan As String
  Public Property Gruppe As String
	Public Property SubGroup As String
	Public Property ExistLink As Boolean?
	Public Property VakLink As String
  Public Property Beginn As String
  Public Property JobProzent As String
  Public Property Anstellung As String
  Public Property Dauer As String
  Public Property MAAge As String
  Public Property MASex As String
  Public Property MAZivil As String
  Public Property MALohn As String
  Public Property Jobtime As String
  Public Property JobOrt As String
  Public Property MAFSchein As String
  Public Property MAAuto As String
  Public Property MANationality As String
  Public Property IEExport As Boolean?

  Public Property KDBeschreibung As String
  Public Property KDBietet As String
  Public Property SBeschreibung As String
  Public Property Reserve1 As String
  Public Property Taetigkeit As String
  Public Property Anforderung As String
  Public Property Reserve2 As String
  Public Property Reserve3 As String
  Public Property Ausbildung As String
  Public Property Weiterbildung As String
  Public Property SKennt As String
  Public Property EDVKennt As String

  Public Property html_KDBeschreibung As String
  Public Property html_KDBietet As String
  Public Property html_SBeschreibung As String
  Public Property html_Reserve1 As String
  Public Property html_Taetigkeit As String
  Public Property html_Anforderung As String
  Public Property html_Reserve2 As String
  Public Property html_Reserve3 As String
  Public Property html_Ausbildung As String
  Public Property html_Weiterbildung As String
  Public Property html_SKennt As String
  Public Property html_EDVKennt As String


  Public Property Branchen As String
  Public Property CreatedOn As DateTime?
  Public Property CreatedFrom As String
  Public Property ChangedOn As DateTime?
  Public Property ChangedFrom As String
  Public Property Transfered_User As String
  Public Property Transfered_On As DateTime?
  Public Property Transfered_Guid As String
  Public Property Result As String
  Public Property Vak_Region As String
  Public Property Vak_Kanton As String
  Public Property User_Nachname_OLD As String
  Public Property User_Vorname_OLD As String
  Public Property User_Telefon_OLD As String
  Public Property User_Telefax_OLD As String
  Public Property User_eMail_OLD As String
  Public Property MSprachen As String
  Public Property SSprachen As String
  Public Property Qualifikation As String
  Public Property SQualifikation As String
  Public Property User_Guid As String

  Public Property JobPLZ As String
  Public Property Job_Categories As String
  Public Property Job_Disciplines As String
  Public Property Job_Position As String


	Public Property Job_Disciplines_Match As String
	Public Property Vak_Region_Match As String


  Public Property User_salutation As String
  Public Property User_Firstname As String
  Public Property User_Lastname As String
  Public Property User_EMail As String
  Public Property User_Telefon As String
  Public Property User_Picture As Byte()
	Public Property IsJobsCHOnline As Boolean?
	Public Property IsOstJobOnline As Boolean?
	Public Property JobChannelPriority As Boolean?

End Class

