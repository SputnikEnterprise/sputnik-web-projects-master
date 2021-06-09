
Imports wsSPS_Services.DataTransferObject.UpdateInfo.DataObjects
Imports wsSPS_Services.SPUtilities
Imports System.Text


Namespace UpdateInfo


	Public Class UpdateInfoDatabaseAccess

		Inherits DatabaseAccessBase
		Implements IUpdateInfoDatabaseAccess


		Private m_utility As ClsUtilities
		Private m_customerID As String
		Private Const ASMX_SERVICE_NAME As String = "UpdateInfo"


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


	End Class


End Namespace
