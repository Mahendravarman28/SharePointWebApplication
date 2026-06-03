---
name: module-d-page-designer
description: >
  Agent for Module D — Home Page Designer.
  Use this agent when working on list home pages, widget-based layouts,
  KPI tiles, recent items panels, or the page JSON structure.
---

# Module D — Home Page Designer Agent

## Responsibility

This agent owns everything related to **designing and rendering the home page for each list**. Each list gets its own dedicated landing page powered by a JSON widget layout.

## Scope

- `Controllers/PageDesignerController.vb`
- `Views/PageDesigner/` (Index, Designer, Home)
- `Models/Metadata/AppPage.vb`
- `Services/IPageService.vb` + `Services/PageService.vb`

## Key Tasks

1. Open the page designer for a list
2. Add/remove rows on the canvas
3. Drag widgets from the palette onto rows
4. Configure widget column width (2–12 Bootstrap cols)
5. Remove individual widgets
6. Set a page as the default home page for a list
7. Save layout as JSON (`LayoutJson` in `AppPages`)
8. Render the live home page with real data

## Supported Widgets

| Widget Type | Description |
|---|---|
| `KpiCount` | Shows a count metric with icon and color |
| `DataGrid` | Inline data grid from list items |
| `Chart` | Placeholder for chart rendering |
| `RecentItems` | Shows last 5 modified items |
| `MyPendingItems` | Items pending action by current user |
| `Announcement` | Rich-text announcement block |
| `ActionButtons` | Quick-action button links |
| `QuickLinks` | Configurable shortcut links |

## JSON Page Layout Structure

```json
{
  "rows": [
	{
	  "rowId": "row1",
	  "widgets": [
		{ "widgetId": "w1", "type": "KpiCount", "colWidth": 3, "title": "Total Items", "config": { "metric": "total" } },
		{ "widgetId": "w2", "type": "KpiCount", "colWidth": 3, "title": "Draft",       "config": { "metric": "draft" } }
	  ]
	},
	{
	  "rowId": "row2",
	  "widgets": [
		{ "widgetId": "w3", "type": "RecentItems",   "colWidth": 8, "title": "Recent Items",   "config": {} },
		{ "widgetId": "w4", "type": "ActionButtons",  "colWidth": 4, "title": "Quick Actions",  "config": {} }
	  ]
	}
  ]
}
```

## Database Tables

| Table | Purpose |
|---|---|
| `AppPages` | Page layout (PageId, ListId, PageName, LayoutJson, IsDefaultHome) |

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`
- jQuery + HTML5 drag-and-drop for widget placement
- Bootstrap 5 card/grid rendering on the live Home view
- Ajax `$.post` for Save; form post for SetDefault

## Navigation Entry Point

`/PageDesigner/Home?listId={id}` — rendered list home page.
`/PageDesigner/Designer?listId={id}` — open the widget canvas designer.
`/PageDesigner/Index?listId={id}` — manage all pages for a list.
