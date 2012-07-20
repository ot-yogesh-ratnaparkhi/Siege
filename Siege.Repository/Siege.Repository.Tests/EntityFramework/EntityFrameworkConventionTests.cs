using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using NUnit.Framework;
using Siege.Repository.EntityFramework;
using Siege.Repository.UnitOfWork;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.Registrations.Conventions;
using Siege.ServiceLocator.Resolution;

namespace Siege.Repository.Tests.EntityFramework
{
    public class EntityFrameworkConventionTests
    {
        private IServiceLocator serviceLocator;
        private TestConnection testConnection;

        [SetUp]
        public void SetUp()
        {
            testConnection = new TestConnection();
            serviceLocator = new ThreadedServiceLocator(new SiegeAdapter())
                .Register(Given<Func<DbContext>>.Then(() => new DbContext(testConnection, true)))
//                .Register(Given<DbContext>.ConstructWith(new List<IResolutionArgument>() { new ConstructorParameter() { Name = "nameOrConnectionString", Value = "MyConnectionString" } }))
                .Register(Using.Convention(new EntityFrameworkConvention<ThreadedUnitOfWorkStore, NullDatabase, DbContext>()));
        }

        [Test]
        public void ShouldRegisterUnitOfWorkFactory()
        {
            var uowFactory = serviceLocator.GetInstance<EntityFrameworkUnitOfWorkFactory<DbContext, NullDatabase>>();
            Assert.IsNotNull(uowFactory);
        }

        [Test]
        public void ShouldRegisterRepository()
        {
            var repository = serviceLocator.GetInstance<IRepository<NullDatabase>>();
            Assert.IsNotNull(repository);
            Assert.IsInstanceOf<Repository<NullDatabase>>(repository);
        }

        [Test]
        public void ShouldRegisterUnitOfWorkManager()
        {
            var uowManager = serviceLocator.GetInstance<EntityFrameworkUnitOfWorkManager>();
            Assert.IsNotNull(uowManager);
        }

        [Test]
        public void ShouldRegisterUnitOfWork()
        {
            var unitOfWork = serviceLocator.GetInstance<IUnitOfWork>();
            Assert.IsNotNull(unitOfWork);
        }

        [Test]
        public void ShouldRegisterUnitOfWorkStoreOfGivenType()
        {
            var unitOfWorkStore = serviceLocator.GetInstance<IUnitOfWorkStore>();
            Assert.IsNotNull(unitOfWorkStore);
            Assert.IsTrue(unitOfWorkStore is ThreadedUnitOfWorkStore);
        }

        [Test]
        public void ShouldRegisterDbContext()
        {
            var dbContext = serviceLocator.GetInstance<Func<DbContext>>().Invoke();

            Assert.IsNotNull(dbContext);
            Assert.AreSame(testConnection.ConnectionString, dbContext.Database.Connection.ConnectionString);
        }

        [TearDown]
        public void TearDown()
        {
            serviceLocator.GetInstance<IUnitOfWorkStore>().Dispose();
        }
         
    }
    
    public class NullDatabase : Database<NullDatabase>
    {
        public NullDatabase(IUnitOfWorkFactory<NullDatabase> unitOfWorkFactory, IUnitOfWorkStore unitOfWorkStore)
            : base(unitOfWorkFactory, unitOfWorkStore)
        {
        }
    }

    public class TestConnection : DbConnection
    {
        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            throw new NotImplementedException();
        }

        public override void Close()
        {
            throw new NotImplementedException();
        }

        public override void ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        protected override DbCommand CreateDbCommand()
        {
            throw new NotImplementedException();
        }

        public override void Open()
        {
            throw new NotImplementedException();
        }

        public override string ConnectionString
        {
            get { return "TestConnectionString"; }
            set { }
        }

        public override string Database
        {
            get { throw new NotImplementedException(); }
        }

        public override ConnectionState State
        {
            get { throw new NotImplementedException(); }
        }

        public override string DataSource
        {
            get { throw new NotImplementedException(); }
        }

        public override string ServerVersion
        {
            get { throw new NotImplementedException(); }
        }
    }
}