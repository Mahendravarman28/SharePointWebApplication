# SharePoint-Like MVC Platform — Full Development Summary

**Project:** SH_WebApplication1
**Stack:** ASP.NET MVC 5 · VB.NET · .NET Framework 4.8 · Entity Framework 6.4.4 · Bootstrap 5 · jQuery 3.7
**Date:** June 2026
**Workspace:** `E:\VB_Projects\Sharepoint-Development\SH_WebApplication1\`

---

## Overview

A **metadata-driven SharePoint-like platform** built from scratch on top of an existing ASP.NET MVC 5 identity scaffold. The platform allows users to dynamically create lists, design forms and views, build home pages, configure approval workflows, and manage role-based permissions — all without writing code or SQL.

---

## Architecture Summary

```
┌─────────────────────────────────────────────────────────┐
│                   Presentation Layer                     │
│  ASP.NET MVC 5 · Razor .vbhtml · Bootstrap 5 · jQuery   │
│  Drag-and-drop designers (Forms / Views / Pages / WF)    │
└───────────────────┬─────────────────────────────────────┘
					│
┌───────────────────▼─────────────────────────────────────┐
│                   Business / Service Layer               │
│  IListService · IFormService · IViewService              │
│  IPageService · IWorkflowService                         │
│  ISecurityService · IItemService                         │
└───────────────────┬─────────────────────────────────────┘
					│
┌───────────────────▼─────────────────────────────────────┐
│                   Data Access Layer                      │
│  Entity Framework 6 Code-First · AppDbContext            │
│  SQL Server LocalDB (DefaultConnection)                  │
└───────────────────┬─────────────────────────────────────┘
					│
┌───────────────────▼─────────────────────────────────────┐
│                   Metadata + Transaction Tables          │
│  AppLists · AppListFields · AppForms · AppViews          │
│  AppPages · AppWorkflows · AppWorkflowStates             │
│  AppWorkflowTransitions · AppRoles · AppPermissions      │
│  AppRolePermissions · AppUserRoles                       │
│  AppItems · AppItemValues · AppWorkflowTransactions      │
│  AppAttachments                                          │
└─────────────────────────────────────────────────────────┘
```

---

## Modules Implemented

### Module A — Dynamic List Builder
**Agent:** `module-a-list-builder.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/ListBuilderController.vb` |
| Views | `Views/ListBuilder/` — Index, Create, Edit, Fields, AddField, EditField |
| Service | `Services/ListService.vb` implementing `IListService` |
| Models | `Models/Metadata/AppList.vb`, `AppListField.vb` |

**Capabilities:**
- Create lists with name, code, description, and feature flags (attachments / versioning / approval workflow)
- Add/edit/delete fields per list
- 15 supported data types: SingleLine, MultiLine, Number, Decimal, Currency, DateTime, YesNo, Dropdown, MultiSelect, UserPicker, Lookup, FileAttachment, Calculated, AutoNumber, Status
- Full field configuration: Required, Unique, Default, ValidationRule, Min/Max, Regex, Searchable, Sortable, Filterable, VisibleInList, VisibleInForm, DisplayOrder
- Soft-delete lists (`IsActive = false`)
- Navigation links from Fields view to all designers

---

### Module B — Dynamic Form Designer
**Agent:** `module-b-form-designer.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/FormDesignerController.vb` |
| Views | `Views/FormDesigner/` — Index, Designer, Preview |
| Service | `Services/FormService.vb` implementing `IFormService` |
| Models | `Models/Metadata/AppForm.vb` |

**Capabilities:**
- Three form types per list: New, Edit, Display
- Drag-and-drop form canvas with field palette and properties panel
- Sections with 1/2/3-column layout
- Per-field configuration: label, help text, Required, ReadOnly, Hidden
- Save form layout as JSON
- Version tracking on save
- Publish form (sets `IsPublished = true`)
- Live preview that renders the form from JSON

---

### Module C — Dynamic List View Designer
**Agent:** `module-c-view-designer.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/ViewDesignerController.vb` |
| Views | `Views/ViewDesigner/` — Index, Designer, Render |
| Service | `Services/ViewService.vb` implementing `IViewService` |
| Models | `Models/Metadata/AppView.vb` |

**Capabilities:**
- Six view types: Grid, Card, Kanban, Grouped, Calendar, Summary
- Column picker with drag-to-reorder
- Default sort (field + direction)
- Group-by field selection
- Page size configuration
- Public vs. personal view flag
- Set default view for a list
- Live data render with quick-search filter
- "Create New Item" button on rendered view

---

### Module D — Home Page Designer
**Agent:** `module-d-page-designer.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/PageDesignerController.vb` |
| Views | `Views/PageDesigner/` — Index, Designer, Home |
| Service | `Services/PageService.vb` implementing `IPageService` |
| Models | `Models/Metadata/AppPage.vb` |

**Capabilities:**
- Widget-based drag-and-drop page layout (rows × columns)
- 8 widget types: KpiCount, DataGrid, Chart, RecentItems, MyPendingItems, Announcement, ActionButtons, QuickLinks
- Per-widget column width (Bootstrap 2–12 cols)
- Default home page setting per list
- Live home page with KPI tiles, recent items table, and quick action links
- JSON-stored layout (`LayoutJson`)

---

### Module E — Workflow Engine
**Agent:** `module-e-workflow-engine.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/WorkflowDesignerController.vb` |
| Views | `Views/WorkflowDesigner/` — Index, Designer, Execute |
| Service | `Services/WorkflowService.vb` implementing `IWorkflowService` |
| Models | `Models/Metadata/AppWorkflow.vb` (AppWorkflow, AppWorkflowState, AppWorkflowTransition) |

**Capabilities:**
- State machine designer with visual state nodes and transitions table
- State colors: secondary (default), warning, info, success, danger, dark
- IsStart / IsEnd flags per state
- Transition: From → Action → To with required role and condition expression
- JSON-stored workflow (`WorkflowJson`) synced to `AppWorkflowStates` on save
- Publish workflow (one active workflow per list)
- Runtime execution: load available transitions for current item state
- Execute transition: updates `AppItem.CurrentStatus` + records `AppWorkflowTransaction` history
- Workflow history display on item page and execution panel

---

### Module F — Security / RBAC
**Agent:** `module-f-security-rbac.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/SecurityController.vb` |
| Views | `Views/Security/` — Roles, CreateRole, EditRole, RolePermissions, Permissions, UserRoles |
| Service | `Services/SecurityService.vb` implementing `ISecurityService` |
| Models | `Models/Metadata/AppSecurity.vb` (AppRole, AppPermission, AppRolePermission, AppUserRole) |

**Capabilities:**
- 7 seeded system roles: Admin, ListDesigner, WorkflowDesigner, Contributor, Reviewer, Approver, Viewer
- 10 seeded permissions across Application / Module / List / Record / Action / Form levels
- Assign/revoke permissions to roles (global or list-scoped)
- Assign users to roles (global or list-scoped)
- Runtime permission check: `UserHasPermission(userId, permissionCode, listId?)`
- System roles protected from deletion

---

### Items Runtime (Cross-cutting)
**Agent:** `items-runtime.agent.md`

| Item | Detail |
|------|--------|
| Controller | `Controllers/ItemsController.vb` |
| Views | `Views/Items/` — Create, Edit, Display |
| Service | `Services/ItemService.vb` implementing `IItemService` |
| Models | `Models/Metadata/AppTransaction.vb` (AppItem, AppItemValue, AppWorkflowTransaction, AppAttachment) |

**Capabilities:**
- Dynamic Create form driven by `AppListFields` (no hardcoded fields)
- EAV storage: one `AppItemValue` row per field per item
- Auto-generated `ItemNo` in format `ITEM-00001`
- Auto-assigns workflow start state on item creation
- Dynamic Edit form pre-filled from stored values
- Display view with read-only field values + workflow history
- Typed value storage: Text / Number / Date / Json per field type

---

## Database Tables Created

### Metadata Tables (16 tables)

| Table | Purpose |
|-------|---------|
| `AppLists` | List master |
| `AppListFields` | Field definitions |
| `AppForms` | Form layout (JSON) |
| `AppViews` | View config (JSON) |
| `AppPages` | Page widget layout (JSON) |
| `AppWorkflows` | Workflow state machine (JSON) |
| `AppWorkflowStates` | Workflow states |
| `AppWorkflowTransitions` | Workflow transitions |
| `AppRoles` | RBAC roles |
| `AppPermissions` | Permission registry |
| `AppRolePermissions` | Role ↔ Permission mapping |
| `AppUserRoles` | User ↔ Role mapping |

### Transaction Tables (4 tables)

| Table | Purpose |
|-------|---------|
| `AppItems` | Item master rows |
| `AppItemValues` | EAV field values per item |
| `AppWorkflowTransactions` | Workflow execution history |
| `AppAttachments` | File attachment metadata |

---

## Files Created (Complete List)

### Models — 8 files
```
SH_WebApplication1/Models/Metadata/AppList.vb
SH_WebApplication1/Models/Metadata/AppListField.vb
SH_WebApplication1/Models/Metadata/AppForm.vb
SH_WebApplication1/Models/Metadata/AppView.vb
SH_WebApplication1/Models/Metadata/AppPage.vb
SH_WebApplication1/Models/Metadata/AppWorkflow.vb
SH_WebApplication1/Models/Metadata/AppSecurity.vb
SH_WebApplication1/Models/Metadata/AppTransaction.vb
```

### Data Layer — 2 files
```
SH_WebApplication1/Data/AppDbContext.vb
SH_WebApplication1/Data/AppDbInitializer.vb
```

### Services — 14 files (7 interfaces + 7 implementations)
```
SH_WebApplication1/Services/IListService.vb       + ListService.vb
SH_WebApplication1/Services/IFormService.vb       + FormService.vb
SH_WebApplication1/Services/IViewService.vb       + ViewService.vb
SH_WebApplication1/Services/IPageService.vb       + PageService.vb
SH_WebApplication1/Services/IWorkflowService.vb   + WorkflowService.vb
SH_WebApplication1/Services/ISecurityService.vb   + SecurityService.vb
SH_WebApplication1/Services/IItemService.vb       + ItemService.vb
```

### Controllers — 6 files
```
SH_WebApplication1/Controllers/ListBuilderController.vb
SH_WebApplication1/Controllers/FormDesignerController.vb
SH_WebApplication1/Controllers/ViewDesignerController.vb
SH_WebApplication1/Controllers/PageDesignerController.vb
SH_WebApplication1/Controllers/WorkflowDesignerController.vb
SH_WebApplication1/Controllers/SecurityController.vb
SH_WebApplication1/Controllers/ItemsController.vb
```

### Views — 29 files
```
Views/ListBuilder/       Index, Create, Edit, Fields, AddField, EditField  (6)
Views/FormDesigner/      Index, Designer, Preview                           (3)
Views/ViewDesigner/      Index, Designer, Render                            (3)
Views/PageDesigner/      Index, Designer, Home                              (3)
Views/WorkflowDesigner/  Index, Designer, Execute                           (3)
Views/Security/          Roles, CreateRole, RolePermissions,
						 Permissions, UserRoles                             (5)
Views/Items/             Create, Edit, Display                              (3)
Views/Home/              Index (updated as platform dashboard)              (1)
Views/Shared/            _Layout.vbhtml (updated with full nav)             (1)
```

### Agent Files — 8 files
```
.github/agents/module-a-list-builder.agent.md
.github/agents/module-b-form-designer.agent.md
.github/agents/module-c-view-designer.agent.md
.github/agents/module-d-page-designer.agent.md
.github/agents/module-e-workflow-engine.agent.md
.github/agents/module-f-security-rbac.agent.md
.github/agents/items-runtime.agent.md
.github/agents/data-layer.agent.md
```

---

## Key Design Decisions

| Decision | Rationale |
|----------|-----------|
| EAV model (AppItemValues) | One physical schema supports any list dynamically |
| JSON-stored layouts | Form, View, Page, Workflow designs are schema-free and versioned |
| Service layer with interfaces | Clean separation; easy to swap or mock |
| Manual DI via constructor | No IoC container dependency for .NET 4.8 simplicity |
| EF CreateDatabaseIfNotExists | Fast dev startup; switch to Migrations for production |
| Cascade delete disabled on WF transitions | Prevents SQL constraint cycle errors |
| Soft-delete on AppLists | Preserves historical item data |
| Bootstrap Icons CDN | No extra NuGet; loaded from CDN in `_Layout.vbhtml` |

---

## URL Map — Quick Reference

| URL | Module | Action |
|-----|--------|--------|
| `/` | Home | Platform dashboard |
| `/ListBuilder` | A | All lists |
| `/ListBuilder/Create` | A | Create list |
| `/ListBuilder/Fields/{id}` | A | Manage fields |
| `/FormDesigner/Index?listId={id}` | B | List forms |
| `/FormDesigner/Designer?listId={id}&formType=New` | B | Design form |
| `/ViewDesigner/Index?listId={id}` | C | List views |
| `/ViewDesigner/Render?listId={id}` | C | Rendered list grid |
| `/PageDesigner/Home?listId={id}` | D | List home page |
| `/PageDesigner/Designer?listId={id}` | D | Widget designer |
| `/WorkflowDesigner/Index?listId={id}` | E | List workflows |
| `/WorkflowDesigner/Designer?listId={id}` | E | State machine designer |
| `/WorkflowDesigner/Execute?itemId={id}` | E | Execute workflow on item |
| `/Security/Roles` | F | Role management |
| `/Security/UserRoles` | F | User-role assignments |
| `/Items/Create?listId={id}` | Runtime | New item form |
| `/Items/Display/{id}` | Runtime | Item detail view |

---

## Next Steps / Recommended Enhancements

1. **Enable EF Migrations** — run `Enable-Migrations` then `Add-Migration Initial` to track schema changes
2. **Dropdown / Lookup field sources** — populate `DropdownConfig` / `LookupConfig` JSON in AddField UI
3. **Conditional field rules** — extend form designer to evaluate show/hide/required expressions at runtime
4. **Excel export** — add export to `/ViewDesigner/Render` using `ClosedXML` NuGet package
5. **File attachment upload** — implement `AppAttachments` table with file upload in `ItemsController`
6. **Email notifications** — add `SmtpClient` notification handler called on workflow transition
7. **Field-level security** — extend `SecurityService` to check field visibility per user role
8. **Kanban / Calendar view rendering** — implement the non-grid view types in `ViewDesigner/Render`
9. **Chart widgets** — integrate Chart.js for KPI and chart widgets in `PageDesigner/Home`
10. **IoC / Dependency Injection** — introduce Autofac or Simple Injector for cleaner DI

---

*Generated by GitHub Copilot — SharePoint-Like MVC Platform build session*
