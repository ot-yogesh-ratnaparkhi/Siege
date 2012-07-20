using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Siege.Repository.EntityFramework;
using Siege.Repository.Web;
using Siege.Security.Admin.Security.Controllers;
using Siege.Security.Admin.Security.ModelBinders;
using Siege.Security.SQL.Conventions;
using Siege.Security.SQL.Mappings;
using Siege.Security.SQL.Repository;
using Siege.Security.Web;
using Siege.ServiceLocator;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;
using Siege.ServiceLocator.Web.Conventions;

namespace Siege.Security.Admin
{
    public class SqlSecurityAdminConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return locator =>
            {
                locator
                    .Register(Using.Convention<ControllerConvention<HomeController>>())
                    .Register(Using.Convention<EntityFrameworkConvention<HttpUnitOfWorkStore, SecurityDatabase, SecurityContext>>())
                    .Register(Using.Convention<SqlSecurityConvention>());

                RegisterModelBinder<JqGridConfiguration, JqGridConfigurationModelBinder>(locator);
                RegisterModelBinder<List<Role>, RolesBinder>(locator);
                RegisterModelBinder<List<Group>, GroupsBinder>(locator);
                RegisterModelBinder<List<Application>, ApplicationsBinder>(locator);
                RegisterModelBinder<List<Permission>, PermissionsBinder>(locator);
                RegisterModelBinder<User, UserBinder>(locator);
                RegisterModelBinder<Application, ApplicationBinder>(locator);
                RegisterModelBinder<Group, GroupBinder>(locator);
                RegisterModelBinder<Consumer, ConsumerBinder>(locator);
                RegisterModelBinder<Role, RoleBinder>(locator);
            };
        }

        protected void RegisterModelBinder<TModel, TBinder>(IServiceLocator locator) where TBinder : IModelBinder
        {
            locator.Register(Given<TBinder>.Then<TBinder>());
            ModelBinders.Binders.Add(typeof(TModel), locator.GetInstance<TBinder>());
        }
    }
}