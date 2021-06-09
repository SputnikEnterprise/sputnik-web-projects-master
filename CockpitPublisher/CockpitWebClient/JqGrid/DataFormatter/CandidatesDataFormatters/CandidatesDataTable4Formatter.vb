'------------------------------------
' File: CandidatesDataTable4Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for candidates data table 4.
''' </summary>
Public Class CandidatesDataTable4Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("CreatedOn", DataFormattingHelper.FormatColumnAsDate(rowData, "CreatedOn"))
        columData.Add("Firma1", DataFormattingHelper.FormatAsString(rowData, "Firma1"))
        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("Bezeichnung", DataFormattingHelper.FormatAsString(rowData, "Bezeichnung"))
        columData.Add("KST", DataFormattingHelper.FormatAsString(rowData, "KST"))

        Dim strArt As String = "0"

        If Not rowData.IsNull("zPropose") Then
            strArt = rowData("zPropose")

        End If

        columData.Add("HighlightRow", (strArt = "1").ToString().ToLower())

        Return columData
    End Function

End Class
