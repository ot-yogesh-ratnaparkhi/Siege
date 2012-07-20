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
using System.Threading;
using NUnit.Framework;
using Rhino.Mocks;
using Siege.Repository.UnitOfWork;

namespace Siege.Repository.Tests
{
    [TestFixture]
    public class ThreadedUnitOfWorkStoreTests
    {
        private MockRepository mocks;
        private IUnitOfWork instance;
        private ThreadedUnitOfWorkStore store;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            instance = MockRepository.GenerateMock<IUnitOfWork>();

            store = new ThreadedUnitOfWorkStore();
        }

        [Test]
        public void UnitOfWorkShouldBeNullInitially()
        {
            Assert.AreEqual(null, store.CurrentFor<IDatabase>());
        }

        [Test]
        public void CurrentShouldBeSameAsSetUnitOfWork()
        {
            store.SetUnitOfWork<IDatabase>(instance);

            Assert.AreEqual(instance, store.CurrentFor<IDatabase>());
        }

        [Test]
        public void DisposeShouldCallDisposeOnUnitOfWorkAndSetToNull()
        {
            using (mocks.Record())
            {
                instance.Expect(i => i.Dispose());
            }

            using (mocks.Playback())
            {
                store.SetUnitOfWork<IDatabase>(instance);
                Assert.AreEqual(instance, store.CurrentFor<IDatabase>());

                store.Dispose();
                Assert.AreEqual(null, store.CurrentFor<IDatabase>());
            }
        }

        [Test]
        public void CurrentForShouldReturnDifferentUnitsOfWorkForDifferentThreads()
        {
            var unitOfWork1 = mocks.DynamicMock<IUnitOfWork>();
            IUnitOfWork thread1UnitOfWork = null;
            Exception threadError1 = null;
            var thread1 = new Thread(() =>
                                         {
                                             try
                                             {
                                                 store.SetUnitOfWork<IDatabase>(unitOfWork1);
                                                 thread1UnitOfWork = store.CurrentFor<IDatabase>();
                                             }
                                             catch (Exception ex)
                                             {
                                                 threadError1 = ex;
                                             }
                                         });

            var unitOfWork2 = mocks.DynamicMock<IUnitOfWork>();
            IUnitOfWork thread2UnitOfWork = null;
            Exception threadError2 = null;
            var thread2 = new Thread(() =>
                                         {
                                             try
                                             {
                                                 store.SetUnitOfWork<IDatabase>(unitOfWork2);
                                                 thread2UnitOfWork = store.CurrentFor<IDatabase>();
                                             }
                                             catch (Exception ex)
                                             {
                                                 threadError2 = ex;
                                             }
                                         });

            thread1.Start();
            thread2.Start();

            thread1.Join();
            thread2.Join();

            Assert.IsNull(threadError1);
            Assert.IsNull(threadError2);

            Assert.AreNotSame(thread1UnitOfWork, thread2UnitOfWork);
            Assert.AreSame(unitOfWork1, thread1UnitOfWork);
            Assert.AreSame(unitOfWork2, thread2UnitOfWork);
        }

        [Test]
        public void DisposeShouldNotFailWhenCalledBeforeUnitOfWorkHasBeenRequested()
        {
            store.Dispose();
        }

        [Test]
        public void DisposeCanBeCalledMultipleTimes()
        {
            store.Dispose();
            store.Dispose();
        }
    }
}