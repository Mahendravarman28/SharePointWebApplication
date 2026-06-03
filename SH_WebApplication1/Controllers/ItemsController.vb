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
            ' Initiate workflow if a published workflow exists for this list
            _wfService.InitiateWorkflow(item.ItemId, listId, User.Identity.Name)
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

        ' POST: /Items/UploadAttachment
        <HttpPost, ValidateAntiForgeryToken>
        Public Function UploadAttachment(itemId As Integer) As ActionResult
            Try
                Dim file = Request.Files(0)
                If file IsNot Nothing AndAlso file.ContentLength > 0 Then
                    Dim item = _itemService.GetItemById(itemId)
                    If item Is Nothing Then Return HttpNotFound()
                    ' Create folder ~/App_Data/Attachments/{itemId}/
                    Dim folder = Server.MapPath($"~/App_Data/Attachments/{itemId}")
                    If Not IO.Directory.Exists(folder) Then IO.Directory.CreateDirectory(folder)
                    Dim fileName = IO.Path.GetFileName(file.FileName)
                    Dim filePath = IO.Path.Combine(folder, fileName)
                    file.SaveAs(filePath)
                    ' Save attachment record
                    Dim att As New AppAttachment With {
                        .ItemId = itemId,
                        .FileName = fileName,
                        .ContentType = file.ContentType,
                        .FileSizeBytes = file.ContentLength,
                        .FilePath = filePath,
                        .UploadedBy = User.Identity.Name,
                        .UploadedDate = DateTime.Now
                    }
                    _db.AppAttachments.Add(att)
                    _db.SaveChanges()
                    TempData("Success") = "File uploaded successfully."
                End If
            Catch ex As Exception
                TempData("Error") = $"Upload failed: {ex.Message}"
            End Try
            Return RedirectToAction("Display", New With {.id = itemId})
        End Function

        ' GET: /Items/DownloadAttachment/5
        Public Function DownloadAttachment(id As Integer) As ActionResult
            Dim att = _db.AppAttachments.Find(id)
            If att Is Nothing OrElse Not IO.File.Exists(att.FilePath) Then Return HttpNotFound()
            Return File(att.FilePath, att.ContentType, att.FileName)
        End Function

        ' GET: /Items/ViewAttachment/5
        Public Function ViewAttachment(id As Integer) As ActionResult
            Dim att = _db.AppAttachments.Find(id)
            If att Is Nothing OrElse Not IO.File.Exists(att.FilePath) Then Return HttpNotFound()

            ' Determine if file can be viewed inline based on extension
            Dim ext = IO.Path.GetExtension(att.FileName).ToLower()
            Dim canViewInline = {".pdf", ".txt", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".svg"}.Contains(ext)

            ' Set content disposition to inline for viewable types
            If canViewInline Then
                Response.AddHeader("Content-Disposition", $"inline; filename=""{att.FileName}""")
                Return File(att.FilePath, att.ContentType)
            Else
                ' For Office documents and others, return file info for external viewer
                ViewBag.Attachment = att
                ViewBag.CanViewInBrowser = {".pdf"}.Contains(ext)
                ViewBag.IsOfficeDoc = {".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx"}.Contains(ext)
                Return View(att)
            End If
        End Function

        ' POST: /Items/DeleteAttachment/5
        <HttpPost, ValidateAntiForgeryToken>
        Public Function DeleteAttachment(id As Integer, itemId As Integer) As ActionResult
            Try
                Dim att = _db.AppAttachments.Find(id)
                If att IsNot Nothing Then
                    If IO.File.Exists(att.FilePath) Then IO.File.Delete(att.FilePath)
                    _db.AppAttachments.Remove(att)
                    _db.SaveChanges()
                    TempData("Success") = "Attachment deleted."
                End If
            Catch ex As Exception
                TempData("Error") = $"Delete failed: {ex.Message}"
            End Try
            Return RedirectToAction("Display", New With {.id = itemId})
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
