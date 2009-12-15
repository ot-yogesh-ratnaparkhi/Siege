using System;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable, IInstanceResolver, IBindingAdapter
    {
        IServiceLocator Register<TService>(IUseCase<TService> useCase);
        new TService GetInstance<TService>(string key);
        TService GetInstance<TService>(Type type);
        new object GetInstance(Type type);
        new object GetInstance(Type type, string key);
    }
}