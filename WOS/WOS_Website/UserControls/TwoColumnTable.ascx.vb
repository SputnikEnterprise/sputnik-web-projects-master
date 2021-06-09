'---------------------------------------------------------
' TwoColumnTable.ascx.vb
' Provides a full dynamically configurable Table 
' with two columns.
'
' © by al Sputnik Informatik GmbH
'---------------------------------------------------------

Partial Class TwoColumnTable
    Inherits System.Web.UI.UserControl
    Private FirstCellRelativeWidth As String = "20%"
    Private SecondCellRelativeWidth As String = "80%"

    ''' <summary>
    ''' Inserts a row in the table consisting of Name Text Key and a class that will be added to the value label.
    ''' </summary>
    Public Sub AddRow(ByVal DetailNameTextKey As String, ByVal DetailValueClass As String)
        Dim tableRow As WebControls.TableRow = New WebControls.TableRow()
        tableId.Rows.Add(tableRow)
        Dim tableCellName As WebControls.TableCell = New WebControls.TableCell()
        tableCellName.Attributes.Add("align", "left")
        tableCellName.Attributes.Add("width", FirstCellRelativeWidth)
        tableCellName.Attributes.Add("valign", "top")

        Dim nameLabel As Imt.Common.I18N.WebControls.Label = New Imt.Common.I18N.WebControls.Label()
        nameLabel.TextKey = DetailNameTextKey
        nameLabel.Attributes.Add("style", "font-weight: bold")
        tableCellName.Controls.Add(nameLabel)

        Dim tableCellValue As WebControls.TableCell = New WebControls.TableCell()
        tableCellValue.Attributes.Add("align", "left")
        tableCellValue.Attributes.Add("width", SecondCellRelativeWidth)
        tableCellValue.Attributes.Add("valign", "top")

        Dim valueSpan As Web.UI.HtmlControls.HtmlGenericControl = New Web.UI.HtmlControls.HtmlGenericControl("span")
        valueSpan.Attributes.Add("class", DetailValueClass)
        tableCellValue.Controls.Add(valueSpan)

        tableRow.Cells.Add(tableCellName)
        tableRow.Cells.Add(tableCellValue)
    End Sub

    ''' <summary>
    ''' Inserts a row in the table consisting of Name Text Key, and a link as value, with a corresponding text key and a class.
    ''' </summary>
    Public Sub AddLinkRow(ByVal DetailNameTextKey As String, ByVal LinkTextKey As String, ByVal LinkCSSClass As String)
        Dim tableRow As WebControls.TableRow = New WebControls.TableRow()
        tableId.Rows.Add(tableRow)
        Dim tableCellName As WebControls.TableCell = New WebControls.TableCell()
        tableCellName.Attributes.Add("align", "left")
        tableCellName.Attributes.Add("width", FirstCellRelativeWidth)
        tableCellName.Attributes.Add("valign", "top")

        Dim nameLabel As Imt.Common.I18N.WebControls.Label = New Imt.Common.I18N.WebControls.Label()
        nameLabel.TextKey = DetailNameTextKey
        nameLabel.Attributes.Add("style", "font-weight: bold")
        tableCellName.Controls.Add(nameLabel)

        Dim tableCellValue As WebControls.TableCell = New WebControls.TableCell()
        tableCellValue.Attributes.Add("align", "left")
        tableCellValue.Attributes.Add("width", SecondCellRelativeWidth)
        tableCellValue.Attributes.Add("valign", "top")

        Dim valueLink As Web.UI.HtmlControls.HtmlAnchor = New Web.UI.HtmlControls.HtmlAnchor()
        valueLink.Attributes.Add("class", LinkCSSClass)
        valueLink.Attributes.Add("href", "#")
        Dim linkLabel As Imt.Common.I18N.WebControls.Label = New Imt.Common.I18N.WebControls.Label()
        linkLabel.TextKey = LinkTextKey
        valueLink.Controls.Add(linkLabel)

        tableCellValue.Controls.Add(valueLink)

        tableRow.Cells.Add(tableCellName)
        tableRow.Cells.Add(tableCellValue)
    End Sub

    ''' <summary>
    ''' Inserts an empty row in the table.
    ''' </summary>
    Public Sub AddEmptyRow()
        Dim tableRow As WebControls.TableRow = New WebControls.TableRow()
        tableId.Rows.Add(tableRow)

        Dim emptyTableCellName As WebControls.TableCell = New WebControls.TableCell()
        emptyTableCellName.Attributes.Add("align", "left")
        emptyTableCellName.Attributes.Add("width", FirstCellRelativeWidth)
        emptyTableCellName.Attributes.Add("valign", "top")
        emptyTableCellName.Text = "&nbsp;"
        tableRow.Cells.Add(emptyTableCellName)

        Dim emptyTableCellValue As WebControls.TableCell = New WebControls.TableCell()
        emptyTableCellValue.Attributes.Add("align", "left")
        emptyTableCellValue.Attributes.Add("width", SecondCellRelativeWidth)
        emptyTableCellValue.Attributes.Add("valign", "top")
        emptyTableCellValue.Text = "&nbsp;"
        tableRow.Cells.Add(emptyTableCellValue)
    End Sub

    ''' <summary>
    ''' Sets the table width and its relative column widths.
    ''' </summary>
    Public Sub SetWidths(ByVal Width As Integer, ByVal FirstCellRelativeWidth As Integer, ByVal SecondCellRelativeWidth As Integer)
        tableId.Width = New Unit(Width)
        If FirstCellRelativeWidth + SecondCellRelativeWidth <> 100 Then
            FirstCellRelativeWidth = 100 * (FirstCellRelativeWidth) / (FirstCellRelativeWidth + SecondCellRelativeWidth)
            SecondCellRelativeWidth = 100 - FirstCellRelativeWidth
        End If
        Me.FirstCellRelativeWidth = FirstCellRelativeWidth & "%"
        Me.SecondCellRelativeWidth = SecondCellRelativeWidth & "%"
    End Sub
End Class
