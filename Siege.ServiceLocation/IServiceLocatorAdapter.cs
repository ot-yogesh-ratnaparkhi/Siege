using System;

namespace Siege.ServiceLocation
{
    public interface IServiceLocatorAdapter : IDisposable, IGetAllInstancesServiceLocator
    {
        void RegisterParentLocator(IContextualServiceLocator locator);
        IGenericFactory<TBaseService> GetFactory<TBaseService>();
    }
}