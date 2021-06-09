Imports JobFinderApp.Contracts

'------------------------------------
' File: XmlConfigService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Namespace Services

    Public Class XmlConfigService
        Implements Contracts.IConfigService

#Region "Private Constants"

        Private Const XML_ELEMENT_SETTING = "Setting"
        Private Const XML_ATTRIBUTE_KEY = "key"

#End Region

#Region "Private Fields"

        ''' <summary>
        '''  The settings dictionary.
        ''' </summary>
        Private settingsDictionary As New Dictionary(Of String, String)

        ''' <summary>
        ''' The logging service.
        ''' </summary>
        Private loggingService As ILoggingService

#End Region

#Region "Constructor"

        ''' <summary>
        ''' The constructor.
        ''' </summary>
        ''' <param name="loggingService">The logging service.</param>
        Public Sub New(loggingService As ILoggingService)
            Me.loggingService = loggingService

        End Sub
#End Region
#Region "Interface IConfigService"

        ''' <see cref="Contracts.IConfigService.GetSetting" />
        Public Function GetSetting(ByVal settingKey As String) As String Implements Contracts.IConfigService.GetSetting
            ' Empty keys are not allowed.
            If String.IsNullOrEmpty(settingKey) Then
                Return String.Empty
            End If

            settingKey = settingKey.ToLower()

            ' Checks if a translations with the translation key is existing.
            If Not Me.settingsDictionary.Keys.Contains(settingKey) Then
                Me.loggingService.Log(String.Format("Could not find setting for setting key {0}.", settingKey), LogLevel.Debug_Level)
                Return String.Empty
            End If

            Return Me.settingsDictionary(settingKey)
        End Function

        ''' <see cref="Contracts.IConfigService.LoadSettings" />
        Public Sub LoadSettings(ByVal settingFilePath As String) Implements Contracts.IConfigService.LoadSettings
            Dim settingsDoc As XDocument = Nothing

            Try
                ' Loads the translations xml file.
                settingsDoc = XDocument.Load(settingFilePath)
            Catch ex As Exception
                Me.loggingService.Log(String.Format("Setting xml file could not be loaded {0}.", ex.ToString), LogLevel.Error_Level)
                Return
            End Try

            ' Read all 'AppSettings' xml elements.
            Dim appSettingsElements = settingsDoc.Descendants(XML_ELEMENT_SETTING)

            For Each settingElement As XElement In appSettingsElements

                If (Not settingElement.Attribute(XML_ATTRIBUTE_KEY) Is Nothing AndAlso _
                    Not String.IsNullOrEmpty(settingElement.Attribute(XML_ATTRIBUTE_KEY).Value.Trim())) Then

                    ' Read the key of the setting string.
                    Dim key As String = settingElement.Attribute(XML_ATTRIBUTE_KEY).Value.Trim().ToLower()

                    ' Read the setting.
                    Dim setting As String = settingElement.Value.Trim()

                    If Not Me.settingsDictionary.Keys.Contains(key) Then
                        Me.settingsDictionary.Add(key, setting)
                    Else
                        Me.loggingService.Log(String.Format("Setting {0} already existing.", key), LogLevel.Debug_Level)
                    End If

                Else
                    Me.loggingService.Log("Setting key is missing or empty.", LogLevel.Debug_Level)
                End If
            Next
        End Sub

#End Region
    End Class

End Namespace
