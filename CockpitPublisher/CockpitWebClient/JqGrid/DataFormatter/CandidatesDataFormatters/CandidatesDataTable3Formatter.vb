'------------------------------------
' File: CandidatesDataTable3Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for candidates data table 3.
''' </summary>
Public Class CandidatesDataTable3Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("GebDat", DataFormattingHelper.FormatColumnAsDate(rowData, "GebDat"))
        columData.Add("Natel", DataFormattingHelper.FormatAsString(rowData, "Natel"))

        Return columData
    End Function

End Class
