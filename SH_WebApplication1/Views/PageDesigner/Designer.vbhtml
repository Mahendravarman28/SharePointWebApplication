@ModelType SH_WebApplication1.Models.Metadata.AppPage
@Code
    ViewData("Title") = "Page Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
End Code

<div class="container-fluid mt-3">
    <div class="d-flex justify-content-between align-items-center mb-2">
        <h4><i class="bi bi-layout-wtf"></i> Home Page Designer — <span class="text-primary">@list.ListName</span></h4>
        <div class="d-flex gap-2">
            <button id="btnSavePage" class="btn btn-primary"><i class="bi bi-save"></i> Save</button>
            @Html.ActionLink("View Home", "Home", New With {.listId = list.ListId}, New With {.class = "btn btn-success"})
            @Html.ActionLink("← Back", "Index", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    @Html.Hidden("hdnPageId", Model.PageId)
    @Html.Hidden("hdnListId", Model.ListId)
    <input type="text" id="pageName" class="form-control mb-3" value="@Model.PageName" placeholder="Page Name" style="max-width:400px">

    <div class="row g-0" style="min-height:600px">
        <!-- Widget Palette -->
        <div class="col-md-2 border-end bg-light p-2">
            <h6 class="fw-bold text-uppercase text-muted small mb-2">Widgets</h6>
            @For Each wt In New List(Of Object) From {
                New With {.type = "KpiCount", .icon = "bi-bar-chart-fill", .label = "KPI Count"},
                New With {.type = "DataGrid", .icon = "bi-table", .label = "Data Grid"},
                New With {.type = "Chart", .icon = "bi-pie-chart-fill", .label = "Chart"},
                New With {.type = "RecentItems", .icon = "bi-clock-history", .label = "Recent Items"},
                New With {.type = "MyPendingItems", .icon = "bi-person-check", .label = "My Pending"},
                New With {.type = "Announcement", .icon = "bi-megaphone-fill", .label = "Announcement"},
                New With {.type = "ActionButtons", .icon = "bi-lightning-fill", .label = "Action Buttons"},
                New With {.type = "QuickLinks", .icon = "bi-link-45deg", .label = "Quick Links"}
            }
                @<div class="palette-widget card card-body p-2 mb-1 shadow-sm"
                      draggable="true"
                      data-widget-type="@wt.type"
                      data-widget-label="@wt.label"
                      style="cursor:grab;font-size:0.8rem">
                    <i class="bi @wt.icon me-1"></i> @wt.label
                </div>
            Next
        </div>

        <!-- Canvas -->
        <div class="col-md-10 p-3" id="pageCanvas">
            <div id="rowsContainer"></div>
            <button class="btn btn-outline-secondary btn-sm mt-2" id="btnAddRow">
                <i class="bi bi-plus-circle"></i> Add Row
            </button>
        </div>
    </div>
</div>

@section scripts
    <script>
        var pageLayout = @Html.Raw(If(String.IsNullOrEmpty(Model.LayoutJson), "{""rows"":[]}", Model.LayoutJson));

        function renderCanvas() {
            var container = $('#rowsContainer');
            container.empty();
            $.each(pageLayout.rows, function (ri, row) {
                var rowHtml = '<div class="page-row border rounded p-2 mb-3" data-row-id="' + row.rowId + '">' +
                    '<div class="d-flex justify-content-between align-items-center mb-2">' +
                    '<span class="fw-bold small text-muted">Row ' + (ri + 1) + '</span>' +
                    '<button class="btn btn-sm btn-outline-danger btn-del-row"><i class="bi bi-trash"></i> Remove Row</button>' +
                    '</div>' +
                    '<div class="row widget-drop-zone">';
                $.each(row.widgets, function (wi, widget) {
                    rowHtml += '<div class="col-md-' + widget.colWidth + ' mb-2 placed-widget" data-widget-id="' + widget.widgetId + '" data-row-id="' + row.rowId + '">' +
                        '<div class="card shadow-sm h-100">' +
                        '<div class="card-header d-flex justify-content-between align-items-center py-1">' +
                        '<span class="small fw-bold"><i class="bi bi-puzzle"></i> ' + widget.title + '</span>' +
                        '<div class="d-flex gap-1">' +
                        '<select class="form-select form-select-sm widget-col-sel" style="width:70px">' +
                        [2,3,4,6,8,12].map(function(c){ return '<option value="'+c+'"'+(widget.colWidth==c?' selected':'')+'>'+c+'</option>'; }).join('') +
                        '</select>' +
                        '<button class="btn btn-sm btn-link text-danger p-0 btn-remove-widget"><i class="bi bi-x-circle"></i></button>' +
                        '</div></div>' +
                        '<div class="card-body py-2 text-center text-muted small">' +
                        '<span class="badge bg-info text-dark">' + widget.type + '</span>' +
                        '</div></div></div>';
                });
                rowHtml += '</div></div>';
                container.append(rowHtml);
            });
            bindDropZones();
        }

        function findWidget(rowId, widgetId) {
            var row = pageLayout.rows.find(function (r) { return r.rowId == rowId; });
            return row ? row.widgets.find(function (w) { return w.widgetId == widgetId; }) : null;
        }

        function bindDropZones() {
            $('.widget-drop-zone')
                .on('dragover', function (e) { e.preventDefault(); $(this).addClass('bg-light'); })
                .on('dragleave', function () { $(this).removeClass('bg-light'); })
                .on('drop', function (e) {
                    e.preventDefault();
                    $(this).removeClass('bg-light');
                    var wtype = e.originalEvent.dataTransfer.getData('widgetType');
                    var wlabel = e.originalEvent.dataTransfer.getData('widgetLabel');
                    var rowId = $(this).closest('.page-row').data('row-id');
                    var row = pageLayout.rows.find(function (r) { return r.rowId == rowId; });
                    if (!row) return;
                    row.widgets.push({ widgetId: 'w_' + Date.now(), type: wtype, title: wlabel, colWidth: 4, config: {} });
                    renderCanvas();
                });
        }

        $(document).on('dragstart', '.palette-widget', function (e) {
            e.originalEvent.dataTransfer.setData('widgetType', $(this).data('widget-type'));
            e.originalEvent.dataTransfer.setData('widgetLabel', $(this).data('widget-label'));
        });

        $(document).on('click', '.btn-del-row', function () {
            var rid = $(this).closest('.page-row').data('row-id');
            pageLayout.rows = pageLayout.rows.filter(function (r) { return r.rowId != rid; });
            renderCanvas();
        });

        $(document).on('click', '.btn-remove-widget', function (e) {
            e.stopPropagation();
            var pw = $(this).closest('.placed-widget');
            var wid = pw.data('widget-id'); var rid = pw.data('row-id');
            var row = pageLayout.rows.find(function (r) { return r.rowId == rid; });
            if (row) row.widgets = row.widgets.filter(function (w) { return w.widgetId != wid; });
            renderCanvas();
        });

        $(document).on('change', '.widget-col-sel', function () {
            var pw = $(this).closest('.placed-widget');
            var wid = pw.data('widget-id'); var rid = pw.data('row-id');
            var w = findWidget(rid, wid);
            if (w) w.colWidth = parseInt($(this).val());
        });

        $('#btnAddRow').click(function () {
            pageLayout.rows.push({ rowId: 'row_' + Date.now(), widgets: [] });
            renderCanvas();
        });

        $('#btnSavePage').click(function () {
            $.post('@Url.Action("Save", "PageDesigner")', {
                listId: $('#hdnListId').val(),
                pageId: $('#hdnPageId').val(),
                pageName: $('#pageName').val(),
                layoutJson: JSON.stringify(pageLayout)
            }, function (res) {
                if (res.success) { $('#hdnPageId').val(res.pageId); alert('Page saved!'); }
                else { alert('Error: ' + res.message); }
            });
        });

        $(function () { renderCanvas(); });
    </script>
End Section
