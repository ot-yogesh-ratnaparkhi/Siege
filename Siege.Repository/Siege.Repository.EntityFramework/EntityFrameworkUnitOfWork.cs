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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Linq;
using System.Transactions;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.EntityFramework
{
	public class EntityFrameworkUnitOfWork : IUnitOfWork
	{
        private DbContext context;
		
        public EntityFrameworkUnitOfWork(DbContext context)
		{
            this.context = context;
		}

        public void Save<T>(T item) where T : class
		{
		    Transact(() => 
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                ObjectStateEntry entry;
                
                if(!objectContext.ObjectStateManager.TryGetObjectStateEntry(item, out entry))
                {
                    context.Set<T>().Add(item);
                }
            });
		}

        public void Delete<T>(T item) where T : class
	    {
            Transact(() => context.Set<T>().Remove(item));
        }

        public IQueryable<T> Query<T>() where T : class
	    {
	        return context.Set<T>().AsQueryable();
	    }

        public T Get<T>(object id) where T : class
		{
			return Transact(() => context.Set<T>().Find(id));
		}

        public void Transact(Action action)
        {
            using (var scope = new TransactionScope())
            {
                action();
                context.SaveChanges();
                scope.Complete();
            }
        }

        public T Transact<T>(Func<T> action) where T : class
		{
            using (var scope = new TransactionScope())
			{
				T result = action();
			    context.SaveChanges();
                scope.Complete();
				return result;
			}
		}

		public void Dispose()
		{
		    if (context == null) return;
		    
		    context.Dispose();
		    context = null;
		}
	}
}