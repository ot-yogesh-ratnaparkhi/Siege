using System;
using System.Linq;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.MSSQL
{
    public class SQLUnitOfWork : IUnitOfWork
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id) where T : class
        {
            throw new NotImplementedException();
        }

        public void Transact(Action action)
        {
            throw new NotImplementedException();
        }

        public T Transact<T>(Func<T> action) where T : class
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T item) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}