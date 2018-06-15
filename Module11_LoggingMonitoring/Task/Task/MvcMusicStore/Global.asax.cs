using Autofac;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;
using MvcMusicStore.Controllers;

namespace MvcMusicStore
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private readonly ILogger logger;

        public MvcApplication()
        {
            logger = LogManager.GetCurrentClassLogger();
        }
        protected void Application_Start()
        {
            var builder = new ContainerBuilder();
            builder.RegisterControllers(typeof(HomeController).Assembly);
            builder.Register(f => LogManager.GetLogger("ForControllers")).As<ILogger>();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(builder.Build()));

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            logger.Info("Application is started");
        }

        protected void Application_Error()
        {
            var ex = Server.GetLastError();
            logger.Error(ex.ToString());
        }
    }
}
