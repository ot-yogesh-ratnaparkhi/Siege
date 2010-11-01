using System;
using Courier.Sample.Messages;
using CourierSample.Security;
using Siege.Courier;
using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace CourierApp.Subscribers
{
    public class AccountSubscriber : Subscriber.For<LogOnAccountMessage>
    {
        private readonly IMembershipService membershipService;
        private readonly IServiceBus serviceBus;

        public AccountSubscriber(IMembershipService membershipService, IServiceBus serviceBus)
        {
            this.membershipService = membershipService;
            this.serviceBus = serviceBus;
        }

        public void Receive(LogOnAccountMessage message)
        {
            if (membershipService.ValidateUser(message.UserName, message.Password))
            {
                serviceBus.Publish(new MemberAuthenticatedMessage());
                return;
            }

            serviceBus.Publish(new MemberFailedAuthenticationMessage());
        }
    }
}