using System;
using Courier.Sample.Messages;
using Siege.Courier.Subscribers;

namespace Courier_Sample.Subscribers
{
    public class AccountSubscriber : Subscriber.For<MemberFailedAuthenticationMessage>,
                                     Subscriber.For<MemberAuthenticatedMessage>
    {
        public void Receive(MemberFailedAuthenticationMessage message)
        {
            throw new NotImplementedException();
        }

        public void Receive(MemberAuthenticatedMessage message)
        {
            throw new NotImplementedException();
        }
    }
}