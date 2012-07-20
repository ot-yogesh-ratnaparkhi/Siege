using System;
using System.Collections.Generic;

namespace Siege.Eventing.Messages
{
    public class MessageMap
    {
        private Dictionary<Type, IProtocolAdapter> map = new Dictionary<Type,IProtocolAdapter>();

        public void Map<TMessage>(IProtocolAdapter adapter)
            where TMessage : IMessage
        {
            map.Add(typeof(TMessage), adapter);
        }

        public IProtocolAdapter GetAdapterFor(Type type)
        {
            return map[type];
        }

        public bool CanHandle(Type type)
        {
            return map.ContainsKey(type);
        }
    }
}