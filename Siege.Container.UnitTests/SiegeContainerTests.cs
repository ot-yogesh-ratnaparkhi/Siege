using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public abstract class SiegeContainerTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();
        protected abstract void RegisterWithoutSiege();

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
        public void Should_Pass_Dictionary_As_A_Constructor_Argument()
        {
            IDictionary dictionary = new Dictionary<string, IConstructorArgument>();
            dictionary.Add("argument", new ConstructorArgument());

            locator.Register(Given<ITestInterface>.Then<TestCase4>());
            Assert.IsTrue(locator.GetInstance<ITestInterface>(dictionary) is TestCase4);
        }

        [Test]
        public void Should_Pass_Anonymous_Type_As_A_Constructor_Argument()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase4>());
            Assert.IsTrue(locator.GetInstance<ITestInterface>(new { argument = new ConstructorArgument() }) is TestCase4);
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

            DependsOnInterface resolution = (DependsOnInterface)locator.GetInstance<ITestInterface>();

            Assert.IsTrue(resolution is DependsOnInterface);
            Assert.AreSame(arg, resolution.Argument);
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            var instances = locator.GetAllInstances<ITestInterface>();

            int count = 0;
            foreach(ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
                count++;
            }

            //Assert.AreEqual(2, count);
        }

        [Test]
        public void Should_Resolve_All_From_Service_Locator_Regardless_Of_Context_Non_Generic()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            var instances = locator.GetAllInstances(typeof(ITestInterface));

            int count = 0;
            foreach (ITestInterface item in instances)
            {
                Assert.IsInstanceOfType(typeof(ITestInterface), item);
                count++;
            }

            //Assert.AreEqual(2, count);
        }

        private TestContext CreateContext(TestEnum types)
        {
            return new TestContext(types);
        }
    }

    public class TestContext
    {
        public TestContext(TestEnum context)
        {
            TestCases = context;
        }

        public TestEnum TestCases { get; set; }
    }

    public enum TestEnum
    {
        Case1,
        Case2,
        Case3
    }

    public interface IUnregisteredInterface {}
    public interface ITestInterface {}
    public interface IConstructorArgument {}
    public class TestCase1 : ITestInterface {}
    public class TestCase2 : ITestInterface {}
    public class UnregisteredClass : IUnregisteredInterface {}
    public class ConstructorArgument : IConstructorArgument {}
    public class TestCase4 : ITestInterface
    {
        public TestCase4(IConstructorArgument argument) {}
    }

    public class DependsOnIServiceLocator : ITestInterface
    {
        public DependsOnIServiceLocator(IServiceLocator locator)
        {
        }
    }

    public class DependsOnInterface : ITestInterface
    {
        private readonly IConstructorArgument argument;

        public DependsOnInterface(IConstructorArgument argument)
        {
            this.argument = argument;
        }

        public IConstructorArgument Argument
        {
            get { return argument; }
        }
    }
}
