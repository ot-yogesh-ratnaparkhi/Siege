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
using Siege.ServiceLocator.Exceptions;
using Siege.ServiceLocator.RegistrationSyntax;
using Siege.ServiceLocator.UnitTests.TestClasses;

namespace Siege.ServiceLocator.UnitTests
{
    [TestFixture]
    public abstract partial class ServiceLocatorTests
    {
        protected IServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();
        protected abstract void RegisterWithoutSiege<TFrom, TTo>()
            where TTo : TFrom
            where TFrom : class;
		protected abstract void ResolveWithoutSiege<T>();

        [SetUp]
        public virtual void SetUp()
        {
            locator = new ThreadedServiceLocator(GetAdapter());
        }

        [TearDown]
        public void TearDown()
        {
            locator.Dispose();
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToAType()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>());
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToATypeNonGeneric()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance(typeof(ITestInterface)));
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToATypeAndResolveWithGetService()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsInstanceOf<TestCase1>(locator.GetService(typeof(ITestInterface)));
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToATypeWithAName()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>("test"));
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToATypeWithANameNonGeneric()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance(typeof(ITestInterface), "test"));
        }

        [Test, ExpectedException(typeof (RegistrationNotFoundException))]
        public virtual void ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenNoNameProvided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>();
        }

        [Test, ExpectedException(typeof (RegistrationNotFoundException))]
        public virtual void ShouldNotBeAbleToBindAnInterfaceToATypeWithANameWhenWrongNameProvided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>("test15");
        }

        [Test]
        public virtual void ShouldDistinguishImplementationsBasedOnName()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.Register(Given<ITestInterface>.Then<TestCase2>("test2"));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>("test"));
            Assert.IsInstanceOf<TestCase2>(locator.GetInstance<ITestInterface>("test2"));
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToAnImplementation()
        {
            locator.Register(Given<ITestInterface>.Then(new TestCase1()));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>());
        }

        [Test]
        public void ShouldBeAbleToBindAnInterfaceToAnImplementationWithAName()
        {
            locator.Register(Given<ITestInterface>.Then("Test", new TestCase1()));

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>("Test"));
        }

        
        [Test]
        public virtual void ShouldResolveIfExistsInIoCButNotRegisteredInContainer()
        {
			RegisterWithoutSiege<IUnregisteredInterface, UnregisteredClass>();
            Assert.IsInstanceOf<UnregisteredClass>(locator.GetInstance<IUnregisteredInterface>());
        }

        [Test]
        public void ShouldResolveIfDependsOnIServiceLocator()
        {
            locator.Register(Given<ITestInterface>.Then<DependsOnIServiceLocator>());
            Assert.IsTrue(locator.GetInstance<ITestInterface>() is DependsOnIServiceLocator);
        }

        [Test]
        public void ShouldResolveIfDependencyIsRegisteredAsInstance()
        {
            var arg = new ConstructorArgument();

            locator
                .Register(Given<ITestInterface>.Then<DependsOnInterface>())
                .Register(Given<IConstructorArgument>.Then(arg));

            var resolution = locator.GetInstance<ITestInterface>();

            Assert.IsTrue(resolution is DependsOnInterface);
            Assert.AreSame(arg, ((DependsOnInterface) resolution).Argument);
        }
        
        private static TestClasses.TestContext CreateContext(TestEnum types)
        {
            return new TestClasses.TestContext(types);
        }
    }
}