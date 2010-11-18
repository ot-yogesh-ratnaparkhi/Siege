using Siege.Courier.Messages;

namespace Courier.Sample.Messages
{
    public class RegistrationSucceededMessage : IMessage
    {
        public string UserName { get; set; }
    }
}