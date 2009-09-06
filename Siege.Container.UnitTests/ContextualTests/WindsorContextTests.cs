using Castle.MicroKernel;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.ContextualTests
{
    public class WindsorContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new WindsorAdapter.WindsorAdapter(new DefaultKernel());
        }
    }
}
