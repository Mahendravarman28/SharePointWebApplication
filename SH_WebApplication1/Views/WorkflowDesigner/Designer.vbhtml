@ModelType SH_WebApplication1.Models.Metadata.AppWorkflow
@Code
    ViewData("Title") = "Workflow Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim roles As List(Of SH_WebApplication1.Models.Metadata.AppRole) = ViewBag.Roles
End Code

<div class="container-fluid mt-3">
    <div class="d-flex justify-content-between align-items-center mb-2">
        <h4><i class="bi bi-diagram-3-fill"></i> Workflow Designer — <span class="text-primary">@list.ListName</span></h4>
        <div class="d-flex gap-2">
            <button id="btnSaveWf" class="btn btn-primary"><i class="bi bi-save"></i> Save</button>
            <button id="btnPublishWf" class="btn btn-success" @(If(Model.WorkflowId = 0, "disabled", ""))><i class="bi bi-cloud-upload"></i> Publish</button>
            @Html.ActionLink("← Back", "Index", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    @Html.Hidden("hdnWfId", Model.WorkflowId)
    @Html.Hidden("hdnListId", Model.ListId)
    <input type="text" id="wfName" class="form-control mb-3" value="@Model.WorkflowName" placeholder="Workflow Name" style="max-width:400px">

    <div class="row g-0" style="min-height:500px">
        <!-- Toolbox -->
        <div class="col-md-2 border-end bg-light p-2">
            <h6 class="fw-bold text-uppercase text-muted small mb-2">Add State</h6>
            <div class="mb-2">
                <input type="text" id="newStateName" class="form-control form-control-sm mb-1" placeholder="State name">
                <input type="text" id="newStateCode" class="form-control form-control-sm mb-1" placeholder="State code (no spaces)">
                <select id="newStateColor" class="form-select form-select-sm mb-1">
                    <option value="secondary">Grey (Default)</option>
                    <option value="warning">Yellow (In Progress)</option>
                    <option value="info">Blue (Review)</option>
                    <option value="success">Green (Approved)</option>
                    <option value="danger">Red (Rejected)</option>
                    <option value="dark">Black (Closed)</option>
                </select>
                <div class="d-flex gap-1 mb-1">
                    <div class="form-check"><input class="form-check-input" type="checkbox" id="chkIsStart"><label class="form-check-label small">Start</label></div>
                    <div class="form-check"><input class="form-check-input" type="checkbox" id="chkIsEnd"><label class="form-check-label small">End</label></div>
                </div>
                <button class="btn btn-sm btn-primary w-100" id="btnAddState"><i class="bi bi-plus-circle"></i> Add State</button>
            </div>
            <hr>
            <h6 class="fw-bold text-uppercase text-muted small mb-2">Add Transition</h6>
            <div>
                <select id="tranFrom" class="form-select form-select-sm mb-1"></select>
                <select id="tranTo" class="form-select form-select-sm mb-1"></select>
                <input type="text" id="tranAction" class="form-control form-control-sm mb-1" placeholder="Action name (e.g. Approve)">
                <select id="tranRole" class="form-select form-select-sm mb-1">
                    <option value="">— No role required —</option>
                    @For Each role In roles
                        @<option value="@role.RoleName">@role.RoleName</option>
                    Next
                </select>
                <button class="btn btn-sm btn-outline-primary w-100" id="btnAddTransition"><i class="bi bi-arrow-right-circle"></i> Add Transition</button>
            </div>
        </div>

        <!-- Canvas -->
        <div class="col-md-10 p-3">
            <!-- State machine visual -->
            <div id="stateMachineCanvas" class="d-flex flex-wrap gap-3 mb-4 p-3 bg-white border rounded" style="min-height:180px">
                <p class="text-muted" id="canvasHint">Add states to begin designing the workflow.</p>
            </div>
            <!-- Transitions table -->
            <h6 class="fw-bold">Transitions</h6>
            <table class="table table-sm table-hover" id="transitionsTable">
                <thead class="table-dark"><tr><th>From</th><th>Action</th><th>To</th><th>Role</th><th>Condition</th><th></th></tr></thead>
                <tbody id="transitionBody"></tbody>
            </table>
        </div>
    </div>
</div>

@section scripts
    <script>
        var wfData = { states: [], transitions: [] };
        @If Model.WorkflowId > 0 AndAlso Not String.IsNullOrEmpty(Model.WorkflowJson) Then
            @Html.Raw("try { wfData = " & Model.WorkflowJson & "; } catch(e) {}")
        End If

        function renderAll() {
            renderStates(); renderTransitions(); refreshStateSelects();
        }

        function renderStates() {
            var canvas = $('#stateMachineCanvas');
            canvas.empty();
            if (wfData.states.length === 0) { canvas.append('<p class="text-muted" id="canvasHint">Add states to begin designing the workflow.</p>'); return; }
            $.each(wfData.states, function (i, s) {
                canvas.append(
                    '<div class="state-node position-relative" data-code="' + s.code + '">' +
                    '<div class="badge bg-' + (s.color || 'secondary') + ' p-3 fs-6" style="min-width:110px;text-align:center">' +
                    (s.isStart ? '<i class="bi bi-play-circle me-1"></i>' : '') +
                    s.name +
                    (s.isEnd ? '<i class="bi bi-stop-circle ms-1"></i>' : '') +
                    '</div>' +
                    '<button class="btn btn-link btn-sm text-danger p-0 btn-del-state position-absolute top-0 end-0" style="font-size:0.7rem" title="Remove"><i class="bi bi-x-circle-fill"></i></button>' +
                    '</div>'
                );
            });
        }

        function renderTransitions() {
            var tbody = $('#transitionBody');
            tbody.empty();
            $.each(wfData.transitions, function (i, t) {
                tbody.append(
                    '<tr data-idx="' + i + '">' +
                    '<td><span class="badge bg-secondary">' + t.fromCode + '</span></td>' +
                    '<td><strong>' + t.actionName + '</strong></td>' +
                    '<td><span class="badge bg-primary">' + t.toCode + '</span></td>' +
                    '<td><small>' + (t.role || '—') + '</small></td>' +
                    '<td><small>' + (t.condition || '—') + '</small></td>' +
                    '<td><button class="btn btn-sm btn-link text-danger p-0 btn-del-trans"><i class="bi bi-trash"></i></button></td>' +
                    '</tr>'
                );
            });
        }

        function refreshStateSelects() {
            var opts = '<option value="">-- Select --</option>';
            $.each(wfData.states, function (i, s) { opts += '<option value="' + s.code + '">' + s.name + '</option>'; });
            $('#tranFrom,#tranTo').html(opts);
        }

        $('#btnAddState').click(function () {
            var name = $('#newStateName').val().trim();
            var code = $('#newStateCode').val().trim().replace(/\s+/g, '_').toUpperCase();
            if (!name || !code) { alert('Please enter state name and code.'); return; }
            if (wfData.states.find(function (s) { return s.code == code; })) { alert('State code already exists.'); return; }
            wfData.states.push({ name: name, code: code, color: $('#newStateColor').val(), isStart: $('#chkIsStart').is(':checked'), isEnd: $('#chkIsEnd').is(':checked') });
            $('#newStateName,#newStateCode').val('');
            $('#chkIsStart,#chkIsEnd').prop('checked', false);
            renderAll();
        });

        $(document).on('click', '.btn-del-state', function () {
            var code = $(this).closest('.state-node').data('code');
            wfData.states = wfData.states.filter(function (s) { return s.code != code; });
            wfData.transitions = wfData.transitions.filter(function (t) { return t.fromCode != code && t.toCode != code; });
            renderAll();
        });

        $('#btnAddTransition').click(function () {
            var from = $('#tranFrom').val(); var to = $('#tranTo').val(); var action = $('#tranAction').val().trim();
            var role = $('#tranRole').val();
            if (!from || !to || !action) { alert('Please fill From, To and Action.'); return; }
            if (from === to) { alert('From and To states cannot be the same.'); return; }
            wfData.transitions.push({ fromCode: from, toCode: to, actionName: action, role: role, condition: '' });
            $('#tranAction').val('');
            $('#tranRole').val('');
            renderTransitions();
        });

        $(document).on('click', '.btn-del-trans', function () {
            var idx = parseInt($(this).closest('tr').data('idx'));
            wfData.transitions.splice(idx, 1);
            renderTransitions();
        });

        $('#btnSaveWf').click(function () {
            $.post('@Url.Action("Save", "WorkflowDesigner")', {
                listId: $('#hdnListId').val(),
                workflowId: $('#hdnWfId').val(),
                workflowName: $('#wfName').val(),
                workflowJson: JSON.stringify(wfData)
            }, function (res) {
                if (res.success) { $('#hdnWfId').val(res.workflowId); $('#btnPublishWf').prop('disabled', false); alert('Workflow saved!'); }
                else { alert('Error: ' + res.message); }
            });
        });

        $('#btnPublishWf').click(function () {
            var wid = $('#hdnWfId').val();
            if (!wid) return;
            $.post('@Url.Action("Publish", "WorkflowDesigner")', { workflowId: wid }, function (res) {
                if (res.success) alert('Workflow published!');
            });
        });

        $(function () { renderAll(); });
    </script>
End Section
