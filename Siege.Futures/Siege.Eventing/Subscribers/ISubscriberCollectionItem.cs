using System.Collections.Generic;

namespace Siege.Eventing.Subscribers
{
    public interface ISubscriberCollectionItem
    {
        IEnumerable<ISubscriber> Subscribers { get; }
        void Add(ISubscriber subscriber);
        void Remove(ISubscriber subscriber);
    }
}