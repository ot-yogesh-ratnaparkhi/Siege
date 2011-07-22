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
            instance = mocks.DynamicMock<IUnitOfWork>();

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
    }
}