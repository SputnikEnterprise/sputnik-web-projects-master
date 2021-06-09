
Imports System.Web.Services
Imports wsSPS_Services.DataTransferObject.SystemInfo.DataObjects
Imports System.IO


Partial Class SPNotification
	Inherits System.Web.Services.WebService


	<WebMethod(Description:="get cvl base table data")>
	Function GetCVLBaseData(ByVal customerID As String, ByVal TableKind As String, ByVal language As String) As CVLBaseDataDTO()
		Dim result As List(Of CVLBaseDataDTO) = Nothing
		m_customerID = customerID

		Try
			result = New List(Of CVLBaseDataDTO)
			result = m_PublicData.LoadCVLBaseTableData(customerID, TableKind, language)

		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "GetCVLBaseData", .MessageContent = msgContent})
		Finally
		End Try

		' Return search data as an array.
		Return (If(result Is Nothing, Nothing, result.ToArray()))
	End Function

  <WebMethod(Description:="save user form control data")>
  Function SaveUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String, ByVal fieldName As String, ByVal fieldData As String, ByVal createdFrom As String) As Boolean
    Dim result As Boolean = True
    m_customerID = customerID

    Try
      result = m_PublicData.AddUserFormControlTemplateData(customerID, userID, templateName, fieldName, fieldData, createdFrom)

    Catch ex As Exception
      Dim msgContent = ex.ToString
      m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "SaveUserFormControlTemplateData", .MessageContent = msgContent})
    Finally
    End Try

    ' Return search data as an array.
    Return result
  End Function

  <WebMethod(Description:="load user saved form control data")>
  Function LoadUserFormControlTemplateData(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As UserFormControlTemplateDTO()
    Dim result As List(Of UserFormControlTemplateDTO) = Nothing
    m_customerID = customerID

    Try
      result = New List(Of UserFormControlTemplateDTO)
      result = m_PublicData.LoadAssignedUserFormControlTemplateData(customerID, userID, templateName)

    Catch ex As Exception
      Dim msgContent = ex.ToString
      m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadUserFormControlTemplateData", .MessageContent = msgContent})
    Finally
    End Try

    ' Return search data as an array.
    Return (If(result Is Nothing, Nothing, result.ToArray()))
  End Function


#Region "Common Setting files"


	<WebMethod(Description:="upload MainView.xml file")>
	Function LoadMainViewSettingFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings("Xml_MainView")
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMainViewSettingFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function

	<WebMethod(Description:="upload TranslationData.xml file")>
	Function LoadTranslateionSettingFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings("Xml_Translation")
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadTranslateionSettingFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function

	<WebMethod(Description:="upload MailTemplates file")>
	Function LoadMailTemplateFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings(templateName)
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMailTemplateFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function

	<WebMethod(Description:="upload MailTempl_Invoice_01 file")>
	Function LoadInvoiceTemplateFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings("invoice.template")
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadInvoiceTemplateFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function

	<WebMethod(Description:="upload MailTempl_MoreInvoices_01 file")>
	Function LoadMoreInvoicesTemplateFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings("moreinvoices.template")
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMoreInvoicesTemplateFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function

	<WebMethod(Description:="upload MailTempl_MorePayrolls_01 file")>
	Function LoadMorePayrollsTemplateFile(ByVal customerID As String, ByVal userID As String, ByVal templateName As String) As MainViewSettingFileDTO
		Dim result As MainViewSettingFileDTO = Nothing
		m_customerID = customerID

		Try
			Dim xmlFilename = System.Configuration.ConfigurationManager.AppSettings("morepayrolls.template")
			m_Logger.LogDebug(String.Format("File: {0} >>> {1}: {2}", xmlFilename, File.GetLastWriteTime(xmlFilename).Date, File.GetLastWriteTime(xmlFilename).ToLocalTime))

			If File.Exists(xmlFilename) Then
				result = New MainViewSettingFileDTO With {.Filename = xmlFilename, .FileDate = File.GetLastWriteTime(xmlFilename).ToLocalTime, .FileContent = m_utility.LoadFileBytes(xmlFilename)}
			End If


		Catch ex As Exception
			Dim msgContent = ex.ToString
			m_utility.AddErrorToDb(New SPUtilities.ErrorMessageData With {.CustomerID = m_customerID, .SourceModul = ASMX_SERVICE_NAME, .MessageHeader = "LoadMorePayrollsTemplateFile", .MessageContent = msgContent})
		Finally
		End Try

		Return result
	End Function



#End Region

End Class