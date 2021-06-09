'------------------------------------
' File: JqGridHelper.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports CockpitPublisher.Common
Imports System.Web.Script.Serialization
Imports System.Runtime.Serialization.Json
Imports System.IO

Public Class JqGridHelper

    '--Fields--

    Private m_TableConfigurationManager As TableConfigurationManager
    Private m_DataFromatterManagar As DataFormatterManager
    Private m_JavaScriptSerialiszer As New JavaScriptSerializer

    '--Methods--

    ''' <summary>
    ''' Constructor
    ''' </summary>
    ''' <param name="tableConfigurationManager">The table configuration manager.</param>
    ''' <param name="dataFormatterManager">The data formatter.</param>
    Public Sub New(ByVal tableConfigurationManager As TableConfigurationManager, ByVal dataFormatterManager As DataFormatterManager)
        m_TableConfigurationManager = tableConfigurationManager
        m_DataFromatterManagar = dataFormatterManager
    End Sub

    ''' <summary>
    ''' Converts the data of a data table to the jqgrid required data format.
    ''' </summary>
    ''' <param name="tableConfiguration">The table table configuration.</param>
    ''' <param name="tableData">The table data.</param>
    ''' <returns>Json string with all required data to construct the grid on the client side.</returns>
    Public Function ConvertDataTableToJqGridJson(ByVal tableConfiguration As TableConfiguration, ByVal tableData As DataTable) As String

        Dim jqGridTableData As New JqGridTableData
        jqGridTableData.ColumnNamesJson = Utility.SerializeToJson(tableConfiguration.TableColumnNames(), GetType(List(Of String)))
        jqGridTableData.ColumnModelsJson = Utility.SerializeToJson(tableConfiguration.ConvertColumnModelsToJqGridColumnModels(), GetType(List(Of JqGridColumnModel)))
        jqGridTableData.PagedListJson = Utility.SerializeToJson(ConvertDataTableToJqGridDataFormat(tableConfiguration.DataFormatternName, tableData)
                                                                )
        Return m_JavaScriptSerialiszer.Serialize(jqGridTableData)
    End Function

    ''' <summary>
    ''' Converts a data table to a jqgrid ready data object.
    ''' </summary>
    ''' <param name="dataFormatterName">The name of the data formatter.</param>
    ''' <param name="dataTable">The data table.</param>
    ''' <returns>JqGrid ready data objet.</returns>
    Private Function ConvertDataTableToJqGridDataFormat(ByVal dataFormatterName As String, ByVal dataTable As DataTable) As JqGridPagedList
        Dim dataFormatter = m_DataFromatterManagar.GetDataFormatterByName(dataFormatterName)
        If Not dataFormatter Is Nothing Then
            Return dataFormatter.FormatTableData(dataTable)
        Else
            Return New JqGridPagedList()
        End If
    End Function

End Class
