Namespace DataTransferObjects.TaxInfoService

    <Serializable()>
    Public Class QstDataDTO

        Public Property Mindest_Abzug As Decimal
        Public Property Steuer_Proz As Decimal
		Public Property Steuer_Fr As Decimal
		Public Property Einkommen As Decimal
		Public Property Schritt As Decimal

    End Class

	<Serializable()>
	Public Class CommunityDataDTO

		Public Property ID As Integer?
		Public Property HistoricNumber As Integer?
		Public Property Canton As String
		Public Property BezirkNumber As Integer?
		Public Property BezirkName As String
		Public Property BFSNumber As Integer?
		Public Property Translated_Value As String

	End Class

	<Serializable()>
	Public Class EmploymentTypeDataDTO

		Public Property ID As Integer
		Public Property Rec_Value As String
		Public Property Translated_Value As String

	End Class

	<Serializable()>
	Public Class TypeOfStayDataDTO

		Public Property ID As Integer
		Public Property Rec_Value As String
		Public Property Translated_Value As String

	End Class

	<Serializable()>
	Public Class PermissionDataDTO

		Public Property ID As Integer
		Public Property RecNr As Integer
		Public Property Rec_Value As String
		Public Property Code As String
		Public Property Translated_Value As String

	End Class

	<Serializable()>
 Public Class ChildEducationDataDTO

		Public Property ID As Integer
		Public Property MDYear As Integer
		Public Property FAK_Kanton As String
		Public Property Fak_Name As String
		Public Property Fak_ZHD As String
		Public Property Fak_Postfach As String
		Public Property Fak_Strasse As String
		Public Property Fak_PLZOrt As String
		Public Property YMinLohn As Decimal?
		Public Property Ki1_Month As Decimal?
		Public Property Ki2_Month As Decimal?
		Public Property Au1_Month As Decimal?
		Public Property Ki1_Std As Decimal?
		Public Property Ki2_Std As Decimal?
		Public Property Ki1_Day As Decimal?
		Public Property Ki2_Day As Decimal?
		Public Property ChangeKiIn As String
		Public Property ChangeKiIn_2 As String
		Public Property Au1_Std As Decimal?
		Public Property Au2_Std As Decimal?
		Public Property Au1_Day As Decimal?
		Public Property Au2_Day As Decimal?
		Public Property ChangeAuIn As String
		Public Property ChangeAuIn_2 As String

		Public Property CreatedOn As DateTime?
		Public Property CreatedFrom As String
		Public Property ChangedOn As DateTime?
		Public Property ChangedFrom As String

		Public Property Ki1_FakMax As Decimal?
		Public Property Ki2_FakMax As Decimal?
		Public Property Fak_Proz As Decimal?
		Public Property Geb_Zulage As Decimal?
		Public Property Ado_Zulage As Decimal?

		Public Property Bemerkung_1 As String
		Public Property Bemerkung_2 As String
		Public Property Bemerkung_3 As String
		Public Property Bemerkung_4 As String

	End Class

End Namespace
