using System.Collections.Generic;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Application : SecurityEntity
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Consumer> Consumers { get; set; }
        public virtual IList<Permission> Permissions { get; set; }
        public virtual IList<User> Users { get; set; } 

        public Application()
        {
            this.Consumers = new List<Consumer>();
            this.Permissions = new List<Permission>();
            this.Users = new List<User>();
        }
    }
}