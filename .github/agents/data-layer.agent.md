---
name: data-layer
description: >
  Agent for the Data Layer — AppDbContext, EF models, DbInitializer, and service interfaces.
  Use this agent when working on database schema changes, new EF entities, migrations,
  seed data, or service interface contracts.
---

# Data Layer Agent

## Responsibility

This agent owns the **EF6 Code-First data layer** — the DbContext, all entity model classes, the DB initializer/seeder, and the service interface definitions.

## Scope

- `Data/AppDbContext.vb`
- `Data/AppDbInitializer.vb`
- `Models/Metadata/*.vb` — all entity classes
- `Services/I*.vb` — all service interfaces
- `Global.asax.vb` — DB initialization registration

## Entity Classes

| File | Entities |
|---|---|
| `AppList.vb` | `AppList` |
| `AppListField.vb` | `AppListField` |
| `AppForm.vb` | `AppForm` |
| `AppView.vb` | `AppView` |
| `AppPage.vb` | `AppPage` |
| `AppWorkflow.vb` | `AppWorkflow`, `AppWorkflowState`, `AppWorkflowTransition` |
| `AppSecurity.vb` | `AppRole`, `AppPermission`, `AppRolePermission`, `AppUserRole` |
| `AppTransaction.vb` | `AppItem`, `AppItemValue`, `AppWorkflowTransaction`, `AppAttachment` |

## DbContext Notes

- Connection string: `DefaultConnection` (LocalDB MSSQLLocalDB)
- Cascade delete disabled on `AppWorkflowTransition` (both `FromState` and `ToState` FKs) to prevent EF cycle errors
- `AppDbInitializer` uses `CreateDatabaseIfNotExists` and seeds system roles + permissions

## DB Initializer

Registered in `Global.asax.vb`:

```vb
Database.SetInitializer(New AppDbInitializer())
Using ctx As New AppDbContext()
	ctx.Database.Initialize(False)
End Using
```

Switch to `MigrateDatabaseToLatestVersion` after enabling EF Migrations.

## Service Interface Summary

| Interface | Implementation | Purpose |
|---|---|---|
| `IListService` | `ListService` | List + field CRUD |
| `IFormService` | `FormService` | Form save/publish |
| `IViewService` | `ViewService` | View save/set-default |
| `IPageService` | `PageService` | Page save/set-default-home |
| `IWorkflowService` | `WorkflowService` | Workflow design + runtime execution |
| `ISecurityService` | `SecurityService` | Roles, permissions, user-role assignment |
| `IItemService` | `ItemService` | Item create/edit/delete + EAV values |

## Coding Standards

- VB.NET, .NET Framework 4.8, Entity Framework 6.4.4
- `Newtonsoft.Json` 13.0.3 for JSON serialization
- All services accept `AppDbContext` via constructor (manual DI)
- Soft-delete pattern on `AppList` (`IsActive` flag)
