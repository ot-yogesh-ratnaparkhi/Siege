using System;
using System.Data;
using System.Data.SqlClient;

namespace Siege.Repository.MSSQL
{
    public class SQLProvider
    {
        public void Delete<T>(T item)
        {
            throw new NotImplementedException();
        }

        public T Save<T>(T item)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(object id)
        {
            throw new NotImplementedException();
        }

        public ITransaction CreateTransaction()
        {
            throw new NotImplementedException();
        }

        private void ExecuteQuery()
        {
            using (var connection = new SqlConnection())
            {
                var command = connection.CreateCommand();
                var parameter = command.CreateParameter();
                command.Parameters.Add(parameter);
                var adapter = new SqlDataAdapter(command);
                adapter.Fill(new DataSet());
            }
        }
    }
}