Imports System.Web.Http
Imports System.Web.Optimization
Imports System.Data.Entity
Imports SH_WebApplication1.Data
' Imports SH_WebApplication1.App_Start

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        ' Initialize the application database (creates schema + seeds data)
        Database.SetInitializer(New AppDbInitializer())
        Using ctx As New AppDbContext()
            ctx.Database.Initialize(False)
        End Using

        ' Configure Autofac IoC container (requires Autofac.Mvc5 NuGet package)
        ' Install via Package Manager Console: Install-Package Autofac.Mvc5
        ' AutofacConfig.ConfigureContainer()

        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub
End Class
