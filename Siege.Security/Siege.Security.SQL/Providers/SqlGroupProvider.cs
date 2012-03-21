using System.Collections.Generic;
using System.Linq;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlGroupProvider : SqlProvider<Group, int?>, IGroupProvider
    {
        public SqlGroupProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }

        public virtual IList<Group> GetForApplication(Application application)
        {
            return repository.Query<Group>(query => query.Where(p => p.Application == application)).Find();
        }
    }
}