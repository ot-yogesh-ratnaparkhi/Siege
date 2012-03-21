using Siege.Repository;
using Siege.Security.Entities;
using Siege.Security.Providers;
using Siege.Security.SQL.Repository;

namespace Siege.Security.SQL.Providers
{
    public abstract class SqlProvider<T, TID> : IProvider<T, TID> where T : SecurityEntity<TID>
    {
        protected readonly IRepository<SecurityDatabase> repository;

        protected SqlProvider(IRepository<SecurityDatabase> repository)
        {
            this.repository = repository;
        }

        #region IProvider<T,TID> Members

        public virtual T Save(T item)
        {
            repository.Save(item);

            return item;
        }

        public virtual void Delete(T item)
        {
            repository.Delete(item);
        }

        public virtual T Find(TID id)
        {
            return repository.Get<T>(id);
        }

        #endregion
    }
}