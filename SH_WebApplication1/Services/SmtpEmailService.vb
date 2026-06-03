Imports System.Net.Mail
Imports System.Net
Imports SH_WebApplication1.Data

Namespace Services

    Public Class SmtpEmailService
        Implements IEmailService

        Private ReadOnly _db As AppDbContext

        Public Sub New(db As AppDbContext)
            _db = db
        End Sub

        Public Function SendEmail(toEmail As String, subject As String, body As String) As Boolean Implements IEmailService.SendEmail
            Try
                ' Read SMTP settings from Web.config <system.net><mailSettings>
                Using client As New SmtpClient()
                    Dim message As New MailMessage()
                    message.To.Add(New MailAddress(toEmail))
                    message.Subject = subject
                    message.Body = body
                    message.IsBodyHtml = True
                    ' SmtpClient uses <system.net><mailSettings> from web.config
                    client.Send(message)
                End Using
                Return True
            Catch ex As Exception
                ' Log error (in production, use logging framework)
                System.Diagnostics.Debug.WriteLine($"Email send failed: {ex.Message}")
                Return False
            End Try
        End Function

        Public Function SendWorkflowNotification(itemId As Integer, fromState As String, toState As String, actionBy As String, comments As String) As Boolean Implements IEmailService.SendWorkflowNotification
            Try
                Dim item = _db.AppItems.Find(itemId)
                If item Is Nothing Then Return False
                Dim list = _db.AppLists.Find(item.ListId)
                ' In production, lookup user emails from ASP.NET Identity or AD
                ' For now, use a placeholder recipient
                Dim toEmail = "workflow-notifications@example.com"
                Dim subject = $"Workflow Update: {list?.ListName} — {item.ItemNo}"
                Dim body = $"<h3>Workflow Transition</h3>" &
                           $"<p><strong>Item:</strong> {item.ItemNo}</p>" &
                           $"<p><strong>List:</strong> {list?.ListName}</p>" &
                           $"<p><strong>From State:</strong> {fromState}</p>" &
                           $"<p><strong>To State:</strong> {toState}</p>" &
                           $"<p><strong>Action By:</strong> {actionBy}</p>" &
                           $"<p><strong>Comments:</strong> {comments}</p>" &
                           $"<p><a href='{GetItemUrl(itemId)}'>View Item</a></p>"
                Return SendEmail(toEmail, subject, body)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine($"Workflow notification failed: {ex.Message}")
                Return False
            End Try
        End Function

        Private Function GetItemUrl(itemId As Integer) As String
            ' In production, build full URL using Request.Url.Scheme + Request.Url.Authority
            Return $"/Items/Display/{itemId}"
        End Function

    End Class

End Namespace
