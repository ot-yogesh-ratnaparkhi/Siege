using System.Collections.Generic;
using System.Linq;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class User : ConsumerBasedSecurityEntity
    {
        public virtual string Password { get; set; }
        public virtual string Salt { get; set; }
        public virtual IList<Group> Groups { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual string Name { get; set; }
        public virtual string Email { get; set; }
        public virtual bool IsAuthenticated { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool IsLockedOut { get; set; }
        public virtual string SecretQuestion { get; set; }
        public virtual string SecretAnswer { get; set; }
        public virtual IList<Application> Applications { get; set; } 

        public User()
        {
            this.Applications = new List<Application>();
            this.Groups = new List<Group>();
            this.Roles = new List<Role>();
        }

        public virtual bool Can(string permission)
        {
            return IsAuthenticated && IsActive && (this.Groups.Any(g => g.Can(permission)) || this.Roles.Any(r => r.Can(permission)));
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