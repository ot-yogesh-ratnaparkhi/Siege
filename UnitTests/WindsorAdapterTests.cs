using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.Container.WindsorAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class WindsorAdapterTests : SiegeContainerTests
    {
        IKernel kernel = new DefaultKernel();

        protected override IContextualServiceLocator GetAdapter()
        {
            return new WindsorAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            kernel.Register(Component.For<IUnregisteredInterface>().ImplementedBy<UnregisteredClass>());
        }
    }
}
