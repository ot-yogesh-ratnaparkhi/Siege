using System;
using System.Collections;

namespace Siege.ServiceLocation
{
    public interface IContextualServiceLocator : IServiceLocator
    {
        T GetInstance<T, TContext>(TContext context);
        T GetInstance<T, TContext>(Type type, TContext context);
        T GetInstance<T, TContext>(TContext context, IDictionary constructorArguments);
        T GetInstance<T, TContext>(Type type, TContext context, IDictionary constructorArguments);
        T GetInstance<T, TContext>(TContext context, object anonymousConstructorArguments);
        T GetInstance<T, TContext>(Type type, TContext context, object anonymousConstructorArguments);
    }

    public interface IServiceLocator
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

    public interface IServiceLocatorAdapter : IServiceLocator
    {
        void RegisterParentLocator(IContextualServiceLocator locator);
    }
}