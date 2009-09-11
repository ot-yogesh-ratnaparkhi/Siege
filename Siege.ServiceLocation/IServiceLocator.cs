using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public interface IServiceLocator : IDisposable
    {
        T GetInstance<T>();
        T GetInstance<T>(IDictionary constructorArguments);
        T GetInstance<T>(object anonymousConstructorArguments);
        T GetInstance<T>(Type type);
        T GetInstance<T>(Type type, IDictionary constructorArguments);
        T GetInstance<T>(string key);
        T GetInstance<T>(string key, IDictionary constructorArguments);
        IServiceLocator Register<T>(IUseCase<T> useCase);
    }
}