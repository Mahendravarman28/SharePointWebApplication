Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services
Imports Newtonsoft.Json

Namespace Controllers

    <Authorize>
    Public Class WorkflowDesignerController
        Inherits Controller

        Private ReadOnly _wfService As IWorkflowService
        Private ReadOnly _listService As IListService
        Private ReadOnly _itemService As IItemService
        Private ReadOnly _securityService As ISecurityService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _wfService = New WorkflowService(_db)
            _listService = New ListService(_db)
            _itemService = New ItemService(_db)
            _securityService = New SecurityService(_db)
        End Sub

        ' GET: /WorkflowDesigner/Index/5
        Public Function Index(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            Return View(_wfService.GetWorkflows(listId))
        End Function

        ' GET: /WorkflowDesigner/Designer?listId=5&workflowId=0
        Public Function Designer(listId As Integer, Optional workflowId As Integer = 0) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim wf As AppWorkflow
            If workflowId > 0 Then
                wf = _wfService.GetWorkflowById(workflowId)
            Else
                wf = New AppWorkflow With {
                    .ListId = listId,
                    .WorkflowName = $"{list.ListName} Workflow",
                    .IsPublished = False
                }
            End If
            ViewBag.List = list
            ViewBag.Roles = _securityService.GetRoles()
            Return View(wf)
        End Function

        ' POST: /WorkflowDesigner/Save
        <HttpPost>
        Public Function Save(listId As Integer, workflowId As Integer, workflowName As String, workflowJson As String) As JsonResult
            Try
                Dim wf As New AppWorkflow With {
                    .WorkflowId = workflowId,
                    .ListId = listId,
                    .WorkflowName = workflowName,
                    .WorkflowJson = workflowJson,
                    .CreatedBy = User.Identity.Name
                }
                wf = _wfService.SaveWorkflow(wf)
                ' sync states & transitions from JSON
                SyncStatesAndTransitions(wf)
                Return Json(New With {.success = True, .workflowId = wf.WorkflowId})
            Catch ex As Exception
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

        ' POST: /WorkflowDesigner/Publish
        <HttpPost>
        Public Function Publish(workflowId As Integer) As JsonResult
            Return Json(New With {.success = _wfService.PublishWorkflow(workflowId)})
        End Function

        ' POST: /WorkflowDesigner/Delete
        <HttpPost>
        Public Function Delete(workflowId As Integer, listId As Integer) As ActionResult
            _wfService.DeleteWorkflow(workflowId)
            Return RedirectToAction("Index", New With {.listId = listId})
        End Function

        ' GET: /WorkflowDesigner/Execute?itemId=5
        Public Function Execute(itemId As Integer) As ActionResult
            Dim item = _itemService.GetItemById(itemId)
            If item Is Nothing Then Return HttpNotFound()
            Dim list = _listService.GetListById(item.ListId)
            Dim wf = _wfService.GetPublishedWorkflow(item.ListId)
            ViewBag.List = list
            ViewBag.Item = item
            ViewBag.Workflow = wf
            ViewBag.AllStates = If(wf IsNot Nothing, _wfService.GetStates(wf.WorkflowId), New List(Of AppWorkflowState)())
            ViewBag.CurrentState = _wfService.GetCurrentState(itemId)
            ViewBag.Transitions = _wfService.GetAvailableTransitions(itemId, User.Identity.Name)
            ViewBag.History = _wfService.GetWorkflowHistory(itemId)
            Return View()
        End Function

        ' POST: /WorkflowDesigner/ExecuteTransition
        <HttpPost, ValidateAntiForgeryToken>
        Public Function ExecuteTransition(itemId As Integer, transitionId As Integer, comments As String) As ActionResult
            _wfService.ExecuteTransition(itemId, transitionId, User.Identity.Name, comments)
            Return RedirectToAction("Execute", New With {.itemId = itemId})
        End Function

        ' GET: /WorkflowDesigner/History?itemId=5
        Public Function History(itemId As Integer) As JsonResult
            Dim historyList = _wfService.GetWorkflowHistory(itemId)
            Return Json(historyList.Select(Function(h) New With {
                .fromState = h.FromState,
                .toState = h.ToState,
                .actionBy = h.ActionBy,
                .actionDate = h.ActionDate.ToString("dd-MMM-yyyy HH:mm"),
                .comments = h.Comments
            }), JsonRequestBehavior.AllowGet)
        End Function

        Private Sub SyncStatesAndTransitions(wf As AppWorkflow)
            If String.IsNullOrEmpty(wf.WorkflowJson) Then Return
            Try
                Dim obj = JsonConvert.DeserializeObject(Of Dictionary(Of String, Object))(wf.WorkflowJson)

                ' States
                If obj.ContainsKey("states") Then
                    Dim statesToken = obj("states")
                    Dim states = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(statesToken.ToString())
                    For Each s In states
                        Dim stateCode = s("code").ToString()
                        Dim existing = _db.AppWorkflowStates.FirstOrDefault(Function(x) x.WorkflowId = wf.WorkflowId AndAlso x.StateCode = stateCode)
                        If existing Is Nothing Then
                            _db.AppWorkflowStates.Add(New AppWorkflowState With {
                                .WorkflowId = wf.WorkflowId,
                                .StateName = s("name").ToString(),
                                .StateCode = stateCode,
                                .IsStart = s.ContainsKey("isStart") AndAlso CBool(s("isStart")),
                                .IsEnd = s.ContainsKey("isEnd") AndAlso CBool(s("isEnd")),
                                .ColorCode = If(s.ContainsKey("color"), s("color").ToString(), "")
                            })
                        End If
                    Next
                    _db.SaveChanges()
                End If

                ' Transitions
                If obj.ContainsKey("transitions") Then
                    Dim transToken = obj("transitions")
                    Dim transitions = JsonConvert.DeserializeObject(Of List(Of Dictionary(Of String, Object)))(transToken.ToString())

                    ' Remove old transitions for this workflow
                    Dim oldTransitions = _db.AppWorkflowTransitions.Where(Function(t) t.WorkflowId = wf.WorkflowId).ToList()
                    For Each oldT In oldTransitions
                        _db.AppWorkflowTransitions.Remove(oldT)
                    Next
                    _db.SaveChanges()

                    ' Add new transitions from JSON
                    For Each t In transitions
                        Dim fromCode = t("fromCode").ToString()
                        Dim toCode = t("toCode").ToString()
                        Dim fromState = _db.AppWorkflowStates.FirstOrDefault(Function(s) s.WorkflowId = wf.WorkflowId AndAlso s.StateCode = fromCode)
                        Dim toState = _db.AppWorkflowStates.FirstOrDefault(Function(s) s.WorkflowId = wf.WorkflowId AndAlso s.StateCode = toCode)

                        If fromState IsNot Nothing AndAlso toState IsNot Nothing Then
                            _db.AppWorkflowTransitions.Add(New AppWorkflowTransition With {
                                .WorkflowId = wf.WorkflowId,
                                .FromStateId = fromState.StateId,
                                .ToStateId = toState.StateId,
                                .ActionName = t("actionName").ToString(),
                                .ActionCode = If(t.ContainsKey("actionCode"), t("actionCode").ToString(), ""),
                                .RoleRequired = If(t.ContainsKey("role"), t("role").ToString(), ""),
                                .ConditionExpression = If(t.ContainsKey("condition"), t("condition").ToString(), "")
                            })
                        End If
                    Next
                    _db.SaveChanges()
                End If
            Catch
                ' Ignore JSON parse errors during sync
            End Try
        End Sub

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
