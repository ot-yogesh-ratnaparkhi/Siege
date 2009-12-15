using System;

namespace Siege.ServiceLocation
{
    public interface IBindingAdapter
    {
        IServiceLocator AddBinding(Type baseBinding, Type targetBinding);
    }
}