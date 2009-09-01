using System.Collections.Generic;
using NHibernate.Criterion;

namespace Siege.Persistence
{
    public interface IFinder<T>
    {
        IList<T> Find();
        int Count();
    }

    public abstract class AbstractFinder<T> : IFinder<T>
    {
        private readonly IPersistenceManager manager;
        private readonly int pageSize;

        protected AbstractFinder(IPersistenceManager manager) : this(manager, 1) {}
        protected AbstractFinder(IPersistenceManager manager, int pageSize)
        {
            this.manager = manager;
            this.pageSize = pageSize;
        }

        public IList<T> Find()
        {
            using(var session = manager.SessionFactory.OpenSession())
            {
                DetachedCriteria criteria = CreateCriteria();

                return criteria.GetExecutableCriteria(session).List<T>();
            }
        }

        public int Count()
        {
            using (var session = manager.SessionFactory.OpenSession())
            {
                DetachedCriteria criteria = CreateCountCriteria();

                return (int)criteria.GetExecutableCriteria(session).UniqueResult();
            }
        }

        protected abstract DetachedCriteria CreateCriteria();
        protected abstract DetachedCriteria CreateCountCriteria();
    }
}
