---
name: module-e-workflow-engine
description: >
  Agent for Module E — Workflow Designer and Engine.
  Use this agent when working on workflow design, state machine configuration,
  transition rules, workflow execution, approval actions, or workflow history.
---

# Module E — Workflow Engine Agent

## Responsibility

This agent owns everything related to **designing and executing metadata-driven state machine workflows** for lists and items.

## Scope

- `Controllers/WorkflowDesignerController.vb`
- `Views/WorkflowDesigner/` (Index, Designer, Execute)
- `Models/Metadata/AppWorkflow.vb` (AppWorkflow, AppWorkflowState, AppWorkflowTransition)
- `Models/Metadata/AppTransaction.vb` (AppWorkflowTransaction)
- `Services/IWorkflowService.vb` + `Services/WorkflowService.vb`

## Key Tasks

### Design Phase
1. Create a new workflow for a list
2. Add states (name, code, color, IsStart, IsEnd flags)
3. Add transitions (From → Action → To, role required, condition expression)
4. Remove states or transitions
5. Save the entire workflow as a JSON state machine (`WorkflowJson` in `AppWorkflows`)
6. Sync states to `AppWorkflowStates` table on save
7. Publish the workflow (only one active workflow per list)

### Runtime Execution Phase
1. On item creation, assign the start state automatically
2. Load available transitions for current item state + current user role
3. Execute a transition:
   - Move item to new state
   - Record action in `AppWorkflowTransactions` (history)
   - Apply field updates if configured
4. Display full workflow history per item

## Workflow JSON Structure

```json
{
  "states": [
	{ "name": "Draft",    "code": "DRAFT",    "color": "secondary", "isStart": true,  "isEnd": false },
	{ "name": "Submitted","code": "SUBMITTED","color": "warning",   "isStart": false, "isEnd": false },
	{ "name": "Approved", "code": "APPROVED", "color": "success",   "isStart": false, "isEnd": true  },
	{ "name": "Rejected", "code": "REJECTED", "color": "danger",    "isStart": false, "isEnd": true  }
  ],
  "transitions": [
	{ "fromCode": "DRAFT",     "toCode": "SUBMITTED", "actionName": "Submit",  "role": "Contributor", "condition": "" },
	{ "fromCode": "SUBMITTED", "toCode": "APPROVED",  "actionName": "Approve", "role": "Approver",    "condition": "" },
	{ "fromCode": "SUBMITTED", "toCode": "REJECTED",  "actionName": "Reject",  "role": "Approver",    "condition": "" }
  ]
}
```

## Database Tables

| Table | Purpose |
|---|---|
| `AppWorkflows` | Workflow master (WorkflowId, ListId, WorkflowName, WorkflowJson, IsPublished) |
| `AppWorkflowStates` | States synced from JSON (StateId, WorkflowId, StateName, StateCode, IsStart, IsEnd) |
| `AppWorkflowTransitions` | Transitions with FK to states (TransitionId, FromStateId, ToStateId, ActionName, RoleRequired) |
| `AppWorkflowTransactions` | Execution history per item (TransactionId, ItemId, FromState, ToState, ActionBy, ActionDate, Comments) |

## Coding Standards

- VB.NET, ASP.NET MVC 5, .NET Framework 4.8
- EF6 via `AppDbContext`; cascade-delete disabled on transitions (configured in `OnModelCreating`)
- jQuery + HTML5 drag interaction on the state node canvas
- Ajax `$.post` for Save and Publish
- `WorkflowService.ExecuteTransition` handles runtime state change + history recording

## Navigation Entry Point

`/WorkflowDesigner/Index?listId={id}` — list all workflows.
`/WorkflowDesigner/Designer?listId={id}` — state machine designer.
`/WorkflowDesigner/Execute?itemId={id}` — runtime execution panel for an item.
