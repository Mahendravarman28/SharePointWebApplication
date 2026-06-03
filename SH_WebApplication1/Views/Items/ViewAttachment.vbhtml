@ModelType SH_WebApplication1.Models.Metadata.AppAttachment
@Code
    ViewData("Title") = "View Document"
    Dim att As SH_WebApplication1.Models.Metadata.AppAttachment = ViewBag.Attachment
    Dim canViewInBrowser As Boolean = ViewBag.CanViewInBrowser
    Dim isOfficeDoc As Boolean = ViewBag.IsOfficeDoc
    Dim ext = IO.Path.GetExtension(att.FileName).ToLower()
End Code

<div class="container-fluid mt-4" style="height:90vh">
    <div class="d-flex justify-content-between align-items-center mb-3">
        <h5><i class="bi bi-file-earmark-text"></i> @att.FileName</h5>
        <div class="d-flex gap-2">
            @Html.ActionLink("Download", "DownloadAttachment", New With {.id = att.AttachmentId}, New With {.class = "btn btn-primary btn-sm"})
            <a href="javascript:history.back()" class="btn btn-outline-secondary btn-sm">← Back</a>
        </div>
    </div>

    @If canViewInBrowser Then
        @* PDF Viewer *@
        <div class="card shadow-sm h-100">
            <div class="card-body p-0">
                <iframe src="@Url.Action("ViewAttachment", New With {.id = att.AttachmentId})" 
                        style="width:100%; height:100%; border:none" 
                        title="Document Viewer"></iframe>
            </div>
        </div>
    ElseIf isOfficeDoc Then
        @* Office Document Viewer *@
        <div class="card shadow-sm">
            <div class="card-body text-center py-5">
                <i class="bi bi-file-earmark-word text-primary" style="font-size:4rem"></i>
                <h4 class="mt-3">Office Document</h4>
                <p class="text-muted">
                    This is a Microsoft Office document (@ext).
                </p>
                <div class="alert alert-info mt-3" style="max-width:600px; margin:0 auto">
                    <strong>Viewing Options:</strong>
                    <ul class="text-start mt-2 mb-0">
                        <li>Click "Download" above to open in Microsoft Office or compatible app</li>
                        <li>For web preview, the file must be hosted on a public URL (Office Online Viewer)</li>
                        <li>In production, integrate with SharePoint Online or Office 365 for web viewing</li>
                    </ul>
                </div>
                <div class="mt-4">
                    @Html.ActionLink("Download Document", "DownloadAttachment", New With {.id = att.AttachmentId}, New With {.class = "btn btn-primary btn-lg"})
                </div>
            </div>
        </div>
    Else
        @* Generic File *@
        <div class="card shadow-sm">
            <div class="card-body text-center py-5">
                <i class="bi bi-file-earmark text-secondary" style="font-size:4rem"></i>
                <h4 class="mt-3">File Preview Not Available</h4>
                <p class="text-muted">
                    File type @ext cannot be previewed in the browser.
                </p>
                <div class="mt-4">
                    @Html.ActionLink("Download File", "DownloadAttachment", New With {.id = att.AttachmentId}, New With {.class = "btn btn-primary btn-lg"})
                </div>
            </div>
        </div>
    End If
</div>
