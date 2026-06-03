Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppViews")>
    Public Class AppView

        <Key>
        Public Property ViewId As Integer

        Public Property ListId As Integer

        <Required, MaxLength(200)>
        Public Property ViewName As String

        ''' <summary>Grid, Card, Kanban, Grouped, Calendar, Summary</summary>
        <MaxLength(50)>
        Public Property ViewType As String = "Grid"

        ''' <summary>JSON: columns, filters, sorts, groupBy, conditionalFormatting</summary>
        Public Property ViewConfigJson As String

        Public Property IsDefault As Boolean = False
        Public Property IsPublic As Boolean = True

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <ForeignKey("ListId")>
        Public Overridable Property List As AppList

    End Class

End Namespace
