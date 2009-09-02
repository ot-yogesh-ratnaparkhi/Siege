using NUnit.Framework;
using Siege.ServiceLocation;
using StructureMap;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public class StructureMapAdapterTests : SiegeContainerTests
    {
        public override void SetUp()
        {
            ObjectFactory.ResetDefaults();
            base.SetUp();
        }

        protected override IServiceLocator GetAdapter()
        {
            return new StructureMapAdapter.StructureMapAdapter();
        }

        protected override void RegisterWithoutSiege()
        {
            ObjectFactory.Initialize(registry => registry.ForRequestedType<IUnregisteredInterface>().TheDefaultIsConcreteType<UnregisteredClass>());
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
