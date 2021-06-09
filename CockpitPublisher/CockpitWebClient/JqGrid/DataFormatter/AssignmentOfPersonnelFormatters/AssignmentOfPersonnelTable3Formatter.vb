'------------------------------------
' File: AssignmentOfPersonnelTable3Formatter.vb
' Date: 25.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatter for assignment of personnel table 3.
''' </summary>
Public Class AssignmentOfPersonnelTable3Formatter
    Inherits DataFormatterAbs

    ''' <summary>
    ''' Formats the required columns of the data row. 
    ''' </summary>
    ''' <param name="rowData">The data row object.</param>
    ''' <returns>Dictionary of formatted column values. The colum name is used as the key.</returns>
    Protected Overrides Function FormatColumnData(ByVal rowData As DataRow) As Dictionary(Of String, String)
        Dim columData As New Dictionary(Of String, String)

        columData.Add("Einstufung", DataFormattingHelper.FormatAsString(rowData, "Einstufung"))
        columData.Add("Anzrec", DataFormattingHelper.FormatAsValue(rowData, "Anzrec"))
        columData.Add("AnzRecOld", DataFormattingHelper.FormatAsValue(rowData, "AnzRecOld"))
        columData.Add("AnzDiff", DataFormattingHelper.FormatAsValue(rowData, "AnzDiff"))

        Dim imagePath As String = String.Empty

        ' Set the imagePath value for the image column
        If (Not rowData.IsNull("AnzDiff")) Then
            Dim anzDiff As Decimal = Convert.ToDecimal(rowData("AnzDiff"))

            If (anzDiff < 0) Then
                imagePath = "./Images/arrowSouthEastRed.png"
            ElseIf (anzDiff = Decimal.Zero) Then
                imagePath = "./Images/arrowEastOrange.png"
            Else
                imagePath = "./Images/arrowNorthEastGreen.png"
            End If

        End If

        columData.Add("image", imagePath)

        Return columData
    End Function

End Class
