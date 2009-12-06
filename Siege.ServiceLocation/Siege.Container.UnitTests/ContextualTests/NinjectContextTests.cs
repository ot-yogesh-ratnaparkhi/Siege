using Ninject;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.ContextualTests
{
    public class NinjectContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new NinjectAdapter.NinjectAdapter(new StandardKernel());
        }
    }
}
