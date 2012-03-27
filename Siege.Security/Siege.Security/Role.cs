using System.Collections.Generic;
using System.Linq;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Role : ConsumerBasedSecurityEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
        public virtual IList<Group> Groups { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual bool IsActive { get; set; }
        
        public Role()
        {
            this.Permissions = new List<Permission>();
            this.Groups = new List<Group>();
            this.Users = new List<User>();
        }

        public virtual bool Can(string permission)
        {
            return this.Permissions.Any(p => p.Can(permission));
        }
    }
}