using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlRoleProvider : SqlProvider<Role, int?>, IRoleProvider
    {
        public SqlRoleProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }
        
        public virtual IList<Role> GetForApplication(Application application)
        {
            return repository.Query<Role>(query => query.Where(p => p.Application == application)).Find();
        }
    }
}