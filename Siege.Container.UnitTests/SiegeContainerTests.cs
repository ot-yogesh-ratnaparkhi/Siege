using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public abstract class SiegeContainerTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IContextualServiceLocator GetAdapter();
        protected abstract void RegisterWithoutSiege();

        [SetUp]
        public void SetUp()
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
        public void Should_Be_Able_To_Bind_An_Interface_To_A_Type_Based_On_Rule()
        {
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case2)) is TestCase2);
        }

        [Test]
        public void Should_Use_Rule_When_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case2)) is TestCase2);
        }

        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules()
        {
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case1).Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case1)) is TestCase1);
        }


        [Test]
        public void Should_Use_Correct_Rule_Given_Multiple_Rules_And_Default()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case1).Then<TestCase1>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case1)) is TestCase1);
        }

        [Test]
        public void Should_Not_Use_Rule_When_Not_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case3)) is TestCase1);
        }

        [Test]
        public void Should_Resolve_If_Exists_In_IoC_But_Not_Registered_In_Container()
        {
            RegisterWithoutSiege();
            Assert.IsTrue(locator.GetInstance<IUnregisteredInterface>() is UnregisteredClass);
        }

        private Context<TestContext> CreateContext(TestEnum types)
        {
            return new Context<TestContext> { Value = new TestContext { TestCases = types } };
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
    public class TestCase1 : ITestInterface {}
    public class TestCase2 : ITestInterface {}
    public class UnregisteredClass : IUnregisteredInterface {}
}
