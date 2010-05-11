using System;
using NUnit.Framework;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Construct_With_A_Factory()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator.Register(Given<ITestInterface>.ConstructWith(func));

            locator.GetInstance<ITestInterface>();

            Assert.IsTrue(factoryMethodInvoked);
        }

        [Test]
        public void Should_Construct_With_A_Factory_When_Context_Is_Satisfied()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator.Register(Given<ITestInterface>
                                .When<TestEnum>(test => test == TestEnum.Case1)
                                .ConstructWith(func));

            locator.AddContext(TestEnum.Case1);

            locator.GetInstance<ITestInterface>();

            Assert.IsTrue(factoryMethodInvoked);
        }

        [Test]
        public void Should_Not_Construct_With_A_Factory_When_Context_Is_Satisfied()
        {
            bool factoryMethodInvoked = false;
            Func<IInstanceResolver, ITestInterface> func = container =>
            {
                factoryMethodInvoked = true;
                return new TestCase1();
            };

            locator
                .Register(Given<ITestInterface>.Then<TestCase2>())
                .Register(Given<ITestInterface>
                                .When<TestEnum>(test => test == TestEnum.Case1)
                                .ConstructWith(func));

            locator.AddContext(TestEnum.Case2);

            locator.GetInstance<ITestInterface>();

            Assert.IsFalse(factoryMethodInvoked);
        }
    }
}