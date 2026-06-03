# Document Viewer for Attachments

## Overview
Successfully implemented inline document viewing capabilities for PDF, Office documents, images, and text files directly from the item Display page.

## Features Implemented

### 1. File Type Detection and Icons
- **Automatic file type recognition** based on file extension
- **Color-coded Bootstrap icons** for different file types:
  - 📕 PDF (red)
  - 📘 Word documents (blue)
  - 📗 Excel spreadsheets (green)
  - 📙 PowerPoint presentations (orange)
  - 🖼️ Images (cyan)
  - 📄 Text files (gray)
  - 📦 Archives (dark)
  - 📋 Generic files (muted)

### 2. Inline Viewing Capabilities
- **PDF Documents**: Full inline viewing in iframe using browser's native PDF renderer
- **Images** (JPG, PNG, GIF, BMP): Direct inline display
- **Text Files**: Inline text viewing
- **Office Documents** (Word, Excel, PowerPoint): Information page with download option

### 3. Modal Viewer (Display Page)
- **Bootstrap 5 modal** with 95% screen width and 90% height
- **Loading spinner** while document loads
- **Dynamic title** showing filename
- **Download button** for quick file download
- **iframe-based** rendering for supported file types
- **Auto-cleanup** when modal closes to prevent memory leaks

### 4. Full-Page Viewer (ViewAttachment Page)
- **Dedicated page** for document viewing
- **PDF full-page view** using iframe
- **Office document guidance** with download options
- **Fallback messages** for unsupported file types
- **Back navigation** and download buttons

## Implementation Details

### Controllers/ItemsController.vb
**New Action: ViewAttachment(id)**
```vb
' Serves files with inline content disposition
' Returns file directly for PDFs and images
' Returns ViewAttachment view for Office docs
```

### Views/Items/Display.vbhtml
**Enhanced Features:**
- `GetFileIcon()` helper function for file-type icons
- `CanViewInline()` helper to determine viewable files
- Enhanced attachment list with icons and metadata
- View/Download/Delete buttons for each attachment
- JavaScript modal viewer with iframe loader

**Modal Structure:**
- XL size modal (95% width, 90vh height)
- Loading indicator
- Dynamic title and download link
- Iframe container for document rendering

### Views/Items/ViewAttachment.vbhtml
**Full-Page Viewer:**
- PDF: Full iframe viewer
- Office docs: Information page with download option
- Other files: Generic download page
- Responsive layout with download and back buttons

## File Type Support

| File Type | Extension | View in Modal | View Full Page | Icon |
|-----------|-----------|---------------|----------------|------|
| PDF | .pdf | ✅ Yes | ✅ Yes | 📕 Red |
| Word | .doc, .docx | ❌ Download | ℹ️ Info page | 📘 Blue |
| Excel | .xls, .xlsx | ❌ Download | ℹ️ Info page | 📗 Green |
| PowerPoint | .ppt, .pptx | ❌ Download | ℹ️ Info page | 📙 Orange |
| Images | .jpg, .png, .gif | ✅ Yes | ✅ Yes | 🖼️ Cyan |
| Text | .txt | ✅ Yes | ✅ Yes | 📄 Gray |
| Archives | .zip, .rar, .7z | ❌ Download | ❌ Download | 📦 Dark |

## User Experience

### Viewing a PDF
1. Click **"View"** button on attachment
2. Modal opens with loading spinner
3. PDF renders in iframe using browser's native viewer
4. User can zoom, search, and navigate pages
5. Click "Download" to save file or "Close" to dismiss

### Viewing Office Documents
1. Click **"View"** button on Office document
2. Navigates to ViewAttachment page
3. Shows information about Office document type
4. Provides download button and explains Office Online Viewer requirements
5. In production, can integrate with SharePoint Online/Office 365

### Viewing Images
1. Click **"View"** button on image
2. Modal opens with image displayed inline
3. Image scales to fit modal size
4. Download or close as needed

## Technical Implementation

### Content Disposition Headers
```vb
' Inline viewing for supported types
Response.AddHeader("Content-Disposition", $"inline; filename=""{fileName}""")

' Forces download for unsupported types
Return File(filePath, contentType, fileName)
```

### Modal JavaScript
```javascript
function viewDocument(attachmentId, fileName) {
	// Show modal
	var modal = new bootstrap.Modal(document.getElementById('documentViewerModal'));

	// Update UI
	document.getElementById('docTitle').textContent = fileName;
	document.getElementById('docDownloadBtn').href = downloadUrl;

	// Load document in iframe
	iframe.src = viewUrl;
}
```

### Memory Management
- Modal clears iframe source on close
- Prevents memory leaks from loaded documents
- Ensures fresh load on each open

## Browser Compatibility

### PDF Viewing
- ✅ Chrome/Edge: Native PDF viewer
- ✅ Firefox: Native PDF viewer
- ✅ Safari: Native PDF viewer
- ⚠️ IE11: May require PDF.js fallback (deprecated browser)

### Image Viewing
- ✅ All modern browsers support inline image display

### Office Documents
- ❌ No browser natively renders Office documents
- 💡 Solutions:
  1. Use Office Online Viewer (requires public URL)
  2. Convert to PDF server-side (requires LibreOffice/Aspose)
  3. Use third-party viewers (Google Docs Viewer, Zoho)
  4. Download and open in desktop app (current implementation)

## Production Enhancements

### Office Online Viewer Integration
For production with public URLs:
```vb
Dim publicUrl = $"https://yourdomain.com/Items/DownloadAttachment/{attachmentId}"
Dim viewerUrl = $"https://view.officeapps.live.com/op/embed.aspx?src={Uri.EscapeDataString(publicUrl)}"
' Return view with viewer URL in iframe
```

### PDF.js Integration
For advanced PDF features:
```html
<script src="https://cdnjs.cloudflare.com/ajax/libs/pdf.js/3.11.174/pdf.min.js"></script>
<!-- Custom PDF rendering with zoom, rotation, search -->
```

### Server-Side Document Conversion
For Office document viewing:
- Install LibreOffice on server
- Convert Office docs to PDF on-demand
- Cache converted PDFs for performance
- Requires significant server resources

## Security Considerations

### Current Implementation
- ✅ Requires authentication ([Authorize] attribute)
- ✅ File ownership verified via attachment table
- ✅ Files stored outside web root (~/App_Data)
- ✅ No directory traversal vulnerabilities

### Additional Security Recommendations
1. **Virus scanning** on upload (integrate ClamAV or similar)
2. **File size limits** to prevent DoS
3. **MIME type validation** to prevent execution of malicious files
4. **Content Security Policy** headers for iframe security
5. **Rate limiting** on view/download endpoints

## Build Status
✅ **Build successful** - All code compiles without errors

## Testing Recommendations

1. **Upload various file types** (PDF, Word, Excel, PowerPoint, images)
2. **Test modal viewer** with different screen sizes
3. **Verify download functionality** for all file types
4. **Test with large files** (10+ MB) to check performance
5. **Check browser compatibility** across Chrome, Firefox, Safari, Edge
6. **Verify security** - try accessing attachments from different users
7. **Test mobile responsiveness** of modal viewer

## Known Limitations

1. **Office Documents**: No inline viewing without external service
2. **Large PDFs**: May be slow to load in iframe
3. **Mobile**: Modal may not work well on small screens
4. **Bandwidth**: Each view loads full file (no streaming)

## Future Enhancements

1. **PDF.js integration** for enhanced PDF viewing with annotations
2. **Office Online Viewer** integration for production deployment
3. **Document thumbnails** for visual preview in attachment list
4. **Version history** for documents
5. **Collaborative editing** integration (SharePoint/Office 365)
6. **Full-text search** across document contents
7. **Document conversion** service for Office docs → PDF
8. **Lazy loading** and pagination for large attachment lists
