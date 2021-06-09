'------------------------------------
' File: CandidatesDataTable1Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for candidates data table 1.
''' </summary>
Public Class CandidatesDataTable1Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("CreatedOn", DataFormattingHelper.FormatColumnAsDate(rowData, "CreatedOn"))
        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("Beruf", DataFormattingHelper.FormatAsString(rowData, "Beruf"))
        columData.Add("PAnz", DataFormattingHelper.FormatAsString(rowData, "PAnz"))
        columData.Add("ESAnz", DataFormattingHelper.FormatAsString(rowData, "ESAnz"))
        columData.Add("NatelState", DataFormattingHelper.FormatAsString(rowData, "NatelState"))
        columData.Add("EmailState", DataFormattingHelper.FormatAsString(rowData, "EmailState"))
        columData.Add("DokAnz", DataFormattingHelper.FormatAsString(rowData, "DokAnz"))
        columData.Add("BildState", DataFormattingHelper.FormatAsString(rowData, "BildState"))

        Return columData
    End Function

End Class
