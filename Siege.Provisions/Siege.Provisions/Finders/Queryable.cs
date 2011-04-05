using System.Collections.Generic;
using System.Linq;
using Siege.Provisions.UnitOfWork;

namespace Siege.Provisions.Finders
{
    public abstract class Queryable<T> : IFinder<T> where T : class
    {
        protected IQueryable<T> query;

        protected Queryable(IUnitOfWork unitOfWork)
        {
            this.query = unitOfWork.Query<T>();
        }

        public virtual IList<T> Find()
        {
            return this.query.ToList();
        }

        public virtual long Count()
        {
            return this.query.Count();
        }
    }
}