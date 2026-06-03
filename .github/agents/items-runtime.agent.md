---
name: items-runtime
description: >
  Agent for the runtime Items engine — dynamic form-driven item create/edit/display.
  Use this agent when working on item creation from dynamic forms, field value storage,
  item display with workflow history, or the EAV (AppItemValues) data model.
---

# Items Runtime Agent

## Responsibility

This agent owns everything related to **creating, editing, and displaying list items dynamically** using the published form layout and stored field definitions.

## Scope

- `Controllers/ItemsController.vb`
- `Views/Items/` (Create, Edit, Display)
- `Models/Metadata/AppTransaction.vb` (AppItem, AppItemValue, AppAttachment)
- `Services/IItemService.vb` + `Services/ItemService.vb`

## Key Tasks

1. Render a dynamic create form (reads `AppListFields` for the list)
2. Save new item: creates `AppItem` row + one `AppItemValue` row per field
3. Assign start workflow state on item creation if a published workflow exists
4. Render dynamic edit form pre-filled with current values
5. Update item: modify existing `AppItemValue` rows
6. Display item in read-only view with field labels and values
7. Show workflow history on the display view

## EAV Storage Pattern

Items use an **Entity–Attribute–Value** pattern:

| Table | Role |
|---|---|
| `AppItems` | Master row (ItemId, ListId, ItemNo, CurrentStatus, CurrentStateId, CreatedBy, CreatedDate) |
| `AppItemValues` | Per-field value row (ItemValueId, ItemId, FieldId, FieldValueText, FieldValueNumber, FieldValueDate, FieldValueJson) |

Field values are typed on write:

| DataType | Column Used |
|---|---|
| SingleLine / MultiLine / Dropdown / Status | `FieldValueText` |
| Number / Decimal / Currency / AutoNumber | `FieldValueNumber` |
| DateTime | `FieldValueDate` |
| MultiSelect / Lookup | `FieldValueJson` |

## Dynamic Form Rendering (Runtime)

The Create/Edit views loop over `AppListFields` where `IsVisibleInForm = True` and render the appropriate HTML control per `DataType`. Field names use the pattern `field_{FieldId}` so the controller can extract them from `FormCollection`.

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`; EF includes used for Values and Attachments
- No hardcoded field names — all driven from `AppListFields` metadata
- `ItemNo` auto-generated: `ITEM-{count:D5}` format

## Navigation Entry Point

`/Items/Create?listId={id}` — create a new item.
`/Items/Edit/{id}` — edit an existing item.
`/Items/Display/{id}` — view an item with workflow history.
