using System;

namespace Siege.Repository.MSSQL
{
    public interface ITransaction : IDisposable
    {
        void Commit();
        void Rollback();
    }
}