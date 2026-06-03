Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppForms")>
    Public Class AppForm

        <Key>
        Public Property FormId As Integer

        Public Property ListId As Integer

        ''' <summary>New, Edit, Display</summary>
        <Required, MaxLength(20)>
        Public Property FormType As String

        <Required, MaxLength(200)>
        Public Property FormName As String

        ''' <summary>JSON layout definition</summary>
        Public Property LayoutJson As String

        Public Property IsPublished As Boolean = False

        Public Property VersionNo As Integer = 1

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <ForeignKey("ListId")>
        Public Overridable Property List As AppList

    End Class

End Namespace
