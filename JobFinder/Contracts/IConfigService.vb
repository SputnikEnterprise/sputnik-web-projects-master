'------------------------------------
' File: ILoggingService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Public Interface IConfigService

    ''' <summary>
    ''' Retreives the setting with a given key.
    ''' </summary>
    ''' <param name="settingKey">Setting key</param>
    ''' <returns>Setting value</returns>
    Function GetSetting(ByVal settingKey As String) As String

    ''' <summary>
    ''' Loads the settings using a certain file path.
    ''' </summary>
    ''' <param name="settingFilePath">setting file path</param>
    ''' <remarks></remarks>
    Sub LoadSettings(ByVal settingFilePath As String)

End Interface
