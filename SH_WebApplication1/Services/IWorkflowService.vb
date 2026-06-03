Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IWorkflowService
        Function GetWorkflows(listId As Integer) As List(Of AppWorkflow)
        Function GetWorkflowById(workflowId As Integer) As AppWorkflow
        Function GetPublishedWorkflow(listId As Integer) As AppWorkflow
        Function SaveWorkflow(workflow As AppWorkflow) As AppWorkflow
        Function PublishWorkflow(workflowId As Integer) As Boolean
        Function DeleteWorkflow(workflowId As Integer) As Boolean
        ' State management
        Function GetStates(workflowId As Integer) As List(Of AppWorkflowState)
        Function SaveState(state As AppWorkflowState) As AppWorkflowState
        Function DeleteState(stateId As Integer) As Boolean
        ' Transition management
        Function GetTransitions(workflowId As Integer) As List(Of AppWorkflowTransition)
        Function SaveTransition(transition As AppWorkflowTransition) As AppWorkflowTransition
        Function DeleteTransition(transitionId As Integer) As Boolean
        ' Runtime execution
        Function InitiateWorkflow(itemId As Integer, listId As Integer, userId As String) As Boolean
        Function GetCurrentState(itemId As Integer) As AppWorkflowState
        Function ExecuteTransition(itemId As Integer, transitionId As Integer, userId As String, comments As String) As Boolean
        Function GetAvailableTransitions(itemId As Integer, userId As String) As List(Of AppWorkflowTransition)
        Function GetWorkflowHistory(itemId As Integer) As List(Of AppWorkflowTransaction)
    End Interface

End Namespace
