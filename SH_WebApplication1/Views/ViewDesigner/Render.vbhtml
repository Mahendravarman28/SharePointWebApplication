@Code
    ViewData("Title") = "List View"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim view As SH_WebApplication1.Models.Metadata.AppView = ViewBag.View
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
    Dim items As List(Of SH_WebApplication1.Models.Metadata.AppItem) = ViewBag.Items
    Dim totalCount As Integer = ViewBag.TotalCount
End Code

<div class="container-fluid mt-4">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h2><i class="bi bi-table"></i> @list.ListName
            <span class="badge bg-info text-dark ms-2">@view.ViewType</span>
        </h2>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New Item", "Create", "Items", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("Views", "Index", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    <!-- Quick search -->
    <div class="mb-3">
        <input type="text" id="quickSearch" class="form-control" placeholder="Search items..." style="max-width:350px">
    </div>

    <!-- Grid View -->
    <div class="card shadow-sm">
        <div class="card-body p-0">
            <table class="table table-hover table-striped mb-0" id="itemsTable">
                <thead class="table-dark">
                    <tr>
                        <th>#</th>
                        <th>Item No</th>
                        @For Each f In fields
                            @<th>@f.DisplayName</th>
                        Next
                        <th>Status</th>
                        <th>Created</th>
                        <th>Actions</th>
                    </tr>
                </thead>
                <tbody>
                    @If items IsNot Nothing AndAlso items.Count > 0 Then
                        Dim rowNum As Integer = 0
                        For Each item In items
                            rowNum += 1
                            @<tr>
                                <td class="text-muted small">@rowNum</td>
                                <td><code>@item.ItemNo</code></td>
                                @For Each f In fields
                                    Dim capturedF = f
                                    Dim val = item.Values?.FirstOrDefault(Function(v) v.FieldId = capturedF.FieldId)
                                    @<td>@(If(val IsNot Nothing, val.FieldValueText, ""))</td>
                                Next
                                <td><span class="badge bg-secondary">@item.CurrentStatus</span></td>
                                <td class="small">@item.CreatedDate.ToString("dd-MMM-yyyy")</td>
                                <td>
                                    @Html.ActionLink("View", "Display", "Items", New With {.id = item.ItemId}, New With {.class = "btn btn-sm btn-outline-info"})
                                    @Html.ActionLink("Edit", "Edit", "Items", New With {.id = item.ItemId}, New With {.class = "btn btn-sm btn-outline-secondary"})
                                </td>
                            </tr>
                        Next
                    Else
                        @<tr><td colspan="@(fields.Count + 5)" class="text-center text-muted py-4">No items found. Click "+ New Item" to create one.</td></tr>
                    End If
                </tbody>
            </table>
        </div>
        <div class="card-footer text-muted small">
            Total: @totalCount items
        </div>
    </div>
</div>

@section scripts
    <script>
        $('#quickSearch').on('keyup', function () {
            var q = $(this).val().toLowerCase();
            $('#itemsTable tbody tr').each(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(q) > -1);
            });
        });
    </script>
End Section
