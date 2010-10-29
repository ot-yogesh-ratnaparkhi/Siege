using Siege.Courier.Messages;

namespace Siege.Courier.Processors
{
    public class PreProcessor
    {
        public interface For<TMessage> where TMessage : IMessage
        {
        }
    }
}