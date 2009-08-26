using Ninject;
using NUnit.Framework;
using Siege.Container;
using Siege.Container.NinjectAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class SiegeContainerTests
    {
        protected IKernel kernel;
        protected IContextualServiceLocator locator;

        [SetUp]
        public void SetUp()
        {
            kernel = new StandardKernel();

            locator = new SiegeContainer(new NinjectServiceLocator(kernel));
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
        public void Should_Not_Use_Rule_When_Not_Satisfied()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<ITestInterface>.When<Context<TestContext>>(context => context.Value.TestCases == TestEnum.Case2).Then<TestCase2>());

            Assert.IsTrue(locator.GetInstance<ITestInterface, Context<TestContext>>(CreateContext(TestEnum.Case3)) is TestCase1);
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

    public interface ITestInterface {}
    public class TestCase1 : ITestInterface {}
    public class TestCase2 : ITestInterface {}
}
