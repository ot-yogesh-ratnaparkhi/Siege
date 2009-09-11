using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.BenchmarkTests
{
    [TestFixture, Ignore]
    public class StructureMapBenchmarkTests : BaseBenchmarkTests
    {
        private StructureMap.Container container;
        [SetUp]
        public void SetUp()
        {
            container = new StructureMap.Container();
        }
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter(container);
        }

        [Test]
        public override void LoadSimpleWithoutSiege()
        {
            Execute("Without Siege", delegate
            {
                container.Configure(registry => registry.ForRequestedType<ITestInterface>().TheDefaultIsConcreteType<TestCase1>());

                container.GetInstance<ITestInterface>();
            });
        }
    }
}
