using NUnit.Framework;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Initialize_Property_After_Resolution()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));

            TestCase1 instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }

        [Test]
        public void Should_Initialize_Property_After_Resolution_Depending_On_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(
                Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "lulz"));

            locator.AddContext(TestEnum.Case2);

            TestCase1 instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }


        [Test]
        public void Should_Initialize_Property_After_Resolution_With_No_Context()
        {
            locator.Register(Given<ITestInterface>.Then<TestCase1>());
            locator.Register(Given<TestCase1>.InitializeWith(testCase1 => testCase1.Property1 = "lulz"));
            locator.Register(
                Given<TestCase1>.When<TestEnum>(x => x == TestEnum.Case2).InitializeWith(
                    testCase1 => testCase1.Property1 = "rofl"));

            TestCase1 instance = (TestCase1) locator.GetInstance<ITestInterface>();
            Assert.AreEqual("lulz", instance.Property1);
        }
    }
}