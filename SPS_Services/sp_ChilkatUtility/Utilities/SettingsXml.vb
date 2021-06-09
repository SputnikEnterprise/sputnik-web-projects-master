
Imports System.Xml
Imports System.Xml.XPath.Extensions
Imports sp_WebServiceUtility.Logging

Namespace CommonXmlUtility

	''' <summary>
	''' Allows to read, add, update and delete settings form a settings xml file.
	''' </summary>
	Public Class SettingsXml

#Region "Private Fields"

		''' <summary>
		''' Settings dictionary.
		''' </summary>
		''' <remarks>The key is the full path to the setting (e.g. RootNode/SubNode1/ValueNode).</remarks>
		Private m_Settings As Dictionary(Of String, String)

		''' <summary>
		''' The logger.
		''' </summary>
		Protected m_Logger As ILogger = New Logger()

#End Region

#Region "Public Properties"

		Public Property SettingsFilePath As String

#End Region

#Region "Constructor"

		''' <summary>
		''' The constructor.
		''' </summary>
		''' <param name="settingsFilePath">The settings file path.</param>
		Public Sub New(ByVal settingsFilePath)

			If (String.IsNullOrEmpty(settingsFilePath)) Then
				Throw New ArgumentException("Invalid argument.")
			End If

			If Not System.IO.File.Exists(settingsFilePath) Then
				Throw New Exception(String.Format("Settings file does not exists.", settingsFilePath))
			End If

			Me.SettingsFilePath = settingsFilePath

			LoadSettingsFile()

		End Sub

#End Region

#Region "Public Methods"

		''' <summary>
		''' Gets the value of a setting by key.
		''' </summary>
		''' <param name="settingFullPath">The full path of the setting.</param>
		''' <returns>Value of setting.</returns>
		''' <remarks>Throws an execption if setting could not be found.</remarks>
		Public Function GetSettingByKey(ByVal settingFullPath As String) As String

			settingFullPath = CheckPath(settingFullPath)

			If m_Settings.ContainsKey(settingFullPath) Then
				Return m_Settings(settingFullPath)
			Else
				'm_Logger.Warn(String.Format("settingFullPath was not founded: {0}", settingFullPath))
				Return String.Empty
				'Throw New Exception(String.Format("Invalid setting key {0}.", settingFullPath))
			End If

		End Function

		Public Function GetSettingByKey(ByVal settingFullPath As String, ByVal logError As Boolean?) As String

			settingFullPath = CheckPath(settingFullPath)

			If m_Settings.ContainsKey(settingFullPath) Then
				Return m_Settings(settingFullPath)
			Else
				If logError.HasValue AndAlso logError Then
					Throw New Exception(String.Format("Invalid setting key {0}.", settingFullPath))
				Else
					'm_Logger.Warn(String.Format("settingFullPath was not founded: {0}", settingFullPath))
					Return String.Empty
				End If

			End If

		End Function

		''' <summary>
		''' Adds or updates a setting.
		''' </summary>
		''' <param name="settingFullPath">The setting full path.</param>
		''' <param name="value">The value to set.</param>
		Public Sub AddOrUpdateSetting(ByVal settingFullPath As String, ByVal value As String)

			If value Is Nothing Then
				value = String.Empty
			End If

			Dim nodeTokens = CheckPath(settingFullPath).Split("/")

			If nodeTokens.Count < 1 Then
				Throw New Exception(String.Format("Invalid settings path {0}.", settingFullPath))
			End If

			If Not System.IO.File.Exists(SettingsFilePath) Then
				Throw New Exception(String.Format("Settings file does not exists.", SettingsFilePath))
			End If

			Dim xdoc = XDocument.Load(Me.SettingsFilePath)

			If (Not xdoc.Root.Name = nodeTokens(0)) Then
				Throw New Exception(String.Format("The settings file has a different root node: {0}.", xdoc.Root.Name.LocalName))
			End If

			Dim currentXNodeInSettingsXml = xdoc.Root

			For i = 1 To nodeTokens.Count - 1

				Dim currentNodeToken = nodeTokens(i)

				If currentXNodeInSettingsXml.Element(currentNodeToken) Is Nothing Then
					currentXNodeInSettingsXml.Add(New XElement(currentNodeToken))
				End If

				currentXNodeInSettingsXml = currentXNodeInSettingsXml.Element(currentNodeToken)

			Next

			If (currentXNodeInSettingsXml.HasElements) Then
				Throw New Exception(String.Format("The path {0} does not point to a single setting.", settingFullPath))
			End If

			currentXNodeInSettingsXml.Value = value

			xdoc.Save(Me.SettingsFilePath)

			' Reload settings
			LoadSettingsFile(xdoc)

		End Sub

		''' <summary>
		''' Deletes a setting.
		''' </summary>
		''' <param name="settingFullPath">The setting full path.</param>
		Public Sub DeleteSetting(ByVal settingFullPath As String)

			settingFullPath = CheckPath(settingFullPath)

			If Not System.IO.File.Exists(SettingsFilePath) Then
				Throw New Exception(String.Format("Settings file does not exists.", SettingsFilePath))
			End If

			Dim xdoc = XDocument.Load(Me.SettingsFilePath)

			' Select node via XPath.
			Dim element = xdoc.Root.XPathSelectElement("/" + settingFullPath)

			If (element Is Nothing) Then
				m_Logger.LogWarning(String.Format("Could not delete setting. Invalid path {0},", settingFullPath))
				Return
			End If

			If (element.HasElements) Then
				m_Logger.LogWarning(String.Format("Could not delete setting. The path {0} does not point to a single setting.", settingFullPath))
				Return
			End If

			element.Remove()

			xdoc.Save(Me.SettingsFilePath)

			' Reload settings.
			LoadSettingsFile(xdoc)

		End Sub

#End Region

#Region "Private Methods"

		''' <summary>
		''' Loads the config data from an xml file.
		''' </summary>
		Private Sub LoadSettingsFile()
			Dim doc = XDocument.Load(SettingsFilePath)
			LoadSettingsFile(doc)

		End Sub

		''' <summary>
		''' Loads the config data from an xml file.
		''' </summary>
		''' <param name="xdoc">The xml document</param>
		Private Sub LoadSettingsFile(ByVal xdoc As XDocument)

			m_Settings = New Dictionary(Of String, String)

			For Each node In xdoc.Root.Elements
				ProcessNode(node, xdoc.Root.Name.LocalName)
			Next

		End Sub

		''' <summary>
		''' Processes a xml node.
		''' </summary>
		''' <param name="node">The xml node.</param>
		''' <param name="parentPath">The parent path.</param>
		Private Sub ProcessNode(ByVal node As XElement, ByVal parentPath As String)

			Try

				If (Not node.HasElements) Then

					Dim setting_key = parentPath + "/" + node.Name.LocalName
					Dim setting_value = node.Value

					m_Settings.Add(setting_key, setting_value)
				Else
					For Each child In node.Elements
						ProcessNode(child, parentPath + "/" + node.Name.LocalName)
					Next
				End If

			Catch ex As Exception
				m_Logger.LogError(String.Format("Error by parentpath: {0}", parentPath))
			End Try

		End Sub

		''' <summary>
		''' Chekcs a settings path.
		''' </summary>
		''' <param name="settingPath">The settings path to check.</param>
		''' <returns>Checked settings path.</returns>
		Private Function CheckPath(ByVal settingPath As String) As String

			If String.IsNullOrEmpty(settingPath) Then
				Throw New Exception("Empty settings path.")
			End If

			settingPath = settingPath.Trim(" ", "/")

			Return settingPath
		End Function

#End Region

	End Class

End Namespace
