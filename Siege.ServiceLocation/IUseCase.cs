using System;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase
    {
        Type GetBoundType();
        Type GetUseCaseBindingType();
        object Resolve(IInstanceResolver locator, IList<object> context);
        object Resolve(IInstanceResolver locator);
    }

    public interface IUseCase<TBaseService> : IUseCase
    {
    }
}