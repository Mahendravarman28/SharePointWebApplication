Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IViewService
        Function GetViews(listId As Integer) As List(Of AppView)
        Function GetViewById(viewId As Integer) As AppView
        Function GetDefaultView(listId As Integer) As AppView
        Function SaveView(view As AppView) As AppView
        Function SetDefaultView(viewId As Integer) As Boolean
        Function DeleteView(viewId As Integer) As Boolean
    End Interface

End Namespace
