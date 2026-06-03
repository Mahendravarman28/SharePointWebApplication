Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IListService
        Function GetAllLists() As List(Of AppList)
        Function GetListById(listId As Integer) As AppList
        Function GetListByCode(listCode As String) As AppList
        Function CreateList(model As AppList) As AppList
        Function UpdateList(model As AppList) As Boolean
        Function DeleteList(listId As Integer) As Boolean
        ' Field management
        Function GetFields(listId As Integer) As List(Of AppListField)
        Function GetFieldById(fieldId As Integer) As AppListField
        Function AddField(field As AppListField) As AppListField
        Function UpdateField(field As AppListField) As Boolean
        Function DeleteField(fieldId As Integer) As Boolean
        Function ReorderFields(fieldIds As List(Of Integer)) As Boolean
    End Interface

End Namespace
