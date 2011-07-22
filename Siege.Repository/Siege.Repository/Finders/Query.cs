using System.Collections.Generic;
using System.Linq;

namespace Siege.Repository.Finders
{
    public class Query<T> : IQuery<T> where T : class
    {
        protected QuerySpecification<T> querySpecification;

		public Query(QuerySpecification<T> querySpecification)
		{
			this.querySpecification = querySpecification;
		}

    	public Query()
    	{
			this.querySpecification = new QuerySpecification<T>();
    	}

		public virtual IList<T> Find()
		{
			return this.querySpecification.ToIQueryable().Select(x => x).ToList();
		}

		public virtual T FindFirstOrDefault()
		{
			return this.querySpecification.ToIQueryable().Select(x => x).FirstOrDefault();
		}

        public virtual int Count()
        {
            return this.querySpecification.ToIQueryable().Count();
        }
    }
}