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
    }

    public interface IServiceLocator
    {
        T GetInstance<T>();
        T GetInstance<T>(IDictionary constructorArguments);
        T GetInstance<T>(Type type);
        T GetInstance<T>(Type type, IDictionary constructorArguments);
        void Register<T>(IUseCase<T> useCase);
    }
}