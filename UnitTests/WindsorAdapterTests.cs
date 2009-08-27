using Castle.MicroKernel;
using NUnit.Framework;
using Siege.Container.WindsorAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class WindsorAdapterTests : SiegeContainerTests
    {
        protected override IContextualServiceLocator GetAdapter()
        {
            IKernel kernel = new DefaultKernel();
            return new WindsorAdapter(kernel);
        }
    }
}
