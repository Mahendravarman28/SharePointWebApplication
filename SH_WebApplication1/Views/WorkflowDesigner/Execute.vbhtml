@Code
    ViewData("Title") = "Workflow Execution"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim item As SH_WebApplication1.Models.Metadata.AppItem = ViewBag.Item
    Dim wf As SH_WebApplication1.Models.Metadata.AppWorkflow = ViewBag.Workflow
    Dim allStates As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowState) = ViewBag.AllStates
    Dim currentState As SH_WebApplication1.Models.Metadata.AppWorkflowState = ViewBag.CurrentState
    Dim transitions As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransition) = ViewBag.Transitions
    Dim history As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransaction) = ViewBag.History
End Code

@Functions
    Private Function StateColour(colorCode As String) As String
        If Not String.IsNullOrEmpty(colorCode) Then Return colorCode
        Return "secondary"
    End Function
End Functions

<div class="container mt-4" style="max-width:960px">

    <!-- Header -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <div>
            <h4><i class="bi bi-diagram-3-fill text-warning"></i>
                @If wf IsNot Nothing Then
                    @wf.WorkflowName
                Else
                    @Html.Raw("Workflow")
                End If
                — <code>@item.ItemNo</code>
            </h4>
            <small class="text-muted">List: @list.ListName</small>
        </div>
        <div class="d-flex gap-2">
            @Html.ActionLink("← Back to Item", "Display", "Items", New With {.id = item.ItemId}, New With {.class = "btn btn-outline-secondary"})
            @Html.ActionLink("List View", "Render", "ViewDesigner", New With {.listId = item.ListId}, New With {.class = "btn btn-outline-dark"})
        </div>
    </div>

    <!-- State Progress Timeline -->
    @If allStates IsNot Nothing AndAlso allStates.Count > 0 Then
        @<div class="card shadow-sm mb-4">
            <div class="card-header fw-bold"><i class="bi bi-map"></i> Workflow Progress</div>
            <div class="card-body">
                <div class="d-flex flex-wrap align-items-center gap-1">
                    @For Each st In allStates.OrderBy(Function(s) s.DisplayOrder)
                        Dim isActive = (currentState IsNot Nothing AndAlso currentState.StateId = st.StateId)
                        Dim isPast = (history IsNot Nothing AndAlso history.Any(Function(h) h.ToState = st.StateCode))
                        Dim dotColour = StateColour(st.ColorCode)
                        @If isActive Then
                            @<span class="badge bg-@dotColour border border-3 border-dark px-3 py-2 fs-6">
                                <i class="bi bi-circle-fill me-1"></i>@st.StateName
                            </span>
                        ElseIf isPast Then
                            @<span class="badge bg-@dotColour opacity-75 px-3 py-2">
                                <i class="bi bi-check-circle me-1"></i>@st.StateName
                            </span>
                        Else
                            @<span class="badge bg-light text-dark border px-3 py-2">@st.StateName</span>
                        End If
                        @If Not st.IsEnd Then
                            @<i class="bi bi-arrow-right text-muted"></i>
                        End If
                    Next
                </div>
            </div>
        </div>
    End If

    <!-- Current Status Banner -->
    <div class="card shadow-sm mb-4">
        <div class="card-body d-flex align-items-center gap-3">
            <i class="bi bi-circle-fill text-warning fs-2"></i>
            <div>
                <div class="fw-bold fs-5">
                    Current Status:
                    @If currentState IsNot Nothing Then
                        @<span class="badge bg-@StateColour(currentState.ColorCode) fs-6">@item.CurrentStatus</span>
                    Else
                        @<span class="badge bg-secondary fs-6">@(If(String.IsNullOrEmpty(item.CurrentStatus), "Not Started", item.CurrentStatus))</span>
                    End If
                </div>
                <div class="text-muted small">Created: @item.CreatedDate.ToString("dd-MMM-yyyy") | By: @item.CreatedBy</div>
            </div>
        </div>
    </div>

    <!-- Available Actions -->
    @If transitions IsNot Nothing AndAlso transitions.Count > 0 Then
        @<div class="card shadow-sm mb-4">
            <div class="card-header bg-warning text-dark fw-bold">
                <i class="bi bi-lightning-charge-fill"></i> Available Actions
            </div>
            <div class="card-body">
                <div class="row g-3">
                    @For Each t In transitions
                        @<div class="col-md-6">
                            @Using Html.BeginForm("ExecuteTransition", "WorkflowDesigner", FormMethod.Post, New With {.class = "card p-3 h-100 border shadow-sm"})
                                @Html.AntiForgeryToken()
                                @Html.Hidden("itemId", item.ItemId)
                                @Html.Hidden("transitionId", t.TransitionId)
                                @<div class="d-flex align-items-center gap-2 mb-2">
                                    <i class="bi bi-arrow-right-circle-fill text-warning fs-5"></i>
                                    <strong>@t.ActionName</strong>
                                    <span class="text-muted small">→</span>
                                    <span class="badge bg-primary">@t.ToState?.StateName</span>
                                </div>
                                @If Not String.IsNullOrEmpty(t.RoleRequired) Then
                                    @<div class="mb-2"><small class="text-muted"><i class="bi bi-shield-lock"></i> Required role: <code>@t.RoleRequired</code></small></div>
                                End If
                                @<div class="input-group input-group-sm">
                                    <input type="text" name="comments" class="form-control" placeholder="Comment (optional)">
                                    <button type="submit" class="btn btn-warning">Execute</button>
                                </div>
                            End Using
                        </div>
                    Next
                </div>
            </div>
        </div>
    Else
        @<div class="alert alert-info mb-4">
            <i class="bi bi-info-circle"></i>
            @If currentState IsNot Nothing AndAlso currentState.IsEnd Then
                @Html.Raw("<strong>This item has reached a terminal state.</strong> No further workflow actions are available.")
            Else
                @Html.Raw("No workflow actions are available for the current state. Ensure a published workflow is configured for this list.")
            End If
        </div>
    End If

    <!-- Workflow History -->
    <div class="card shadow-sm">
        <div class="card-header fw-bold"><i class="bi bi-clock-history"></i> Workflow History</div>
        <div class="table-responsive">
            <table class="table table-hover mb-0">
                <thead class="table-dark">
                    <tr>
                        <th>#</th>
                        <th>From State</th>
                        <th>To State</th>
                        <th>Action By</th>
                        <th>Date &amp; Time</th>
                        <th>Comments</th>
                    </tr>
                </thead>
                <tbody>
                    @If history IsNot Nothing AndAlso history.Count > 0 Then
                        Dim seq As Integer = history.Count
                        For Each h In history
                            @<tr>
                                <td class="text-muted small">@seq</td>
                                <td><span class="badge bg-secondary">@h.FromState</span></td>
                                <td><span class="badge bg-primary">@h.ToState</span></td>
                                <td class="small">@h.ActionBy</td>
                                <td class="small">@h.ActionDate.ToString("dd-MMM-yyyy HH:mm")</td>
                                <td class="small">@(If(String.IsNullOrEmpty(h.Comments), "-", h.Comments))</td>
                            </tr>
                            seq -= 1
                        Next
                    Else
                        @<tr>
                            <td colspan="6" class="text-center text-muted py-4">
                                <i class="bi bi-hourglass"></i> No workflow history yet.
                            </td>
                        </tr>
                    End If
                </tbody>
            </table>
        </div>
    </div>

</div>
