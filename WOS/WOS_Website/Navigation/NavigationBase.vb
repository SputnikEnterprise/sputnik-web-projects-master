'---------------------------------------------------------
' NavigationBase.vb
'
' © by mf Sputnik Informatik GmbH  
'---------------------------------------------------------

Imports Microsoft.VisualBasic

' Base class of navigation menu
Public MustInherit Class NavigationBase
    Inherits UserControl

    Public MustOverride Sub InitNavigationMenu()

End Class
