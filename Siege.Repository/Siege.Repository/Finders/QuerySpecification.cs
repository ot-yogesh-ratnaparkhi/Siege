using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.Finders
{
	public class QuerySpecification<T>
	{
		protected IQueryable<T> linqQuery;
		private readonly List<Action> conditions = new List<Action>();
		
		public QuerySpecification()
		{
		}

		public QuerySpecification(IQueryable<T> query)
		{
			linqQuery = query;
		}

		internal void WithUnitOfWork(IUnitOfWork unitOfWork)
		{
			this.linqQuery = unitOfWork.Query<T>();
		}

		public QuerySpecification<T> Where(Expression<Func<T, bool>> expression)
		{
			this.conditions.Add(() => linqQuery = linqQuery.Where(expression));
			return this;
		}
		
		public QuerySpecification<T> Do(Func<IQueryable<T>, IQueryable<T>> query)
		{
			this.conditions.Add(() => linqQuery = query(linqQuery));
			return this;
		}

		internal IQueryable<T> ToIQueryable()
		{
			foreach(var condition in conditions)
			{
				condition();
			}

			return this.linqQuery;
		}
	}
}