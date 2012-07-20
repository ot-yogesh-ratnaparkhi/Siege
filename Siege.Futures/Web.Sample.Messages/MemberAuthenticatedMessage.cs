using Siege.Eventing.Messages;

namespace Web.Sample.Messages
{
    public class MemberAuthenticatedMessage : IMessage
    {
        public string UserName { get; set; }
        public bool RememberMe { get; set; }
    }
}