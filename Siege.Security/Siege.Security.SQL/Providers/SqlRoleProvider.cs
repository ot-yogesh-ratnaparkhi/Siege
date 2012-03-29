using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlRoleProvider : SqlProvider<Role>, IRoleProvider
    {
        public SqlRoleProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }

        public Role Find(int? id, bool includeHiddenPermissions)
        {
            var role = repository.Get<Role>(id);

            if (!includeHiddenPermissions && role.Permissions.Any(p => p.ExcludeFromAssignment)) return null;

            return role;
        }

        public virtual IList<Role> GetForConsumer(Consumer consumer, bool includeHiddenPermissions)
        {
            var list = consumer.Roles;

            if (!includeHiddenPermissions && list.Any(r => r.Permissions.Any(p => p.ExcludeFromAssignment)))
            {
                var newList = new List<Role>();
                newList.AddRange(list.Where(r => r.Permissions.Any(p => !p.ExcludeFromAssignment)).ToList());

                return newList;
            }

            return list;
        }
        
        public virtual IList<Role> GetForApplicationAndConsumer(Application application, Consumer consumer, bool includeHiddenPermissions)
        {
            var list = consumer.Roles.Where(g => g.Permissions.Any(p => p.Application.ID == application.ID)).ToList();

            if (!includeHiddenPermissions && list.Any(r => r.Permissions.Any(p => p.ExcludeFromAssignment)))
            {
                var newList = new List<Role>();
                newList.AddRange(list.Where(r => r.Permissions.Any(p => !p.ExcludeFromAssignment)).ToList());

                return newList;
            }

            return list;
        }
    }
}