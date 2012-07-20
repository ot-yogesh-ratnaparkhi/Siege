using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MvcContrib.PortableAreas;
using Siege.Security.Admin;
using Siege.Security.Providers;
using Siege.Security.SampleApplication.Controllers;
using Siege.Security.Web;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.Native.ConstructionStrategies;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;
using Siege.ServiceLocator.Web;
using Siege.ServiceLocator.Web.Conventions;
using IServiceLocatorAccessor = Siege.Repository.Web.IServiceLocatorAccessor;

namespace Siege.Security.SampleApplication
{
    public class MvcApplication : ServiceLocatorHttpApplication, IServiceLocatorAccessor,
                                  ServiceLocator.IServiceLocatorAccessor
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        protected override IServiceLocatorAdapter GetServiceLocatorAdapter()
        {
            return new SiegeAdapter(new ReflectionConstructionStrategy());
        }

        public override void RegisterRoutes(RouteCollection routes)
        {
            AreaRegistration.RegisterAllAreas();

            PortableAreaRegistration.RegisterEmbeddedViewEngine();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new {controller = "Home", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] {typeof (HomeController).Namespace}
                );
        }

        protected override void OnApplicationStarted()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            BundleTable.Bundles.RegisterTemplateBundles();

            base.OnApplicationStarted();

            ServiceLocator
                .Register(Given<IAuthenticationProvider>.Then<WebAuthenticationProvider>())
                .Register(Using.Convention(new SqlSecurityAdminConvention()))
                .Register(Using.Convention<ControllerConvention<AccountController>>());
        }
    }
}