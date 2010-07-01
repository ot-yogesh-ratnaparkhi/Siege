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
using Siege.ServiceLocation.RhinoMocksAdapter;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public partial class SiegeContainerTests
    {
        [Test]
        public void Should_AutoMock_All_Dependencies()
        {
            var repository = new MockRepository();

            locator.Register(RhinoMock<DependsOnMultipleInterfaceTypes>.Using(repository));

            Assert.IsNotNull(locator.GetInstance<IConstructorArgument>());
            Assert.IsNotNull(locator.GetInstance<IServiceLocator>());
            Assert.IsNotNull(locator.GetInstance<DependsOnMultipleInterfaceTypes>());
        }

        [Test]
        public void Should_AutoStub_All_Dependencies()
        {
            var repository = new MockRepository();

            locator.Register(RhinoMock<MultiConstructorType>.Using(repository));

            Assert.IsNotNull(locator.GetInstance<TypeA>());
            Assert.IsNotNull(locator.GetInstance<TypeB>());
            Assert.IsNotNull(locator.GetInstance<TypeC>());
            Assert.IsNotNull(locator.GetInstance<TypeD>());
            Assert.IsNotNull(locator.GetInstance<TypeE>());
            Assert.IsNotNull(locator.GetInstance<MultiConstructorType>());
        }
    }
}