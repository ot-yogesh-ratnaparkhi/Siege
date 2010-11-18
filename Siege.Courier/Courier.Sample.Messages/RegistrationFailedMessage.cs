using System.Web.Security;
using Siege.Courier.Messages;

namespace Courier.Sample.Messages
{
    public class RegistrationFailedMessage : IMessage
    {
        public MembershipCreateStatus Status { get; set; }
    }
}