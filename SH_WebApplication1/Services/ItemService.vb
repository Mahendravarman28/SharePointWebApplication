Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata
Imports System.Data.Entity

Namespace Services

    Public Class ItemService
        Implements IItemService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetItems(listId As Integer, Optional pageIndex As Integer = 0, Optional pageSize As Integer = 50) As List(Of AppItem) Implements IItemService.GetItems
            Return _db.AppItems _
                      .Include(Function(i) i.Values) _
                      .Where(Function(i) i.ListId = listId) _
                      .OrderByDescending(Function(i) i.CreatedDate) _
                      .Skip(pageIndex * pageSize) _
                      .Take(pageSize) _
                      .ToList()
        End Function

        Public Function GetItemById(itemId As Integer) As AppItem Implements IItemService.GetItemById
            Return _db.AppItems _
                      .Include(Function(i) i.Values) _
                      .Include(Function(i) i.Attachments) _
                      .FirstOrDefault(Function(i) i.ItemId = itemId)
        End Function

        Public Function CreateItem(listId As Integer, fieldValues As Dictionary(Of Integer, String), userId As String) As AppItem Implements IItemService.CreateItem
            Dim count = _db.AppItems.Count(Function(i) i.ListId = listId) + 1
            Dim item As New AppItem With {
                .ListId = listId,
                .ItemNo = $"ITEM-{count:D5}",
                .CurrentStatus = "Draft",
                .CreatedBy = userId,
                .CreatedDate = DateTime.Now
            }
            _db.AppItems.Add(item)
            _db.SaveChanges()

            For Each kv In fieldValues
                Dim field = _db.AppListFields.Find(kv.Key)
                If field Is Nothing Then Continue For
                Dim val As New AppItemValue With {
                    .ItemId = item.ItemId,
                    .FieldId = kv.Key
                }
                SetTypedValue(val, field.DataType, kv.Value)
                _db.AppItemValues.Add(val)
            Next
            _db.SaveChanges()
            Return item
        End Function

        Public Function UpdateItem(itemId As Integer, fieldValues As Dictionary(Of Integer, String), userId As String) As Boolean Implements IItemService.UpdateItem
            Dim item = _db.AppItems.Find(itemId)
            If item Is Nothing Then Return False
            item.ModifiedBy = userId
            item.ModifiedDate = DateTime.Now
            For Each kv In fieldValues
                Dim existing = _db.AppItemValues.FirstOrDefault(Function(v) v.ItemId = itemId AndAlso v.FieldId = kv.Key)
                Dim field = _db.AppListFields.Find(kv.Key)
                If field Is Nothing Then Continue For
                If existing Is Nothing Then
                    existing = New AppItemValue With {.ItemId = itemId, .FieldId = kv.Key}
                    _db.AppItemValues.Add(existing)
                End If
                SetTypedValue(existing, field.DataType, kv.Value)
            Next
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteItem(itemId As Integer) As Boolean Implements IItemService.DeleteItem
            Dim item = _db.AppItems.Find(itemId)
            If item Is Nothing Then Return False
            _db.AppItems.Remove(item)
            _db.SaveChanges()
            Return True
        End Function

        Public Function GetItemValue(itemId As Integer, fieldId As Integer) As AppItemValue Implements IItemService.GetItemValue
            Return _db.AppItemValues.FirstOrDefault(Function(v) v.ItemId = itemId AndAlso v.FieldId = fieldId)
        End Function

        Public Function CountItems(listId As Integer) As Integer Implements IItemService.CountItems
            Return _db.AppItems.Count(Function(i) i.ListId = listId)
        End Function

        Private Sub SetTypedValue(val As AppItemValue, dataType As String, rawValue As String)
            val.FieldValueText = rawValue
            Select Case dataType.ToLower()
                Case "number", "autonumber"
                    Dim n As Decimal
                    If Decimal.TryParse(rawValue, n) Then val.FieldValueNumber = n
                Case "decimal", "currency"
                    Dim d As Decimal
                    If Decimal.TryParse(rawValue, d) Then val.FieldValueNumber = d
                Case "datetime"
                    Dim dt As DateTime
                    If DateTime.TryParse(rawValue, dt) Then val.FieldValueDate = dt
                Case "multiselect", "lookup"
                    val.FieldValueJson = rawValue
            End Select
        End Sub

    End Class

End Namespace
