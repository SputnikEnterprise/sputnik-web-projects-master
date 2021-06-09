'------------------------------------
' File: CustomerDataTable1Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for customer data table 1.
''' </summary>
Public Class CustomerDataTable1Formatter
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
        columData.Add("TelefonState", DataFormattingHelper.FormatAsString(rowData, "TelefonState"))
        columData.Add("TelefaxState", DataFormattingHelper.FormatAsString(rowData, "TelefaxState"))
        columData.Add("EmailState", DataFormattingHelper.FormatAsString(rowData, "EmailState"))

        columData.Add("KDKAnz", DataFormattingHelper.FormatAsString(rowData, "KDKAnz"))
        columData.Add("KDZAnz", DataFormattingHelper.FormatAsString(rowData, "KDZAnz"))
        columData.Add("PAnz", DataFormattingHelper.FormatAsString(rowData, "PAnz"))
        columData.Add("ESAnz", DataFormattingHelper.FormatAsString(rowData, "ESAnz"))

        Return columData
    End Function

End Class
