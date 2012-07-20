using System.Collections.Generic;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Permission : SecurityEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool ExcludeFromAssignment { get; set; }
        public virtual Application Application { get; set; }
        public virtual IList<Role> Roles { get; set; }

        public Permission()
        {
            this.Roles = new List<Role>();
        }

        public virtual bool Can(string permission)
        {
            return permission == Name;
        }
    }
}