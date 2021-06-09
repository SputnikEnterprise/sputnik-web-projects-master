
Namespace JobPlatform.AVAM

	Public Interface iVacancyDatabaseAccess

		Function AddAVAMNotifyResultData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal jobroomID As String, ByVal queryContent As String, ByVal resultContent As String,
												 ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?, ByVal Notify As Boolean?, ByVal syncFrom As String) As Boolean
		Function AddAVAMQueryResultData(ByVal customerID As String, ByVal userid As String, ByVal vacancyNumber As Integer, ByVal jobroomID As String, ByVal queryContent As String, ByVal resultContent As String,
												 ByVal ReportingObligation As Boolean, ByVal ReportingObligationEndDate As DateTime?, ByVal syncFrom As String) As Boolean

	End Interface


End Namespace

