using System;
using Ninject;
using Ninject.Planning.Bindings;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests
{
    [TestFixture]
    public class NinjectAdapterTests : SiegeContainerTests
    {
        private IKernel kernel;

        public override void SetUp()
        {
            kernel = new StandardKernel();
            base.SetUp();
        }

        protected override IServiceLocator GetAdapter()
        {
            return new NinjectAdapter.NinjectAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            Type type = typeof(UnregisteredClass);
            BindingBuilder<IUnregisteredInterface> builder = new BindingBuilder<IUnregisteredInterface>(new Binding(typeof(IUnregisteredInterface)));

            builder.To(type).InTransientScope();
            kernel.AddBinding(builder.Binding);
        }

        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_No_Name_Provided();

            Assert.IsTrue(locator.GetInstance<ITestInterface>() is TestCase1);
        }

        public override void Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided()
        {
            base.Should_Not_Be_Able_To_Bind_An_Interface_To_A_Type_With_A_Name_When_Wrong_Name_Provided();

            Assert.IsNull(locator.GetInstance<ITestInterface>("test15"));
        }
    }
}
