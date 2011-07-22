using System;
using System.Collections.Generic;
using Siege.Eventing.Messages;

namespace Siege.Eventing
{
    public class ThreadedMessageTracker : IMessageTracker
    {
        [ThreadStatic] private static List<IMessage> receivedMessages;

        public void Track(IMessage message)
        {
            receivedMessages = receivedMessages ?? new List<IMessage>();
     
            if (receivedMessages.Contains(message)) return;

            receivedMessages.Add(message);
        }

        public bool IsTracked(IMessage message)
        {
            return receivedMessages.Contains(message);
        }

        public void Clear()
        {
            receivedMessages.Clear();
        }
    }
}