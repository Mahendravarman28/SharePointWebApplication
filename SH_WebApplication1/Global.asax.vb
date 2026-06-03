Imports System.Web.Http
Imports System.Web.Optimization
Imports System.Data.Entity
Imports SH_WebApplication1.Data

Public Class MvcApplication
    Inherits System.Web.HttpApplication

    Protected Sub Application_Start()
        ' Initialize the application database (creates schema + seeds data)
        Database.SetInitializer(New AppDbInitializer())
        Using ctx As New AppDbContext()
            ctx.Database.Initialize(False)
        End Using

        AreaRegistration.RegisterAllAreas()
        GlobalConfiguration.Configure(AddressOf WebApiConfig.Register)
        FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters)
        RouteConfig.RegisterRoutes(RouteTable.Routes)
        BundleConfig.RegisterBundles(BundleTable.Bundles)
    End Sub
End Class
