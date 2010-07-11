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
using Siege.Requisitions.Exceptions;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.InternalStorage;

namespace Siege.Requisitions.UnitTests
{
	[TestFixture]
    public abstract partial class SiegeContainerTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();
		protected abstract void RegisterWithoutSiege<TFrom, TTo>() where TTo : TFrom;
		protected abstract void ResolveWithoutSiege<T>();

        [SetUp]
        public virtual void SetUp()
        {
            locator = new SiegeContainer(GetAdapter(), new ThreadedServiceLocatorStore());
        }

        [TearDown]
        public void TearDown()
        {
            locator.Dispose();
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance(typeof (ITestInterface)) is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_And_Resolve_With_Get_Service()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsTrue(locator.GetService(typeof (ITestInterface)) is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

			Assert.IsInstanceOfType(typeof(TestCase1), locator.GetInstance<ITestInterface>("test"));
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsInstanceOfType(typeof (TestCase1), locator.GetInstance(typeof (ITestInterface), "test"));
        }

        [Test, ExpectedException(typeof (RegistrationNotFoundException))]
        public virtual void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>();
        }

        [Test, ExpectedException(typeof (RegistrationNotFoundException))]
        public virtual void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>("test15");
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_An_Implementation()
        {
            locator.Register(Given<ITestInterface>.Then(new TestCase1()));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_An_Implementation_With_A_Name()
        {
            locator.Register(Given<ITestInterface>.Then("Test", new TestCase1()));

            Assert.IsTrue(locator.GetInstance<ITestInterface>("Test") is TestCase1);
        }

        
        [Test]
        public virtual void Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container()
        {
			RegisterWithoutSiege<IUnregisteredInterface, UnregisteredClass>();
            Assert.IsTrue(locator.GetInstance<IUnregisteredInterface>() is UnregisteredClass);
        }

        [Test]
        public void Should_Resolve_If_Depends_On_IServiceLocator()
        {
            locator.Register(Given<ITestInterface>.Then<DependsOnIServiceLocator>());
            Assert.IsTrue(locator.GetInstance<ITestInterface>() is DependsOnIServiceLocator);
        }

        [Test]
        public void Should_Resolve_If_Dependency_Is_Registered_As_Instance()
        {
            var arg = new ConstructorArgument();

            locator
                .Register(Given<ITestInterface>.Then<DependsOnInterface>())
                .Register(Given<IConstructorArgument>.Then(arg));

            var resolution = locator.GetInstance<ITestInterface>();

            Assert.IsTrue(resolution is DependsOnInterface);
            Assert.AreSame(arg, ((DependsOnInterface) resolution).Argument);
        }

        private TestContext CreateContext(TestEnum types)
        {
            return new TestContext(types);
        }
    }
}