@ModelType SH_WebApplication1.Models.Metadata.AppForm
@Code
    ViewData("Title") = "Form Designer"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container-fluid mt-3">
    <div class="d-flex justify-content-between align-items-center mb-2">
        <h4><i class="bi bi-ui-checks-grid"></i> Form Designer — <span class="text-primary">@list.ListName</span>
            <span class="badge bg-info text-dark ms-2">@Model.FormType</span>
        </h4>
        <div class="d-flex gap-2">
            <button id="btnSave" class="btn btn-primary"><i class="bi bi-save"></i> Save</button>
            <button id="btnPublish" class="btn btn-success" @(If(Model.FormId = 0, "disabled", ""))><i class="bi bi-cloud-upload"></i> Publish</button>
            @Html.ActionLink("Preview", "Preview", New With {.formId = Model.FormId}, New With {.class = "btn btn-outline-info" & If(Model.FormId = 0, " disabled", "")})
            @Html.ActionLink("← Back", "Index", New With {.listId = list.ListId}, New With {.class = "btn btn-outline-secondary"})
        </div>
    </div>

    @Html.Hidden("hdnFormId", Model.FormId)
    @Html.Hidden("hdnListId", Model.ListId)
    @Html.Hidden("hdnFormType", Model.FormType)

    <div class="row g-0" style="height:calc(100vh - 140px)">
        <!-- Left panel: field palette -->
        <div class="col-md-2 border-end bg-light overflow-auto p-2" id="fieldPalette">
            <h6 class="fw-bold text-uppercase text-muted small mb-2">Available Fields</h6>
            @For Each f In fields
                @<div class="palette-field card card-body p-2 mb-1 shadow-sm draggable-field"
                      draggable="true"
                      data-field-id="@f.FieldId"
                      data-field-name="@f.InternalName"
                      data-field-label="@f.DisplayName"
                      data-field-type="@f.DataType"
                      style="cursor:grab;font-size:0.8rem">
                    <strong>@f.DisplayName</strong>
                    <span class="badge bg-secondary float-end" style="font-size:0.65rem">@f.DataType</span>
                </div>
            Next
        </div>

        <!-- Center: canvas -->
        <div class="col-md-8 overflow-auto p-3" id="formCanvas">
            <div id="sectionsContainer"></div>
            <button class="btn btn-outline-secondary btn-sm mt-2" id="btnAddSection">
                <i class="bi bi-plus-circle"></i> Add Section
            </button>
        </div>

        <!-- Right panel: properties -->
        <div class="col-md-2 border-start bg-light overflow-auto p-2" id="propertiesPanel">
            <h6 class="fw-bold text-uppercase text-muted small mb-2">Properties</h6>
            <div id="propContent">
                <p class="text-muted small">Select a field or section to edit properties.</p>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script>
        // ── Layout state ──────────────────────────────────────────────────
        var formLayout = @Html.Raw(If(String.IsNullOrEmpty(Model.LayoutJson), "{""sections"":[],""rules"":[]}", Model.LayoutJson));
        var selectedElement = null;

        // ── Render sections from state ────────────────────────────────────
        function renderCanvas() {
            var container = $('#sectionsContainer');
            container.empty();
            $.each(formLayout.sections, function (si, section) {
                var sectionHtml =
                    '<div class="form-section card mb-3 shadow-sm" data-section-id="' + section.sectionId + '">' +
                    '  <div class="card-header d-flex justify-content-between align-items-center section-header" style="cursor:pointer">' +
                    '    <span class="fw-bold"><i class="bi bi-layout-text-sidebar"></i> ' + section.title + '</span>' +
                    '    <div class="d-flex gap-1">' +
                    '      <select class="form-select form-select-sm col-select" style="width:90px">' +
                    '        <option value="1"' + (section.columns == 1 ? ' selected' : '') + '>1 Col</option>' +
                    '        <option value="2"' + (section.columns == 2 ? ' selected' : '') + '>2 Col</option>' +
                    '        <option value="3"' + (section.columns == 3 ? ' selected' : '') + '>3 Col</option>' +
                    '      </select>' +
                    '      <button class="btn btn-sm btn-outline-danger btn-del-section"><i class="bi bi-trash"></i></button>' +
                    '    </div>' +
                    '  </div>' +
                    '  <div class="card-body">' +
                    '    <div class="row section-fields drop-zone">';

                var colClass = 'col-md-' + Math.floor(12 / section.columns);
                $.each(section.fields, function (fi, field) {
                    sectionHtml +=
                        '<div class="' + colClass + ' mb-2 placed-field" data-field-id="' + field.fieldId + '" data-section-id="' + section.sectionId + '">' +
                        '  <div class="card card-body p-2 shadow-sm field-card" style="font-size:0.8rem;cursor:pointer">' +
                        '    <div class="d-flex justify-content-between">' +
                        '      <strong>' + field.label + '</strong>' +
                        '      <button class="btn btn-sm btn-link text-danger p-0 btn-remove-field"><i class="bi bi-x-circle"></i></button>' +
                        '    </div>' +
                        '    <span class="badge bg-secondary">' + field.dataType + '</span>' +
                        (field.required ? ' <span class="badge bg-danger">Required</span>' : '') +
                        (field.readOnly ? ' <span class="badge bg-light text-dark border">ReadOnly</span>' : '') +
                        '  </div>' +
                        '</div>';
                });

                sectionHtml += '  </div></div></div></div>';
                container.append(sectionHtml);
            });

            bindDropZones();
        }

        // ── Drag from palette ─────────────────────────────────────────────
        $(document).on('dragstart', '.draggable-field', function (e) {
            e.originalEvent.dataTransfer.setData('fieldId', $(this).data('field-id'));
            e.originalEvent.dataTransfer.setData('fieldName', $(this).data('field-name'));
            e.originalEvent.dataTransfer.setData('fieldLabel', $(this).data('field-label'));
            e.originalEvent.dataTransfer.setData('fieldType', $(this).data('field-type'));
        });

        function bindDropZones() {
            $('.drop-zone').on('dragover', function (e) { e.preventDefault(); $(this).addClass('bg-light'); });
            $('.drop-zone').on('dragleave', function () { $(this).removeClass('bg-light'); });
            $('.drop-zone').on('drop', function (e) {
                e.preventDefault();
                $(this).removeClass('bg-light');
                var fid = parseInt(e.originalEvent.dataTransfer.getData('fieldId'));
                var fname = e.originalEvent.dataTransfer.getData('fieldName');
                var flabel = e.originalEvent.dataTransfer.getData('fieldLabel');
                var ftype = e.originalEvent.dataTransfer.getData('fieldType');
                var sectionId = $(this).closest('.form-section').data('section-id');
                var section = formLayout.sections.find(function (s) { return s.sectionId == sectionId; });
                if (!section) return;
                var exists = section.fields.find(function (f) { return f.fieldId == fid; });
                if (exists) return;
                section.fields.push({ fieldId: fid, name: fname, label: flabel, dataType: ftype, required: false, readOnly: false, hidden: false, helpText: '' });
                renderCanvas();
            });
        }

        // ── Remove field ──────────────────────────────────────────────────
        $(document).on('click', '.btn-remove-field', function (e) {
            e.stopPropagation();
            var pf = $(this).closest('.placed-field');
            var fieldId = parseInt(pf.data('field-id'));
            var sectionId = pf.data('section-id');
            var section = formLayout.sections.find(function (s) { return s.sectionId == sectionId; });
            if (section) section.fields = section.fields.filter(function (f) { return f.fieldId != fieldId; });
            renderCanvas();
        });

        // ── Delete section ────────────────────────────────────────────────
        $(document).on('click', '.btn-del-section', function () {
            var sid = $(this).closest('.form-section').data('section-id');
            formLayout.sections = formLayout.sections.filter(function (s) { return s.sectionId != sid; });
            renderCanvas();
        });

        // ── Column change ─────────────────────────────────────────────────
        $(document).on('change', '.col-select', function () {
            var sid = $(this).closest('.form-section').data('section-id');
            var section = formLayout.sections.find(function (s) { return s.sectionId == sid; });
            if (section) section.columns = parseInt($(this).val());
            renderCanvas();
        });

        // ── Field properties ──────────────────────────────────────────────
        $(document).on('click', '.field-card', function () {
            var pf = $(this).closest('.placed-field');
            var fieldId = parseInt(pf.data('field-id'));
            var sectionId = pf.data('section-id');
            var section = formLayout.sections.find(function (s) { return s.sectionId == sectionId; });
            var field = section ? section.fields.find(function (f) { return f.fieldId == fieldId; }) : null;
            if (!field) return;
            selectedElement = { type: 'field', sectionId: sectionId, fieldId: fieldId };
            $('#propContent').html(
                '<label class="form-label fw-bold small">Label</label>' +
                '<input class="form-control form-control-sm mb-2" id="propLabel" value="' + field.label + '">' +
                '<label class="form-label fw-bold small">Help Text</label>' +
                '<input class="form-control form-control-sm mb-2" id="propHelp" value="' + (field.helpText || '') + '">' +
                '<div class="form-check mb-1"><input class="form-check-input" type="checkbox" id="propRequired"' + (field.required ? ' checked' : '') + '><label class="form-check-label small">Required</label></div>' +
                '<div class="form-check mb-1"><input class="form-check-input" type="checkbox" id="propReadOnly"' + (field.readOnly ? ' checked' : '') + '><label class="form-check-label small">Read Only</label></div>' +
                '<div class="form-check mb-2"><input class="form-check-input" type="checkbox" id="propHidden"' + (field.hidden ? ' checked' : '') + '><label class="form-check-label small">Hidden</label></div>' +
                '<button class="btn btn-sm btn-primary w-100" id="btnApplyProp">Apply</button>'
            );
        });

        $(document).on('click', '#btnApplyProp', function () {
            if (!selectedElement || selectedElement.type != 'field') return;
            var section = formLayout.sections.find(function (s) { return s.sectionId == selectedElement.sectionId; });
            var field = section ? section.fields.find(function (f) { return f.fieldId == selectedElement.fieldId; }) : null;
            if (!field) return;
            field.label = $('#propLabel').val();
            field.helpText = $('#propHelp').val();
            field.required = $('#propRequired').is(':checked');
            field.readOnly = $('#propReadOnly').is(':checked');
            field.hidden = $('#propHidden').is(':checked');
            renderCanvas();
        });

        // ── Add section ───────────────────────────────────────────────────
        $('#btnAddSection').click(function () {
            var sid = 'section' + (formLayout.sections.length + 1) + '_' + Date.now();
            formLayout.sections.push({ sectionId: sid, title: 'New Section', columns: 2, fields: [] });
            renderCanvas();
        });

        // ── Save ──────────────────────────────────────────────────────────
        $('#btnSave').click(function () {
            $.post('@Url.Action("Save", "FormDesigner")', {
                listId: $('#hdnListId').val(),
                formType: $('#hdnFormType').val(),
                formId: $('#hdnFormId').val(),
                formName: '@Model.FormName',
                layoutJson: JSON.stringify(formLayout)
            }, function (res) {
                if (res.success) {
                    $('#hdnFormId').val(res.formId);
                    $('#btnPublish').prop('disabled', false);
                    alert('Form saved successfully.');
                } else { alert('Error: ' + res.message); }
            });
        });

        // ── Publish ───────────────────────────────────────────────────────
        $('#btnPublish').click(function () {
            var fid = $('#hdnFormId').val();
            if (!fid) return;
            $.post('@Url.Action("Publish", "FormDesigner")', { formId: fid }, function (res) {
                if (res.success) alert('Form published!');
            });
        });

        // ── Init ──────────────────────────────────────────────────────────
        $(function () { renderCanvas(); });
    </script>
End Section
