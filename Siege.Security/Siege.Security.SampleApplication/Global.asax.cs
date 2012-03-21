using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Siege.Repository.NHibernate;
using Siege.Repository.Web;
using Siege.Security.ASPNetMembership;
using Siege.Security.Providers;
using Siege.Security.SQL.Conventions;
using Siege.Security.SQL.Repository;
using Siege.Security.SampleApplication.Areas.Security.ModelBinders;
using Siege.Security.SampleApplication.AutoMapper;
using Siege.Security.SampleApplication.Controllers;
using Siege.Security.Web;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.Native.ConstructionStrategies;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;
using Siege.ServiceLocator.Web;
using Siege.ServiceLocator.Web.Conventions;
using IServiceLocatorAccessor = Siege.ServiceLocator.IServiceLocatorAccessor;

namespace Siege.Security.SampleApplication
{
    public class MvcApplication : ServiceLocatorHttpApplication, Siege.Repository.Web.IServiceLocatorAccessor, IServiceLocatorAccessor
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
            AutoMap.Build();

           // PortableAreaRegistration.RegisterEmbeddedViewEngine();
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
                );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { typeof(HomeController).Namespace }
                );
        }

        protected override void OnApplicationStarted()
        {
            RegisterGlobalFilters(GlobalFilters.Filters);
            BundleTable.Bundles.RegisterTemplateBundles();

            base.OnApplicationStarted();

            ServiceLocator
                .Register(Using.Convention(new NHibernateConvention<HttpUnitOfWorkStore, SecurityDatabase>(SecurityConfiguration.SessionFactoryFor("Security"))))
                .Register(Using.Convention<ControllerConvention<AccountController>>())
                .Register(Using.Convention<SqlSecurityConvention>())
                .Register(Using.Convention<AspNetMembershipConvention>());
                //.Register(Awareness.Of(() => ServiceLocator.GetInstance<IApplicationConfiguration>()))
                //.Register(Awareness.Of(ModelBinding.For<Consumer>().Using<ApplicationBinder>(ServiceLocator)))
                
            InitializeModelBinders();
        }

        private void InitializeModelBinders()
        {
            RegisterModelBinder<JqGridConfiguration, JqGridConfigurationModelBinder>();
            RegisterModelBinder<Application, AutoMappingBinder<Application, Guid?, IApplicationProvider>>();
            RegisterModelBinder<User, AutoMappingBinder<User, Guid?, IUserProvider>>();
            RegisterModelBinder<Group, AutoMappingBinder<Group, int?, IGroupProvider>>();
            RegisterModelBinder<Role, AutoMappingBinder<Role, int?, IRoleProvider>>();
            RegisterModelBinder<List<Role>, RolesBinder>();
            RegisterModelBinder<List<Group>, GroupsBinder>();
            RegisterModelBinder<List<Permission>, PermissionsBinder>();
            //ModelBinders.Binders.Add(typeof(JqGridSettings), new JqGridSettingsModelBinder());
            //ModelBinders.Binders.Add(typeof(JqGridSearchCriteria), new JqGridSearchCriteriaModelBinder());
        }

        protected void RegisterModelBinder<TModel, TBinder>() where TBinder : IModelBinder
        {
            locator.Register(Given<TBinder>.Then<TBinder>());
            ModelBinders.Binders.Add(typeof(TModel), ServiceLocator.GetInstance<TBinder>());
        }
    }
}