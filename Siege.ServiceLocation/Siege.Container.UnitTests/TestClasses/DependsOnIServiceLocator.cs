using Siege.ServiceLocation;

namespace Siege.Container.UnitTests.TestClasses
{
    public class DependsOnIServiceLocator : ITestInterface
    {
        public DependsOnIServiceLocator(IServiceLocator locator)
        {
        }
    }
}