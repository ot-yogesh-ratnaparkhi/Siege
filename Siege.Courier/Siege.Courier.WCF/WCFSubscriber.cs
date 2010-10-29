using System;
using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier.WCF
{
    public class WCFSubscriber : Subscriber.For<IMessage>
    {
        public void Receive(IMessage message)
        {
            throw new NotImplementedException();
        }
    }
}