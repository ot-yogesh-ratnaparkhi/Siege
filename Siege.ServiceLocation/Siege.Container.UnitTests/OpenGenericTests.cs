using System;
using NUnit.Framework;
using Siege.ServiceLocation.Extensions.ExtendedSyntax;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    public abstract partial class SiegeContainerTests
    {
        [Test]
        public void Should_Resolve_Open_Generic()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof (ISomeType<>)).Then(typeof (ConcreteType<>)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Type: Siege.ServiceLocation.UnitTests.TestClasses.ITestInterface is not a generic type.")]
        public void Should_Throw_Exception_On_NonGeneric_Source()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof(ITestInterface)).Then(typeof(ConcreteType<>)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }

        [Test]
        [ExpectedException(typeof(Exception), ExpectedMessage = "Type: Siege.ServiceLocation.UnitTests.TestClasses.ITestInterface is not a generic type.")]
        public void Should_Throw_Exception_On_NonGeneric_Target()
        {
            locator
                .Register(Given<TestCase1>.Then<TestCase1>())
                .Register(Given.OpenType(typeof(ISomeType<>)).Then(typeof(ITestInterface)));

            Assert.IsInstanceOfType(typeof(ConcreteType<TestCase1>), locator.GetInstance<ISomeType<TestCase1>>());
        }
    }

    public interface ISomeType<T> {}
    public class ConcreteType<T> : ISomeType<T> {}
}