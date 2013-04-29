using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using MVCSignalRSQLDependency.Hubs;
using Microsoft.AspNet.SignalR;
using StructureMap;
using IDependencyResolver = Microsoft.AspNet.SignalR.IDependencyResolver;

namespace MVCSignalRSQLDependency
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // DI
            var container = ConfigureContainer();

            // SignalR
            GlobalHost.DependencyResolver = container.GetInstance<IDependencyResolver>();
            RouteTable.Routes.MapHubs();

            // MVC
            ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory(container));
            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            SqlDependency.Start(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }


        protected void Application_End()
        {
            SqlDependency.Start(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
        }

        private static Container ConfigureContainer()
        {
            var container = new Container();
            container.Configure(cfg =>
            {
                cfg.For<IDependencyResolver>().Singleton().Add<StructureMapDependencyResolver>();
                cfg.For<IRepository>().Use<Repository>();
                cfg.For<IHubContext>().Use(() => GlobalHost.ConnectionManager.GetHubContext<ChatHub>());
            });
            return container;
        }
    }
}