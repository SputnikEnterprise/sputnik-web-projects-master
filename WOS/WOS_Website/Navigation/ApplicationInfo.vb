'---------------------------------------------------------
' ApplicationInfo.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic

''' <summary>
''' Application Information
''' </summary>
Public Class ApplicationInfo

    ' Key to access the application information form the session
    Public Const SESSION_KEY As String = "ApplicationInfo"

  ' Query parameter name for candidates (Kandidaten)
  Dim _CandidateQueryParameterName As String = System.Configuration.ConfigurationManager.AppSettings("Candidate_QueryParameterName").ToString
  Public ReadOnly Property CandidateQueryParameterName() As String
        Get
            Return _CandidateQueryParameterName
        End Get
    End Property

  ' Query parameter name for KD's
  Dim _KDQueryParameterName As String = System.Configuration.ConfigurationManager.AppSettings("KD_QueryParameterName").ToString
  Public ReadOnly Property KDQueryParameterName() As String
        Get
            Return _KDQueryParameterName
        End Get
    End Property

  ' Query parameter name for ZHD's
  Dim _ZHDQueryParameterName As String = System.Configuration.ConfigurationManager.AppSettings("ZHD_QueryParameterName").ToString
  Public ReadOnly Property ZHDQueryParameterName() As String
        Get
            Return _ZHDQueryParameterName
        End Get
    End Property

  ' Email address which gets all emails if test mode is enabled
  Dim _EmailTestModeReceiver As String = System.Configuration.ConfigurationManager.AppSettings("EmailTestModeReceiver").ToString
  Public ReadOnly Property EmailTestModeReceiver() As String
        Get
            Return _EmailTestModeReceiver
        End Get
    End Property

  ' Boolean flag if email test mode is activated in web.config
  Dim _EmailTestModeEnabled As String = System.Configuration.ConfigurationManager.AppSettings("EmailTestModeEnabled").ToString
  Public ReadOnly Property EmailTestModeEnabled() As Boolean
        Get
            Return Convert.ToBoolean(_EmailTestModeEnabled)
        End Get
    End Property

  ' Email address which is uses as the application's email sender address. 
  Dim _EmailSenderAddress As String = System.Configuration.ConfigurationManager.AppSettings("EmailSenderAddress").ToString
  Public ReadOnly Property EmailSenderAddress() As String
        Get
            Return _EmailSenderAddress
        End Get
    End Property

    ' The name of the current GUID parameter
    Dim _CurrentGuidName As String
    Public Property CurrentGuidName() As String
        Get
            Return _CurrentGuidName
        End Get

        Set(ByVal value As String)
            _CurrentGuidName = value
        End Set
    End Property


    ' The current GUID value
    Dim _CurrentGuidValue As String
    Public Property CurrentGuidValue() As String
        Get
            Return _CurrentGuidValue
        End Get
        Set(ByVal value As String)
            _CurrentGuidValue = value
        End Set
    End Property

    ' The document SQL strings
    Dim _DocumentSQLStrings As DocumentSQLStrings
    Public Property DocumentSQLStrings() As DocumentSQLStrings
        Get
            Return _DocumentSQLStrings
        End Get
        Set(ByVal value As DocumentSQLStrings)
            _DocumentSQLStrings = value
        End Set
    End Property

  ' Regular expression to check a GUID
  Dim _RegexGui As String = System.Configuration.ConfigurationManager.AppSettings("RegexGui").ToString
  Public ReadOnly Property RegexGui() As String
        Get
            Return _RegexGui
        End Get
    End Property

  ' The connection string to the database.
  Dim _ConnectionString As String = System.Configuration.ConfigurationManager.AppSettings("connectionString").ToString
  Public ReadOnly Property ConnectionString() As String
        Get
            Return _ConnectionString
        End Get
    End Property

  ' The smtp-server name.
  Dim _SmtpAddress As String = System.Configuration.ConfigurationManager.AppSettings("SMTPServerAddress").ToString
  Public ReadOnly Property SmtpAddress() As String
        Get
            Return _SmtpAddress
        End Get
    End Property

	Dim _SmtpTLS As Boolean? = System.Configuration.ConfigurationManager.AppSettings("SMTPUseTLS").ToString
	Public ReadOnly Property SmtpTLS() As Boolean?
		Get
			Return _SmtpTLS
		End Get
	End Property

	' The smtp-server username.
	Dim _SmtpUser As String = System.Configuration.ConfigurationManager.AppSettings("SMTPServerUserName").ToString
  Public ReadOnly Property SmtpUser() As String
		Get
			Return _SmtpUser
		End Get
	End Property

  ' The smtp-server password.
  Dim _SmtpPassword As String = System.Configuration.ConfigurationManager.AppSettings("SMTPServerUserPassword").ToString
  Public ReadOnly Property SmtpUserPassword() As String
		Get
			Return _SmtpPassword
		End Get
	End Property

  ' The smtp-server Port.
  Dim _SmtpPort As String = System.Configuration.ConfigurationManager.AppSettings("SMTPServerPort").ToString
  Public ReadOnly Property SmtpServerPort() As String
		Get
			If String.IsNullOrWhiteSpace(_SmtpPort) Then _SmtpPort = 25
			Return _SmtpPort
		End Get
	End Property

End Class
