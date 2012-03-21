using System;
using System.Web.Security;
using Siege.Security.Providers;
using Siege.ServiceLocator;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;

namespace Siege.Security.ASPNetMembership
{
    public class AspNetMembershipConvention : IConvention
    {
        public Action<IServiceLocator> Build()
        {
            return serviceLocator => serviceLocator
                .Register(Given<MembershipProvider>.Then(Membership.Provider))
                .Register(Given<IAuthenticationProvider>.Then<AspNetMembershipProvider>())
                .Register(Given<IIdentityProvider>.Then<AspNetMembershipProvider>());
        }
    }
}