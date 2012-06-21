/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using System;
using System.Linq;
using Siege.Repository.Finders;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository
{
    public class Repository<TDatabase>  : IRepository<TDatabase> where TDatabase : IDatabase
    {
        protected readonly IUnitOfWorkManager unitOfWork;

        public Repository(IUnitOfWorkManager unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public T Get<T>(object id) where T : class 
        {
            return unitOfWork.For<TDatabase>().Get<T>(id);
        }

        public void Save<T>(T item) where T : class
        {
            unitOfWork.For<TDatabase>().Transact(() => unitOfWork.For<TDatabase>().Save(item));
        }

        public void Delete<T>(T item) where T : class
        {
            unitOfWork.For<TDatabase>().Transact(() => unitOfWork.For<TDatabase>().Delete(item));
        }

        public void Transact(Action<IRepository<TDatabase>> transactor)
        {
            unitOfWork.For<TDatabase>().Transact(() => transactor(this));
        }
        
        [Obsolete("Use Queryable<T> instead.")]
		public IQuery<T> Query<T>(Func<IQueryable<T>, IQueryable<T>> expression) where T : class
		{
			var query = new QuerySpecification<T>();

			query.WithUnitOfWork(unitOfWork.For<TDatabase>());
			query = new QuerySpecification<T>(expression(query.ToIQueryable()));

			return new Query<T>(query);
		}

        public IQuery<T> Query<T>(QuerySpecification<T> querySpecification) where T : class
		{
			querySpecification.WithUnitOfWork(unitOfWork.For<TDatabase>());
			return new Query<T>(querySpecification);
		}

        public IQuery<T> Query<T>() where T : class
		{
			return Query(new QuerySpecification<T>());
		}

        public IQueryable<T> Queryable<T>() where T : class
        {
            var query = new QuerySpecification<T>();
            query.WithUnitOfWork(unitOfWork.For<TDatabase>());

            return query.ToIQueryable();
        }
    }
}