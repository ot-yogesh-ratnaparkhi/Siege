using NUnit.Framework;
using Siege.ServiceLocation.Extensions.AutoScanner;
using Siege.ServiceLocation.Extensions.Conventions;

namespace Siege.ServiceLocation.UnitTests
{
    public partial class SiegeContainerTests
    {
        [Test]
        public void Should_Use_Simple_Auto_Scanning_Conventions()
        {
            locator.Register(Using.Convention<TestConvention>());

            Assert.IsInstanceOfType(typeof (AutoScannedType), locator.GetInstance<IAutoScannedInterface>());
        }
    }

    public class TestConvention : AutoScanningConvention
    {
        public TestConvention()
        {
            FromAssemblyContaining<AutoScannedType>();
            ForTypesImplementing<IAutoScannedInterface>();
        }
    }

    public interface IAutoScannedInterface
    {
    }

    public class AutoScannedType : IAutoScannedInterface
    {
    }
}