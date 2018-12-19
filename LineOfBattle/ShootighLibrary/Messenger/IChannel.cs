using System;
using System.Collections.Generic;

namespace ShootighLibrary.Messenger
{
    internal interface IChannel
    {
        IEnumerable<Type> EnumeratePublishers();
        IEnumerable<Type> EnumerateSubscribers();
    }

    interface IChannel<TArgs> : IChannel
    {
        void AddPublisher( Type publisherType );
        void AddSubscriber( Type subscriberType, Action<TArgs> callback );
        void MulticastToSubscribers( IMessage<TArgs> message );
    }
}
