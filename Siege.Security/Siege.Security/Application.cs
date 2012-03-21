using System;
using Siege.Security.Entities;

namespace Siege.Security
{
    public class Application : SecurityEntity<Guid?>
    {
        public virtual string Name { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual string LoweredName
        {
            get { return Name != null ? Name.ToLower() : null; }
            set
            {
                
            }
        }
        public virtual string Description { get; set; }
    }
}