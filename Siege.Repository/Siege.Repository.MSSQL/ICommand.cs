using System.Data.SqlClient;

namespace Siege.Repository.MSSQL
{
    public interface ICommand
    {
        SqlCommand GenerateFor<T>(T item) where T : class;
    }
}