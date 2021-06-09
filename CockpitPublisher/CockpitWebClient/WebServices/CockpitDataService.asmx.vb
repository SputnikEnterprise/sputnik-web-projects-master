'------------------------------------
' File: CockpitDataService.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Web.Script.Services
Imports System.Web.Script.Serialization

''' <summary>
''' Webservice that supplied the client with table data in json format.
''' </summary>
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class CockpitDataService
    Inherits System.Web.Services.WebService

    ''' <summary>
    ''' Profides a javascript client with json data for grid creation.
    ''' </summary>
    ''' <param name="mdGuid">The mdGuid value.</param>
    ''' <param name="kst">The kst value.</param>
    ''' <param name="tableId">The id of the table to load.</param>
    ''' <returns>jqGrid ready json data.</returns>
    <WebMethod()> _
    <ScriptMethod()>
    Public Function GetTableData(ByVal mdGuid As String, ByVal kst As String, ByVal tableId As String) As String

        Dim jsonData As String = "{}"

        ' Check if required manager objects are in cache
        If (Application(ApplicationCacheKeys.TableConfigurationManager) Is Nothing Or
            Application(ApplicationCacheKeys.DataFormatterManager) Is Nothing) Then

            ' Retun empty json object.
            Return jsonData
        End If

        ' Read the table configuration manager from the application cache.
        Dim tableConfigurationManager = CType(Application(ApplicationCacheKeys.TableConfigurationManager), TableConfigurationManager)

        ' Read the data formatter manager from the application cache.
        Dim dataFormatterManager = CType(Application(ApplicationCacheKeys.DataFormatterManager), DataFormatterManager)

        ' Read the table configuration data
        Dim tableConfiguration = tableConfigurationManager.GetTableConfigurationById(tableId)

        If Not (tableConfiguration Is Nothing) Then
            Dim dbAccess As New DatabaseAccess

            ' Read the data of the table with the use of a stored procedure
            Dim parameters As New Dictionary(Of String, String)

            ' Todo: Extend webservice with USKst parameter
            parameters.Add("@SelectedMDGuid", mdGuid)
            parameters.Add("@USKst", kst)

            Dim tableData As DataTable = dbAccess.GetTableDataWidthStoredProcedure(tableConfiguration.StoredProcedure, parameters)

            If Not tableData Is Nothing Then
                Dim jqGridHelper As New JqGridHelper(tableConfigurationManager, dataFormatterManager)

                ' Convert the data to json format
                jsonData = jqGridHelper.ConvertDataTableToJqGridJson(tableConfiguration, tableData)
            End If

        End If

        Return jsonData

    End Function

End Class


