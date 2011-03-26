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

namespace Siege.Provisions.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
        private MockRepository mocks;
        private IUnitOfWorkManager unitOfWorkManager;
        private Repository<NullDatabase> repository;

        [SetUp]
        public void SetUp()
        {
            mocks = new MockRepository();
            unitOfWorkManager = mocks.DynamicMock<IUnitOfWorkManager>();
            repository = new Repository<NullDatabase>(unitOfWorkManager);
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnGet()
        {
            using (mocks.Record())
            {
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>()).Repeat.Any();
            }

            using (mocks.Playback())
            {
                repository.Get<object>(1);
            }
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnSave()
        {
            using (mocks.Record())
            {
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>());
            }

            using (mocks.Playback())
            {
                repository.Save<object>(1);
            }
        }

        [Test]
        public void ShouldCallUnitOfWorkManagerCurrentOnDelete()
        {
            using (mocks.Record())
            {
                unitOfWorkManager.Expect(uow => uow.For<NullDatabase>());
            }

            using (mocks.Playback())
            {
                repository.Delete<object>(1);
            }
        }
    }
}