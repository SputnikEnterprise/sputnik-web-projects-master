'------------------------------------
' File: CustomerDataTable3Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for customer data table 3.
''' </summary>
Public Class CustomerDataTable3Formatter
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

        columData.Add("KontaktType1", DataFormattingHelper.FormatAsString(rowData, "KontaktType1"))
        columData.Add("KST", DataFormattingHelper.FormatAsString(rowData, "KST"))

        Return columData
    End Function

End Class
