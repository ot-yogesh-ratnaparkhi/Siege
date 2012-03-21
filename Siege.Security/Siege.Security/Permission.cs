using Siege.Security.Entities;

namespace Siege.Security
{
    public class Permission : SecurityEntity<int?>
    {
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual bool ExcludeFromAssignment { get; set; }

        public virtual bool Can(string permission)
        {
            return permission == Name;
        }
    }
}