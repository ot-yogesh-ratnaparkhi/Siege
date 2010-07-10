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
using Siege.ServiceLocation.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Choose_Constructor_Argument_Based_On_Type_Injected_Into()
        {
            locator
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case1)
                              .Then<DependsOnInterface>())
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case2)
                              .Then<DependsOnAlternateConstructorImplicitly>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnInterface>()
                              .Then<ConstructorArgument>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnAlternateConstructorImplicitly>()
                              .Then<AlternateConstructorArgument>());

            locator.AddContext(TestEnum.Case2);

            var instance = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOfType(typeof (DependsOnAlternateConstructorImplicitly), instance);
            Assert.IsInstanceOfType(typeof (AlternateConstructorArgument),
                                    ((DependsOnAlternateConstructorImplicitly) instance).Argument);
        }

        [Test]
        public void Should_Choose_Constructor_Argument_Based_On_Type_Injected_Into_And_Use_Instance()
        {
            var arg = new AlternateConstructorArgument();

            locator
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case1)
                              .Then<DependsOnInterface>())
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case2)
                              .Then<DependsOnAlternateConstructorImplicitly>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnInterface>()
                              .Then<ConstructorArgument>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnAlternateConstructorImplicitly>()
                              .Then(arg));

            locator.AddContext(TestEnum.Case2);

            var instance = locator.GetInstance<ITestInterface>();

            Assert.IsInstanceOfType(typeof (DependsOnAlternateConstructorImplicitly), instance);
            Assert.IsInstanceOfType(typeof (AlternateConstructorArgument),
                                    ((DependsOnAlternateConstructorImplicitly) instance).Argument);
            Assert.AreSame(arg, ((DependsOnAlternateConstructorImplicitly) instance).Argument);
        }

        [Test]
        public void Should_Construct_With_A_Factory_When_Injected_Into_Particular_Type()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, IConstructorArgument> func = container =>
            {
                factoryMethodInvoked = true;
                return new AlternateConstructorArgument();
            };

            locator
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case1)
                              .Then<DependsOnInterface>())
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case2)
                              .Then<DependsOnAlternateConstructorImplicitly>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnInterface>()
                              .Then<ConstructorArgument>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnAlternateConstructorImplicitly>()
                              .ConstructWith(func));

            locator.AddContext(TestEnum.Case2);

            locator.GetInstance<ITestInterface>();

            Assert.IsTrue(factoryMethodInvoked);
        }

        [Test]
        public void Should_Not_Construct_With_A_Factory_When_Not_Injected_Into_Particular_Type()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, IConstructorArgument> func = container =>
            {
                factoryMethodInvoked = true;
                return new AlternateConstructorArgument();
            };

            locator
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case1)
                              .Then<DependsOnInterface>())
                .Register(Given<ITestInterface>
                              .When<TestEnum>(test => test == TestEnum.Case2)
                              .Then<DependsOnAlternateConstructorImplicitly>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnInterface>()
                              .Then<ConstructorArgument>())
                .Register(Given<IConstructorArgument>
                              .WhenInjectingInto<DependsOnAlternateConstructorImplicitly>()
                              .ConstructWith(func));

            locator.AddContext(TestEnum.Case1);

            locator.GetInstance<ITestInterface>();

            Assert.IsFalse(factoryMethodInvoked);
        }
    }
}