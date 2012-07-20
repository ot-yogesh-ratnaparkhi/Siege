using System.Web.Security;
using Siege.Eventing.Messages;

namespace Web.Sample.Messages
{
    public class RegistrationFailedMessage : IMessage
    {
        public MembershipCreateStatus Status { get; set; }
    }
}