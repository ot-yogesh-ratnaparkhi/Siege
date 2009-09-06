using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.ContextualTests
{
    public class StructureMapContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }
    }
}
