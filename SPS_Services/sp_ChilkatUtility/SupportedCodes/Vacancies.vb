
Imports System.Data.SqlClient
Imports sp_WebServiceUtility.DatabaseAccessBase
Imports sp_WebServiceUtility.Logging
Imports sp_WebServiceUtility.SPUtilities
Imports sp_WebServiceUtility.SystemInfo
Imports sp_WebServiceUtility.WOSInfo

Namespace JobPlatform


	Public Class VacancyUtilities
		'Inherits DatabaseAccessBase

		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()

		Private m_utility As ClsUtilities
		Private m_customerID As String
		Private Const ASMX_SERVICE_NAME As String = "JobPlatform.Vacancy"

		Private m_SysInfo As ISystemInfoDatabaseAccess
		Private m_JobData As IJobDatabaseAccess


#Region "Constructor"


		Public Sub New()

			m_utility = New ClsUtilities
			If Not m_utility.LoadSettingData Then
				m_Logger.LogError(String.Format("LoadSettingData was not successful!!!"))

				Return
			End If
			Dim ConnStr_SystemInfo = m_utility.SettingFileContent.ConnstringSysteminfo
			Dim ConnStr_spJobplattforms = m_utility.SettingFileContent.ConnstringJobplattforms

			m_SysInfo = New SystemInfoDatabaseAccess(ConnStr_SystemInfo, Language.German)
			m_JobData = New JobDatabaseAccess(ConnStr_spJobplattforms, Language.German)

			m_Logger.LogInfo(String.Format("ConnStr_SystemInfo: {1}{0}ConnStr_spJobplattforms: {2}", vbNewLine, ConnStr_SystemInfo, ConnStr_spJobplattforms))

		End Sub

#End Region



#Region "Protected Methods"

		''' <summary>
		''' Returns a string or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetString(columnIndex)
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns an integer or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetInt32(columnIndex)
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns a boolean or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetBoolean(columnIndex)
			Else
				Return defaultValue
			End If
		End Function

		''' <summary>
		''' Returns an datetime or the default value if its nothing.
		''' </summary>
		''' <param name="reader">The reader.</param>
		''' <param name="columnName">The column name.</param>
		''' <param name="defaultValue">The default value.</param>
		''' <returns>Value or default value if the value is nothing</returns>
		Protected Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

			Dim columnIndex As Integer = reader.GetOrdinal(columnName)

			If (Not reader.IsDBNull(columnIndex)) Then
				Return reader.GetDateTime(columnIndex)
			Else
				Return defaultValue
			End If
		End Function

#End Region


	End Class



	Public Class VacancyDatabase
		Inherits DatabaseAccessBase

		Private m_utility As ClsUtilities
		Private m_customerID As String
		Private Const ASMX_SERVICE_NAME As String = "JobPlatform.Vacancy"


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



#Region "Protected Methods"

		'''' <summary>
		'''' Returns a string or the default value if its nothing.
		'''' </summary>
		'''' <param name="reader">The reader.</param>
		'''' <param name="columnName">The column name.</param>
		'''' <param name="defaultValue">The default value.</param>
		'''' <returns>Value or default value if the value is nothing</returns>
		'Protected Shared Function SafeGetString(ByVal reader As SqlDataReader, ByVal columnName As String, Optional ByVal defaultValue As String = Nothing) As String

		'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		'	If (Not reader.IsDBNull(columnIndex)) Then
		'		Return reader.GetString(columnIndex)
		'	Else
		'		Return defaultValue
		'	End If
		'End Function

		'''' <summary>
		'''' Returns an integer or the default value if its nothing.
		'''' </summary>
		'''' <param name="reader">The reader.</param>
		'''' <param name="columnName">The column name.</param>
		'''' <param name="defaultValue">The default value.</param>
		'''' <returns>Value or default value if the value is nothing</returns>
		'Protected Shared Function SafeGetInteger(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Integer?) As Integer?

		'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		'	If (Not reader.IsDBNull(columnIndex)) Then
		'		Return reader.GetInt32(columnIndex)
		'	Else
		'		Return defaultValue
		'	End If
		'End Function

		'''' <summary>
		'''' Returns a boolean or the default value if its nothing.
		'''' </summary>
		'''' <param name="reader">The reader.</param>
		'''' <param name="columnName">The column name.</param>
		'''' <param name="defaultValue">The default value.</param>
		'''' <returns>Value or default value if the value is nothing</returns>
		'Protected Shared Function SafeGetBoolean(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As Boolean?) As Boolean?

		'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		'	If (Not reader.IsDBNull(columnIndex)) Then
		'		Return reader.GetBoolean(columnIndex)
		'	Else
		'		Return defaultValue
		'	End If
		'End Function

		'''' <summary>
		'''' Returns an datetime or the default value if its nothing.
		'''' </summary>
		'''' <param name="reader">The reader.</param>
		'''' <param name="columnName">The column name.</param>
		'''' <param name="defaultValue">The default value.</param>
		'''' <returns>Value or default value if the value is nothing</returns>
		'Protected Shared Function SafeGetDateTime(ByVal reader As SqlDataReader, ByVal columnName As String, ByVal defaultValue As DateTime?) As DateTime?

		'	Dim columnIndex As Integer = reader.GetOrdinal(columnName)

		'	If (Not reader.IsDBNull(columnIndex)) Then
		'		Return reader.GetDateTime(columnIndex)
		'	Else
		'		Return defaultValue
		'	End If
		'End Function

#End Region


	End Class


End Namespace
