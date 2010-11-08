using System;
using System.Collections.Generic;
using System.Linq;
using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier
{
    public class ServiceBus : IServiceBus
    {
        private readonly MessageMap messageMap;
        private readonly IMessageBucket bucket;
        private readonly SubscriberCollection subscribers;

        public ServiceBus(MessageMap messageMap, IMessageBucket bucket)
        {
            this.messageMap = messageMap;
            this.bucket = bucket;
            subscribers = new SubscriberCollection();
        }

        public void Subscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage
        {
            subscribers.Add(subscriber);
        }

        public void Unsubscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage
        {
            subscribers.Remove(subscriber);
        }

        public void Publish<TMessage>(TMessage message) where TMessage : class, IMessage
        {
            var errors = new List<ExceptionMessage>();
            IEnumerable<ISubscriber> messageSubscribers = subscribers.For<TMessage>();
        
            if(messageSubscribers.Count() == 0)
            {
                if (messageMap.CanHandle(message.GetType()))
                {
                    var adapter = messageMap.GetAdapterFor(message.GetType());
                    messageSubscribers = new List<ISubscriber> {adapter};
                }
                else
                {
                    bucket.Add(message);
                }
            }

            foreach (var subscriber in messageSubscribers)
            {
                try
                {
                    if(subscriber is Subscriber.For<TMessage>)
                    {
                        ((Subscriber.For<TMessage>) subscriber).Receive(message);
                    }
                    else
                    {
                        ((IProtocolAdapter)subscriber).Receive(message);
                    }
                }
                catch (Exception ex)
                {
                    errors.Add(new ExceptionMessage(message, subscriber, ex));
                }
            }
            
            if (errors.Count > 0)
            {
                foreach (var error in errors)
                {
                    Publish(error);
                }
            }

            if (errors.Count > 0)
            {
                throw new MessageProcessingException(
                    "One or more errors occured during message processing.",
                    message != null ? message.GetType() : null);
            }
        }
    }
}