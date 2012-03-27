using System.Collections.Generic;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlConsumerProvider : SqlProvider<Consumer>, IConsumerProvider
    {
        public SqlConsumerProvider(IRepository<SecurityDatabase> repository) : base(repository)
        {
        }

        public IList<Consumer> All()
        {
            return repository.Query<Consumer>().Find();
        }
    }
}