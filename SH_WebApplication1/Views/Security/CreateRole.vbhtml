@ModelType SH_WebApplication1.Models.Metadata.AppRole
@Code
    ViewData("Title") = "Create Role"
End Code

<div class="container mt-4" style="max-width:600px">
    <h2><i class="bi bi-plus-circle"></i> Create Role</h2>
    <hr />
    @Using Html.BeginForm()
        @Html.AntiForgeryToken()
        @<div class="card shadow-sm">
            <div class="card-body">
                <div class="mb-3">
                    @Html.LabelFor(Function(m) m.RoleName, New With {.class = "form-label fw-bold"})
                    @Html.TextBoxFor(Function(m) m.RoleName, New With {.class = "form-control"})
                    @Html.ValidationMessageFor(Function(m) m.RoleName, "", New With {.class = "text-danger"})
                </div>
                <div class="mb-3">
                    @Html.LabelFor(Function(m) m.Description, New With {.class = "form-label fw-bold"})
                    @Html.TextAreaFor(Function(m) m.Description, 3, 0, New With {.class = "form-control"})
                </div>
                <div class="d-flex gap-2">
                    <button type="submit" class="btn btn-primary">Create</button>
                    @Html.ActionLink("Cancel", "Roles", Nothing, New With {.class = "btn btn-secondary"})
                </div>
            </div>
        </div>
    End Using
</div>
