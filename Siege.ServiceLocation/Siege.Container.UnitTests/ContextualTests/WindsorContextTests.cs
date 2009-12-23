using Castle.MicroKernel;
using Siege.SeviceLocation.WindsorAdapter;

namespace Siege.ServiceLocation.UnitTests.ContextualTests
{
    public class WindsorContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new WindsorAdapter(new DefaultKernel());
        }
    }
}
