@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppView)
@Code
    ViewData("Title") = "View Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
End Code

@Functions
    Private Function ViewDefaultBadge(isDefault As Boolean) As String
        If isDefault Then Return "<span class='badge bg-primary'>Default</span>"
        Return ""
    End Function
    Private Function ViewPublicBadge(isPublic As Boolean) As String
        If isPublic Then Return "<span class='badge bg-success ms-1'>Public</span>"
        Return "<span class='badge bg-secondary ms-1'>Personal</span>"
    End Function
    Private Function ViewCardBorder(isDefault As Boolean) As String
        If isDefault Then Return "border-primary"
        Return ""
    End Function
End Functions

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-grid-3x3-gap"></i> Views — <span class="text-primary">@list.ListName</span></h2>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New View", "Designer", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("← Fields", "Fields", "ListBuilder", New With {.id = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>
    <div class="row">
        @For Each v In Model
            @<div class="col-md-4 mb-3">
                <div class="card shadow-sm h-100 @ViewCardBorder(v.IsDefault)">
                    <div class="card-header d-flex justify-content-between">
                        <span><i class="bi bi-eye"></i> @v.ViewName</span>
                        <div>
                            <span class="badge bg-info text-dark me-1">@v.ViewType</span>
                            @Html.Raw(ViewDefaultBadge(v.IsDefault))
                            @Html.Raw(ViewPublicBadge(v.IsPublic))
                        </div>
                    </div>
                    <div class="card-body">
                        <p class="text-muted small mb-0">Created: @v.CreatedDate.ToString("dd-MMM-yyyy")</p>
                    </div>
                    <div class="card-footer d-flex gap-1">
                        @Html.ActionLink("Design", "Designer", New With {.listId = v.ListId, .viewId = v.ViewId}, New With {.class = "btn btn-sm btn-primary"})
                        @Html.ActionLink("Open", "Render", New With {.listId = v.ListId, .viewId = v.ViewId}, New With {.class = "btn btn-sm btn-outline-info"})
                        @Using Html.BeginForm("SetDefault", "ViewDesigner", FormMethod.Post, New With {.style = "display:inline"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("viewId", v.ViewId)
                            @<button type="submit" class="btn btn-sm btn-outline-primary">Set Default</button>
                        End Using
                        @Using Html.BeginForm("Delete", "ViewDesigner", FormMethod.Post, New With {.style = "display:inline"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("viewId", v.ViewId)
                            @Html.Hidden("listId", v.ListId)
                            @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete view?')">Delete</button>
                        End Using
                    </div>
                </div>
            </div>
        Next
    </div>
</div>
