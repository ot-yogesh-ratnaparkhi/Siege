using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.UnitTests.ContextualTests
{
    public class StructureMapContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }

        public override void SetUp()
        {
            ObjectFactory.ResetDefaults();
            base.SetUp();
        }
    }
}
