'------------------------------------
' File: AssignmentOfPersonnelTable2Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for assignment of personnel table 2.
''' </summary>
Public Class AssignmentOfPersonnelTable2Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("ES_Ab", DataFormattingHelper.FormatColumnAsDate(rowData, "ES_Ab"))
        columData.Add("Firma1", DataFormattingHelper.FormatAsString(rowData, "Firma1"))
        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("ES_Als", DataFormattingHelper.FormatAsString(rowData, "ES_Als"))
        columData.Add("ESKST", DataFormattingHelper.FormatAsString(rowData, "ESKST"))

        Return columData
    End Function

End Class
