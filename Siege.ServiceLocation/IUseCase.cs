using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IUseCase
    {
        Type GetBoundType();
        object Resolve(IMinimalServiceLocator locator, IList<object> context, IDictionary dictionary);
        object Resolve(IMinimalServiceLocator locator, IDictionary dictionary);
    }

    public interface IUseCase<TBaseService> : IUseCase
    {
    }
}