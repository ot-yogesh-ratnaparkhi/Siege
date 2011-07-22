using System;
using System.Collections.Generic;

namespace Siege.Eventing.Subscribers
{
    public class SubscriberCollectionItem : ISubscriberCollectionItem
    {
        private Dictionary<Type, ISubscriber> subscribers;

        public SubscriberCollectionItem()
        {
            subscribers = new Dictionary<Type, ISubscriber>();
        }

        public IEnumerable<ISubscriber> Subscribers
        {
            get { return subscribers.Values; }
        }

        public void Add(ISubscriber subscriber)
        {
            if (!subscribers.ContainsKey(subscriber.GetType()))
            {
                subscribers.Add(subscriber.GetType(), subscriber);
            }
        }

        public void Remove(ISubscriber subscriber)
        {
            if (subscribers.ContainsKey(subscriber.GetType()))
            {
                subscribers.Remove(subscriber.GetType());
            }
        }
    }
}