Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppPages")>
    Public Class AppPage

        <Key>
        Public Property PageId As Integer

        Public Property ListId As Integer

        <Required, MaxLength(200)>
        Public Property PageName As String

        ''' <summary>JSON widget layout</summary>
        Public Property LayoutJson As String

        Public Property IsDefaultHome As Boolean = False

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <ForeignKey("ListId")>
        Public Overridable Property List As AppList

    End Class

End Namespace
