using FluentNHibernate.Cfg;
using NHibernate;

namespace Siege.Persistence
{
    public class SQLServerPersistenceManager : IPersistenceManager
    {
        private static ISessionFactory sessionFactory;
        private static readonly object lockObject = new object();
        private readonly FluentConfiguration configuration;

        public SQLServerPersistenceManager(FluentConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public ISessionFactory SessionFactory
        {
            get
            {
                if (sessionFactory == null)
                {
                    lock (lockObject)
                    {
                        if (sessionFactory == null)
                        {
                            sessionFactory = sessionFactory ?? CreateSessionFactory();
                        }
                    }
                }

                return sessionFactory;
            }
        }

        private ISessionFactory CreateSessionFactory()
        {
            return configuration.BuildSessionFactory();
        }
    }
}
