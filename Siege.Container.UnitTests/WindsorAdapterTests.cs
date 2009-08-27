using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public class WindsorAdapterTests : SiegeContainerTests
    {
        readonly IKernel kernel = new DefaultKernel();

        protected override IContextualServiceLocator GetAdapter()
        {
            return new WindsorAdapter.WindsorAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            kernel.Register(Component.For<IUnregisteredInterface>().ImplementedBy<UnregisteredClass>());
        }
    }
}
