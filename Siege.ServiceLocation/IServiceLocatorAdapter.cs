namespace Siege.ServiceLocation
{
    public interface IServiceLocatorAdapter : IServiceLocator
    {
        void RegisterParentLocator(IContextualServiceLocator locator);
    }
}