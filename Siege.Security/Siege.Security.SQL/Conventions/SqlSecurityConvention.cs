using System;
using Siege.Security.Providers;
using Siege.Security.SQL.Providers;
using Siege.ServiceLocator;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;

namespace Siege.Security.SQL.Conventions
{
    public class SqlSecurityConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator => serviceLocator
                .Register(Given<IIdentityProvider>.Then<SqlIdentityProvider>())
                .Register(Given<IConsumerProvider>.Then<SqlConsumerProvider>())
                .Register(Given<IPermissionProvider>.Then<SqlPermissionProvider>())
                .Register(Given<IRoleProvider>.Then<SqlRoleProvider>())
                .Register(Given<IGroupProvider>.Then<SqlGroupProvider>())
                .Register(Given<IUserProvider>.Then<SqlUserProvider>())
                .Register(Given<IApplicationProvider>.Then<SqlApplicationProvider>());
        }
    }
}