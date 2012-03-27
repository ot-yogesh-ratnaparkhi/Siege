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

using System;
using NHibernate;
using NUnit.Framework;
using Rhino.Mocks;
using Siege.Repository.NHibernate;

namespace Siege.Repository.Tests
{
    [TestFixture]
    public class NHibernateUnitOfWorkTests
    {
        private MockRepository mocks;
        private ISessionFactory factory;
        private ISession session;
        private NHibernateUnitOfWork unitOfWork;
        private ITransaction transaction;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            factory = mocks.DynamicMock<ISessionFactory>();
            session = mocks.DynamicMock<ISession>();
            transaction = mocks.DynamicMock<ITransaction>();
            unitOfWork = mocks.Stub<NHibernateUnitOfWork>(factory);
        }

        [Test]
        public void ShouldCreateASessionWhenOneDoesntExist()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession());
            }

            using (mocks.Playback())
            {
                var localSession = unitOfWork.Session;
            }
        }

        [Test]
        public void ShouldCreateASessionWhenOneDoesntExistOnlyOnce()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Once();
            }

            using (mocks.Playback())
            {
                var localSession = unitOfWork.Session;
                var localSession2 = unitOfWork.Session;

                Assert.AreEqual(localSession, localSession2);
            }
        }

        [Test]
        public void SaveShouldOpenTransactionAndCallSessionSaveOrUpdate()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Once();
                session.Expect(s => s.SaveOrUpdate(1));
                transaction.Expect(t => t.Commit()).Repeat.Once();
            }

            using (mocks.Playback())
            {
                unitOfWork.Save(new object());
            }
        }

        [Test]
        public void SaveShouldCallSessionSaveOrUpdateWithoutOpeningATransaction()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(true).Repeat.Any();
                session.Expect(s => s.SaveOrUpdate(1));
            }

            using (mocks.Playback())
            {
                unitOfWork.Save(new object());
            }
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public void SaveShouldRollbackTransactionAndCloseThenDisposeOnException()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Once();
                session.Expect(s => s.SaveOrUpdate(1)).Throw(new Exception());
                transaction.Expect(t => t.Rollback()).Repeat.Any();

                session.Expect(s => s.Close()).Return(null);
                session.Expect(s => s.Dispose());
            }

            using (mocks.Playback())
            {
                unitOfWork.Save(new object());
            }
        }

        [Test]
        public void GetShouldOpenTransactionAndCallSessionGet()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Once();
                transaction.Expect(t => t.Commit()).Repeat.Once();
                session.Expect(s => s.Get<object>(1)).Return(null);
            }

            using (mocks.Playback())
            {
                unitOfWork.Get<object>(1);
            }
        }

        [Test]
        [ExpectedException(typeof (Exception))]
        public void GetShouldRollbackTransactionAndCloseThenDisposeOnException()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(false).Repeat.Any();
                session.Expect(s => s.BeginTransaction()).Return(transaction).Repeat.Once();
                transaction.Expect(t => t.Commit()).Repeat.Once();
                session.Expect(s => s.Get<object>(1)).Throw(new Exception());

                session.Expect(s => s.Close()).Return(null);
                session.Expect(s => s.Dispose());
            }

            using (mocks.Playback())
            {
                unitOfWork.Get<object>(1);
            }
        }

        [Test]
        public void GetShouldCallSessionGetWithoutOpeningATransaction()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Any();
                session.Expect(s => s.Transaction).Return(transaction).Repeat.Any();
                transaction.Expect(t => t.IsActive).Return(true).Repeat.Any();
                session.Expect(s => s.Get<object>(1)).Return(null);
            }

            using (mocks.Playback())
            {
                unitOfWork.Get<object>(1);
            }
        }

        [Test]
        public void DisposeShouldCallSessionDisposeAndSetToNull()
        {
            using (mocks.Record())
            {
                factory.Expect(f => f.OpenSession()).Return(session).Repeat.Twice();
                session.Expect(s => s.Close());
                session.Expect(s => s.Dispose());
            }

            using (mocks.Playback())
            {
                var localSession = unitOfWork.Session;
                unitOfWork.Dispose();

                var localSession2 = unitOfWork.Session;
            }
        }
    }
}