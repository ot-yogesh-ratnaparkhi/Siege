namespace Siege.ServiceLocation.UnitTests.ContextualTests
{
    public class UnityContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new UnityAdapter.UnityAdapter();
        }
    }
}
