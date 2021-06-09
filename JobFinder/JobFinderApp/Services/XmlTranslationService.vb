'------------------------------------
' File: TranslationService.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Imports JobFinderApp.Contracts

Namespace Services

    ''' <summary>
    ''' Translation service implemenation that uses xml files for string storage.
    ''' </summary>
    Public Class XmlTranslationService
        Implements ITranslationService

#Region "Private Constants"

        Private Const XML_ELEMENT_STRING = "String"
        Private Const ATTRIBUTE_KEY = "key"

#End Region

#Region "Private Fields"

        ''' <summary>
        '''  The translation dictionary.
        ''' </summary>
        Private translationDictionary As New Dictionary(Of String, String)

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

#Region "Public Methods"

        ''' <summary>
        ''' <see cref="ITranslationService.LoadTranslations"/>
        ''' </summary>
        Public Sub LoadTranslations(ByVal translationslFilePath As String) Implements ITranslationService.LoadTranslations

            Dim translationsDoc As XDocument = Nothing

            Try
                ' Loads the translations xml file.
                translationsDoc = Me.LoadXmlFile(translationslFilePath)
            Catch ex As Exception
                Me.loggingService.Log(String.Format("Translation xml file could not be loaded {0}.", ex.ToString), LogLevel.Error_Level)
                Return
            End Try

            ' Read all 'String' xml elements.
            Dim stringElements = translationsDoc.Descendants(XML_ELEMENT_STRING)

            For Each stringElement As XElement In stringElements

                If (Not stringElement.Attribute(ATTRIBUTE_KEY) Is Nothing AndAlso _
                    Not String.IsNullOrEmpty(stringElement.Attribute(ATTRIBUTE_KEY).Value.Trim())) Then

                    ' Read the key of the translation string.
                    Dim key As String = stringElement.Attribute(ATTRIBUTE_KEY).Value.Trim()

                    ' Read the translations.
                    Dim translations As List(Of XElement) = stringElement.Elements().ToList()

                    For Each translation As XElement In translations
                        ' The language is the element name.
                        Dim language As String = translation.Name.LocalName

                        ' The translation text.
                        Dim value As String = translation.Value.Trim()

                        If Not String.IsNullOrEmpty(value) Then
                            ' The key for the translation is a combination of the key and the language.
                            Dim translationKey As String = String.Format("{0}_{1}", key, language.ToUpper())

                            If Not Me.translationDictionary.Keys.Contains(translationKey) Then
                                Me.translationDictionary.Add(translationKey, value)
                            Else
                                Me.loggingService.Log(String.Format("Translation {0} already existing.", translationKey), LogLevel.Debug_Level)
                            End If
                        Else
                            Me.loggingService.Log(String.Format("Missing translation for key '{0}' and '{1}'.", key, language), LogLevel.Debug_Level)
                        End If
                    Next
                Else
                    Me.loggingService.Log("String key is missing or empty.", LogLevel.Debug_Level)
                End If
            Next
        End Sub

        ''' <summary>
        ''' <see cref="ITranslationService.GetTranslation"/>
        ''' </summary>
        Public Function GetTranslation(ByVal key As String, ByVal language As IsoLanguage) As String Implements ITranslationService.GetTranslation

            ' Empty keys are not allowed.
            If String.IsNullOrEmpty(key) Then
                Return String.Empty
            End If

            ' The translation key is a combination of the key and language.
            Dim translationKey As String = String.Format("{0}_LANGUAGE_{1}", key, language.ToString().ToUpper())

            ' Checks if a translations with the translation key is existing.
            If Not Me.translationDictionary.Keys.Contains(translationKey) Then
                Me.loggingService.Log(String.Format("Could not find translation for translation key {0}.", translationKey), LogLevel.Debug_Level)
                Return String.Empty
            End If

            Return Me.translationDictionary(translationKey)

        End Function

#End Region

#Region "Protected Methods"

        ''' <summary>
        ''' Loads an xml file.
        ''' </summary>
        ''' <param name="translationsXmlFilePath">The xml file path.</param>
        ''' <returns>The xml document.</returns>
        Protected Function LoadXmlFile(ByVal translationsXmlFilePath As String) As XDocument
            Return XDocument.Load(translationsXmlFilePath)
        End Function

#End Region

    End Class

End Namespace