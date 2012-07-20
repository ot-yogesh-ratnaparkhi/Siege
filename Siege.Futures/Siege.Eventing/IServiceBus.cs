﻿using Siege.Eventing.Messages;
using Siege.Eventing.Subscribers;

namespace Siege.Eventing
{
    public interface IServiceBus
    {
        void Subscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage;
        void Unsubscribe<TMessage>(Subscriber.For<TMessage> subscriber) where TMessage : IMessage;
        void Publish<TMessage>(TMessage message) where TMessage : class, IMessage;
    }
}