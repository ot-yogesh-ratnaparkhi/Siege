using Ninject;
using Ninject.Modules;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.BenchmarkTests
{
    [TestFixture, Ignore]
    public class NinjectBenchmarkTests : BaseBenchmarkTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new NinjectAdapter.NinjectAdapter(new StandardKernel());
        }

        [Test]
        public override void LoadSimpleWithoutSiege()
        {
            Execute("Without Siege", delegate
            {
                StandardKernel kernel = new StandardKernel(new SimpleTestModule());

                kernel.Get<ITestInterface>();
            });
        }
    }


    public class SimpleTestModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ITestInterface>().To<TestCase1>();
        }
    }
}
