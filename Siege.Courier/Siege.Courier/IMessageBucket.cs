using System.Collections.Generic;
using Siege.Courier.Messages;

namespace Siege.Courier
{
    public interface IMessageBucket
    {
        void Add(IMessage message);
        List<IMessage> All();
        void Clear();
    }
}