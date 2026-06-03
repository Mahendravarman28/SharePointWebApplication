@Code
    ViewData("Title") = "Execute Workflow"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim item As SH_WebApplication1.Models.Metadata.AppItem = ViewBag.Item
    Dim transitions As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransition) = ViewBag.Transitions
    Dim history As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransaction) = ViewBag.History
End Code

<div class="container mt-4" style="max-width:900px">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-play-circle-fill"></i> Workflow — <code>@item.ItemNo</code></h4>
        @Html.ActionLink("← Back to Item", "Display", "Items", New With {.id = item.ItemId}, New With {.class = "btn btn-outline-secondary"})
    </div>

    <!-- Current Status -->
    <div class="card shadow-sm mb-4">
        <div class="card-body d-flex align-items-center gap-3">
            <i class="bi bi-circle-fill text-primary fs-3"></i>
            <div>
                <div class="fw-bold fs-5">Current Status: <span class="badge bg-secondary fs-6">@item.CurrentStatus</span></div>
                <div class="text-muted small">List: @list.ListName | Created: @item.CreatedDate.ToString("dd-MMM-yyyy")</div>
            </div>
        </div>
    </div>

    <!-- Available Actions -->
    @If transitions IsNot Nothing AndAlso transitions.Count > 0 Then
        @<div class="card shadow-sm mb-4">
            <div class="card-header fw-bold"><i class="bi bi-lightning-fill"></i> Available Actions</div>
            <div class="card-body">
                @For Each t In transitions
                    @Using Html.BeginForm("ExecuteTransition", "WorkflowDesigner", FormMethod.Post, New With {.class = "d-inline-block me-2 mb-2"})
                        @Html.AntiForgeryToken()
                        @Html.Hidden("itemId", item.ItemId)
                        @Html.Hidden("transitionId", t.TransitionId)
                        @<div class="input-group mb-2" style="max-width:500px">
                            <span class="input-group-text"><strong>@t.ActionName</strong> → @t.ToState?.StateName</span>
                            <input type="text" name="comments" class="form-control" placeholder="Comments (optional)">
                            <button type="submit" class="btn btn-primary">Execute</button>
                        </div>
                    End Using
                Next
            </div>
        </div>
    Else
        @<div class="alert alert-info"><i class="bi bi-info-circle"></i> No available transitions for the current state.</div>
    End If

    <!-- Workflow History -->
    <div class="card shadow-sm">
        <div class="card-header fw-bold"><i class="bi bi-clock-history"></i> Workflow History</div>
        <div class="table-responsive">
            <table class="table table-hover mb-0">
                <thead class="table-dark">
                    <tr><th>From</th><th>To</th><th>Action By</th><th>Date</th><th>Comments</th></tr>
                </thead>
                <tbody>
                    @If history IsNot Nothing AndAlso history.Count > 0 Then
                        For Each h In history
                            @<tr>
                                <td><span class="badge bg-secondary">@h.FromState</span></td>
                                <td><span class="badge bg-primary">@h.ToState</span></td>
                                <td>@h.ActionBy</td>
                                <td class="small">@h.ActionDate.ToString("dd-MMM-yyyy HH:mm")</td>
                                <td>@h.Comments</td>
                            </tr>
                        Next
                    Else
                        @<tr><td colspan="5" class="text-center text-muted py-3">No workflow history yet.</td></tr>
                    End If
                </tbody>
            </table>
        </div>
    </div>
</div>
