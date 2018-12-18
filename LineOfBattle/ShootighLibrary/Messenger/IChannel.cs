using System;
using System.Collections.Generic;

namespace ShootighLibrary.Messenger
{
    interface IChannel<TArgs>
    {
        void AddPublisher( Type publisherType );
        void AddSubscriber( Type subscriberType, Action<TArgs> callback );
        IEnumerable<Type> EnumeratePublishers();
        IEnumerable<Type> EnumerateSubscribers();
        void MulticastToSubscribers( IMessage<TArgs> message );
    }
}
