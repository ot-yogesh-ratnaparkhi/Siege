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
using NHibernate;
using NHibernate.Linq;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.NHibernate
{
	public class NHibernateUnitOfWork : IUnitOfWork
	{
		private readonly ISessionFactory sessionFactory;
		private ISession session;

		public NHibernateUnitOfWork(ISessionFactory sessionFactory)
		{
			this.sessionFactory = sessionFactory;
		}

		public ISession Session
		{
			get
			{
				if (this.session == null) this.session = sessionFactory.OpenSession();

				return session;
			}
		}

		public void Save<T>(T item) where T : class
		{
		    Transact(() => Session.SaveOrUpdate(item));
		}

        public void Delete<T>(T item) where T : class
	    {
            Transact(() => Session.Delete(item));
        }

        public IQueryable<T> Query<T>() where T : class
	    {
	        return Session.Query<T>();
	    }

        public T Get<T>(object id) where T : class
		{
			return Transact(() => Session.Get<T>(id));
		}

        public void Transact(Action action)
        {
            if (Session.Transaction.IsActive)
            {
                action();
            }
            else
            {
                using (var transaction = this.Session.BeginTransaction())
                {
                    try
                    {
                        action();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        if (transaction != null) transaction.Rollback();
                        session = null;
						throw;
                    }
                }
            }
        }

        public T Transact<T>(Func<T> action) where T : class
		{
            if (Session.Transaction.IsActive)
            {
                T result = action();
                return result;
            }

			using (var transaction = this.Session.BeginTransaction())
			{
				try
				{
					T result = action();
					transaction.Commit();
					return result;
				}
				catch (Exception)
				{
					if (transaction != null) transaction.Rollback();
                    session = null;
					throw;
				}
			}
		}

		public void Dispose()
		{
			if (session != null)
			{
			    session.Close();
				session.Dispose();
				session = null;
			}
		}
	}
}