using System;
using System.Collections.Generic;

namespace LineOfBattle.Messenger
{
    interface IChannel<TArgs>
    {
        void AddSubscriber<TSubscriber>( Action<TArgs> callback );

        IEnumerable<Type> EnumerateSubscribers();

        void AddPublisher<TPublisher>();

        IEnumerable<Type> EnumeratePublishers();

        void MulticastToSubscribers( IMessage<TArgs> message );
    }
}
