using System.Web.Mvc;
using Web.Sample.Messages;
using Siege.Eventing.Web;
using Siege.Eventing.Subscribers;
using Web.Sample.Controllers;
using Web.Sample.Models;

namespace Web.Sample.Subscribers
{
    public class AccountSubscriber : MvcSubscriber,
                                     Subscriber.For<MemberFailedAuthenticationMessage>,
                                     Subscriber.For<MemberAuthenticatedMessage>,
                                     Subscriber.For<LogOffAccountMessage>,
                                     Subscriber.For<MessageValidationFailedMessage<LogOnAccountMessage>>,
                                     Subscriber.For<RegistrationSucceededMessage>,
                                     Subscriber.For<RegistrationFailedMessage>
    {
        private readonly IFormsAuthenticationService formsService;

        public AccountSubscriber(IFormsAuthenticationService formsService, ControllerContext controllerContext) : base(controllerContext)
        {
            this.formsService = formsService;
        }

        public void Receive(MemberFailedAuthenticationMessage message)
        {
            AddModelError("", "The user name or password provided is incorrect.");
        }

        public void Receive(MemberAuthenticatedMessage message)
        {
            formsService.SignIn(message.UserName, message.RememberMe);

            if (QueryString.ReturnUrl == null)
            {
                RedirectTo<HomeController>(home => home.Index());
                return;
            }

            Redirect(QueryString.ReturnUrl);
        }

        public void Receive(MessageValidationFailedMessage<LogOnAccountMessage> message)
        {
            AddModelError("", "The user name or password provided is incorrect.");
        }

        public void Receive(LogOffAccountMessage message)
        {
            formsService.SignOut();
            RedirectTo<HomeController>(home => home.Index());
        }

        public void Receive(RegistrationSucceededMessage message)
        {
            formsService.SignIn(message.UserName, false /* createPersistentCookie */);
        }

        public void Receive(RegistrationFailedMessage message)
        {
            AddModelError("", AccountValidation.ErrorCodeToString(message.Status));
        }
    }
}