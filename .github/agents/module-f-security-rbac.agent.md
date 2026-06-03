---
name: module-f-security-rbac
description: >
  Agent for Module F — Security and Role-Based Access Control (RBAC).
  Use this agent when working on roles, permissions, user-role assignments,
  permission checks, or field-level/list-level security.
---

# Module F — Security / RBAC Agent

## Responsibility

This agent owns everything related to **role-based access control** across the entire platform — from application-level down to field-level security.

## Scope

- `Controllers/SecurityController.vb`
- `Views/Security/` (Roles, CreateRole, EditRole, RolePermissions, Permissions, UserRoles)
- `Models/Metadata/AppSecurity.vb` (AppRole, AppPermission, AppRolePermission, AppUserRole)
- `Services/ISecurityService.vb` + `Services/SecurityService.vb`

## Key Tasks

1. View and manage all roles (including seeded system roles)
2. Create custom roles
3. Assign/revoke permissions to a role (globally or scoped to a list)
4. Assign users to roles (globally or scoped to a list)
5. Runtime permission checks via `UserHasPermission(userId, permissionCode, listId?)`

## Access Control Levels

| Level | Description |
|---|---|
| Application | Platform-wide permissions (e.g., Create List) |
| Module | Module-level (e.g., Design Workflow) |
| List | Per-list permissions (e.g., Create Item, View All Items) |
| Field | Field-level visibility (e.g., Salary visible to HR only) |
| Form | Form publishing permissions |
| View | View-level access |
| Action | Action-level (Approve, Reject) |
| Record | Row-level security (optional advanced) |

## Seeded System Roles

| Role | Description |
|---|---|
| Admin | Full platform access |
| ListDesigner | Create and edit list schemas |
| WorkflowDesigner | Design workflows |
| Contributor | Create and edit own items |
| Reviewer | Review submitted items |
| Approver | Approve or reject items |
| Viewer | Read-only access |

## Seeded Permissions

| Code | Name | Level |
|---|---|---|
| `LIST_CREATE` | Create List | Application |
| `LIST_EDIT` | Edit List Schema | List |
| `FORM_PUBLISH` | Publish Form | Form |
| `WF_DESIGN` | Design Workflow | Module |
| `ITEM_CREATE` | Create Item | List |
| `ITEM_EDIT_OWN` | Edit Own Item | Record |
| `ITEM_APPROVE` | Approve Item | Action |
| `ITEM_VIEW_ALL` | View All Items | List |
| `ITEM_DELETE` | Delete Item | List |
| `ITEM_EXPORT` | Export Items | List |

## Database Tables

| Table | Purpose |
|---|---|
| `AppRoles` | Role master (RoleId, RoleName, IsSystem) |
| `AppPermissions` | Permission registry (PermissionId, PermissionCode, PermissionLevel) |
| `AppRolePermissions` | Role ↔ Permission mapping (with optional ListId scope) |
| `AppUserRoles` | User ↔ Role mapping (with optional ListId scope) |

## Key Service Method

```vb
' Returns True if the user has the given permission (globally or for the list)
Function UserHasPermission(userId As String, permissionCode As String, listId As Integer?) As Boolean
```

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`
- System roles are protected from deletion (`IsSystem = True`)
- Ajax `$.post` for Assign/Revoke permission actions
- Scope: `ListId = Nothing` means global permission; `ListId = value` means list-scoped

## Navigation Entry Point

`/Security/Roles` — manage all roles and access permissions.
`/Security/UserRoles` — assign users to roles.
`/Security/Permissions` — view all registered permissions.
