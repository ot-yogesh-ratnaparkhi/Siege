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
using Siege.ServiceLocation.Exceptions;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_Based_On_Rule()
        {
            locator
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                            .Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_An_Implementation_Based_On_Rule()
        {
            locator
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                            .Then(new TestCase2()));
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Rule_When_Satisfied()
        {
            locator
                .Register(Given<ITestInterface>.Then<TestCase1>())
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                            .Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules()
        {
            locator
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                            .Then<TestCase2>())
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case1)
                            .Then<TestCase1>());
            locator.AddContext(CreateContext(TestEnum.Case1));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Change_Implementation_When_Context_Is_Added()
        {
            locator
                .Register(Given<ITestInterface>.Then<TestCase1>())
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                            .Then<TestCase2>())
                .Register(Given<ITestInterface>
                            .When<TestContext>(context => context.TestCases == TestEnum.Case1)
                            .Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);

            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules_And_Default()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                                .Then<TestCase2>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case1)
                                .Then<TestCase1>());
            locator.AddContext(CreateContext(TestEnum.Case1));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test, ExpectedException(typeof(RegistrationNotFoundException))]
        public void Should_Throw_Exception_When_Type_No_Default_Specified_And_No_Rules_Match()
        {
            locator.Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                                .Then<TestCase2>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case1)
                                .Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Not_Use_Rule_When_Not_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                                .Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case3));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                                .Then<TestCase2>());

            var instances = locator.GetAllInstances<ITestInterface>();

            foreach (ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
            }
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>())
                   .Register(Given<ITestInterface>
                                .When<TestContext>(context => context.TestCases == TestEnum.Case2)
                                .Then<TestCase2>());

            var instances = locator.GetAllInstances(typeof(ITestInterface));

            foreach (ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
            }
        }
    }
}