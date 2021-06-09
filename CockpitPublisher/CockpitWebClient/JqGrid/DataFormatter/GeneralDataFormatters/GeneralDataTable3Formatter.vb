'------------------------------------
' File: GeneralDataTable3Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for general data table 3.
''' </summary>
Public Class GeneralDataTable3Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("KontaktType1", DataFormattingHelper.FormatAsString(rowData, "KontaktType1"))
        columData.Add("AnzRec", DataFormattingHelper.FormatAsString(rowData, "AnzRec"))

        Return columData
    End Function

End Class
