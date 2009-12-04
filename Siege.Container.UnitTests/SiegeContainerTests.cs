using System;
using NUnit.Framework;
using Siege.Container.UnitTests.RegistrationExtensions;
using Siege.Container.UnitTests.TestClasses;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public abstract class SiegeContainerTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();
        protected abstract void RegisterWithoutSiege();
        protected abstract Type GetDecoratorUseCaseBinding();

        [SetUp]
        public virtual void SetUp()
        {
            locator = new SiegeContainer(GetAdapter());
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

            Assert.IsTrue(locator.GetInstance(typeof(ITestInterface)) is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_And_Resolve_With_Get_Service()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());

            Assert.IsTrue(locator.GetService(typeof(ITestInterface)) is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsTrue(locator.GetInstance<ITestInterface>("test") is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsInstanceOfType(typeof(TestCase1), locator.GetInstance(typeof(ITestInterface), "test"));
        }

        [Test]
        public virtual void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>();
        }

        [Test]
        public virtual void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));
            locator.GetInstance<ITestInterface>("test15");
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_Based_On_Rule()
        {
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_An_Implementation()
        {
            locator.Register(Given<ITestInterface>.Then(new TestCase1()));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Be_Able_To_Bind_An_Interface_To_An_Implementation_Based_On_Rule()
        {
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then(new TestCase2()));
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Rule_When_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules()
        {
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case1).Then<TestCase1>());
            locator.AddContext(CreateContext(TestEnum.Case1));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Change_Implementation_When_Context_Is_Added()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case1).Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);

            locator.AddContext(CreateContext(TestEnum.Case2));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules_And_Default()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case1).Then<TestCase1>());
            locator.AddContext(CreateContext(TestEnum.Case1));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Not_Use_Rule_When_Not_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.AddContext(CreateContext(TestEnum.Case3));

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        [Test]
        public void Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container()
        {
            RegisterWithoutSiege();
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
            Assert.AreSame(arg, ((DependsOnInterface)resolution).Argument);
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            var instances = locator.GetAllInstances<ITestInterface>();
            
            foreach(ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
            }
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            var instances = locator.GetAllInstances(typeof(ITestInterface));

            foreach (ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
            }
        }

        [Test]
        public void Should_Be_Able_To_Extend_Registration()
        {
            locator
                .Register(Given<ITestInterface>.Then<TestCase1>())
                .Register(Given<TestCase1>
                            .When<TestEnum>(context => context == TestEnum.Case1)
                            .DecorateWith<TestCase1, TestDecorator>());

            locator.AddContext(TestEnum.Case1);

            var useCase = locator.GetInstance<TestCase1>();

            Assert.IsInstanceOfType(typeof(TestDecorator), useCase);
            Assert.IsInstanceOfType(typeof(TestCase1), ((TestDecorator)useCase).WrappedObject);
        }

        private TestContext CreateContext(TestEnum types)
        {
            return new TestContext(types);
        }
    }
}
