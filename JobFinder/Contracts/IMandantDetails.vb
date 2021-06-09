'------------------------------------
' File: IMandantDetails.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

Public Interface IMandantDetails

    ''' <summary>
    ''' The mandant guid.
    ''' </summary>
    Property Guid As String

    ''' <summary>
    ''' Tells whether the mandant guid esists and is valid.
    ''' </summary>
    ''' <returns>True if the mandant guid esists and is valid, False otherwise.</returns>
    Property IsValidMandantGuid As Boolean

    ''' <summary>
    ''' The mandant name.
    ''' </summary>
    ''' <remarks>Use this only if IsValidMandantGuid is True.</remarks>
    Property Name As String

    ''' <summary>
    ''' The mandant home page.
    ''' </summary>
    ''' <remarks>Use this only if IsValidMandantGuid is True.</remarks>
    Property HomePage As String

    ''' <summary>
    ''' The mandant Email Address.
    ''' </summary>
    ''' <remarks>Use this only if IsValidMandantGuid is True.</remarks>
    Property EmailAddress As String

End Interface
