'------------------------------------
' File: Global.asax.vb
' Date: 24.10.2011
'
' ©2011 Sputnik Informatik GmbH
'------------------------------------

Imports System.Web.SessionState
Imports log4net
Imports System.Reflection
Imports log4net.Config

''' <summary>
''' Asp.net's application lifecycle event class.
''' </summary>
Public Class Global_asax

    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)

        ' Configure log4net
        XmlConfigurator.Configure()

        Dim logger As ILog = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType)

        ' Load the table configurations and store them in application cache.
        Try
            Dim tableConfigManager As New TableConfigurationManager
            tableConfigManager.LoadFromXML(Server.MapPath("~/TableConfig/TableConfigurations.xml"))
            Application.Add(ApplicationCacheKeys.TableConfigurationManager, tableConfigManager)
        Catch ex As Exception
            logger.Error("Error while loading table configurations(/TableConfig/TableConfigurations.xml).", ex)
        End Try

        ' Init the data formatter manager and store it in application cahce.
        Dim dataFromatterManager As New DataFormatterManager
        Application.Add(ApplicationCacheKeys.DataFormatterManager, dataFromatterManager)
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
    End Sub

End Class