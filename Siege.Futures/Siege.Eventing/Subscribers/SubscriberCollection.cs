using System;
using System.Collections.Generic;
using System.Linq;
using Siege.Eventing.Messages;

namespace Siege.Eventing.Subscribers
{
    public class SubscriberCollection
    {
        private readonly Dictionary<Type, ISubscriberCollectionItem> subscribersByMessageType;
        private static readonly object subscribersLock = new object();

        public SubscriberCollection()
        {
            subscribersByMessageType = new Dictionary<Type, ISubscriberCollectionItem>();
        }

        public SubscriberCollection Add<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage
        {
            var subscribers = SubscribersFor<TMessage>();
            lock (subscribersLock)
            {
                subscribers.Add(subscriber);
            }
            return this;
        }

        public SubscriberCollection Remove<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage
        {
            var subscribers = SubscribersFor<TMessage>();
            lock (subscribersLock)
            {
                subscribers.Remove(subscriber);
            }
            return this;
        }

        private ISubscriberCollectionItem SubscribersFor<TMessage>() where TMessage : IMessage
        {
            return SubscribersFor(typeof (TMessage));
        }

        private ISubscriberCollectionItem SubscribersFor(Type type)
        {
            if (!subscribersByMessageType.ContainsKey(type))
            {
                lock (subscribersLock)
                {
                    if (!subscribersByMessageType.ContainsKey(type))
                    {
                        subscribersByMessageType[type] = new SubscriberCollectionItem();
                    }
                }
            }
            return subscribersByMessageType[type];
        }

        public IEnumerable<ISubscriber> For<TMessage>() where TMessage : IMessage
        {
            return For(typeof(TMessage));
        }

        public IEnumerable<ISubscriber> For(Type type)
        {
            return SubscribersFor(type).Subscribers;
        }

        public IEnumerable<ISubscriber> All(Type type)
        {
            var subscribers = new List<ISubscriber>();
            var list = subscribersByMessageType.Where(item => item.Key.IsAssignableFrom(type)).Select(key => subscribersByMessageType[key.Key]);

            foreach(var item in list)
            {
                subscribers.AddRange(item.Subscribers);
            }

            return subscribers;
        }
    }
}