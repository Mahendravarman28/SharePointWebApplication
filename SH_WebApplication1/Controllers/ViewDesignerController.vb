Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services
Imports Newtonsoft.Json

Namespace Controllers

    <Authorize>
    Public Class ViewDesignerController
        Inherits Controller

        Private ReadOnly _viewService As IViewService
        Private ReadOnly _listService As IListService
        Private ReadOnly _itemService As IItemService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _viewService = New ViewService(_db)
            _listService = New ListService(_db)
            _itemService = New ItemService(_db)
        End Sub

        ' GET: /ViewDesigner/Index/5
        Public Function Index(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            Return View(_viewService.GetViews(listId))
        End Function

        ' GET: /ViewDesigner/Designer?listId=5&viewId=0
        Public Function Designer(listId As Integer, Optional viewId As Integer = 0) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim vwModel As AppView
            If viewId > 0 Then
                vwModel = _viewService.GetViewById(viewId)
            Else
                vwModel = New AppView With {
                    .ListId = listId,
                    .ViewName = $"{list.ListName} — Default View",
                    .ViewType = "Grid",
                    .ViewConfigJson = GetDefaultViewConfig(listId),
                    .IsPublic = True
                }
            End If
            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(listId)
            Return View(vwModel)
        End Function

        ' POST: /ViewDesigner/Save
        <HttpPost>
        Public Function Save(listId As Integer, viewId As Integer, viewName As String, viewType As String, viewConfigJson As String, isPublic As Boolean) As JsonResult
            Try
                Dim view As New AppView With {
                    .ViewId = viewId,
                    .ListId = listId,
                    .ViewName = viewName,
                    .ViewType = viewType,
                    .ViewConfigJson = viewConfigJson,
                    .IsPublic = isPublic,
                    .CreatedBy = User.Identity.Name
                }
                view = _viewService.SaveView(view)
                Return Json(New With {.success = True, .viewId = view.ViewId})
            Catch ex As Exception
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

        ' POST: /ViewDesigner/SetDefault
        <HttpPost>
        Public Function SetDefault(viewId As Integer) As JsonResult
            Return Json(New With {.success = _viewService.SetDefaultView(viewId)})
        End Function

        ' POST: /ViewDesigner/Delete
        <HttpPost>
        Public Function Delete(viewId As Integer, listId As Integer) As ActionResult
            _viewService.DeleteView(viewId)
            Return RedirectToAction("Index", New With {.listId = listId})
        End Function

        ' GET: /ViewDesigner/Render?listId=5&viewId=2
        Public Function Render(listId As Integer, Optional viewId As Integer = 0) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim vwModel = If(viewId > 0, _viewService.GetViewById(viewId), _viewService.GetDefaultView(listId))
            If vwModel Is Nothing Then
                vwModel = New AppView With {.ViewType = "Grid", .ViewConfigJson = GetDefaultViewConfig(listId)}
            End If
            ViewBag.List = list
            ViewBag.View = vwModel
            ViewBag.Fields = _listService.GetFields(listId).Where(Function(f) f.IsVisibleInList).ToList()
            ViewBag.Items = _itemService.GetItems(listId)
            ViewBag.TotalCount = _itemService.CountItems(listId)
            Return View("Render")
        End Function

        Private Function GetDefaultViewConfig(listId As Integer) As String
            Dim fields = _listService.GetFields(listId).Where(Function(f) f.IsVisibleInList).Take(6)
            Dim config = New With {
                .columns = fields.Select(Function(f) New With {Key .fieldId = f.FieldId, Key .label = f.DisplayName, Key .sortable = f.IsSortable}).ToList(),
                .defaultSort = New With {Key .fieldId = 0, Key .direction = "asc"},
                .filters = New List(Of Object)(),
                .groupBy = "",
                .pageSize = 20
            }
            Return JsonConvert.SerializeObject(config)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
