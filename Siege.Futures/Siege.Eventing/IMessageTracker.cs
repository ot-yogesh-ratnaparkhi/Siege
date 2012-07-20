using Siege.Eventing.Messages;

namespace Siege.Eventing
{
    public interface IMessageTracker
    {
        void Track(IMessage message);
        bool IsTracked(IMessage message);
        void Clear();
    }
}