using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlGroupProvider : SqlProvider<Group>, IGroupProvider
    {
        public SqlGroupProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }

        public Group Find(int? id, bool includeHiddenPermissions)
        {
            var group = repository.Get<Group>(id);

            if (!includeHiddenPermissions && group.Permissions.Any(p => p.ExcludeFromAssignment)) return null;

            return group;
        }

        public virtual IList<Group> GetForConsumer(Consumer consumer, bool includeHiddenPermissions)
        {
            var list = consumer.Groups;

            if (!includeHiddenPermissions && list.Any(r => r.Permissions.Any(p => p.ExcludeFromAssignment)))
            {
                var newList = new List<Group>();
                newList.AddRange(list.Where(r => r.Permissions.Any(p => !p.ExcludeFromAssignment)).ToList());

                return newList;
            }

            return list;
        }

        public virtual IList<Group> GetForApplicationAndConsumer(Application application, Consumer consumer, bool includeHiddenPermissions)
        {
            var list = consumer.Groups.Where(g => g.Permissions.Any(p => p.Application.ID == application.ID)).ToList();

            if (!includeHiddenPermissions && list.Any(r => r.Permissions.Any(p => p.ExcludeFromAssignment)))
            {
                var newList = new List<Group>();
                newList.AddRange(list.Where(r => r.Permissions.Any(p => !p.ExcludeFromAssignment)).ToList());

                return newList;
            }

            return list;
        }
    }
}