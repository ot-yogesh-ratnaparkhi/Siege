using Siege.Repository;
using Siege.Security.Entities;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public abstract class SqlProvider<T> : IProvider<T> where T : SecurityEntity
    {
        protected readonly IRepository<SecurityDatabase> repository;

        protected SqlProvider(IRepository<SecurityDatabase> repository)
        {
            this.repository = repository;
        }

        public virtual T Save(T item)
        {
            repository.Save(item);

            return item;
        }

        public virtual void Delete(T item)
        {
            repository.Delete(item);
        }

        public virtual T Find(int? id)
        {
            return repository.Get<T>(id);
        }
    }
}