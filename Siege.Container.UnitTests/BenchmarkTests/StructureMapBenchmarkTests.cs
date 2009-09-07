using NUnit.Framework;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.UnitTests.BenchmarkTests
{
    public class StructureMapBenchmarkTests : BaseBenchmarkTests
    {
        [SetUp]
        public void SetUp()
        {
            ObjectFactory.ResetDefaults();
        }
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }

        [Test]
        public override void LoadSimpleWithoutSiege()
        {
            Execute("Without Siege", delegate
            {
                ObjectFactory.Initialize(registry => registry.ForRequestedType<ITestInterface>().TheDefaultIsConcreteType<TestCase1>());

                ObjectFactory.GetInstance<ITestInterface>();
            });
        }
    }
}
