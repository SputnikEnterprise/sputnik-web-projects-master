'------------------------------------
' File: CustomerDataTable4Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for customer data table 4.
''' </summary>
Public Class CustomerDataTable4Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("Firma", String.Format("{0}, {1} {2}", DataFormattingHelper.FormatAsString(rowData, "Firma1"), DataFormattingHelper.FormatAsString(rowData, "PLZ"), DataFormattingHelper.FormatAsString(rowData, "Ort")))

        If Not (rowData.IsNull("KreditlimitBis") Or String.IsNullOrEmpty(rowData("KreditlimitBis"))) Then
            columData.Add("Kreditdaten", String.Format("({0}-{1}) / {2}", _
                                                       DataFormattingHelper.FormatAsString(rowData, "KreditLimite"), _
                                                       DataFormattingHelper.FormatAsString(rowData, "KreditLimite_2"), _
                                                       DataFormattingHelper.FormatColumnAsDate(rowData, "KreditlimiteBis")))
        Else
            columData.Add("Kreditdaten", "?")
        End If

        Return columData
    End Function

End Class
