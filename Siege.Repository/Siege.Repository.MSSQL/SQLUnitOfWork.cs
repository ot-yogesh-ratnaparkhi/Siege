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

        public T Get<T>(object id)
        {
            throw new NotImplementedException();
        }

        public void Transact(Action action)
        {
            throw new NotImplementedException();
        }

        public T Transact<T>(Func<T> action)
        {
            throw new NotImplementedException();
        }

        public void Save<T>(T item)
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>()
        {
            throw new NotImplementedException();
        }
    }
}