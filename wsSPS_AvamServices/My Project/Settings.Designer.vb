﻿'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:4.0.30319.42000
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    <Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0"),  _
     Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
    Partial Friend NotInheritable Class MySettings
        Inherits Global.System.Configuration.ApplicationSettingsBase
        
        Private Shared defaultInstance As MySettings = CType(Global.System.Configuration.ApplicationSettingsBase.Synchronized(New MySettings()),MySettings)
        
#Region "Automatische My.Settings-Speicherfunktion"
#If _MyType = "WindowsForms" Then
    Private Shared addedHandler As Boolean

    Private Shared addedHandlerLockObject As New Object

    <Global.System.Diagnostics.DebuggerNonUserCodeAttribute(), Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)> _
    Private Shared Sub AutoSaveSettings(sender As Global.System.Object, e As Global.System.EventArgs)
        If My.Application.SaveMySettingsOnExit Then
            My.Settings.Save()
        End If
    End Sub
#End If
#End Region
        
        Public Shared ReadOnly Property [Default]() As MySettings
            Get
                
#If _MyType = "WindowsForms" Then
               If Not addedHandler Then
                    SyncLock addedHandlerLockObject
                        If Not addedHandler Then
                            AddHandler My.Application.Shutdown, AddressOf AutoSaveSettings
                            addedHandler = True
                        End If
                    End SyncLock
                End If
#End If
                Return defaultInstance
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("spwebService@sputnik-it.com")>  _
        Public ReadOnly Property errMailFrom() As String
            Get
                Return CType(Me("errMailFrom"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Fehler...")>  _
        Public ReadOnly Property errMailBody() As String
            Get
                Return CType(Me("errMailBody"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=1234;Persist Security Info=True;User ID=GAVUser;Initial Catalog=Sputnik "& _ 
            "InfoSystem;Data Source=192.168.19.53")>  _
        Public ReadOnly Property Connstr_InfoService() As String
            Get
                Return CType(Me("Connstr_InfoService"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=1234;Persist Security Info=True;User ID=GAVUser;Initial Catalog=Sputnik "& _ 
            "GAV;Data Source=192.168.19.53")>  _
        Public ReadOnly Property ConnStr_ServiceUtil() As String
            Get
                Return CType(Me("ConnStr_ServiceUtil"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=Sputnik InfoSystem;Data Source=192.168.19.53")>  _
        Public ReadOnly Property ConnStr_SputnikInfoSystem() As String
            Get
                Return CType(Me("ConnStr_SputnikInfoSystem"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=1234;Persist Security Info=True;User ID=GAVUser;Initial Catalog=Sputnik "& _ 
            "GAV;Data Source=192.168.19.53")>  _
        Public ReadOnly Property ConnStr_SputnikGAV() As String
            Get
                Return CType(Me("ConnStr_SputnikGAV"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\192.168.19.54\ReportDropIn$")>  _
        Public ReadOnly Property ReportDropInFolder() As String
            Get
                Return CType(Me("ReportDropInFolder"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("\\192.168.19.54\CVDropIn$")>  _
        Public ReadOnly Property CVDropInFolder() As String
            Get
                Return CType(Me("CVDropInFolder"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("<font face=""Trebuchet MS"" size=""2""><b>Achtung:</b> {0} Rapporte konnten nicht ric"& _ 
            "htig zugeordnet werden.<br>Bitte gehen Sie in <b>Hauptübersicht: Rapportverwaltu"& _ 
            "ng -> Neu -> Automatische Rapportimport</b> und ordnen Sie die Rapporte manuelle"& _ 
            " zu.")>  _
        Public ReadOnly Property ERROR_Message_ReportNotFounded_MailBody() As String
            Get
                Return CType(Me("ERROR_Message_ReportNotFounded_MailBody"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spJobPlattforms;Data Source=192.168.19.55;Current Language=German"& _ 
            "")>  _
        Public ReadOnly Property ConnStr_Jobplattforms() As String
            Get
                Return CType(Me("ConnStr_Jobplattforms"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spContract;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_New_spContract() As String
            Get
                Return CType(Me("ConnStr_New_spContract"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("email-smtp.eu-west-1.amazonaws.com")>  _
        Public ReadOnly Property MySmtpServer() As String
            Get
                Return CType(Me("MySmtpServer"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("spwebService@sputnik-it.ch")>  _
        Public ReadOnly Property errMailSubject() As String
            Get
                Return CType(Me("errMailSubject"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spContract;Data Source=192.168.19.49;Current Language=German;Pool"& _ 
            "ing=false")>  _
        Public ReadOnly Property ConnStr_Vak() As String
            Get
                Return CType(Me("ConnStr_Vak"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spContract;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property SettingDb_Connection() As String
            Get
                Return CType(Me("SettingDb_Connection"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spContract;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_spContract() As String
            Get
                Return CType(Me("ConnStr_spContract"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spContract;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_JobCh() As String
            Get
                Return CType(Me("ConnStr_JobCh"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=GAV_PVL16092019;Data Source=192.168.19.49;Current Language=German"& _ 
            "")>  _
        Public ReadOnly Property ConnStr_SputnikCurrentPVLDb() As String
            Get
                Return CType(Me("ConnStr_SputnikCurrentPVLDb"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=SpUpdates;Data Source=192.168.19.49;Current Language=German;Max P"& _ 
            "ool Size=1000")>  _
        Public ReadOnly Property ConnStr_SputnikUpdateSystem() As String
            Get
                Return CType(Me("ConnStr_SputnikUpdateSystem"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spScanJobs;Data Source=192.168.19.49;Current Language=German;Max "& _ 
            "Pool Size=1000")>  _
        Public ReadOnly Property ConnStr_Scanning() As String
            Get
                Return CType(Me("ConnStr_Scanning"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spSystemInfo;Data Source=192.168.19.49;Current Language=German;Ma"& _ 
            "x Pool Size=1000")>  _
        Public ReadOnly Property ConnStr_SystemInfo() As String
            Get
                Return CType(Me("ConnStr_SystemInfo"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=applicant;Data Source=192.168.19.49")>  _
        Public ReadOnly Property ConnStr_Applicant() As String
            Get
                Return CType(Me("ConnStr_Applicant"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spPVLPublicData;Data Source=192.168.19.49;Current Language=German"& _ 
            "")>  _
        Public ReadOnly Property ConnStr_PVLPublicInfo() As String
            Get
                Return CType(Me("ConnStr_PVLPublicInfo"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spSystemInfo;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property Connstr_spSystemInfo_2016() As String
            Get
                Return CType(Me("Connstr_spSystemInfo_2016"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spScanJobs;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_Scanning_2016() As String
            Get
                Return CType(Me("ConnStr_Scanning_2016"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spCVLizerBaseInfo;Data Source=192.168.19.49;Current Language=Germ"& _ 
            "an")>  _
        Public ReadOnly Property ConnStr_cvLizer() As String
            Get
                Return CType(Me("ConnStr_cvLizer"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spPublicData;Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_spPublicData() As String
            Get
                Return CType(Me("ConnStr_spPublicData"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog=spJobPlattforms;Data Source=192.168.19.49;Current Language=German"& _ 
            "")>  _
        Public ReadOnly Property ConnStr_spJobplattforms() As String
            Get
                Return CType(Me("ConnStr_spJobplattforms"),String)
            End Get
        End Property
        
        <Global.System.Configuration.ApplicationScopedSettingAttribute(),  _
         Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
         Global.System.Configuration.DefaultSettingValueAttribute("Password=12SPContract_User34;Persist Security Info=True;User ID=SPContract_User;I"& _ 
            "nitial Catalog={0};Data Source=192.168.19.49;Current Language=German")>  _
        Public ReadOnly Property ConnStr_CurrentPVLArchiveDbName() As String
            Get
                Return CType(Me("ConnStr_CurrentPVLArchiveDbName"),String)
            End Get
        End Property
    End Class
End Namespace

Namespace My
    
    <Global.Microsoft.VisualBasic.HideModuleNameAttribute(),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Module MySettingsProperty
        
        <Global.System.ComponentModel.Design.HelpKeywordAttribute("My.Settings")>  _
        Friend ReadOnly Property Settings() As Global.wsSPS_AvamServices.My.MySettings
            Get
                Return Global.wsSPS_AvamServices.My.MySettings.Default
            End Get
        End Property
    End Module
End Namespace