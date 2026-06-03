Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppLists")>
    Public Class AppList

        <Key>
        Public Property ListId As Integer

        <Required, MaxLength(200)>
        Public Property ListName As String

        <Required, MaxLength(100)>
        Public Property ListCode As String

        <MaxLength(1000)>
        Public Property Description As String

        Public Property IsActive As Boolean = True

        Public Property EnableAttachments As Boolean = False

        Public Property EnableVersioning As Boolean = False

        Public Property EnableApprovalWorkflow As Boolean = False

        Public Property VersionNo As Integer = 1

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <MaxLength(256)>
        Public Property ModifiedBy As String

        Public Property ModifiedDate As DateTime?

        Public Overridable Property Fields As ICollection(Of AppListField) = New List(Of AppListField)()
        Public Overridable Property Forms As ICollection(Of AppForm) = New List(Of AppForm)()
        Public Overridable Property Views As ICollection(Of AppView) = New List(Of AppView)()
        Public Overridable Property Pages As ICollection(Of AppPage) = New List(Of AppPage)()
        Public Overridable Property Workflows As ICollection(Of AppWorkflow) = New List(Of AppWorkflow)()

    End Class

End Namespace
