@ModelType SH_WebApplication1.Models.Metadata.AppListField
@Code
    ViewData("Title") = "Add Field"
    Dim dataTypes As New List(Of String) From {
        "SingleLine", "MultiLine", "Number", "Decimal", "Currency",
        "DateTime", "YesNo", "Dropdown", "MultiSelect", "UserPicker",
        "Lookup", "FileAttachment", "Calculated", "AutoNumber", "Status"
    }
End Code

<div class="container mt-4" style="max-width:800px">
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
                            New With {.class = "form-select"})
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
