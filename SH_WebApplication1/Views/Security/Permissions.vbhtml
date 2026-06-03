@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppPermission)
@Code
    ViewData("Title") = "All Permissions"
End Code

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-key-fill"></i> All Permissions</h2>
        @Html.ActionLink("← Roles", "Roles", Nothing, New With {.class = "btn btn-outline-secondary"})
    </div>
    <div class="card shadow-sm">
        <div class="card-body p-0">
            <table class="table table-hover mb-0">
                <thead class="table-dark">
                    <tr><th>Permission Name</th><th>Code</th><th>Level</th><th>Description</th></tr>
                </thead>
                <tbody>
                    @For Each p In Model
                        @<tr>
                            <td><strong>@p.PermissionName</strong></td>
                            <td><code>@p.PermissionCode</code></td>
                            <td><span class="badge bg-info text-dark">@p.PermissionLevel</span></td>
                            <td class="text-muted small">@p.Description</td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>
