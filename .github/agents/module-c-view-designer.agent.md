---
name: module-c-view-designer
description: >
  Agent for Module C — Dynamic List View Designer.
  Use this agent when working on view configuration, column selection,
  sorting, filtering, grouping, or rendering list data in different view types.
---

# Module C — Dynamic List View Designer Agent

## Responsibility

This agent owns everything related to **configuring and rendering list views** for any list in the platform.

## Scope

- `Controllers/ViewDesignerController.vb`
- `Views/ViewDesigner/` (Index, Designer, Render)
- `Models/Metadata/AppView.vb`
- `Services/IViewService.vb` + `Services/ViewService.vb`

## Key Tasks

1. List all saved views for a given list
2. Create or edit a view configuration
3. Select visible columns from the list's fields
4. Drag-reorder columns
5. Set default sort (field + direction)
6. Set group-by field
7. Configure page size
8. Toggle public vs. personal view
9. Mark a view as the default view for the list
10. Render the view with live data from `AppItems` + `AppItemValues`
11. Quick search within the rendered grid

## Supported View Types

| Type | Description |
|---|---|
| Grid | Standard tabular data grid (default) |
| Card | Card-per-row layout |
| Kanban | Status-column board |
| Grouped | Rows grouped by a selected field |
| Calendar | Date-field based calendar display |
| Summary | Dashboard summary with counts |

## JSON View Config Structure

```json
{
  "columns": [
	{ "fieldId": 1, "label": "Title", "sortable": true, "width": "" },
	{ "fieldId": 2, "label": "Amount", "sortable": true, "width": "" }
  ],
  "defaultSort": { "fieldId": 1, "direction": "asc" },
  "filters": [],
  "groupBy": "",
  "pageSize": 20
}
```

## Database Tables

| Table | Purpose |
|---|---|
| `AppViews` | View config (ViewId, ListId, ViewName, ViewType, ViewConfigJson, IsDefault, IsPublic) |

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`
- jQuery for column reorder (HTML5 drag)
- Bootstrap 5 table rendering with quick-search filter
- Ajax `$.post` for Save and SetDefault

## Navigation Entry Point

`/ViewDesigner/Index?listId={id}` — list all views.
`/ViewDesigner/Designer?listId={id}` — open view designer.
`/ViewDesigner/Render?listId={id}` — render default view with data.
