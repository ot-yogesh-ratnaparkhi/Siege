using Siege.Security.Principals;

namespace Siege.Security.Providers
{
    public interface IAuthenticationProvider
    {
        ISecurityPrincipal GetAuthenticatedUser();
        bool Authenticate(string userName, string password, bool rememberMe);
        void Clear();
    }
}