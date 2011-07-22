using NUnit.Framework;
using Siege.ServiceLocator.Extensions.ConditionalAwareness;
using Siege.ServiceLocator.Extensions.ExtendedRegistrationSyntax;
using Siege.ServiceLocator.UnitTests.TestClasses;

namespace Siege.ServiceLocator.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test]
        public void ShouldFindContextWithoutExplicitlyAdding()
        {
            locator
                .Register(Awareness.Of(() => locator.GetInstance<ISomeService>()))
                .Register(Given<ISomeService>.Then<SomeService>())
                .Register(Given<ITestInterface>.When<ISomeService>(i => i.SomeMethod()).Then<TestCase1>())
                .Register(Given<ITestInterface>.Then<TestCase2>());

            Assert.IsInstanceOf<TestCase1>(locator.GetInstance<ITestInterface>());
        }

        public interface ISomeService
        {
            bool SomeMethod();
        }

        public class SomeService : ISomeService
        {
            public bool SomeMethod()
            {
                return true;
            }
        }
    }
}