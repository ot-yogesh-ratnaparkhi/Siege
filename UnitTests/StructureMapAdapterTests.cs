using NUnit.Framework;
using Siege.Container.StructureMapAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class StructureMapAdapterTests : SiegeContainerTests
    {
        protected override IContextualServiceLocator GetAdapter()
        {
            return new StructureMapAdapter();
        }
    }
}
