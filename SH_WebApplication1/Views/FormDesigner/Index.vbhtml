@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppForm)
@Code
    ViewData("Title") = "Form Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
End Code

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-ui-checks-grid"></i> Forms — <span class="text-primary">@list.ListName</span></h2>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New Form", "Designer", New With {.listId = list.ListId, .formType = "New"}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("Edit Form", "Designer", New With {.listId = list.ListId, .formType = "Edit"}, New With {.class = "btn btn-outline-secondary"})
            @Html.ActionLink("Display Form", "Designer", New With {.listId = list.ListId, .formType = "Display"}, New With {.class = "btn btn-outline-info"})
            @Html.ActionLink("← Lists", "Index", "ListBuilder", Nothing, New With {.class = "btn btn-outline-dark"})
        </div>
    </div>

    <div class="row">
        @For Each form In Model
            @<div class="col-md-4 mb-3">
                <div class="card shadow-sm h-100">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-file-earmark-text"></i> @form.FormName</span>
                        @If form.IsPublished Then
                            @Html.Raw("<span class='badge bg-success'>Published</span>")
                        Else
                            @Html.Raw("<span class='badge bg-warning text-dark'>Draft</span>")
                        End If
                    </div>
                    <div class="card-body">
                        <p class="mb-1"><strong>Type:</strong> <span class="badge bg-info text-dark">@form.FormType</span></p>
                        <p class="mb-1"><strong>Version:</strong> @form.VersionNo</p>
                        <p class="text-muted small mb-0">Created: @form.CreatedDate.ToString("dd-MMM-yyyy")</p>
                    </div>
                    <div class="card-footer d-flex gap-2">
                        @Html.ActionLink("Design", "Designer", New With {.listId = form.ListId, .formId = form.FormId}, New With {.class = "btn btn-sm btn-primary"})
                        @Html.ActionLink("Preview", "Preview", New With {.formId = form.FormId}, New With {.class = "btn btn-sm btn-outline-info"})
                        @Using Html.BeginForm("Delete", "FormDesigner", FormMethod.Post, New With {.style = "display:inline"})
                            @Html.AntiForgeryToken()
                            @Html.Hidden("formId", form.FormId)
                            @Html.Hidden("listId", form.ListId)
                            @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete form?')">Delete</button>
                        End Using
                    </div>
                </div>
            </div>
        Next
    </div>
</div>
