using Siege.Courier.Messages;

namespace Siege.Courier
{
    public interface IMessageTracker
    {
        void Track(IMessage message);
        bool IsTracked(IMessage message);
        void Clear();
    }
}