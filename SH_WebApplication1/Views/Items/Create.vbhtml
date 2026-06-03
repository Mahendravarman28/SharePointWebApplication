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
                <div class="row">
                    @For Each f In fields
                        Dim req = If(f.IsRequired, "required", "")
                        Dim reqStar = If(f.IsRequired, " <span class='text-danger'>*</span>", "")
                        Dim ctrl As String
                        Select Case f.DataType.ToLower()
                            Case "multiline"
                                ctrl = $"<textarea class='form-control' name='field_{f.FieldId}' rows='3' {req}></textarea>"
                            Case "yesno"
                                ctrl = $"<div class='form-check'><input class='form-check-input' type='checkbox' name='field_{f.FieldId}' value='true'><label class='form-check-label'>Yes</label></div>"
                            Case "datetime"
                                ctrl = $"<input type='datetime-local' class='form-control' name='field_{f.FieldId}' {req}>"
                            Case "number", "decimal", "currency", "autonumber"
                                ctrl = $"<input type='number' class='form-control' name='field_{f.FieldId}' {req} step='any'>"
                            Case "dropdown", "status"
                                ctrl = $"<select class='form-select' name='field_{f.FieldId}'><option value=''>-- Select --</option></select>"
                            Case Else
                                ctrl = $"<input type='text' class='form-control' name='field_{f.FieldId}' {req} placeholder='{f.DisplayName}'>"
                        End Select
                        @<div class="col-md-6 mb-3">
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
