using System.Security.Principal;

namespace Siege.Security.Principals
{
    public class SecurityPrincipal : ISecurityPrincipal
    {
        private readonly User user;

        public SecurityPrincipal(User user)
        {
            this.user = user;
        }

        public bool IsInRole(string role)
        {
            return Can(role);
        }

        public IIdentity Identity
        {
            get { return user; }
        }

        public bool Can(string permission)
        {
            return user.Can(permission);
        }
    }
}