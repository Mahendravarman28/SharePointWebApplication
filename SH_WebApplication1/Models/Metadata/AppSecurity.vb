Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppRoles")>
    Public Class AppRole

        <Key>
        Public Property RoleId As Integer

        <Required, MaxLength(100)>
        Public Property RoleName As String

        <MaxLength(500)>
        Public Property Description As String

        Public Property IsSystem As Boolean = False

    End Class


    <Table("AppPermissions")>
    Public Class AppPermission

        <Key>
        Public Property PermissionId As Integer

        <Required, MaxLength(200)>
        Public Property PermissionName As String

        <Required, MaxLength(100)>
        Public Property PermissionCode As String

        ''' <summary>Application, Module, List, Field, Form, View, Action, Record</summary>
        <MaxLength(50)>
        Public Property PermissionLevel As String

        <MaxLength(500)>
        Public Property Description As String

    End Class


    <Table("AppRolePermissions")>
    Public Class AppRolePermission

        <Key>
        Public Property RolePermissionId As Integer

        Public Property RoleId As Integer
        Public Property PermissionId As Integer
        Public Property ListId As Integer?

        <ForeignKey("RoleId")>
        Public Overridable Property Role As AppRole

        <ForeignKey("PermissionId")>
        Public Overridable Property Permission As AppPermission

    End Class


    <Table("AppUserRoles")>
    Public Class AppUserRole

        <Key>
        Public Property UserRoleId As Integer

        <Required, MaxLength(256)>
        Public Property UserId As String

        Public Property RoleId As Integer
        Public Property ListId As Integer?

        <ForeignKey("RoleId")>
        Public Overridable Property Role As AppRole

    End Class

End Namespace
