@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppRole)
@Code
    ViewData("Title") = "Roles"
End Code

@Functions
    Private Function SystemBadge(isSystem As Boolean) As String
        If isSystem Then Return "<span class='badge bg-warning text-dark'>System</span>"
        Return ""
    End Function
    Private Function CardBorder(isSystem As Boolean) As String
        If isSystem Then Return "border-warning"
        Return ""
    End Function
End Functions

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-shield-lock-fill"></i> Roles &amp; Permissions</h2>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New Role", "CreateRole", Nothing, New With {.class = "btn btn-primary"})
            @Html.ActionLink("All Permissions", "Permissions", Nothing, New With {.class = "btn btn-outline-info"})
            @Html.ActionLink("User Assignments", "UserRoles", Nothing, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    <div class="row">
        @For Each role In Model
            @<div class="col-md-4 mb-3">
                <div class="card shadow-sm @CardBorder(role.IsSystem)">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <span><i class="bi bi-person-badge-fill me-1"></i> @role.RoleName</span>
                        @Html.Raw(SystemBadge(role.IsSystem))
                    </div>
                    <div class="card-body">
                        <p class="text-muted small mb-0">@role.Description</p>
                    </div>
                    <div class="card-footer d-flex gap-1">
                        @Html.ActionLink("Permissions", "RolePermissions", New With {.id = role.RoleId}, New With {.class = "btn btn-sm btn-outline-primary"})
                        @If Not role.IsSystem Then
                            @Html.ActionLink("Edit", "EditRole", New With {.id = role.RoleId}, New With {.class = "btn btn-sm btn-outline-secondary"})
                            @Using Html.BeginForm("DeleteRole", "Security", New With {.id = role.RoleId}, FormMethod.Post, New With {.style = "display:inline"})
                                @Html.AntiForgeryToken()
                                @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete role?')">Delete</button>
                            End Using
                        End If
                    </div>
                </div>
            </div>
        Next
    </div>
</div>
