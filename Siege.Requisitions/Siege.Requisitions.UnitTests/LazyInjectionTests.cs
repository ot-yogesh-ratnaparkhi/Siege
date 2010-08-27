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
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.InternalStorage;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public abstract partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldInjectLazilyForDefaultRegistrations()
        {
            locator
                .Register(Given<TestClass>.Then<TestClass>())
                .Register(Given<ITestInterface>.Then<TestCase1>());

            var instance = locator.GetInstance<TestClass>();

            Assert.IsInstanceOf<TestCase1>(instance.Invoke());
        }

        [Test]
        public void ShouldInjectLazilyForDefaultInstanceRegistrations()
        {
            var sample = new TestCase1();

            locator
                .Register(Given<ITestInterface>.Then(sample))
                .Register(Given<TestClass>.Then<TestClass>());

            var instance = locator.GetInstance<TestClass>();

            Assert.AreSame(sample, instance.Invoke());
        }

        [Test]
        public void ShouldInjectLazilyForConditionalRegistrations()
        {
            locator
                .Register(Given<ITestInterface>.When<int>(i => i == 1).Then<TestCase1>())
                .Register(Given<ITestInterface>.When<int>(i => i == 2).Then<TestCase2>())
                .Register(Given<TestClass>.Then<TestClass>());

            locator.AddContext(1);
            var instance = locator.GetInstance<TestClass>();
            Assert.IsInstanceOf<TestCase1>(instance.Invoke());

            locator.Store.Get<IContextStore>().Clear();
            locator.AddContext(2);

            instance = locator.GetInstance<TestClass>();
            Assert.IsInstanceOf<TestCase2>(instance.Invoke());
        }

        [Test]
        public void ShouldInjectLazilyForConditionalInstanceRegistrations()
        {
            var sample1 = new TestCase1();
            var sample2 = new TestCase2();

            locator
                .Register(Given<ITestInterface>.When<int>(i => i == 1).Then(sample1))
                .Register(Given<ITestInterface>.When<int>(i => i == 2).Then(sample2))
                .Register(Given<TestClass>.Then<TestClass>());

            locator.AddContext(1);
            var instance = locator.GetInstance<TestClass>();
            Assert.AreSame(sample1, instance.Invoke());

            locator.Store.Get<IContextStore>().Clear();
            locator.AddContext(2);

            instance = locator.GetInstance<TestClass>();
            Assert.AreSame(sample2, instance.Invoke());
        }

        [Test]
        public void ShouldInjectLazilyForFactoryMethods()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator
                .Register(Given<ITestInterface>.ConstructWith(func))
                .Register(Given<TestClass>.Then<TestClass>());

            var instance = locator.GetInstance<TestClass>();
            Assert.IsInstanceOf<TestCase1>(instance.Invoke());
            Assert.IsTrue(factoryMethodInvoked);
        }
    }

    public class TestClass
    {
        private readonly Func<ITestInterface> test;

        public TestClass(Func<ITestInterface> test)
        {
            this.test = test;
        }

        public ITestInterface Invoke()
        {
            return test();
        }
    }
}