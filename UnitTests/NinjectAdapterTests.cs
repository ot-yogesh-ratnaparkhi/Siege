using System;
using Ninject;
using Ninject.Planning.Bindings;
using NUnit.Framework;
using Siege.Container.NinjectAdapter;
using Siege.ServiceLocation;

namespace UnitTests
{
    [TestFixture]
    public class NinjectAdapterTests : SiegeContainerTests
    {
        IKernel kernel = new StandardKernel();

        protected override IContextualServiceLocator GetAdapter()
        {
            return new NinjectAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            Type type = typeof(UnregisteredClass);
            BindingBuilder<IUnregisteredInterface> builder = new BindingBuilder<IUnregisteredInterface>(new Binding(typeof(IUnregisteredInterface)));

            builder.To(type).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }
    }
}
