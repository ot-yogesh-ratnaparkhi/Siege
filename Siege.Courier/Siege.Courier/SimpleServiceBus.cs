using System;
using System.Collections.Generic;
using Siege.Courier.Messages;
using Siege.Courier.Subscribers;

namespace Siege.Courier
{
    public class SimpleServiceBus : IServiceBus
    {
        private readonly SubscriberCollection subscribers;

        public SimpleServiceBus()
        {
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

        public void Publish<TMessage>(TMessage message) where TMessage : IMessage
        {
            var errors = new List<ExceptionMessage>();
            IEnumerable<ISubscriber> messageSubscribers;
            
            if(message is ExceptionMessage)
                messageSubscribers = subscribers.For<TMessage>();
            else
                messageSubscribers = subscribers.All(message.GetType());

            foreach (var subscriber in messageSubscribers)
            {
                try
                {
                    ((Subscriber.For<TMessage>)subscriber).Receive(message);
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
                throw errors[0].Exception;
                //throw new MessageProcessingException(
                //    "One or more errors occured during message processing.",
                //    message != null ? message.GetType() : null);
            }
        }
    }
}