using System.Security.Principal;

namespace Siege.Security.Principals
{
    public class SecurityPrincipal : ISecurityPrincipal, IIdentity
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
            get { return this; }
        }

        public bool Can(string permission)
        {
            return user.Can(permission);
        }

        public string Name
        {
            get { return user.Name; }
        }

        public string AuthenticationType
        {
            get { return "Siege"; }
        }

        public bool IsAuthenticated
        {
            get { return user.IsAuthenticated; }
        }
    }
}