Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Class PageService
        Implements IPageService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function GetPages(listId As Integer) As List(Of AppPage) Implements IPageService.GetPages
            Return _db.AppPages.Where(Function(p) p.ListId = listId).ToList()
        End Function

        Public Function GetPageById(pageId As Integer) As AppPage Implements IPageService.GetPageById
            Return _db.AppPages.Find(pageId)
        End Function

        Public Function GetDefaultHomePage(listId As Integer) As AppPage Implements IPageService.GetDefaultHomePage
            Return _db.AppPages.FirstOrDefault(Function(p) p.ListId = listId AndAlso p.IsDefaultHome)
        End Function

        Public Function SavePage(page As AppPage) As AppPage Implements IPageService.SavePage
            If page.PageId = 0 Then
                page.CreatedDate = DateTime.Now
                _db.AppPages.Add(page)
            Else
                Dim existing = _db.AppPages.Find(page.PageId)
                If existing IsNot Nothing Then
                    existing.PageName = page.PageName
                    existing.LayoutJson = page.LayoutJson
                End If
            End If
            _db.SaveChanges()
            Return page
        End Function

        Public Function SetDefaultHomePage(pageId As Integer) As Boolean Implements IPageService.SetDefaultHomePage
            Dim page = _db.AppPages.Find(pageId)
            If page Is Nothing Then Return False
            For Each p In _db.AppPages.Where(Function(x) x.ListId = page.ListId)
                p.IsDefaultHome = (p.PageId = pageId)
            Next
            _db.SaveChanges()
            Return True
        End Function

        Public Function DeletePage(pageId As Integer) As Boolean Implements IPageService.DeletePage
            Dim page = _db.AppPages.Find(pageId)
            If page Is Nothing Then Return False
            _db.AppPages.Remove(page)
            _db.SaveChanges()
            Return True
        End Function

    End Class

End Namespace
