

<Serializable()>
Public Class CustomerSearchResultDTO

	Public Property customer_Name As String
	Public Property VakGuid As String
	Public Property employeeGuid As String
	Public Property customer_ID As String

End Class

<Serializable()>
Public Class CustomerUserNameSearchResultDTO

	Public Property UserName As String

End Class


<Serializable()>
Public Class PaymentSearchResultDTO

	Public Property RecID As Integer
	Public Property CustomerGuid As String
	Public Property UserGuid As String
	Public Property ServiceName As String
	Public Property ServiceDate As DateTime?
	Public Property CreatedOn As DateTime?
	Public Property CreatedFrom As String
	Public Property AuthorizedItems As Decimal
	Public Property AuthorizedCredit As Decimal
	Public Property JobID As String
	Public Property Validated As Boolean

	Public Property BookedPayment As Boolean?
	Public Property BookedDate As DateTime?

End Class


<Serializable()>
Public Class PaidSearchResultDTO

	Public Property RecID As Integer
	Public Property ServiceDate As DateTime?
	Public Property CustomerGuid As String
	Public Property Content As String
	Public Property ServiceName As String
	Public Property Recipient As String
	Public Property AuthorizedItems As Decimal
	Public Property Status As Integer
	Public Property Sender As String
	Public Property UserData As String
	Public Property BookedPayment As Boolean?
	Public Property BookedDate As DateTime?

End Class
