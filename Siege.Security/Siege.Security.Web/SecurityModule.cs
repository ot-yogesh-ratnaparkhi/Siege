using System;
using System.Threading;
using System.Web;
using Siege.Security.Principals;
using Siege.Security.Providers;
using Siege.ServiceLocator;

namespace Siege.Security.Web
{
    public class SecurityModule : IHttpModule
    {
        public void Init(HttpApplication context)
        {
            if (!(context is IServiceLocatorAccessor))
            {
                throw new Exception("HttpApplication must inherit from IServiceLocatorAccessor.");
            }

            var serviceLocator = ((IServiceLocatorAccessor) context).ServiceLocator;

            if (serviceLocator.HasTypeRegistered(typeof(IUserProvider)))
            {
                context.PostAuthenticateRequest += RequestStart;
            }
        }

        private void RequestStart(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;

            if (!context.User.Identity.IsAuthenticated) return;

            var serviceLocator = ((IServiceLocatorAccessor)application).ServiceLocator;
            var userProvider = serviceLocator.GetInstance<IUserProvider>();
            var user = userProvider.FindByUserName(context.User.Identity.Name);

            if (user == null) return;

            user.IsAuthenticated = context.User.Identity.IsAuthenticated;
            context.User = new SecurityPrincipal(user);
            Thread.CurrentPrincipal = context.User;
        }

        public void Dispose()
        {
        }
    }
}
