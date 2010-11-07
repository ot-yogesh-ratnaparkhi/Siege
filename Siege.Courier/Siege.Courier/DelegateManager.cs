using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Siege.Courier.Messages;

namespace Siege.Courier
{
    public class DelegateManager
    {
        private static readonly Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();
        private static readonly object lockObject = new object();

        public Delegate CreateDelegate(IMessage message, Func<IServiceBus> serviceBus)
        {
            var type = message.GetType();

            if (!delegates.ContainsKey(type))
            {
                lock (lockObject)
                {
                    if (!delegates.ContainsKey(type))
                    {
                        Type messageConvertor = typeof (Action<>).MakeGenericType(type);
                        var parameter = Expression.Parameter(type, "x");
                        delegates.Add(type,
                                      Expression.Lambda(messageConvertor,
                                                        Expression.Call(Expression.Constant(serviceBus()),
                                                                        typeof (IServiceBus).GetMethod("Publish").
                                                                            MakeGenericMethod(type),
                                                                        Expression.Constant(message, type)),
                                                        parameter).Compile());
                    }
                }
            }

            return delegates[type];
        }
    }
}