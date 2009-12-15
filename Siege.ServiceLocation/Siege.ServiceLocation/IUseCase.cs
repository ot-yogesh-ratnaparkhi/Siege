using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase
    {
        Type GetBoundType();
        Type GetUseCaseBindingType();
        Type GetBaseBindingType();
        object Resolve(IInstanceResolver locator, IList<object> context);
        object Resolve(IInstanceResolver locator);
        bool IsValid(IList<object> context);
    }

    public interface IUseCase<TBaseService> : IUseCase
    {
    }
}