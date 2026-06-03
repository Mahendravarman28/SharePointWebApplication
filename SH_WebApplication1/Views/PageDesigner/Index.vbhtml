@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppPage)
@Code
    ViewData("Title") = "Pages"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
End Code

@Functions
    Private Function DefaultHomeBadge(isDefault As Boolean) As String
        If isDefault Then Return "<span class='badge bg-success'>Default Home</span>"
        Return ""
    End Function
    Private Function PageCardBorder(isDefault As Boolean) As String
        If isDefault Then Return "border-success"
        Return ""
    End Function
End Functions

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-layout-wtf"></i> Pages — <span class="text-primary">@list.ListName</span></h2>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New Page", "Designer", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("View Home", "Home", New With {.listId = list.ListId}, New With {.class = "btn btn-success"})
        </div>
    </div>
    <div class="row">
        @For Each p In Model
            @<div class="col-md-4 mb-3">
                <div class="card shadow-sm @PageCardBorder(p.IsDefaultHome)">
                    <div class="card-header d-flex justify-content-between">
                        <span><i class="bi bi-file-earmark-richtext"></i> @p.PageName</span>
                        @Html.Raw(DefaultHomeBadge(p.IsDefaultHome))
                    </div>
                    <div class="card-body">
                        <p class="text-muted small mb-0">Created: @p.CreatedDate.ToString("dd-MMM-yyyy")</p>
                    </div>
                    <div class="card-footer d-flex gap-1">
                        @Html.ActionLink("Design", "Designer", New With {.listId = p.ListId, .pageId = p.PageId}, New With {.class = "btn btn-sm btn-primary"})
                        @Html.ActionLink("Preview", "Home", New With {.listId = p.ListId}, New With {.class = "btn btn-sm btn-outline-success"})
                        @If Not p.IsDefaultHome Then
                            @Using Html.BeginForm("SetDefault", "PageDesigner", FormMethod.Post, New With {.style = "display:inline"})
                                @Html.AntiForgeryToken()
                                @Html.Hidden("pageId", p.PageId)
                                @<button type="submit" class="btn btn-sm btn-outline-primary">Set Default</button>
                            End Using
                        End If
                    </div>
                </div>
            </div>
        Next
    </div>
</div>
