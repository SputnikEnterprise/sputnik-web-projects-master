'------------------------------------
' File: GeneralDataTable2Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for general data table 2.
''' </summary>
Public Class GeneralDataTable2Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("CreatedOn", DataFormattingHelper.FormatColumnAsDate(rowData, "CreatedOn"))
        columData.Add("eMail_Subjet", DataFormattingHelper.FormatAsString(rowData, "Bezeichnung"))

        Return columData
    End Function

End Class
