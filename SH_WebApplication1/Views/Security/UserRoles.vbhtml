@Code
    ViewData("Title") = "User Role Assignments"
    Dim roles As List(Of SH_WebApplication1.Models.Metadata.AppRole) = ViewBag.Roles
    Dim lists As List(Of SH_WebApplication1.Models.Metadata.AppList) = ViewBag.Lists
End Code

<div class="container mt-4" style="max-width:700px">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-person-check-fill"></i> Assign User Roles</h4>
        @Html.ActionLink("← Roles", "Roles", Nothing, New With {.class = "btn btn-outline-secondary"})
    </div>

    <div class="card shadow-sm">
        <div class="card-body">
            <div class="mb-3">
                <label class="form-label fw-bold">User ID / Email</label>
                <input type="text" id="userId" class="form-control" placeholder="user@example.com">
            </div>
            <div class="mb-3">
                <label class="form-label fw-bold">Role</label>
                <select id="roleId" class="form-select">
                    @For Each r In roles
                        @<option value="@r.RoleId">@r.RoleName</option>
                    Next
                </select>
            </div>
            <div class="mb-3">
                <label class="form-label fw-bold">Scope (optional — leave blank for global)</label>
                <select id="listId" class="form-select">
                    <option value="">Global (All Lists)</option>
                    @For Each l In lists
                        @<option value="@l.ListId">@l.ListName</option>
                    Next
                </select>
            </div>
            <button class="btn btn-primary" id="btnAssignRole"><i class="bi bi-person-plus"></i> Assign Role</button>
            <div id="assignMsg" class="mt-2"></div>
        </div>
    </div>
</div>

@section scripts
    <script>
        $('#btnAssignRole').click(function () {
            var uid = $('#userId').val().trim();
            if (!uid) { alert('Please enter a user ID.'); return; }
            var listVal = $('#listId').val();
            $.post('@Url.Action("AssignUserRole", "Security")', {
                userId: uid,
                roleId: $('#roleId').val(),
                listId: listVal === '' ? null : listVal
            }, function (res) {
                if (res.success) { $('#assignMsg').html('<div class="alert alert-success">Role assigned successfully.</div>'); }
                else { $('#assignMsg').html('<div class="alert alert-danger">Failed to assign role.</div>'); }
            });
        });
    </script>
End Section
