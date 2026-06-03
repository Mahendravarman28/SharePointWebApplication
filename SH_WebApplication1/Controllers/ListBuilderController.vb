Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services

Namespace Controllers

    <Authorize>
    Public Class ListBuilderController
        Inherits Controller

        Private ReadOnly _listService As IListService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _listService = New ListService(_db)

        End Sub

        ' GET: /ListBuilder/
        Public Function Index() As ActionResult
            Dim lists = _listService.GetAllLists()
            Return View(lists)
        End Function

        ' GET: /ListBuilder/Create
        Public Function Create() As ActionResult
            Return View(New AppList())
        End Function

        ' POST: /ListBuilder/Create
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Create(model As AppList) As ActionResult
            If ModelState.IsValid Then
                model.CreatedBy = User.Identity.Name
                _listService.CreateList(model)
                Return RedirectToAction("Fields", New With {.id = model.ListId})
            End If
            Return View(model)
        End Function

        ' GET: /ListBuilder/Edit/5
        Public Function Edit(id As Integer) As ActionResult
            Dim model = _listService.GetListById(id)
            If model Is Nothing Then Return HttpNotFound()
            Return View(model)
        End Function

        ' POST: /ListBuilder/Edit/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Edit(model As AppList) As ActionResult
            If ModelState.IsValid Then
                model.ModifiedBy = User.Identity.Name
                _listService.UpdateList(model)
                Return RedirectToAction("Index")
            End If
            Return View(model)
        End Function

        ' GET: /ListBuilder/Fields/5
        Public Function Fields(id As Integer) As ActionResult
            Dim list = _listService.GetListById(id)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(id)
            Return View()
        End Function

        ' GET: /ListBuilder/AddField/5
        Public Function AddField(listId As Integer) As ActionResult
            ViewBag.ListId = listId
            Return View(New AppListField With {.ListId = listId})
        End Function

        ' POST: /ListBuilder/AddField
        <HttpPost, ValidateAntiForgeryToken>
        Public Function AddField(model As AppListField) As ActionResult
            If ModelState.IsValid Then
                _listService.AddField(model)
                Return RedirectToAction("Fields", New With {.id = model.ListId})
            End If
            ViewBag.ListId = model.ListId
            Return View(model)
        End Function

        ' GET: /ListBuilder/EditField/5
        Public Function EditField(id As Integer) As ActionResult
            Dim field = _listService.GetFieldById(id)
            If field Is Nothing Then Return HttpNotFound()
            Return View(field)
        End Function

        ' POST: /ListBuilder/EditField
        <HttpPost, ValidateAntiForgeryToken>
        Public Function EditField(model As AppListField) As ActionResult
            If ModelState.IsValid Then
                _listService.UpdateField(model)
                Return RedirectToAction("Fields", New With {.id = model.ListId})
            End If
            Return View(model)
        End Function

        ' POST: /ListBuilder/DeleteField/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function DeleteField(id As Integer, listId As Integer) As ActionResult
            _listService.DeleteField(id)
            Return RedirectToAction("Fields", New With {.id = listId})
        End Function

        ' POST: /ListBuilder/Delete/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function Delete(id As Integer) As ActionResult
            _listService.DeleteList(id)
            Return RedirectToAction("Index")
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
