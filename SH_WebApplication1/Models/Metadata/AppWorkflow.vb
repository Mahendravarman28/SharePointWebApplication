Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Models.Metadata

    <Table("AppWorkflows")>
    Public Class AppWorkflow

        <Key>
        Public Property WorkflowId As Integer

        Public Property ListId As Integer

        <Required, MaxLength(200)>
        Public Property WorkflowName As String

        ''' <summary>Full state-machine JSON definition</summary>
        Public Property WorkflowJson As String

        Public Property IsPublished As Boolean = False

        <MaxLength(256)>
        Public Property CreatedBy As String

        Public Property CreatedDate As DateTime = DateTime.Now

        <ForeignKey("ListId")>
        Public Overridable Property List As AppList

        Public Overridable Property States As ICollection(Of AppWorkflowState) = New List(Of AppWorkflowState)()
        Public Overridable Property Transitions As ICollection(Of AppWorkflowTransition) = New List(Of AppWorkflowTransition)()

    End Class


    <Table("AppWorkflowStates")>
    Public Class AppWorkflowState

        <Key>
        Public Property StateId As Integer

        Public Property WorkflowId As Integer

        <Required, MaxLength(200)>
        Public Property StateName As String

        <Required, MaxLength(100)>
        Public Property StateCode As String

        Public Property IsStart As Boolean = False
        Public Property IsEnd As Boolean = False

        Public Property DisplayOrder As Integer = 0

        <MaxLength(50)>
        Public Property ColorCode As String

        <ForeignKey("WorkflowId")>
        Public Overridable Property Workflow As AppWorkflow

    End Class


    <Table("AppWorkflowTransitions")>
    Public Class AppWorkflowTransition

        <Key>
        Public Property TransitionId As Integer

        Public Property WorkflowId As Integer

        Public Property FromStateId As Integer

        Public Property ToStateId As Integer

        <Required, MaxLength(200)>
        Public Property ActionName As String

        <MaxLength(200)>
        Public Property ActionCode As String

        ''' <summary>Expression evaluated at runtime e.g. Amount > 10000</summary>
        Public Property ConditionExpression As String

        ''' <summary>Role required to perform this transition</summary>
        <MaxLength(200)>
        Public Property RoleRequired As String

        ''' <summary>JSON: field updates to apply on this transition</summary>
        Public Property ActionJson As String

        ''' <summary>JSON: notification config</summary>
        Public Property NotificationJson As String

        <ForeignKey("WorkflowId")>
        Public Overridable Property Workflow As AppWorkflow

        <ForeignKey("FromStateId")>
        Public Overridable Property FromState As AppWorkflowState

        <ForeignKey("ToStateId")>
        Public Overridable Property ToState As AppWorkflowState

    End Class

End Namespace
