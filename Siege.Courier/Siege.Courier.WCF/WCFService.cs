using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.ServiceModel;
using Siege.Courier.Messages;

namespace Siege.Courier.WCF
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public class WCFService : IWCFProtocol
    {
        private readonly IServiceBus serviceBus;
        private readonly static Dictionary<Type, Delegate> delegates = new Dictionary<Type, Delegate>();
        private readonly static object lockObject = new object();

        public WCFService(IServiceBus serviceBus)
        {
            this.serviceBus = serviceBus;
        }

        public IMessage Send(IMessage message)
        {
            if(!delegates.ContainsKey(message.GetType()))
            {
                lock(lockObject)
                {
                    if(!delegates.ContainsKey(message.GetType()))
                    {
                        Type messageConvertor = typeof(Action<>).MakeGenericType(message.GetType());
                        var parameter = Expression.Parameter(message.GetType(), "x");
                        delegates.Add(message.GetType(), Expression.Lambda(messageConvertor, Expression.Call(Expression.Constant(serviceBus), typeof(IServiceBus).GetMethod("Publish").MakeGenericMethod(message.GetType()), Expression.Constant(message, message.GetType())), parameter).Compile());
                    }
                }
            }

            delegates[message.GetType()].DynamicInvoke(message);

            return null;
        }
    }
}