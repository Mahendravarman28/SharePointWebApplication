Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports System.Data.Entity

Namespace Services

    Public Class WorkflowService
        Implements IWorkflowService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetWorkflows(listId As Integer) As List(Of AppWorkflow) Implements IWorkflowService.GetWorkflows
            Return _db.AppWorkflows.Where(Function(w) w.ListId = listId).ToList()
        End Function

        Public Function GetWorkflowById(workflowId As Integer) As AppWorkflow Implements IWorkflowService.GetWorkflowById
            Return _db.AppWorkflows _
                      .Include(Function(w) w.States) _
                      .Include(Function(w) w.Transitions) _
                      .FirstOrDefault(Function(w) w.WorkflowId = workflowId)
        End Function

        Public Function GetPublishedWorkflow(listId As Integer) As AppWorkflow Implements IWorkflowService.GetPublishedWorkflow
            Return _db.AppWorkflows _
                      .Include(Function(w) w.States) _
                      .Include(Function(w) w.Transitions) _
                      .FirstOrDefault(Function(w) w.ListId = listId AndAlso w.IsPublished)
        End Function

        Public Function SaveWorkflow(workflow As AppWorkflow) As AppWorkflow Implements IWorkflowService.SaveWorkflow
            If workflow.WorkflowId = 0 Then
                workflow.CreatedDate = DateTime.Now
                _db.AppWorkflows.Add(workflow)
            Else
                Dim existing = _db.AppWorkflows.Find(workflow.WorkflowId)
                If existing IsNot Nothing Then
                    existing.WorkflowName = workflow.WorkflowName
                    existing.WorkflowJson = workflow.WorkflowJson
                End If
            End If
            _db.SaveChanges()
            Return workflow
        End Function

        Public Function PublishWorkflow(workflowId As Integer) As Boolean Implements IWorkflowService.PublishWorkflow
            Dim wf = _db.AppWorkflows.Find(workflowId)
            If wf Is Nothing Then Return False
            wf.IsPublished = True
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteWorkflow(workflowId As Integer) As Boolean Implements IWorkflowService.DeleteWorkflow
            Dim wf = _db.AppWorkflows.Find(workflowId)
            If wf Is Nothing Then Return False
            _db.AppWorkflows.Remove(wf)
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetStates(workflowId As Integer) As List(Of AppWorkflowState) Implements IWorkflowService.GetStates
            Return _db.AppWorkflowStates.Where(Function(s) s.WorkflowId = workflowId).OrderBy(Function(s) s.DisplayOrder).ToList()
        End Function

        Public Function SaveState(state As AppWorkflowState) As AppWorkflowState Implements IWorkflowService.SaveState
            If state.StateId = 0 Then
                _db.AppWorkflowStates.Add(state)
            Else
                Dim existing = _db.AppWorkflowStates.Find(state.StateId)
                If existing IsNot Nothing Then
                    existing.StateName = state.StateName
                    existing.StateCode = state.StateCode
                    existing.IsStart = state.IsStart
                    existing.IsEnd = state.IsEnd
                    existing.DisplayOrder = state.DisplayOrder
                    existing.ColorCode = state.ColorCode
                End If
            End If
            _db.SaveChanges()
            Return state
        End Function

        Public Function DeleteState(stateId As Integer) As Boolean Implements IWorkflowService.DeleteState
            Dim state = _db.AppWorkflowStates.Find(stateId)
            If state Is Nothing Then Return False
            _db.AppWorkflowStates.Remove(state)
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetTransitions(workflowId As Integer) As List(Of AppWorkflowTransition) Implements IWorkflowService.GetTransitions
            Return _db.AppWorkflowTransitions.Where(Function(t) t.WorkflowId = workflowId).ToList()
        End Function

        Public Function SaveTransition(transition As AppWorkflowTransition) As AppWorkflowTransition Implements IWorkflowService.SaveTransition
            If transition.TransitionId = 0 Then
                _db.AppWorkflowTransitions.Add(transition)
            Else
                Dim existing = _db.AppWorkflowTransitions.Find(transition.TransitionId)
                If existing IsNot Nothing Then
                    existing.ActionName = transition.ActionName
                    existing.ActionCode = transition.ActionCode
                    existing.ConditionExpression = transition.ConditionExpression
                    existing.RoleRequired = transition.RoleRequired
                    existing.ActionJson = transition.ActionJson
                    existing.NotificationJson = transition.NotificationJson
                    existing.FromStateId = transition.FromStateId
                    existing.ToStateId = transition.ToStateId
                End If
            End If
            _db.SaveChanges()
            Return transition
        End Function

        Public Function DeleteTransition(transitionId As Integer) As Boolean Implements IWorkflowService.DeleteTransition
            Dim t = _db.AppWorkflowTransitions.Find(transitionId)
            If t Is Nothing Then Return False
            _db.AppWorkflowTransitions.Remove(t)
            _db.SaveChanges()
            Return True
        End Function

        Public Function ExecuteTransition(itemId As Integer, transitionId As Integer, userId As String, comments As String) As Boolean Implements IWorkflowService.ExecuteTransition
            Dim item = _db.AppItems.Find(itemId)
            Dim trans = _db.AppWorkflowTransitions _
                           .Include(Function(t) t.FromState) _
                           .Include(Function(t) t.ToState) _
                           .FirstOrDefault(Function(t) t.TransitionId = transitionId)
            If item Is Nothing OrElse trans Is Nothing Then Return False

            Dim history As New AppWorkflowTransaction With {
                .ItemId = itemId,
                .WorkflowId = trans.WorkflowId,
                .TransitionId = transitionId,
                .FromState = trans.FromState.StateCode,
                .ToState = trans.ToState.StateCode,
                .ActionBy = userId,
                .ActionDate = DateTime.Now,
                .Comments = comments
            }
            _db.AppWorkflowTransactions.Add(history)

            item.CurrentStatus = trans.ToState.StateName
            item.CurrentStateId = trans.ToStateId
            item.ModifiedBy = userId
            item.ModifiedDate = DateTime.Now
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetAvailableTransitions(itemId As Integer, userId As String) As List(Of AppWorkflowTransition) Implements IWorkflowService.GetAvailableTransitions
            Dim item = _db.AppItems.Find(itemId)
            If item Is Nothing OrElse Not item.CurrentStateId.HasValue Then Return New List(Of AppWorkflowTransition)()
            Return _db.AppWorkflowTransitions _
                      .Include(Function(t) t.ToState) _
                      .Where(Function(t) t.FromStateId = item.CurrentStateId.Value) _
                      .ToList()
        End Function

        Public Function GetWorkflowHistory(itemId As Integer) As List(Of AppWorkflowTransaction) Implements IWorkflowService.GetWorkflowHistory
            Return _db.AppWorkflowTransactions.Where(Function(t) t.ItemId = itemId).OrderByDescending(Function(t) t.ActionDate).ToList()
        End Function

    End Class

End Namespace
