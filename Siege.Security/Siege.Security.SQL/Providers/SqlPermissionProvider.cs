using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlPermissionProvider : SqlProvider<Permission>, IPermissionProvider
    {
        public SqlPermissionProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }

        public IList<Permission> All(bool includeHiddenPermissions)
        {
            if(!includeHiddenPermissions) return repository.Query<Permission>(query => query.Where(p => p.IsActive && !p.ExcludeFromAssignment)).Find();

            return repository.Query<Permission>(query => query.Where(p => p.IsActive)).Find();
        }

        public IList<Permission> ForApplication(Application application, bool includeHiddenPermissions)
        {
            if (!includeHiddenPermissions) return repository.Query<Permission>(query => query.Where(p => p.IsActive && !p.ExcludeFromAssignment && p.Application.ID == application.ID)).Find();

            return repository.Query<Permission>(query => query.Where(p => p.IsActive && p.Application.ID == application.ID)).Find();
        }
    }
}