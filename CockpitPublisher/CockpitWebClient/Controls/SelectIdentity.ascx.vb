'------------------------------------
' File: SelectIdentity.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports CockpitPublisher.Common
Imports log4net
Imports System.Reflection

''' <summary>
''' Allows to select a 'Mandant'.
''' </summary>
Public Class SelectIdentity
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' Stored procedure to read all allowed MD data. 
    ''' </summary>
    ''' 
    Private Const STORED_PROCEDURE_LIST_ALL_ALLOWED_MD_DATA As String = "[dbo].[Cockpit. List My Allowed MDData]"
    Private Const STORED_PROCEDURE_LIST_ALL_ALLOWED_BERATER As String = "[dbo].[Cockpit. List All Allowed Berater]"


    ''' <summary>
    ''' The page load method.
    ''' </summary>
    ''' <param name="sender">The sender.</param>
    ''' <param name="e">The args.</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' Read the MDGuids query stirng parameter
        Dim mdGuidsParameter As String = Request.QueryString("MDGuids")

        If Not String.IsNullOrEmpty(mdGuidsParameter) Then

            Try
                ' Deserialize the MD guids.
                Dim derserializedGuids As List(Of String) = Utility.DeSerializeGuidList(mdGuidsParameter)

                ' Concatenated the list of MD guids.
                Dim concatenatedListOfClientGuids As String = Utility.ConcatenateList(derserializedGuids, ",")

                ' Init MD drowp down.
                InitMDDropDown(concatenatedListOfClientGuids)

                ' Init KST drop down.
                InitKSTDropDown(concatenatedListOfClientGuids)

            Catch ex As Exception
                Dim logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)
                logger.Warn("Initialization of md guid and kst drop downs faild.", ex)

            End Try
        End If

    End Sub

    ''' <summary>
    ''' Inits the MD drop down.
    ''' </summary>
    ''' <param name="concatenatedListOfAllMDGuids">The concatedanted list of all md guids.</param>
    Private Sub InitMDDropDown(ByVal concatenatedListOfAllMDGuids As String)
        Dim dbAccess As New DatabaseAccess()

        ' Read the allowed MD data from the database.

        Dim parameters As New Dictionary(Of String, String)
        parameters.Add("@AllMDGuid", concatenatedListOfAllMDGuids)

        Dim mdTable As DataTable = dbAccess.GetTableDataWidthStoredProcedure(STORED_PROCEDURE_LIST_ALL_ALLOWED_MD_DATA, parameters)

        If (Not mdTable Is Nothing) AndAlso mdTable.Rows.Count > 0 Then

            ddlMDs.Items.Add(New ListItem("Alle (Keine Einschränkungen)", concatenatedListOfAllMDGuids))

            For Each mdRow As DataRow In mdTable.Rows

                If Not (mdRow.IsNull("MDGuid") Or String.IsNullOrEmpty(mdRow("MDGuid"))) And
                    Not (mdRow.IsNull("MDName") Or String.IsNullOrEmpty(mdRow("MDName"))) Then

                    ' Add the value to the drop down.
                    ddlMDs.Items.Add(New ListItem(mdRow("MDName"), mdRow("MDGuid")))
                End If

            Next

            ' Select the first drop down item.
            ddlMDs.SelectedIndex = 0
        End If
    End Sub

    ''' <summary>
    ''' Inits the KST drop down.
    ''' </summary>
    ''' <param name="concatenatedListOfAllMDGuids">The concatedanted list of all md guids.</param>
    Private Sub InitKSTDropDown(ByVal concatenatedListOfAllMDGuids As String)
        ' Add always the "All KST" choice
        ddlKSTs.Items.Add(New ListItem("Alle (Keine Einschränkungen)", String.Empty))

        Dim dbAccess As New DatabaseAccess()

        ' Read the allowed MD data from the database.

        Dim parameters As New Dictionary(Of String, String)
        parameters.Add("@AllMDGuid", concatenatedListOfAllMDGuids)

        Dim advisors As DataTable = dbAccess.GetTableDataWidthStoredProcedure(STORED_PROCEDURE_LIST_ALL_ALLOWED_BERATER, parameters)

        If (Not advisors Is Nothing) AndAlso advisors.Rows.Count > 0 Then

            For Each client As DataRow In advisors.Rows

                If Not (client.IsNull("Vorname") Or String.IsNullOrEmpty(client("Vorname"))) And
                    Not (client.IsNull("Nachname") Or String.IsNullOrEmpty(client("Nachname"))) And
                    Not (client.IsNull("KST") Or String.IsNullOrEmpty(client("KST"))) Then


                    ' Add the value to the drop down.
                    ddlKSTs.Items.Add(New ListItem(String.Format("{0} {1}", client("Vorname"), client("Nachname")), client("KST")))
                End If

            Next

            ' Select the first drop down item.
            ddlKSTs.SelectedIndex = 0
        End If
    End Sub

End Class