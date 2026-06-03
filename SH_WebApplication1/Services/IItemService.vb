Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IItemService
        Function GetItems(listId As Integer, Optional pageIndex As Integer = 0, Optional pageSize As Integer = 50) As List(Of AppItem)
        Function GetItemById(itemId As Integer) As AppItem
        Function CreateItem(listId As Integer, fieldValues As Dictionary(Of Integer, String), userId As String) As AppItem
        Function UpdateItem(itemId As Integer, fieldValues As Dictionary(Of Integer, String), userId As String) As Boolean
        Function DeleteItem(itemId As Integer) As Boolean
        Function GetItemValue(itemId As Integer, fieldId As Integer) As AppItemValue
        Function CountItems(listId As Integer) As Integer
    End Interface

End Namespace
