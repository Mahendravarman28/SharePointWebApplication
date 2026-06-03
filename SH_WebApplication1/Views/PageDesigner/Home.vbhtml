@ModelType SH_WebApplication1.Models.Metadata.AppPage
@Code
    ViewData("Title") = "List Home"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim totalItems As Integer = ViewBag.TotalItems
    Dim recentItems As List(Of SH_WebApplication1.Models.Metadata.AppItem) = ViewBag.RecentItems
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container-fluid mt-4">
    <!-- Header -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <h2><i class="bi bi-house-door-fill text-primary"></i> @list.ListName</h2>
            <p class="text-muted mb-0">@list.Description</p>
        </div>
        <div class="d-flex gap-2">
            @Html.ActionLink("+ New Item", "Create", "Items", New With {.listId = list.ListId}, New With {.class = "btn btn-primary"})
            @Html.ActionLink("View All Items", "Render", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-info"})
            @Html.ActionLink("Design Page", "Designer", "PageDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    <!-- KPI Row -->
    <div class="row mb-4">
        <div class="col-md-3">
            <div class="card shadow-sm border-0 bg-primary text-white">
                <div class="card-body d-flex align-items-center gap-3">
                    <i class="bi bi-collection-fill fs-2"></i>
                    <div><div class="fs-4 fw-bold">@totalItems</div><div class="small">Total Items</div></div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card shadow-sm border-0 bg-warning text-dark">
                <div class="card-body d-flex align-items-center gap-3">
                    <i class="bi bi-pencil-square fs-2"></i>
                    <div><div class="fs-4 fw-bold" id="kpiDraft">—</div><div class="small">Draft</div></div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card shadow-sm border-0 bg-success text-white">
                <div class="card-body d-flex align-items-center gap-3">
                    <i class="bi bi-check-circle-fill fs-2"></i>
                    <div><div class="fs-4 fw-bold" id="kpiApproved">—</div><div class="small">Approved</div></div>
                </div>
            </div>
        </div>
        <div class="col-md-3">
            <div class="card shadow-sm border-0 bg-danger text-white">
                <div class="card-body d-flex align-items-center gap-3">
                    <i class="bi bi-hourglass-split fs-2"></i>
                    <div><div class="fs-4 fw-bold" id="kpiPending">—</div><div class="small">Pending</div></div>
                </div>
            </div>
        </div>
    </div>

    <!-- Recent Items + Quick Actions -->
    <div class="row">
        <div class="col-md-8">
            <div class="card shadow-sm mb-4">
                <div class="card-header fw-bold"><i class="bi bi-clock-history"></i> Recent Items</div>
                <div class="table-responsive">
                    <table class="table table-hover mb-0">
                        <thead class="table-light">
                            <tr>
                                <th>Item No</th>
                                @For Each f In fields
                                    @<th>@f.DisplayName</th>
                                Next
                                <th>Status</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            @If recentItems IsNot Nothing AndAlso recentItems.Count > 0 Then
                                For Each item In recentItems
                                    @<tr>
                                        <td><a href="@Url.Action("Display", "Items", New With {.id = item.ItemId})"><code>@item.ItemNo</code></a></td>
                                        @For Each f In fields
                                            Dim capturedF = f
                                            Dim val = item.Values?.FirstOrDefault(Function(v) v.FieldId = capturedF.FieldId)
                                            @<td>@(If(val IsNot Nothing, val.FieldValueText, ""))</td>
                                        Next
                                        <td><span class="badge bg-secondary">@item.CurrentStatus</span></td>
                                        <td class="small">@item.CreatedDate.ToString("dd-MMM-yyyy")</td>
                                    </tr>
                                Next
                            Else
                                @<tr><td colspan="@(fields.Count + 3)" class="text-center text-muted py-3">No items yet.</td></tr>
                            End If
                        </tbody>
                    </table>
                </div>
                <div class="card-footer">
                    @Html.ActionLink("View All →", "Render", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-sm btn-outline-primary"})
                </div>
            </div>
        </div>

        <!-- Quick Actions -->
        <div class="col-md-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header fw-bold"><i class="bi bi-lightning-fill"></i> Quick Actions</div>
                <div class="list-group list-group-flush">
                    @Html.ActionLink("➕ Create New Item", "Create", "Items", New With {.listId = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                    @Html.ActionLink("View All Items", "Render", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                    @Html.ActionLink("Manage Fields", "Fields", "ListBuilder", New With {.id = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                    @Html.ActionLink("Form Designer", "Index", "FormDesigner", New With {.listId = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                    @Html.ActionLink("View Designer", "Index", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                    @Html.ActionLink("⚙️ Workflow", "Index", "WorkflowDesigner", New With {.listId = list.ListId}, New With {.class = "list-group-item list-group-item-action"})
                </div>
            </div>
        </div>
    </div>
</div>
