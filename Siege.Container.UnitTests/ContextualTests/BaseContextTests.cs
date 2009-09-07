using NUnit.Framework;
using Siege.Container.UnitTests.ContextualTests.Classes;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.ContextualTests
{
    [TestFixture]
    public abstract class BaseContextTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();

        [SetUp]
        public virtual void SetUp()
        {
            locator = new SiegeContainer(GetAdapter());
        }

        [Test]
        public void Should_Choose_Test_Service_2()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<ITestRepository>.Then<TestRepository1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>());
            
            locator.AddContext(new TestCondition(TestTypes.Test2));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(TestService2), controller.Service);
        }

        [Test]
        public void Should_Choose_Test_Service_1()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<ITestRepository>.Then<TestRepository1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>());
            locator.AddContext(new TestCondition(TestTypes.Test1));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(TestService1), controller.Service);
            Assert.IsInstanceOfType(typeof(TestRepository1), controller.Service.Repository);
        }

        [Test]
        public void Complex_Scenario_1()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then<TestRepository1>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then<TestRepository2>());
           
            locator.AddContext(new TestCondition(TestTypes.Test1));
            locator.AddContext(new RepositoryCondition(Conditions.ConditionA));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(TestService1), controller.Service);
            Assert.IsInstanceOfType(typeof(TestRepository1), controller.Service.Repository);
        }

        [Test]
        public void Complex_Scenario_2()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then<TestRepository1>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then<TestRepository2>());

            locator.AddContext(new TestCondition(TestTypes.Test2));
            locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(TestService2), controller.Service);
            Assert.IsInstanceOfType(typeof(TestRepository2), controller.Service.Repository);
        }

        [Test]
        public void Complex_Scenario_3()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then<TestRepository1>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then<TestRepository2>())
                     .Register(Given<IBaseService>.Then<DefaultTestService>());

            locator.AddContext(new TestCondition(TestTypes.Test3));
            locator.AddContext(new RepositoryCondition(Conditions.ConditionB));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(DefaultTestService), controller.Service);
            Assert.IsInstanceOfType(typeof(TestRepository2), controller.Service.Repository);
        }

        [Test]
        public void Should_Choose_Defaults_When_No_Context_Applies()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then<TestRepository1>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then<TestRepository2>())
                     .Register(Given<IBaseService>.Then<DefaultTestService>())
                     .Register(Given<ITestRepository>.Then<DefaultTestRepository>());

            locator.AddContext(new TestCondition(TestTypes.Test3));
            locator.AddContext(new RepositoryCondition(Conditions.ConditionC));

            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(DefaultTestService), controller.Service);
            Assert.IsInstanceOfType(typeof(DefaultTestRepository), controller.Service.Repository);
        }

        [Test]
        public void Should_Choose_Defaults_When_No_Context_Provided()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionA).Then<TestRepository1>())
                     .Register(Given<ITestRepository>.When<IRepositoryCondition>(context => context.Condition == Conditions.ConditionB).Then<TestRepository2>())
                     .Register(Given<IBaseService>.Then<DefaultTestService>())
                     .Register(Given<ITestRepository>.Then<DefaultTestRepository>());


            ITestController controller = locator.GetInstance<ITestController>();
            Assert.IsInstanceOfType(typeof(DefaultTestService), controller.Service);
            Assert.IsInstanceOfType(typeof(DefaultTestRepository), controller.Service.Repository);
        }
    }
}
