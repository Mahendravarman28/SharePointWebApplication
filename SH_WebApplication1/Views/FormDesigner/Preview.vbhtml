@ModelType SH_WebApplication1.Models.Metadata.AppForm
@Code
    ViewData("Title") = "Form Preview"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container mt-4" style="max-width:900px">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-eye"></i> Preview — <span class="text-primary">@Model.FormName</span>
            <span class="badge bg-info text-dark ms-2">@Model.FormType</span>
        </h4>
        @Html.ActionLink("← Back to Designer", "Designer", New With {.listId = Model.ListId, .formId = Model.FormId}, New With {.class = "btn btn-outline-secondary"})
    </div>

    <div id="previewContainer" class="card shadow-sm">
        <div class="card-body">
            <p class="text-muted text-center" id="loadingMsg">Rendering form preview...</p>
        </div>
    </div>
</div>

@section scripts
    <script>
        var layout = @Html.Raw(If(String.IsNullOrEmpty(Model.LayoutJson), "{""sections"":[]}", Model.LayoutJson));
        var allFields = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(
            fields.Select(Function(f) New With {
                Key .fieldId = f.FieldId,
                Key .internalName = f.InternalName,
                Key .displayName = f.DisplayName,
                Key .dataType = f.DataType
            })
        ));

        function getFieldDef(fieldId) {
            return allFields.find(function (f) { return f.fieldId == fieldId; });
        }

        function renderFieldControl(field) {
            var dt = field.dataType ? field.dataType.toLowerCase() : 'singleline';
            switch (dt) {
                case 'multiline': return '<textarea class="form-control" rows="3" disabled placeholder="' + field.label + '"></textarea>';
                case 'yesno': return '<div class="form-check"><input class="form-check-input" type="checkbox" disabled><label class="form-check-label">Yes/No</label></div>';
                case 'datetime': return '<input type="datetime-local" class="form-control" disabled>';
                case 'number': case 'decimal': case 'currency': return '<input type="number" class="form-control" disabled>';
                case 'dropdown': case 'status': return '<select class="form-select" disabled><option>-- Select --</option></select>';
                default: return '<input type="text" class="form-control" disabled placeholder="' + field.label + '">';
            }
        }

        $(function () {
            var html = '<form>';
            $.each(layout.sections, function (si, section) {
                html += '<fieldset class="border rounded p-3 mb-3"><legend class="float-none w-auto px-2 fw-bold fs-6">' + section.title + '</legend>';
                html += '<div class="row">';
                var colClass = 'col-md-' + Math.floor(12 / (section.columns || 2));
                $.each(section.fields, function (fi, f) {
                    if (f.hidden) return;
                    html += '<div class="' + colClass + ' mb-3">';
                    html += '<label class="form-label">' + f.label + (f.required ? ' <span class="text-danger">*</span>' : '') + '</label>';
                    if (f.helpText) html += '<div class="form-text mb-1">' + f.helpText + '</div>';
                    html += renderFieldControl(f);
                    html += '</div>';
                });
                html += '</div></fieldset>';
            });
            html += '<div class="d-flex gap-2 mt-2">';
            html += '<button type="button" class="btn btn-primary">Submit</button>';
            html += '<button type="button" class="btn btn-secondary">Cancel</button>';
            html += '</div>';
            html += '</form>';
            $('#previewContainer .card-body').html(html);
        });
    </script>
End Section
