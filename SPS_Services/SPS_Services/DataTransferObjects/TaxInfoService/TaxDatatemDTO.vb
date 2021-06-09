
Namespace DataTransferObjects.TaxInfoService

  <Serializable()>
  Public Class TaxDataItemDTO
    Public Property Kanton As String
    Public Property Gruppe As String
    Public Property Kinder As Short
    Public Property Kirchensteuer As String
  End Class

	<Serializable()>
	Public Class TaxCodeDataDTO

		Public Property ID As Integer
		Public Property Rec_Value As String
		Public Property Translated_Value As String

	End Class

	<Serializable()>
	Public Class TaxChurchCodeDataDTO

		Public Property ID As Integer
		Public Property Rec_Value As String
		Public Property Translated_Value As String

	End Class

End Namespace
