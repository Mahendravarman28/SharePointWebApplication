@ModelType SH_WebApplication1.Models.Metadata.AppItem
@Code
    ViewData("Title") = "View Item"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim _fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
    Dim transitions As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransition) = ViewBag.Transitions
    Dim history As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransaction) = ViewBag.History
End Code

<div class="container mt-4" style="max-width:900px">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-file-earmark-text-fill"></i> Item — <code>@Model.ItemNo</code></h4>
        <div class="d-flex gap-2">
            @Html.ActionLink("Edit", "Edit", New With {.id = Model.ItemId}, New With {.class = "btn btn-outline-secondary"})
            @Html.ActionLink("Workflow", "Execute", "WorkflowDesigner", New With {.itemId = Model.ItemId}, New With {.class = "btn btn-outline-warning"})
            @Html.ActionLink("← Back", "Render", "ViewDesigner", New With {.listId = Model.ListId}, New With {.class = "btn btn-outline-dark"})
        </div>
    </div>

    <div class="card shadow-sm mb-4">
        <div class="card-header d-flex justify-content-between">
            <span class="fw-bold">@list.ListName</span>
            <span class="badge bg-secondary">@Model.CurrentStatus</span>
        </div>
        <div class="card-body">
            <div class="row">
                @If _fields IsNot Nothing AndAlso _fields.Count > 0 Then
                @For Each ff In _fields
                    Dim capturedF = ff
                    Dim val = Model.Values?.FirstOrDefault(Function(v) v.FieldId = capturedF.FieldId)
                    Dim displayVal = If(val IsNot Nothing, If(Not String.IsNullOrEmpty(val.FieldValueText), val.FieldValueText,
                                      If(val.FieldValueNumber.HasValue, val.FieldValueNumber.Value.ToString(),
                                      If(val.FieldValueDate.HasValue, val.FieldValueDate.Value.ToString("dd-MMM-yyyy"), ""))), "")
                    @<div class="col-md-6 mb-3">
                        <label class="form-label text-muted small fw-bold">@f.DisplayName</label>
                        <div class="form-control-plaintext border-bottom pb-1">@(If(String.IsNullOrEmpty(displayVal), "—", displayVal))</div>
                    </div>
                Next
                END IF
            </div>
            <div class="mt-2 text-muted small">
                @Code
                    Dim modText = If(Model.ModifiedDate.HasValue, $" | Modified: {Model.ModifiedDate.Value.ToString("dd-MMM-yyyy HH:mm")}", "")
                End Code
                Created by @Model.CreatedBy on @Model.CreatedDate.ToString("dd-MMM-yyyy HH:mm") @Html.Raw(modText)
            </div>
        </div>
    </div>

    @If history IsNot Nothing AndAlso history.Count > 0 Then
        @<div class="card shadow-sm">
            <div class="card-header fw-bold"><i class="bi bi-clock-history"></i> Workflow History</div>
            <table class="table table-sm mb-0">
                <thead class="table-light"><tr><th>From</th><th>To</th><th>By</th><th>Date</th><th>Comments</th></tr></thead>
                <tbody>
                    @For Each h In history
                        @<tr>
                            <td><span class="badge bg-secondary">@h.FromState</span></td>
                            <td><span class="badge bg-primary">@h.ToState</span></td>
                            <td class="small">@h.ActionBy</td>
                            <td class="small">@h.ActionDate.ToString("dd-MMM-yyyy HH:mm")</td>
                            <td class="small">@h.Comments</td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    End If
</div>
