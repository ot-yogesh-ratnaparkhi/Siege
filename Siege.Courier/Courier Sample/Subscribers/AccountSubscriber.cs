using System.Web.Mvc;
using Courier.Sample.Messages;
using Courier_Sample.Controllers;
using Courier_Sample.Models;
using Siege.Courier.Subscribers;
using Siege.Courier.Web;

namespace Courier_Sample.Subscribers
{
    public class AccountSubscriber : MvcSubscriber,
                                     Subscriber.For<MemberFailedAuthenticationMessage>,
                                     Subscriber.For<MemberAuthenticatedMessage>,
                                     Subscriber.For<LogOffAccountMessage>,
                                     Subscriber.For<MessageValidationFailedMessage<LogOnAccountMessage>>
    {
        private readonly IFormsAuthenticationService formsService;

        public AccountSubscriber(IFormsAuthenticationService formsService, ControllerContext controllerContext) : base(controllerContext)
        {
            this.formsService = formsService;
        }

        public void Receive(MemberFailedAuthenticationMessage message)
        {
            AddModelError("", "The user name or password provided is incorrect.");
          //  View(null);
        }

        public void Receive(MemberAuthenticatedMessage message)
        {
            formsService.SignIn(message.Request.UserName, message.Request.RememberMe);

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
            View(message.InnerMessage);
        }

        public void Receive(LogOffAccountMessage message)
        {
            formsService.SignOut();
            RedirectTo<HomeController>(home => home.Index());
        }
    }
}