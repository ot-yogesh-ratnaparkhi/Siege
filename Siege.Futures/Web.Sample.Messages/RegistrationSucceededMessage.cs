using Siege.Eventing.Messages;

namespace Web.Sample.Messages
{
    public class RegistrationSucceededMessage : IMessage
    {
        public string UserName { get; set; }
    }
}