@Code
    ViewData("Title") = "Edit Item"
    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim item As SH_WebApplication1.Models.Metadata.AppItem = ViewBag.Item
    Dim fields As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
End Code

<div class="container mt-4" style="max-width:900px">
    <h4><i class="bi bi-pencil-fill"></i> Edit — <code>@item.ItemNo</code> — <span class="text-primary">@list.ListName</span></h4>
    <hr />
    @Using Html.BeginForm("Edit", "Items", New With {.id = item.ItemId}, FormMethod.Post)
        @Html.AntiForgeryToken()
        @<div class="card shadow-sm">
            <div class="card-body">
                <div class="row" id="formFields">
                    @For Each f In fields
                        Dim capturedF = f
                        Dim val = item.Values?.FirstOrDefault(Function(v) v.FieldId = capturedF.FieldId)
                        Dim currentVal = If(val IsNot Nothing AndAlso Not String.IsNullOrEmpty(val.FieldValueText), val.FieldValueText, "")
                        Dim reqStar = If(f.IsRequired, " <span class='text-danger'>*</span>", "")
                        Dim fieldId = $"field_{f.FieldId}"
                        Dim ctrl As String
                        Select Case f.DataType.ToLower()
                            Case "multiline"
                                ctrl = $"<textarea class='form-control' id='{fieldId}' name='{fieldId}' rows='3'>{currentVal}</textarea>"
                            Case "yesno"
                                Dim chk = If(currentVal = "true", "checked", "")
                                ctrl = $"<div class='form-check'><input class='form-check-input' id='{fieldId}' type='checkbox' name='{fieldId}' value='true' {chk}><label class='form-check-label'>Yes</label></div>"
                            Case "datetime"
                                ctrl = $"<input type='datetime-local' class='form-control' id='{fieldId}' name='{fieldId}' value='{currentVal}'>"
                            Case "number", "decimal", "currency"
                                ctrl = $"<input type='number' class='form-control' id='{fieldId}' name='{fieldId}' value='{currentVal}' step='any'>"
                            Case "dropdown", "status", "multiselect"
                                ctrl = $"<select class='form-select' id='{fieldId}' name='{fieldId}' data-config='{System.Web.HttpUtility.HtmlAttributeEncode(f.DropdownConfig)}' data-selected='{currentVal}'><option value=''>-- Select --</option></select>"
                            Case "lookup"
                                ctrl = $"<select class='form-select' id='{fieldId}' name='{fieldId}' data-lookup='{System.Web.HttpUtility.HtmlAttributeEncode(f.LookupConfig)}' data-selected='{currentVal}'><option value=''>-- Select --</option></select>"
                            Case Else
                                ctrl = $"<input type='text' class='form-control' id='{fieldId}' name='{fieldId}' value='{currentVal}'>"
                        End Select
                        @<div class="col-md-6 mb-3 field-wrapper" data-field-id="@f.FieldId" data-internal-name="@f.InternalName">
                            <label class="form-label fw-bold">@f.DisplayName @Html.Raw(reqStar)</label>
                            @Html.Raw(ctrl)
                        </div>
                    Next
                </div>
                <div class="d-flex gap-2 mt-2">
                    <button type="submit" class="btn btn-primary">Save Changes</button>
                    @Html.ActionLink("Cancel", "Display", New With {.id = item.ItemId}, New With {.class = "btn btn-secondary"})
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
            var selected = sel.getAttribute('data-selected');
            if (cfg.options && Array.isArray(cfg.options)) {
                cfg.options.forEach(function (opt) {
                    var o = document.createElement('option');
                    o.value = opt;
                    o.text = opt;
                    if (opt === selected) o.selected = true;
                    sel.add(o);
                });
            }
        } catch (e) { console.warn('Dropdown config parse error', e); }
    });

    // Conditional field rules hook
    document.getElementById('formFields').addEventListener('change', function (e) {
        // Placeholder for custom conditional field rule evaluation
    });
})();
</script>
End Section