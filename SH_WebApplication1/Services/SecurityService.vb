Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports System.Data.Entity

Namespace Services

    Public Class SecurityService
        Implements ISecurityService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetRoles() As List(Of AppRole) Implements ISecurityService.GetRoles
            Return _db.AppRoles.OrderBy(Function(r) r.RoleName).ToList()
        End Function

        Public Function GetRoleById(roleId As Integer) As AppRole Implements ISecurityService.GetRoleById
            Return _db.AppRoles.Find(roleId)
        End Function

        Public Function SaveRole(role As AppRole) As AppRole Implements ISecurityService.SaveRole
            If role.RoleId = 0 Then
                _db.AppRoles.Add(role)
            Else
                Dim existing = _db.AppRoles.Find(role.RoleId)
                If existing IsNot Nothing Then
                    existing.RoleName = role.RoleName
                    existing.Description = role.Description
                End If
            End If
            _db.SaveChanges()
            Return role
        End Function

        Public Function DeleteRole(roleId As Integer) As Boolean Implements ISecurityService.DeleteRole
            Dim role = _db.AppRoles.Find(roleId)
            If role Is Nothing OrElse role.IsSystem Then Return False
            _db.AppRoles.Remove(role)
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetPermissions() As List(Of AppPermission) Implements ISecurityService.GetPermissions
            Return _db.AppPermissions.OrderBy(Function(p) p.PermissionLevel).ThenBy(Function(p) p.PermissionName).ToList()
        End Function

        Public Function GetRolePermissions(roleId As Integer) As List(Of AppRolePermission) Implements ISecurityService.GetRolePermissions
            Return _db.AppRolePermissions _
                      .Include(Function(rp) rp.Permission) _
                      .Where(Function(rp) rp.RoleId = roleId).ToList()
        End Function

        Public Function AssignPermission(roleId As Integer, permissionId As Integer, listId As Integer?) As Boolean Implements ISecurityService.AssignPermission
            Dim exists = _db.AppRolePermissions.Any(Function(rp) rp.RoleId = roleId AndAlso rp.PermissionId = permissionId AndAlso rp.ListId = listId)
            If exists Then Return True
            _db.AppRolePermissions.Add(New AppRolePermission With {.RoleId = roleId, .PermissionId = permissionId, .ListId = listId})
            _db.SaveChanges()
            Return True
        End Function

        Public Function RevokePermission(rolePermissionId As Integer) As Boolean Implements ISecurityService.RevokePermission
            Dim rp = _db.AppRolePermissions.Find(rolePermissionId)
            If rp Is Nothing Then Return False
            _db.AppRolePermissions.Remove(rp)
            _db.SaveChanges()
            Return True
        End Function

        Public Function AssignUserRole(userId As String, roleId As Integer, listId As Integer?) As Boolean Implements ISecurityService.AssignUserRole
            Dim exists = _db.AppUserRoles.Any(Function(ur) ur.UserId = userId AndAlso ur.RoleId = roleId AndAlso ur.ListId = listId)
            If exists Then Return True
            _db.AppUserRoles.Add(New AppUserRole With {.UserId = userId, .RoleId = roleId, .ListId = listId})
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetUserRoles(userId As String) As List(Of AppRole) Implements ISecurityService.GetUserRoles
            Dim roleIds = _db.AppUserRoles.Where(Function(ur) ur.UserId = userId).Select(Function(ur) ur.RoleId).ToList()
            Return _db.AppRoles.Where(Function(r) roleIds.Contains(r.RoleId)).ToList()
        End Function

        Public Function UserHasPermission(userId As String, permissionCode As String, listId As Integer?) As Boolean Implements ISecurityService.UserHasPermission
            Dim userRoleIds = _db.AppUserRoles _
                                 .Where(Function(ur) ur.UserId = userId AndAlso (ur.ListId Is Nothing OrElse ur.ListId = listId)) _
                                 .Select(Function(ur) ur.RoleId).ToList()
            Dim permId = _db.AppPermissions.Where(Function(p) p.PermissionCode = permissionCode).Select(Function(p) p.PermissionId).FirstOrDefault()
            If permId = 0 Then Return False
            Return _db.AppRolePermissions.Any(Function(rp) userRoleIds.Contains(rp.RoleId) AndAlso rp.PermissionId = permId)
        End Function


        ''' <summary>Get fields visible and editable to a user based on field-level permissions</summary>
        Public Function GetVisibleFields(listId As Integer, userId As String) As List(Of AppListField) Implements ISecurityService.GetVisibleFields
            ' Get user role IDs
            Dim userRoleIds = _db.AppUserRoles _
                                 .Where(Function(ur) ur.UserId = userId) _
                                 .Select(Function(ur) ur.RoleId).ToList()
            ' If user is admin or no roles, return all fields
            If Not userRoleIds.Any() Then
                Return _db.AppListFields.Where(Function(f) f.ListId = listId).OrderBy(Function(f) f.DisplayOrder).ToList()
            End If
            ' Get field permissions for user's roles
            Dim fieldPerms = _db.AppFieldPermissions _
                                .Where(Function(fp) userRoleIds.Contains(fp.RoleId)) _
                                .GroupBy(Function(fp) fp.FieldId) _
                                .Select(Function(grp) New With {
                                    .FieldId = grp.Key,
                                    .CanView = grp.Any(Function(p) p.CanView),
                                    .CanEdit = grp.Any(Function(p) p.CanEdit)
                                }).ToList()
            ' Load all fields for this list
            Dim fields = _db.AppListFields _
                            .Where(Function(f) f.ListId = listId) _
                            .OrderBy(Function(f) f.DisplayOrder).ToList()
            ' If no field-level permissions exist, return all fields
            If Not fieldPerms.Any() Then Return fields
            ' Filter by CanView
            Dim visibleFieldIds = fieldPerms.Where(Function(fp) fp.CanView).Select(Function(fp) fp.FieldId).ToList()
            Return fields.Where(Function(f) visibleFieldIds.Contains(f.FieldId)).ToList()
        End Function

    End Class

End Namespace
