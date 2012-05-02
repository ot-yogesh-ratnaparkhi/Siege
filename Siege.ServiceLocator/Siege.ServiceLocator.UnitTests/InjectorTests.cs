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
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.UnitTests.TestClasses;

namespace Siege.ServiceLocator.UnitTests
{
    [TestFixture]
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldChooseConstructorArgumentBasedOnTypeInjectedInto()
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

            Assert.IsInstanceOf<DependsOnAlternateConstructorImplicitly>(instance);
            Assert.IsInstanceOf<AlternateConstructorArgument>(((DependsOnAlternateConstructorImplicitly)instance).Argument);
        }

        [Test]
        public void ShouldChooseConstructorArgumentBasedOnTypeInjectedIntoAndUseInstance()
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

            Assert.IsInstanceOf<DependsOnAlternateConstructorImplicitly>(instance);
            Assert.IsInstanceOf<AlternateConstructorArgument>(((DependsOnAlternateConstructorImplicitly)instance).Argument);
            Assert.AreSame(arg, ((DependsOnAlternateConstructorImplicitly) instance).Argument);
        }

        [Test]
        public void ShouldConstructWithAFactoryWhenInjectedIntoParticularType()
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
        public void ShouldNotConstructWithAFactoryWhenNotInjectedIntoParticularType()
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