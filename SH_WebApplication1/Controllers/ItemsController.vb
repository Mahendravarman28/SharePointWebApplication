Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services

Namespace Controllers

    <Authorize>
    Public Class ItemsController
        Inherits Controller

        Private ReadOnly _itemService As IItemService
        Private ReadOnly _listService As IListService
        Private ReadOnly _formService As IFormService
        Private ReadOnly _wfService As IWorkflowService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _itemService = New ItemService(_db)
            _listService = New ListService(_db)
            _formService = New FormService(_db)
            _wfService = New WorkflowService(_db)
        End Sub

        ' GET: /Items/Create?listId=5
        Public Function Create(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(listId).Where(Function(f) f.IsVisibleInForm).ToList()
            ViewBag.Form = _formService.GetFormByType(listId, "New")
            Return View()
        End Function

        ' POST: /Items/Create
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Create(listId As Integer, formCollection As FormCollection) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim fields = _listService.GetFields(listId)
            Dim values As New Dictionary(Of Integer, String)()
            For Each f In fields
                Dim key = $"field_{f.FieldId}"
                If formCollection.AllKeys.Contains(key) Then
                    values(f.FieldId) = formCollection(key)
                End If
            Next
            Dim item = _itemService.CreateItem(listId, values, User.Identity.Name)
            ' start workflow if published
            Dim wf = _wfService.GetPublishedWorkflow(listId)
            If wf IsNot Nothing Then
                Dim startState = _db.AppWorkflowStates.FirstOrDefault(Function(s) s.WorkflowId = wf.WorkflowId AndAlso s.IsStart)
                If startState IsNot Nothing Then
                    item.CurrentStateId = startState.StateId
                    item.CurrentStatus = startState.StateName
                    _db.SaveChanges()
                End If
            End If
            Return RedirectToAction("Display", New With {.id = item.ItemId})
        End Function

        ' GET: /Items/Edit/5
        Public Function Edit(id As Integer) As ActionResult
            Dim item = _itemService.GetItemById(id)
            If item Is Nothing Then Return HttpNotFound()
            Dim list = _listService.GetListById(item.ListId)
            ViewBag.List = list
            ViewBag.Item = item
            ViewBag.Fields = _listService.GetFields(item.ListId).Where(Function(f) f.IsVisibleInForm).ToList()
            ViewBag.Form = _formService.GetFormByType(item.ListId, "Edit")
            Return View()
        End Function

        ' POST: /Items/Edit/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Edit(id As Integer, formCollection As FormCollection) As ActionResult
            Dim item = _itemService.GetItemById(id)
            If item Is Nothing Then Return HttpNotFound()
            Dim fields = _listService.GetFields(item.ListId)
            Dim values As New Dictionary(Of Integer, String)()
            For Each f In fields
                Dim key = $"field_{f.FieldId}"
                If formCollection.AllKeys.Contains(key) Then
                    values(f.FieldId) = formCollection(key)
                End If
            Next
            _itemService.UpdateItem(id, values, User.Identity.Name)
            Return RedirectToAction("Display", New With {.id = id})
        End Function

        ' GET: /Items/Display/5
        Public Function Display(id As Integer) As ActionResult
            Dim item = _itemService.GetItemById(id)
            If item Is Nothing Then Return HttpNotFound()
            Dim list = _listService.GetListById(item.ListId)
            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(item.ListId).Where(Function(f) f.IsVisibleInForm).ToList()
            ViewBag.Transitions = _wfService.GetAvailableTransitions(id, User.Identity.Name)
            ViewBag.History = _wfService.GetWorkflowHistory(id)
            Return View(item)
        End Function

        ' POST: /Items/Delete/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Delete(id As Integer, listId As Integer) As ActionResult
            _itemService.DeleteItem(id)
            Return RedirectToAction("Render", "ViewDesigner", New With {.listId = listId})
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
