@ModelType SH_WebApplication1.Models.Metadata.AppList
@Code
    ViewData("Title") = "Edit List"
End Code

<div class="container mt-4" style="max-width:700px">
    <h2><i class="bi bi-pencil-square"></i> Edit List</h2>
    <hr />
    @Using Html.BeginForm()
        @Html.AntiForgeryToken()
        @Html.HiddenFor(Function(m) m.ListId)
        @Html.HiddenFor(Function(m) m.ListCode)
        @<div class="card shadow-sm">
            <div class="card-body">
                <div class="mb-3">
                    @Html.LabelFor(Function(m) m.ListName, New With {.class = "form-label fw-bold"})
                    @Html.TextBoxFor(Function(m) m.ListName, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.ListName, "", New With {.class = "text-danger"})
                </div>
                <div class="mb-3">
                    @Html.LabelFor(Function(m) m.Description, New With {.class = "form-label fw-bold"})
                    @Html.TextAreaFor(Function(m) m.Description, 3, 0, New With {.class = "form-control"})
                </div>
                <div class="row mb-3">
                    <div class="col-md-4">
                        <div class="form-check form-switch">
                            @Html.CheckBoxFor(Function(m) m.EnableAttachments, New With {.class = "form-check-input"})
                            <label class="form-check-label">Enable Attachments</label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-check form-switch">
                            @Html.CheckBoxFor(Function(m) m.EnableVersioning, New With {.class = "form-check-input"})
                            <label class="form-check-label">Enable Versioning</label>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="form-check form-switch">
                            @Html.CheckBoxFor(Function(m) m.EnableApprovalWorkflow, New With {.class = "form-check-input"})
                            <label class="form-check-label">Enable Approval Workflow</label>
                        </div>
                    </div>
                </div>
                <div class="d-flex gap-2">
                    <button type="submit" class="btn btn-primary">Save</button>
                    @Html.ActionLink("Fields", "Fields", New With {.id = Model.ListId}, New With {.class = "btn btn-outline-primary"})
                    @Html.ActionLink("Cancel", "Index", Nothing, New With {.class = "btn btn-secondary"})
                </div>
            </div>
        </div>
    End Using
</div>
