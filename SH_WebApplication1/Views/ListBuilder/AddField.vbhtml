@ModelType SH_WebApplication1.Models.Metadata.AppListField
@Code
    ViewData("Title") = "Add Field"
    Dim dataTypes As New List(Of String) From {
        "SingleLine", "MultiLine", "Number", "Decimal", "Currency",
        "DateTime", "YesNo", "Dropdown", "MultiSelect", "UserPicker",
        "Lookup", "FileAttachment", "Calculated", "AutoNumber", "Status"
    }
End Code

<div class="container mt-4" style="max-width:860px">
    <h2><i class="bi bi-plus-square"></i> Add Field</h2>
    <hr />
    @Using Html.BeginForm()
        @Html.AntiForgeryToken()
        @Html.HiddenFor(Function(m) m.ListId)
        @<div class="card shadow-sm">
            <div class="card-body">
                <div class="row mb-3">
                    <div class="col-md-6">
                        @Html.LabelFor(Function(m) m.DisplayName, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.DisplayName, New With {.class = "form-control", .placeholder = "e.g. Request Title"})
                        @Html.ValidationMessageFor(Function(m) m.DisplayName, "", New With {.class = "text-danger"})
                    </div>
                    <div class="col-md-6">
                        @Html.LabelFor(Function(m) m.InternalName, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.InternalName, New With {.class = "form-control", .placeholder = "e.g. RequestTitle"})
                        @Html.ValidationMessageFor(Function(m) m.InternalName, "", New With {.class = "text-danger"})
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-6">
                        @Html.LabelFor(Function(m) m.DataType, New With {.class = "form-label fw-bold"})
                        @Html.DropDownListFor(Function(m) m.DataType,
                            dataTypes.Select(Function(d) New SelectListItem With {.Text = d, .Value = d}),
                            New With {.class = "form-select", .id = "dataTypeSelect"})
                    </div>
                    <div class="col-md-6">
                        @Html.LabelFor(Function(m) m.DefaultValue, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.DefaultValue, New With {.class = "form-control"})
                    </div>
                </div>
                <div class="row mb-3">
                    <div class="col-md-6">
                        @Html.LabelFor(Function(m) m.ValidationRule, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.ValidationRule, New With {.class = "form-control", .placeholder = "e.g. Value > 0"})
                    </div>
                    <div class="col-md-3">
                        @Html.LabelFor(Function(m) m.MinLength, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.MinLength, New With {.class = "form-control", .type = "number"})
                    </div>
                    <div class="col-md-3">
                        @Html.LabelFor(Function(m) m.MaxLength, New With {.class = "form-label fw-bold"})
                        @Html.TextBoxFor(Function(m) m.MaxLength, New With {.class = "form-control", .type = "number"})
                    </div>
                </div>

                <!-- Dropdown Options Panel -->
                <div id="dropdownPanel" class="mb-3 d-none">
                    <div class="card border-info">
                        <div class="card-header text-info fw-bold"><i class="bi bi-list-ul"></i> Dropdown Options</div>
                        <div class="card-body">
                            <p class="text-muted small">Enter one option per line. These become the selectable values at runtime.</p>
                            <textarea id="dropdownOptions" class="form-control font-monospace" rows="5" placeholder="Option A&#10;Option B&#10;Option C"></textarea>
                            @Html.HiddenFor(Function(m) m.DropdownConfig, New With {.id = "DropdownConfig"})
                        </div>
                    </div>
                </div>

                <!-- Lookup Config Panel -->
                <div id="lookupPanel" class="mb-3 d-none">
                    <div class="card border-warning">
                        <div class="card-header text-warning fw-bold"><i class="bi bi-link-45deg"></i> Lookup Configuration</div>
                        <div class="card-body">
                            <div class="row g-2">
                                <div class="col-md-4">
                                    <label class="form-label small fw-bold">Source List Code</label>
                                    <input type="text" id="lookupListCode" class="form-control" placeholder="e.g. DEPARTMENTS">
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label small fw-bold">Value Field (internal name)</label>
                                    <input type="text" id="lookupValueField" class="form-control" placeholder="e.g. DeptId">
                                </div>
                                <div class="col-md-4">
                                    <label class="form-label small fw-bold">Display Field (internal name)</label>
                                    <input type="text" id="lookupDisplayField" class="form-control" placeholder="e.g. DeptName">
                                </div>
                            </div>
                            @Html.HiddenFor(Function(m) m.LookupConfig, New With {.id = "LookupConfig"})
                        </div>
                    </div>
                </div>

                <div class="row mb-3">
                    <div class="col-12">
                        <label class="form-label fw-bold">Options</label>
                        <div class="d-flex flex-wrap gap-3">
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsRequired, New With {.class = "form-check-input"}) <label class="form-check-label">Required</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsUnique, New With {.class = "form-check-input"}) <label class="form-check-label">Unique</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsSearchable, New With {.class = "form-check-input"}) <label class="form-check-label">Searchable</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsSortable, New With {.class = "form-check-input"}) <label class="form-check-label">Sortable</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsFilterable, New With {.class = "form-check-input"}) <label class="form-check-label">Filterable</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsVisibleInList, New With {.class = "form-check-input"}) <label class="form-check-label">Visible in List</label></div>
                            <div class="form-check">@Html.CheckBoxFor(Function(m) m.IsVisibleInForm, New With {.class = "form-check-input"}) <label class="form-check-label">Visible in Form</label></div>
                        </div>
                    </div>
                </div>
                <div class="d-flex gap-2">
                    <button type="submit" class="btn btn-primary">Add Field</button>
                    @Html.ActionLink("Back to Fields", "Fields", New With {.id = Model.ListId}, New With {.class = "btn btn-secondary"})
                </div>
            </div>
        </div>
    End Using
</div>

@section scripts
<script>
    (function () {
        var dtSel = document.getElementById('dataTypeSelect');
        function togglePanels() {
            var v = dtSel.value.toLowerCase();
            document.getElementById('dropdownPanel').classList.toggle('d-none', v !== 'dropdown' && v !== 'multiselectect' && v !== 'status' && v !== 'multiselect');
            document.getElementById('lookupPanel').classList.toggle('d-none', v !== 'lookup');
        }
        dtSel.addEventListener('change', togglePanels);
        togglePanels();

        // Serialize dropdown options to JSON before submit
        document.querySelector('form').addEventListener('submit', function () {
            var opts = document.getElementById('dropdownOptions').value;
            if (opts.trim()) {
                var arr = opts.split('\n').map(function (o) { return o.trim(); }).filter(function (o) { return o; });
                document.getElementById('DropdownConfig').value = JSON.stringify({ options: arr });
            }
            var lc = document.getElementById('lookupListCode').value.trim();
            var lv = document.getElementById('lookupValueField').value.trim();
            var ld = document.getElementById('lookupDisplayField').value.trim();
            if (lc) {
                document.getElementById('LookupConfig').value = JSON.stringify({ listCode: lc, valueField: lv, displayField: ld });
            }
        });

        // Pre-populate from existing values (EditField reuse)
        var existingDd = document.getElementById('DropdownConfig').value;
        if (existingDd) {
            try {
                var ddObj = JSON.parse(existingDd);
                if (ddObj.options) document.getElementById('dropdownOptions').value = ddObj.options.join('\n');
            } catch (e) {}
        }
        var existingLk = document.getElementById('LookupConfig').value;
        if (existingLk) {
            try {
                var lkObj = JSON.parse(existingLk);
                document.getElementById('lookupListCode').value = lkObj.listCode || '';
                document.getElementById('lookupValueField').value = lkObj.valueField || '';
                document.getElementById('lookupDisplayField').value = lkObj.displayField || '';
            } catch (e) {}
        }
    })();
</script>
End Section