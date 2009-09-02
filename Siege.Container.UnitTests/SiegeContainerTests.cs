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
        protected abstract IServiceLocator GetAdapter();
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
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>("test"));

            Assert.IsTrue(locator.GetInstance<ITestInterface>("test") is TestCase1);
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

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case2)) is TestCase2);
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

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case2)) is TestCase2);
        }

        [Test]
        public void Should_Use_Rule_When_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case2)) is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules()
        {
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case1).Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case1)) is TestCase1);
        }


        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules_And_Default()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case1).Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case1)) is TestCase1);
        }

        [Test]
        public void Should_Not_Use_Rule_When_Not_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<TestContext>(context => context.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, TestContext>(CreateContext(TestEnum.Case3)) is TestCase1);
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

        private TestContext CreateContext(TestEnum types)
        {
            return new TestContext { TestCases = types };
        }
    }

    public class TestContext
    {
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
}
