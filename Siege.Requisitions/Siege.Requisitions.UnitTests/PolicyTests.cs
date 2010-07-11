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
using Siege.Requisitions.UnitTests.TestClasses;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.RegistrationPolicies;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Registering_Transient_Should_Return_New_Item()
        {
            locator.Register<Transient>(Given<ITestInterface>.Then<TestCase1>());

            var firstInstance = locator.GetInstance<ITestInterface>();
            var secondInstance = locator.GetInstance<ITestInterface>();

            Assert.AreNotSame(firstInstance, secondInstance);
        }

        [Test]
        public void Registering_Singleton_Should_Not_Return_New_Item()
        {
            locator.Register<Singleton>(Given<ITestInterface>.Then<TestCase1>());

            var firstInstance = locator.GetInstance<ITestInterface>();
            var secondInstance = locator.GetInstance<ITestInterface>();

            Assert.AreSame(firstInstance, secondInstance);
        }
    }
}