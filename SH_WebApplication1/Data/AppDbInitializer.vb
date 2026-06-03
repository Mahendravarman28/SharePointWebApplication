Imports System.Data.Entity
Imports SH_WebApplication1.Models.Metadata

Namespace Data

    ''' <summary>
    ''' Creates the database on first run; seeds default roles and permissions.
    ''' Switch to MigrateDatabaseToLatestVersion after enabling EF Migrations.
    ''' </summary>
    Public Class AppDbInitializer
        Inherits CreateDatabaseIfNotExists(Of AppDbContext)

        Protected Overrides Sub Seed(context As AppDbContext)
            MyBase.Seed(context)

            ' Seed default roles
            If Not context.AppRoles.Any() Then
                context.AppRoles.AddRange(New List(Of AppRole) From {
                    New AppRole With {.RoleName = "Admin", .Description = "Full platform administrator", .IsSystem = True},
                    New AppRole With {.RoleName = "ListDesigner", .Description = "Can create and edit list schemas", .IsSystem = True},
                    New AppRole With {.RoleName = "WorkflowDesigner", .Description = "Can design workflows", .IsSystem = True},
                    New AppRole With {.RoleName = "Contributor", .Description = "Can create and edit own items", .IsSystem = True},
                    New AppRole With {.RoleName = "Reviewer", .Description = "Can review submitted items", .IsSystem = True},
                    New AppRole With {.RoleName = "Approver", .Description = "Can approve or reject items", .IsSystem = True},
                    New AppRole With {.RoleName = "Viewer", .Description = "Read-only access", .IsSystem = True}
                })
            End If

            ' Seed default permissions
            If Not context.AppPermissions.Any() Then
                context.AppPermissions.AddRange(New List(Of AppPermission) From {
                    New AppPermission With {.PermissionName = "Create List", .PermissionCode = "LIST_CREATE", .PermissionLevel = "Application"},
                    New AppPermission With {.PermissionName = "Edit List Schema", .PermissionCode = "LIST_EDIT", .PermissionLevel = "List"},
                    New AppPermission With {.PermissionName = "Publish Form", .PermissionCode = "FORM_PUBLISH", .PermissionLevel = "Form"},
                    New AppPermission With {.PermissionName = "Design Workflow", .PermissionCode = "WF_DESIGN", .PermissionLevel = "Module"},
                    New AppPermission With {.PermissionName = "Create Item", .PermissionCode = "ITEM_CREATE", .PermissionLevel = "List"},
                    New AppPermission With {.PermissionName = "Edit Own Item", .PermissionCode = "ITEM_EDIT_OWN", .PermissionLevel = "Record"},
                    New AppPermission With {.PermissionName = "Approve Item", .PermissionCode = "ITEM_APPROVE", .PermissionLevel = "Action"},
                    New AppPermission With {.PermissionName = "View All Items", .PermissionCode = "ITEM_VIEW_ALL", .PermissionLevel = "List"},
                    New AppPermission With {.PermissionName = "Delete Item", .PermissionCode = "ITEM_DELETE", .PermissionLevel = "List"},
                    New AppPermission With {.PermissionName = "Export Items", .PermissionCode = "ITEM_EXPORT", .PermissionLevel = "List"}
                })
            End If

            context.SaveChanges()
        End Sub

    End Class

End Namespace
