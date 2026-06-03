@Code
    ViewData("Title") = "Manage Fields"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

@Functions
    Private Function BoolIcon(value As Boolean) As String
        If value Then
            Return "<i class='bi bi-check-circle-fill text-success'></i>"
        End If
        Return "<i class='bi bi-dash text-muted'></i>"
    End Function
End Functions

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <h2><i class="bi bi-layout-text-window-reverse"></i> Fields — <span class="text-primary">@list.ListName</span></h2>
            <small class="text-muted">Code: <code>@list.ListCode</code></small>
        </div>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ Add Field", "AddField", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("← Back to Lists", "Index", Nothing, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    <div class="card shadow-sm">
        <div class="card-body p-0">
            <table class="table table-hover mb-0" id="fieldsTable">
                <thead class="table-dark">
                    <tr>
                        <th style="width:40px">#</th>
                        <th>Display Name</th>
                        <th>Internal Name</th>
                        <th>Data Type</th>
                        <th>Required</th>
                        <th>In List</th>
                        <th>In Form</th>
                        <th>Searchable</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    @For Each f In fields
                        @<tr data-field-id="@f.FieldId">
                            <td class="text-muted small">@f.DisplayOrder</td>
                            <td><strong>@f.DisplayName</strong></td>
                            <td><code>@f.InternalName</code></td>
                            <td><span class="badge bg-info text-dark">@f.DataType</span></td>
                            <td>@Html.Raw(BoolIcon(f.IsRequired))</td>
                            <td>@Html.Raw(BoolIcon(f.IsVisibleInList))</td>
                            <td>@Html.Raw(BoolIcon(f.IsVisibleInForm))</td>
                            <td>@Html.Raw(BoolIcon(f.IsSearchable))</td>
                            <td>
                                @Html.ActionLink("Edit", "EditField", New With {.id = f.FieldId}, New With {.class = "btn btn-sm btn-outline-secondary"})
                                @Using Html.BeginForm("DeleteField", "ListBuilder", FormMethod.Post, New With {.style = "display:inline"})
                                    @Html.AntiForgeryToken()
                                    @Html.Hidden("id", f.FieldId)
                                    @Html.Hidden("listId", list.ListId)
                                    @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete field?')">Delete</button>
                                End Using
                            </td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    </div>

    <div class="mt-3 d-flex gap-2">
        @Html.ActionLink("Design Form", "Designer", "FormDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-primary"})
        @Html.ActionLink("Design View", "Designer", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-info"})
        @Html.ActionLink("Home Page", "Designer", "PageDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-success"})
        @Html.ActionLink("Workflow", "Designer", "WorkflowDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-warning"})
    </div>
</div>
