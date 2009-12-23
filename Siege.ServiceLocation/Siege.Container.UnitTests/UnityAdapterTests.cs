using System;
using Microsoft.Practices.Unity;
using NUnit.Framework;
using Siege.ServiceLocation.UnitTests.RegistrationExtensions.Unity;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    [TestFixture]
    public class UnityAdapterTests : SiegeContainerTests
    {
        private UnityContainer container;

        public override void SetUp()
        {
            container = new UnityContainer();
            base.SetUp();
        }

        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new UnityAdapter.UnityAdapter(container);
        }

        protected override void RegisterWithoutSiege()
        {
            container.RegisterType<IUnregisteredInterface, UnregisteredClass>();
        }

        protected override Type GetDecoratorUseCaseBinding()
        {
            return typeof (DecoratorUseCaseBinding<>);
        }

        [ExpectedException(typeof(ResolutionFailedException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();
        }

        [ExpectedException(typeof(ResolutionFailedException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();
        }
    }
}
