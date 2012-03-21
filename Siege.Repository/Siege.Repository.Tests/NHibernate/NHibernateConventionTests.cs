/*   Copyright 2009 - 2010 Marcus Bratton

     Licensed under the Apache License, Version 2.0 (the "License");
     you may not use this file except in compliance with the License.
     You may obtain a copy of the License at

     http://www.apache.org/licenses/LICENSE-2.0

     Unless required by applicable law or agreed to in writing, software
     distributed under the License is distributed on an "AS IS" BASIS,
     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
     See the License for the specific language governing permissions and
     limitations under the License.
*/

using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using Siege.Repository.NHibernate;
using Siege.Repository.UnitOfWork;
using Siege.ServiceLocator;
using Siege.ServiceLocator.Native;
using Siege.ServiceLocator.Registrations.Conventions;

namespace Siege.Repository.Tests
{
    public class NullDatabase : Database<NullDatabase>
    {
    	public NullDatabase(IUnitOfWorkFactory<NullDatabase> unitOfWorkFactory, IUnitOfWorkStore unitOfWorkStore) : base(unitOfWorkFactory, unitOfWorkStore)
    	{
    	}
    }

    [TestFixture]
    public class NHibernateConventionTests
    {
        private MockRepository mocks;
        private ISessionFactory sessionFactory;
        private IServiceLocator serviceLocator;

        [SetUp]
        public void SetUp()
        {
            serviceLocator = new ThreadedServiceLocator(new SiegeAdapter());

            mocks = new MockRepository();
            sessionFactory = mocks.DynamicMock<ISessionFactory>();

            serviceLocator.Register(
                Using.Convention(new NHibernateConvention<ThreadedUnitOfWorkStore, NullDatabase>(sessionFactory)));
        }
		
        [Test]
        public void ShouldRegisterUnitOfWorkFactory()
        {
			var uowFactory = serviceLocator.GetInstance<NHibernateUnitOfWorkFactory<NullDatabase>>();
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
            var uowManager = serviceLocator.GetInstance<NHibernateUnitOfWorkManager>();
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

        [TearDown]
        public void TearDown()
        {
            serviceLocator.GetInstance<IUnitOfWorkStore>().Dispose();
        }
    }
}