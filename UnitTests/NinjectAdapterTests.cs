using Ninject;
using NUnit.Framework;
using Siege.Container.NinjectAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class NinjectAdapterTests : SiegeContainerTests
    {
        protected override IContextualServiceLocator GetAdapter()
        {
            IKernel kernel = new StandardKernel();
            return new NinjectAdapter(kernel);
        }
    }
}
