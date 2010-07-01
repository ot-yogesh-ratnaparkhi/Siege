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
using NUnit.Framework;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Construct_With_A_Factory()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator.Register(Given<ITestInterface>.ConstructWith(func));

            locator.GetInstance<ITestInterface>();

            Assert.IsTrue(factoryMethodInvoked);
        }

        [Test]
        public void Should_Construct_With_A_Factory_When_Context_Is_Satisfied()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator.Register(Given<ITestInterface>
                                .When<TestEnum>(test => test == TestEnum.Case1)
                                .ConstructWith(func));

            locator.AddContext(TestEnum.Case1);

            locator.GetInstance<ITestInterface>();

            Assert.IsTrue(factoryMethodInvoked);
        }

        [Test]
        public void Should_Not_Construct_With_A_Factory_When_Context_Is_Satisfied()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator
                .Register(Given<ITestInterface>.Then<TestCase2>())
                .Register(Given<ITestInterface>
                                .When<TestEnum>(test => test == TestEnum.Case1)
                                .ConstructWith(func));

            locator.AddContext(TestEnum.Case2);

            locator.GetInstance<ITestInterface>();

            Assert.IsFalse(factoryMethodInvoked);
        }
    }
}