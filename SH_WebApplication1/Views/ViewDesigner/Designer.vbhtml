@ModelType SH_WebApplication1.Models.Metadata.AppView
@Code
    ViewData("Title") = "View Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container-fluid mt-3">
    <div class="d-flex justify-content-between align-items-center mb-2">
        <h4><i class="bi bi-grid-3x3-gap"></i> View Designer — <span class="text-primary">@list.ListName</span></h4>
        <div class="d-flex gap-2">
            <button id="btnSaveView" class="btn btn-primary"><i class="bi bi-save"></i> Save View</button>
            @Html.ActionLink("← Views", "Index", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    @Html.Hidden("hdnViewId", Model.ViewId)
    @Html.Hidden("hdnListId", Model.ListId)

    <div class="row">
        <!-- Settings Panel -->
        <div class="col-md-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header fw-bold">View Settings</div>
                <div class="card-body">
                    <div class="mb-3">
                        <label class="form-label fw-bold">View Name</label>
                        <input type="text" id="viewName" class="form-control" value="@Model.ViewName">
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">View Type</label>
                        <select id="viewType" class="form-select">
                            @For Each vt In New List(Of String) From {"Grid", "Card", "Kanban", "Grouped", "Calendar", "Summary"}
                                @<option value="@vt" @(If(Model.ViewType = vt, "selected", ""))>@vt</option>
                            Next
                        </select>
                    </div>
                    <div class="mb-3">
                        <label class="form-label fw-bold">Page Size</label>
                        <select id="pageSize" class="form-select">
                            @For Each ps In New List(Of Integer) From {10, 20, 50, 100}
                                @<option value="@ps">@ps</option>
                            Next
                        </select>
                    </div>
                    <div class="form-check form-switch mb-2">
                        <input class="form-check-input" type="checkbox" id="isPublic" @(If(Model.IsPublic, "checked", ""))>
                        <label class="form-check-label">Public View</label>
                    </div>
                </div>
            </div>

            <!-- Column selection -->
            <div class="card shadow-sm mb-3">
                <div class="card-header fw-bold">Available Fields</div>
                <div class="card-body p-2" id="availableFields" style="max-height:300px;overflow-y:auto">
                    @For Each f In fields
                        @<div class="form-check">
                            <input class="form-check-input field-check" type="checkbox"
                                   data-field-id="@f.FieldId"
                                   data-field-label="@f.DisplayName"
                                   data-sortable="@f.IsSortable.ToString().ToLower()"
                                   id="fc_@f.FieldId">
                            <label class="form-check-label small" for="fc_@f.FieldId">@f.DisplayName <span class="badge bg-secondary" style="font-size:0.6rem">@f.DataType</span></label>
                        </div>
                    Next
                </div>
                <div class="card-footer">
                    <button class="btn btn-sm btn-outline-primary w-100" id="btnAddCols">Add Selected Columns →</button>
                </div>
            </div>
        </div>

        <!-- Column ordering -->
        <div class="col-md-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header fw-bold">View Columns (drag to reorder)</div>
                <ul class="list-group list-group-flush" id="colList" style="min-height:200px"></ul>
                <div class="card-footer text-muted small">Drag to reorder columns</div>
            </div>
        </div>

        <!-- Filters & Sort -->
        <div class="col-md-4">
            <div class="card shadow-sm mb-3">
                <div class="card-header fw-bold">Default Sort</div>
                <div class="card-body">
                    <select id="sortField" class="form-select mb-2">
                        <option value="">-- None --</option>
                        @For Each f In fields.Where(Function(x) x.IsSortable)
                            @<option value="@f.FieldId">@f.DisplayName</option>
                        Next
                    </select>
                    <select id="sortDir" class="form-select">
                        <option value="asc">Ascending</option>
                        <option value="desc">Descending</option>
                    </select>
                </div>
            </div>
            <div class="card shadow-sm">
                <div class="card-header fw-bold">Group By</div>
                <div class="card-body">
                    <select id="groupByField" class="form-select">
                        <option value="">-- None --</option>
                        @For Each f In fields
                            @<option value="@f.FieldId">@f.DisplayName</option>
                        Next
                    </select>
                </div>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script>
        var viewConfig = @Html.Raw(If(String.IsNullOrEmpty(Model.ViewConfigJson), "{""columns"":[],""defaultSort"":{""fieldId"":0,""direction"":""asc""},""filters"":[],""groupBy"":"""",""pageSize"":20}", Model.ViewConfigJson));

        function renderColList() {
            var ul = $('#colList');
            ul.empty();
            $.each(viewConfig.columns, function (i, col) {
                ul.append(
                    '<li class="list-group-item d-flex justify-content-between align-items-center" data-field-id="' + col.fieldId + '">' +
                    '<span><i class="bi bi-grip-vertical text-muted me-1" style="cursor:grab"></i>' + col.label + '</span>' +
                    '<button class="btn btn-sm btn-link text-danger p-0 btn-remove-col"><i class="bi bi-x-circle"></i></button>' +
                    '</li>'
                );
            });
            // Make sortable using drag-drop
            makeDraggableList('#colList');
        }

        function makeDraggableList(selector) {
            var $ul = $(selector);
            var dragging = null;
            $ul.find('li').attr('draggable', 'true')
                .on('dragstart', function () { dragging = this; $(this).addClass('opacity-50'); })
                .on('dragend', function () { $(this).removeClass('opacity-50'); syncColOrder(); })
                .on('dragover', function (e) { e.preventDefault(); if (this !== dragging) { $(this).before($(dragging)); } });
        }

        function syncColOrder() {
            var ordered = [];
            $('#colList li').each(function () {
                var fid = parseInt($(this).data('field-id'));
                var existing = viewConfig.columns.find(function (c) { return c.fieldId == fid; });
                if (existing) ordered.push(existing);
            });
            viewConfig.columns = ordered;
        }

        $(document).on('click', '.btn-remove-col', function () {
            var fid = parseInt($(this).closest('li').data('field-id'));
            viewConfig.columns = viewConfig.columns.filter(function (c) { return c.fieldId != fid; });
            renderColList();
        });

        $('#btnAddCols').click(function () {
            $('.field-check:checked').each(function () {
                var fid = parseInt($(this).data('field-id'));
                var exists = viewConfig.columns.find(function (c) { return c.fieldId == fid; });
                if (!exists) {
                    viewConfig.columns.push({
                        fieldId: fid,
                        label: $(this).data('field-label'),
                        sortable: $(this).data('sortable') === 'true',
                        width: ''
                    });
                }
            });
            renderColList();
        });

        $('#btnSaveView').click(function () {
            syncColOrder();
            viewConfig.defaultSort = { fieldId: parseInt($('#sortField').val()) || 0, direction: $('#sortDir').val() };
            viewConfig.groupBy = $('#groupByField').val();
            viewConfig.pageSize = parseInt($('#pageSize').val());
            $.post('@Url.Action("Save", "ViewDesigner")', {
                listId: $('#hdnListId').val(),
                viewId: $('#hdnViewId').val(),
                viewName: $('#viewName').val(),
                viewType: $('#viewType').val(),
                viewConfigJson: JSON.stringify(viewConfig),
                isPublic: $('#isPublic').is(':checked')
            }, function (res) {
                if (res.success) {
                    $('#hdnViewId').val(res.viewId);
                    alert('View saved!');
                } else { alert('Error: ' + res.message); }
            });
        });

        $(function () { renderColList(); });
    </script>
End Section
