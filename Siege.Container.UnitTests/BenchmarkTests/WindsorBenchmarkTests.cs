using Castle.MicroKernel;
using Castle.MicroKernel.Registration;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.BenchmarkTests
{
    [TestFixture, Ignore]
    public class WindsorBenchmarkTests : BaseBenchmarkTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new WindsorAdapter.WindsorAdapter(new DefaultKernel());
        }

        [Test]
        public override void LoadSimpleWithoutSiege()
        {
            Execute("Without Siege", delegate
            {
                DefaultKernel kernel = new DefaultKernel();
                kernel.Register(Component.For<ITestInterface>().ImplementedBy<TestCase1>().LifeStyle.Transient);
                kernel.Resolve<ITestInterface>();
            });
        }
    }
}
