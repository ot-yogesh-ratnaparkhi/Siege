using System;
using NUnit.Framework;
using Siege.ServiceLocation;
using Siege.ServiceLocation.StructureMapAdapter;
using Siege.ServiceLocation.UnitTests.RegistrationExtensions.StructureMap;
using Siege.ServiceLocation.UnitTests.TestClasses;
using StructureMap;

namespace Siege.ServiceLocation.UnitTests
{
    [TestFixture]
    public class StructureMapAdapterTests : SiegeContainerTests
    {
        private StructureMap.Container container;
        protected override IServiceLocatorAdapter GetAdapter()
        {
            container = new StructureMap.Container();
            return new StructureMapAdapter.StructureMapAdapter(container);
        }

        protected override void RegisterWithoutSiege()
        {
            container.Configure(registry => registry.ForRequestedType<IUnregisteredInterface>().TheDefaultIsConcreteType<UnregisteredClass>());
        }

        protected override Type GetDecoratorUseCaseBinding()
        {
            return typeof (DecoratorUseCaseBinding<>);
        }

        [ExpectedException(typeof(StructureMapException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();
        }

        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();
            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }
    }
}
