namespace Siege.Container.UnitTests.ContextualTests.Classes
{
    public class TestController : ITestController
    {
        private readonly IBaseService service;

        public TestController(IBaseService service)
        {
            this.service = service;
        }

        public IBaseService Service
        {
            get { return service; }
        }
    }
}
