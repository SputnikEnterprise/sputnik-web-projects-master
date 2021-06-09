
Imports SR = System.Reflection
Interface IAssInfo
  ReadOnly Property Title() As String
  ReadOnly Property Description() As String
  ReadOnly Property Company() As String
  ReadOnly Property Product() As String
  ReadOnly Property Copyright() As String
  ReadOnly Property Trademark() As String
End Interface

Public Class ClsAssInfo
  Implements IAssInfo

  Dim m_AssInfo As System.Reflection.Assembly
  Sub New()
    m_AssInfo = System.Reflection.Assembly.GetExecutingAssembly
  End Sub

  Public ReadOnly Property Company() As String Implements IAssInfo.Company
    Get
      Dim m_Company As SR.AssemblyCompanyAttribute
      m_Company = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyCompanyAttribute), False)(0),  _
                        Reflection.AssemblyCompanyAttribute)
      Return m_Company.Company.ToString
    End Get
  End Property

  Public ReadOnly Property Copyright() As String Implements IAssInfo.Copyright
    Get
      Dim m_Copyright As SR.AssemblyCopyrightAttribute
      m_Copyright = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyCopyrightAttribute), False)(0),  _
                            Reflection.AssemblyCopyrightAttribute)
      Return m_Copyright.Copyright.ToCharArray
    End Get
  End Property

  Public ReadOnly Property Description() As String Implements IAssInfo.Description
    Get
      Dim m_Description As SR.AssemblyDescriptionAttribute
      m_Description = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyDescriptionAttribute), False)(0),  _
                            Reflection.AssemblyDescriptionAttribute)
      Return m_Description.Description.ToString
    End Get
  End Property

  Public ReadOnly Property Product() As String Implements IAssInfo.Product
    Get
      Dim m_Product As SR.AssemblyProductAttribute
      m_Product = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyProductAttribute), False)(0),  _
                          Reflection.AssemblyProductAttribute)
      Return m_Product.Product.ToString
    End Get
  End Property

  Public ReadOnly Property Title() As String Implements IAssInfo.Title
    Get
      Dim m_Title As SR.AssemblyTitleAttribute
      m_Title = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyTitleAttribute), False)(0),  _
                        Reflection.AssemblyTitleAttribute)
      Return m_Title.Title.ToString
    End Get
  End Property

  Public ReadOnly Property Trademark() As String Implements IAssInfo.Trademark
    Get
      Dim m_Trademark As SR.AssemblyTrademarkAttribute
      m_Trademark = CType(m_AssInfo.GetCustomAttributes(GetType(SR.AssemblyTrademarkAttribute), False)(0),  _
                          Reflection.AssemblyTrademarkAttribute)
      Return m_Trademark.Trademark.ToString
    End Get
  End Property
End Class
