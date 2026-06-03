Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports SH_WebApplication1.Services
Imports Newtonsoft.Json

Namespace Controllers

    <Authorize>
    Public Class FormDesignerController
        Inherits Controller

        Private ReadOnly _formService As IFormService
        Private ReadOnly _listService As IListService
        Private ReadOnly _db As AppDbContext

        Public Sub New()
            _db = New AppDbContext()
            _formService = New FormService(_db)
            _listService = New ListService(_db)
        End Sub

        ' GET: /FormDesigner/Index/5  (list all forms for a list)
        Public Function Index(listId As Integer) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()
            ViewBag.List = list
            Dim forms = _formService.GetForms(listId)
            Return View(forms)
        End Function

        ' GET: /FormDesigner/Designer?listId=5&formType=New
        Public Function Designer(listId As Integer, Optional formType As String = "New", Optional formId As Integer = 0) As ActionResult
            Dim list = _listService.GetListById(listId)
            If list Is Nothing Then Return HttpNotFound()

            Dim form As AppForm
            If formId > 0 Then
                form = _formService.GetFormById(formId)
            Else
                form = _formService.GetFormByType(listId, formType)
            End If

            If form Is Nothing Then
                form = New AppForm With {
                    .ListId = listId,
                    .FormType = formType,
                    .FormName = $"{list.ListName} — {formType} Form",
                    .LayoutJson = GetDefaultLayout(list),
                    .IsPublished = False
                }
            End If

            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(listId)
            Return View(form)
        End Function

        ' POST: /FormDesigner/Save
        <HttpPost>
        Public Function Save(listId As Integer, formType As String, formId As Integer, formName As String, layoutJson As String) As JsonResult
            Try
                Dim form As New AppForm With {
                    .FormId = formId,
                    .ListId = listId,
                    .FormType = formType,
                    .FormName = formName,
                    .LayoutJson = layoutJson,
                    .CreatedBy = User.Identity.Name
                }
                form = _formService.SaveForm(form)
                Return Json(New With {.success = True, .formId = form.FormId})
            Catch ex As Exception
                Return Json(New With {.success = False, .message = ex.Message})
            End Try
        End Function

        ' POST: /FormDesigner/Publish/5
        <HttpPost>
        Public Function Publish(formId As Integer) As JsonResult
            Dim ok = _formService.PublishForm(formId)
            Return Json(New With {.success = ok})
        End Function

        ' GET: /FormDesigner/Preview/5
        Public Function Preview(formId As Integer) As ActionResult
            Dim form = _formService.GetFormById(formId)
            If form Is Nothing Then Return HttpNotFound()
            Dim list = _listService.GetListById(form.ListId)
            ViewBag.List = list
            ViewBag.Fields = _listService.GetFields(form.ListId)
            Return View(form)
        End Function

        ' DELETE: /FormDesigner/Delete/5
        <HttpPost>
        Public Function Delete(formId As Integer, listId As Integer) As ActionResult
            _formService.DeleteForm(formId)
            Return RedirectToAction("Index", New With {.listId = listId})
        End Function

        Private Function GetDefaultLayout(list As AppList) As String
            Dim layout = New With {
                .sections = New List(Of Object) From {
                    New With {
                        .sectionId = "section1",
                        .title = "General Information",
                        .columns = 2,
                        .fields = New List(Of Object)()
                    }
                },
                .rules = New List(Of Object)()
            }
            Return JsonConvert.SerializeObject(layout)
        End Function

        Protected Overrides Sub Dispose(disposing As Boolean)
            If disposing Then _db.Dispose()
            MyBase.Dispose(disposing)
        End Sub

    End Class

End Namespace
