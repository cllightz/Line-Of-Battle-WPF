using System;
using System.Collections.Generic;
using System.Linq;

namespace LineOfBattle.Messenger
{
    /// <summary>
    /// Publish() されたら即座に Subscriber のコールバックを呼び出すチャンネル
    /// </summary>
    /// <typeparam name="TArgs">メッセージの引数</typeparam>
    internal class ImmediateFiringChannel<TArgs> : IChannel<TArgs>
    {
        private HashSet<Type> _publishers = new HashSet<Type>();
        private List<(Type SubscriberType, Action<TArgs> Callback)> _subscribers = new List<(Type, Action<TArgs>)>();

        public void AddPublisher<TPublisher>()
            => _publishers.Add( typeof( TPublisher ) );

        public void AddSubscriber<TSubscriber>( Action<TArgs> callback )
            => _subscribers.Add( (typeof( TSubscriber ), callback) );

        public IEnumerable<Type> EnumeratePublishers()
            => _publishers;

        public IEnumerable<Type> EnumerateSubscribers()
            => _subscribers.Select( tuple => tuple.SubscriberType );

        public void MulticastToSubscribers( IMessage<TArgs> message )
        {
#if DEBUG
            if ( !_publishers.Contains( message.PublisherType ) ) {
                throw new PublisherNotRegisteredException( message.PublisherType, typeof( TArgs ) );
            }
#endif

            _subscribers.ForEach( tuple => tuple.Callback( message.Args ) );
        }
    }
}