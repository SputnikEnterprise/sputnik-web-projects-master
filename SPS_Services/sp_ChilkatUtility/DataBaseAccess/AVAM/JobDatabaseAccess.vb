
Imports sp_WebServiceUtility.DataTransferObject.SystemInfo.DataObjects
Imports sp_WebServiceUtility.SPUtilities
Imports System.Text


Namespace WOSInfo


	Public Class JobDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IJobDatabaseAccess


		Private m_utility As ClsUtilities
		Private m_customerID As String
		Private Const ASMX_SERVICE_NAME As String = "WOSInfo"


#Region "Constructor"

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As Language)
			MyBase.New(connectionString, translationLanguage)
			m_utility = New ClsUtilities

			'If Not m_utility.LoadSettingData Then
			'	m_Logger.LogError(String.Format("LoadSettingData was not successful!!!"))

			'	Return
			'End If

		End Sub

		''' <summary>
		''' Constructor.
		''' </summary>
		''' <param name="connectionString">The connection string.</param>
		''' <param name="translationLanguage">The translation language.</param>
		Public Sub New(ByVal connectionString As String, ByVal translationLanguage As String)
			MyBase.New(connectionString, translationLanguage)
			m_utility = New ClsUtilities

			'If Not m_utility.LoadSettingData Then
			'	m_Logger.LogError(String.Format("LoadSettingData was not successful!!!"))

			'	Return
			'End If

		End Sub

#End Region


	End Class


End Namespace
