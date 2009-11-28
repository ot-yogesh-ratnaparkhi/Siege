using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable, IMinimalServiceLocator
    {
        TService GetInstance<TService>(IDictionary constructorArguments);
        new TService GetInstance<TService>(string key);
        TService GetInstance<TService>(object anonymousConstructorArguments);
        TService GetInstance<TService>(Type type);
        TService GetInstance<TService>(Type type, IDictionary constructorArguments);
        IServiceLocator Register<TService>(IUseCase<TService> useCase);
    }

    public interface IMinimalServiceLocator
    {
        object GetInstance(Type type, IDictionary constructorArguments);
        object GetInstance(Type serviceType, string key, IDictionary constructorArguments);
        TService GetInstance<TService>(string key, IDictionary constructorArguments);
    }

    public interface IGetAllInstancesServiceLocator : IMinimalServiceLocator
    {
        IEnumerable<object> GetAllInstances(Type serviceType);
        IEnumerable<TService> GetAllInstances<TService>();
    }
}