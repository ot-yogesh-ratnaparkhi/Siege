namespace Siege.Container.UnitTests.ContextualTests.Classes
{
    public class TestService2 : IBaseService
    {
        public TestService2(ITestRepository repository)
        {
            this.Repository = repository;
        }

        public ITestRepository Repository
        {
            get; set;
        }
    }
}
