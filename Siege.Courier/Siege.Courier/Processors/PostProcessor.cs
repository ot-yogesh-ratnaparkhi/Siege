using Siege.Courier.Messages;

namespace Siege.Courier.Processors
{
    public class PostProcessor
    {
        public interface For<TMessage> where TMessage : IMessage
        {
            TMessage Process(TMessage message);
        }
    }
}