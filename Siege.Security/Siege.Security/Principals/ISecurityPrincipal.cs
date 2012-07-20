using System.Security.Principal;

namespace Siege.Security.Principals
{
    public interface ISecurityPrincipal : IPrincipal
    {
        bool Can(string permission);
    }
}