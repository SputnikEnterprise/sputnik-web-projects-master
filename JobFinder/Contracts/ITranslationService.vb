'------------------------------------
' File: ITranslationService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Supported languages for translations.
''' </summary>
Public Enum IsoLanguage As Integer
    DE ' German
    EN ' English
    FR ' Frensch
    IT ' Italian
End Enum

''' <summary>
''' Translations service interface.
''' </summary>
Public Interface ITranslationService

    ''' <summary>
    ''' Loads the translations form a file.
    ''' </summary>
    ''' <param name="translationslFilePath">The translations file path.</param>
    Sub LoadTranslations(ByVal translationslFilePath As String)

    ''' <summary>
    ''' Gets a translation by key and language.
    ''' </summary>
    ''' <param name="key">The translation key.</param>
    ''' <param name="language">The language.</param>
    ''' <returns>The translated text.</returns>
    Function GetTranslation(ByVal key As String, ByVal language As IsoLanguage) As String

End Interface
