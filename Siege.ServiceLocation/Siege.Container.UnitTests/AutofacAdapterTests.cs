using System;
using Autofac;
using Autofac.Builder;
using NUnit.Framework;
using Siege.ServiceLocation.UnitTests.RegistrationExtensions.Autofac;
using Siege.ServiceLocation.UnitTests.TestClasses;

namespace Siege.ServiceLocation.UnitTests
{
    [TestFixture]
    public class AutofacAdapterTests : SiegeContainerTests
    {
        private IContainer container;

        public override void SetUp()
        {
            container = new Container();
            base.SetUp();
        }
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new AutofacAdapter.AutofacAdapter(container);
        }

        protected override void RegisterWithoutSiege()
        {
            var builder = new ContainerBuilder();
            builder.Register<UnregisteredClass>().As<IUnregisteredInterface>();
            builder.Build(container);
        }

        protected override Type GetDecoratorUseCaseBinding()
        {
            return typeof (DecoratorUseCaseBinding<>);
        }

        [ExpectedException(typeof(ComponentNotRegisteredException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();
        }

        [ExpectedException(typeof(ComponentNotRegisteredException))]
        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();
        }
    }
}
