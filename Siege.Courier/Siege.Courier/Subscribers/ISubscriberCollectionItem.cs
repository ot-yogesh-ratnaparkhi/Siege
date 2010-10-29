using System.Collections.Generic;

namespace Siege.Courier.Subscribers
{
    public interface ISubscriberCollectionItem
    {
        IEnumerable<ISubscriber> Subscribers { get; }
        void Add(ISubscriber subscriber);
        void Remove(ISubscriber subscriber);
    }
}