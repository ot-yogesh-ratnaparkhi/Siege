using Ninject;

namespace Siege.ServiceLocation.UnitTests.ContextualTests
{
    public class NinjectContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new NinjectAdapter.NinjectAdapter(new StandardKernel());
        }
    }
}
