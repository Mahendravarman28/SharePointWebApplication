---
name: module-b-form-designer
description: >
  Agent for Module B — Dynamic Form Designer.
  Use this agent when working on form layouts, drag-and-drop form design,
  conditional rules, form publishing, or runtime dynamic form rendering.
---

# Module B — Dynamic Form Designer Agent

## Responsibility

This agent owns everything related to **designing and rendering forms dynamically** for any list in the platform.

## Scope

- `Controllers/FormDesignerController.vb`
- `Views/FormDesigner/` (Index, Designer, Preview)
- `Models/Metadata/AppForm.vb`
- `Services/IFormService.vb` + `Services/FormService.vb`

## Key Tasks

1. List all forms for a given list (New / Edit / Display types)
2. Open the drag-and-drop form designer canvas
3. Add/remove sections (1-col, 2-col, 3-col layout)
4. Drag fields from palette into sections
5. Configure per-field properties:
   - Label override, help text
   - Required, ReadOnly, Hidden flags
6. Save form layout as JSON (`LayoutJson` column in `AppForms`)
7. Publish a form (sets `IsPublished = True`)
8. Preview a form rendered from its JSON definition at runtime

## JSON Layout Structure

```json
{
  "sections": [
	{
	  "sectionId": "section1",
	  "title": "General Information",
	  "columns": 2,
	  "fields": [
		{
		  "fieldId": 1,
		  "name": "RequestTitle",
		  "label": "Request Title",
		  "dataType": "SingleLine",
		  "required": true,
		  "readOnly": false,
		  "hidden": false,
		  "helpText": "Enter the title of the request"
		}
	  ]
	}
  ],
  "rules": []
}
```

## Runtime Rendering

At runtime (`ItemsController`), the published form JSON is loaded and fields are rendered as HTML controls matching their `DataType`.

| DataType | HTML Control |
|---|---|
| SingleLine | `<input type="text">` |
| MultiLine | `<textarea>` |
| Number / Decimal / Currency | `<input type="number">` |
| DateTime | `<input type="datetime-local">` |
| YesNo | `<input type="checkbox">` |
| Dropdown / Status | `<select>` |

## Database Tables

| Table | Purpose |
|---|---|
| `AppForms` | Form master (FormId, ListId, FormType, LayoutJson, IsPublished, VersionNo) |

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`
- jQuery drag-and-drop (HTML5 native dragstart/drop)
- `Newtonsoft.Json` for JSON serialization
- Ajax `$.post` for Save and Publish actions

## Navigation Entry Point

`/FormDesigner/Index?listId={id}` — list forms for a list.
`/FormDesigner/Designer?listId={id}&formType=New` — open the designer.
