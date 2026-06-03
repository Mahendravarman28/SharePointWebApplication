Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services
Imports Newtonsoft.Json

Namespace Controllers

    <Authorize>
    Public Class PageDesignerController
        Inherits Controller

        Private ReadOnly _pageService As IPageService
        Private ReadOnly _listService As IListService
        Private ReadOnly _itemService As IItemService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _pageService = New PageService(_db)
            _listService = New ListService(_db)
            _itemService = New ItemService(_db)
        End Sub

        ' GET: /PageDesigner/Index/5
        Public Function Index(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            Return View(_pageService.GetPages(listId))
        End Function

        ' GET: /PageDesigner/Designer?listId=5&pageId=0
        Public Function Designer(listId As Integer, Optional pageId As Integer = 0) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim page As AppPage
            If pageId > 0 Then
                page = _pageService.GetPageById(pageId)
            Else
                page = _pageService.GetDefaultHomePage(listId)
            End If
            If page Is Nothing Then
                page = New AppPage With {
                    .ListId = listId,
                    .PageName = $"{list.ListName} Home",
                    .LayoutJson = GetDefaultPageLayout(),
                    .IsDefaultHome = True
                }
            End If
            ViewBag.List = list
            Return View(page)
        End Function

        ' POST: /PageDesigner/Save
        <HttpPost>
        Public Function Save(listId As Integer, pageId As Integer, pageName As String, layoutJson As String) As JsonResult
            Try
                Dim page As New AppPage With {
                    .PageId = pageId,
                    .ListId = listId,
                    .PageName = pageName,
                    .LayoutJson = layoutJson,
                    .CreatedBy = User.Identity.Name
                }
                page = _pageService.SavePage(page)
                Return Json(New With {.success = True, .pageId = page.PageId})
            Catch ex As Exception
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

        ' POST: /PageDesigner/SetDefault
        <HttpPost>
        Public Function SetDefault(pageId As Integer) As JsonResult
            Return Json(New With {.success = _pageService.SetDefaultHomePage(pageId)})
        End Function

        ' GET: /PageDesigner/Home?listId=5
        Public Function Home(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            Dim page = _pageService.GetDefaultHomePage(listId)
            If page Is Nothing Then Return RedirectToAction("Designer", New With {.listId = listId})
            ViewBag.List = list
            ViewBag.TotalItems = _itemService.CountItems(listId)
            ViewBag.RecentItems = _itemService.GetItems(listId, 0, 5)
            ViewBag.Fields = _listService.GetFields(listId).Where(Function(f) f.IsVisibleInList).Take(4).ToList()
            Return View(page)
        End Function

        Private Function GetDefaultPageLayout() As String
            Dim layout = New With {
                .rows = New List(Of Object) From {
                    New With {
                        .rowId = "row1",
                        .widgets = New List(Of Object) From {
                            New With {Key .widgetId = "w1", Key .type = "KpiCount", Key .colWidth = 3, Key .title = "Total Items", Key .config = New With {Key .metric = "total"}},
                            New With {Key .widgetId = "w2", Key .type = "KpiCount", Key .colWidth = 3, Key .title = "Draft", Key .config = New With {Key .metric = "draft"}},
                            New With {Key .widgetId = "w3", Key .type = "KpiCount", Key .colWidth = 3, Key .title = "Approved", Key .config = New With {Key .metric = "approved"}},
                            New With {Key .widgetId = "w4", Key .type = "KpiCount", Key .colWidth = 3, Key .title = "Pending", Key .config = New With {Key .metric = "pending"}}
                        }
                    },
                    New With {
                        .rowId = "row2",
                        .widgets = New List(Of Object) From {
                            New With {Key .widgetId = "w5", Key .type = "RecentItems", Key .colWidth = 8, Key .title = "Recent Items", Key .config = New Object},
                            New With {Key .widgetId = "w6", Key .type = "ActionButtons", Key .colWidth = 4, Key .title = "Quick Actions", Key .config = New Object}
                        }
                    }
                }
            }
            Return JsonConvert.SerializeObject(layout)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
