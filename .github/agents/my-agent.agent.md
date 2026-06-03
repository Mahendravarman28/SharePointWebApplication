---
name: my-agent
description: Describe what this custom agent does and when to use it.
---

# my-agent

Below is a **practical development plan** for building a **SharePoint List–like MVC application** using **ASP.NET MVC 5 + VB.NET + .NET Framework 4.8**, where users can:

* **Create lists dynamically**
* **Define columns/fields from client side**
* **Design New / Edit / Display forms**
* **Configure list views**
* **Build a home page for each list**
* **Create workflows dynamically**
* **Manage permissions and roles**

***

# 1) Solution Vision

You are not building a normal CRUD application.  
You are building a **metadata-driven platform** (similar to SharePoint Lists / Power Apps style behavior) where:

* The **structure of lists** is stored in **database metadata**
* The **forms** are generated dynamically from metadata
* The **views/pages** are also generated from configuration
* The **workflow engine** is rule-based and configurable by users
* The application supports **reusability**, **dynamic rendering**, and **extensibility**

This should be developed as a **platform-first architecture**, not page-by-page hardcoded development.

***

# 2) Recommended Architecture

## Core Architecture Style

Use a **metadata-driven, modular MVC architecture**.

### Recommended Layers

1. **Presentation Layer**
   * ASP.NET MVC 5
   * Razor Views
   * jQuery / JavaScript
   * Bootstrap / Admin theme
   * Drag-drop designer UI for forms/views/workflows

2. **Business Layer**
   * List Engine
   * Dynamic Form Engine
   * Workflow Engine
   * View Builder Engine
   * Security / RBAC Engine
   * Notification Engine

3. **Data Access Layer**
   * Entity Framework 6 or ADO.NET
   * SQL Server
   * Stored procedures where performance or dynamic querying is required

4. **Metadata Layer**
   * Stores list definition
   * field definition
   * form schema
   * workflow schema
   * page/view layout
   * permissions
   * validation rules

***

# 3) Major Functional Modules

***

## Module A — Dynamic List Builder

### Purpose

Allow admin/key users to create a new list from UI.

### Features

* Create list
* Set list name, code, description
* Enable attachments yes/no
* Enable versioning yes/no
* Enable approval workflow yes/no
* Define columns dynamically:
  * Single line text
  * Multi-line text
  * Number
  * Decimal
  * Currency
  * Date/Time
  * Yes/No
  * Dropdown
  * Multi-select
  * User picker
  * Lookup
  * File attachment
  * Calculated field
  * Auto number
  * Status field

### Field configuration

Each field should support:

* Internal name
* Display name
* Data type
* Required
* Unique
* Default value
* Validation expression
* Min / Max length
* Regex validation
* Dropdown source
* Lookup source
* Is searchable
* Is sortable
* Is filterable
* Is shown in list view
* Is shown in forms
* Order index

### Technical direction

Store all of the above in metadata tables rather than creating hardcoded columns in physical tables.

***

## Module B — Dynamic Form Designer

### Purpose

Allow users to design:

* **New Form**
* **Edit Form**
* **Display Form**

### Features

* Drag-and-drop layout builder
* 1-column / 2-column / section layout
* Tabs / accordion sections
* Place fields into sections
* Field visibility rules
* Conditional field behavior
* Read-only fields
* Hidden fields
* Dynamic labels/help text
* Button configuration
* Form actions

### Designer capabilities

* Select fields from list metadata
* Drag fields into canvas
* Set order and grouping
* Configure rules:
  * show/hide field
  * required when
  * disable when
  * default value by expression
* Preview form before save

### Save format

Store form design as:

* JSON layout
* Section metadata
* control settings
* business rules

### Runtime behavior

At runtime, MVC should render the form dynamically from metadata + JSON definition.

***

## Module C — Dynamic List Views / View Designer

### Purpose

Enable users to design how list data is shown.

### Types of views

* Grid view
* Card view
* Kanban-style status view
* Grouped view
* Calendar view
* Summary/dashboard view

### Features

* Select visible columns
* Define default sort
* Define filters
* Group by status/category/owner
* Conditional formatting
* Quick search
* Saved filters
* Personal view / public view
* Export to Excel
* Print view
* Paging

### Special requirement

Users should be able to mark one view as:

* **Default list landing page**
* **Home page view for the list**

***

## Module D — Home Page Designer for Each List

### Purpose

Each list should have its own “home page” similar to a SharePoint list landing page.

### Page should support

* Title
* summary tiles
* quick links
* recent items
* pending tasks
* charts / KPIs
* saved views
* create new button
* custom HTML/text widgets
* workflow pending approvals section

### Widgets to support

* Data grid widget
* KPI count widget
* Chart widget
* My pending items widget
* Recently modified widget
* Announcement widget
* Action buttons widget

### Design approach

Store widget layout as JSON:

* row
* column width
* widget type
* widget config
* permissions

***

## Module E — Workflow Designer / Engine

### Purpose

Allow users to create workflows from client side for each list.

### Workflow capabilities

* Draft → Submitted → Reviewed → Approved → Closed
* Rejected / Returned
* Parallel approval
* Sequential approval
* Auto-assign based on role/department
* Email notification
* Escalation
* SLA reminder
* Field update on action
* Approval comments
* Workflow history

### Workflow elements

* State
* Transition
* Condition
* Action
* Notification
* Role mapping
* Rule expression

### Examples

* If Amount > 10,000 → send to Manager Approval
* If Category = CAPEX → include Finance Approval
* If Rejected → return to Initiator and mark editable
* On Approved → lock record and send notification

### Workflow design UI

Use drag-drop/state-machine style builder:

* Nodes = states
* Arrows = transitions
* Transition rules/config
* Action types:
  * approve
  * reject
  * return
  * assign
  * close
  * cancel

### Best design

Build this as a **metadata-driven state machine engine**, not hardcoded business logic.

***

## Module F — Security / Role-Based Access Control

### Purpose

Different users/roles should have different access.

### Access control levels

1. Application level
2. Module level
3. List level
4. Field level
5. Form level
6. View level
7. Action level
8. Record level (optional advanced)

### Roles

Example:

* Admin
* List Designer
* Workflow Designer
* Contributor
* Reviewer
* Approver
* Viewer

### Permissions

* Create list
* Edit list schema
* Publish forms
* Design workflow
* Create item
* Edit own item
* Approve item
* View all items
* Delete item
* Export item

### Advanced requirement

Field-level security:

* Salary field visible only to HR
* Approval comment visible only to approvers

***

# 4) Recommended Database Design

Since this is dynamic, avoid creating one physical SQL table per list unless your business absolutely requires it.

A **metadata + transactional hybrid model** is the safest.

***

## A. Metadata Tables

### 1. AppLists

Stores list definition

* ListId
* ListName
* ListCode
* Description
* IsActive
* VersionNo
* CreatedBy
* CreatedDate

### 2. AppListFields

Stores field definitions

* FieldId
* ListId
* InternalName
* DisplayName
* DataType
* IsRequired
* IsUnique
* DefaultValue
* ValidationRule
* LookupConfig
* DisplayOrder
* IsVisible
* IsSearchable
* IsSortable
* IsFilterable

### 3. AppForms

Stores form master

* FormId
* ListId
* FormType (New/Edit/Display)
* FormName
* LayoutJson
* IsPublished
* VersionNo

### 4. AppViews

Stores list views

* ViewId
* ListId
* ViewName
* ViewType
* ViewConfigJson
* IsDefault
* IsPublic

### 5. AppPages

Stores list home page design

* PageId
* ListId
* PageName
* LayoutJson
* IsDefaultHome

### 6. AppWorkflows

Stores workflow master

* WorkflowId
* ListId
* WorkflowName
* WorkflowJson
* IsPublished

### 7. AppWorkflowStates

* StateId
* WorkflowId
* StateName
* StateCode
* IsStart
* IsEnd

### 8. AppWorkflowTransitions

* TransitionId
* WorkflowId
* FromStateId
* ToStateId
* ActionName
* ConditionExpression
* RoleRequired

### 9. AppRoles / AppPermissions / AppRolePermissions

RBAC tables

***

## B. Transaction Tables

### 1. AppItems

Master item row

* ItemId
* ListId
* ItemNo
* CurrentStatus
* CurrentStateId
* CreatedBy
* CreatedDate
* ModifiedBy
* ModifiedDate

### 2. AppItemValues

Stores field values dynamically

* ItemValueId
* ItemId
* FieldId
* FieldValueText
* FieldValueNumber
* FieldValueDate
* FieldValueJson

### 3. AppWorkflowTransactions

Workflow history

* TransactionId
* ItemId
* WorkflowId
* FromState
* ToState
* ActionBy
* ActionDate
* Comments

### 4. AppAttachments

Stores attachments metadata

* AttachmentId
* ItemId
* FileName
* FilePath
* FileSize

***

# 5) Rendering Strategy

This is the most important part.

## Runtime rendering must be metadata-driven

For each screen:

### List Home Page

* Read page JSON from `AppPages`
* Build widgets dynamically

### List View

* Read selected view from `AppViews`
* Build filters/grid dynamically
* Query `AppItems + AppItemValues`

### New Form

* Read form schema from `AppForms`
* Render controls based on `AppListFields`
* Apply rules and validation

### Edit Form

* Load field values from `AppItemValues`
* Apply field-level permissions
* Check workflow state rules

### Display Form

* Render read-only controls or display template
* Show history + workflow + attachments

***

# 6) Workflow Engine Design Approach

Use a **state machine engine**.

## Workflow processing model

When a user clicks action:

1. Load item current state
2. Load eligible transitions
3. Validate role/permission
4. Evaluate conditions
5. Move to next state
6. Update item state/status
7. Log transaction history
8. Trigger notifications
9. Trigger optional field updates / automation

### Good practice

Build workflow conditions using a simple expression engine, for example:

* `Amount > 10000`
* `Department = 'QA'`
* `Priority = 'High' And Category = 'CAPA'`

You can store these conditions in DB and evaluate them via a custom expression parser.

***

# 7) UI/UX Plan

Since users need to design lists, forms, views, and workflows from client side, create these UI modules:

## Admin Studio / Designer Portal

A separate area in the application:

* List Designer
* Field Designer
* Form Designer
* View Designer
* Page Designer
* Workflow Designer
* Security Designer

### Suggested navigation

* **Applications**
  * Lists
  * Pages
  * Workflows
  * Security
  * Settings

### Recommended front-end behavior

You can stay with MVC + jQuery for runtime pages, but for the designers:

* jQuery UI drag/drop is acceptable in .NET 4.8 ecosystem
* or integrate lightweight Vue/React only for designer screens if needed

If you want lowest complexity and full compatibility with MVC 4.8:

* Use **jQuery + Bootstrap + SortableJS / jQuery UI**

***

# 8) Recommended Development Phases

***

## Phase 1 — Foundation / Core Platform

### Goal

Build the base platform skeleton.

### Deliverables

* ASP.NET MVC 5 solution in VB.NET
* Common layout/master page
* Authentication and RBAC
* Metadata database schema
* Logging and exception handling
* Audit trail framework
* File upload framework
* Common helper services

### Duration

2–3 weeks

***

## Phase 2 — Dynamic List Engine

### Goal

Build list schema management.

### Deliverables

* Create/edit/delete list
* Field designer
* Field metadata storage
* Dynamic server-side validation rules
* List schema preview

### Duration

2–3 weeks

***

## Phase 3 — Form Designer + Dynamic Form Runtime

### Goal

Allow user-designed forms.

### Deliverables

* New/Edit/Display form builder
* Drag-drop sections and controls
* Save form JSON
* Render forms dynamically
* Client + server validation
* Conditional visibility

### Duration

4–6 weeks

***

## Phase 4 — Dynamic Data Entry & Item Management

### Goal

Support full CRUD using dynamic forms.

### Deliverables

* Create item
* Edit item
* View item
* Delete item
* Attachments
* Version history
* Audit history

### Duration

3–4 weeks

***

## Phase 5 — View Designer + List Home Page

### Goal

Build views and home page layout.

### Deliverables

* Grid/card/dashboard views
* Saved view config
* Search/filter/sort/group
* Home page widgets
* Default landing view per list

### Duration

3–4 weeks

***

## Phase 6 — Workflow Designer + Engine

### Goal

Build configurable workflow.

### Deliverables

* Workflow designer UI
* States/transitions/actions
* Role-based approval
* Approval comments/history
* Notifications
* Escalation/reminders

### Duration

5–7 weeks

***

## Phase 7 — Security, Performance & Publishing

### Goal

Production readiness.

### Deliverables

* Field-level security
* Record-level filtering
* Caching
* indexing optimization
* export/print
* deployment scripts
* backup/versioning of metadata

### Duration

3–4 weeks

***

# 9) Minimum Viable Product (MVP)

If you want to deliver fast, first release should include:

## MVP Scope

* Create dynamic list
* Define columns
* Design simple New/Edit/Display forms
* Create item / edit item / view item
* Simple list grid view
* Basic workflow: Draft → Submit → Approve / Reject
* Email notification
* Role-based access

### Avoid in MVP

* Kanban view
* Advanced dashboard widgets
* expression parser complexity
* record-level security
* calendar view
* parallel approvals
* advanced formula/calculated fields

This gives a maintainable release path.

***

# 10) Recommended Project Structure in MVC

Example solution structure:

```text
/Areas
   /AdminStudio
      /Controllers
      /Views
   /Runtime
      /Controllers
      /Views

/Controllers
/Models
   /Metadata
   /Runtime
   /ViewModels
/Services
   /ListEngine
   /FormEngine
   /WorkflowEngine
   /PermissionEngine
   /NotificationEngine
/Repositories
/Helpers
/Scripts
/Content
```

***

# 11) Key Technical Decisions You Must Make Early

***

## Decision 1 — Data storage model

Choose one:

### Option A — Fully metadata/EAV model

Best for flexibility  
Harder for reporting/performance

### Option B — Hybrid model (**recommended**)

* Metadata tables define lists/forms/views/workflows
* Transaction data stored in generic item/value tables
* Special indexed fields supported for common filtering

This is the best balance.

***

## Decision 2 — Form layout storage

Recommended:

* store layout in **JSON**
* render dynamically in MVC

***

## Decision 3 — Workflow model

Recommended:

* state machine-based
* transitions stored in DB
* actions executed by engine service

***

## Decision 4 — Client-side designer technology

Recommended for .NET 4.8:

* Bootstrap
* jQuery
* jQuery UI / SortableJS
* Select2
* jsTree (optional for navigation)

***

# 12) Risks and Mitigation

## Risk 1 — Over-complexity

This kind of system becomes huge very fast.

### Mitigation

Start with MVP and fixed scope.

***

## Risk 2 — Performance issues on dynamic querying

Dynamic list values stored as metadata can be slow.

### Mitigation

* use SQL indexing
* cache metadata
* use optimized stored procedures for grid loading
* denormalized summary columns if required

***

## Risk 3 — Security gaps

Dynamic forms and workflows may expose unauthorized fields/actions.

### Mitigation

Enforce permission checks in:

* UI rendering
* controller action
* service layer
* workflow transition logic

***

## Risk 4 — Difficult maintenance

If code is not modular, this becomes unmanageable.

### Mitigation

Keep engines separated:

* Form engine
* List engine
* Workflow engine
* Permission engine

***

# 13) Suggested Team Roles

For a serious build:

* **1 Solution Architect**
* **1–2 MVC/VB.NET Developers**
* **1 Front-end Developer**
* **1 SQL Developer**
* **1 QA Tester**
* **1 Business Process Owner**

If only one developer is doing it, definitely implement in **phases**, not all at once.

***

# 14) Recommended Development Sequence

Here is the best sequence to reduce rework:

1. **Authentication + RBAC**
2. **Metadata schema**
3. **List designer**
4. **Dynamic item storage**
5. **Form renderer**
6. **Item CRUD**
7. **Views/grid engine**
8. **Page/home designer**
9. **Workflow engine**
10. **Notifications + reporting**
11. **Optimization + deployment**

This order prevents you from redesigning core tables later.

***

# 15) Example User Journey

## Business user should be able to:

### Step 1

Create List: **Engineering Change Request**

### Step 2

Add fields:

* Request No
* Title
* Description
* Department
* Priority
* Requested By
* Target Date
* Attachment

### Step 3

Design New Form:

* General Information section
* Approval section
* Attachment section

### Step 4

Design Default View:

* columns: Request No, Title, Department, Priority, Status, Requested By

### Step 5

Design Home Page:

* tile: Pending Requests
* tile: My Open Requests
* grid: Recently Updated
* chart: Requests by Status

### Step 6

Define Workflow:
Draft → Submitted → Engineering Review → QA Review → Approved → Closed

That is the exact kind of flow your platform should support.

***

# 16) Best-Practice Recommendation

For your use case, I strongly recommend this implementation style:

## Recommended Final Design

* **ASP.NET MVC 5**
* **VB.NET (.NET Framework 4.8)**
* **SQL Server**
* **Entity Framework 6 for metadata**
* **Stored procedures for dynamic grid/query performance**
* **JSON-based form/page/workflow definitions**
* **jQuery/Bootstrap-based designer**
* **State-machine workflow engine**
* **Role-based permission engine**
* **Audit + versioning from day one**

***

# 17) Delivery Plan (Realistic Timeline)

If done properly:

## MVP

**10–14 weeks**

## Full low-code style platform

**5–7 months**

depending on:

* number of designers
* complexity of workflows
* dashboard requirements
* security complexity
* reporting/export requirements

***

# 18) Final Recommendation

If your goal is to build a **SharePoint List-like internal platform**, do **not** start by designing screens.

Start with these **4 engines first**:

1. **Metadata Engine**
2. **Dynamic Form Engine**
3. **Workflow Engine**
4. **Permission Engine**

Once these engines are stable, the rest (list pages, views, home page widgets, forms) becomes much easier.

***

If you want, I can prepare the **next step** in one of these formats:

### Option 1

**High-level solution architecture diagram**

### Option 2

**Detailed SQL table design for this platform**

### Option 3

**Full module-wise development roadmap with sprint breakdown**

### Option 4

**VB.NET MVC project folder structure + controller/service design**

### Option 5

**UI wireframe plan for List Designer, Form Designer, Workflow Designer**

If you want, I can do the **complete enterprise design document next** with:

* architecture
* database schema
* services
* controllers
* workflow engine design
* sample JSON model for forms/views/workflows.
