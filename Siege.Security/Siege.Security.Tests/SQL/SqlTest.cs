//using System;
//using System.Web.Security;
//using FluentNHibernate.Cfg;
//using FluentNHibernate.Cfg.Db;
//using NHibernate.Tool.hbm2ddl;
//using NUnit.Framework;
//using Siege.Repository;
//using Siege.Repository.NHibernate;
//using Siege.Repository.UnitOfWork;
//using Siege.Security.Providers;
//using Siege.Security.SQL.Providers;
//using Siege.Security.SQL.Repository;
//using Siege.ServiceLocator;
//using Siege.ServiceLocator.InternalStorage;
//using Siege.ServiceLocator.Native;
//using Siege.ServiceLocator.Native.ConstructionStrategies;
//using Siege.ServiceLocator.RegistrationSyntax;
//using Siege.ServiceLocator.Registrations.Conventions;

//namespace Siege.Security.Tests.SQL
//{
//    public class SqlTest
//    {
//        protected IApplicationProvider applicationProvider;
//        protected IPermissionProvider permissionProvider;
//        protected IRoleProvider roleProvider;
//        protected IGroupProvider groupProvider;
//        protected IUserProvider userProvider;
//        protected IRepository<SecurityDatabase> repository;
//        protected IServiceLocator serviceLocator;
//        protected Application application;

//        private void BuildSchema(FluentConfiguration configuration)
//        {
//            new SchemaExport(configuration.BuildConfiguration()).Execute(false, true, false);
//        }

//        [SetUp]
//        public void SetUp()
//        {
//            var configuration = SecurityConfiguration.ConfigurationFor(SQLiteConfiguration.Standard.InMemory().Provider("Siege.Security.Tests.SqlLiteAdapter, Siege.Security.Tests"));

//            BuildSchema(configuration);

//            serviceLocator = new ThreadedServiceLocator(new SiegeAdapter(new ReflectionConstructionStrategy()));
//            serviceLocator
//                .Register(Using.Convention(new NHibernateConvention<ThreadedUnitOfWorkStore, SecurityDatabase>(configuration.BuildSessionFactory())))
//                .Register(Given<IPermissionProvider>.Then<SqlPermissionProvider>())
//                .Register(Given<IRoleProvider>.Then<Security.SQL.Providers.SqlRoleProvider>())
//                .Register(Given<IGroupProvider>.Then<SqlGroupProvider>())
//                .Register(Given<IUserProvider>.Then<SqlUserProvider>())
//                .Register(Given<IApplicationProvider>.Then<SqlApplicationProvider>())
//                .Register(Given<MembershipProvider>.Then(Membership.Provider));


//            applicationProvider = serviceLocator.GetInstance<IApplicationProvider>();
//            permissionProvider = serviceLocator.GetInstance<IPermissionProvider>();
//            roleProvider = serviceLocator.GetInstance<IRoleProvider>();
//            groupProvider = serviceLocator.GetInstance<IGroupProvider>();
//            userProvider = serviceLocator.GetInstance<IUserProvider>();
//            repository = serviceLocator.GetInstance<IRepository<SecurityDatabase>>();

//            application = new Application
//            {
//                Name = "Test Consumer",
//                Description = "Consumer for Testing Purposes"
//            };

//            var user = new User
//            {
//                Name = "Test User",
//                Consumer = application,
//                IsActive = true,
//                IsLockedOut = false,
//                Password = "pass1234",
//                ID = Guid.NewGuid()
//            };

//            var group = new Group
//            {
//                Name = "Test Group",
//                Description = "Group for Testing Purposes",
//                Consumer = application
//            };

//            var role = new Role
//            {
//                Name = "Test Role",
//                Description = "Role for Testing Purposes",
//                Consumer = application
//            };

//            var permission = new Permission
//            {
//                Name = "Test Permission",
//                Description = "Permission for Testing Purposes"
//            };

//            role.Permissions.Add(permission);
//            group.Roles.Add(role);
//            user.Groups.Add(group);

//            application = applicationProvider.Save(application);
//            roleProvider.Save(role);
//            groupProvider.Save(group);

//            repository.Save(user);
//        }

//        [TearDown]
//        public virtual void TearDown()
//        {
//            serviceLocator.GetInstance<IUnitOfWorkStore>().Dispose();
//            serviceLocator.GetInstance<IServiceLocatorStore>().Dispose();
//        }
//    }
//}