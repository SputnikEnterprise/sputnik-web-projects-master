'------------------------------------
' File: SalesFiguresTable3Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for sales figures table 3.
''' </summary>
Public Class SalesFiguresTable3Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("Art", DataFormattingHelper.FormatAsString(rowData, "Art"))
        columData.Add("Fak_Dat", DataFormattingHelper.FormatColumnAsDate(rowData, "Fak_Dat"))
        columData.Add("R_Name1", DataFormattingHelper.FormatAsString(rowData, "R_Name1"))
        columData.Add("TotalBetrag", DataFormattingHelper.FormatAsValueWithThousandSeapartor(rowData, "TotalBetrag", "0.00"))
        columData.Add("KST", DataFormattingHelper.FormatAsString(rowData, "KST"))

        Return columData
    End Function

End Class
