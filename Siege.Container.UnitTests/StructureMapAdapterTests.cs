using NUnit.Framework;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public class StructureMapAdapterTests : SiegeContainerTests
    {
        protected override IServiceLocator GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }

        protected override void RegisterWithoutSiege()
        {
            ObjectFactory.Initialize(registry => registry.ForRequestedType<IUnregisteredInterface>().TheDefaultIsConcreteType<UnregisteredClass>());
        }
    }
}
