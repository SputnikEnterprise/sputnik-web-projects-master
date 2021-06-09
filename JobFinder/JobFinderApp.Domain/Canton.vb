'------------------------------------
' File: Canton.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Canton name.
''' </summary>
Public Class Canton

#Region "Constructor"

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="cantonName">The canton name string.</param>
    ''' <param name="cantonAbbreviation">The canton abbreviation string.</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal cantonName As String, cantonAbbreviation As String)
        Me.CantonName = cantonName
        Me.CantonAbbreviation = cantonAbbreviation
    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' The canton name string.
    ''' </summary>
    Public Property CantonName As String

    ''' <summary>
    ''' The canton abbreviation string
    ''' </summary>
    Public Property CantonAbbreviation As String

    ''' <summary>
    ''' The canton name combined with the canton abbreviation.
    ''' </summary>
    Public ReadOnly Property CantonNameWithAbbreviation As String
        Get

            Dim cantonName As String = IIf(String.IsNullOrEmpty(Me.CantonName), "-", Me.CantonName)
            Dim cantonAbbreviation As String = IIf(String.IsNullOrEmpty(Me.CantonAbbreviation), "-", Me.CantonAbbreviation)

            Return String.Format("{0} ({1})", cantonName, cantonAbbreviation)

        End Get
    End Property

#End Region

End Class
