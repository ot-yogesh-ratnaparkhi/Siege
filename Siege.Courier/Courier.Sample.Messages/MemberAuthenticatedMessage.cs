using Siege.Courier.Messages;

namespace Courier.Sample.Messages
{
    public class MemberAuthenticatedMessage : IMessage
    {
        private readonly LogOnAccountMessage request;

        public MemberAuthenticatedMessage(LogOnAccountMessage request)
        {
            this.request = request;
        }

        public LogOnAccountMessage Request
        {
            get { return request; }
        }
    }
}