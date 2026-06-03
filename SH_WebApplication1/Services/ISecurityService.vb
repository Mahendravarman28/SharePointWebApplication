Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface ISecurityService
        Function GetRoles() As List(Of AppRole)
        Function GetRoleById(roleId As Integer) As AppRole
        Function SaveRole(role As AppRole) As AppRole
        Function DeleteRole(roleId As Integer) As Boolean
        Function GetPermissions() As List(Of AppPermission)
        Function GetRolePermissions(roleId As Integer) As List(Of AppRolePermission)
        Function AssignPermission(roleId As Integer, permissionId As Integer, listId As Integer?) As Boolean
        Function RevokePermission(rolePermissionId As Integer) As Boolean
        Function AssignUserRole(userId As String, roleId As Integer, listId As Integer?) As Boolean
        Function GetUserRoles(userId As String) As List(Of AppRole)
        Function UserHasPermission(userId As String, permissionCode As String, listId As Integer?) As Boolean
    End Interface

End Namespace
