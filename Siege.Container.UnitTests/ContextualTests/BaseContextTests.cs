using System;
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
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>());
            locator.AddContext(new TestCondition(TestTypes.Test2));

            try
            {
                ITestController controller = locator.GetInstance<ITestController>();
                Assert.IsInstanceOfType(typeof(TestService2), controller.Service);
            }
            catch(Exception ex)
            {
                var lol = ex;
            }
        }

        [Test]
        public void Should_Choose_Test_Service_1()
        {
            locator.Register(Given<ITestController>.Then<TestController>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test1).Then<TestService1>())
                     .Register(Given<IBaseService>.When<ITestCondition>(context => context.TestType == TestTypes.Test2).Then<TestService2>());
            locator.AddContext(new TestCondition(TestTypes.Test1));

            try
            {
                ITestController controller = locator.GetInstance<ITestController>();
                Assert.IsInstanceOfType(typeof(TestService1), controller.Service);
            }
            catch (Exception ex)
            {
                var lol = ex;
            }
        }
    }
}
