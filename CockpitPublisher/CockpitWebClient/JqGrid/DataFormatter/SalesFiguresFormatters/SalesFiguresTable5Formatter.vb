'------------------------------------
' File: SalesFiguresTable5Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for sales figures table 5.
''' </summary>
Public Class SalesFiguresTable5Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("Firma1", DataFormattingHelper.FormatAsString(rowData, "Firma1"))
        columData.Add("Kandidat", String.Format("{0}, {1}", DataFormattingHelper.FormatAsString(rowData, "Nachname"), DataFormattingHelper.FormatAsString(rowData, "Vorname")))
        columData.Add("KSTBetrag", DataFormattingHelper.FormatAsValueWithThousandSeapartor(rowData, "KSTBetrag", "0.00"))
        columData.Add("RPKST", DataFormattingHelper.FormatAsString(rowData, "RPKST"))

        ' Determine CSS Class here, because of the thousands separator. It can be a "'" or a "," and it becomes difficult on the client to parse this number correctly.
        Dim kstBetrag As Decimal = Decimal.Zero
        If Not rowData.IsNull("KSTBetrag") Then
            kstBetrag = Convert.ToDecimal(rowData("KSTBetrag"))
        End If

        If kstBetrag > Decimal.Zero Then
            columData.Add("RowCssClass", "RowBlackCss")
        ElseIf kstBetrag < Decimal.Zero Then
            columData.Add("RowCssClass", "RowRedCss")
        Else
            columData.Add("RowCssClass", String.Empty)
        End If

        Return columData
    End Function

End Class
