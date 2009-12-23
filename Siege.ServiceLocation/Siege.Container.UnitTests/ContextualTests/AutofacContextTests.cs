namespace Siege.ServiceLocation.UnitTests.ContextualTests
{
    public class AutofacContextTests : BaseContextTests
    {
        protected override IServiceLocatorAdapter GetAdapter()
        {
            return new AutofacAdapter.AutofacAdapter();
        }
    }
}
