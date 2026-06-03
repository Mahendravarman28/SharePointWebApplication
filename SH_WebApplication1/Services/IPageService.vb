Imports SH_WebApplication1.Models.Metadata

Namespace Services

    Public Interface IPageService
        Function GetPages(listId As Integer) As List(Of AppPage)
        Function GetPageById(pageId As Integer) As AppPage
        Function GetDefaultHomePage(listId As Integer) As AppPage
        Function SavePage(page As AppPage) As AppPage
        Function SetDefaultHomePage(pageId As Integer) As Boolean
        Function DeletePage(pageId As Integer) As Boolean
    End Interface

End Namespace
