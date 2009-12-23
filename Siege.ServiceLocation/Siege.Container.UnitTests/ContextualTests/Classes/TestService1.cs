namespace Siege.ServiceLocation.UnitTests.ContextualTests.Classes
{
    public class TestService1 : IBaseService
    {
        public TestService1(ITestRepository repository)
        {
            Repository = repository;
        }

        public ITestRepository Repository
        {
            get; set;
        }
    }
}
