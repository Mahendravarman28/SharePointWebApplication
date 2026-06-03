Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppItems")>
    Public Class AppItem

        <Key>
        Public Property ItemId As Integer

        Public Property ListId As Integer

        <MaxLength(50)>
        Public Property ItemNo As String

        <MaxLength(100)>
        Public Property CurrentStatus As String = "Draft"

        Public Property CurrentStateId As Integer?

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <MaxLength(256)>
        Public Property ModifiedBy As String

        Public Property ModifiedDate As DateTime?

        Public Overridable Property Values As ICollection(Of AppItemValue) = New List(Of AppItemValue)()

        Public Overridable Property WorkflowTransactions As ICollection(Of AppWorkflowTransaction) = New List(Of AppWorkflowTransaction)()

        Public Overridable Property Attachments As ICollection(Of AppAttachment) = New List(Of AppAttachment)()

    End Class


    <Table("AppItemValues")>
    Public Class AppItemValue

        <Key>
        Public Property ItemValueId As Integer

        Public Property ItemId As Integer
        Public Property FieldId As Integer

        Public Property FieldValueText As String
        Public Property FieldValueNumber As Decimal?
        Public Property FieldValueDate As DateTime?
        Public Property FieldValueJson As String

        <ForeignKey("ItemId")>
        Public Overridable Property Item As AppItem

        <ForeignKey("FieldId")>
        Public Overridable Property Field As AppListField

    End Class


    <Table("AppWorkflowTransactions")>
    Public Class AppWorkflowTransaction

        <Key>
        Public Property TransactionId As Integer

        Public Property ItemId As Integer
        Public Property WorkflowId As Integer
        Public Property TransitionId As Integer?

        <MaxLength(100)>
        Public Property FromState As String

        <MaxLength(100)>
        Public Property ToState As String

        <MaxLength(256)>
        Public Property ActionBy As String

        Public Property ActionDate As DateTime = DateTime.Now

        Public Property Comments As String

        <ForeignKey("ItemId")>
        Public Overridable Property Item As AppItem

    End Class


    <Table("AppAttachments")>
    Public Class AppAttachment

        <Key>
        Public Property AttachmentId As Integer

        Public Property ItemId As Integer

        <Required, MaxLength(500)>
        Public Property FileName As String

        <MaxLength(200)>
        Public Property ContentType As String

        Public Property FileSizeBytes As Long

        <Required, MaxLength(1000)>
        Public Property FilePath As String

        <MaxLength(256)>
        Public Property UploadedBy As String

        Public Property UploadedDate As DateTime = DateTime.Now

        <ForeignKey("ItemId")>
        Public Overridable Property Item As AppItem

    End Class

End Namespace
