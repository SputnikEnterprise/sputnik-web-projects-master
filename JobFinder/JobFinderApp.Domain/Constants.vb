'------------------------------------
' File: Constants.vb
'
' ©2012 Sputnik Informatik GmbH
'------------------------------------

''' <summary>
''' Class with public contants only.
''' </summary>
Public Class Constants

#Region "Public Constants"

    ''' <summary>
    ''' Commonly used const to define the meaning of "wildcard" for getting all values.
    ''' </summary>
    Public Const SELECT_ALL_WILDCARD As String = "All"

    ''' <summary>
    ''' The key used to retrieve the translation service form the http cache.
    ''' </summary>
    Public Const TRANSLATION_SERVICE_CACHE_KEY As String = "TRANSLATIONSERVICE"

    ''' <summary>
    ''' The relative URL to the Icon "Handler".
    ''' </summary>
    Public Const ICON_RELATIVE_URL As String = "/File/GetMandantIcon?mandantGuid="

    ''' <summary>
    ''' The relative URL to the Icon "Handler".
    ''' </summary>
    Public Const MANDANT_USER_PICTURE_RELATIVE_URL As String = "/File/GetMandantUserPicture?mandantGuid={0}&userId={1}"

    ''' <summary>
    ''' The relative URL to the App.
    ''' </summary>
    Public Const APP_RELATIVE_URL As String = "/Home/Index?stream="

    ''' <summary>
    ''' Secrect key that is used by default to encrypt and decript sensitive informations.
    ''' </summary>
    Public Const DEFAULT_KRYPTOGRAPHIC_SECRET_KEY As String = "12Sputnik_JobFinder_Secret_Key34"

    ''' <summary>
    ''' Name prefix for column translations.
    ''' </summary>
    Public Const COLUMN_TRANSLATION_PREFIX = "TEXT_COLUMN_"

    ''' <summary>
    ''' Column name of user telephone.
    ''' </summary>
    Public Const COLUM_USER_TELEPHONE = "User_Telefon"

    ''' <summary>
    ''' Column name of user email.
    ''' </summary>
    Public Const COLUM_USER_EMAIL = "User_eMail"

    ''' <summary>
    ''' Column name of customer telefon.
    ''' </summary>
    Public Const COLUM_CUSTOMER_TELEPHONE = "Customer_Telefon"

    ''' <summary>
    ''' Column name of customer email.
    ''' </summary>
    Public Const COLUM_CUSTOMER_EMAIL = "Customer_eMail"

    ''' <summary>
    ''' Column name of the mandant user.
    ''' </summary>
    ''' <remarks></remarks>
    Public Const COLUMN_MANDANT_USER_PICTURE_URL = "User_Picture_Url"

#End Region

End Class


