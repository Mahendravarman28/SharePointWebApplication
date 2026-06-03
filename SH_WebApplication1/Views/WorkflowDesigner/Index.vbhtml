@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppWorkflow)
@Code
    ViewData("Title") = "Workflows"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
End Code

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-diagram-3-fill"></i> Workflows — <span class="text-primary">@list.ListName</span></h2>
        @Html.ActionLink("+ New Workflow", "Designer", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
    </div>
    <div class="row">
        @For Each wf In Model
            Dim publishedBadge = If(wf.IsPublished, "<span class='badge bg-success'>Published</span>", "<span class='badge bg-warning text-dark'>Draft</span>")
            Dim borderClass = If(wf.IsPublished, "border-success", "")
            @<div class="col-md-4 mb-3">
                <div class="card shadow-sm @borderClass">
                    <div class="card-header d-flex justify-content-between">
                        <span><i class="bi bi-diagram-3"></i> @wf.WorkflowName</span>
                        @Html.Raw(publishedBadge)
                    </div>
                    <div class="card-body">
                        <p class="text-muted small mb-0">Created: @wf.CreatedDate.ToString("dd-MMM-yyyy")</p>
                    </div>
                    <div class="card-footer d-flex gap-1">
                        @Html.ActionLink("Design", "Designer", New With {.listId = wf.ListId, .workflowId = wf.WorkflowId}, New With {.class = "btn btn-sm btn-primary"})
                        @Using Html.BeginForm("Delete", "WorkflowDesigner", FormMethod.Post, New With {.style = "display:inline"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("workflowId", wf.WorkflowId)
                            @Html.Hidden("listId", wf.ListId)
                            @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete workflow?')">Delete</button>
                        End Using
                    </div>
                </div>
            </div>
        Next
    </div>
</div>
