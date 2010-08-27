using NUnit.Framework;
using Siege.Requisitions.Extensions.ConditionalAwareness;
using Siege.Requisitions.Extensions.ExtendedRegistrationSyntax;
using Siege.Requisitions.UnitTests.TestClasses;

namespace Siege.Requisitions.UnitTests
{
    public partial class ServiceLocatorTests
    {
        [Test, Ignore]
        public void ShouldFindContextWithoutExplicitlyAdding()
        {
            locator
                .Register(Awareness.Of(serviceLocator => serviceLocator.GetInstance<ISomeService>()))
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