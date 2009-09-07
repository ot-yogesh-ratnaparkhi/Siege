using System;
using NUnit.Framework;
using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.BenchmarkTests
{
    [TestFixture]
    public abstract class BaseBenchmarkTests
    {
        protected IContextualServiceLocator locator;
        protected abstract IServiceLocatorAdapter GetAdapter();
        public abstract void LoadSimpleWithoutSiege();
        protected double withSiege;
        protected double withoutSiege;
        protected int testInterval = 1000;
        
        [Test]
        public void LoadSimpleWithSiege()
        {
            Execute("Siege", delegate
            {
                locator = new SiegeContainer(GetAdapter());

                locator.Register(Given<ITestInterface>.Then<TestCase1>());

                locator.GetInstance<ITestInterface>();
            });
        }

        protected void Execute(string token, Action action)
        {
            for (int i = 0; i < testInterval; i++)
            {
                DateTime startTime = DateTime.Now;

                action();

                DateTime endTime = DateTime.Now;

                withSiege += (endTime - startTime).TotalMilliseconds;
            }

            Console.WriteLine(token + " total execution time: " + (withSiege / 1000F));
        }
    }
}
