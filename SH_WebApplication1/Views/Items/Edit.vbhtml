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
                <div class="row">
                    @For Each f In fields
                        Dim capturedF = f
                        Dim val = item.Values?.FirstOrDefault(Function(v) v.FieldId = capturedF.FieldId)
                        Dim currentVal = If(val IsNot Nothing AndAlso Not String.IsNullOrEmpty(val.FieldValueText), val.FieldValueText, "")
                        Dim reqStar = If(f.IsRequired, " <span class='text-danger'>*</span>", "")
                        Dim ctrl As String
                        Select Case f.DataType.ToLower()
                            Case "multiline"
                                ctrl = $"<textarea class='form-control' name='field_{f.FieldId}' rows='3'>{currentVal}</textarea>"
                            Case "yesno"
                                Dim chk = If(currentVal = "true", "checked", "")
                                ctrl = $"<div class='form-check'><input class='form-check-input' type='checkbox' name='field_{f.FieldId}' value='true' {chk}><label class='form-check-label'>Yes</label></div>"
                            Case "datetime"
                                ctrl = $"<input type='datetime-local' class='form-control' name='field_{f.FieldId}' value='{currentVal}'>"
                            Case "number", "decimal", "currency"
                                ctrl = $"<input type='number' class='form-control' name='field_{f.FieldId}' value='{currentVal}' step='any'>"
                            Case Else
                                ctrl = $"<input type='text' class='form-control' name='field_{f.FieldId}' value='{currentVal}'>"
                        End Select
                        @<div class="col-md-6 mb-3">
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
