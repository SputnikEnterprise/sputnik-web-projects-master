'---------------------------------------------------------
' DocumentSQLStrings.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic

' Document retrieval SQL string wrapper
Public Class DocumentSQLStrings

	Public Property MySettingsCustomerDataSQL As String
	Public Property WelcomeMessageSQL As String
	Public Property LoadDocumentInformationSQL As String
	Public Property LoadDocumentForEmailForwardSQL As String
  Public Property LoadDocScanSQL As String
  Public Property UpdateDocumentView As String

End Class


'' SQL query from document retrieval
'Dim _MySettingsCustomerDataSQL As String
'   Public Property MySettingsCustomerDataSQL() As String
'       Get
'           Return _MySettingsCustomerDataSQL
'       End Get

'       Set(ByVal value As String)
'           _MySettingsCustomerDataSQL = value
'       End Set
'   End Property

'   ' SQL query from document retrieval
'   Dim _WelcomeMessageSQL As String
'   Public Property WelcomeMessageSQL() As String
'       Get
'           Return _WelcomeMessageSQL
'       End Get

'       Set(ByVal value As String)
'           _WelcomeMessageSQL = value
'       End Set
'   End Property

'   ' SQL query from document retrieval
'   Dim _LoadDocumentInformationSQL As String
'   Public Property LoadDocumentInformationSQL() As String
'       Get
'           Return _LoadDocumentInformationSQL
'       End Get

'       Set(ByVal value As String)
'           _LoadDocumentInformationSQL = value
'       End Set
'   End Property

'   ' SQL query for email forward
'   Dim _LoadDocumentForEmailForwardSQL As String
'   Public Property LoadDocumentForEmailForwardSQL() As String
'       Get
'           Return _LoadDocumentForEmailForwardSQL
'       End Get

'       Set(ByVal value As String)
'           _LoadDocumentForEmailForwardSQL = value
'       End Set
'   End Property

'   ' SQL query from document retrieval
'   Dim _LoadDocScanSQL As String
'   Public Property LoadDocScanSQL() As String
'       Get
'           Return _LoadDocScanSQL
'       End Get

'       Set(ByVal value As String)
'           _LoadDocScanSQL = value
'       End Set
'   End Property

