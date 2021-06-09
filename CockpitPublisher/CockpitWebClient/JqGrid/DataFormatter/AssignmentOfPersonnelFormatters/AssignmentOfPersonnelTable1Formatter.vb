'------------------------------------
' File: AssignmentOfPersonnelTable1Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for assignment of personnel table 1.
''' </summary>
Public Class AssignmentOfPersonnelTable1Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("ESNr", DataFormattingHelper.FormatAsString(rowData, "ESNr"))
        columData.Add("MANr", DataFormattingHelper.FormatAsString(rowData, "MANr"))
        columData.Add("KDNr", DataFormattingHelper.FormatAsString(rowData, "KDNr"))
        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("Firma1", rowData("Firma1"))
        columData.Add("ES_Als", rowData("ES_Als"))
        columData.Add("From_To", String.Format("{0} - {1}", DataFormattingHelper.FormatColumnAsDate(rowData, "ES_Ab"), DataFormattingHelper.FormatColumnAsDate(rowData, "ES_Ende")))

        Return columData
    End Function

End Class
