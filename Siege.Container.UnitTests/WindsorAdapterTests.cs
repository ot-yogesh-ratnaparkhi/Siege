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

        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new WindsorAdapter.WindsorAdapter(kernel);
        }

        protected override void RegisterWithoutSiege()
        {
            kernel.Register(Component.For<IUnregisteredInterface>().ImplementedBy<UnregisteredClass>());
        }
        
        [Test]
        public virtual void Should_Dispose_From_Containers()
        {
            DefaultKernel disposableKernel = new DefaultKernel();
            using (var disposableLocater = new SiegeContainer(new WindsorAdapter.WindsorAdapter(disposableKernel)))
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
