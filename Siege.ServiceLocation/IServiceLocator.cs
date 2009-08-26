using System;

namespace Siege.ServiceLocation
{
    public interface IContextualServiceLocator : IServiceLocator
    {
        T GetInstance<T, TContext>(TContext context) where TContext : IContext;
    }

    public interface IServiceLocator
    {
        T GetInstance<T>();
        T GetInstance<T>(Type type);
        T GetInstance<T>(string name);
        void Register<T>(IUseCase<T> useCase);
    }
}