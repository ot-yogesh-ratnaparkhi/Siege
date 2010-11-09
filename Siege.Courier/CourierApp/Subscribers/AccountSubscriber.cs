using System;
using Courier.Sample.Messages;
using CourierApp.Services;
using Siege.Courier;
using Siege.Courier.Subscribers;

namespace CourierApp.Subscribers
{
    public class AccountSubscriber : Subscriber.For<LogOnAccountMessage>
    {
        private readonly IMembershipService membershipService;
        private readonly Func<IServiceBus> serviceBus;

        public AccountSubscriber(IMembershipService membershipService, Func<IServiceBus> serviceBus)
        {
            this.membershipService = membershipService;
            this.serviceBus = serviceBus;
        }

        public void Receive(LogOnAccountMessage message)
        {
            if (membershipService.ValidateUser(message.UserName, message.Password))
            {
                serviceBus().Publish(new MemberAuthenticatedMessage(message));
                return;
            }

            serviceBus().Publish(new MemberFailedAuthenticationMessage());
        }
    }
}