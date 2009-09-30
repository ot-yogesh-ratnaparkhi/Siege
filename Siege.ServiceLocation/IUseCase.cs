using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase
    {
        Type GetBoundType();
        object Resolve(IServiceLocator locator, IList<object> context, IDictionary dictionary);
        object Resolve(IServiceLocator locator, IDictionary dictionary);
    }

    public interface IUseCase<TBaseType> : IUseCase
    {
    }
}