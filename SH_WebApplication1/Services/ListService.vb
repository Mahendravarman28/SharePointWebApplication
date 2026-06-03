Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports System.Data.Entity

Namespace Services

    Public Class ListService
        Implements IListService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetAllLists() As List(Of AppList) Implements IListService.GetAllLists
            Return _db.AppLists.Where(Function(l) l.IsActive).OrderBy(Function(l) l.ListName).ToList()
        End Function

        Public Function GetListById(listId As Integer) As AppList Implements IListService.GetListById
            Return _db.AppLists.Include(Function(l) l.Fields).FirstOrDefault(Function(l) l.ListId = listId)
        End Function

        Public Function GetListByCode(listCode As String) As AppList Implements IListService.GetListByCode
            Return _db.AppLists.Include(Function(l) l.Fields).FirstOrDefault(Function(l) l.ListCode = listCode)
        End Function

        Public Function CreateList(model As AppList) As AppList Implements IListService.CreateList
            model.CreatedDate = DateTime.Now
            model.VersionNo = 1
            _db.AppLists.Add(model)
            _db.SaveChanges()
            Return model
        End Function

        Public Function UpdateList(model As AppList) As Boolean Implements IListService.UpdateList
            Dim existing = _db.AppLists.Find(model.ListId)
            If existing Is Nothing Then Return False
            existing.ListName = model.ListName
            existing.Description = model.Description
            existing.EnableAttachments = model.EnableAttachments
            existing.EnableVersioning = model.EnableVersioning
            existing.EnableApprovalWorkflow = model.EnableApprovalWorkflow
            existing.ModifiedBy = model.ModifiedBy
            existing.ModifiedDate = DateTime.Now
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteList(listId As Integer) As Boolean Implements IListService.DeleteList
            Dim existing = _db.AppLists.Find(listId)
            If existing Is Nothing Then Return False
            existing.IsActive = False
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetFields(listId As Integer) As List(Of AppListField) Implements IListService.GetFields
            Return _db.AppListFields.Where(Function(f) f.ListId = listId).OrderBy(Function(f) f.DisplayOrder).ToList()
        End Function

        Public Function GetFieldById(fieldId As Integer) As AppListField Implements IListService.GetFieldById
            Return _db.AppListFields.Find(fieldId)
        End Function

        Public Function AddField(field As AppListField) As AppListField Implements IListService.AddField
            If field.DisplayOrder = 0 Then
                Dim maxOrder = _db.AppListFields.Where(Function(f) f.ListId = field.ListId) _
                                               .Select(Function(f) f.DisplayOrder) _
                                               .DefaultIfEmpty(0).Max()
                field.DisplayOrder = maxOrder + 10
            End If
            _db.AppListFields.Add(field)
            _db.SaveChanges()
            Return field
        End Function

        Public Function UpdateField(field As AppListField) As Boolean Implements IListService.UpdateField
            Dim existing = _db.AppListFields.Find(field.FieldId)
            If existing Is Nothing Then Return False
            existing.DisplayName = field.DisplayName
            existing.DataType = field.DataType
            existing.IsRequired = field.IsRequired
            existing.IsUnique = field.IsUnique
            existing.DefaultValue = field.DefaultValue
            existing.ValidationRule = field.ValidationRule
            existing.MinLength = field.MinLength
            existing.MaxLength = field.MaxLength
            existing.RegexValidation = field.RegexValidation
            existing.DropdownConfig = field.DropdownConfig
            existing.LookupConfig = field.LookupConfig
            existing.IsSearchable = field.IsSearchable
            existing.IsSortable = field.IsSortable
            existing.IsFilterable = field.IsFilterable
            existing.IsVisibleInList = field.IsVisibleInList
            existing.IsVisibleInForm = field.IsVisibleInForm
            existing.DisplayOrder = field.DisplayOrder
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteField(fieldId As Integer) As Boolean Implements IListService.DeleteField
            Dim existing = _db.AppListFields.Find(fieldId)
            If existing Is Nothing Then Return False
            _db.AppListFields.Remove(existing)
            _db.SaveChanges()
            Return True
        End Function

        Public Function ReorderFields(fieldIds As List(Of Integer)) As Boolean Implements IListService.ReorderFields
            For i = 0 To fieldIds.Count - 1
                Dim f = _db.AppListFields.Find(fieldIds(i))
                If f IsNot Nothing Then f.DisplayOrder = (i + 1) * 10
            Next
            _db.SaveChanges()
            Return True
        End Function

    End Class

End Namespace
