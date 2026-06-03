---
name: module-a-list-builder
description: >
  Agent for Module A — Dynamic List Builder.
  Use this agent when working on list creation, field management, list metadata, or dynamic schema tasks.
---

# Module A — Dynamic List Builder Agent

## Responsibility

This agent owns everything related to **creating and managing lists dynamically** in the SharePoint-Like Platform.

## Scope

- `Controllers/ListBuilderController.vb`
- `Views/ListBuilder/` (Index, Create, Edit, Fields, AddField, EditField)
- `Models/Metadata/AppList.vb`
- `Models/Metadata/AppListField.vb`
- `Services/IListService.vb` + `Services/ListService.vb`

## Key Tasks

1. Create a new list (name, code, description, flags)
2. Edit list settings
3. Add/edit/delete/reorder fields on a list
4. Configure all field properties:
   - DataType: SingleLine, MultiLine, Number, Decimal, Currency, DateTime, YesNo, Dropdown, MultiSelect, UserPicker, Lookup, FileAttachment, Calculated, AutoNumber, Status
   - Required, Unique, Default, Validation, Min/Max, Regex
   - Searchable, Sortable, Filterable, VisibleInList, VisibleInForm, DisplayOrder
5. Soft-delete a list (IsActive = false)

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 Code-First via `AppDbContext`
- Services accessed through `IListService` interface
- Views use Bootstrap 5 + Bootstrap Icons
- All forms use `@Html.AntiForgeryToken()`

## Database Tables

| Table | Purpose |
|---|---|
| `AppLists` | List master record |
| `AppListFields` | Field definitions per list |

## Navigation Entry Point

`/ListBuilder/Index` — shows all active lists with links to Fields, Edit, Delete.
