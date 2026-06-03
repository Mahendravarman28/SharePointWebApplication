Imports Autofac
Imports Autofac.Integration.Mvc
Imports System.Reflection
Imports System.Web.Mvc
Imports SH_WebApplication1.Data
Imports SH_WebApplication1.Services

Namespace App_Start

    Public Class AutofacConfig

        Public Shared Sub ConfigureContainer()
            Dim builder As New ContainerBuilder()

            ' Register controllers
            builder.RegisterControllers(Assembly.GetExecutingAssembly())

            ' Register DbContext
            builder.RegisterType(Of AppDbContext)().InstancePerRequest()

            ' Register services
            builder.RegisterType(Of ListService)().As(Of IListService)().InstancePerRequest()
            builder.RegisterType(Of FormService)().As(Of IFormService)().InstancePerRequest()
            builder.RegisterType(Of ViewService)().As(Of IViewService)().InstancePerRequest()
            builder.RegisterType(Of PageService)().As(Of IPageService)().InstancePerRequest()
            builder.RegisterType(Of ItemService)().As(Of IItemService)().InstancePerRequest()
            builder.RegisterType(Of WorkflowService)().As(Of IWorkflowService)().InstancePerRequest()
            builder.RegisterType(Of SecurityService)().As(Of ISecurityService)().InstancePerRequest()
            builder.RegisterType(Of SmtpEmailService)().As(Of IEmailService)().InstancePerRequest()

            ' Build container
            Dim container = builder.Build()
            DependencyResolver.SetResolver(New AutofacDependencyResolver(container))
        End Sub

    End Class

End Namespace
