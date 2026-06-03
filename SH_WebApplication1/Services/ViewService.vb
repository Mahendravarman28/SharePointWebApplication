Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Class ViewService
        Implements IViewService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetViews(listId As Integer) As List(Of AppView) Implements IViewService.GetViews
            Return _db.AppViews.Where(Function(v) v.ListId = listId).OrderBy(Function(v) v.ViewName).ToList()
        End Function

        Public Function GetViewById(viewId As Integer) As AppView Implements IViewService.GetViewById
            Return _db.AppViews.Find(viewId)
        End Function

        Public Function GetDefaultView(listId As Integer) As AppView Implements IViewService.GetDefaultView
            Return _db.AppViews.FirstOrDefault(Function(v) v.ListId = listId AndAlso v.IsDefault)
        End Function

        Public Function SaveView(view As AppView) As AppView Implements IViewService.SaveView
            If view.ViewId = 0 Then
                view.CreatedDate = DateTime.Now
                _db.AppViews.Add(view)
            Else
                Dim existing = _db.AppViews.Find(view.ViewId)
                If existing IsNot Nothing Then
                    existing.ViewName = view.ViewName
                    existing.ViewType = view.ViewType
                    existing.ViewConfigJson = view.ViewConfigJson
                    existing.IsPublic = view.IsPublic
                End If
            End If
            _db.SaveChanges()
            Return view
        End Function

        Public Function SetDefaultView(viewId As Integer) As Boolean Implements IViewService.SetDefaultView
            Dim view = _db.AppViews.Find(viewId)
            If view Is Nothing Then Return False
            For Each v In _db.AppViews.Where(Function(x) x.ListId = view.ListId)
                v.IsDefault = (v.ViewId = viewId)
            Next
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeleteView(viewId As Integer) As Boolean Implements IViewService.DeleteView
            Dim view = _db.AppViews.Find(viewId)
            If view Is Nothing Then Return False
            _db.AppViews.Remove(view)
            _db.SaveChanges()
            Return True
        End Function

    End Class

End Namespace
