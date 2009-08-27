using System;
using NUnit.Framework;
using Siege.Container.StructureMapAdapter;
using Siege.ServiceLocation;
using StructureMap;

namespace UnitTests
{
    [TestFixture]
    public class StructureMapAdapterTests : SiegeContainerTests
    {
        protected override IContextualServiceLocator GetAdapter()
        {
            return new StructureMapAdapter();
        }

        protected override void RegisterWithoutSiege()
        {
            ObjectFactory.Initialize(registry => registry.ForRequestedType<IUnregisteredInterface>().TheDefaultIsConcreteType<UnregisteredClass>());
        }
    }
}
