'------------------------------------
' File: TableConfigurationManager.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports log4net
Imports System.Reflection

''' <summary>
''' Stores and manages table configurations which where form xml table configuration file.
''' </summary>
Public Class TableConfigurationManager

    '--Fields--'

    Private m_TableConfigurations As Dictionary(Of String, TableConfiguration)

    '--Methods--'

    ''' <summary>
    ''' Loads the table configurations from a xml file.
    ''' </summary>
    ''' <param name="path">The path to the xml file.</param>
    Public Sub LoadFromXML(ByVal path As String)

        m_TableConfigurations = New Dictionary(Of String, TableConfiguration)

        ' Load the configuration xml file.
        Dim tableConfigurationsXML As XDocument = XDocument.Load(path)

        ' Query for all table configurations in the xml file.
        Dim tableConfigsQuery = From tableConfig In tableConfigurationsXML.Descendants("tableConfiguration") Select tableConfig

        ' Convert all table configuration xml elemtens to strongly typed objects.
        For Each tableConfigElement As XElement In tableConfigsQuery

            Dim tableConfig As New TableConfiguration

            Try
                ' Read the attributes of the current table configuration
                tableConfig.ID = ReadMandatoryAttribute(tableConfigElement, "id", "Id not yet read.")
                tableConfig.StoredProcedure = ReadMandatoryAttribute(tableConfigElement, "storedProcedure", tableConfig.ID)
                tableConfig.DataFormatternName = ReadMandatoryAttribute(tableConfigElement, "dataFormatterName", tableConfig.ID)
                tableConfig.Title = ReadMandatoryAttribute(tableConfigElement, "title", tableConfig.ID)

                ' Convert all column xml elements to stronly typed objects
                For Each columnElement As XElement In tableConfigElement.Elements("column")
                    Dim tableColumn As New ColumnConfiguration

                    ' Read the attributes of the current column element.
                    tableColumn.ID = ReadMandatoryAttribute(columnElement, "id", tableConfig.ID)
                    tableColumn.Title = ReadMandatoryAttribute(columnElement, "title", tableConfig.ID)
                    tableColumn.ColumnWidth = Convert.ToInt32(ReadMandatoryAttribute(columnElement, "columnWidth", tableConfig.ID))
                    tableColumn.Align = ReadMandatoryAttribute(columnElement, "align", tableConfig.ID)
                    tableColumn.Hidden = Convert.ToBoolean(ReadAttribute(columnElement, "hidden", tableConfig.ID, "false"))
                    tableColumn.JavaScriptColumnFormatterFunctionName = ReadAttribute(columnElement, "javaScriptColumnFormatterFunctionName", tableConfig.ID, String.Empty)

                    tableConfig.AddColumnConfigurations(tableColumn)
                Next

                m_TableConfigurations.Add(tableConfig.ID, tableConfig)

            Catch ex As Exception
                Dim logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
                logger.Error(String.Format("Error while processing table configuration xml file. TableConfigurationId={0}", ReadMandatoryAttribute(tableConfigElement, "id", "Id not yet read.")), ex)
                Throw ex
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Reads mandatory attribute values. If the attribute is missing an exception will be thrown.
    ''' </summary>
    ''' <param name="xmlElement">The xml element.</param>
    ''' <param name="attributeName">The attribute name.</param>
    ''' <param name="tableConfigId">The table config id. This value is used as a hint in error case.</param>
    ''' <returns>Attribute value as string or exception in error case.</returns>
    ''' <remarks></remarks>
    Public Function ReadMandatoryAttribute(ByVal xmlElement As XElement, ByVal attributeName As String, ByRef tableConfigId As String) As String
        If (xmlElement.Attribute(attributeName) Is Nothing) Then
            Throw New Exception(String.Format("Manadorty column attribute {0} is missing. TableConfigurationID={1}", attributeName, tableConfigId))
        End If

        Return xmlElement.Attribute(attributeName)

    End Function


    ''' <summary>
    ''' Reads attribute values.
    ''' </summary>
    ''' <param name="xmlElement">The xml element.</param>
    ''' <param name="attributeName">The attribute name.</param>
    ''' <param name="tableConfigId">The table config id. This value is used as a hint in error case.</param>
    ''' <returns>Attribute value as string or empty string if the attribute is missing.</returns>
    ''' <remarks></remarks>
    Public Function ReadAttribute(ByVal xmlElement As XElement, ByVal attributeName As String, ByRef tableConfigId As String, ByRef defaultValue As String) As String
        If (xmlElement.Attribute(attributeName) Is Nothing) Then
            Return defaultValue
        End If

        Return xmlElement.Attribute(attributeName)

    End Function

    ''' <summary>
    ''' Returns a table configuration by its unique id.
    ''' </summary>
    ''' <param name="tableId">The unique tableId</param>
    ''' <returns>The table configuration object.</returns>
    Public Function GetTableConfigurationById(ByVal tableId As String) As TableConfiguration

        If Not m_TableConfigurations.Keys.Contains(tableId) Then
            Return Nothing
        End If

        Return m_TableConfigurations(tableId)
    End Function

End Class
