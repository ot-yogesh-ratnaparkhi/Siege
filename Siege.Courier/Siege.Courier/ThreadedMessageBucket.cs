using System;
using System.Collections.Generic;
using Siege.Courier.Messages;

namespace Siege.Courier
{
    public class ThreadedMessageBucket : IMessageBucket
    {
        [ThreadStatic]
        private static List<IMessage> messages;

        public void Add(IMessage message)
        {
            messages = messages ?? new List<IMessage>();
            messages.Add(message);
        }

        public List<IMessage> All()
        {
            messages = messages ?? new List<IMessage>();

            return messages;
        }

        public void Clear()
        {
            if (messages == null) return;
            messages.Clear();
        }
    }
}