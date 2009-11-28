using System;

namespace Siege.ServiceLocation
{
    public interface IServiceLocatorAdapter : IDisposable, IGetAllInstancesServiceLocator
    {
        void RegisterParentLocator(IContextualServiceLocator locator);
        IGenericFactory<TBaseService> GetFactory<TBaseService>();
        void RegisterBinding(Type baseBinding, Type targetBinding);
        Type ConditionalUseCaseBinding { get; }
        Type DefaultUseCaseBinding { get; }
        Type DefaultInstanceUseCaseBinding { get; }
        Type KeyBasedUseCaseBinding { get; }
    }
}