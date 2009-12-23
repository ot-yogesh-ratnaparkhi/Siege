namespace Siege.ServiceLocation.UnitTests.ContextualTests
{
    public class StructureMapContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }
    }
}
