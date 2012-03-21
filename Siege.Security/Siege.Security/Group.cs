using System.Collections.Generic;
using System.Linq;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Group : ApplicationBasedSecurityEntity<int?>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Role> Roles { get; set; }
        public virtual bool IsActive { get; set; }

        public Group()
        {
            this.Roles = new List<Role>();
        }

        public virtual bool Can(string permission)
        {
            return this.Roles.Any(r => r.Can(permission));
        }

        public virtual List<Permission> Permissions
        {
            get
            {
                return Roles.SelectMany(role => role.Permissions).ToList();
            }
        }
    }
}