using System;
using System.Collections;
using System.Collections.Generic;

namespace Siege.ServiceLocation
{
    public interface IContext
    {
    }
    public class Context : IContext
    {
        public Context(object contextItem)
        {
            Value = contextItem;
        }

        public object Value { get; set; }
    }

    public class Context<T> : Context
    {
        public Context(object contextItem) : base(contextItem)
        {
        }

        public new T Value { get; set; }
    }

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