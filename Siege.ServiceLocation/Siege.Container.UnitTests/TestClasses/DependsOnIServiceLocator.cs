using Siege.ServiceLocation;

namespace Siege.ServiceLocation.UnitTests.TestClasses
{
    public class DependsOnIServiceLocator : ITestInterface
    {
        public DependsOnIServiceLocator(IServiceLocator locator)
        {
        }
    }
}