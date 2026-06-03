Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Class FormService
        Implements IFormService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetForms(listId As Integer) As List(Of AppForm) Implements IFormService.GetForms
            Return _db.AppForms.Where(Function(f) f.ListId = listId).ToList()
        End Function

        Public Function GetFormById(formId As Integer) As AppForm Implements IFormService.GetFormById
            Return _db.AppForms.Find(formId)
        End Function

        Public Function GetFormByType(listId As Integer, formType As String) As AppForm Implements IFormService.GetFormByType
            Return _db.AppForms.FirstOrDefault(Function(f) f.ListId = listId AndAlso f.FormType = formType AndAlso f.IsPublished)
        End Function

        Public Function SaveForm(form As AppForm) As AppForm Implements IFormService.SaveForm
            If form.FormId = 0 Then
                form.CreatedDate = DateTime.Now
                form.VersionNo = 1
                _db.AppForms.Add(form)
            Else
                Dim existing = _db.AppForms.Find(form.FormId)
                If existing IsNot Nothing Then
                    existing.FormName = form.FormName
                    existing.LayoutJson = form.LayoutJson
                    existing.VersionNo += 1
                End If
            End If
            _db.SaveChanges()
            Return form
        End Function

        Public Function PublishForm(formId As Integer) As Boolean Implements IFormService.PublishForm
            Dim form = _db.AppForms.Find(formId)
            If form Is Nothing Then Return False
            form.IsPublished = True
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteForm(formId As Integer) As Boolean Implements IFormService.DeleteForm
            Dim form = _db.AppForms.Find(formId)
            If form Is Nothing Then Return False
            _db.AppForms.Remove(form)
            _db.SaveChanges()
            Return True
        End Function

    End Class

End Namespace
