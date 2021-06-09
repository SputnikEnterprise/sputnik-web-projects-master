'------------------------------------
' File: Global.asax.vb
' Date: 26.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Keys used for application cache access.
''' </summary>
Public Class ApplicationCacheKeys
    ''' <summary>
    ''' Key to access the table configuration manager.
    ''' </summary>
    Public Shared ReadOnly TableConfigurationManager As String = "TABLE_CONFIGURATION_MANAGER"

    ''' <summary>
    ''' Key to access the data formatter manager.
    ''' </summary>
    Public Shared ReadOnly DataFormatterManager As String = "DATA_FORMATTER_MANGAGER"
End Class
