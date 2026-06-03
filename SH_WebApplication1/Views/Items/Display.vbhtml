@ModelType SH_WebApplication1.Models.Metadata.AppItem
@Code
    ViewData("Title") = "View Item"

    Dim list As SH_WebApplication1.Models.Metadata.AppList = ViewBag.List
    Dim _fieldsData As List(Of SH_WebApplication1.Models.Metadata.AppListField) = ViewBag.Fields
    Dim transitions As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransition) = ViewBag.Transitions
    Dim history As List(Of SH_WebApplication1.Models.Metadata.AppWorkflowTransaction) = ViewBag.History
End Code

@Functions
    Private Function StatusColour(status As String) As String

        If String.IsNullOrEmpty(status) Then Return "secondary"

        Select Case status.ToLower()
            Case "draft"
                Return "secondary"
            Case "submitted"
                Return "info"
            Case "under review", "reviewed"
                Return "warning"
            Case "approved"
                Return "success"
            Case "rejected"
                Return "danger"
            Case "closed"
                Return "dark"
            Case Else
                Return "primary"
        End Select
    End Function

    Private Function GetFileIcon(fileName As String) As String
        Dim ext = IO.Path.GetExtension(fileName).ToLower()
        Select Case ext
            Case ".pdf"
                Return "bi-file-earmark-pdf text-danger"
            Case ".doc", ".docx"
                Return "bi-file-earmark-word text-primary"
            Case ".xls", ".xlsx"
                Return "bi-file-earmark-excel text-success"
            Case ".ppt", ".pptx"
                Return "bi-file-earmark-ppt text-warning"
            Case ".jpg", ".jpeg", ".png", ".gif", ".bmp"
                Return "bi-file-earmark-image text-info"
            Case ".txt"
                Return "bi-file-earmark-text text-secondary"
            Case ".zip", ".rar", ".7z"
                Return "bi-file-earmark-zip text-dark"
            Case Else
                Return "bi-file-earmark text-muted"
        End Select
    End Function

    Private Function CanViewInline(fileName As String) As Boolean
        Dim ext = IO.Path.GetExtension(fileName).ToLower()
        Return {".pdf", ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".txt"}.Contains(ext)
    End Function
End Functions

<div class="container mt-4" style="max-width:960px">

    <!-- Header -->
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h4><i class="bi bi-file-earmark-text-fill"></i> Item — <code>@Model.ItemNo</code></h4>
        <div class="d-flex gap-2">
            @Html.ActionLink("Edit", "Edit", New With {.id = Model.ItemId}, New With {.class = "btn btn-outline-secondary"})
            @Html.ActionLink("List", "Render", "ViewDesigner", New With {.listId = Model.ListId}, New With {.class = "btn btn-outline-dark"})
        </div>
    </div>

    <!-- Item Fields Card -->
    <div class="card shadow-sm mb-4">
        <div class="card-header d-flex justify-content-between align-items-center">
            <span class="fw-bold">@list.ListName</span>
            <span class="badge bg-@StatusColour(Model.CurrentStatus) fs-6">@Model.CurrentStatus</span>
        </div>
        <div class="card-body">
            <div class="row">
                @For Each ff In _fieldsData
                    @Code
                        Dim val = Model.Values.FirstOrDefault(Function(v) v.FieldId = ff.FieldId)
                        Dim displayVal = If(val IsNot Nothing, If(Not String.IsNullOrEmpty(val.FieldValueText), val.FieldValueText, If(val.FieldValueNumber.HasValue, val.FieldValueNumber.Value.ToString(), If(val.FieldValueDate.HasValue, val.FieldValueDate.Value.ToString("dd-MMM-yyyy"), ""))), "")
                    End Code
                    @<div class="col-md-6 mb-3">
                        <label class="form-label text-muted small fw-bold">@ff.DisplayName</label>
                        <div class="form-control-plaintext border-bottom pb-1">@(If(String.IsNullOrEmpty(displayVal), "-", displayVal))</div>
                    </div>
                        Next
            </div>
            <div class="mt-2 text-muted small">
                @Code
                    Dim modText = If(Model.ModifiedDate.HasValue, $" | Modified: {Model.ModifiedDate.Value.ToString("dd-MMM-yyyy HH:mm")}", "")
                End Code
                Created by @Model.CreatedBy on @Model.CreatedDate.ToString("dd-MMM-yyyy HH:mm")  @Html.Raw(modText)
            </div>
        </div>
    </div>

    <!-- Inline Workflow Actions -->
    @If transitions IsNot Nothing AndAlso transitions.Count > 0 Then
        @<div class="card shadow-sm border-warning mb-4">
            <div class="card-header bg-warning text-dark fw-bold">
                <i class="bi bi-lightning-charge-fill"></i> Workflow Actions
            </div>
            <div class="card-body">
                <p class="text-muted small mb-3">
                    Current State: <strong>@Model.CurrentStatus</strong> — choose an action below to advance the workflow.
                </p>
                <div class="row g-3">
                    @For Each t In transitions
                        @<div class="col-md-6">
                            @Using Html.BeginForm("ExecuteTransition", "WorkflowDesigner", FormMethod.Post, New With {.class = "card p-3 h-100 border"})
                                @Html.AntiForgeryToken()
                                @Html.Hidden("itemId", Model.ItemId)
                                @Html.Hidden("transitionId", t.TransitionId)
                                @<div class="d-flex align-items-center gap-2 mb-2">
                                    <i class="bi bi-arrow-right-circle-fill text-warning fs-5"></i>
                                    <strong>@t.ActionName</strong>
                                    <span class="text-muted">→</span>
                                    <span class="badge bg-primary">@t.ToState?.StateName</span>
                                </div>
                                @If Not String.IsNullOrEmpty(t.RoleRequired) Then
                                    @<div class="mb-2">
                                        <small class="text-muted">
                                            <i class="bi bi-shield-check"></i> Role: <span class="badge bg-info text-dark">@t.RoleRequired</span>
                                        </small>
                                    </div>
                                End If
                                @<div class="input-group input-group-sm">
                                    <input type="text" name="comments" class="form-control" placeholder="Add a comment (optional)">
                                    <button type="submit" class="btn btn-warning">Confirm</button>
                                </div>
                            End Using
                        </div>
                    Next
                </div>
            </div>
        </div>
    End If

    <!-- File Attachments -->
    <div class="card shadow-sm mb-4">
        <div class="card-header fw-bold"><i class="bi bi-paperclip"></i> Attachments</div>
        <div class="card-body">
            @If Model.Attachments IsNot Nothing AndAlso Model.Attachments.Count > 0 Then
                @<ul class="list-group mb-3">
                    @For Each att In Model.Attachments
                        @<li class="list-group-item d-flex justify-content-between align-items-center">
                            <div class="d-flex align-items-center gap-2">
                                <i class="bi @GetFileIcon(att.FileName) fs-4"></i>
                                <div>
                                    <div>
                                        @Html.ActionLink(att.FileName, "DownloadAttachment", New With {.id = att.AttachmentId}, New With {.class = "text-decoration-none fw-bold"})
                                    </div>
                                    <small class="text-muted">
                                        @(Math.Round(att.FileSizeBytes / 1024.0, 1)) KB
                                        | Uploaded by @att.UploadedBy on @att.UploadedDate.ToString("dd-MMM-yyyy")
                                    </small>
                                </div>
                            </div>
                            <div class="d-flex gap-2">
                                @If CanViewInline(att.FileName) Then
                                    @<button type="button" class="btn btn-sm btn-outline-primary" onclick="viewDocument(@att.AttachmentId, '@att.FileName')">
                                        <i class="bi bi-eye"></i> View
                                    </button>
                                Else
                                    @Html.ActionLink("View", "ViewAttachment", New With {.id = att.AttachmentId}, New With {.class = "btn btn-sm btn-outline-primary"})
                                End If
                                @Html.ActionLink("Download", "DownloadAttachment", New With {.id = att.AttachmentId}, New With {.class = "btn btn-sm btn-outline-secondary"})
                                @Using Html.BeginForm("DeleteAttachment", "Items", FormMethod.Post, New With {.style = "display:inline"})
                                    @Html.AntiForgeryToken()
                                    @Html.Hidden("id", att.AttachmentId)
                                    @Html.Hidden("itemId", Model.ItemId)
                                    @<button type="submit" class="btn btn-sm btn-outline-danger" onclick="return confirm('Delete this file?')">Delete</button>
                                End Using
                            </div>
                        </li>
                    Next
                </ul>
            Else
                @<p class="text-muted small">No attachments yet.</p>
            End If
            @Using Html.BeginForm("UploadAttachment", "Items", FormMethod.Post, New With {.enctype = "multipart/form-data"})
                @Html.AntiForgeryToken()
                @Html.Hidden("itemId", Model.ItemId)
                @<div class="input-group">
                    <input type="file" name="file" class="form-control" required>
                    <button type="submit" class="btn btn-primary">Upload</button>
                </div>
            End Using
        </div>
    </div>

    <!-- Workflow History -->
    @If history IsNot Nothing AndAlso history.Count > 0 Then
        @<div class="card shadow-sm">
            <div class="card-header fw-bold"><i class="bi bi-clock-history"></i> Workflow History</div>
            <table class="table table-sm table-hover mb-0">
                <thead class="table-light">
                    <tr><th>From</th><th>To</th><th>By</th><th>Date</th><th>Comments</th></tr>
                </thead>
                <tbody>
                    @For Each h In history
                        @<tr>
                            <td><span class="badge bg-secondary">@h.FromState</span></td>
                            <td><span class="badge bg-primary">@h.ToState</span></td>
                            <td class="small">@h.ActionBy</td>
                            <td class="small">@h.ActionDate.ToString("dd-MMM-yyyy HH:mm")</td>
                            <td class="small">@h.Comments</td>
                        </tr>
                    Next
                </tbody>
            </table>
        </div>
    End If

</div>

<!-- Document Viewer Modal -->
<div class="modal fade" id="documentViewerModal" tabindex="-1" aria-labelledby="documentViewerLabel" aria-hidden="true">
    <div class="modal-dialog modal-xl modal-dialog-centered" style="max-width:95%; height:90vh">
        <div class="modal-content" style="height:100%">
            <div class="modal-header">
                <h5 class="modal-title" id="documentViewerLabel">
                    <i class="bi bi-file-earmark-text"></i> <span id="docTitle">Document Viewer</span>
                </h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <div class="modal-body p-0" style="height:calc(100% - 120px)">
                <div id="docViewerLoading" class="text-center py-5">
                    <div class="spinner-border text-primary" role="status">
                        <span class="visually-hidden">Loading...</span>
                    </div>
                    <p class="mt-3 text-muted">Loading document...</p>
                </div>
                <iframe id="docViewerFrame" style="width:100%; height:100%; border:none; display:none"></iframe>
            </div>
            <div class="modal-footer">
                <a id="docDownloadBtn" href="#" class="btn btn-primary btn-sm">
                    <i class="bi bi-download"></i> Download
                </a>
                <button type="button" class="btn btn-secondary btn-sm" data-bs-dismiss="modal">Close</button>
            </div>
        </div>
    </div>
</div>

@section scripts
    <script>
        function viewDocument(attachmentId, fileName) {
            // Show modal
            var modal = new bootstrap.Modal(document.getElementById('documentViewerModal'));
            modal.show();

            // Update title and download link
            document.getElementById('docTitle').textContent = fileName;
            document.getElementById('docDownloadBtn').href = '@Url.Action("DownloadAttachment", "Items")?id=' + attachmentId;

            // Show loading
            document.getElementById('docViewerLoading').style.display = 'block';
            document.getElementById('docViewerFrame').style.display = 'none';

            // Load document in iframe
            var viewUrl = '@Url.Action("ViewAttachment", "Items")?id=' + attachmentId;
            var iframe = document.getElementById('docViewerFrame');

            iframe.onload = function() {
                document.getElementById('docViewerLoading').style.display = 'none';
                iframe.style.display = 'block';
            };

            iframe.src = viewUrl;
        }

        // Clean up when modal closes
        document.getElementById('documentViewerModal').addEventListener('hidden.bs.modal', function () {
            document.getElementById('docViewerFrame').src = '';
        });
    </script>
End Section
