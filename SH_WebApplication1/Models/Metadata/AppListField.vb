Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppListFields")>
    Public Class AppListField

        <Key>
        Public Property FieldId As Integer

        Public Property ListId As Integer

        <Required, MaxLength(100)>
        Public Property InternalName As String

        <Required, MaxLength(200)>
        Public Property DisplayName As String

        ''' <summary>
        ''' SingleLine, MultiLine, Number, Decimal, Currency, DateTime,
        ''' YesNo, Dropdown, MultiSelect, UserPicker, Lookup,
        ''' FileAttachment, Calculated, AutoNumber, Status
        ''' </summary>
        <Required, MaxLength(50)>
        Public Property DataType As String

        Public Property IsRequired As Boolean = False
        Public Property IsUnique As Boolean = False

        <MaxLength(500)>
        Public Property DefaultValue As String

        <MaxLength(1000)>
        Public Property ValidationRule As String

        Public Property MinLength As Integer?
        Public Property MaxLength As Integer?

        <MaxLength(500)>
        Public Property RegexValidation As String

        ''' <summary>JSON config for dropdown source options</summary>
        Public Property DropdownConfig As String

        ''' <summary>JSON config for lookup: ListCode, ValueField, DisplayField</summary>
        Public Property LookupConfig As String

        Public Property IsSearchable As Boolean = True
        Public Property IsSortable As Boolean = True
        Public Property IsFilterable As Boolean = True
        Public Property IsVisibleInList As Boolean = True
        Public Property IsVisibleInForm As Boolean = True
        Public Property DisplayOrder As Integer = 0

        <ForeignKey("ListId")>
        Public Overridable Property List As AppList

    End Class

End Namespace
