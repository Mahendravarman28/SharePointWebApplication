Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services

Namespace Controllers

    <Authorize>
    Public Class SecurityController
        Inherits Controller

        Private ReadOnly _securityService As ISecurityService
        Private ReadOnly _listService As IListService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _securityService = New SecurityService(_db)
            _listService = New ListService(_db)
        End Sub

        ' GET: /Security/Roles
        Public Function Roles() As ActionResult
            Return View(_securityService.GetRoles())
        End Function

        ' GET: /Security/CreateRole
        Public Function CreateRole() As ActionResult
            Return View(New AppRole())
        End Function

        ' POST: /Security/CreateRole
        <HttpPost, ValidateAntiForgeryToken>
        Public Function CreateRole(model As AppRole) As ActionResult
            If ModelState.IsValid Then
                _securityService.SaveRole(model)
                Return RedirectToAction("Roles")
            End If
            Return View(model)
        End Function

        ' GET: /Security/EditRole/5
        Public Function EditRole(id As Integer) As ActionResult
            Dim role = _securityService.GetRoleById(id)
            If role Is Nothing Then Return HttpNotFound()
            Return View(role)
        End Function

        ' POST: /Security/EditRole
        <HttpPost, ValidateAntiForgeryToken>
        Public Function EditRole(model As AppRole) As ActionResult
            If ModelState.IsValid Then
                _securityService.SaveRole(model)
                Return RedirectToAction("Roles")
            End If
            Return View(model)
        End Function

        ' POST: /Security/DeleteRole/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function DeleteRole(id As Integer) As ActionResult
            _securityService.DeleteRole(id)
            Return RedirectToAction("Roles")
        End Function

        ' GET: /Security/Permissions
        Public Function Permissions() As ActionResult
            Return View(_securityService.GetPermissions())
        End Function

        ' GET: /Security/RolePermissions/5
        Public Function RolePermissions(id As Integer) As ActionResult
            Dim role = _securityService.GetRoleById(id)
            If role Is Nothing Then Return HttpNotFound()
            ViewBag.Role = role
            ViewBag.AllPermissions = _securityService.GetPermissions()
            ViewBag.AssignedPermissions = _securityService.GetRolePermissions(id)
            Return View()
        End Function

        ' POST: /Security/AssignPermission
        <HttpPost>
        Public Function AssignPermission(roleId As Integer, permissionId As Integer, listId As Integer?) As JsonResult
            Dim ok = _securityService.AssignPermission(roleId, permissionId, listId)
            Return Json(New With {.success = ok})
        End Function

        ' POST: /Security/RevokePermission
        <HttpPost>
        Public Function RevokePermission(rolePermissionId As Integer) As JsonResult
            Dim ok = _securityService.RevokePermission(rolePermissionId)
            Return Json(New With {.success = ok})
        End Function

        ' GET: /Security/UserRoles
        Public Function UserRoles() As ActionResult
            ViewBag.Roles = _securityService.GetRoles()
            ViewBag.Lists = _listService.GetAllLists()
            Return View()
        End Function

        ' POST: /Security/AssignUserRole
        <HttpPost>
        Public Function AssignUserRole(userId As String, roleId As Integer, listId As Integer?) As JsonResult
            Dim ok = _securityService.AssignUserRole(userId, roleId, listId)
            Return Json(New With {.success = ok})
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
