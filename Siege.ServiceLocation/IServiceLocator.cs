using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable, IInstanceResolver, IBindingAdapter
    {
        new TService GetInstance<TService>(string key);
        TService GetInstance<TService>(Type type);
        new object GetInstance(Type type);
        new object GetInstance(Type type, string key);
    }

    public interface IInstanceResolver
    {
        object GetInstance(Type type, string key);
        object GetInstance(Type type);
    }

    public interface IBindingAdapter
    {
        IServiceLocator Register<TService>(IUseCase<TService> useCase);
        IServiceLocator AddBinding(Type baseBinding, Type targetBinding);
    }

    public interface IGetAllInstancesServiceLocator
    {
        IEnumerable<object> GetAllInstances(Type serviceType);
        IEnumerable<TService> GetAllInstances<TService>();
    }
}