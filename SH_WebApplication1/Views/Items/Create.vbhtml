@Code
    ViewData("Title") = "New Item"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container mt-4" style="max-width:900px">
    <h4><i class="bi bi-plus-circle-fill"></i> New Item — <span class="text-primary">@list.ListName</span></h4>
    <hr />
    @Using Html.BeginForm("Create", "Items", New With {.listId = list.ListId}, FormMethod.Post)
        @Html.AntiForgeryToken()
        @Html.Hidden("listId", list.ListId)
        @<div class="card shadow-sm">
            <div class="card-body">
                <div class="row" id="formFields">
                    @For Each f In fields
                        Dim req = If(f.IsRequired, "required", "")
                        Dim reqStar = If(f.IsRequired, " <span class='text-danger'>*</span>", "")
                        Dim ctrl As String
                        Dim fieldId = $"field_{f.FieldId}"
                        Select Case f.DataType.ToLower()
                            Case "multiline"
                                ctrl = $"<textarea class='form-control' id='{fieldId}' name='{fieldId}' rows='3' {req}></textarea>"
                            Case "yesno"
                                ctrl = $"<div class='form-check'><input class='form-check-input' id='{fieldId}' type='checkbox' name='{fieldId}' value='true'><label class='form-check-label'>Yes</label></div>"
                            Case "datetime"
                                ctrl = $"<input type='datetime-local' class='form-control' id='{fieldId}' name='{fieldId}' {req}>"
                            Case "number", "decimal", "currency", "autonumber"
                                ctrl = $"<input type='number' class='form-control' id='{fieldId}' name='{fieldId}' {req} step='any'>"
                            Case "dropdown", "status", "multiselect"
                                ctrl = $"<select class='form-select' id='{fieldId}' name='{fieldId}' data-config='{System.Web.HttpUtility.HtmlAttributeEncode(f.DropdownConfig)}'><option value=''>-- Select --</option></select>"
                            Case "lookup"
                                ctrl = $"<select class='form-select' id='{fieldId}' name='{fieldId}' data-lookup='{System.Web.HttpUtility.HtmlAttributeEncode(f.LookupConfig)}'><option value=''>-- Select --</option></select>"
                            Case Else
                                ctrl = $"<input type='text' class='form-control' id='{fieldId}' name='{fieldId}' {req} placeholder='{f.DisplayName}'>"
                        End Select
                        @<div class="col-md-6 mb-3 field-wrapper" data-field-id="@f.FieldId" data-internal-name="@f.InternalName">
                            <label class="form-label fw-bold">@f.DisplayName @Html.Raw(reqStar)</label>
                            @Html.Raw(ctrl)
                        </div>
                    Next
                </div>
                <div class="d-flex gap-2 mt-2">
                    <button type="submit" class="btn btn-primary">Save Item</button>
                    @Html.ActionLink("Cancel", "Render", "ViewDesigner", New With {.listId = list.ListId}, New With {.class = "btn btn-secondary"})
                </div>
            </div>
        </div>
    End Using
</div>

@section scripts
<script>
(function () {
    // Populate dropdown options from JSON config
    document.querySelectorAll('select[data-config]').forEach(function (sel) {
        try {
            var cfg = JSON.parse(sel.getAttribute('data-config') || '{}');
            if (cfg.options && Array.isArray(cfg.options)) {
                cfg.options.forEach(function (opt) {
                    var o = document.createElement('option');
                    o.value = opt;
                    o.text = opt;
                    sel.add(o);
                });
            }
        } catch (e) { console.warn('Dropdown config parse error', e); }
    });

    // Conditional field rules (example: show field B if field A = "Yes")
    // This is a placeholder for dynamic rule evaluation from ValidationRule or custom JSON
    // Future: read rules from field metadata and eval at runtime
    document.getElementById('formFields').addEventListener('change', function (e) {
        // Example rule: if any field changes, you can show/hide others
        // For now, this is a hook for custom implementations
    });
})();
</script>
End Section