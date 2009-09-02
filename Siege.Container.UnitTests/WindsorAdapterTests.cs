using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public class WindsorAdapterTests : SiegeContainerTests
    {
        private IKernel kernel;

        public override void SetUp()
        {
            kernel = new DefaultKernel();
            base.SetUp();
        }

        protected override IServiceLocator GetAdapter()
        {
            return new WindsorAdapter.WindsorAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            kernel.Register(Component.For<IUnregisteredInterface>().ImplementedBy<UnregisteredClass>());
        }

        [ExpectedException(typeof(ComponentNotFoundException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();
        }

        [ExpectedException(typeof(ComponentNotFoundException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();
        }
    }
}
