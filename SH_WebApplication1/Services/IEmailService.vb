Imports System.Net.Mail

Namespace Services

    Public Interface IEmailService
        Function SendEmail(toEmail As String, subject As String, body As String) As Boolean
        Function SendWorkflowNotification(itemId As Integer, fromState As String, toState As String, actionBy As String, comments As String) As Boolean
    End Interface

End Namespace
