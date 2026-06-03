@Code
    ViewData("Title") = "Role Permissions"
    Dim role As SH_WebApplication1.Models.Metadata.AppRole = ViewBag.Role
    Dim allPerms As List(Of SH_WebApplication1.Models.Metadata.AppPermission) = ViewBag.AllPermissions
    Dim assignedPerms As List(Of SH_WebApplication1.Models.Metadata.AppRolePermission) = ViewBag.AssignedPermissions
End Code

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-key-fill"></i> Permissions — <span class="text-primary">@role.RoleName</span></h4>
        @Html.ActionLink("← Roles", "Roles", Nothing, New With {.class = "btn btn-outline-secondary"})
    </div>

    <div class="row">
        <!-- All Permissions -->
        <div class="col-md-6">
            <div class="card shadow-sm">
                <div class="card-header fw-bold">All Permissions</div>
                <div class="list-group list-group-flush" style="max-height:500px;overflow-y:auto" id="allPermList">
                    @For Each p In allPerms
                        Dim isAssigned = assignedPerms.Any(Function(rp) rp.PermissionId = p.PermissionId)
                        @<div class="list-group-item d-flex justify-content-between align-items-center">
                            <div>
                                <strong>@p.PermissionName</strong>
                                <span class="badge bg-info text-dark ms-1" style="font-size:0.65rem">@p.PermissionLevel</span>
                                <div class="text-muted small">@p.PermissionCode</div>
                            </div>
                            @If Not isAssigned Then
                                @<button class="btn btn-sm btn-outline-success btn-assign-perm"
                                         data-role-id="@role.RoleId"
                                         data-perm-id="@p.PermissionId">
                                    <i class="bi bi-plus"></i> Assign
                                </button>
                            Else
                                @Html.Raw("<span class='badge bg-success'>Assigned</span>")
                            End If
                        </div>
                    Next
                </div>
            </div>
        </div>

        <!-- Assigned Permissions -->
        <div class="col-md-6">
            <div class="card shadow-sm">
                <div class="card-header fw-bold">Assigned Permissions</div>
                <div class="list-group list-group-flush" style="max-height:500px;overflow-y:auto" id="assignedPermList">
                    @For Each rp In assignedPerms
                        @<div class="list-group-item d-flex justify-content-between align-items-center" data-rp-id="@rp.RolePermissionId">
                            <div>
                                <strong>@rp.Permission?.PermissionName</strong>
                                <div class="text-muted small">@rp.Permission?.PermissionCode</div>
                            </div>
                            <button class="btn btn-sm btn-outline-danger btn-revoke-perm" data-rp-id="@rp.RolePermissionId">
                                <i class="bi bi-dash-circle"></i> Revoke
                            </button>
                        </div>
                    Next
                </div>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script>
        $(document).on('click', '.btn-assign-perm', function () {
            var roleId = $(this).data('role-id');
            var permId = $(this).data('perm-id');
            var btn = $(this);
            $.post('@Url.Action("AssignPermission", "Security")', { roleId: roleId, permissionId: permId, listId: null }, function (res) {
                if (res.success) { location.reload(); }
            });
        });

        $(document).on('click', '.btn-revoke-perm', function () {
            var rpId = $(this).data('rp-id');
            $.post('@Url.Action("RevokePermission", "Security")', { rolePermissionId: rpId }, function (res) {
                if (res.success) { location.reload(); }
            });
        });
    </script>
End Section
