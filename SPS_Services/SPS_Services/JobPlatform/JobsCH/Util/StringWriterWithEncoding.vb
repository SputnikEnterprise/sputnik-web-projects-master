Imports System.IO
Imports System.Text

Namespace JobPlatform.JobsCH

  ''' <summary>
  ''' StringWriter which allows to set the encoding.
  ''' </summary>
  Public Class StringWriterWithEncoding
    Inherits StringWriter

#Region "Private Fields"

    Private m_Encoding As Text.Encoding

#End Region

#Region "Constructor"

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="enc">The encoding.</param>
    Public Sub New(ByVal enc As Text.Encoding)
      m_Encoding = enc
    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="sb">The string builder.</param>
    Public Sub New(ByVal sb As StringBuilder)
      MyBase.New(sb)
    End Sub

    ''' <summary>
    ''' Constructor.
    ''' </summary>
    ''' <param name="enc">The encoding.</param>
    ''' <param name="sb">The string builder.</param>
    Public Sub New(ByVal enc As Encoding, ByVal sb As StringBuilder)
      MyBase.New(sb)

      m_Encoding = enc

    End Sub

#End Region

#Region "Public Properties"

    ''' <summary>
    ''' Gets the encoding
    ''' </summary>
    ''' <returns>The encoding.</returns>
    Public Overrides ReadOnly Property Encoding As System.Text.Encoding
      Get
        Return m_Encoding
      End Get
    End Property
#End Region

  End Class

End Namespace
