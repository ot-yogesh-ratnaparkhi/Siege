using System;
using System.Collections.Generic;
using Siege.Repository;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public class SqlApplicationProvider : IApplicationProvider
    {
        private readonly IRepository<SecurityDatabase> repository;

        public SqlApplicationProvider(IRepository<SecurityDatabase> repository)
        {
            this.repository = repository;
        }

        public IList<Application> GetAllApplications()
        {
            return repository.Query<Application>().Find();
        }

        public Application Save(Application item)
        {
            repository.Save(item);

            return item;
        }

        public void Delete(Application item)
        {
            repository.Delete(item);
        }

        public Application Find(int? id)
        {
            return repository.Get<Application>(id);
        }
    }
}
