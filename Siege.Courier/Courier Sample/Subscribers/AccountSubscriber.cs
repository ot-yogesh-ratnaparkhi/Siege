using Courier_Sample.Messages;
using Courier_Sample.Models;
using Siege.Courier;
using Siege.Courier.Subscribers;

namespace Courier_Sample.Subscribers
{
    public class AccountSubscriber : Subscriber.For<LogOnAccountMessage>,
                                     Subscriber.For<MemberAuthenticatedMessage>,
                                     Subscriber.For<MemberFailedAuthenticationMessage>
    {
        private readonly IMembershipService membershipService;
        private readonly IFormsAuthenticationService formsAuthenticationService;
        private readonly IServiceBus serviceBus;

        public AccountSubscriber(/*IMembershipService membershipService, IFormsAuthenticationService formsAuthenticationService, IServiceBus serviceBus*/)
        {
            this.membershipService = membershipService;
            this.formsAuthenticationService = formsAuthenticationService;
            this.serviceBus = serviceBus;
        }

        public void Receive(LogOnAccountMessage message)
        {
            if (membershipService.ValidateUser(message.UserName, message.Password))
            {
                formsAuthenticationService.SignIn(message.UserName, message.RememberMe);
                serviceBus.Publish(new MemberAuthenticatedMessage());

                return;
            }

            serviceBus.Publish(new MemberFailedAuthenticationMessage());
        }

        public void Receive(MemberAuthenticatedMessage message)
        {
        }

        public void Receive(MemberFailedAuthenticationMessage message)
        {
        }
    }
}