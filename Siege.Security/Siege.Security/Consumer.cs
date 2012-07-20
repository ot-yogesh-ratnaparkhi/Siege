using System.Collections.Generic;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Consumer : SecurityEntity
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual IList<Application> Applications { get; set; }
        public virtual IList<User> Users { get; set; }
        public virtual IList<Group> Groups { get; set; }
        public virtual IList<Role> Roles { get; set; } 

        public Consumer()
        {
            this.Applications = new List<Application>();
            this.Users = new List<User>();
            this.Groups = new List<Group>();
            this.Roles = new List<Role>();
        }
    }
}