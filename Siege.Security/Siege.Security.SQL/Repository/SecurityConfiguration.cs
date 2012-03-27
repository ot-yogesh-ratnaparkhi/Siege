//using FluentNHibernate.Cfg;
//using FluentNHibernate.Cfg.Db;
//using NHibernate;
//using Siege.Security.SQL.Conventions;
//using Siege.Security.SQL.Mappings;
//namespace Siege.Security.SQL.Repository
//{
//    public class SecurityConfiguration
//    {
//        private static volatile ISessionFactory sessionFactory;
//        private static readonly object configLock = new object();
//        private static readonly object sessionLock = new object();
//        private static volatile FluentConfiguration configuration;

//        public static ISessionFactory SessionFactoryFor(string configurationName, IPersistenceConfigurer configurer)
//        {
//            if (sessionFactory == null)
//            {
//                lock (sessionLock)
//                {
//                    if (sessionFactory == null)
//                    {
//                        sessionFactory = ConfigurationFor(configurer).BuildSessionFactory();
//                    }
//                }
//            }

//            return sessionFactory;
//        }

//        public static ISessionFactory SessionFactoryFor(string configurationName)
//        {
//            if (sessionFactory == null)
//            {
//                lock (sessionLock)
//                {
//                    if (sessionFactory == null)
//                    {
//                        sessionFactory = ConfigurationFor(MsSqlConfiguration.MsSql2005.ConnectionString(c => c.FromConnectionStringWithKey(configurationName))).BuildSessionFactory();
//                    }
//                }
//            }

//            return sessionFactory;
//        }

//        public static FluentConfiguration ConfigurationFor(IPersistenceConfigurer configurer)
//        {
//            if (configuration == null)
//            {
//                lock (configLock)
//                {
//                    if (configuration == null)
//                    {
//                        configuration = Fluently.Configure()
//                            .Database(configurer)
//                            .Mappings(m => m.FluentMappings
//                                .Add<ApplicationMap>()
//                                .Add<GroupMap>()
//                                .Add<PermissionMap>()
//                                .Add<RoleMap>()
//                                .Add<UserMap>()
//                            .Conventions
//                                .AddFromAssemblyOf<SecurityClassConvention>());
//                    }
//                }
//            }

//            return configuration;
//        }
//    }
//}