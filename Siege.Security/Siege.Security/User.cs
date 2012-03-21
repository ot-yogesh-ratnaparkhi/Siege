using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Siege.Security.Entities;
using Siege.Security.Principals;

namespace Siege.Security
{
    public class User : ApplicationBasedSecurityEntity<Guid?>, IIdentity, ISecurityPrincipal
    {
        public virtual string Password { get; set; }
        public virtual IList<Group> Groups { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual string Name { get; set; }
        public virtual string AuthenticationType { get; set; }
        public virtual bool IsAuthenticated { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsLockedOut { get; set; }

        public User()
        {
            this.Groups = new List<Group>();
            this.Roles = new List<Role>();
        }

        public virtual bool Can(string permission)
        {
            return IsAuthenticated && IsActive && (this.Groups.Any(g => g.Can(permission)) || this.Roles.Any(r => r.Can(permission)));
        }

        bool IPrincipal.IsInRole(string role)
        {
            return Can(role);
        }

        IIdentity IPrincipal.Identity
        {
            get { return this; }
        }

        public virtual List<Permission> Permissions
        {
            get
            {
                var permissions = new List<Permission>(); 
                
                permissions.AddRange(Groups.SelectMany(group => group.Permissions));
                permissions.AddRange(Roles.SelectMany(role => role.Permissions));

                return permissions;
            }
        }
    }
}