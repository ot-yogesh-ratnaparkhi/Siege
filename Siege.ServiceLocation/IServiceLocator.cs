using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable
    {
        TService GetInstance<TService>(IDictionary constructorArguments);
        new TService GetInstance<TService>(string key);
        TService GetInstance<TService>(object anonymousConstructorArguments);
        TService GetInstance<TService>(Type type);
        TService GetInstance<TService>(Type type, IDictionary constructorArguments);
        TService GetInstance<TService>(string key, IDictionary constructorArguments);
        object GetInstance(Type type, IDictionary constructorArguments);
        object GetInstance(Type serviceType, string key, IDictionary constructorArguments);
        IServiceLocator Register<TService>(IUseCase<TService> useCase);
    }
}