'------------------------------------
' File: DataFormattingHelper.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Data formatting helper class.
''' </summary>
Public Class DataFormattingHelper

    ''' <summary>
    ''' Formats a column of a data row as a date.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatColumnAsDate(ByVal row As DataRow, ByVal columName As String) As String

        If (row.IsNull(columName)) Then
            Return String.Empty
        End If

        Return String.Format("{0:d}", row(columName))
    End Function


    ''' <summary>
    ''' Formats a column of a data row as a long date.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatColumnAsLongDate(ByVal row As DataRow, ByVal columName As String) As String

        If (row.IsNull(columName)) Then
            Return String.Empty
        End If

        Return String.Format("{0:G}", row(columName))
    End Function


    ''' <summary>
    ''' Formats a column of a data row as a string.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatAsString(ByVal row As DataRow, ByVal columName As String) As String
        If (row.IsNull(columName)) Then
            Return String.Empty
        End If

        Return row(columName)
    End Function

    ''' <summary>
    ''' Formats a column of a data row as a value.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatAsValue(ByVal row As DataRow, ByVal columName As String) As String

        If (row.IsNull(columName)) Then
            Return String.Empty
        End If

        Return String.Format("{0:0.00}", row(columName))
    End Function

    ''' <summary>
    ''' Formats a column of a data row as a value with thousand separator.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <param name="emptyText">Text to reaturn if column is null.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatAsValueWithThousandSeapartor(ByVal row As DataRow, ByVal columName As String, ByVal emptyText As String) As String

        If (row.IsNull(columName)) Then
            Return emptyText
        End If

        Return String.Format("{0:n}", row(columName))
    End Function


    ''' <summary>
    ''' Formats a column of a data row as a month.
    ''' </summary>
    ''' <param name="row">The data row.</param>
    ''' <param name="columName">The column name.</param>
    ''' <returns>Formatted column.</returns>
    Public Shared Function FormatASMonth(ByVal row As DataRow, ByVal columName As String) As String

        If (row.IsNull(columName)) Then
            Return String.Empty
        End If

        Return String.Format("{0:M MM}", row(columName))
    End Function


End Class
