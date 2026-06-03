@ModelType IEnumerable(Of SH_WebApplication1.Models.Metadata.AppList)
@Code
    ViewData("Title") = "All Lists"
End Code

@Functions
    Private Function FlagBadge(value As Boolean) As String
        If value Then Return "<span class='badge bg-success'>Yes</span>"
        Return "<span class='badge bg-secondary'>No</span>"
    End Function
End Functions

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-table"></i> List Builder</h2>
        @Html.ActionLink("+ Create New List", "Create", Nothing, New With {.class = "btn btn-primary"})
    </div>

    <div class="card shadow-sm">
        <div class="card-body p-0">
            <table class="table table-hover table-striped mb-0">
                <thead class="table-dark">
                    <tr>
                        <th>List Name</th>
                        <th>Code</th>
                        <th>Description</th>
                        <th>Attachments</th>
                        <th>Versioning</th>
                        <th>Approval</th>
                        <th>Created</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    @For Each item In Model
                        @<tr>
                            <td><strong>@item.ListName</strong></td>
                            <td><code>@item.ListCode</code></td>
                            <td>@item.Description</td>
                            <td>@Html.Raw(FlagBadge(item.EnableAttachments))</td>
                            <td>@Html.Raw(FlagBadge(item.EnableVersioning))</td>
                            <td>@Html.Raw(FlagBadge(item.EnableApprovalWorkflow))</td>
                            <td>@item.CreatedDate.ToString("dd-MMM-yyyy")</td>
                            <td>
                                @Html.ActionLink("Fields", "Fields", New With {.id = item.ListId}, New With {.class = "btn btn-sm btn-outline-primary"})
                                @Html.ActionLink("Edit", "Edit", New With {.id = item.ListId}, New With {.class = "btn btn-sm btn-outline-secondary"})
                                @Using Html.BeginForm("Delete", "ListBuilder", New With {.id = item.ListId}, FormMethod.Post, New With {.style = "display:inline"})
                                    @Html.AntiForgeryToken()
                                    @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete this list?')">Delete</button>
                                End Using
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>
</div>
