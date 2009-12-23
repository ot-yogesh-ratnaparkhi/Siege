using System;
using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.ServiceLocation;
using Siege.ServiceLocation.UnitTests.RegistrationExtensions.Castle;
using Siege.ServiceLocation.UnitTests.TestClasses;
using Siege.SeviceLocation.WindsorAdapter;

namespace Siege.ServiceLocation.UnitTests
{
    [TestFixture]
    public class WindsorAdapterTests : SiegeContainerTests
    {
        private IKernel kernel;

        protected override Type GetDecoratorUseCaseBinding()
        {
            return typeof(DecoratorUseCaseBinding<>);
        }

        public override void SetUp()
        {
            kernel = new DefaultKernel();
            base.SetUp();
        }

        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new WindsorAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            kernel.Register(Component.For<IUnregisteredInterface>().ImplementedBy<UnregisteredClass>());
        }
        
        [Test]
        public virtual void Should_Dispose_From_Containers()
        {
            DefaultKernel disposableKernel = new DefaultKernel();
            using (var disposableLocater = new SiegeContainer(new WindsorAdapter(disposableKernel)))
            {
                disposableLocater.Register(Given<ITestInterface>.Then<TestCase1>());
                Assert.IsTrue(disposableLocater.GetInstance<ITestInterface>() is TestCase1);
            }
            
            Assert.IsFalse(disposableKernel.HasComponent(typeof(ITestInterface)));
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
