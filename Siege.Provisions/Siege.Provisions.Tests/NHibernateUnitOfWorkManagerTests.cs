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
using Siege.Provisions.NHibernate;
using Siege.Provisions.UnitOfWork;

namespace Siege.Provisions.Tests
{
    [TestFixture]
    public class NHibernateUnitOfWorkManagerTests
    {
        private MockRepository mocks;
        private IUnitOfWorkStore store;
        private NHibernateUnitOfWorkFactory factory;
        private ISessionFactory sessionFactory;
        private ISession session;
        private NHibernateUnitOfWorkManager unitOfWorkManager;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            session = mocks.DynamicMock<ISession>();
            sessionFactory = mocks.DynamicMock<ISessionFactory>();
            store = new ThreadedUnitOfWorkStore();
            factory = mocks.Stub<NHibernateUnitOfWorkFactory>(sessionFactory);
            unitOfWorkManager = mocks.Stub<NHibernateUnitOfWorkManager>();
            unitOfWorkManager.Add(new NullDatabase(factory));
        }

        [Test]
        public void CurrentSessionCallsCurrentUnitOfWork()
        {
            using (mocks.Record())
            {
                sessionFactory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
            }

            using (mocks.Playback())
            {
                var instance = unitOfWorkManager.SessionFor<NullDatabase>();
            }
        }

        [Test]
        public void UnitOfWorkFactoryDisposeCallsSessionFactoryDispose()
        {
            using (mocks.Record())
            {
                sessionFactory.Expect(f => f.Dispose());
            }

            using (mocks.Playback())
            {
                factory.Dispose();
            }
        }

        [TearDown]
        public void TearDown()
        {
            mocks.BackToRecordAll();
            store.Dispose();
        }
    }
}