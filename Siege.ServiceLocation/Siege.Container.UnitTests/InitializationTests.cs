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
using Siege.ServiceLocation.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Initialize_Property_After_Resolution()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));

            var instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }

        [Test]
        public void Should_Initialize_Property_After_Resolution_Depending_On_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(
                Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "lulz"));

            locator.AddContext(TestEnum.Case2);

            var instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }


        [Test]
        public void Should_Initialize_Property_After_Resolution_With_No_Context()
        {
            locator
                .Register(Given<ITestInterface>.Then<TestCase1>())
                .Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"))
                .Register(Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "rofl"));

            var instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }
    }
}