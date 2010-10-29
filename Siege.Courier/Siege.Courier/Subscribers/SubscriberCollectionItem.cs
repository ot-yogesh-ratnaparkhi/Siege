using System.Collections.Generic;

namespace Siege.Courier.Subscribers
{
    public class SubscriberCollectionItem : ISubscriberCollectionItem
    {
        private List<ISubscriber> subscribers;

        public SubscriberCollectionItem()
        {
            subscribers = new List<ISubscriber>();
        }

        public IEnumerable<ISubscriber> Subscribers
        {
            get { return subscribers; }
        }

        public void Add(ISubscriber subscriber)
        {
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }

        public void Remove(ISubscriber subscriber)
        {
            if (subscribers.Contains(subscriber))
            {
                subscribers.Remove(subscriber);
            }
        }
    }
}