using System;

namespace Siege.ServiceLocation
{
    public interface IBindingAdapter
    {
        IServiceLocator Register<TService>(IUseCase<TService> useCase);
        IServiceLocator AddBinding(Type baseBinding, Type targetBinding);
    }
}