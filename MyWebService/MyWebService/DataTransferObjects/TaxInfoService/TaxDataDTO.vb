
Namespace DataTransferObjects.TaxInfoService

  <Serializable()>
  Public Class TaxDataDTO

#Region "Constructor"

    Public Sub New()

    End Sub

    ''' <summary>
    ''' The constructor.
    ''' </summary>
    ''' <param name="data">The data.</param>
    Public Sub New(ByVal data As TaxDataItemDTO())
      Me.Data = data

    End Sub

#End Region

#Region "Public Properties"

    Public Property Data As TaxDataItemDTO()

#End Region

  End Class
End Namespace
