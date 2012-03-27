using System.Web;
using System.Web.Security;
using Siege.Security.Principals;
using Siege.Security.Providers;

namespace Siege.Security.Web
{
    public class WebAuthenticationProvider : IAuthenticationProvider
    {
        private readonly IIdentityProvider provider;

        public WebAuthenticationProvider(IIdentityProvider provider)
        {
            this.provider = provider;
        }

        public ISecurityPrincipal GetAuthenticatedUser()
        {
            return (ISecurityPrincipal)HttpContext.Current.User;
        }

        public bool Authenticate(string userName, string password, bool rememberMe)
        {
            if (provider.ValidateUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, rememberMe);
                return true;
            }

            return false;
        }

        public void Clear()
        {
            FormsAuthentication.SignOut();
        }
    }
}
