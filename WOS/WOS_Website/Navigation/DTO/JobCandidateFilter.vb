Imports Microsoft.VisualBasic
Imports System.Collections.Generic

Public Class JobCandidateFilter
    Dim _CustomerID As String
    Public Property CustomerID() As String
        Get
            Return _CustomerID
        End Get

        Set(ByVal value As String)
            _CustomerID = value
        End Set
    End Property

    Dim _Branch As String
    Public Property Branch() As String
        Get
            Return _Branch
        End Get

        Set(ByVal value As String)
            _Branch = value
        End Set
    End Property

    Public ReadOnly Property BranchEntries() As String()
        Get
            If (String.IsNullOrEmpty(Branch)) Then
                Dim strEmpty(-1) As String
                Return strEmpty
            End If
            Return Branch.Split("#")
        End Get
    End Property

    Dim _SearchQuery As String
    Public Property SearchQuery() As String
        Get
            Return _SearchQuery
        End Get

        Set(ByVal value As String)
            _SearchQuery = value
        End Set
    End Property

    Dim _IsValid As Boolean
    Public Property IsValid() As String
        Get
            Return _IsValid
        End Get

        Set(ByVal value As String)
            _IsValid = value
        End Set
    End Property
End Class
