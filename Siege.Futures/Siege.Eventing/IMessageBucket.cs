using System.Collections.Generic;
using Siege.Eventing.Messages;

namespace Siege.Eventing
{
    public interface IMessageBucket
    {
        void Add(IMessage message);
        List<IMessage> All();
        void Clear();
    }
}