using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase
    {
        Type GetBoundType();
    }

    public interface IUseCase<TBaseType> : IUseCase
    {
        TBaseType Resolve(IServiceLocator locator, IList<object> context, IDictionary dictionary);
        TBaseType Resolve(IServiceLocator locator, IDictionary dictionary);
    }
}