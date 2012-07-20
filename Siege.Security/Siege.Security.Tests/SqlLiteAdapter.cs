using System.Data;
using NHibernate.Connection;

namespace Siege.Security.Tests
{
    public class SqlLiteAdapter : DriverConnectionProvider
    {
        private static IDbConnection connection;

        public override IDbConnection GetConnection()
        {
            return connection ?? (connection = base.GetConnection());
        }

        public override void CloseConnection(IDbConnection conn)
        {
            
        }
    }
}