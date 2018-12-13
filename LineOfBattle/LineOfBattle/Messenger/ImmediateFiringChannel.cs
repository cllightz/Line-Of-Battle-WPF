using System;
using System.Collections.Generic;

namespace LineOfBattle.Messenger
{
    internal class ImmediateFiringChannel<TArgs> : IChannel<TArgs>
    {
        // 追記する

        public void AddPublisher<TPublisher>() => throw new NotImplementedException();
        public void AddSubscriber<TSubscriber>( Action<TArgs> callback ) => throw new NotImplementedException();
        public IEnumerable<Type> EnumeratePublishers() => throw new NotImplementedException();
        public IEnumerable<Type> EnumerateSubscribers() => throw new NotImplementedException();
        public void MulticastToSubscribers( IMessage<TArgs> message ) => throw new NotImplementedException();
    }
}