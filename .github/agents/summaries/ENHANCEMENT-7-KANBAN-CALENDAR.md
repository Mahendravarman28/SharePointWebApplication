# Enhancement 7 - Kanban & Calendar Views

## Implementation Status

**Kanban and Calendar view rendering added with placeholder UI** due to VB Razor inline rendering limitations.

## Current State

`Views\ViewDesigner\Render.vbhtml` now shows an alert message when ViewType is "Kanban" or "Calendar" and falls back to Grid view.

## VB Razor Limitation

VB Razor parser is extremely sensitive to inline control-flow (If/ElseIf/For Each) combined with HTML prefix syntax (`@:`).  
The initial full implementation caused 50+ syntax errors due to parser state confusion.

## Recommended Next Step

Implement Kanban and Calendar rendering in **separate partial views**:
- `_KanbanPartial.vbhtml` - Status-grouped board columns
- `_CalendarPartial.vbhtml` - Date-based calendar grid with FullCalendar.js

This approach will isolate complex rendering logic and avoid VB Razor inline parser issues.

## Workaround Used

Reverted to Grid-only rendering with a TODO note. The placeholder alert informs users that Kanban/Calendar views are planned.
