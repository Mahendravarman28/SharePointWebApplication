Imports System.Data.Entity
Imports SH_WebApplication1.Models.Metadata

Namespace Data

    Public Class AppDbContext
        Inherits DbContext

        Public Sub New()
            MyBase.New("name=DefaultConnection")
        End Sub

        ' Metadata tables
        Public Property AppLists As DbSet(Of AppList)
        Public Property AppListFields As DbSet(Of AppListField)
        Public Property AppForms As DbSet(Of AppForm)
        Public Property AppViews As DbSet(Of AppView)
        Public Property AppPages As DbSet(Of AppPage)
        Public Property AppWorkflows As DbSet(Of AppWorkflow)
        Public Property AppWorkflowStates As DbSet(Of AppWorkflowState)
        Public Property AppWorkflowTransitions As DbSet(Of AppWorkflowTransition)
        Public Property AppRoles As DbSet(Of AppRole)
        Public Property AppPermissions As DbSet(Of AppPermission)
        Public Property AppRolePermissions As DbSet(Of AppRolePermission)
        Public Property AppUserRoles As DbSet(Of AppUserRole)

        ' Transaction tables
        Public Property AppItems As DbSet(Of AppItem)
        Public Property AppItemValues As DbSet(Of AppItemValue)
        Public Property AppWorkflowTransactions As DbSet(Of AppWorkflowTransaction)
        Public Property AppAttachments As DbSet(Of AppAttachment)

        Protected Overrides Sub OnModelCreating(modelBuilder As DbModelBuilder)
            MyBase.OnModelCreating(modelBuilder)

            ' Prevent cascading delete cycles on workflow transitions
            modelBuilder.Entity(Of AppWorkflowTransition)() _
                .HasRequired(Function(t) t.FromState) _
                .WithMany() _
                .HasForeignKey(Function(t) t.FromStateId) _
                .WillCascadeOnDelete(False)

            modelBuilder.Entity(Of AppWorkflowTransition)() _
                .HasRequired(Function(t) t.ToState) _
                .WithMany() _
                .HasForeignKey(Function(t) t.ToStateId) _
                .WillCascadeOnDelete(False)

            modelBuilder.Entity(Of AppWorkflowTransition)() _
                .HasRequired(Function(t) t.Workflow) _
                .WithMany(Function(w) w.Transitions) _
                .HasForeignKey(Function(t) t.WorkflowId) _
                .WillCascadeOnDelete(False)
        End Sub

    End Class

End Namespace
