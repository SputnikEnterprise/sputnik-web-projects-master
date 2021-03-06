'------------------------------------------------------------------------------
' <auto-generated>
'     Dieser Code wurde von einem Tool generiert.
'     Laufzeitversion:4.0.30319.239
'
'     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
'     der Code erneut generiert wird.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System
Imports System.Runtime.Serialization

Namespace CockpitWCFServices
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),  _
     System.Runtime.Serialization.DataContractAttribute(Name:="TableInfo", [Namespace]:="http://schemas.datacontract.org/2004/07/CockpitWSFServices"),  _
     System.SerializableAttribute()>  _
    Partial Public Class TableInfo
        Inherits Object
        Implements System.Runtime.Serialization.IExtensibleDataObject, System.ComponentModel.INotifyPropertyChanged
        
        <System.NonSerializedAttribute()>  _
        Private extensionDataField As System.Runtime.Serialization.ExtensionDataObject
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private CompressedTableDataField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private CompressedTableSchemaField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private CustomerNameField As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private MDGuidsField() As String
        
        <System.Runtime.Serialization.OptionalFieldAttribute()>  _
        Private TableNameField As String
        
        <Global.System.ComponentModel.BrowsableAttribute(false)>  _
        Public Property ExtensionData() As System.Runtime.Serialization.ExtensionDataObject Implements System.Runtime.Serialization.IExtensibleDataObject.ExtensionData
            Get
                Return Me.extensionDataField
            End Get
            Set
                Me.extensionDataField = value
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property CompressedTableData() As String
            Get
                Return Me.CompressedTableDataField
            End Get
            Set
                If (Object.ReferenceEquals(Me.CompressedTableDataField, value) <> true) Then
                    Me.CompressedTableDataField = value
                    Me.RaisePropertyChanged("CompressedTableData")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property CompressedTableSchema() As String
            Get
                Return Me.CompressedTableSchemaField
            End Get
            Set
                If (Object.ReferenceEquals(Me.CompressedTableSchemaField, value) <> true) Then
                    Me.CompressedTableSchemaField = value
                    Me.RaisePropertyChanged("CompressedTableSchema")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property CustomerName() As String
            Get
                Return Me.CustomerNameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.CustomerNameField, value) <> true) Then
                    Me.CustomerNameField = value
                    Me.RaisePropertyChanged("CustomerName")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property MDGuids() As String()
            Get
                Return Me.MDGuidsField
            End Get
            Set
                If (Object.ReferenceEquals(Me.MDGuidsField, value) <> true) Then
                    Me.MDGuidsField = value
                    Me.RaisePropertyChanged("MDGuids")
                End If
            End Set
        End Property
        
        <System.Runtime.Serialization.DataMemberAttribute()>  _
        Public Property TableName() As String
            Get
                Return Me.TableNameField
            End Get
            Set
                If (Object.ReferenceEquals(Me.TableNameField, value) <> true) Then
                    Me.TableNameField = value
                    Me.RaisePropertyChanged("TableName")
                End If
            End Set
        End Property
        
        Public Event PropertyChanged As System.ComponentModel.PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
        
        Protected Sub RaisePropertyChanged(ByVal propertyName As String)
            Dim propertyChanged As System.ComponentModel.PropertyChangedEventHandler = Me.PropertyChangedEvent
            If (Not (propertyChanged) Is Nothing) Then
                propertyChanged(Me, New System.ComponentModel.PropertyChangedEventArgs(propertyName))
            End If
        End Sub
    End Class
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0"),  _
     System.ServiceModel.ServiceContractAttribute(ConfigurationName:="CockpitWCFServices.ITableDataUploadService")>  _
    Public Interface ITableDataUploadService
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/ITableDataUploadService/ProcessTableData", ReplyAction:="http://tempuri.org/ITableDataUploadService/ProcessTableDataResponse")>  _
        Function ProcessTableData(ByVal tableInfo As CockpitWCFServices.TableInfo) As Boolean
        
        <System.ServiceModel.OperationContractAttribute(Action:="http://tempuri.org/ITableDataUploadService/LogError", ReplyAction:="http://tempuri.org/ITableDataUploadService/LogErrorResponse")>  _
        Sub LogError(ByVal shortDescription As String, ByVal exception As String, ByVal customerName As String)
    End Interface
    
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Public Interface ITableDataUploadServiceChannel
        Inherits CockpitWCFServices.ITableDataUploadService, System.ServiceModel.IClientChannel
    End Interface
    
    <System.Diagnostics.DebuggerStepThroughAttribute(),  _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")>  _
    Partial Public Class TableDataUploadServiceClient
        Inherits System.ServiceModel.ClientBase(Of CockpitWCFServices.ITableDataUploadService)
        Implements CockpitWCFServices.ITableDataUploadService
        
        Public Sub New()
            MyBase.New
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String)
            MyBase.New(endpointConfigurationName)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As String)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal endpointConfigurationName As String, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(endpointConfigurationName, remoteAddress)
        End Sub
        
        Public Sub New(ByVal binding As System.ServiceModel.Channels.Binding, ByVal remoteAddress As System.ServiceModel.EndpointAddress)
            MyBase.New(binding, remoteAddress)
        End Sub
        
        Public Function ProcessTableData(ByVal tableInfo As CockpitWCFServices.TableInfo) As Boolean Implements CockpitWCFServices.ITableDataUploadService.ProcessTableData
            Return MyBase.Channel.ProcessTableData(tableInfo)
        End Function
        
        Public Sub LogError(ByVal shortDescription As String, ByVal exception As String, ByVal customerName As String) Implements CockpitWCFServices.ITableDataUploadService.LogError
            MyBase.Channel.LogError(shortDescription, exception, customerName)
        End Sub
    End Class
End Namespace
